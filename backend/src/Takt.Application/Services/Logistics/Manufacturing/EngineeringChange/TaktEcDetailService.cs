// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变明细应用服务实现
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
/// 设变明细应用服务
/// </summary>
public class TaktEcDetailService : TaktServiceBase, ITaktEcDetailService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecGijutsuRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecGijutsuRepository">设变技术课主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDetailService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcGijutsu> ecGijutsuRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
        _ecGijutsuRepository = ecGijutsuRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDetailDto>> GetEcDetailListAsync(TaktEcDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcDetailDto>.Create(
            data.Adapt<List<TaktEcDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto?> GetEcDetailByIdAsync(long id)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcDetailDto>();
    }

    /// <summary>
    /// 获取设变明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EcCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EcCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> CreateEcDetailAsync(TaktEcDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcDetail>();
        entity.IsObsolete = 0;
        await StampEcDetailEcGijutsuAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDetailRepository,
            x => x.EcId == entity.EcId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
        {
            throw new TaktBusinessException("设变明细的EcId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.EcId,
                x => x.LineNumber);
            var businessCode = entity.EcId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecDetailRepository.CreateAsync(entity);
        return await GetEcDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktEcDetailDto>();
    }

    /// <summary>
    /// 更新设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> UpdateEcDetailAsync(long id, TaktEcDetailUpdateDto dto)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        dto.Adapt(entity);
        await StampEcDetailEcGijutsuAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecDetailRepository,
            x => x.EcId == entity.EcId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
        {
            throw new TaktBusinessException("设变明细的EcId、LineNumber已存在");
        }
        await _ecDetailRepository.UpdateAsync(entity);
        return await GetEcDetailByIdAsync(id) ?? throw new TaktBusinessException("设变明细不存在");
    }

    /// <summary>
    /// 删除设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDetailByIdAsync(long id)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变明细已作废");
        }
        entity.IsObsolete = 1;
        await _ecDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除设变明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcDetailDto> UpdateEcDetailObsoleteAsync(TaktEcDetailObsoleteDto dto)
    {
        var entity = await _ecDetailRepository.GetByIdAsync(dto.EcDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecDetailRepository.UpdateAsync(entity);
        return await GetEcDetailByIdAsync(dto.EcDetailId) ?? throw new TaktBusinessException("设变明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcDetailTemplateDto>(
            sheetName ?? "设变明细导入模板",
            fileName ?? "设变明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcDetailImportDto>(fileStream, sheetName ?? "设变明细导入模板");
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
                var entity = rows[i].Adapt<TaktEcDetail>();
                var importDto = rows[i].Adapt<TaktEcDetailCreateDto>();
                await StampEcDetailEcGijutsuAsync(entity, importDto);
                var importKey = $"{entity.EcId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecDetailRepository,
                    x => x.EcId == entity.EcId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
                {
                    throw new TaktBusinessException("设变明细的EcId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.EcId,
                        x => x.LineNumber);
                    var businessCode = entity.EcId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecDetailRepository.CreateAsync(entity);
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
    /// 导出设变明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcDetailAsync(TaktEcDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcDetailQueryDto());
        var list = await _ecDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcDetailExportDto>(),
                sheetName ?? "设变明细数据",
                fileName ?? "设变明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变明细数据",
            fileName ?? "设变明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设变明细主表外键（ManyToOne → 设变技术课主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcDetailEcGijutsuAsync(TaktEcDetail entity, TaktEcDetailCreateDto dto)
    {
        if (dto.EcId <= 0)
        {
            return;
        }
        var master = await _ecGijutsuRepository.GetByIdAsync(dto.EcId);
        if (master == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        entity.EcId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> QueryExpression(TaktEcDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();

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
                SqlFunc.ToString(x.EcId).Contains(keywords)
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.EcBomLineCode != null && x.EcBomLineCode.Contains(keywords))
                || (x.EcModel != null && x.EcModel.Contains(keywords))
                || (x.EcBomItem != null && x.EcBomItem.Contains(keywords))
                || (x.EcBomItemText != null && x.EcBomItemText.Contains(keywords))
                || (x.EcBomSubItem != null && x.EcBomSubItem.Contains(keywords))
                || (x.EcBomSubItemText != null && x.EcBomSubItemText.Contains(keywords))
                || SqlFunc.ToString(x.IsEndOfLine).Contains(keywords)
                || (x.EcOldItem != null && x.EcOldItem.Contains(keywords))
                || (x.EcOldText != null && x.EcOldText.Contains(keywords))
                || SqlFunc.ToString(x.EcOldUsage).Contains(keywords)
                || (x.EcOldPosition != null && x.EcOldPosition.Contains(keywords))
                || SqlFunc.ToString(x.EcOldStock).Contains(keywords)
                || (x.EcOldWarehouse != null && x.EcOldWarehouse.Contains(keywords))
                || SqlFunc.ToString(x.IsOldProcurement).Contains(keywords)
                || SqlFunc.ToString(x.IsOldCheck).Contains(keywords)
                || (x.EcNewItem != null && x.EcNewItem.Contains(keywords))
                || (x.EcNewText != null && x.EcNewText.Contains(keywords))
                || SqlFunc.ToString(x.EcNewUsage).Contains(keywords)
                || (x.EcNewPosition != null && x.EcNewPosition.Contains(keywords))
                || SqlFunc.ToString(x.EcNewStock).Contains(keywords)
                || (x.EcNewWarehouse != null && x.EcNewWarehouse.Contains(keywords))
                || SqlFunc.ToString(x.IsNewProcurement).Contains(keywords)
                || SqlFunc.ToString(x.IsNewCheck).Contains(keywords)
                || (x.EcIsCompatible != null && x.EcIsCompatible.Contains(keywords))
                || (x.EcSecondDistinction != null && x.EcSecondDistinction.Contains(keywords))
                || (x.EcInstruction != null && x.EcInstruction.Contains(keywords))
                || (x.EcLegacyPartDisposition != null && x.EcLegacyPartDisposition.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcBomDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EcId.HasValue == true)
        {
            exp = exp.And(x => x.EcId == queryDto.EcId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomLineCode))
        {
            exp = exp.And(x => x.EcBomLineCode != null && x.EcBomLineCode.Contains(queryDto.EcBomLineCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcModel))
        {
            exp = exp.And(x => x.EcModel != null && x.EcModel.Contains(queryDto.EcModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomItem))
        {
            exp = exp.And(x => x.EcBomItem != null && x.EcBomItem.Contains(queryDto.EcBomItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomItemText))
        {
            exp = exp.And(x => x.EcBomItemText != null && x.EcBomItemText.Contains(queryDto.EcBomItemText));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomSubItem))
        {
            exp = exp.And(x => x.EcBomSubItem != null && x.EcBomSubItem.Contains(queryDto.EcBomSubItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomSubItemText))
        {
            exp = exp.And(x => x.EcBomSubItemText != null && x.EcBomSubItemText.Contains(queryDto.EcBomSubItemText));
        }

        if (queryDto?.IsEndOfLine.HasValue == true)
        {
            exp = exp.And(x => x.IsEndOfLine == queryDto.IsEndOfLine);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldItem))
        {
            exp = exp.And(x => x.EcOldItem != null && x.EcOldItem.Contains(queryDto.EcOldItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldText))
        {
            exp = exp.And(x => x.EcOldText != null && x.EcOldText.Contains(queryDto.EcOldText));
        }

        if (queryDto?.EcOldUsage.HasValue == true)
        {
            exp = exp.And(x => x.EcOldUsage == queryDto.EcOldUsage);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldPosition))
        {
            exp = exp.And(x => x.EcOldPosition != null && x.EcOldPosition.Contains(queryDto.EcOldPosition));
        }

        if (queryDto?.EcOldStock.HasValue == true)
        {
            exp = exp.And(x => x.EcOldStock == queryDto.EcOldStock);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldWarehouse))
        {
            exp = exp.And(x => x.EcOldWarehouse != null && x.EcOldWarehouse.Contains(queryDto.EcOldWarehouse));
        }

        if (queryDto?.IsOldProcurement.HasValue == true)
        {
            exp = exp.And(x => x.IsOldProcurement == queryDto.IsOldProcurement);
        }

        if (queryDto?.IsOldCheck.HasValue == true)
        {
            exp = exp.And(x => x.IsOldCheck == queryDto.IsOldCheck);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewItem))
        {
            exp = exp.And(x => x.EcNewItem != null && x.EcNewItem.Contains(queryDto.EcNewItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewText))
        {
            exp = exp.And(x => x.EcNewText != null && x.EcNewText.Contains(queryDto.EcNewText));
        }

        if (queryDto?.EcNewUsage.HasValue == true)
        {
            exp = exp.And(x => x.EcNewUsage == queryDto.EcNewUsage);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewPosition))
        {
            exp = exp.And(x => x.EcNewPosition != null && x.EcNewPosition.Contains(queryDto.EcNewPosition));
        }

        if (queryDto?.EcNewStock.HasValue == true)
        {
            exp = exp.And(x => x.EcNewStock == queryDto.EcNewStock);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewWarehouse))
        {
            exp = exp.And(x => x.EcNewWarehouse != null && x.EcNewWarehouse.Contains(queryDto.EcNewWarehouse));
        }

        if (queryDto?.IsNewProcurement.HasValue == true)
        {
            exp = exp.And(x => x.IsNewProcurement == queryDto.IsNewProcurement);
        }

        if (queryDto?.IsNewCheck.HasValue == true)
        {
            exp = exp.And(x => x.IsNewCheck == queryDto.IsNewCheck);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcIsCompatible))
        {
            exp = exp.And(x => x.EcIsCompatible != null && x.EcIsCompatible.Contains(queryDto.EcIsCompatible));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcSecondDistinction))
        {
            exp = exp.And(x => x.EcSecondDistinction != null && x.EcSecondDistinction.Contains(queryDto.EcSecondDistinction));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcInstruction))
        {
            exp = exp.And(x => x.EcInstruction != null && x.EcInstruction.Contains(queryDto.EcInstruction));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcLegacyPartDisposition))
        {
            exp = exp.And(x => x.EcLegacyPartDisposition != null && x.EcLegacyPartDisposition.Contains(queryDto.EcLegacyPartDisposition));
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

        if (queryDto?.EcBomDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcBomDate >= queryDto.EcBomDateStart);
        }

        if (queryDto?.EcBomDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcBomDate <= queryDto.EcBomDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
