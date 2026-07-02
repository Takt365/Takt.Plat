// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceCustomerResponseService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务顾客品质要求对应费用明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质业务顾客品质要求对应费用明细应用服务
/// </summary>
public class TaktQualityAssuranceCustomerResponseService : TaktServiceBase, ITaktQualityAssuranceCustomerResponseService
{
    private readonly ITaktCompanyRepository<TaktQualityAssuranceCustomerResponse> _qualityAssuranceCustomerResponseRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssurance> _qualityAssuranceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityAssuranceCustomerResponseRepository">品质业务顾客品质要求对应费用明细仓储</param>
    /// <param name="qualityAssuranceRepository">品质业务主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityAssuranceCustomerResponseService(
        ITaktCompanyRepository<TaktQualityAssuranceCustomerResponse> qualityAssuranceCustomerResponseRepository,
        ITaktCompanyRepository<TaktQualityAssurance> qualityAssuranceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityAssuranceCustomerResponseRepository = qualityAssuranceCustomerResponseRepository;
        _qualityAssuranceRepository = qualityAssuranceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质业务顾客品质要求对应费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityAssuranceCustomerResponseDto>> GetQualityAssuranceCustomerResponseListAsync(TaktQualityAssuranceCustomerResponseQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityAssuranceCustomerResponseRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityAssuranceCustomerResponseDto>.Create(
            data.Adapt<List<TaktQualityAssuranceCustomerResponseDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="id">品质业务顾客品质要求对应费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceCustomerResponseDto?> GetQualityAssuranceCustomerResponseByIdAsync(long id)
    {
        var entity = await _qualityAssuranceCustomerResponseRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityAssuranceCustomerResponseDto>();
    }

    /// <summary>
    /// 获取品质业务顾客品质要求对应费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityAssuranceCustomerResponseOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _qualityAssuranceCustomerResponseRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.QualityAssuranceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.QualityAssuranceCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceCustomerResponseDto> CreateQualityAssuranceCustomerResponseAsync(TaktQualityAssuranceCustomerResponseCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityAssuranceCustomerResponse>();
        await StampQualityAssuranceCustomerResponseQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceCustomerResponseRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique)
        {
            throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的QualityAssuranceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityAssuranceCustomerResponseRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityAssuranceCustomerResponseRepository.CreateAsync(entity);
        return await GetQualityAssuranceCustomerResponseByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityAssuranceCustomerResponseDto>();
    }

    /// <summary>
    /// 更新品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="id">品质业务顾客品质要求对应费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceCustomerResponseDto> UpdateQualityAssuranceCustomerResponseAsync(long id, TaktQualityAssuranceCustomerResponseUpdateDto dto)
    {
        var entity = await _qualityAssuranceCustomerResponseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务顾客品质要求对应费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityAssuranceCustomerResponseQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceCustomerResponseRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique)
        {
            throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的QualityAssuranceId、LineNumber已存在");
        }
        await _qualityAssuranceCustomerResponseRepository.UpdateAsync(entity);
        return await GetQualityAssuranceCustomerResponseByIdAsync(id) ?? throw new TaktBusinessException("品质业务顾客品质要求对应费用明细不存在");
    }

    /// <summary>
    /// 删除品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="id">品质业务顾客品质要求对应费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceCustomerResponseByIdAsync(long id)
    {
        var deleted = await _qualityAssuranceCustomerResponseRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质业务顾客品质要求对应费用明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceCustomerResponseBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityAssuranceCustomerResponseByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityAssuranceCustomerResponseTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityAssuranceCustomerResponseTemplateDto>(
            sheetName ?? "品质业务顾客品质要求对应费用明细导入模板",
            fileName ?? "品质业务顾客品质要求对应费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityAssuranceCustomerResponseAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityAssuranceCustomerResponseImportDto>(fileStream, sheetName ?? "品质业务顾客品质要求对应费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityAssuranceCustomerResponse>();
                var importDto = rows[i].Adapt<TaktQualityAssuranceCustomerResponseCreateDto>();
                await StampQualityAssuranceCustomerResponseQualityAssuranceAsync(entity, importDto);
                var importKey = $"{entity.QualityAssuranceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityAssuranceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityAssuranceCustomerResponseRepository,
                    x => x.QualityAssuranceId == entity.QualityAssuranceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_assurance_customer_response_line_unique)
                {
                    throw new TaktBusinessException("品质业务顾客品质要求对应费用明细的QualityAssuranceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityAssuranceCustomerResponseRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityAssuranceCustomerResponseRepository.CreateAsync(entity);
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
    /// 导出品质业务顾客品质要求对应费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityAssuranceCustomerResponseAsync(TaktQualityAssuranceCustomerResponseQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityAssuranceCustomerResponseQueryDto());
        var list = await _qualityAssuranceCustomerResponseRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceCustomerResponseExportDto>(),
                sheetName ?? "品质业务顾客品质要求对应费用明细数据",
                fileName ?? "品质业务顾客品质要求对应费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityAssuranceCustomerResponseExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质业务顾客品质要求对应费用明细数据",
            fileName ?? "品质业务顾客品质要求对应费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步品质业务顾客品质要求对应费用明细主表外键（ManyToOne → 品质业务主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityAssuranceCustomerResponseQualityAssuranceAsync(TaktQualityAssuranceCustomerResponse entity, TaktQualityAssuranceCustomerResponseCreateDto dto)
    {
        if (dto.QualityAssuranceId <= 0)
        {
            return;
        }
        var master = await _qualityAssuranceRepository.GetByIdAsync(dto.QualityAssuranceId);
        if (master == null)
        {
            throw new TaktBusinessException("品质业务主不存在");
        }
        entity.QualityAssuranceId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质业务顾客品质要求对应费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityAssuranceCustomerResponse, bool>> QueryExpression(TaktQualityAssuranceCustomerResponseQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityAssuranceCustomerResponse>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.QualityAssuranceId).Contains(keywords)
                || (x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.ResponseCost).Contains(keywords)
                || SqlFunc.ToString(x.WorkTimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.OtherExpenses).Contains(keywords)
                || (x.CustomerResponseNote != null && x.CustomerResponseNote.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.QualityAssuranceId.HasValue == true)
        {
            exp = exp.And(x => x.QualityAssuranceId == queryDto.QualityAssuranceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityAssuranceCode))
        {
            exp = exp.And(x => x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(queryDto.QualityAssuranceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.ResponseCost.HasValue == true)
        {
            exp = exp.And(x => x.ResponseCost == queryDto.ResponseCost);
        }

        if (queryDto?.WorkTimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.WorkTimeMinutes == queryDto.WorkTimeMinutes);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            exp = exp.And(x => x.OtherExpenses == queryDto.OtherExpenses);
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerResponseNote))
        {
            exp = exp.And(x => x.CustomerResponseNote != null && x.CustomerResponseNote.Contains(queryDto.CustomerResponseNote));
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
