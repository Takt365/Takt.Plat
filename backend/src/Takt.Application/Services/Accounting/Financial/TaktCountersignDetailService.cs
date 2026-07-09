// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCountersignDetailService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会签单明细应用服务
/// </summary>
public class TaktCountersignDetailService : TaktServiceBase, ITaktCountersignDetailService
{
    private readonly ITaktCompanyRepository<TaktCountersignDetail> _countersignDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignDetailRepository">会签单明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCountersignDetailService(
        ITaktCompanyRepository<TaktCountersignDetail> countersignDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _countersignDetailRepository = countersignDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会签单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCountersignDetailDto>> GetCountersignDetailListAsync(TaktCountersignDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _countersignDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCountersignDetailDto>.Create(
            data.Adapt<List<TaktCountersignDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDetailDto?> GetCountersignDetailByIdAsync(long id)
    {
        var entity = await _countersignDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCountersignDetailDto>();
    }

    /// <summary>
    /// 获取会签单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCountersignDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _countersignDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CountersignCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CountersignCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会签单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDetailDto> CreateCountersignDetailAsync(TaktCountersignDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktCountersignDetail>();
        entity.IsObsolete = 0;
        var isUnique_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignDetailRepository,
            x => x.CountersignId == entity.CountersignId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_countersign_detail_line_unique)
        {
            throw new TaktBusinessException("会签单明细的CountersignId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _countersignDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CountersignId == entity.CountersignId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.CountersignCode) ? entity.CountersignCode : entity.CountersignId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _countersignDetailRepository.CreateAsync(entity);
        return await GetCountersignDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktCountersignDetailDto>();
    }

    /// <summary>
    /// 更新会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDetailDto> UpdateCountersignDetailAsync(long id, TaktCountersignDetailUpdateDto dto)
    {
        var entity = await _countersignDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignDetailRepository,
            x => x.CountersignId == entity.CountersignId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_countersign_detail_line_unique)
        {
            throw new TaktBusinessException("会签单明细的CountersignId、LineNumber已存在");
        }
        await _countersignDetailRepository.UpdateAsync(entity);
        return await GetCountersignDetailByIdAsync(id) ?? throw new TaktBusinessException("会签单明细不存在");
    }

    /// <summary>
    /// 删除会签单明细
    /// </summary>
    /// <param name="id">会签单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignDetailByIdAsync(long id)
    {
        var entity = await _countersignDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会签单明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("会签单明细已作废");
        }
        entity.IsObsolete = 1;
        await _countersignDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除会签单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCountersignDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会签单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDetailDto> UpdateCountersignDetailObsoleteAsync(TaktCountersignDetailObsoleteDto dto)
    {
        var entity = await _countersignDetailRepository.GetByIdAsync(dto.CountersignDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会签单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _countersignDetailRepository.UpdateAsync(entity);
        return await GetCountersignDetailByIdAsync(dto.CountersignDetailId) ?? throw new TaktBusinessException("会签单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCountersignDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCountersignDetailTemplateDto>(
            sheetName ?? "会签单明细导入模板",
            fileName ?? "会签单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入会签单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCountersignDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCountersignDetailImportDto>(fileStream, sheetName ?? "会签单明细导入模板");
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
                var entity = rows[i].Adapt<TaktCountersignDetail>();
                var importKey = $"{entity.CountersignId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountersignId、LineNumber）");
                }
                var isUnique_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _countersignDetailRepository,
                    x => x.CountersignId == entity.CountersignId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_countersign_detail_line_unique)
                {
                    throw new TaktBusinessException("会签单明细的CountersignId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _countersignDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CountersignId == entity.CountersignId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CountersignCode) ? entity.CountersignCode : entity.CountersignId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _countersignDetailRepository.CreateAsync(entity);
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
    /// 导出会签单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCountersignDetailAsync(TaktCountersignDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCountersignDetailQueryDto());
        var list = await _countersignDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCountersignDetailExportDto>(),
                sheetName ?? "会签单明细数据",
                fileName ?? "会签单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCountersignDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会签单明细数据",
            fileName ?? "会签单明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会签单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCountersignDetail, bool>> QueryExpression(TaktCountersignDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCountersignDetail>();

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
                SqlFunc.ToString(x.CountersignId).Contains(keywords)
                || (x.CountersignCode != null && x.CountersignCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.AllocationCategory != null && x.AllocationCategory.Contains(keywords))
                || (x.AccountTitle != null && x.AccountTitle.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.ItemDescription != null && x.ItemDescription.Contains(keywords))
                || SqlFunc.ToString(x.ItemQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ItemAmount).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.CountersignId.HasValue == true)
        {
            exp = exp.And(x => x.CountersignId == queryDto.CountersignId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CountersignCode))
        {
            exp = exp.And(x => x.CountersignCode != null && x.CountersignCode.Contains(queryDto.CountersignCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.AllocationCategory))
        {
            exp = exp.And(x => x.AllocationCategory != null && x.AllocationCategory.Contains(queryDto.AllocationCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountTitle))
        {
            exp = exp.And(x => x.AccountTitle != null && x.AccountTitle.Contains(queryDto.AccountTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemName))
        {
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(queryDto.ItemName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemDescription))
        {
            exp = exp.And(x => x.ItemDescription != null && x.ItemDescription.Contains(queryDto.ItemDescription));
        }

        if (queryDto?.ItemQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ItemQuantity == queryDto.ItemQuantity);
        }

        if (queryDto?.ItemAmount.HasValue == true)
        {
            exp = exp.And(x => x.ItemAmount == queryDto.ItemAmount);
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
