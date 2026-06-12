// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktAccountTitleService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目应用服务实现
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
using Takt.Shared.Enums;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会计科目应用服务
/// </summary>
public class TaktAccountTitleService : TaktServiceBase, ITaktAccountTitleService
{
    private readonly ITaktCompanyRepository<TaktAccountTitle> _accountTitleRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="accountTitleRepository">会计科目仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAccountTitleService(
        ITaktCompanyRepository<TaktAccountTitle> accountTitleRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _accountTitleRepository = accountTitleRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会计科目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAccountTitleDto>> GetAccountTitleListAsync(TaktAccountTitleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _accountTitleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAccountTitleDto>.Create(
            data.Adapt<List<TaktAccountTitleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto?> GetAccountTitleByIdAsync(long id)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAccountTitleDto>();
    }

    /// <summary>
    /// 获取会计科目树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    public async Task<List<TaktTreeSelectOption>> GetAccountTitleTreeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _accountTitleRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TitleStatus == 1);
        return BuildAccountTitleTreeOptions(list, 0);
    }

    /// <summary>
    /// 在内存中构建会计科目树形选项（递归，按 ParentId）
    /// </summary>
    private List<TaktTreeSelectOption> BuildAccountTitleTreeOptions(List<TaktAccountTitle> all, long parentId)
    {
        var result = new List<TaktTreeSelectOption>();
        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder))
        {
            var option = new TaktTreeSelectOption
            {
                DictValue = item.Id,
                DictLabel = item.TitleName ?? item.Id.ToString(),
                SortOrder = item.SortOrder,
            };
            var children = BuildAccountTitleTreeOptions(all, item.Id);
            if (children.Count > 0)
            {
                option.Children = children;
            }
            result.Add(option);
        }
        return result;
    }

    /// <summary>
    /// 获取会计科目树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    public async Task<List<TaktAccountTitleTreeDto>> GetAccountTitleTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        var list = await _accountTitleRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        var filtered = includeDisabled
            ? list
            : list.Where(x => x.TitleStatus == 1).ToList();
        return BuildAccountTitleTree(filtered, parentId);
    }

    /// <summary>
    /// 在内存中构建会计科目树（递归，按 ParentId）
    /// </summary>
    private List<TaktAccountTitleTreeDto> BuildAccountTitleTree(List<TaktAccountTitle> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.SortOrder)
            .ToList();
        var treeList = new List<TaktAccountTitleTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktAccountTitleTreeDto>();
            var childTree = BuildAccountTitleTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }
            treeList.Add(treeDto);
        }
        return treeList;
    }

    /// <summary>
    /// 创建会计科目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> CreateAccountTitleAsync(TaktAccountTitleCreateDto dto)
    {
        var entity = dto.Adapt<TaktAccountTitle>();
        var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
            _accountTitleRepository,
            x => x.TitleCode == entity.TitleCode);
        if (!isUnique_ix_account_title_code_unique)
        {
            throw new TaktBusinessException("会计科目的TitleCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _accountTitleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _accountTitleRepository.CreateAsync(entity);
        return await GetAccountTitleByIdAsync(entity.Id) ?? entity.Adapt<TaktAccountTitleDto>();
    }

    /// <summary>
    /// 更新会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleAsync(long id, TaktAccountTitleUpdateDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
            _accountTitleRepository,
            x => x.TitleCode == entity.TitleCode,
            id);
        if (!isUnique_ix_account_title_code_unique)
        {
            throw new TaktBusinessException("会计科目的TitleCode已存在");
        }
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(id) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 删除会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleByIdAsync(long id)
    {

        var hasChildren = await _accountTitleRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _accountTitleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会计科目不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会计科目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAccountTitleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAccountTitleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会计科目状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleStatusAsync(TaktAccountTitleStatusDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(dto.AccountTitleId);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        entity.TitleStatus = dto.TitleStatus;
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(dto.AccountTitleId) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 更新会计科目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAccountTitleDto> UpdateAccountTitleSortAsync(TaktAccountTitleSortDto dto)
    {
        var entity = await _accountTitleRepository.GetByIdAsync(dto.AccountTitleId);
        if (entity == null)
        {
            throw new TaktBusinessException("会计科目不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _accountTitleRepository.UpdateAsync(entity);
        return await GetAccountTitleByIdAsync(dto.AccountTitleId) ?? throw new TaktBusinessException("会计科目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAccountTitleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAccountTitleTemplateDto>(
            sheetName ?? "会计科目导入模板",
            fileName ?? "会计科目导入模板.xlsx");
    }

    /// <summary>
    /// 导入会计科目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAccountTitleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAccountTitleImportDto>(fileStream, sheetName ?? "会计科目导入模板");
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
                var entity = rows[i].Adapt<TaktAccountTitle>();
                var importKey = $"{entity.TitleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TitleCode）");
                }
                var isUnique_ix_account_title_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _accountTitleRepository,
                    x => x.TitleCode == entity.TitleCode);
                if (!isUnique_ix_account_title_code_unique)
                {
                    throw new TaktBusinessException("会计科目的TitleCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _accountTitleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _accountTitleRepository.CreateAsync(entity);
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
    /// 导出会计科目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAccountTitleAsync(TaktAccountTitleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAccountTitleQueryDto());
        var list = await _accountTitleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAccountTitleExportDto>(),
                sheetName ?? "会计科目数据",
                fileName ?? "会计科目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAccountTitleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会计科目数据",
            fileName ?? "会计科目导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会计科目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAccountTitle, bool>> QueryExpression(TaktAccountTitleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAccountTitle>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TitleCode != null && x.TitleCode.Contains(keywords))
                || (x.TitleName != null && x.TitleName.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.TitleType).Contains(keywords)
                || SqlFunc.ToString(x.BalanceDirection).Contains(keywords)
                || SqlFunc.ToString(x.TitleLevel).Contains(keywords)
                || SqlFunc.ToString(x.IsLeaf).Contains(keywords)
                || SqlFunc.ToString(x.IsAuxiliary).Contains(keywords)
                || SqlFunc.ToString(x.AuxiliaryType).Contains(keywords)
                || SqlFunc.ToString(x.IsQuantity).Contains(keywords)
                || SqlFunc.ToString(x.IsCurrency).Contains(keywords)
                || SqlFunc.ToString(x.IsCash).Contains(keywords)
                || SqlFunc.ToString(x.IsBank).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.TitleStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TitleCode))
        {
            exp = exp.And(x => x.TitleCode != null && x.TitleCode.Contains(queryDto.TitleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TitleName))
        {
            exp = exp.And(x => x.TitleName != null && x.TitleName.Contains(queryDto.TitleName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShortName))
        {
            exp = exp.And(x => x.TitleName != null && x.TitleName.Contains(queryDto.ShortName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TitleDesc))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.TitleDesc));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.TitleType.HasValue == true)
        {
            exp = exp.And(x => x.TitleType == queryDto.TitleType);
        }

        if (queryDto?.BalanceDirection.HasValue == true)
        {
            exp = exp.And(x => x.BalanceDirection == queryDto.BalanceDirection);
        }

        if (queryDto?.TitleLevel.HasValue == true)
        {
            exp = exp.And(x => x.TitleLevel == queryDto.TitleLevel);
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            exp = exp.And(x => x.IsLeaf == queryDto.IsLeaf);
        }

        if (queryDto?.IsAuxiliary.HasValue == true)
        {
            exp = exp.And(x => x.IsAuxiliary == queryDto.IsAuxiliary);
        }

        if (queryDto?.AuxiliaryType.HasValue == true)
        {
            exp = exp.And(x => x.AuxiliaryType == queryDto.AuxiliaryType);
        }

        if (queryDto?.IsQuantity.HasValue == true)
        {
            exp = exp.And(x => x.IsQuantity == queryDto.IsQuantity);
        }

        if (queryDto?.IsCurrency.HasValue == true)
        {
            exp = exp.And(x => x.IsCurrency == queryDto.IsCurrency);
        }

        if (queryDto?.IsCash.HasValue == true)
        {
            exp = exp.And(x => x.IsCash == queryDto.IsCash);
        }

        if (queryDto?.IsBank.HasValue == true)
        {
            exp = exp.And(x => x.IsBank == queryDto.IsBank);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.TitleStatus.HasValue == true)
        {
            exp = exp.And(x => x.TitleStatus == queryDto.TitleStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
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
