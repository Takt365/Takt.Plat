// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Application.Services.Routine.DocumentCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.DocumentCenter;

/// <summary>
/// 文管文档变更日志控制器
/// 提供文管文档变更日志的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "文管文档变更日志")]
public class TaktDocumentChangeLogsController : TaktControllerBase
{
    private readonly ITaktDocumentChangeLogService _documentChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentChangeLogService">文管文档变更日志服务</param>
    public TaktDocumentChangeLogsController(ITaktDocumentChangeLogService documentChangeLogService)
    {
        _documentChangeLogService = documentChangeLogService;
    }

    /// <summary>
    /// 获取文管文档变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:list", "文管文档变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDocumentChangeLogListAsync([FromQuery] TaktDocumentChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _documentChangeLogService.GetDocumentChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <returns>文管文档变更日志DTO</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:query", "文管文档变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _documentChangeLogService.GetDocumentChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("文管文档变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取文管文档变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:query", "文管文档变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDocumentChangeLogOptionsAsync()
    {
        try
        {
            var result = await _documentChangeLogService.GetDocumentChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建文管文档变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>文管文档变更日志DTO</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:create", "创建文管文档变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateDocumentChangeLogAsync([FromBody] TaktDocumentChangeLogCreateDto dto)
    {
        try
        {
            var result = await _documentChangeLogService.CreateDocumentChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>文管文档变更日志DTO</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:update", "更新文管文档变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocumentChangeLogAsync(long id, [FromBody] TaktDocumentChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _documentChangeLogService.UpdateDocumentChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:delete", "删除文管文档变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentChangeLogByIdAsync(long id)
    {
        try
        {
            await _documentChangeLogService.DeleteDocumentChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除文管文档变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:delete", "批量删除文管文档变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDocumentChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _documentChangeLogService.DeleteDocumentChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出文管文档变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:documentcenter:documentchangelog:export", "导出文管文档变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDocumentChangeLogAsync([FromQuery] TaktDocumentChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _documentChangeLogService.ExportDocumentChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
