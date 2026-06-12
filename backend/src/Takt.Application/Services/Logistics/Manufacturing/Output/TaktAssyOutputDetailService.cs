// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报明细应用服务
/// </summary>
public class TaktAssyOutputDetailService : TaktServiceBase, ITaktAssyOutputDetailService
{
    private readonly ITaktCompanyRepository<TaktAssyOutputDetail> _assyOutputDetailRepository;
    private readonly ITaktCompanyRepository<TaktAssyOutput> _assyOutputRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyOutputDetailService(
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyOutputDetailRepository = assyOutputDetailRepository;
        _assyOutputRepository = assyOutputRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取组立日报明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssyOutputDetailDto>> GetAssyOutputDetailListAsync(TaktAssyOutputDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assyOutputDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssyOutputDetailDto>.Create(
            data.Adapt<List<TaktAssyOutputDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto?> GetAssyOutputDetailByIdAsync(long id)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssyOutputDetailDto>();
    }

    /// <summary>
    /// 获取组立日报明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssyOutputDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _assyOutputDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProdOrderCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建组立日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto> CreateAssyOutputDetailAsync(TaktAssyOutputDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssyOutputDetail>();
        await StampAssyOutputDetailAssyOutputAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputDetailRepository,
            x => x.AssyOutputId == entity.AssyOutputId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立日报明细的AssyOutputId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _assyOutputDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyOutputId == entity.AssyOutputId,
                x => x.LineNumber);
            var businessCode = entity.AssyOutputId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _assyOutputDetailRepository.CreateAsync(entity);
        return await GetAssyOutputDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktAssyOutputDetailDto>();
    }

    /// <summary>
    /// 更新组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssyOutputDetailDto> UpdateAssyOutputDetailAsync(long id, TaktAssyOutputDetailUpdateDto dto)
    {
        var entity = await _assyOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("组立日报明细不存在");
        }
        dto.Adapt(entity);
        await StampAssyOutputDetailAssyOutputAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _assyOutputDetailRepository,
            x => x.AssyOutputId == entity.AssyOutputId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique)
        {
            throw new TaktBusinessException("组立日报明细的AssyOutputId、LineNumber已存在");
        }
        await _assyOutputDetailRepository.UpdateAsync(entity);
        return await GetAssyOutputDetailByIdAsync(id) ?? throw new TaktBusinessException("组立日报明细不存在");
    }

    /// <summary>
    /// 删除组立日报明细
    /// </summary>
    /// <param name="id">组立日报明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputDetailByIdAsync(long id)
    {
        var deleted = await _assyOutputDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("组立日报明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除组立日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssyOutputDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssyOutputDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssyOutputDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssyOutputDetailTemplateDto>(
            sheetName ?? "组立日报明细导入模板",
            fileName ?? "组立日报明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入组立日报明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssyOutputDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssyOutputDetailImportDto>(fileStream, sheetName ?? "组立日报明细导入模板");
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
                var entity = rows[i].Adapt<TaktAssyOutputDetail>();
                var importDto = rows[i].Adapt<TaktAssyOutputDetailCreateDto>();
                await StampAssyOutputDetailAssyOutputAsync(entity, importDto);
                var importKey = $"{entity.AssyOutputId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssyOutputId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _assyOutputDetailRepository,
                    x => x.AssyOutputId == entity.AssyOutputId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_output_assy_detail_line_unique)
                {
                    throw new TaktBusinessException("组立日报明细的AssyOutputId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _assyOutputDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssyOutputId == entity.AssyOutputId,
                        x => x.LineNumber);
                    var businessCode = entity.AssyOutputId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _assyOutputDetailRepository.CreateAsync(entity);
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
    /// 导出组立日报明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssyOutputDetailAsync(TaktAssyOutputDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssyOutputDetailQueryDto());
        var list = await _assyOutputDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssyOutputDetailExportDto>(),
                sheetName ?? "组立日报明细数据",
                fileName ?? "组立日报明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssyOutputDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "组立日报明细数据",
            fileName ?? "组立日报明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步组立日报明细主表外键（ManyToOne → 组立日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampAssyOutputDetailAssyOutputAsync(TaktAssyOutputDetail entity, TaktAssyOutputDetailCreateDto dto)
    {
        if (dto.AssyOutputId <= 0)
        {
            return;
        }
        var master = await _assyOutputRepository.GetByIdAsync(dto.AssyOutputId);
        if (master == null)
        {
            throw new TaktBusinessException("组立日报不存在");
        }
        entity.AssyOutputId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建组立日报明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssyOutputDetail, bool>> QueryExpression(TaktAssyOutputDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssyOutputDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.AssyOutputId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.TimePeriod != null && x.TimePeriod.Contains(keywords))
                || SqlFunc.ToString(x.ProdActualQty).Contains(keywords)
                || SqlFunc.ToString(x.DowntimeMinutes).Contains(keywords)
                || (x.DowntimeReason != null && x.DowntimeReason.Contains(keywords))
                || (x.DowntimeDescription != null && x.DowntimeDescription.Contains(keywords))
                || (x.UnachievedReason != null && x.UnachievedReason.Contains(keywords))
                || (x.UnachievedDescription != null && x.UnachievedDescription.Contains(keywords))
                || SqlFunc.ToString(x.InputMinutes).Contains(keywords)
                || SqlFunc.ToString(x.ProdMinutes).Contains(keywords)
                || SqlFunc.ToString(x.ActualMinutes).Contains(keywords)
                || SqlFunc.ToString(x.AchievementRate).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.AssyOutputId.HasValue == true)
        {
            exp = exp.And(x => x.AssyOutputId == queryDto.AssyOutputId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.TimePeriod))
        {
            exp = exp.And(x => x.TimePeriod != null && x.TimePeriod.Contains(queryDto.TimePeriod));
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdActualQty == queryDto.ProdActualQty);
        }

        if (queryDto?.DowntimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.DowntimeMinutes == queryDto.DowntimeMinutes);
        }

        if (!string.IsNullOrEmpty(queryDto?.DowntimeReason))
        {
            exp = exp.And(x => x.DowntimeReason != null && x.DowntimeReason.Contains(queryDto.DowntimeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.DowntimeDescription))
        {
            exp = exp.And(x => x.DowntimeDescription != null && x.DowntimeDescription.Contains(queryDto.DowntimeDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.UnachievedReason))
        {
            exp = exp.And(x => x.UnachievedReason != null && x.UnachievedReason.Contains(queryDto.UnachievedReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.UnachievedDescription))
        {
            exp = exp.And(x => x.UnachievedDescription != null && x.UnachievedDescription.Contains(queryDto.UnachievedDescription));
        }

        if (queryDto?.InputMinutes.HasValue == true)
        {
            exp = exp.And(x => x.InputMinutes == queryDto.InputMinutes);
        }

        if (queryDto?.ProdMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ProdMinutes == queryDto.ProdMinutes);
        }

        if (queryDto?.ActualMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ActualMinutes == queryDto.ActualMinutes);
        }

        if (queryDto?.AchievementRate.HasValue == true)
        {
            exp = exp.And(x => x.AchievementRate == queryDto.AchievementRate);
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
