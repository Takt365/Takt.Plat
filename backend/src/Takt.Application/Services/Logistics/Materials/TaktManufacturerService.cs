// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktManufacturerService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：制造商信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 制造商信息应用服务
/// </summary>
public class TaktManufacturerService : TaktServiceBase, ITaktManufacturerService
{
    private readonly ITaktCompanyRepository<TaktManufacturer> _manufacturerRepository;
    private readonly ITaktCompanyRepository<TaktManufacturerMaterial> _manufacturerMaterialRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="manufacturerRepository">制造商信息仓储</param>
    /// <param name="manufacturerMaterialRepository">ManufacturerMaterial仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktManufacturerService(
        ITaktCompanyRepository<TaktManufacturer> manufacturerRepository,
        ITaktCompanyRepository<TaktManufacturerMaterial> manufacturerMaterialRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _manufacturerRepository = manufacturerRepository;
        _manufacturerMaterialRepository = manufacturerMaterialRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取制造商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktManufacturerDto>> GetManufacturerListAsync(TaktManufacturerQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _manufacturerRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktManufacturerDto>.Create(
            data.Adapt<List<TaktManufacturerDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktManufacturerDto?> GetManufacturerByIdAsync(long id)
    {
        var entity = await _manufacturerRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktManufacturerDto>();
        await FillManufacturerDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取制造商信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetManufacturerOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _manufacturerRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ManufacturerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ManufacturerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建制造商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktManufacturerDto> CreateManufacturerAsync(TaktManufacturerCreateDto dto)
    {
        var entity = dto.Adapt<TaktManufacturer>();
        var isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique = await _uniqueValidator.IsUniqueAsync(
            _manufacturerRepository,
            x => x.ManufacturerCode == entity.ManufacturerCode);
        if (!isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique)
        {
            throw new TaktBusinessException("制造商信息的ManufacturerCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _manufacturerRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _manufacturerRepository.CreateAsync(entity);
                await SaveManufacturerChildrenAsync(entity, dto);
        return await GetManufacturerByIdAsync(entity.Id) ?? entity.Adapt<TaktManufacturerDto>();
    }

    /// <summary>
    /// 更新制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktManufacturerDto> UpdateManufacturerAsync(long id, TaktManufacturerUpdateDto dto)
    {
        var entity = await _manufacturerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("制造商信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique = await _uniqueValidator.IsUniqueAsync(
            _manufacturerRepository,
            x => x.ManufacturerCode == entity.ManufacturerCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique)
        {
            throw new TaktBusinessException("制造商信息的ManufacturerCode已存在");
        }
        await _manufacturerRepository.UpdateAsync(entity);
                await SaveManufacturerChildrenAsync(entity, dto);
        return await GetManufacturerByIdAsync(id) ?? throw new TaktBusinessException("制造商信息不存在");
    }

    /// <summary>
    /// 删除制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteManufacturerByIdAsync(long id)
    {
        var entity = await _manufacturerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("制造商信息不存在或已删除");
        }
        await _manufacturerMaterialRepository.DeleteAsync(x => x.ManufacturerId == entity.Id);
        var deleted = await _manufacturerRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("制造商信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除制造商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteManufacturerBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteManufacturerByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新制造商信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktManufacturerDto> UpdateManufacturerStatusAsync(TaktManufacturerStatusDto dto)
    {
        var entity = await _manufacturerRepository.GetByIdAsync(dto.ManufacturerId);
        if (entity == null)
        {
            throw new TaktBusinessException("制造商信息不存在");
        }
        entity.ManufacturerStatus = dto.ManufacturerStatus;
        await _manufacturerRepository.UpdateAsync(entity);
        return await GetManufacturerByIdAsync(dto.ManufacturerId) ?? throw new TaktBusinessException("制造商信息不存在");
    }

    /// <summary>
    /// 更新制造商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktManufacturerDto> UpdateManufacturerSortAsync(TaktManufacturerSortDto dto)
    {
        var entity = await _manufacturerRepository.GetByIdAsync(dto.ManufacturerId);
        if (entity == null)
        {
            throw new TaktBusinessException("制造商信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _manufacturerRepository.UpdateAsync(entity);
        return await GetManufacturerByIdAsync(dto.ManufacturerId) ?? throw new TaktBusinessException("制造商信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetManufacturerTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktManufacturerTemplateDto>(
            sheetName ?? "制造商信息导入模板",
            fileName ?? "制造商信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入制造商信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportManufacturerAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktManufacturerImportDto>(fileStream, sheetName ?? "制造商信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _manufacturerRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktManufacturer>();
                var importKey = $"{entity.ManufacturerCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ManufacturerCode）");
                }
                var isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _manufacturerRepository,
                    x => x.ManufacturerCode == entity.ManufacturerCode);
                if (!isUnique_ix_takt_logistics_materials_manufacturer_manufacturer_code_unique)
                {
                    throw new TaktBusinessException("制造商信息的ManufacturerCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _manufacturerRepository.CreateAsync(entity);
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
    /// 导出制造商信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportManufacturerAsync(TaktManufacturerQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktManufacturerQueryDto());
        var list = await _manufacturerRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktManufacturerExportDto>(),
                sheetName ?? "制造商信息数据",
                fileName ?? "制造商信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktManufacturerExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "制造商信息数据",
            fileName ?? "制造商信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充制造商信息详情（加载 OneToMany 子表：制造商物料明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillManufacturerDetailsAsync(TaktManufacturerDto dto, TaktManufacturer entity)
    {
        if (dto == null)
        {
            return;
        }
        // 制造商物料明细 → dto.ManufacturerMaterials
        var manufacturermaterials = await _manufacturerMaterialRepository.GetListAsync(x => x.ManufacturerId == entity.Id);
        dto.ManufacturerMaterials = manufacturermaterials.Adapt<List<TaktManufacturerMaterialDto>>();
    }

    /// <summary>
    /// 保存制造商信息子表级联（制造商物料明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveManufacturerChildrenAsync(TaktManufacturer entity, TaktManufacturerCreateDto dto)
    {
        // 制造商物料明细（ManufacturerMaterials）
        if (dto.ManufacturerMaterials is not { Count: > 0 })
        {
            await _manufacturerMaterialRepository.DeleteAsync(x => x.ManufacturerId == entity.Id);
        }
        else
        {
            var manufacturermaterials = dto.ManufacturerMaterials.Adapt<List<TaktManufacturerMaterial>>();
            foreach (var child in manufacturermaterials)
            {
                child.ManufacturerId = entity.Id;
            }
            var manufacturermaterialsNeedLine = manufacturermaterials.Where(c => c.LineNumber <= 0).ToList();
            if (manufacturermaterialsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.ManufacturerCode) ? entity.ManufacturerCode : entity.Id.ToString();
                var maxLine = await _manufacturerMaterialRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ManufacturerId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, manufacturermaterialsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in manufacturermaterials)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < manufacturermaterials.Count; i++)
                        {
                            var key = $"{manufacturermaterials[i].CompanyCode}|{manufacturermaterials[i].ManufacturerId}|{manufacturermaterials[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"制造商物料明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ManufacturerId、LineNumber）");
                            }
                        }
            await _manufacturerMaterialRepository.DeleteAsync(x => x.ManufacturerId == entity.Id);
            foreach (var child in manufacturermaterials)
            {
            var isUnique_ix_takt_logistics_materials_manufacturer_material_line_unique = await _uniqueValidator.IsUniqueAsync(
                _manufacturerMaterialRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ManufacturerId == child.ManufacturerId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_materials_manufacturer_material_line_unique)
            {
                throw new TaktBusinessException("制造商物料明细的CompanyCode、ManufacturerId、LineNumber已存在");
            }
            var isUnique_ix_takt_logistics_materials_manufacturer_material_unique = await _uniqueValidator.IsUniqueAsync(
                _manufacturerMaterialRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ManufacturerId == child.ManufacturerId
                    && x.ManufacturerMaterialCode == child.ManufacturerMaterialCode);
            if (!isUnique_ix_takt_logistics_materials_manufacturer_material_unique)
            {
                throw new TaktBusinessException("制造商物料明细的CompanyCode、ManufacturerId、ManufacturerMaterialCode已存在");
            }
            }
            await _manufacturerMaterialRepository.CreateRangeAsync(manufacturermaterials);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建制造商信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktManufacturer, bool>> QueryExpression(TaktManufacturerQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktManufacturer>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ManufacturerCode != null && x.ManufacturerCode.Contains(keywords))
                || (x.ManufacturerName != null && x.ManufacturerName.Contains(keywords))
                || (x.ManufacturerShortName != null && x.ManufacturerShortName.Contains(keywords))
                || SqlFunc.ToString(x.ManufacturerType).Contains(keywords)
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.ManufacturerTaxNumber != null && x.ManufacturerTaxNumber.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.ManufacturerPhone != null && x.ManufacturerPhone.Contains(keywords))
                || (x.ManufacturerFax != null && x.ManufacturerFax.Contains(keywords))
                || (x.ManufacturerEmail != null && x.ManufacturerEmail.Contains(keywords))
                || (x.ManufacturerWebsite != null && x.ManufacturerWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || SqlFunc.ToString(x.ManufacturerLevel).Contains(keywords)
                || SqlFunc.ToString(x.QualityCertification).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationScore).Contains(keywords)
                || SqlFunc.ToString(x.IsQualified).Contains(keywords)
                || SqlFunc.ToString(x.ManufacturerStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerCode))
        {
            exp = exp.And(x => x.ManufacturerCode != null && x.ManufacturerCode.Contains(queryDto.ManufacturerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerName))
        {
            exp = exp.And(x => x.ManufacturerName != null && x.ManufacturerName.Contains(queryDto.ManufacturerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerShortName))
        {
            exp = exp.And(x => x.ManufacturerShortName != null && x.ManufacturerShortName.Contains(queryDto.ManufacturerShortName));
        }

        if (queryDto?.ManufacturerType.HasValue == true)
        {
            exp = exp.And(x => x.ManufacturerType == queryDto.ManufacturerType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerTaxNumber))
        {
            exp = exp.And(x => x.ManufacturerTaxNumber != null && x.ManufacturerTaxNumber.Contains(queryDto.ManufacturerTaxNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationCountry))
        {
            exp = exp.And(x => x.RegistrationCountry != null && x.RegistrationCountry.Contains(queryDto.RegistrationCountry));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress1))
        {
            exp = exp.And(x => x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(queryDto.RegistrationAddress1));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress2))
        {
            exp = exp.And(x => x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(queryDto.RegistrationAddress2));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress3))
        {
            exp = exp.And(x => x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(queryDto.RegistrationAddress3));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerPhone))
        {
            exp = exp.And(x => x.ManufacturerPhone != null && x.ManufacturerPhone.Contains(queryDto.ManufacturerPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerFax))
        {
            exp = exp.And(x => x.ManufacturerFax != null && x.ManufacturerFax.Contains(queryDto.ManufacturerFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerEmail))
        {
            exp = exp.And(x => x.ManufacturerEmail != null && x.ManufacturerEmail.Contains(queryDto.ManufacturerEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerWebsite))
        {
            exp = exp.And(x => x.ManufacturerWebsite != null && x.ManufacturerWebsite.Contains(queryDto.ManufacturerWebsite));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPerson))
        {
            exp = exp.And(x => x.ContactPerson != null && x.ContactPerson.Contains(queryDto.ContactPerson));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (queryDto?.ManufacturerLevel.HasValue == true)
        {
            exp = exp.And(x => x.ManufacturerLevel == queryDto.ManufacturerLevel);
        }

        if (queryDto?.QualityCertification.HasValue == true)
        {
            exp = exp.And(x => x.QualityCertification == queryDto.QualityCertification);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationScore == queryDto.EvaluationScore);
        }

        if (queryDto?.IsQualified.HasValue == true)
        {
            exp = exp.And(x => x.IsQualified == queryDto.IsQualified);
        }

        if (queryDto?.ManufacturerStatus.HasValue == true)
        {
            exp = exp.And(x => x.ManufacturerStatus == queryDto.ManufacturerStatus);
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
