// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesQuotationChangeLogsController.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售报价变更记录控制器
/// 提供销售报价变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售报价变更记录")]
public class TaktSalesQuotationChangeLogsController : TaktControllerBase
{
    private readonly ITaktSalesQuotationChangeLogService _salesQuotationChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesQuotationChangeLogService">销售报价变更记录服务</param>
    public TaktSalesQuotationChangeLogsController(ITaktSalesQuotationChangeLogService salesQuotationChangeLogService)
    {
        _salesQuotationChangeLogService = salesQuotationChangeLogService;
    }

    /// <summary>
    /// 获取销售报价变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:quotation:list", "销售报价变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesQuotationChangeLogListAsync([FromQuery] TaktSalesQuotationChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _salesQuotationChangeLogService.GetSalesQuotationChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <returns>销售报价变更记录DTO</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesQuotationChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _salesQuotationChangeLogService.GetSalesQuotationChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售报价变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售报价变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesQuotationChangeLogOptionsAsync()
    {
        try
        {
            var result = await _salesQuotationChangeLogService.GetSalesQuotationChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售报价变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售报价变更记录DTO</returns>
    [TaktPermission("logistics:sales:quotation:create", "创建销售报价变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesQuotationChangeLogAsync([FromBody] TaktSalesQuotationChangeLogCreateDto dto)
    {
        try
        {
            var result = await _salesQuotationChangeLogService.CreateSalesQuotationChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售报价变更记录DTO</returns>
    [TaktPermission("logistics:sales:quotation:update", "更新销售报价变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesQuotationChangeLogAsync(long id, [FromBody] TaktSalesQuotationChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _salesQuotationChangeLogService.UpdateSalesQuotationChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "删除销售报价变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesQuotationChangeLogByIdAsync(long id)
    {
        try
        {
            await _salesQuotationChangeLogService.DeleteSalesQuotationChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售报价变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "批量删除销售报价变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesQuotationChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesQuotationChangeLogService.DeleteSalesQuotationChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出销售报价变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:quotation:export", "导出销售报价变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesQuotationChangeLogAsync([FromQuery] TaktSalesQuotationChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesQuotationChangeLogService.ExportSalesQuotationChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
