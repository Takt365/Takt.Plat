// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktAccountTitleChangeLogService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目变更记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会计科目变更记录应用服务
/// </summary>
public class TaktAccountTitleChangeLogService : TaktServiceBase, ITaktAccountTitleChangeLogService
{
    private readonly ITaktCompanyRepository<TaktAccountTitleChangeLog> _accountTitleChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktAccountTitle> _accountTitleRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="accountTitleChangeLogRepository">会计科目变更记录仓储</param>
    /// <param name="accountTitleRepository">会计科目仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAccountTitleChangeLogService(
        ITaktCompanyRepository<TaktAccountTitleChangeLog> accountTitleChangeLogRepository,
        ITaktCompanyRepository<TaktAccountTitle> accountTitleRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _accountTitleChangeLogRepository = accountTitleChangeLogRepository;
        _accountTitleRepository = accountTitleRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会计科目变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAccountTitleChangeLogDto>> GetAccountTitleChangeLogListAsync(TaktAccountTitleChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _accountTitleChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAccountTitleChangeLogDto>.Create(
            data.Adapt<List<TaktAccountTitleChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleChangeLogDto?> GetAccountTitleChangeLogByIdAsync(long id)
    {
        var entity = await _accountTitleChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAccountTitleChangeLogDto>();
    }

    /// <summary>
    /// 获取会计科目变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAccountTitleChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _accountTitleChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TitleCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TitleCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会计科目变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleChangeLogDto> CreateAccountTitleChangeLogAsync(TaktAccountTitleChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktAccountTitleChangeLog>();
        await StampAccountTitleChangeLogAccountTitleAsync(entity, dto);
        entity = await _accountTitleChangeLogRepository.CreateAsync(entity);
        return await GetAccountTitleChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktAccountTitleChangeLogDto>();
    }

    /// <summary>
    /// 更新会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleChangeLogDto> UpdateAccountTitleChangeLogAsync(long id, TaktAccountTitleChangeLogUpdateDto dto)
    {
        var entity = await _accountTitleChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目变更记录不存在");
        }
        dto.Adapt(entity);
        await StampAccountTitleChangeLogAccountTitleAsync(entity, dto);
        await _accountTitleChangeLogRepository.UpdateAsync(entity);
        return await GetAccountTitleChangeLogByIdAsync(id) ?? throw new TaktBusinessException("会计科目变更记录不存在");
    }

    /// <summary>
    /// 删除会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleChangeLogByIdAsync(long id)
    {
        var deleted = await _accountTitleChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会计科目变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会计科目变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAccountTitleChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出会计科目变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAccountTitleChangeLogAsync(TaktAccountTitleChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAccountTitleChangeLogQueryDto());
        var list = await _accountTitleChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAccountTitleChangeLogExportDto>(),
                sheetName ?? "会计科目变更记录数据",
                fileName ?? "会计科目变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAccountTitleChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会计科目变更记录数据",
            fileName ?? "会计科目变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步会计科目变更记录主表外键（ManyToOne → 会计科目）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampAccountTitleChangeLogAccountTitleAsync(TaktAccountTitleChangeLog entity, TaktAccountTitleChangeLogCreateDto dto)
    {
        if (dto.AccountTitleId <= 0)
        {
            return;
        }
        var master = await _accountTitleRepository.GetByIdAsync(dto.AccountTitleId);
        if (master == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        entity.AccountTitleId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会计科目变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAccountTitleChangeLog, bool>> QueryExpression(TaktAccountTitleChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAccountTitleChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.AccountTitleId).Contains(keywords)
                || (x.TitleCode != null && x.TitleCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.AccountTitleId.HasValue == true)
        {
            exp = exp.And(x => x.AccountTitleId == queryDto.AccountTitleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TitleCode))
        {
            exp = exp.And(x => x.TitleCode != null && x.TitleCode.Contains(queryDto.TitleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeBy))
        {
            exp = exp.And(x => x.ChangeBy != null && x.ChangeBy.Contains(queryDto.ChangeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ChangeTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime >= queryDto.ChangeTimeStart);
        }

        if (queryDto?.ChangeTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime <= queryDto.ChangeTimeEnd);
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
