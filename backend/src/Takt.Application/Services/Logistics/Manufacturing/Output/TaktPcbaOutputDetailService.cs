// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报明细应用服务实现
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
/// PCBA日报明细应用服务
/// </summary>
public class TaktPcbaOutputDetailService : TaktServiceBase, ITaktPcbaOutputDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaOutput> _pcbaOutputRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputDetailRepository">PCBA日报明细仓储</param>
    /// <param name="pcbaOutputRepository">PCBA日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaOutputDetailService(
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
        _pcbaOutputRepository = pcbaOutputRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA日报明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaOutputDetailDto>> GetPcbaOutputDetailListAsync(TaktPcbaOutputDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaOutputDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaOutputDetailDto>.Create(
            data.Adapt<List<TaktPcbaOutputDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto?> GetPcbaOutputDetailByIdAsync(long id)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaOutputDetailDto>();
    }

    /// <summary>
    /// 获取PCBA日报明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaOutputDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _pcbaOutputDetailRepository.GetListAsync(
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
    /// 创建PCBA日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> CreatePcbaOutputDetailAsync(TaktPcbaOutputDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaOutputDetail>();
        await StampPcbaOutputDetailPcbaOutputAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputDetailRepository,
            x => x.PcbaOutputId == entity.PcbaOutputId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaOutputDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaOutputId == entity.PcbaOutputId,
                x => x.LineNumber);
            var businessCode = entity.PcbaOutputId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _pcbaOutputDetailRepository.CreateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaOutputDetailDto>();
    }

    /// <summary>
    /// 更新PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> UpdatePcbaOutputDetailAsync(long id, TaktPcbaOutputDetailUpdateDto dto)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        dto.Adapt(entity);
        await StampPcbaOutputDetailPcbaOutputAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputDetailRepository,
            x => x.PcbaOutputId == entity.PcbaOutputId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、LineNumber已存在");
        }
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA日报明细不存在");
    }

    /// <summary>
    /// 删除PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputDetailByIdAsync(long id)
    {
        var deleted = await _pcbaOutputDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA日报明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaOutputDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新PCBA日报明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> UpdatePcbaOutputDetailStatusAsync(TaktPcbaOutputDetailStatusDto dto)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(dto.PcbaOutputDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        entity.CompletedStatus = dto.CompletedStatus;
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(dto.PcbaOutputDetailId) ?? throw new TaktBusinessException("PCBA日报明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaOutputDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaOutputDetailTemplateDto>(
            sheetName ?? "PCBA日报明细导入模板",
            fileName ?? "PCBA日报明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA日报明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaOutputDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaOutputDetailImportDto>(fileStream, sheetName ?? "PCBA日报明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaOutputDetail>();
                var importDto = rows[i].Adapt<TaktPcbaOutputDetailCreateDto>();
                await StampPcbaOutputDetailPcbaOutputAsync(entity, importDto);
                var importKey = $"{entity.PcbaOutputId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaOutputId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaOutputDetailRepository,
                    x => x.PcbaOutputId == entity.PcbaOutputId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique)
                {
                    throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaOutputDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaOutputId == entity.PcbaOutputId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaOutputId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _pcbaOutputDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA日报明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaOutputDetailAsync(TaktPcbaOutputDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaOutputDetailQueryDto());
        var list = await _pcbaOutputDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputDetailExportDto>(),
                sheetName ?? "PCBA日报明细数据",
                fileName ?? "PCBA日报明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaOutputDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA日报明细数据",
            fileName ?? "PCBA日报明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步PCBA日报明细主表外键（ManyToOne → PCBA日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaOutputDetailPcbaOutputAsync(TaktPcbaOutputDetail entity, TaktPcbaOutputDetailCreateDto dto)
    {
        if (dto.PcbaOutputId <= 0)
        {
            return;
        }
        var master = await _pcbaOutputRepository.GetByIdAsync(dto.PcbaOutputId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA日报不存在");
        }
        entity.PcbaOutputId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA日报明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaOutputDetail, bool>> QueryExpression(TaktPcbaOutputDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaOutputDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.PcbaOutputId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.TimePeriod != null && x.TimePeriod.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || (x.PcbBoardType != null && x.PcbBoardType.Contains(keywords))
                || (x.PanelSide != null && x.PanelSide.Contains(keywords))
                || SqlFunc.ToString(x.BatchQty).Contains(keywords)
                || SqlFunc.ToString(x.DailyCompletedQty).Contains(keywords)
                || SqlFunc.ToString(x.TotalCompletedQty).Contains(keywords)
                || SqlFunc.ToString(x.CompletedStatus).Contains(keywords)
                || (x.SerialNo != null && x.SerialNo.Contains(keywords))
                || SqlFunc.ToString(x.DefectCount).Contains(keywords)
                || SqlFunc.ToString(x.InputMinutes).Contains(keywords)
                || SqlFunc.ToString(x.RepairMinutes).Contains(keywords)
                || SqlFunc.ToString(x.SwitchCount).Contains(keywords)
                || SqlFunc.ToString(x.SwitchTime).Contains(keywords)
                || SqlFunc.ToString(x.StopTime).Contains(keywords)
                || SqlFunc.ToString(x.TotalMinutes).Contains(keywords)
                || (x.UnachievedReason != null && x.UnachievedReason.Contains(keywords))
                || (x.UnachievedDescription != null && x.UnachievedDescription.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.PcbaOutputId.HasValue == true)
        {
            exp = exp.And(x => x.PcbaOutputId == queryDto.PcbaOutputId);
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

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.PcbBoardType))
        {
            exp = exp.And(x => x.PcbBoardType != null && x.PcbBoardType.Contains(queryDto.PcbBoardType));
        }

        if (!string.IsNullOrEmpty(queryDto?.PanelSide))
        {
            exp = exp.And(x => x.PanelSide != null && x.PanelSide.Contains(queryDto.PanelSide));
        }

        if (queryDto?.BatchQty.HasValue == true)
        {
            exp = exp.And(x => x.BatchQty == queryDto.BatchQty);
        }

        if (queryDto?.DailyCompletedQty.HasValue == true)
        {
            exp = exp.And(x => x.DailyCompletedQty == queryDto.DailyCompletedQty);
        }

        if (queryDto?.TotalCompletedQty.HasValue == true)
        {
            exp = exp.And(x => x.TotalCompletedQty == queryDto.TotalCompletedQty);
        }

        if (queryDto?.CompletedStatus.HasValue == true)
        {
            exp = exp.And(x => x.CompletedStatus == queryDto.CompletedStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNo))
        {
            exp = exp.And(x => x.SerialNo != null && x.SerialNo.Contains(queryDto.SerialNo));
        }

        if (queryDto?.DefectCount.HasValue == true)
        {
            exp = exp.And(x => x.DefectCount == queryDto.DefectCount);
        }

        if (queryDto?.InputMinutes.HasValue == true)
        {
            exp = exp.And(x => x.InputMinutes == queryDto.InputMinutes);
        }

        if (queryDto?.RepairMinutes.HasValue == true)
        {
            exp = exp.And(x => x.RepairMinutes == queryDto.RepairMinutes);
        }

        if (queryDto?.SwitchCount.HasValue == true)
        {
            exp = exp.And(x => x.SwitchCount == queryDto.SwitchCount);
        }

        if (queryDto?.SwitchTime.HasValue == true)
        {
            exp = exp.And(x => x.SwitchTime == queryDto.SwitchTime);
        }

        if (queryDto?.StopTime.HasValue == true)
        {
            exp = exp.And(x => x.StopTime == queryDto.StopTime);
        }

        if (queryDto?.TotalMinutes.HasValue == true)
        {
            exp = exp.And(x => x.TotalMinutes == queryDto.TotalMinutes);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnachievedReason))
        {
            exp = exp.And(x => x.UnachievedReason != null && x.UnachievedReason.Contains(queryDto.UnachievedReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.UnachievedDescription))
        {
            exp = exp.And(x => x.UnachievedDescription != null && x.UnachievedDescription.Contains(queryDto.UnachievedDescription));
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
