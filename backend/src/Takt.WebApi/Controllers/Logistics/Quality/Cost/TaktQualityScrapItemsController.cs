// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapItemsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：品质废弃明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 品质废弃明细控制器
/// 提供品质废弃明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "品质废弃明细")]
public class TaktQualityScrapItemsController : TaktControllerBase
{
    private readonly ITaktQualityScrapItemService _qualityScrapItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityScrapItemService">品质废弃明细服务</param>
    public TaktQualityScrapItemsController(ITaktQualityScrapItemService qualityScrapItemService)
    {
        _qualityScrapItemService = qualityScrapItemService;
    }

    /// <summary>
    /// 获取品质废弃明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:list", "品质废弃明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityScrapItemListAsync([FromQuery] TaktQualityScrapItemQueryDto queryDto)
    {
        try
        {
            var result = await _qualityScrapItemService.GetQualityScrapItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质废弃明细
    /// </summary>
    /// <param name="id">品质废弃明细ID</param>
    /// <returns>品质废弃明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:query", "品质废弃明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityScrapItemByIdAsync(long id)
    {
        try
        {
            var result = await _qualityScrapItemService.GetQualityScrapItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质废弃明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质废弃明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:query", "品质废弃明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityScrapItemOptionsAsync()
    {
        try
        {
            var result = await _qualityScrapItemService.GetQualityScrapItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质废弃明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质废弃明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:create", "创建品质废弃明细")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityScrapItemAsync([FromBody] TaktQualityScrapItemCreateDto dto)
    {
        try
        {
            var result = await _qualityScrapItemService.CreateQualityScrapItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质废弃明细
    /// </summary>
    /// <param name="id">品质废弃明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质废弃明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:update", "更新品质废弃明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityScrapItemAsync(long id, [FromBody] TaktQualityScrapItemUpdateDto dto)
    {
        try
        {
            var result = await _qualityScrapItemService.UpdateQualityScrapItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质废弃明细
    /// </summary>
    /// <param name="id">品质废弃明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:delete", "删除品质废弃明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityScrapItemByIdAsync(long id)
    {
        try
        {
            await _qualityScrapItemService.DeleteQualityScrapItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质废弃明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:delete", "批量删除品质废弃明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityScrapItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityScrapItemService.DeleteQualityScrapItemBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:qualityscrapitem:import", "获取品质废弃明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityScrapItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityScrapItemService.GetQualityScrapItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质废弃明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:import", "导入品质废弃明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityScrapItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityScrapItemService.ImportQualityScrapItemAsync(stream, sheetName);
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
    /// 导出品质废弃明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:qualityscrapitem:export", "导出品质废弃明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityScrapItemAsync([FromQuery] TaktQualityScrapItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityScrapItemService.ExportQualityScrapItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
