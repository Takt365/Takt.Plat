// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityFailurePcbaReworksController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题PCBA不良改修费用明细控制器
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
/// 质量问题PCBA不良改修费用明细控制器
/// 提供质量问题PCBA不良改修费用明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "质量问题PCBA不良改修费用明细")]
public class TaktQualityFailurePcbaReworksController : TaktControllerBase
{
    private readonly ITaktQualityFailurePcbaReworkService _qualityFailurePcbaReworkService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityFailurePcbaReworkService">质量问题PCBA不良改修费用明细服务</param>
    public TaktQualityFailurePcbaReworksController(ITaktQualityFailurePcbaReworkService qualityFailurePcbaReworkService)
    {
        _qualityFailurePcbaReworkService = qualityFailurePcbaReworkService;
    }

    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:list", "质量问题PCBA不良改修费用明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityFailurePcbaReworkListAsync([FromQuery] TaktQualityFailurePcbaReworkQueryDto queryDto)
    {
        try
        {
            var result = await _qualityFailurePcbaReworkService.GetQualityFailurePcbaReworkListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>质量问题PCBA不良改修费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:query", "质量问题PCBA不良改修费用明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityFailurePcbaReworkByIdAsync(long id)
    {
        try
        {
            var result = await _qualityFailurePcbaReworkService.GetQualityFailurePcbaReworkByIdAsync(id);
            if (result == null)
            {
                return NotFound("质量问题PCBA不良改修费用明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:query", "质量问题PCBA不良改修费用明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityFailurePcbaReworkOptionsAsync()
    {
        try
        {
            var result = await _qualityFailurePcbaReworkService.GetQualityFailurePcbaReworkOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>质量问题PCBA不良改修费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:create", "创建质量问题PCBA不良改修费用明细")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityFailurePcbaReworkAsync([FromBody] TaktQualityFailurePcbaReworkCreateDto dto)
    {
        try
        {
            var result = await _qualityFailurePcbaReworkService.CreateQualityFailurePcbaReworkAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>质量问题PCBA不良改修费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:update", "更新质量问题PCBA不良改修费用明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityFailurePcbaReworkAsync(long id, [FromBody] TaktQualityFailurePcbaReworkUpdateDto dto)
    {
        try
        {
            var result = await _qualityFailurePcbaReworkService.UpdateQualityFailurePcbaReworkAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:delete", "删除质量问题PCBA不良改修费用明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityFailurePcbaReworkByIdAsync(long id)
    {
        try
        {
            await _qualityFailurePcbaReworkService.DeleteQualityFailurePcbaReworkByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:delete", "批量删除质量问题PCBA不良改修费用明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityFailurePcbaReworkBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityFailurePcbaReworkService.DeleteQualityFailurePcbaReworkBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:import", "获取质量问题PCBA不良改修费用明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityFailurePcbaReworkTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityFailurePcbaReworkService.GetQualityFailurePcbaReworkTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:import", "导入质量问题PCBA不良改修费用明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityFailurePcbaReworkAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityFailurePcbaReworkService.ImportQualityFailurePcbaReworkAsync(stream, sheetName);
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
    /// 导出质量问题PCBA不良改修费用明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:qualityfailurepcbarework:export", "导出质量问题PCBA不良改修费用明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityFailurePcbaReworkAsync([FromQuery] TaktQualityFailurePcbaReworkQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityFailurePcbaReworkService.ExportQualityFailurePcbaReworkAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
