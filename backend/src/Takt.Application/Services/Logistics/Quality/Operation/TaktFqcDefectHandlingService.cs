// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktFqcDefectHandlingService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验不良处理记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 出货检验不良处理记录应用服务
/// </summary>
public class TaktFqcDefectHandlingService : TaktServiceBase, ITaktFqcDefectHandlingService
{
    private readonly ITaktCompanyRepository<TaktFqcDefectHandling> _fqcDefectHandlingRepository;
    private readonly ITaktCompanyRepository<TaktFqcOrderItem> _fqcOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcDefectHandlingRepository">出货检验不良处理记录仓储</param>
    /// <param name="fqcOrderItemRepository">出货检验单明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFqcDefectHandlingService(
        ITaktCompanyRepository<TaktFqcDefectHandling> fqcDefectHandlingRepository,
        ITaktCompanyRepository<TaktFqcOrderItem> fqcOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fqcDefectHandlingRepository = fqcDefectHandlingRepository;
        _fqcOrderItemRepository = fqcOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取出货检验不良处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFqcDefectHandlingDto>> GetFqcDefectHandlingListAsync(TaktFqcDefectHandlingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _fqcDefectHandlingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFqcDefectHandlingDto>.Create(
            data.Adapt<List<TaktFqcDefectHandlingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcDefectHandlingDto?> GetFqcDefectHandlingByIdAsync(long id)
    {
        var entity = await _fqcDefectHandlingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFqcDefectHandlingDto>();
    }

    /// <summary>
    /// 获取出货检验不良处理记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFqcDefectHandlingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _fqcDefectHandlingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.HandlingStatus == 1,
            x => x.FqcDefectHandlingCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FqcDefectHandlingCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建出货检验不良处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcDefectHandlingDto> CreateFqcDefectHandlingAsync(TaktFqcDefectHandlingCreateDto dto)
    {
        var entity = dto.Adapt<TaktFqcDefectHandling>();
        entity.IsObsolete = 0;
        await StampFqcDefectHandlingFqcOrderItemAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcDefectHandlingRepository,
            x => x.FqcOrderItemId == entity.FqcOrderItemId
                && x.DefectCode == entity.DefectCode
                && x.HandlingMethod == entity.HandlingMethod);
        if (!isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique)
        {
            throw new TaktBusinessException("出货检验不良处理记录的FqcOrderItemId、DefectCode、HandlingMethod已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _fqcDefectHandlingRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderItemId == entity.FqcOrderItemId,
                x => x.LineNumber);
            var businessCode = entity.FqcOrderItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _fqcDefectHandlingRepository.CreateAsync(entity);
        return await GetFqcDefectHandlingByIdAsync(entity.Id) ?? entity.Adapt<TaktFqcDefectHandlingDto>();
    }

    /// <summary>
    /// 更新出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcDefectHandlingDto> UpdateFqcDefectHandlingAsync(long id, TaktFqcDefectHandlingUpdateDto dto)
    {
        var entity = await _fqcDefectHandlingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在");
        }
        dto.Adapt(entity);
        await StampFqcDefectHandlingFqcOrderItemAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcDefectHandlingRepository,
            x => x.FqcOrderItemId == entity.FqcOrderItemId
                && x.DefectCode == entity.DefectCode
                && x.HandlingMethod == entity.HandlingMethod,
            id);
        if (!isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique)
        {
            throw new TaktBusinessException("出货检验不良处理记录的FqcOrderItemId、DefectCode、HandlingMethod已存在");
        }
        await _fqcDefectHandlingRepository.UpdateAsync(entity);
        return await GetFqcDefectHandlingByIdAsync(id) ?? throw new TaktBusinessException("出货检验不良处理记录不存在");
    }

    /// <summary>
    /// 删除出货检验不良处理记录
    /// </summary>
    /// <param name="id">出货检验不良处理记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcDefectHandlingByIdAsync(long id)
    {
        var entity = await _fqcDefectHandlingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("出货检验不良处理记录已作废");
        }
        entity.IsObsolete = 1;
        await _fqcDefectHandlingRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除出货检验不良处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcDefectHandlingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFqcDefectHandlingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新出货检验不良处理记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcDefectHandlingDto> UpdateFqcDefectHandlingStatusAsync(TaktFqcDefectHandlingStatusDto dto)
    {
        var entity = await _fqcDefectHandlingRepository.GetByIdAsync(dto.FqcDefectHandlingId);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在");
        }
        entity.HandlingStatus = dto.HandlingStatus;
        await _fqcDefectHandlingRepository.UpdateAsync(entity);
        return await GetFqcDefectHandlingByIdAsync(dto.FqcDefectHandlingId) ?? throw new TaktBusinessException("出货检验不良处理记录不存在");
    }

    /// <summary>
    /// 更新出货检验不良处理记录作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcDefectHandlingDto> UpdateFqcDefectHandlingObsoleteAsync(TaktFqcDefectHandlingObsoleteDto dto)
    {
        var entity = await _fqcDefectHandlingRepository.GetByIdAsync(dto.FqcDefectHandlingId);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("出货检验不良处理记录不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _fqcDefectHandlingRepository.UpdateAsync(entity);
        return await GetFqcDefectHandlingByIdAsync(dto.FqcDefectHandlingId) ?? throw new TaktBusinessException("出货检验不良处理记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFqcDefectHandlingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFqcDefectHandlingTemplateDto>(
            sheetName ?? "出货检验不良处理记录导入模板",
            fileName ?? "出货检验不良处理记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入出货检验不良处理记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFqcDefectHandlingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFqcDefectHandlingImportDto>(fileStream, sheetName ?? "出货检验不良处理记录导入模板");
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
                var entity = rows[i].Adapt<TaktFqcDefectHandling>();
                var importDto = rows[i].Adapt<TaktFqcDefectHandlingCreateDto>();
                await StampFqcDefectHandlingFqcOrderItemAsync(entity, importDto);
                var importKey = $"{entity.FqcOrderItemId}|{entity.DefectCode}|{entity.HandlingMethod}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FqcOrderItemId、DefectCode、HandlingMethod）");
                }
                var isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique = await _uniqueValidator.IsUniqueAsync(
                    _fqcDefectHandlingRepository,
                    x => x.FqcOrderItemId == entity.FqcOrderItemId
                        && x.DefectCode == entity.DefectCode
                        && x.HandlingMethod == entity.HandlingMethod);
                if (!isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique)
                {
                    throw new TaktBusinessException("出货检验不良处理记录的FqcOrderItemId、DefectCode、HandlingMethod已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _fqcDefectHandlingRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderItemId == entity.FqcOrderItemId,
                        x => x.LineNumber);
                    var businessCode = entity.FqcOrderItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _fqcDefectHandlingRepository.CreateAsync(entity);
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
    /// 导出出货检验不良处理记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFqcDefectHandlingAsync(TaktFqcDefectHandlingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFqcDefectHandlingQueryDto());
        var list = await _fqcDefectHandlingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFqcDefectHandlingExportDto>(),
                sheetName ?? "出货检验不良处理记录数据",
                fileName ?? "出货检验不良处理记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFqcDefectHandlingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "出货检验不良处理记录数据",
            fileName ?? "出货检验不良处理记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步出货检验不良处理记录主表外键（ManyToOne → 出货检验单明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampFqcDefectHandlingFqcOrderItemAsync(TaktFqcDefectHandling entity, TaktFqcDefectHandlingCreateDto dto)
    {
        if (dto.FqcOrderItemId <= 0)
        {
            return;
        }
        var master = await _fqcOrderItemRepository.GetByIdAsync(dto.FqcOrderItemId);
        if (master == null)
        {
            throw new TaktBusinessException("出货检验单明细不存在");
        }
        entity.FqcOrderItemId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建出货检验不良处理记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFqcDefectHandling, bool>> QueryExpression(TaktFqcDefectHandlingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFqcDefectHandling>();

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
                (x.FqcDefectHandlingCode != null && x.FqcDefectHandlingCode.Contains(keywords))
                || SqlFunc.ToString(x.FqcOrderItemId).Contains(keywords)
                || (x.FqcOrderCode != null && x.FqcOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.DefectType).Contains(keywords)
                || (x.DefectCode != null && x.DefectCode.Contains(keywords))
                || (x.DefectDescription != null && x.DefectDescription.Contains(keywords))
                || SqlFunc.ToString(x.DefectQuantity).Contains(keywords)
                || SqlFunc.ToString(x.HandlingMethod).Contains(keywords)
                || (x.HandlingDescription != null && x.HandlingDescription.Contains(keywords))
                || (x.ResponsibleDept != null && x.ResponsibleDept.Contains(keywords))
                || (x.ResponsibleBy != null && x.ResponsibleBy.Contains(keywords))
                || (x.HandlerBy != null && x.HandlerBy.Contains(keywords))
                || (x.CorrectiveAction != null && x.CorrectiveAction.Contains(keywords))
                || (x.DefectImages != null && x.DefectImages.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.HandlingStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.HandlingAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FqcDefectHandlingCode))
        {
            exp = exp.And(x => x.FqcDefectHandlingCode != null && x.FqcDefectHandlingCode.Contains(queryDto.FqcDefectHandlingCode));
        }

        if (queryDto?.FqcOrderItemId.HasValue == true)
        {
            exp = exp.And(x => x.FqcOrderItemId == queryDto.FqcOrderItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FqcOrderCode))
        {
            exp = exp.And(x => x.FqcOrderCode != null && x.FqcOrderCode.Contains(queryDto.FqcOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.DefectType.HasValue == true)
        {
            exp = exp.And(x => x.DefectType == queryDto.DefectType);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectCode))
        {
            exp = exp.And(x => x.DefectCode != null && x.DefectCode.Contains(queryDto.DefectCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectDescription))
        {
            exp = exp.And(x => x.DefectDescription != null && x.DefectDescription.Contains(queryDto.DefectDescription));
        }

        if (queryDto?.DefectQuantity.HasValue == true)
        {
            exp = exp.And(x => x.DefectQuantity == queryDto.DefectQuantity);
        }

        if (queryDto?.HandlingMethod.HasValue == true)
        {
            exp = exp.And(x => x.HandlingMethod == queryDto.HandlingMethod);
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingDescription))
        {
            exp = exp.And(x => x.HandlingDescription != null && x.HandlingDescription.Contains(queryDto.HandlingDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleDept))
        {
            exp = exp.And(x => x.ResponsibleDept != null && x.ResponsibleDept.Contains(queryDto.ResponsibleDept));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleBy))
        {
            exp = exp.And(x => x.ResponsibleBy != null && x.ResponsibleBy.Contains(queryDto.ResponsibleBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlerBy))
        {
            exp = exp.And(x => x.HandlerBy != null && x.HandlerBy.Contains(queryDto.HandlerBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.CorrectiveAction))
        {
            exp = exp.And(x => x.CorrectiveAction != null && x.CorrectiveAction.Contains(queryDto.CorrectiveAction));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectImages))
        {
            exp = exp.And(x => x.DefectImages != null && x.DefectImages.Contains(queryDto.DefectImages));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.HandlingStatus.HasValue == true)
        {
            exp = exp.And(x => x.HandlingStatus == queryDto.HandlingStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.HandlingAtStart.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt >= queryDto.HandlingAtStart);
        }

        if (queryDto?.HandlingAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.HandlingAt <= queryDto.HandlingAtEnd);
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
