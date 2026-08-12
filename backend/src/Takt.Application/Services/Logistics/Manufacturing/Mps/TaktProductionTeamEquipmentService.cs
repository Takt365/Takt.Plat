// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipmentService.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组设备组应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组设备组应用服务
/// </summary>
public class TaktProductionTeamEquipmentService : TaktServiceBase, ITaktProductionTeamEquipmentService
{
    private readonly ITaktCompanyRepository<TaktProductionTeamEquipment> _productionTeamEquipmentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionTeamEquipmentRepository">生产班组设备组仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionTeamEquipmentService(
        ITaktCompanyRepository<TaktProductionTeamEquipment> productionTeamEquipmentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionTeamEquipmentRepository = productionTeamEquipmentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产班组设备组列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionTeamEquipmentDto>> GetProductionTeamEquipmentListAsync(TaktProductionTeamEquipmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionTeamEquipmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionTeamEquipmentDto>.Create(
            data.Adapt<List<TaktProductionTeamEquipmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamEquipmentDto?> GetProductionTeamEquipmentByIdAsync(long id)
    {
        var entity = await _productionTeamEquipmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionTeamEquipmentDto>();
    }

    /// <summary>
    /// 获取生产班组设备组选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionTeamEquipmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _productionTeamEquipmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TeamEquipStatus == 1 && x.IsObsolete == 0,
            x => x.TeamCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TeamCode,
            DictLabel = e.TeamCode,
        }).ToList();
    }

    /// <summary>
    /// 创建生产班组设备组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamEquipmentDto> CreateProductionTeamEquipmentAsync(TaktProductionTeamEquipmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionTeamEquipment>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamEquipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdTeamId == entity.ProdTeamId
                && x.ProdEquipId == entity.ProdEquipId);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique)
        {
            throw new TaktBusinessException("生产班组设备组的PlantCode、ProdTeamId、ProdEquipId已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamEquipmentRepository,
            x => x.ProdTeamId == entity.ProdTeamId
                && x.LineNumber == entity.LineNumber
                && x.ProdEquipCode == entity.ProdEquipCode);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique)
        {
            throw new TaktBusinessException("生产班组设备组的ProdTeamId、LineNumber、ProdEquipCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _productionTeamEquipmentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProdTeamId == entity.ProdTeamId,
                x => x.LineNumber);
            var businessCode = entity.ProdTeamId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _productionTeamEquipmentRepository.CreateAsync(entity);
        return await GetProductionTeamEquipmentByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionTeamEquipmentDto>();
    }

    /// <summary>
    /// 更新生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamEquipmentDto> UpdateProductionTeamEquipmentAsync(long id, TaktProductionTeamEquipmentUpdateDto dto)
    {
        var entity = await _productionTeamEquipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组设备组不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamEquipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdTeamId == entity.ProdTeamId
                && x.ProdEquipId == entity.ProdEquipId,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique)
        {
            throw new TaktBusinessException("生产班组设备组的PlantCode、ProdTeamId、ProdEquipId已存在");
        }
        var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique = await _uniqueValidator.IsUniqueAsync(
            _productionTeamEquipmentRepository,
            x => x.ProdTeamId == entity.ProdTeamId
                && x.LineNumber == entity.LineNumber
                && x.ProdEquipCode == entity.ProdEquipCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique)
        {
            throw new TaktBusinessException("生产班组设备组的ProdTeamId、LineNumber、ProdEquipCode已存在");
        }
        await _productionTeamEquipmentRepository.UpdateAsync(entity);
        return await GetProductionTeamEquipmentByIdAsync(id) ?? throw new TaktBusinessException("生产班组设备组不存在");
    }

    /// <summary>
    /// 删除生产班组设备组
    /// </summary>
    /// <param name="id">生产班组设备组ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamEquipmentByIdAsync(long id)
    {
        var entity = await _productionTeamEquipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组设备组不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("生产班组设备组不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("生产班组设备组已作废");
        }
        entity.IsObsolete = 1;
        await _productionTeamEquipmentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除生产班组设备组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionTeamEquipmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionTeamEquipmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产班组设备组状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamEquipmentDto> UpdateProductionTeamEquipmentStatusAsync(TaktProductionTeamEquipmentStatusDto dto)
    {
        var entity = await _productionTeamEquipmentRepository.GetByIdAsync(dto.ProductionTeamEquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组设备组不存在");
        }
        entity.TeamEquipStatus = dto.TeamEquipStatus;
        await _productionTeamEquipmentRepository.UpdateAsync(entity);
        return await GetProductionTeamEquipmentByIdAsync(dto.ProductionTeamEquipmentId) ?? throw new TaktBusinessException("生产班组设备组不存在");
    }

    /// <summary>
    /// 更新生产班组设备组作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionTeamEquipmentDto> UpdateProductionTeamEquipmentObsoleteAsync(TaktProductionTeamEquipmentObsoleteDto dto)
    {
        var entity = await _productionTeamEquipmentRepository.GetByIdAsync(dto.ProductionTeamEquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产班组设备组不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("生产班组设备组不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _productionTeamEquipmentRepository.UpdateAsync(entity);
        return await GetProductionTeamEquipmentByIdAsync(dto.ProductionTeamEquipmentId) ?? throw new TaktBusinessException("生产班组设备组不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionTeamEquipmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionTeamEquipmentTemplateDto>(
            sheetName ?? "生产班组设备组导入模板",
            fileName ?? "生产班组设备组导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产班组设备组
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionTeamEquipmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionTeamEquipmentImportDto>(fileStream, sheetName ?? "生产班组设备组导入模板");
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
                var entity = rows[i].Adapt<TaktProductionTeamEquipment>();
                var importKey = $"{entity.PlantCode}|{entity.ProdTeamId}|{entity.ProdEquipId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProdTeamId、ProdEquipId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionTeamEquipmentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProdTeamId == entity.ProdTeamId
                        && x.ProdEquipId == entity.ProdEquipId);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_unique)
                {
                    throw new TaktBusinessException("生产班组设备组的PlantCode、ProdTeamId、ProdEquipId已存在");
                }
                var isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionTeamEquipmentRepository,
                    x => x.ProdTeamId == entity.ProdTeamId
                        && x.LineNumber == entity.LineNumber
                        && x.ProdEquipCode == entity.ProdEquipCode);
                if (!isUnique_ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique)
                {
                    throw new TaktBusinessException("生产班组设备组的ProdTeamId、LineNumber、ProdEquipCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _productionTeamEquipmentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProdTeamId == entity.ProdTeamId,
                        x => x.LineNumber);
                    var businessCode = entity.ProdTeamId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _productionTeamEquipmentRepository.CreateAsync(entity);
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
    /// 导出生产班组设备组
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionTeamEquipmentAsync(TaktProductionTeamEquipmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionTeamEquipmentQueryDto());
        var list = await _productionTeamEquipmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionTeamEquipmentExportDto>(),
                sheetName ?? "生产班组设备组数据",
                fileName ?? "生产班组设备组导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionTeamEquipmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产班组设备组数据",
            fileName ?? "生产班组设备组导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产班组设备组查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionTeamEquipment, bool>> QueryExpression(TaktProductionTeamEquipmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionTeamEquipment>();

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
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdTeamId).Contains(keywords)
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.ProdEquipId).Contains(keywords)
                || (x.ProdEquipCode != null && x.ProdEquipCode.Contains(keywords))
                || SqlFunc.ToString(x.EquipQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TeamEquipStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.ProdTeamId.HasValue == true)
        {
            exp = exp.And(x => x.ProdTeamId == queryDto.ProdTeamId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.ProdEquipId.HasValue == true)
        {
            exp = exp.And(x => x.ProdEquipId == queryDto.ProdEquipId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdEquipCode))
        {
            exp = exp.And(x => x.ProdEquipCode != null && x.ProdEquipCode.Contains(queryDto.ProdEquipCode));
        }

        if (queryDto?.EquipQuantity.HasValue == true)
        {
            exp = exp.And(x => x.EquipQuantity == queryDto.EquipQuantity);
        }

        if (queryDto?.TeamEquipStatus.HasValue == true)
        {
            exp = exp.And(x => x.TeamEquipStatus == queryDto.TeamEquipStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
