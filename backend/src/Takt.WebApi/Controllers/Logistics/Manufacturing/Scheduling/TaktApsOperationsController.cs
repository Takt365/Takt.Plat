// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsOperationsController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS工序排程控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;
using Takt.Application.Services.Logistics.Manufacturing.Scheduling;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Scheduling;

/// <summary>
/// APS工序排程控制器
/// 提供APS工序排程的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS工序排程")]
public class TaktApsOperationsController : TaktControllerBase
{
    private readonly ITaktApsOperationService _apsOperationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOperationService">APS工序排程服务</param>
    public TaktApsOperationsController(ITaktApsOperationService apsOperationService)
    {
        _apsOperationService = apsOperationService;
    }

    /// <summary>
    /// 获取APS工序排程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:list", "APS工序排程列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsOperationListAsync([FromQuery] TaktApsOperationQueryDto queryDto)
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:query", "APS工序排程详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsOperationByIdAsync(long id)
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS工序排程不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS工序排程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:query", "APS工序排程选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsOperationOptionsAsync()
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS工序排程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:create", "创建APS工序排程")]
    [HttpPost]
    public async Task<IActionResult> CreateApsOperationAsync([FromBody] TaktApsOperationCreateDto dto)
    {
        try
        {
            var result = await _apsOperationService.CreateApsOperationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:update", "更新APS工序排程")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsOperationAsync(long id, [FromBody] TaktApsOperationUpdateDto dto)
    {
        try
        {
            var result = await _apsOperationService.UpdateApsOperationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:delete", "删除APS工序排程")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsOperationByIdAsync(long id)
    {
        try
        {
            await _apsOperationService.DeleteApsOperationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS工序排程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:delete", "批量删除APS工序排程")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsOperationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsOperationService.DeleteApsOperationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS工序排程状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:update", "更新APS工序排程状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsOperationStatusAsync([FromBody] TaktApsOperationStatusDto dto)
    {
        try
        {
            var result = await _apsOperationService.UpdateApsOperationStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:import", "获取APS工序排程导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsOperationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsOperationService.GetApsOperationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS工序排程
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:import", "导入APS工序排程")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsOperationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsOperationService.ImportApsOperationAsync(stream, sheetName);
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
    /// 导出APS工序排程
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:scheduling:aps:operation:export", "导出APS工序排程")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsOperationAsync([FromQuery] TaktApsOperationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsOperationService.ExportApsOperationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
