// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailService.cs
// 创建时间：2026-06-06
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
    private readonly ITaktCompanyRepository<TaktEcDept> _ecDeptRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecDeptRepository">EcDept仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDetailService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
        _ecDeptRepository = ecDeptRepository;
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
        var dto = entity.Adapt<TaktEcDetailDto>();
        await FillEcDetailDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EcNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EcNo ?? e.Id.ToString(),
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
                await SaveEcDetailChildrenAsync(entity, dto);
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
                await SaveEcDetailChildrenAsync(entity, dto);
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
        await _ecDeptRepository.DeleteAsync(x => x.EcnDetailId == entity.Id);
        var deleted = await _ecDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变明细不存在或已删除");
        }
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
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充设变明细详情（加载 OneToMany 子表：设变部门）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEcDetailDetailsAsync(TaktEcDetailDto dto, TaktEcDetail entity)
    {
        if (dto == null)
        {
            return;
        }
        // 设变部门 → dto.DeptRecords
        var deptrecords = await _ecDeptRepository.GetListAsync(x => x.EcnDetailId == entity.Id);
        dto.DeptRecords = deptrecords.Adapt<List<TaktEcDeptDto>>();
    }

    /// <summary>
    /// 保存设变明细子表级联（设变部门；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEcDetailChildrenAsync(TaktEcDetail entity, TaktEcDetailCreateDto dto)
    {
        // 设变部门（DeptRecords）
        if (dto.DeptRecords is not { Count: > 0 })
        {
            await _ecDeptRepository.DeleteAsync(x => x.EcnDetailId == entity.Id);
        }
        else
        {
            var deptrecords = dto.DeptRecords.Adapt<List<TaktEcDept>>();
            foreach (var child in deptrecords)
            {
                child.EcnDetailId = entity.Id;
            }
            var deptrecordsNeedLine = deptrecords.Where(c => c.LineNumber <= 0).ToList();
            if (deptrecordsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecDeptRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcnDetailId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, deptrecordsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in deptrecords)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < deptrecords.Count; i++)
                        {
                            var key = $"{deptrecords[i].CompanyCode}|{deptrecords[i].EcnDetailId}|{deptrecords[i].DeptCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设变部门第{i + 1}项与本次提交的其他项重复（CompanyCode、EcnDetailId、DeptCode）");
                            }
                        }
            await _ecDeptRepository.DeleteAsync(x => x.EcnDetailId == entity.Id);
            foreach (var child in deptrecords)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_dept_unique = await _uniqueValidator.IsUniqueAsync(
                _ecDeptRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcnDetailId == child.EcnDetailId
                    && x.DeptCode == child.DeptCode);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_dept_unique)
            {
                throw new TaktBusinessException("设变部门的CompanyCode、EcnDetailId、DeptCode已存在");
            }
            }
            await _ecDeptRepository.CreateRangeAsync(deptrecords);
        }
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

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcId).Contains(keywords)
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.EcModel != null && x.EcModel.Contains(keywords))
                || (x.EcBomItem != null && x.EcBomItem.Contains(keywords))
                || (x.EcBomSubItem != null && x.EcBomSubItem.Contains(keywords))
                || (x.EcBomNo != null && x.EcBomNo.Contains(keywords))
                || (x.EcChange != null && x.EcChange.Contains(keywords))
                || (x.EcLocal != null && x.EcLocal.Contains(keywords))
                || (x.EcNote != null && x.EcNote.Contains(keywords))
                || (x.EcProcess != null && x.EcProcess.Contains(keywords))
                || (x.EcOldItem != null && x.EcOldItem.Contains(keywords))
                || (x.EcOldText != null && x.EcOldText.Contains(keywords))
                || SqlFunc.ToString(x.EcOldQty).Contains(keywords)
                || (x.EcOldSet != null && x.EcOldSet.Contains(keywords))
                || (x.EcNewItem != null && x.EcNewItem.Contains(keywords))
                || (x.EcNewText != null && x.EcNewText.Contains(keywords))
                || SqlFunc.ToString(x.EcNewQty).Contains(keywords)
                || (x.EcNewSet != null && x.EcNewSet.Contains(keywords))
                || SqlFunc.ToString(x.IsProcurement).Contains(keywords)
                || SqlFunc.ToString(x.IsCheck).Contains(keywords)
                || (x.EcWarehouse != null && x.EcWarehouse.Contains(keywords))
                || SqlFunc.ToString(x.IsEndOfLine).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcBomDate).Contains(keywords)
                || SqlFunc.ToString(x.EcEntryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EcId.HasValue == true)
        {
            exp = exp.And(x => x.EcId == queryDto.EcId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcModel))
        {
            exp = exp.And(x => x.EcModel != null && x.EcModel.Contains(queryDto.EcModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomItem))
        {
            exp = exp.And(x => x.EcBomItem != null && x.EcBomItem.Contains(queryDto.EcBomItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomSubItem))
        {
            exp = exp.And(x => x.EcBomSubItem != null && x.EcBomSubItem.Contains(queryDto.EcBomSubItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcBomNo))
        {
            exp = exp.And(x => x.EcBomNo != null && x.EcBomNo.Contains(queryDto.EcBomNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcChange))
        {
            exp = exp.And(x => x.EcChange != null && x.EcChange.Contains(queryDto.EcChange));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcLocal))
        {
            exp = exp.And(x => x.EcLocal != null && x.EcLocal.Contains(queryDto.EcLocal));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNote))
        {
            exp = exp.And(x => x.EcNote != null && x.EcNote.Contains(queryDto.EcNote));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcProcess))
        {
            exp = exp.And(x => x.EcProcess != null && x.EcProcess.Contains(queryDto.EcProcess));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldItem))
        {
            exp = exp.And(x => x.EcOldItem != null && x.EcOldItem.Contains(queryDto.EcOldItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldText))
        {
            exp = exp.And(x => x.EcOldText != null && x.EcOldText.Contains(queryDto.EcOldText));
        }

        if (queryDto?.EcOldQty.HasValue == true)
        {
            exp = exp.And(x => x.EcOldQty == queryDto.EcOldQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcOldSet))
        {
            exp = exp.And(x => x.EcOldSet != null && x.EcOldSet.Contains(queryDto.EcOldSet));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewItem))
        {
            exp = exp.And(x => x.EcNewItem != null && x.EcNewItem.Contains(queryDto.EcNewItem));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewText))
        {
            exp = exp.And(x => x.EcNewText != null && x.EcNewText.Contains(queryDto.EcNewText));
        }

        if (queryDto?.EcNewQty.HasValue == true)
        {
            exp = exp.And(x => x.EcNewQty == queryDto.EcNewQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNewSet))
        {
            exp = exp.And(x => x.EcNewSet != null && x.EcNewSet.Contains(queryDto.EcNewSet));
        }

        if (queryDto?.IsProcurement.HasValue == true)
        {
            exp = exp.And(x => x.IsProcurement == queryDto.IsProcurement);
        }

        if (queryDto?.IsCheck.HasValue == true)
        {
            exp = exp.And(x => x.IsCheck == queryDto.IsCheck);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcWarehouse))
        {
            exp = exp.And(x => x.EcWarehouse != null && x.EcWarehouse.Contains(queryDto.EcWarehouse));
        }

        if (queryDto?.IsEndOfLine.HasValue == true)
        {
            exp = exp.And(x => x.IsEndOfLine == queryDto.IsEndOfLine);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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

        if (queryDto?.EcEntryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate >= queryDto.EcEntryDateStart);
        }

        if (queryDto?.EcEntryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate <= queryDto.EcEntryDateEnd);
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
