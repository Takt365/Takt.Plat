// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修明细应用服务实现
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
/// PCBA改修明细应用服务
/// </summary>
public class TaktPcbaRepairDetailService : TaktServiceBase, ITaktPcbaRepairDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaRepairDetail> _pcbaRepairDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaRepair> _pcbaRepairRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairDetailRepository">PCBA改修明细仓储</param>
    /// <param name="pcbaRepairRepository">PCBA改修日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaRepairDetailService(
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaRepairDetailRepository = pcbaRepairDetailRepository;
        _pcbaRepairRepository = pcbaRepairRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA改修明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaRepairDetailDto>> GetPcbaRepairDetailListAsync(TaktPcbaRepairDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaRepairDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaRepairDetailDto>.Create(
            data.Adapt<List<TaktPcbaRepairDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto?> GetPcbaRepairDetailByIdAsync(long id)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaRepairDetailDto>();
    }

    /// <summary>
    /// 获取PCBA改修明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaRepairDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _pcbaRepairDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProdOrderCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA改修明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto> CreatePcbaRepairDetailAsync(TaktPcbaRepairDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaRepairDetail>();
                await StampPcbaRepairDetailPcbaRepairAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaRepairDetailRepository,
            x => x.PcbaRepairId == entity.PcbaRepairId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaRepairDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaRepairId == entity.PcbaRepairId,
                x => x.LineNumber);
            var businessCode = entity.PcbaRepairId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _pcbaRepairDetailRepository.CreateAsync(entity);
        return await GetPcbaRepairDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaRepairDetailDto>();
    }

    /// <summary>
    /// 更新PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto> UpdatePcbaRepairDetailAsync(long id, TaktPcbaRepairDetailUpdateDto dto)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA改修明细不存在");
        }
        dto.Adapt(entity);
                await StampPcbaRepairDetailPcbaRepairAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaRepairDetailRepository,
            x => x.PcbaRepairId == entity.PcbaRepairId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
        }
        await _pcbaRepairDetailRepository.UpdateAsync(entity);
        return await GetPcbaRepairDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA改修明细不存在");
    }

    /// <summary>
    /// 删除PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaRepairDetailByIdAsync(long id)
    {
        var deleted = await _pcbaRepairDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA改修明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA改修明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaRepairDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaRepairDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaRepairDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaRepairDetailTemplateDto>(
            sheetName ?? "PCBA改修明细导入模板",
            fileName ?? "PCBA改修明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA改修明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaRepairDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaRepairDetailImportDto>(fileStream, sheetName ?? "PCBA改修明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaRepairDetail>();
                var importDto = rows[i].Adapt<TaktPcbaRepairDetailCreateDto>();
                await StampPcbaRepairDetailPcbaRepairAsync(entity, importDto);
                var importKey = $"{entity.PcbaRepairId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaRepairId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaRepairDetailRepository,
                    x => x.PcbaRepairId == entity.PcbaRepairId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
                {
                    throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaRepairDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaRepairId == entity.PcbaRepairId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaRepairId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _pcbaRepairDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA改修明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaRepairDetailAsync(TaktPcbaRepairDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaRepairDetailQueryDto());
        var list = await _pcbaRepairDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaRepairDetailExportDto>(),
                sheetName ?? "PCBA改修明细数据",
                fileName ?? "PCBA改修明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaRepairDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA改修明细数据",
            fileName ?? "PCBA改修明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步PCBA改修明细主表外键（ManyToOne → PCBA改修日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaRepairDetailPcbaRepairAsync(TaktPcbaRepairDetail entity, TaktPcbaRepairDetailCreateDto dto)
    {
        if (dto.PcbaRepairId <= 0)
        {
            return;
        }
        var master = await _pcbaRepairRepository.GetByIdAsync(dto.PcbaRepairId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA改修日报不存在");
        }
        entity.PcbaRepairId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA改修明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaRepairDetail, bool>> QueryExpression(TaktPcbaRepairDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaRepairDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PcbaRepairId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.PcbaBoardType != null && x.PcbaBoardType.Contains(keywords))
                || SqlFunc.ToString(x.ProdActualQty).Contains(keywords)
                || (x.ProdLine != null && x.ProdLine.Contains(keywords))
                || (x.CardNo != null && x.CardNo.Contains(keywords))
                || (x.DefectSymptom != null && x.DefectSymptom.Contains(keywords))
                || (x.DefectEngineering != null && x.DefectEngineering.Contains(keywords))
                || (x.DefectReason != null && x.DefectReason.Contains(keywords))
                || SqlFunc.ToString(x.DefectQty).Contains(keywords)
                || (x.DefectResponsibility != null && x.DefectResponsibility.Contains(keywords))
                || (x.DefectNature != null && x.DefectNature.Contains(keywords))
                || (x.RepairOperator != null && x.RepairOperator.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PcbaRepairId.HasValue == true)
        {
            exp = exp.And(x => x.PcbaRepairId == queryDto.PcbaRepairId);
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

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdActualQty == queryDto.ProdActualQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdLine))
        {
            exp = exp.And(x => x.ProdLine != null && x.ProdLine.Contains(queryDto.ProdLine));
        }

        if (!string.IsNullOrEmpty(queryDto?.CardNo))
        {
            exp = exp.And(x => x.CardNo != null && x.CardNo.Contains(queryDto.CardNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectSymptom))
        {
            exp = exp.And(x => x.DefectSymptom != null && x.DefectSymptom.Contains(queryDto.DefectSymptom));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectEngineering))
        {
            exp = exp.And(x => x.DefectEngineering != null && x.DefectEngineering.Contains(queryDto.DefectEngineering));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectReason))
        {
            exp = exp.And(x => x.DefectReason != null && x.DefectReason.Contains(queryDto.DefectReason));
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            exp = exp.And(x => x.DefectQty == queryDto.DefectQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectResponsibility))
        {
            exp = exp.And(x => x.DefectResponsibility != null && x.DefectResponsibility.Contains(queryDto.DefectResponsibility));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectNature))
        {
            exp = exp.And(x => x.DefectNature != null && x.DefectNature.Contains(queryDto.DefectNature));
        }

        if (!string.IsNullOrEmpty(queryDto?.RepairOperator))
        {
            exp = exp.And(x => x.RepairOperator != null && x.RepairOperator.Contains(queryDto.RepairOperator));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
