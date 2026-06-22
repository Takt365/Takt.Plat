// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM变更记录控制器
/// 提供BOM变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM变更记录")]
public class TaktBillOfMaterialChangeLogsController : TaktControllerBase
{
    private readonly ITaktBillOfMaterialChangeLogService _billOfMaterialChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialChangeLogService">BOM变更记录服务</param>
    public TaktBillOfMaterialChangeLogsController(ITaktBillOfMaterialChangeLogService billOfMaterialChangeLogService)
    {
        _billOfMaterialChangeLogService = billOfMaterialChangeLogService;
    }

    /// <summary>
    /// 获取BOM变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:list", "BOM变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBillOfMaterialChangeLogListAsync([FromQuery] TaktBillOfMaterialChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _billOfMaterialChangeLogService.GetBillOfMaterialChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <returns>BOM变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:query", "BOM变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillOfMaterialChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _billOfMaterialChangeLogService.GetBillOfMaterialChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("BOM变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取BOM变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:query", "BOM变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBillOfMaterialChangeLogOptionsAsync()
    {
        try
        {
            var result = await _billOfMaterialChangeLogService.GetBillOfMaterialChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建BOM变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>BOM变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:create", "创建BOM变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateBillOfMaterialChangeLogAsync([FromBody] TaktBillOfMaterialChangeLogCreateDto dto)
    {
        try
        {
            var result = await _billOfMaterialChangeLogService.CreateBillOfMaterialChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>BOM变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:update", "更新BOM变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBillOfMaterialChangeLogAsync(long id, [FromBody] TaktBillOfMaterialChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _billOfMaterialChangeLogService.UpdateBillOfMaterialChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除BOM变更记录
    /// </summary>
    /// <param name="id">BOM变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:delete", "删除BOM变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBillOfMaterialChangeLogByIdAsync(long id)
    {
        try
        {
            await _billOfMaterialChangeLogService.DeleteBillOfMaterialChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除BOM变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:delete", "批量删除BOM变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBillOfMaterialChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _billOfMaterialChangeLogService.DeleteBillOfMaterialChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出BOM变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:change:log:export", "导出BOM变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBillOfMaterialChangeLogAsync([FromQuery] TaktBillOfMaterialChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _billOfMaterialChangeLogService.ExportBillOfMaterialChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
