// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktOvertimeService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：加班信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.HumanResource.Attendance;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 加班信息应用服务
/// </summary>
public class TaktOvertimeService : TaktServiceBase, ITaktOvertimeService
{
    private readonly ITaktApprovalRepository<TaktOvertime> _overtimeRepository;
    private readonly ITaktCompanyRepository<TaktOvertimeItem> _overtimeItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overtimeRepository">加班信息仓储</param>
    /// <param name="overtimeItemRepository">OvertimeItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOvertimeService(
        ITaktApprovalRepository<TaktOvertime> overtimeRepository,
        ITaktCompanyRepository<TaktOvertimeItem> overtimeItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _overtimeRepository = overtimeRepository;
        _overtimeItemRepository = overtimeItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取加班信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOvertimeDto>> GetOvertimeListAsync(TaktOvertimeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _overtimeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktOvertimeDto>.Create(
            data.Adapt<List<TaktOvertimeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto?> GetOvertimeByIdAsync(long id)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktOvertimeDto>();
        await FillOvertimeDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取加班信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOvertimeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _overtimeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建加班信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> CreateOvertimeAsync(TaktOvertimeCreateDto dto)
    {
        var entity = dto.Adapt<TaktOvertime>();
        var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeRepository,
            x => x.DeptId == entity.DeptId
                && x.OvertimeDate == entity.OvertimeDate);
        if (!isUnique_ix_overtime_dept_date_unique)
        {
            throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
        }
        entity = await _overtimeRepository.CreateAsync(entity);
                await SaveOvertimeChildrenAsync(entity, dto);
        return await GetOvertimeByIdAsync(entity.Id) ?? entity.Adapt<TaktOvertimeDto>();
    }

    /// <summary>
    /// 更新加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> UpdateOvertimeAsync(long id, TaktOvertimeUpdateDto dto)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
            _overtimeRepository,
            x => x.DeptId == entity.DeptId
                && x.OvertimeDate == entity.OvertimeDate,
            id);
        if (!isUnique_ix_overtime_dept_date_unique)
        {
            throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
        }
        await _overtimeRepository.UpdateAsync(entity);
                await SaveOvertimeChildrenAsync(entity, dto);
        return await GetOvertimeByIdAsync(id) ?? throw new TaktBusinessException("加班信息不存在");
    }

    /// <summary>
    /// 删除加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeByIdAsync(long id)
    {
        var entity = await _overtimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在或已删除");
        }
        await _overtimeItemRepository.DeleteAsync(x => x.OvertimeId == entity.Id);
        var deleted = await _overtimeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("加班信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除加班信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOvertimeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOvertimeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新加班信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOvertimeDto> UpdateOvertimeStatusAsync(TaktOvertimeStatusDto dto)
    {
        var entity = await _overtimeRepository.GetByIdAsync(dto.OvertimeId);
        if (entity == null)
        {
            throw new TaktBusinessException("加班信息不存在");
        }
        entity.OvertimeStatus = dto.OvertimeStatus;
        await _overtimeRepository.UpdateAsync(entity);
        return await GetOvertimeByIdAsync(dto.OvertimeId) ?? throw new TaktBusinessException("加班信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetOvertimeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktOvertimeTemplateDto>(
            sheetName ?? "加班信息导入模板",
            fileName ?? "加班信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入加班信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportOvertimeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktOvertimeImportDto>(fileStream, sheetName ?? "加班信息导入模板");
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
                var entity = rows[i].Adapt<TaktOvertime>();
                var importKey = $"{entity.DeptId}|{entity.OvertimeDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DeptId、OvertimeDate）");
                }
                var isUnique_ix_overtime_dept_date_unique = await _uniqueValidator.IsUniqueAsync(
                    _overtimeRepository,
                    x => x.DeptId == entity.DeptId
                        && x.OvertimeDate == entity.OvertimeDate);
                if (!isUnique_ix_overtime_dept_date_unique)
                {
                    throw new TaktBusinessException("加班信息的DeptId、OvertimeDate已存在");
                }
                await _overtimeRepository.CreateAsync(entity);
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
    /// 导出加班信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOvertimeAsync(TaktOvertimeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktOvertimeQueryDto());
        var list = await _overtimeRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOvertimeExportDto>(),
                sheetName ?? "加班信息数据",
                fileName ?? "加班信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOvertimeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "加班信息数据",
            fileName ?? "加班信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充加班信息详情（加载 OneToMany 子表：加班明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillOvertimeDetailsAsync(TaktOvertimeDto dto, TaktOvertime entity)
    {
        if (dto == null)
        {
            return;
        }
        // 加班明细 → dto.Items
        var items = await _overtimeItemRepository.GetListAsync(x => x.OvertimeId == entity.Id);
        dto.Items = items.Adapt<List<TaktOvertimeItemDto>>();
    }

    /// <summary>
    /// 保存加班信息子表级联（加班明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveOvertimeChildrenAsync(TaktOvertime entity, TaktOvertimeCreateDto dto)
    {
        // 加班明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _overtimeItemRepository.DeleteAsync(x => x.OvertimeId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktOvertimeItem>>();
            foreach (var child in items)
            {
                child.OvertimeId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _overtimeItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OvertimeId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].OvertimeId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"加班明细第{i + 1}项与本次提交的其他项重复（CompanyCode、OvertimeId、LineNumber）");
                            }
                        }
            await _overtimeItemRepository.DeleteAsync(x => x.OvertimeId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_overtime_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                _overtimeItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.OvertimeId == child.OvertimeId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_overtime_item_request_line_unique)
            {
                throw new TaktBusinessException("加班明细的CompanyCode、OvertimeId、LineNumber已存在");
            }
            }
            await _overtimeItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建加班信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOvertime, bool>> QueryExpression(TaktOvertimeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOvertime>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.TotalEmployees).Contains(keywords)
                || SqlFunc.ToString(x.TotalPlannedHours).Contains(keywords)
                || SqlFunc.ToString(x.TotalActualHours).Contains(keywords)
                || SqlFunc.ToString(x.OvertimeType).Contains(keywords)
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || SqlFunc.ToString(x.HandlingBy).Contains(keywords)
                || (x.HandlingComment != null && x.HandlingComment.Contains(keywords))
                || SqlFunc.ToString(x.OvertimeStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OvertimeDate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.HandlingAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.TotalEmployees.HasValue == true)
        {
            exp = exp.And(x => x.TotalEmployees == queryDto.TotalEmployees);
        }

        if (queryDto?.TotalPlannedHours.HasValue == true)
        {
            exp = exp.And(x => x.TotalPlannedHours == queryDto.TotalPlannedHours);
        }

        if (queryDto?.TotalActualHours.HasValue == true)
        {
            exp = exp.And(x => x.TotalActualHours == queryDto.TotalActualHours);
        }

        if (queryDto?.OvertimeType.HasValue == true)
        {
            exp = exp.And(x => x.OvertimeType == queryDto.OvertimeType);
        }

        if (!string.IsNullOrEmpty(queryDto?.Reason))
        {
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(queryDto.Reason));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (queryDto?.HandlingBy.HasValue == true)
        {
            exp = exp.And(x => x.HandlingBy == queryDto.HandlingBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingComment))
        {
            exp = exp.And(x => x.HandlingComment != null && x.HandlingComment.Contains(queryDto.HandlingComment));
        }

        if (queryDto?.OvertimeStatus.HasValue == true)
        {
            exp = exp.And(x => x.OvertimeStatus == queryDto.OvertimeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.OvertimeDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OvertimeDate >= queryDto.OvertimeDateStart);
        }

        if (queryDto?.OvertimeDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OvertimeDate <= queryDto.OvertimeDateEnd);
        }

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime >= queryDto.PlannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime <= queryDto.PlannedEndTimeEnd);
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
