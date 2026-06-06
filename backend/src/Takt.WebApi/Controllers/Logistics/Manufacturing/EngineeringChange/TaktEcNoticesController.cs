// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticesController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单控制器
/// 提供工程变更通知单的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "工程变更通知单")]
public class TaktEcNoticesController : TaktControllerBase
{
    private readonly ITaktEcNoticeService _ecNoticeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecNoticeService">工程变更通知单服务</param>
    public TaktEcNoticesController(ITaktEcNoticeService ecNoticeService)
    {
        _ecNoticeService = ecNoticeService;
    }

    /// <summary>
    /// 获取工程变更通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:list", "工程变更通知单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcNoticeListAsync([FromQuery] TaktEcNoticeQueryDto queryDto)
    {
        try
        {
            var result = await _ecNoticeService.GetEcNoticeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:query", "工程变更通知单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcNoticeByIdAsync(long id)
    {
        try
        {
            var result = await _ecNoticeService.GetEcNoticeByIdAsync(id);
            if (result == null)
            {
                return NotFound("工程变更通知单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工程变更通知单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:query", "工程变更通知单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcNoticeOptionsAsync()
    {
        try
        {
            var result = await _ecNoticeService.GetEcNoticeOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工程变更通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:create", "创建工程变更通知单")]
    [HttpPost]
    public async Task<IActionResult> CreateEcNoticeAsync([FromBody] TaktEcNoticeCreateDto dto)
    {
        try
        {
            var result = await _ecNoticeService.CreateEcNoticeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:update", "更新工程变更通知单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcNoticeAsync(long id, [FromBody] TaktEcNoticeUpdateDto dto)
    {
        try
        {
            var result = await _ecNoticeService.UpdateEcNoticeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:delete", "删除工程变更通知单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcNoticeByIdAsync(long id)
    {
        try
        {
            await _ecNoticeService.DeleteEcNoticeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工程变更通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:delete", "批量删除工程变更通知单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcNoticeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecNoticeService.DeleteEcNoticeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工程变更通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:update", "更新工程变更通知单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEcNoticeStatusAsync([FromBody] TaktEcNoticeStatusDto dto)
    {
        try
        {
            var result = await _ecNoticeService.UpdateEcNoticeStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:import", "获取工程变更通知单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcNoticeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecNoticeService.GetEcNoticeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工程变更通知单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:import", "导入工程变更通知单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcNoticeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecNoticeService.ImportEcNoticeAsync(stream, sheetName);
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
    /// 导出工程变更通知单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecnotice:export", "导出工程变更通知单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcNoticeAsync([FromQuery] TaktEcNoticeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecNoticeService.ExportEcNoticeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
