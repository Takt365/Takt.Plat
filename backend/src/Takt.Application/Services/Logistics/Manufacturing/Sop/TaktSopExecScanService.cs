// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecScanService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP物料扫码记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP物料扫码记录应用服务
/// </summary>
public class TaktSopExecScanService : TaktServiceBase, ITaktSopExecScanService
{
    private readonly ITaktCompanyRepository<TaktSopExecScan> _sopExecScanRepository;
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecScanRepository">SOP物料扫码记录仓储</param>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopExecScanService(
        ITaktCompanyRepository<TaktSopExecScan> sopExecScanRepository,
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopExecScanRepository = sopExecScanRepository;
        _sopExecRepository = sopExecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP物料扫码记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopExecScanDto>> GetSopExecScanListAsync(TaktSopExecScanQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopExecScanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopExecScanDto>.Create(
            data.Adapt<List<TaktSopExecScanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecScanDto?> GetSopExecScanByIdAsync(long id)
    {
        var entity = await _sopExecScanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopExecScanDto>();
    }

    /// <summary>
    /// 获取SOP物料扫码记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopExecScanOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopExecScanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ExpectedMaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ExpectedMaterialCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP物料扫码记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecScanDto> CreateSopExecScanAsync(TaktSopExecScanCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopExecScan>();
        await StampSopExecScanSopExecAsync(entity, dto);
        entity = await _sopExecScanRepository.CreateAsync(entity);
        return await GetSopExecScanByIdAsync(entity.Id) ?? entity.Adapt<TaktSopExecScanDto>();
    }

    /// <summary>
    /// 更新SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecScanDto> UpdateSopExecScanAsync(long id, TaktSopExecScanUpdateDto dto)
    {
        var entity = await _sopExecScanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP物料扫码记录不存在");
        }
        dto.Adapt(entity);
        await StampSopExecScanSopExecAsync(entity, dto);
        await _sopExecScanRepository.UpdateAsync(entity);
        return await GetSopExecScanByIdAsync(id) ?? throw new TaktBusinessException("SOP物料扫码记录不存在");
    }

    /// <summary>
    /// 删除SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecScanByIdAsync(long id)
    {
        var deleted = await _sopExecScanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP物料扫码记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP物料扫码记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecScanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopExecScanByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopExecScanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopExecScanTemplateDto>(
            sheetName ?? "SOP物料扫码记录导入模板",
            fileName ?? "SOP物料扫码记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP物料扫码记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopExecScanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopExecScanImportDto>(fileStream, sheetName ?? "SOP物料扫码记录导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopExecScan>();
                var importDto = rows[i].Adapt<TaktSopExecScanCreateDto>();
                await StampSopExecScanSopExecAsync(entity, importDto);
                await _sopExecScanRepository.CreateAsync(entity);
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
    /// 导出SOP物料扫码记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopExecScanAsync(TaktSopExecScanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopExecScanQueryDto());
        var list = await _sopExecScanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecScanExportDto>(),
                sheetName ?? "SOP物料扫码记录数据",
                fileName ?? "SOP物料扫码记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopExecScanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP物料扫码记录数据",
            fileName ?? "SOP物料扫码记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP物料扫码记录主表外键（ManyToOne → SOP工位执行）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopExecScanSopExecAsync(TaktSopExecScan entity, TaktSopExecScanCreateDto dto)
    {
        if (dto.ExecId <= 0)
        {
            return;
        }
        var master = await _sopExecRepository.GetByIdAsync(dto.ExecId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP物料扫码记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopExecScan, bool>> QueryExpression(TaktSopExecScanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopExecScan>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ExecId).Contains(keywords)
                || SqlFunc.ToString(x.ExecStepId).Contains(keywords)
                || SqlFunc.ToString(x.StepId).Contains(keywords)
                || (x.ScannedBarcode != null && x.ScannedBarcode.Contains(keywords))
                || (x.ExpectedMaterialCode != null && x.ExpectedMaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.ScanResult).Contains(keywords)
                || (x.MatchMessage != null && x.MatchMessage.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScannedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ExecId.HasValue == true)
        {
            exp = exp.And(x => x.ExecId == queryDto.ExecId);
        }

        if (queryDto?.ExecStepId.HasValue == true)
        {
            exp = exp.And(x => x.ExecStepId == queryDto.ExecStepId);
        }

        if (queryDto?.StepId.HasValue == true)
        {
            exp = exp.And(x => x.StepId == queryDto.StepId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ScannedBarcode))
        {
            exp = exp.And(x => x.ScannedBarcode != null && x.ScannedBarcode.Contains(queryDto.ScannedBarcode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExpectedMaterialCode))
        {
            exp = exp.And(x => x.ExpectedMaterialCode != null && x.ExpectedMaterialCode.Contains(queryDto.ExpectedMaterialCode));
        }

        if (queryDto?.ScanResult.HasValue == true)
        {
            exp = exp.And(x => x.ScanResult == queryDto.ScanResult);
        }

        if (!string.IsNullOrEmpty(queryDto?.MatchMessage))
        {
            exp = exp.And(x => x.MatchMessage != null && x.MatchMessage.Contains(queryDto.MatchMessage));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ScannedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ScannedAt >= queryDto.ScannedAtStart);
        }

        if (queryDto?.ScannedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScannedAt <= queryDto.ScannedAtEnd);
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
