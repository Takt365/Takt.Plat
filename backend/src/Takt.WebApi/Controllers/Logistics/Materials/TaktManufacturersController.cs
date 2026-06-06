// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktManufacturersController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：制造商信息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 制造商信息控制器
/// 提供制造商信息的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "制造商信息")]
public class TaktManufacturersController : TaktControllerBase
{
    private readonly ITaktManufacturerService _manufacturerService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="manufacturerService">制造商信息服务</param>
    public TaktManufacturersController(ITaktManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    /// <summary>
    /// 获取制造商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:manufacturer:list", "制造商信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetManufacturerListAsync([FromQuery] TaktManufacturerQueryDto queryDto)
    {
        try
        {
            var result = await _manufacturerService.GetManufacturerListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <returns>制造商信息DTO</returns>
    [TaktPermission("logistics:materials:manufacturer:query", "制造商信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManufacturerByIdAsync(long id)
    {
        try
        {
            var result = await _manufacturerService.GetManufacturerByIdAsync(id);
            if (result == null)
            {
                return NotFound("制造商信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取制造商信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:manufacturer:query", "制造商信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetManufacturerOptionsAsync()
    {
        try
        {
            var result = await _manufacturerService.GetManufacturerOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建制造商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>制造商信息DTO</returns>
    [TaktPermission("logistics:materials:manufacturer:create", "创建制造商信息")]
    [HttpPost]
    public async Task<IActionResult> CreateManufacturerAsync([FromBody] TaktManufacturerCreateDto dto)
    {
        try
        {
            var result = await _manufacturerService.CreateManufacturerAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>制造商信息DTO</returns>
    [TaktPermission("logistics:materials:manufacturer:update", "更新制造商信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManufacturerAsync(long id, [FromBody] TaktManufacturerUpdateDto dto)
    {
        try
        {
            var result = await _manufacturerService.UpdateManufacturerAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除制造商信息
    /// </summary>
    /// <param name="id">制造商信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:manufacturer:delete", "删除制造商信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManufacturerByIdAsync(long id)
    {
        try
        {
            await _manufacturerService.DeleteManufacturerByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除制造商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:manufacturer:delete", "批量删除制造商信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteManufacturerBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _manufacturerService.DeleteManufacturerBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制造商信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>制造商信息DTO</returns>
    [TaktPermission("logistics:materials:manufacturer:update", "更新制造商信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateManufacturerStatusAsync([FromBody] TaktManufacturerStatusDto dto)
    {
        try
        {
            var result = await _manufacturerService.UpdateManufacturerStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制造商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>制造商信息DTO</returns>
    [TaktPermission("logistics:materials:manufacturer:update", "更新制造商信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateManufacturerSortAsync([FromBody] TaktManufacturerSortDto dto)
    {
        try
        {
            var result = await _manufacturerService.UpdateManufacturerSortAsync(dto);
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
    [TaktPermission("logistics:materials:manufacturer:import", "获取制造商信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetManufacturerTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _manufacturerService.GetManufacturerTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入制造商信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:manufacturer:import", "导入制造商信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportManufacturerAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _manufacturerService.ImportManufacturerAsync(stream, sheetName);
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
    /// 导出制造商信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:manufacturer:export", "导出制造商信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportManufacturerAsync([FromQuery] TaktManufacturerQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _manufacturerService.ExportManufacturerAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
