// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialsController.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：物料控制器
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
/// 物料控制器
/// 提供物料的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "物料")]
public class TaktMaterialsController : TaktControllerBase
{
    private readonly ITaktMaterialService _materialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialService">物料服务</param>
    public TaktMaterialsController(ITaktMaterialService materialService)
    {
        _materialService = materialService;
    }

    /// <summary>
    /// 获取物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:list", "物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialListAsync([FromQuery] TaktMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _materialService.GetMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <returns>物料DTO</returns>
    [TaktPermission("logistics:materials:material:query", "物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _materialService.GetMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:query", "物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialOptionsAsync()
    {
        try
        {
            var result = await _materialService.GetMaterialOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料DTO</returns>
    [TaktPermission("logistics:materials:material:create", "创建物料")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialAsync([FromBody] TaktMaterialCreateDto dto)
    {
        try
        {
            var result = await _materialService.CreateMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料DTO</returns>
    [TaktPermission("logistics:materials:material:update", "更新物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialAsync(long id, [FromBody] TaktMaterialUpdateDto dto)
    {
        try
        {
            var result = await _materialService.UpdateMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料
    /// </summary>
    /// <param name="id">物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:delete", "删除物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialByIdAsync(long id)
    {
        try
        {
            await _materialService.DeleteMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:delete", "批量删除物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialService.DeleteMaterialBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>物料DTO</returns>
    [TaktPermission("logistics:materials:material:update", "更新物料状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaterialStatusAsync([FromBody] TaktMaterialStatusDto dto)
    {
        try
        {
            var result = await _materialService.UpdateMaterialStatusAsync(dto);
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
    [TaktPermission("logistics:materials:material:import", "获取物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialService.GetMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:import", "导入物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialService.ImportMaterialAsync(stream, sheetName);
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
    /// 导出物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:export", "导出物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialAsync([FromQuery] TaktMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialService.ExportMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
