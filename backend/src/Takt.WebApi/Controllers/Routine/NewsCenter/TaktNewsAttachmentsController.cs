// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsAttachmentsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心附件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Application.Services.Routine.NewsCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.NewsCenter;

/// <summary>
/// 新闻中心附件控制器
/// 提供新闻中心附件的 REST API
/// </summary>
[ApiModule(TaktModule.Routine, "日常事务")]
[Route("api/[controller]", Name = "新闻中心附件")]
public class TaktNewsAttachmentsController : TaktControllerBase
{
    private readonly ITaktNewsAttachmentService _newsAttachmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsAttachmentService">新闻中心附件服务</param>
    public TaktNewsAttachmentsController(ITaktNewsAttachmentService newsAttachmentService)
    {
        _newsAttachmentService = newsAttachmentService;
    }

    /// <summary>
    /// 获取新闻中心附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:newscenter:newsattachment:list", "新闻中心附件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsAttachmentListAsync([FromQuery] TaktNewsAttachmentQueryDto queryDto)
    {
        try
        {
            var result = await _newsAttachmentService.GetNewsAttachmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>新闻中心附件DTO</returns>
    [TaktPermission("routine:newscenter:newsattachment:query", "新闻中心附件详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsAttachmentByIdAsync(long id)
    {
        try
        {
            var result = await _newsAttachmentService.GetNewsAttachmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心附件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:newscenter:newsattachment:query", "新闻中心附件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsAttachmentOptionsAsync()
    {
        try
        {
            var result = await _newsAttachmentService.GetNewsAttachmentOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心附件DTO</returns>
    [TaktPermission("routine:newscenter:newsattachment:create", "创建新闻中心附件")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsAttachmentAsync([FromBody] TaktNewsAttachmentCreateDto dto)
    {
        try
        {
            var result = await _newsAttachmentService.CreateNewsAttachmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心附件DTO</returns>
    [TaktPermission("routine:newscenter:newsattachment:update", "更新新闻中心附件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsAttachmentAsync(long id, [FromBody] TaktNewsAttachmentUpdateDto dto)
    {
        try
        {
            var result = await _newsAttachmentService.UpdateNewsAttachmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:newscenter:newsattachment:delete", "删除新闻中心附件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsAttachmentByIdAsync(long id)
    {
        try
        {
            await _newsAttachmentService.DeleteNewsAttachmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:newscenter:newsattachment:delete", "批量删除新闻中心附件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsAttachmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsAttachmentService.DeleteNewsAttachmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心附件排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>新闻中心附件DTO</returns>
    [TaktPermission("routine:newscenter:newsattachment:update", "更新新闻中心附件排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateNewsAttachmentSortAsync([FromBody] TaktNewsAttachmentSortDto dto)
    {
        try
        {
            var result = await _newsAttachmentService.UpdateNewsAttachmentSortAsync(dto);
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
    [TaktPermission("routine:newscenter:newsattachment:import", "获取新闻中心附件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsAttachmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsAttachmentService.GetNewsAttachmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心附件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:newscenter:newsattachment:import", "导入新闻中心附件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsAttachmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsAttachmentService.ImportNewsAttachmentAsync(stream, sheetName);
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
    /// 导出新闻中心附件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:newscenter:newsattachment:export", "导出新闻中心附件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsAttachmentAsync([FromQuery] TaktNewsAttachmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsAttachmentService.ExportNewsAttachmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
