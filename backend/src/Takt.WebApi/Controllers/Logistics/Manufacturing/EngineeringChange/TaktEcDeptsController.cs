// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门控制器
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
/// 设变部门控制器
/// 提供设变部门的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "设变部门")]
public class TaktEcDeptsController : TaktControllerBase
{
    private readonly ITaktEcDeptService _ecDeptService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDeptService">设变部门服务</param>
    public TaktEcDeptsController(ITaktEcDeptService ecDeptService)
    {
        _ecDeptService = ecDeptService;
    }

    /// <summary>
    /// 获取设变部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:list", "设变部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcDeptListAsync([FromQuery] TaktEcDeptQueryDto queryDto)
    {
        try
        {
            var result = await _ecDeptService.GetEcDeptListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>设变部门DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:query", "设变部门详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcDeptByIdAsync(long id)
    {
        try
        {
            var result = await _ecDeptService.GetEcDeptByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变部门不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变部门选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:query", "设变部门选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcDeptOptionsAsync()
    {
        try
        {
            var result = await _ecDeptService.GetEcDeptOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变部门DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:create", "创建设变部门")]
    [HttpPost]
    public async Task<IActionResult> CreateEcDeptAsync([FromBody] TaktEcDeptCreateDto dto)
    {
        try
        {
            var result = await _ecDeptService.CreateEcDeptAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变部门DTO</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:update", "更新设变部门")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcDeptAsync(long id, [FromBody] TaktEcDeptUpdateDto dto)
    {
        try
        {
            var result = await _ecDeptService.UpdateEcDeptAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:delete", "删除设变部门")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcDeptByIdAsync(long id)
    {
        try
        {
            await _ecDeptService.DeleteEcDeptByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:delete", "批量删除设变部门")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcDeptBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecDeptService.DeleteEcDeptBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:import", "获取设变部门导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcDeptTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecDeptService.GetEcDeptTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变部门
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:import", "导入设变部门")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcDeptAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecDeptService.ImportEcDeptAsync(stream, sheetName);
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
    /// 导出设变部门
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineeringchange:ecdept:export", "导出设变部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcDeptAsync([FromQuery] TaktEcDeptQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecDeptService.ExportEcDeptAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
