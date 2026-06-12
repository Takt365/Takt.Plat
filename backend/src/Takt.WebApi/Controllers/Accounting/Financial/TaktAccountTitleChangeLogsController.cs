// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktAccountTitleChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 会计科目变更记录控制器
/// 提供会计科目变更记录的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "会计科目变更记录")]
public class TaktAccountTitleChangeLogsController : TaktControllerBase
{
    private readonly ITaktAccountTitleChangeLogService _accountTitleChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="accountTitleChangeLogService">会计科目变更记录服务</param>
    public TaktAccountTitleChangeLogsController(ITaktAccountTitleChangeLogService accountTitleChangeLogService)
    {
        _accountTitleChangeLogService = accountTitleChangeLogService;
    }

    /// <summary>
    /// 获取会计科目变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:list", "会计科目变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAccountTitleChangeLogListAsync([FromQuery] TaktAccountTitleChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _accountTitleChangeLogService.GetAccountTitleChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>会计科目变更记录DTO</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:query", "会计科目变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountTitleChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _accountTitleChangeLogService.GetAccountTitleChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("会计科目变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会计科目变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:query", "会计科目变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAccountTitleChangeLogOptionsAsync()
    {
        try
        {
            var result = await _accountTitleChangeLogService.GetAccountTitleChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会计科目变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会计科目变更记录DTO</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:create", "创建会计科目变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateAccountTitleChangeLogAsync([FromBody] TaktAccountTitleChangeLogCreateDto dto)
    {
        try
        {
            var result = await _accountTitleChangeLogService.CreateAccountTitleChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会计科目变更记录DTO</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:update", "更新会计科目变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccountTitleChangeLogAsync(long id, [FromBody] TaktAccountTitleChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _accountTitleChangeLogService.UpdateAccountTitleChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:delete", "删除会计科目变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountTitleChangeLogByIdAsync(long id)
    {
        try
        {
            await _accountTitleChangeLogService.DeleteAccountTitleChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会计科目变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:delete", "批量删除会计科目变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAccountTitleChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _accountTitleChangeLogService.DeleteAccountTitleChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出会计科目变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:accounttitlechangelog:export", "导出会计科目变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAccountTitleChangeLogAsync([FromQuery] TaktAccountTitleChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _accountTitleChangeLogService.ExportAccountTitleChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
