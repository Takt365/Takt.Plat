// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：设变主控制器
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
/// 设变主控制器
/// 提供设变主的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "设变主")]
public class TaktEcsController : TaktControllerBase
{
    private readonly ITaktEcService _ecService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecService">设变主服务</param>
    public TaktEcsController(ITaktEcService ecService)
    {
        _ecService = ecService;
    }

    /// <summary>
    /// 获取设变主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:list", "设变主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcListAsync([FromQuery] TaktEcQueryDto queryDto)
    {
        try
        {
            var result = await _ecService.GetEcListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>设变主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:query", "设变主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcByIdAsync(long id)
    {
        try
        {
            var result = await _ecService.GetEcByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:query", "设变主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcOptionsAsync()
    {
        try
        {
            var result = await _ecService.GetEcOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:create", "创建设变主")]
    [HttpPost]
    public async Task<IActionResult> CreateEcAsync([FromBody] TaktEcCreateDto dto)
    {
        try
        {
            var result = await _ecService.CreateEcAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:update", "更新设变主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcAsync(long id, [FromBody] TaktEcUpdateDto dto)
    {
        try
        {
            var result = await _ecService.UpdateEcAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:delete", "删除设变主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcByIdAsync(long id)
    {
        try
        {
            await _ecService.DeleteEcByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:delete", "批量删除设变主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecService.DeleteEcBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>设变主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:update", "更新设变主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEcStatusAsync([FromBody] TaktEcStatusDto dto)
    {
        try
        {
            var result = await _ecService.UpdateEcStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:import", "获取设变主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecService.GetEcTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:import", "导入设变主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecService.ImportEcAsync(stream, sheetName);
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
    /// 导出设变主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ec:export", "导出设变主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcAsync([FromQuery] TaktEcQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecService.ExportEcAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
