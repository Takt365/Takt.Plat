// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查明细应用服务
/// </summary>
public class TaktPcbaInspectionDetailService : TaktServiceBase, ITaktPcbaInspectionDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaInspection> _pcbaInspectionRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionDetailRepository">PCBA检查明细仓储</param>
    /// <param name="pcbaInspectionRepository">PCBA检查日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaInspectionDetailService(
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
        _pcbaInspectionRepository = pcbaInspectionRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA检查明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaInspectionDetailDto>> GetPcbaInspectionDetailListAsync(TaktPcbaInspectionDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaInspectionDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaInspectionDetailDto>.Create(
            data.Adapt<List<TaktPcbaInspectionDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto?> GetPcbaInspectionDetailByIdAsync(long id)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaInspectionDetailDto>();
    }

    /// <summary>
    /// 获取PCBA检查明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaInspectionDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _pcbaInspectionDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.InspectorName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.InspectorName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA检查明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> CreatePcbaInspectionDetailAsync(TaktPcbaInspectionDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaInspectionDetail>();
                await StampPcbaInspectionDetailPcbaInspectionAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionDetailRepository,
            x => x.PcbaInspectionId == entity.PcbaInspectionId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaInspectionDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaInspectionId == entity.PcbaInspectionId,
                x => x.LineNumber);
            var businessCode = entity.PcbaInspectionId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _pcbaInspectionDetailRepository.CreateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaInspectionDetailDto>();
    }

    /// <summary>
    /// 更新PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> UpdatePcbaInspectionDetailAsync(long id, TaktPcbaInspectionDetailUpdateDto dto)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        dto.Adapt(entity);
                await StampPcbaInspectionDetailPcbaInspectionAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionDetailRepository,
            x => x.PcbaInspectionId == entity.PcbaInspectionId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
        }
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA检查明细不存在");
    }

    /// <summary>
    /// 删除PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionDetailByIdAsync(long id)
    {
        var deleted = await _pcbaInspectionDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA检查明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA检查明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaInspectionDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新PCBA检查明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> UpdatePcbaInspectionDetailStatusAsync(TaktPcbaInspectionDetailStatusDto dto)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(dto.PcbaInspectionDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        entity.InspectionStatus = dto.InspectionStatus;
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(dto.PcbaInspectionDetailId) ?? throw new TaktBusinessException("PCBA检查明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaInspectionDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaInspectionDetailTemplateDto>(
            sheetName ?? "PCBA检查明细导入模板",
            fileName ?? "PCBA检查明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA检查明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaInspectionDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaInspectionDetailImportDto>(fileStream, sheetName ?? "PCBA检查明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaInspectionDetail>();
                var importDto = rows[i].Adapt<TaktPcbaInspectionDetailCreateDto>();
                await StampPcbaInspectionDetailPcbaInspectionAsync(entity, importDto);
                var importKey = $"{entity.PcbaInspectionId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaInspectionId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaInspectionDetailRepository,
                    x => x.PcbaInspectionId == entity.PcbaInspectionId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                {
                    throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaInspectionDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaInspectionId == entity.PcbaInspectionId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaInspectionId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _pcbaInspectionDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA检查明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaInspectionDetailAsync(TaktPcbaInspectionDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaInspectionDetailQueryDto());
        var list = await _pcbaInspectionDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionDetailExportDto>(),
                sheetName ?? "PCBA检查明细数据",
                fileName ?? "PCBA检查明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaInspectionDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA检查明细数据",
            fileName ?? "PCBA检查明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步PCBA检查明细主表外键（ManyToOne → PCBA检查日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaInspectionDetailPcbaInspectionAsync(TaktPcbaInspectionDetail entity, TaktPcbaInspectionDetailCreateDto dto)
    {
        if (dto.PcbaInspectionId <= 0)
        {
            return;
        }
        var master = await _pcbaInspectionRepository.GetByIdAsync(dto.PcbaInspectionId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在");
        }
        entity.PcbaInspectionId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA检查明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaInspectionDetail, bool>> QueryExpression(TaktPcbaInspectionDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaInspectionDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PcbaInspectionId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.PcbaBoardType != null && x.PcbaBoardType.Contains(keywords))
                || (x.VisualInspectionLine != null && x.VisualInspectionLine.Contains(keywords))
                || (x.AoiLine != null && x.AoiLine.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || (x.InspectorName != null && x.InspectorName.Contains(keywords))
                || SqlFunc.ToString(x.DailyCompletedQty).Contains(keywords)
                || SqlFunc.ToString(x.InspectionQty).Contains(keywords)
                || SqlFunc.ToString(x.InspectionStatus).Contains(keywords)
                || (x.ProdLine != null && x.ProdLine.Contains(keywords))
                || SqlFunc.ToString(x.InspectionWorkHours).Contains(keywords)
                || SqlFunc.ToString(x.AoiWorkHours).Contains(keywords)
                || SqlFunc.ToString(x.DefectQty).Contains(keywords)
                || (x.HandPlacement != null && x.HandPlacement.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.DefectLocation != null && x.DefectLocation.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.BSideAssemblyDate).Contains(keywords)
                || SqlFunc.ToString(x.TSideAssemblyDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PcbaInspectionId.HasValue == true)
        {
            exp = exp.And(x => x.PcbaInspectionId == queryDto.PcbaInspectionId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.PcbaBoardType))
        {
            exp = exp.And(x => x.PcbaBoardType != null && x.PcbaBoardType.Contains(queryDto.PcbaBoardType));
        }

        if (!string.IsNullOrEmpty(queryDto?.VisualInspectionLine))
        {
            exp = exp.And(x => x.VisualInspectionLine != null && x.VisualInspectionLine.Contains(queryDto.VisualInspectionLine));
        }

        if (!string.IsNullOrEmpty(queryDto?.AoiLine))
        {
            exp = exp.And(x => x.AoiLine != null && x.AoiLine.Contains(queryDto.AoiLine));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectorName))
        {
            exp = exp.And(x => x.InspectorName != null && x.InspectorName.Contains(queryDto.InspectorName));
        }

        if (queryDto?.DailyCompletedQty.HasValue == true)
        {
            exp = exp.And(x => x.DailyCompletedQty == queryDto.DailyCompletedQty);
        }

        if (queryDto?.InspectionQty.HasValue == true)
        {
            exp = exp.And(x => x.InspectionQty == queryDto.InspectionQty);
        }

        if (queryDto?.InspectionStatus.HasValue == true)
        {
            exp = exp.And(x => x.InspectionStatus == queryDto.InspectionStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdLine))
        {
            exp = exp.And(x => x.ProdLine != null && x.ProdLine.Contains(queryDto.ProdLine));
        }

        if (queryDto?.InspectionWorkHours.HasValue == true)
        {
            exp = exp.And(x => x.InspectionWorkHours == queryDto.InspectionWorkHours);
        }

        if (queryDto?.AoiWorkHours.HasValue == true)
        {
            exp = exp.And(x => x.AoiWorkHours == queryDto.AoiWorkHours);
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            exp = exp.And(x => x.DefectQty == queryDto.DefectQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.HandPlacement))
        {
            exp = exp.And(x => x.HandPlacement != null && x.HandPlacement.Contains(queryDto.HandPlacement));
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNumber))
        {
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(queryDto.SerialNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.Content))
        {
            exp = exp.And(x => x.Content != null && x.Content.Contains(queryDto.Content));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectLocation))
        {
            exp = exp.And(x => x.DefectLocation != null && x.DefectLocation.Contains(queryDto.DefectLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.BSideAssemblyDateStart.HasValue == true)
        {
            exp = exp.And(x => x.BSideAssemblyDate >= queryDto.BSideAssemblyDateStart);
        }

        if (queryDto?.BSideAssemblyDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.BSideAssemblyDate <= queryDto.BSideAssemblyDateEnd);
        }

        if (queryDto?.TSideAssemblyDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TSideAssemblyDate >= queryDto.TSideAssemblyDateStart);
        }

        if (queryDto?.TSideAssemblyDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TSideAssemblyDate <= queryDto.TSideAssemblyDateEnd);
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
