// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良日报应用服务实现
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
/// 组立不良日报应用服务
/// </summary>
public class TaktAssyDefectService : TaktServiceBase, ITaktAssyDefectService
{
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">AssyDefectDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyDefectService(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyDefectRepository = assyDefectRepository;
        _assyDefectDetailRepository = assyDefectDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立不良日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyDefectDto>> GetAssyDefectListAsync(TaktAssyDefectQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyDefectRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyDefectDto>.Create(
            data.Adapt<List<TaktAssyDefectDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto?> GetAssyDefectByIdAsync(long id)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktAssyDefectDto>();
        await FillAssyDefectDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取组立不良日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyDefectOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _assyDefectRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建组立不良日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto> CreateAssyDefectAsync(TaktAssyDefectCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyDefect>();
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdLine == entity.ProdLine
                && x.ShiftNo == entity.ShiftNo
                && x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
        {
            throw new TaktBusinessException("组立不良日报的PlantCode、ProdCategory、ProdDate、ProdLine、ShiftNo、ProdOrderCode已存在");
        }
        entity = await _assyDefectRepository.CreateAsync(entity);
                await SaveAssyDefectChildrenAsync(entity, dto);
        return await GetAssyDefectByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyDefectDto>();
    }

    /// <summary>
    /// 更新组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto> UpdateAssyDefectAsync(long id, TaktAssyDefectUpdateDto dto)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
            _assyDefectRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdLine == entity.ProdLine
                && x.ShiftNo == entity.ShiftNo
                && x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
        {
            throw new TaktBusinessException("组立不良日报的PlantCode、ProdCategory、ProdDate、ProdLine、ShiftNo、ProdOrderCode已存在");
        }
        await _assyDefectRepository.UpdateAsync(entity);
                await SaveAssyDefectChildrenAsync(entity, dto);
        return await GetAssyDefectByIdAsync(id) ?? throw new TaktBusinessException("组立不良日报不存在");
    }

    /// <summary>
    /// 删除组立不良日报
    /// </summary>
    /// <param name="id">组立不良日报ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectByIdAsync(long id)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良日报不存在或已删除");
        }
        await _assyDefectDetailRepository.DeleteAsync(x => x.AssyDefectId == entity.Id);
        var deleted = await _assyDefectRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立不良日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除组立不良日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyDefectBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyDefectByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新组立不良日报状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyDefectDto> UpdateAssyDefectStatusAsync(TaktAssyDefectStatusDto dto)
    {
        var entity = await _assyDefectRepository.GetByIdAsync(dto.AssyDefectId);
        if (entity == null)
        {
            throw new TaktBusinessException("组立不良日报不存在");
        }
        entity.Status = dto.Status;
        await _assyDefectRepository.UpdateAsync(entity);
        return await GetAssyDefectByIdAsync(dto.AssyDefectId) ?? throw new TaktBusinessException("组立不良日报不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyDefectTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyDefectTemplateDto>(
            sheetName ?? "组立不良日报导入模板",
            fileName ?? "组立不良日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立不良日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyDefectAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyDefectImportDto>(fileStream, sheetName ?? "组立不良日报导入模板");
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
                var entity = rows[i].Adapt<TaktAssyDefect>();
                var importKey = $"{entity.PlantCode}|{entity.ProdCategory}|{entity.ProdDate}|{entity.ProdLine}|{entity.ShiftNo}|{entity.ProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProdCategory、ProdDate、ProdLine、ShiftNo、ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_assy_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyDefectRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProdCategory == entity.ProdCategory
                        && x.ProdDate == entity.ProdDate
                        && x.ProdLine == entity.ProdLine
                        && x.ShiftNo == entity.ShiftNo
                        && x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_unique)
                {
                    throw new TaktBusinessException("组立不良日报的PlantCode、ProdCategory、ProdDate、ProdLine、ShiftNo、ProdOrderCode已存在");
                }
                await _assyDefectRepository.CreateAsync(entity);
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
    /// 导出组立不良日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyDefectAsync(TaktAssyDefectQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssyDefectQueryDto());
        var list = await _assyDefectRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyDefectExportDto>(),
                sheetName ?? "组立不良日报数据",
                fileName ?? "组立不良日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyDefectExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立不良日报数据",
            fileName ?? "组立不良日报导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充组立不良日报详情（加载 OneToMany 子表：组立不良明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillAssyDefectDetailsAsync(TaktAssyDefectDto dto, TaktAssyDefect entity)
    {
        if (dto == null)
        {
            return;
        }
        // 组立不良明细 → dto.AssyDefectDetails
        var assydefectdetails = await _assyDefectDetailRepository.GetListAsync(x => x.AssyDefectId == entity.Id);
        dto.AssyDefectDetails = assydefectdetails.Adapt<List<TaktAssyDefectDetailDto>>();
    }

    /// <summary>
    /// 保存组立不良日报子表级联（组立不良明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveAssyDefectChildrenAsync(TaktAssyDefect entity, TaktAssyDefectCreateDto dto)
    {
        // 组立不良明细（AssyDefectDetails）
        if (dto.AssyDefectDetails is not { Count: > 0 })
        {
            await _assyDefectDetailRepository.DeleteAsync(x => x.AssyDefectId == entity.Id);
        }
        else
        {
            var assydefectdetails = dto.AssyDefectDetails.Adapt<List<TaktAssyDefectDetail>>();
            foreach (var child in assydefectdetails)
            {
                child.AssyDefectId = entity.Id;
            }
            var assydefectdetailsNeedLine = assydefectdetails.Where(c => c.LineNumber <= 0).ToList();
            if (assydefectdetailsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.ProdOrderCode) ? entity.ProdOrderCode : entity.Id.ToString();
                var maxLine = await _assyDefectDetailRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyDefectId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, assydefectdetailsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in assydefectdetails)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < assydefectdetails.Count; i++)
                        {
                            var key = $"{assydefectdetails[i].CompanyCode}|{assydefectdetails[i].AssyDefectId}|{assydefectdetails[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"组立不良明细第{i + 1}项与本次提交的其他项重复（CompanyCode、AssyDefectId、LineNumber）");
                            }
                        }
            await _assyDefectDetailRepository.DeleteAsync(x => x.AssyDefectId == entity.Id);
            foreach (var child in assydefectdetails)
            {
            var isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                _assyDefectDetailRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.AssyDefectId == child.AssyDefectId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_defect_assy_detail_line_unique)
            {
                throw new TaktBusinessException("组立不良明细的CompanyCode、AssyDefectId、LineNumber已存在");
            }
            }
            await _assyDefectDetailRepository.CreateRangeAsync(assydefectdetails);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立不良日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyDefect, bool>> QueryExpression(TaktAssyDefectQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyDefect>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ProdLine != null && x.ProdLine.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdOrderQty).Contains(keywords)
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.BatchNo != null && x.BatchNo.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdActualQty).Contains(keywords)
                || SqlFunc.ToString(x.GoodQuantity).Contains(keywords)
                || SqlFunc.ToString(x.Status).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ProdDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdCategory))
        {
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(queryDto.ProdCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdLine))
        {
            exp = exp.And(x => x.ProdLine != null && x.ProdLine.Contains(queryDto.ProdLine));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdOrderQty == queryDto.ProdOrderQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchNo))
        {
            exp = exp.And(x => x.BatchNo != null && x.BatchNo.Contains(queryDto.BatchNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdActualQty == queryDto.ProdActualQty);
        }

        if (queryDto?.GoodQuantity.HasValue == true)
        {
            exp = exp.And(x => x.GoodQuantity == queryDto.GoodQuantity);
        }

        if (queryDto?.Status.HasValue == true)
        {
            exp = exp.And(x => x.Status == queryDto.Status);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate >= queryDto.ProdDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate <= queryDto.ProdDateEnd);
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
