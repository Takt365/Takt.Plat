// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityOperationIncomingsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务来料检验费用明细控制器
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
/// 品质业务来料检验费用明细控制器
/// 提供品质业务来料检验费用明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "品质业务来料检验费用明细")]
public class TaktQualityOperationIncomingsController : TaktControllerBase
{
    private readonly ITaktQualityOperationIncomingService _qualityOperationIncomingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityOperationIncomingService">品质业务来料检验费用明细服务</param>
    public TaktQualityOperationIncomingsController(ITaktQualityOperationIncomingService qualityOperationIncomingService)
    {
        _qualityOperationIncomingService = qualityOperationIncomingService;
    }

    /// <summary>
    /// 获取品质业务来料检验费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:list", "品质业务来料检验费用明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityOperationIncomingListAsync([FromQuery] TaktQualityOperationIncomingQueryDto queryDto)
    {
        try
        {
            var result = await _qualityOperationIncomingService.GetQualityOperationIncomingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质业务来料检验费用明细
    /// </summary>
    /// <param name="id">品质业务来料检验费用明细ID</param>
    /// <returns>品质业务来料检验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:query", "品质业务来料检验费用明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityOperationIncomingByIdAsync(long id)
    {
        try
        {
            var result = await _qualityOperationIncomingService.GetQualityOperationIncomingByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质业务来料检验费用明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质业务来料检验费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:query", "品质业务来料检验费用明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityOperationIncomingOptionsAsync()
    {
        try
        {
            var result = await _qualityOperationIncomingService.GetQualityOperationIncomingOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质业务来料检验费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质业务来料检验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:create", "创建品质业务来料检验费用明细")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityOperationIncomingAsync([FromBody] TaktQualityOperationIncomingCreateDto dto)
    {
        try
        {
            var result = await _qualityOperationIncomingService.CreateQualityOperationIncomingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质业务来料检验费用明细
    /// </summary>
    /// <param name="id">品质业务来料检验费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质业务来料检验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:update", "更新品质业务来料检验费用明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityOperationIncomingAsync(long id, [FromBody] TaktQualityOperationIncomingUpdateDto dto)
    {
        try
        {
            var result = await _qualityOperationIncomingService.UpdateQualityOperationIncomingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质业务来料检验费用明细
    /// </summary>
    /// <param name="id">品质业务来料检验费用明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:delete", "删除品质业务来料检验费用明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityOperationIncomingByIdAsync(long id)
    {
        try
        {
            await _qualityOperationIncomingService.DeleteQualityOperationIncomingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质业务来料检验费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:delete", "批量删除品质业务来料检验费用明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityOperationIncomingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityOperationIncomingService.DeleteQualityOperationIncomingBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:import", "获取品质业务来料检验费用明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityOperationIncomingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityOperationIncomingService.GetQualityOperationIncomingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质业务来料检验费用明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:import", "导入品质业务来料检验费用明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityOperationIncomingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityOperationIncomingService.ImportQualityOperationIncomingAsync(stream, sheetName);
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
    /// 导出品质业务来料检验费用明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:qualityoperationincoming:export", "导出品质业务来料检验费用明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityOperationIncomingAsync([FromQuery] TaktQualityOperationIncomingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityOperationIncomingService.ExportQualityOperationIncomingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
