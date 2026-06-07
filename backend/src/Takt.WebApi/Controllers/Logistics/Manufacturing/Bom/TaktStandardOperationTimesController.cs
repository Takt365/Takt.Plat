// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimesController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工序时间控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 标准工序时间控制器
/// 提供标准工序时间的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "标准工序时间")]
public class TaktStandardOperationTimesController : TaktControllerBase
{
    private readonly ITaktStandardOperationTimeService _standardOperationTimeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationTimeService">标准工序时间服务</param>
    public TaktStandardOperationTimesController(ITaktStandardOperationTimeService standardOperationTimeService)
    {
        _standardOperationTimeService = standardOperationTimeService;
    }

    /// <summary>
    /// 获取标准工序时间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:list", "标准工序时间列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStandardOperationTimeListAsync([FromQuery] TaktStandardOperationTimeQueryDto queryDto)
    {
        try
        {
            var result = await _standardOperationTimeService.GetStandardOperationTimeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>标准工序时间DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:query", "标准工序时间详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStandardOperationTimeByIdAsync(long id)
    {
        try
        {
            var result = await _standardOperationTimeService.GetStandardOperationTimeByIdAsync(id);
            if (result == null)
            {
                return NotFound("标准工序时间不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取标准工序时间选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:query", "标准工序时间选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStandardOperationTimeOptionsAsync()
    {
        try
        {
            var result = await _standardOperationTimeService.GetStandardOperationTimeOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建标准工序时间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>标准工序时间DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:create", "创建标准工序时间")]
    [HttpPost]
    public async Task<IActionResult> CreateStandardOperationTimeAsync([FromBody] TaktStandardOperationTimeCreateDto dto)
    {
        try
        {
            var result = await _standardOperationTimeService.CreateStandardOperationTimeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>标准工序时间DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:update", "更新标准工序时间")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStandardOperationTimeAsync(long id, [FromBody] TaktStandardOperationTimeUpdateDto dto)
    {
        try
        {
            var result = await _standardOperationTimeService.UpdateStandardOperationTimeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:delete", "删除标准工序时间")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandardOperationTimeByIdAsync(long id)
    {
        try
        {
            await _standardOperationTimeService.DeleteStandardOperationTimeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除标准工序时间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:delete", "批量删除标准工序时间")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStandardOperationTimeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _standardOperationTimeService.DeleteStandardOperationTimeBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:import", "获取标准工序时间导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetStandardOperationTimeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _standardOperationTimeService.GetStandardOperationTimeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入标准工序时间
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:import", "导入标准工序时间")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportStandardOperationTimeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _standardOperationTimeService.ImportStandardOperationTimeAsync(stream, sheetName);
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
    /// 导出标准工序时间
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:standardoperationtime:export", "导出标准工序时间")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStandardOperationTimeAsync([FromQuery] TaktStandardOperationTimeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _standardOperationTimeService.ExportStandardOperationTimeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
