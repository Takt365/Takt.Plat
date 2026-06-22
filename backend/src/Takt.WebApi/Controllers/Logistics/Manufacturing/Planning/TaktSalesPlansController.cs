// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlansController.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：销售计划控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;
using Takt.Application.Services.Logistics.Manufacturing.Planning;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Planning;

/// <summary>
/// 销售计划控制器
/// 提供销售计划的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售计划")]
public class TaktSalesPlansController : TaktControllerBase
{
    private readonly ITaktSalesPlanService _salesPlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPlanService">销售计划服务</param>
    public TaktSalesPlansController(ITaktSalesPlanService salesPlanService)
    {
        _salesPlanService = salesPlanService;
    }

    /// <summary>
    /// 获取销售计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:list", "销售计划列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesPlanListAsync([FromQuery] TaktSalesPlanQueryDto queryDto)
    {
        try
        {
            var result = await _salesPlanService.GetSalesPlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <returns>销售计划DTO</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:query", "销售计划详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesPlanByIdAsync(long id)
    {
        try
        {
            var result = await _salesPlanService.GetSalesPlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售计划不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:query", "销售计划选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesPlanOptionsAsync()
    {
        try
        {
            var result = await _salesPlanService.GetSalesPlanOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售计划DTO</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:create", "创建销售计划")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesPlanAsync([FromBody] TaktSalesPlanCreateDto dto)
    {
        try
        {
            var result = await _salesPlanService.CreateSalesPlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售计划DTO</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:update", "更新销售计划")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesPlanAsync(long id, [FromBody] TaktSalesPlanUpdateDto dto)
    {
        try
        {
            var result = await _salesPlanService.UpdateSalesPlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:delete", "删除销售计划")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesPlanByIdAsync(long id)
    {
        try
        {
            await _salesPlanService.DeleteSalesPlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:delete", "批量删除销售计划")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesPlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesPlanService.DeleteSalesPlanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售计划状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售计划DTO</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:update", "更新销售计划状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesPlanStatusAsync([FromBody] TaktSalesPlanStatusDto dto)
    {
        try
        {
            var result = await _salesPlanService.UpdateSalesPlanStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:planning:sales:plan:import", "获取销售计划导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesPlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesPlanService.GetSalesPlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售计划
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:import", "导入销售计划")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesPlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesPlanService.ImportSalesPlanAsync(stream, sheetName);
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
    /// 导出销售计划
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:planning:sales:plan:export", "导出销售计划")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesPlanAsync([FromQuery] TaktSalesPlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesPlanService.ExportSalesPlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
