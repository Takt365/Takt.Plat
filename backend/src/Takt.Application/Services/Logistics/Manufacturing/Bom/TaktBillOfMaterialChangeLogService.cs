// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialChangeLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM变更记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM变更记录应用服务
/// </summary>
public class TaktBillOfMaterialChangeLogService : TaktServiceBase, ITaktBillOfMaterialChangeLogService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterialChangeLog> _billOfMaterialChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _billOfMaterialRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialChangeLogRepository">BOM变更记录仓储</param>
    /// <param name="billOfMaterialRepository">物料清单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialChangeLogService(
        ITaktCompanyRepository<TaktBillOfMaterialChangeLog> billOfMaterialChangeLogRepository,
        ITaktCompanyRepository<TaktBillOfMaterial> billOfMaterialRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialChangeLogRepository = billOfMaterialChangeLogRepository;
        _billOfMaterialRepository = billOfMaterialRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialChangeLogDto>> GetBillOfMaterialChangeLogListAsync(TaktBillOfMaterialChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialChangeLogDto>.Create(
            data.Adapt<List<TaktBillOfMaterialChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialChangeLogDto?> GetBillOfMaterialChangeLogByIdAsync(long id)
    {
        var entity = await _billOfMaterialChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBillOfMaterialChangeLogDto>();
    }

    /// <summary>
    /// 获取BOM变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.BomCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.BomCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建BOM变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialChangeLogDto> CreateBillOfMaterialChangeLogAsync(TaktBillOfMaterialChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterialChangeLog>();
                await StampBillOfMaterialChangeLogBillOfMaterialAsync(entity, dto);
        entity = await _billOfMaterialChangeLogRepository.CreateAsync(entity);
        return await GetBillOfMaterialChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialChangeLogDto>();
    }

    /// <summary>
    /// 更新BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialChangeLogDto> UpdateBillOfMaterialChangeLogAsync(long id, TaktBillOfMaterialChangeLogUpdateDto dto)
    {
        var entity = await _billOfMaterialChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM变更记录不存在");
        }
        dto.Adapt(entity);
                await StampBillOfMaterialChangeLogBillOfMaterialAsync(entity, dto);
        await _billOfMaterialChangeLogRepository.UpdateAsync(entity);
        return await GetBillOfMaterialChangeLogByIdAsync(id) ?? throw new TaktBusinessException("BOM变更记录不存在");
    }

    /// <summary>
    /// 删除BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialChangeLogByIdAsync(long id)
    {
        var deleted = await _billOfMaterialChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("BOM变更记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除BOM变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出BOM变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialChangeLogAsync(TaktBillOfMaterialChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBillOfMaterialChangeLogQueryDto());
        var list = await _billOfMaterialChangeLogRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialChangeLogExportDto>(),
                sheetName ?? "BOM变更记录数据",
                fileName ?? "BOM变更记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM变更记录数据",
            fileName ?? "BOM变更记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步BOM变更记录主表外键（ManyToOne → 物料清单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialChangeLogBillOfMaterialAsync(TaktBillOfMaterialChangeLog entity, TaktBillOfMaterialChangeLogCreateDto dto)
    {
        if (dto.BillOfMaterialId <= 0)
        {
            return;
        }
        var master = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (master == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.BillOfMaterialId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM变更记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterialChangeLog, bool>> QueryExpression(TaktBillOfMaterialChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterialChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.BillOfMaterialId).Contains(keywords)
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeBy != null && x.ChangeBy.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ChangeTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.BillOfMaterialId.HasValue == true)
        {
            exp = exp.And(x => x.BillOfMaterialId == queryDto.BillOfMaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.BomCode))
        {
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(queryDto.BomCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeBy))
        {
            exp = exp.And(x => x.ChangeBy != null && x.ChangeBy.Contains(queryDto.ChangeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ChangeTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime >= queryDto.ChangeTimeStart);
        }

        if (queryDto?.ChangeTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ChangeTime <= queryDto.ChangeTimeEnd);
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
