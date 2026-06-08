// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门应用服务实现
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
/// 设变部门应用服务
/// </summary>
public class TaktEcDeptService : TaktServiceBase, ITaktEcDeptService
{
    private readonly ITaktCompanyRepository<TaktEcDept> _ecDeptRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDeptRepository">设变部门仓储</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDeptService(
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDeptRepository = ecDeptRepository;
        _ecDetailRepository = ecDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDeptDto>> GetEcDeptListAsync(TaktEcDeptQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecDeptRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcDeptDto>.Create(
            data.Adapt<List<TaktEcDeptDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDeptDto?> GetEcDeptByIdAsync(long id)
    {
        var entity = await _ecDeptRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcDeptDto>();
    }

    /// <summary>
    /// 获取设变部门选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcDeptOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecDeptRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDeptDto> CreateEcDeptAsync(TaktEcDeptCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcDept>();
        await StampEcDeptEcDetailAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_dept_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDeptRepository,
            x => x.EcnDetailId == entity.EcnDetailId
                && x.DeptCode == entity.DeptCode);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_dept_unique)
        {
            throw new TaktBusinessException("设变部门的EcnDetailId、DeptCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecDeptRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == entity.EcnDetailId,
                x => x.LineNumber);
            var businessCode = entity.EcnDetailId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecDeptRepository.CreateAsync(entity);
        return await GetEcDeptByIdAsync(entity.Id) ?? entity.Adapt<TaktEcDeptDto>();
    }

    /// <summary>
    /// 更新设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDeptDto> UpdateEcDeptAsync(long id, TaktEcDeptUpdateDto dto)
    {
        var entity = await _ecDeptRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变部门不存在");
        }
        dto.Adapt(entity);
        await StampEcDeptEcDetailAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_dept_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDeptRepository,
            x => x.EcnDetailId == entity.EcnDetailId
                && x.DeptCode == entity.DeptCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_dept_unique)
        {
            throw new TaktBusinessException("设变部门的EcnDetailId、DeptCode已存在");
        }
        await _ecDeptRepository.UpdateAsync(entity);
        return await GetEcDeptByIdAsync(id) ?? throw new TaktBusinessException("设变部门不存在");
    }

    /// <summary>
    /// 删除设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDeptByIdAsync(long id)
    {
        var deleted = await _ecDeptRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变部门不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDeptBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcDeptByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcDeptTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcDeptTemplateDto>(
            sheetName ?? "设变部门导入模板",
            fileName ?? "设变部门导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变部门
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcDeptAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcDeptImportDto>(fileStream, sheetName ?? "设变部门导入模板");
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
                var entity = rows[i].Adapt<TaktEcDept>();
                var importDto = rows[i].Adapt<TaktEcDeptCreateDto>();
                await StampEcDeptEcDetailAsync(entity, importDto);
                var importKey = $"{entity.EcnDetailId}|{entity.DeptCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcnDetailId、DeptCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_dept_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecDeptRepository,
                    x => x.EcnDetailId == entity.EcnDetailId
                        && x.DeptCode == entity.DeptCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_dept_unique)
                {
                    throw new TaktBusinessException("设变部门的EcnDetailId、DeptCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecDeptRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == entity.EcnDetailId,
                        x => x.LineNumber);
                    var businessCode = entity.EcnDetailId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecDeptRepository.CreateAsync(entity);
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
    /// 导出设变部门
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcDeptAsync(TaktEcDeptQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcDeptQueryDto());
        var list = await _ecDeptRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcDeptExportDto>(),
                sheetName ?? "设变部门数据",
                fileName ?? "设变部门导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcDeptExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变部门数据",
            fileName ?? "设变部门导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设变部门主表外键（ManyToOne → 设变明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcDeptEcDetailAsync(TaktEcDept entity, TaktEcDeptCreateDto dto)
    {
        if (dto.EcnDetailId <= 0)
        {
            return;
        }
        var master = await _ecDetailRepository.GetByIdAsync(dto.EcnDetailId);
        if (master == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        entity.EcnDetailId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变部门查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcDept, bool>> QueryExpression(TaktEcDeptQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDept>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcnDetailId).Contains(keywords)
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || SqlFunc.ToString(x.IsImplemented).Contains(keywords)
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.ScheduledBatch != null && x.ScheduledBatch.Contains(keywords))
                || (x.PoRemainder != null && x.PoRemainder.Contains(keywords))
                || (x.Balance != null && x.Balance.Contains(keywords))
                || (x.OldProductHandling != null && x.OldProductHandling.Contains(keywords))
                || (x.Supplier != null && x.Supplier.Contains(keywords))
                || (x.PurchaseOrderNo != null && x.PurchaseOrderNo.Contains(keywords))
                || (x.IqcOrderNo != null && x.IqcOrderNo.Contains(keywords))
                || (x.OutboundBatch != null && x.OutboundBatch.Contains(keywords))
                || (x.ProductionBatch != null && x.ProductionBatch.Contains(keywords))
                || (x.OutboundOrderNo != null && x.OutboundOrderNo.Contains(keywords))
                || (x.ProductionTeam != null && x.ProductionTeam.Contains(keywords))
                || (x.InspectionBatch != null && x.InspectionBatch.Contains(keywords))
                || (x.SamplingNo != null && x.SamplingNo.Contains(keywords))
                || SqlFunc.ToString(x.IsSopUpdated).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScheduledProductionDate).Contains(keywords)
                || SqlFunc.ToString(x.PurchaseOrderIssueDate).Contains(keywords)
                || SqlFunc.ToString(x.InspectionDate).Contains(keywords)
                || SqlFunc.ToString(x.OutboundDate).Contains(keywords)
                || SqlFunc.ToString(x.ProductionDate).Contains(keywords)
                || SqlFunc.ToString(x.ImplementationDate).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.Content))
        {
            exp = exp.And(x => x.Content != null && x.Content.Contains(queryDto.Content));
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

        if (!string.IsNullOrEmpty(queryDto?.Supplier))
        {
            exp = exp.And(x => x.Supplier != null && x.Supplier.Contains(queryDto.Supplier));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrderNo))
        {
            exp = exp.And(x => x.PurchaseOrderNo != null && x.PurchaseOrderNo.Contains(queryDto.PurchaseOrderNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.IqcOrderNo))
        {
            exp = exp.And(x => x.IqcOrderNo != null && x.IqcOrderNo.Contains(queryDto.IqcOrderNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundBatch))
        {
            exp = exp.And(x => x.OutboundBatch != null && x.OutboundBatch.Contains(queryDto.OutboundBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionBatch))
        {
            exp = exp.And(x => x.ProductionBatch != null && x.ProductionBatch.Contains(queryDto.ProductionBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundOrderNo))
        {
            exp = exp.And(x => x.OutboundOrderNo != null && x.OutboundOrderNo.Contains(queryDto.OutboundOrderNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionTeam))
        {
            exp = exp.And(x => x.ProductionTeam != null && x.ProductionTeam.Contains(queryDto.ProductionTeam));
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectionBatch))
        {
            exp = exp.And(x => x.InspectionBatch != null && x.InspectionBatch.Contains(queryDto.InspectionBatch));
        }

        if (!string.IsNullOrEmpty(queryDto?.SamplingNo))
        {
            exp = exp.And(x => x.SamplingNo != null && x.SamplingNo.Contains(queryDto.SamplingNo));
        }

        if (queryDto?.IsSopUpdated.HasValue == true)
        {
            exp = exp.And(x => x.IsSopUpdated == queryDto.IsSopUpdated);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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

        if (queryDto?.PurchaseOrderIssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseOrderIssueDate >= queryDto.PurchaseOrderIssueDateStart);
        }

        if (queryDto?.PurchaseOrderIssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseOrderIssueDate <= queryDto.PurchaseOrderIssueDateEnd);
        }

        if (queryDto?.InspectionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate >= queryDto.InspectionDateStart);
        }

        if (queryDto?.InspectionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate <= queryDto.InspectionDateEnd);
        }

        if (queryDto?.OutboundDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate >= queryDto.OutboundDateStart);
        }

        if (queryDto?.OutboundDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate <= queryDto.OutboundDateEnd);
        }

        if (queryDto?.ProductionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProductionDate >= queryDto.ProductionDateStart);
        }

        if (queryDto?.ProductionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProductionDate <= queryDto.ProductionDateEnd);
        }

        if (queryDto?.ImplementationDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ImplementationDate >= queryDto.ImplementationDateStart);
        }

        if (queryDto?.ImplementationDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ImplementationDate <= queryDto.ImplementationDateEnd);
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
