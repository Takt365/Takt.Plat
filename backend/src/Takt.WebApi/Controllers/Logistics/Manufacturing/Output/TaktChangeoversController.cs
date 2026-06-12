// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoversController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：切换记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 切换记录控制器
/// 提供切换记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "切换记录")]
public class TaktChangeoversController : TaktControllerBase
{
    private readonly ITaktChangeoverService _changeoverService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="changeoverService">切换记录服务</param>
    public TaktChangeoversController(ITaktChangeoverService changeoverService)
    {
        _changeoverService = changeoverService;
    }

    /// <summary>
    /// 获取切换记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:list", "切换记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetChangeoverListAsync([FromQuery] TaktChangeoverQueryDto queryDto)
    {
        try
        {
            var result = await _changeoverService.GetChangeoverListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:query", "切换记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChangeoverByIdAsync(long id)
    {
        try
        {
            var result = await _changeoverService.GetChangeoverByIdAsync(id);
            if (result == null)
            {
                return NotFound("切换记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取切换记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:query", "切换记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetChangeoverOptionsAsync()
    {
        try
        {
            var result = await _changeoverService.GetChangeoverOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建切换记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:create", "创建切换记录")]
    [HttpPost]
    public async Task<IActionResult> CreateChangeoverAsync([FromBody] TaktChangeoverCreateDto dto)
    {
        try
        {
            var result = await _changeoverService.CreateChangeoverAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:update", "更新切换记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChangeoverAsync(long id, [FromBody] TaktChangeoverUpdateDto dto)
    {
        try
        {
            var result = await _changeoverService.UpdateChangeoverAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:delete", "删除切换记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChangeoverByIdAsync(long id)
    {
        try
        {
            await _changeoverService.DeleteChangeoverByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除切换记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:delete", "批量删除切换记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteChangeoverBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _changeoverService.DeleteChangeoverBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:manufacturing:output:changeover:import", "获取切换记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetChangeoverTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _changeoverService.GetChangeoverTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入切换记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:import", "导入切换记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportChangeoverAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _changeoverService.ImportChangeoverAsync(stream, sheetName);
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
    /// 导出切换记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:export", "导出切换记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportChangeoverAsync([FromQuery] TaktChangeoverQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _changeoverService.ExportChangeoverAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
