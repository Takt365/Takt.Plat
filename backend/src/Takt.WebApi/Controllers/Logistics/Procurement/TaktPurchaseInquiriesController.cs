// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInquiriesController.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement.Chain;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 采购询价控制器
/// 提供采购询价的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购询价")]
public class TaktPurchaseInquiriesController : TaktControllerBase
{
    private readonly ITaktPurchaseInquiryService _purchaseInquiryService;
    private readonly ITaktProcurementChainOrchestrator _procurementChainOrchestrator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInquiryService">采购询价服务</param>
    /// <param name="procurementChainOrchestrator">采购全链路编排</param>
    public TaktPurchaseInquiriesController(
        ITaktPurchaseInquiryService purchaseInquiryService,
        ITaktProcurementChainOrchestrator procurementChainOrchestrator)
    {
        _purchaseInquiryService = purchaseInquiryService;
        _procurementChainOrchestrator = procurementChainOrchestrator;
    }

    /// <summary>
    /// 获取采购询价列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:list", "采购询价列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInquiryListAsync([FromQuery] TaktPurchaseInquiryQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInquiryService.GetPurchaseInquiryListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>采购询价DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:query", "采购询价详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseInquiryByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInquiryService.GetPurchaseInquiryByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购询价不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购询价选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:query", "采购询价选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInquiryOptionsAsync()
    {
        try
        {
            var result = await _purchaseInquiryService.GetPurchaseInquiryOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购询价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购询价DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:create", "创建采购询价")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInquiryAsync([FromBody] TaktPurchaseInquiryCreateDto dto)
    {
        try
        {
            var result = await _purchaseInquiryService.CreatePurchaseInquiryAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购询价DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:update", "更新采购询价")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInquiryAsync(long id, [FromBody] TaktPurchaseInquiryUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInquiryService.UpdatePurchaseInquiryAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:delete", "删除采购询价")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInquiryByIdAsync(long id)
    {
        try
        {
            await _purchaseInquiryService.DeletePurchaseInquiryByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购询价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:delete", "批量删除采购询价")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInquiryBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInquiryService.DeletePurchaseInquiryBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购询价状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购询价DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:update", "更新采购询价状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseInquiryStatusAsync([FromBody] TaktPurchaseInquiryStatusDto dto)
    {
        try
        {
            var result = await _purchaseInquiryService.UpdatePurchaseInquiryStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:import", "获取采购询价导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInquiryTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInquiryService.GetPurchaseInquiryTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购询价
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:import", "导入采购询价")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInquiryAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInquiryService.ImportPurchaseInquiryAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 提交采购询价会签审批（方案一/二入口）
    /// </summary>
    /// <param name="id">询价主键</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:update", "提交采购询价会签")]
    [HttpPost("{id}/submit-countersign")]
    public async Task<IActionResult> SubmitPurchaseInquiryCountersignAsync(long id)
    {
        try
        {
            await _procurementChainOrchestrator.SubmitPurchaseInquiryForCountersignAsync(id);
            return Success<object?>(null, "提交成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出采购询价
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:export", "导出采购询价")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInquiryAsync([FromQuery] TaktPurchaseInquiryQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInquiryService.ExportPurchaseInquiryAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
