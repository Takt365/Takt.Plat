// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetailService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良明细应用服务实现
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

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良明细应用服务
/// </summary>
public class TaktAssyDefectDetailService : TaktServiceBase, ITaktAssyDefectDetailService
{
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyDefectDetailService(
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyDefectDetailRepository = assyDefectDetailRepository;
        _assyDefectRepository = assyDefectRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立不良明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyDefectDetailDto>> GetAssyDefectDetailListAsync(TaktAssyDefectDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyDefectDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyDefectDetailDto>.Create(
            data.Adapt<List<TaktAssyDefectDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto?> GetAssyDefectDetailByIdAsync(long id)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssyDefectDetailDto>();
    }

    /// <summary>
    /// 获取组立不良明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyDefectDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _assyDefectDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProdOrderCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建组立不良明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto> CreateAssyDefectDetailAsync(TaktAssyDefectDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyDefectDetail>();
        await StampAssyDefectDetailAssyDefectAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectDetailRepository,
            x => x.AssyDefectId == entity.AssyDefectId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _assyDefectDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyDefectId == entity.AssyDefectId,
                x => x.LineNumber);
            var businessCode = entity.AssyDefectId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _assyDefectDetailRepository.CreateAsync(entity);
        return await GetAssyDefectDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyDefectDetailDto>();
    }

    /// <summary>
    /// 更新组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDetailDto> UpdateAssyDefectDetailAsync(long id, TaktAssyDefectDetailUpdateDto dto)
    {
        var entity = await _assyDefectDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良明细不存在");
        }
        dto.Adapt(entity);
        await StampAssyDefectDetailAssyDefectAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectDetailRepository,
            x => x.AssyDefectId == entity.AssyDefectId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
        }
        await _assyDefectDetailRepository.UpdateAsync(entity);
        return await GetAssyDefectDetailByIdAsync(id) ?? throw new TaktBusinessException("组立不良明细不存在");
    }

    /// <summary>
    /// 删除组立不良明细
    /// </summary>
    /// <param name="id">组立不良明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectDetailByIdAsync(long id)
    {
        var deleted = await _assyDefectDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立不良明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除组立不良明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyDefectDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyDefectDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyDefectDetailTemplateDto>(
            sheetName ?? "组立不良明细导入模板",
            fileName ?? "组立不良明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立不良明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyDefectDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyDefectDetailImportDto>(fileStream, sheetName ?? "组立不良明细导入模板");
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
                var entity = rows[i].Adapt<TaktAssyDefectDetail>();
                var importDto = rows[i].Adapt<TaktAssyDefectDetailCreateDto>();
                await StampAssyDefectDetailAssyDefectAsync(entity, importDto);
                var importKey = $"{entity.AssyDefectId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssyDefectId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyDefectDetailRepository,
                    x => x.AssyDefectId == entity.AssyDefectId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
                {
                    throw new TaktBusinessException("组立不良明细的AssyDefectId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _assyDefectDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyDefectId == entity.AssyDefectId,
                        x => x.LineNumber);
                    var businessCode = entity.AssyDefectId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _assyDefectDetailRepository.CreateAsync(entity);
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
    /// 导出组立不良明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyDefectDetailAsync(TaktAssyDefectDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssyDefectDetailQueryDto());
        var list = await _assyDefectDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectDetailExportDto>(),
                sheetName ?? "组立不良明细数据",
                fileName ?? "组立不良明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyDefectDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立不良明细数据",
            fileName ?? "组立不良明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步组立不良明细主表外键（ManyToOne → 组立不良日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampAssyDefectDetailAssyDefectAsync(TaktAssyDefectDetail entity, TaktAssyDefectDetailCreateDto dto)
    {
        if (dto.AssyDefectId <= 0)
        {
            return;
        }
        var master = await _assyDefectRepository.GetByIdAsync(dto.AssyDefectId);
        if (master == null)
        {
            throw new TaktBusinessException("组立不良日报不存在");
        }
        entity.AssyDefectId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立不良明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyDefectDetail, bool>> QueryExpression(TaktAssyDefectDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyDefectDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.AssyDefectId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DefectCategory != null && x.DefectCategory.Contains(keywords))
                || SqlFunc.ToString(x.DefectQty).Contains(keywords)
                || SqlFunc.ToString(x.CumulativeDefectQty).Contains(keywords)
                || (x.RandomCardNo != null && x.RandomCardNo.Contains(keywords))
                || (x.OccurrenceEngineering != null && x.OccurrenceEngineering.Contains(keywords))
                || (x.TestStep != null && x.TestStep.Contains(keywords))
                || (x.DefectSymptom != null && x.DefectSymptom.Contains(keywords))
                || (x.DefectLocation != null && x.DefectLocation.Contains(keywords))
                || (x.DefectReason != null && x.DefectReason.Contains(keywords))
                || (x.RepairOperator != null && x.RepairOperator.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.AssyDefectId.HasValue == true)
        {
            exp = exp.And(x => x.AssyDefectId == queryDto.AssyDefectId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectCategory))
        {
            exp = exp.And(x => x.DefectCategory != null && x.DefectCategory.Contains(queryDto.DefectCategory));
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            exp = exp.And(x => x.DefectQty == queryDto.DefectQty);
        }

        if (queryDto?.CumulativeDefectQty.HasValue == true)
        {
            exp = exp.And(x => x.CumulativeDefectQty == queryDto.CumulativeDefectQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.RandomCardNo))
        {
            exp = exp.And(x => x.RandomCardNo != null && x.RandomCardNo.Contains(queryDto.RandomCardNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.OccurrenceEngineering))
        {
            exp = exp.And(x => x.OccurrenceEngineering != null && x.OccurrenceEngineering.Contains(queryDto.OccurrenceEngineering));
        }

        if (!string.IsNullOrEmpty(queryDto?.TestStep))
        {
            exp = exp.And(x => x.TestStep != null && x.TestStep.Contains(queryDto.TestStep));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectSymptom))
        {
            exp = exp.And(x => x.DefectSymptom != null && x.DefectSymptom.Contains(queryDto.DefectSymptom));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectLocation))
        {
            exp = exp.And(x => x.DefectLocation != null && x.DefectLocation.Contains(queryDto.DefectLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectReason))
        {
            exp = exp.And(x => x.DefectReason != null && x.DefectReason.Contains(queryDto.DefectReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.RepairOperator))
        {
            exp = exp.And(x => x.RepairOperator != null && x.RepairOperator.Contains(queryDto.RepairOperator));
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
