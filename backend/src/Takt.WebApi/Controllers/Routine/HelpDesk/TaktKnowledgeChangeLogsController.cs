// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktKnowledgeChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：知识库变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 知识库变更日志控制器
/// 提供知识库变更日志的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "知识库变更日志")]
public class TaktKnowledgeChangeLogsController : TaktControllerBase
{
    private readonly ITaktKnowledgeChangeLogService _knowledgeChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="knowledgeChangeLogService">知识库变更日志服务</param>
    public TaktKnowledgeChangeLogsController(ITaktKnowledgeChangeLogService knowledgeChangeLogService)
    {
        _knowledgeChangeLogService = knowledgeChangeLogService;
    }

    /// <summary>
    /// 获取知识库变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:list", "知识库变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetKnowledgeChangeLogListAsync([FromQuery] TaktKnowledgeChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _knowledgeChangeLogService.GetKnowledgeChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <returns>知识库变更日志DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:query", "知识库变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetKnowledgeChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _knowledgeChangeLogService.GetKnowledgeChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("知识库变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取知识库变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:query", "知识库变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetKnowledgeChangeLogOptionsAsync()
    {
        try
        {
            var result = await _knowledgeChangeLogService.GetKnowledgeChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建知识库变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>知识库变更日志DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:create", "创建知识库变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateKnowledgeChangeLogAsync([FromBody] TaktKnowledgeChangeLogCreateDto dto)
    {
        try
        {
            var result = await _knowledgeChangeLogService.CreateKnowledgeChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>知识库变更日志DTO</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:update", "更新知识库变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateKnowledgeChangeLogAsync(long id, [FromBody] TaktKnowledgeChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _knowledgeChangeLogService.UpdateKnowledgeChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:delete", "删除知识库变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKnowledgeChangeLogByIdAsync(long id)
    {
        try
        {
            await _knowledgeChangeLogService.DeleteKnowledgeChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除知识库变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:delete", "批量删除知识库变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteKnowledgeChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _knowledgeChangeLogService.DeleteKnowledgeChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出知识库变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:knowledge:change:log:export", "导出知识库变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportKnowledgeChangeLogAsync([FromQuery] TaktKnowledgeChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _knowledgeChangeLogService.ExportKnowledgeChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
