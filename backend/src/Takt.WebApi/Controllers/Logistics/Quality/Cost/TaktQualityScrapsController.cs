// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityScrapsController.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：品质废弃主控制器
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
/// 品质废弃主控制器
/// 提供品质废弃主的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "品质废弃主")]
public class TaktQualityScrapsController : TaktControllerBase
{
    private readonly ITaktQualityScrapService _qualityScrapService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityScrapService">品质废弃主服务</param>
    public TaktQualityScrapsController(ITaktQualityScrapService qualityScrapService)
    {
        _qualityScrapService = qualityScrapService;
    }

    /// <summary>
    /// 获取品质废弃主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:list", "品质废弃主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityScrapListAsync([FromQuery] TaktQualityScrapQueryDto queryDto)
    {
        try
        {
            var result = await _qualityScrapService.GetQualityScrapListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <returns>品质废弃主DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:query", "品质废弃主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityScrapByIdAsync(long id)
    {
        try
        {
            var result = await _qualityScrapService.GetQualityScrapByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质废弃主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质废弃主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:query", "品质废弃主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityScrapOptionsAsync()
    {
        try
        {
            var result = await _qualityScrapService.GetQualityScrapOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质废弃主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质废弃主DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:create", "创建品质废弃主")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityScrapAsync([FromBody] TaktQualityScrapCreateDto dto)
    {
        try
        {
            var result = await _qualityScrapService.CreateQualityScrapAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质废弃主DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:update", "更新品质废弃主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityScrapAsync(long id, [FromBody] TaktQualityScrapUpdateDto dto)
    {
        try
        {
            var result = await _qualityScrapService.UpdateQualityScrapAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质废弃主
    /// </summary>
    /// <param name="id">品质废弃主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:delete", "删除品质废弃主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityScrapByIdAsync(long id)
    {
        try
        {
            await _qualityScrapService.DeleteQualityScrapByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质废弃主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:delete", "批量删除品质废弃主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityScrapBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityScrapService.DeleteQualityScrapBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:qualityscrap:import", "获取品质废弃主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityScrapTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityScrapService.GetQualityScrapTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质废弃主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:import", "导入品质废弃主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityScrapAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityScrapService.ImportQualityScrapAsync(stream, sheetName);
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
    /// 导出品质废弃主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:qualityscrap:export", "导出品质废弃主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityScrapAsync([FromQuery] TaktQualityScrapQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityScrapService.ExportQualityScrapAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
