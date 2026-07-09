// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeikanService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变生管执行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变生管执行应用服务
/// </summary>
public class TaktEcSeikanService : TaktServiceBase, ITaktEcSeikanService
{
    private readonly ITaktCompanyRepository<TaktEcSeikan> _ecSeikanRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecSeikanRepository">设变生管执行仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcSeikanService(
        ITaktCompanyRepository<TaktEcSeikan> ecSeikanRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecSeikanRepository = ecSeikanRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变生管执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcSeikanDto>> GetEcSeikanListAsync(TaktEcSeikanQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecSeikanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcSeikanDto>.Create(
            data.Adapt<List<TaktEcSeikanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变生管执行
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcSeikanDto?> GetEcSeikanByIdAsync(long id)
    {
        var entity = await _ecSeikanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcSeikanDto>();
    }

    /// <summary>
    /// 获取设变生管执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcSeikanOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecSeikanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.DeptCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变生管执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcSeikanDto> CreateEcSeikanAsync(TaktEcSeikanCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcSeikan>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique = await _uniqueValidator.IsUniqueAsync(
            _ecSeikanRepository,
            x => x.EcnDetailId == entity.EcnDetailId);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique)
        {
            throw new TaktBusinessException("设变生管执行的EcnDetailId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecSeikanRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == entity.EcnDetailId,
                x => x.LineNumber);
            var businessCode = entity.EcnDetailId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecSeikanRepository.CreateAsync(entity);
        return await GetEcSeikanByIdAsync(entity.Id) ?? entity.Adapt<TaktEcSeikanDto>();
    }

    /// <summary>
    /// 更新设变生管执行
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcSeikanDto> UpdateEcSeikanAsync(long id, TaktEcSeikanUpdateDto dto)
    {
        var entity = await _ecSeikanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique = await _uniqueValidator.IsUniqueAsync(
            _ecSeikanRepository,
            x => x.EcnDetailId == entity.EcnDetailId,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique)
        {
            throw new TaktBusinessException("设变生管执行的EcnDetailId已存在");
        }
        await _ecSeikanRepository.UpdateAsync(entity);
        return await GetEcSeikanByIdAsync(id) ?? throw new TaktBusinessException("设变生管执行不存在");
    }

    /// <summary>
    /// 删除设变生管执行
    /// </summary>
    /// <param name="id">设变生管执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcSeikanByIdAsync(long id)
    {
        var entity = await _ecSeikanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变生管执行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变生管执行已作废");
        }
        entity.IsObsolete = 1;
        await _ecSeikanRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除设变生管执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcSeikanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcSeikanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变生管执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcSeikanDto> UpdateEcSeikanObsoleteAsync(TaktEcSeikanObsoleteDto dto)
    {
        var entity = await _ecSeikanRepository.GetByIdAsync(dto.EcSeikanId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变生管执行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecSeikanRepository.UpdateAsync(entity);
        return await GetEcSeikanByIdAsync(dto.EcSeikanId) ?? throw new TaktBusinessException("设变生管执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcSeikanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcSeikanTemplateDto>(
            sheetName ?? "设变生管执行导入模板",
            fileName ?? "设变生管执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变生管执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcSeikanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcSeikanImportDto>(fileStream, sheetName ?? "设变生管执行导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEcSeikan>();
                var importKey = $"{entity.EcnDetailId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcnDetailId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecSeikanRepository,
                    x => x.EcnDetailId == entity.EcnDetailId);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_seikan_unique)
                {
                    throw new TaktBusinessException("设变生管执行的EcnDetailId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecSeikanRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == entity.EcnDetailId,
                        x => x.LineNumber);
                    var businessCode = entity.EcnDetailId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecSeikanRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出设变生管执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcSeikanAsync(TaktEcSeikanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcSeikanQueryDto());
        var list = await _ecSeikanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcSeikanExportDto>(),
                sheetName ?? "设变生管执行数据",
                fileName ?? "设变生管执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcSeikanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变生管执行数据",
            fileName ?? "设变生管执行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变生管执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcSeikan, bool>> QueryExpression(TaktEcSeikanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcSeikan>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcnDetailId).Contains(keywords)
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || SqlFunc.ToString(x.IsImplemented).Contains(keywords)
                || (x.ExecContent != null && x.ExecContent.Contains(keywords))
                || (x.ScheduledBatch != null && x.ScheduledBatch.Contains(keywords))
                || (x.PoRemainder != null && x.PoRemainder.Contains(keywords))
                || (x.Balance != null && x.Balance.Contains(keywords))
                || (x.OldProductHandling != null && x.OldProductHandling.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScheduledProductionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EcnDetailId.HasValue == true)
        {
            exp = exp.And(x => x.EcnDetailId == queryDto.EcnDetailId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptCode))
        {
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(queryDto.DeptCode));
        }

        if (queryDto?.IsImplemented.HasValue == true)
        {
            exp = exp.And(x => x.IsImplemented == queryDto.IsImplemented);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecContent))
        {
            exp = exp.And(x => x.ExecContent != null && x.ExecContent.Contains(queryDto.ExecContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScheduledBatch))
        {
            exp = exp.And(x => x.ScheduledBatch != null && x.ScheduledBatch.Contains(queryDto.ScheduledBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.PoRemainder))
        {
            exp = exp.And(x => x.PoRemainder != null && x.PoRemainder.Contains(queryDto.PoRemainder));
        }

        if (!string.IsNullOrEmpty(queryDto?.Balance))
        {
            exp = exp.And(x => x.Balance != null && x.Balance.Contains(queryDto.Balance));
        }

        if (!string.IsNullOrEmpty(queryDto?.OldProductHandling))
        {
            exp = exp.And(x => x.OldProductHandling != null && x.OldProductHandling.Contains(queryDto.OldProductHandling));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ScheduledProductionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledProductionDate >= queryDto.ScheduledProductionDateStart);
        }

        if (queryDto?.ScheduledProductionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledProductionDate <= queryDto.ScheduledProductionDateEnd);
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
