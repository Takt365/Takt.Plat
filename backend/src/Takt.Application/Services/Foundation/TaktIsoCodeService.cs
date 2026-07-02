// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktIsoCodeService.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：ISO编码应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// ISO编码应用服务
/// </summary>
public class TaktIsoCodeService : TaktServiceBase, ITaktIsoCodeService
{
    private readonly ITaktTenantRepository<TaktIsoCode> _isoCodeRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isoCodeRepository">ISO编码仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIsoCodeService(
        ITaktTenantRepository<TaktIsoCode> isoCodeRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _isoCodeRepository = isoCodeRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取ISO编码列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktIsoCodeDto>> GetIsoCodeListAsync(TaktIsoCodeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _isoCodeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktIsoCodeDto>.Create(
            data.Adapt<List<TaktIsoCodeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktIsoCodeDto?> GetIsoCodeByIdAsync(long id)
    {
        var entity = await _isoCodeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktIsoCodeDto>();
    }

    /// <summary>
    /// 获取ISO编码选项列表（仅启用；DictValue=IsoCode 供编号规则等部门段引用）
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetIsoCodeOptionsAsync(int? isoCodeCategory = null)
    {
        var list = await _isoCodeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.IsoCodeStatus == 1
                && (isoCodeCategory == null || x.IsoCodeCategory == isoCodeCategory),
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.IsoCode,
            DictLabel = string.IsNullOrWhiteSpace(e.IsoName) ? e.IsoCode : e.IsoName,
            ExtValue = e.IsoCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建ISO编码
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIsoCodeDto> CreateIsoCodeAsync(TaktIsoCodeCreateDto dto)
    {
        var entity = dto.Adapt<TaktIsoCode>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_iso_code_category_unique = await _uniqueValidator.IsUniqueAsync(
            _isoCodeRepository,
            x => x.IsoCodeCategory == entity.IsoCodeCategory
                && x.IsoCode == entity.IsoCode);
        if (!isUnique_ix_iso_code_category_unique)
        {
            throw new TaktBusinessException("ISO编码的IsoCodeCategory、IsoCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _isoCodeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _isoCodeRepository.CreateAsync(entity);
        return await GetIsoCodeByIdAsync(entity.Id) ?? entity.Adapt<TaktIsoCodeDto>();
    }

    /// <summary>
    /// 更新ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIsoCodeDto> UpdateIsoCodeAsync(long id, TaktIsoCodeUpdateDto dto)
    {
        var entity = await _isoCodeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("ISO编码不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_iso_code_category_unique = await _uniqueValidator.IsUniqueAsync(
            _isoCodeRepository,
            x => x.IsoCodeCategory == entity.IsoCodeCategory
                && x.IsoCode == entity.IsoCode,
            id);
        if (!isUnique_ix_iso_code_category_unique)
        {
            throw new TaktBusinessException("ISO编码的IsoCodeCategory、IsoCode已存在");
        }
        await _isoCodeRepository.UpdateAsync(entity);
        return await GetIsoCodeByIdAsync(id) ?? throw new TaktBusinessException("ISO编码不存在");
    }

    /// <summary>
    /// 删除ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>任务</returns>
    public async Task DeleteIsoCodeByIdAsync(long id)
    {
        var entity = await _isoCodeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("ISO编码不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置ISO编码不允许删除");
        }
        var deleted = await _isoCodeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("ISO编码不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除ISO编码
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteIsoCodeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _isoCodeRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置ISO编码不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteIsoCodeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新ISO编码状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIsoCodeDto> UpdateIsoCodeStatusAsync(TaktIsoCodeStatusDto dto)
    {
        var entity = await _isoCodeRepository.GetByIdAsync(dto.IsoCodeId);
        if (entity == null)
        {
            throw new TaktBusinessException("ISO编码不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.IsoCodeStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置ISO编码");
        }
        entity.IsoCodeStatus = dto.IsoCodeStatus;
        await _isoCodeRepository.UpdateAsync(entity);
        return await GetIsoCodeByIdAsync(dto.IsoCodeId) ?? throw new TaktBusinessException("ISO编码不存在");
    }

    /// <summary>
    /// 更新ISO编码排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktIsoCodeDto> UpdateIsoCodeSortAsync(TaktIsoCodeSortDto dto)
    {
        var entity = await _isoCodeRepository.GetByIdAsync(dto.IsoCodeId);
        if (entity == null)
        {
            throw new TaktBusinessException("ISO编码不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _isoCodeRepository.UpdateAsync(entity);
        return await GetIsoCodeByIdAsync(dto.IsoCodeId) ?? throw new TaktBusinessException("ISO编码不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetIsoCodeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktIsoCodeTemplateDto>(
            sheetName ?? "ISO编码导入模板",
            fileName ?? "ISO编码导入模板.xlsx");
    }

    /// <summary>
    /// 导入ISO编码
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportIsoCodeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktIsoCodeImportDto>(fileStream, sheetName ?? "ISO编码导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _isoCodeRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktIsoCode>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.IsoCodeCategory}|{entity.IsoCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（IsoCodeCategory、IsoCode）");
                }
                var isUnique_ix_iso_code_category_unique = await _uniqueValidator.IsUniqueAsync(
                    _isoCodeRepository,
                    x => x.IsoCodeCategory == entity.IsoCodeCategory
                        && x.IsoCode == entity.IsoCode);
                if (!isUnique_ix_iso_code_category_unique)
                {
                    throw new TaktBusinessException("ISO编码的IsoCodeCategory、IsoCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _isoCodeRepository.CreateAsync(entity);
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
    /// 导出ISO编码
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIsoCodeAsync(TaktIsoCodeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktIsoCodeQueryDto());
        var list = await _isoCodeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktIsoCodeExportDto>(),
                sheetName ?? "ISO编码数据",
                fileName ?? "ISO编码导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktIsoCodeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "ISO编码数据",
            fileName ?? "ISO编码导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建ISO编码查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktIsoCode, bool>> QueryExpression(TaktIsoCodeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktIsoCode>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.IsoCodeCategory).Contains(keywords)
                || (x.IsoCode != null && x.IsoCode.Contains(keywords))
                || (x.IsoName != null && x.IsoName.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || (x.IsoCodeDescription != null && x.IsoCodeDescription.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.IsoCodeStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.IsoCodeCategory.HasValue == true)
        {
            exp = exp.And(x => x.IsoCodeCategory == queryDto.IsoCodeCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.IsoCode))
        {
            exp = exp.And(x => x.IsoCode != null && x.IsoCode.Contains(queryDto.IsoCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.IsoName))
        {
            exp = exp.And(x => x.IsoName != null && x.IsoName.Contains(queryDto.IsoName));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (!string.IsNullOrEmpty(queryDto?.IsoCodeDescription))
        {
            exp = exp.And(x => x.IsoCodeDescription != null && x.IsoCodeDescription.Contains(queryDto.IsoCodeDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.IsoCodeStatus.HasValue == true)
        {
            exp = exp.And(x => x.IsoCodeStatus == queryDto.IsoCodeStatus);
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
