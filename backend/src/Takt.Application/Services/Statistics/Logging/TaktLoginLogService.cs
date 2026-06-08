// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktLoginLogService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 登录日志应用服务
/// </summary>
public class TaktLoginLogService : TaktServiceBase, ITaktLoginLogService
{
    private readonly ITaktCompanyRepository<TaktLoginLog> _loginLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="loginLogRepository">登录日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktLoginLogService(
        ITaktCompanyRepository<TaktLoginLog> loginLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _loginLogRepository = loginLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktLoginLogDto>> GetLoginLogListAsync(TaktLoginLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _loginLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktLoginLogDto>.Create(
            data.Adapt<List<TaktLoginLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto?> GetLoginLogByIdAsync(long id)
    {
        var entity = await _loginLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktLoginLogDto>();
    }

    /// <summary>
    /// 获取登录日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetLoginLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _loginLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.Username,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Username ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建登录日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto> CreateLoginLogAsync(TaktLoginLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktLoginLog>();
        entity = await _loginLogRepository.CreateAsync(entity);
        return await GetLoginLogByIdAsync(entity.Id) ?? entity.Adapt<TaktLoginLogDto>();
    }

    /// <summary>
    /// 更新登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto> UpdateLoginLogAsync(long id, TaktLoginLogUpdateDto dto)
    {
        var entity = await _loginLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("登录日志不存在");
        }
        dto.Adapt(entity);
        await _loginLogRepository.UpdateAsync(entity);
        return await GetLoginLogByIdAsync(id) ?? throw new TaktBusinessException("登录日志不存在");
    }

    /// <summary>
    /// 删除登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteLoginLogByIdAsync(long id)
    {
        var deleted = await _loginLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("登录日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除登录日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteLoginLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteLoginLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportLoginLogAsync(TaktLoginLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktLoginLogQueryDto());
        var list = await _loginLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktLoginLogExportDto>(),
                sheetName ?? "登录日志数据",
                fileName ?? "登录日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktLoginLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "登录日志数据",
            fileName ?? "登录日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建登录日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktLoginLog, bool>> QueryExpression(TaktLoginLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktLoginLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.Username != null && x.Username.Contains(keywords))
                || (x.LoginType != null && x.LoginType.Contains(keywords))
                || (x.Browser != null && x.Browser.Contains(keywords))
                || (x.Os != null && x.Os.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || SqlFunc.ToString(x.LoginResult).Contains(keywords)
                || (x.LoginMessage != null && x.LoginMessage.Contains(keywords))
                || (x.LoginIp != null && x.LoginIp.Contains(keywords))
                || (x.LoginLocation != null && x.LoginLocation.Contains(keywords))
                || SqlFunc.ToString(x.SessionDuration).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LogoutAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.Username))
        {
            exp = exp.And(x => x.Username != null && x.Username.Contains(queryDto.Username));
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginType))
        {
            exp = exp.And(x => x.LoginType != null && x.LoginType.Contains(queryDto.LoginType));
        }

        if (!string.IsNullOrEmpty(queryDto?.Browser))
        {
            exp = exp.And(x => x.Browser != null && x.Browser.Contains(queryDto.Browser));
        }

        if (!string.IsNullOrEmpty(queryDto?.Os))
        {
            exp = exp.And(x => x.Os != null && x.Os.Contains(queryDto.Os));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (queryDto?.LoginResult.HasValue == true)
        {
            exp = exp.And(x => x.LoginResult == queryDto.LoginResult);
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginMessage))
        {
            exp = exp.And(x => x.LoginMessage != null && x.LoginMessage.Contains(queryDto.LoginMessage));
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginIp))
        {
            exp = exp.And(x => x.LoginIp != null && x.LoginIp.Contains(queryDto.LoginIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginLocation))
        {
            exp = exp.And(x => x.LoginLocation != null && x.LoginLocation.Contains(queryDto.LoginLocation));
        }

        if (queryDto?.SessionDuration.HasValue == true)
        {
            exp = exp.And(x => x.SessionDuration == queryDto.SessionDuration);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.LogoutAtStart.HasValue == true)
        {
            exp = exp.And(x => x.LogoutAt >= queryDto.LogoutAtStart);
        }

        if (queryDto?.LogoutAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.LogoutAt <= queryDto.LogoutAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
