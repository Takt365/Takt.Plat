// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktOnlinesController.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 在线用户控制器
/// 提供在线用户的 REST API
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "在线用户")]
public class TaktOnlinesController : TaktControllerBase
{
    private readonly ITaktOnlineService _onlineService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineService">在线用户服务</param>
    public TaktOnlinesController(ITaktOnlineService onlineService)
    {
        _onlineService = onlineService;
    }

    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:online:list", "在线用户列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetOnlineListAsync([FromQuery] TaktOnlineQueryDto queryDto)
    {
        try
        {
            var result = await _onlineService.GetOnlineListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:query", "在线用户详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOnlineByIdAsync(long id)
    {
        try
        {
            var result = await _onlineService.GetOnlineByIdAsync(id);
            if (result == null)
            {
                return NotFound("在线用户不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:online:query", "在线用户选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetOnlineOptionsAsync()
    {
        try
        {
            var result = await _onlineService.GetOnlineOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建在线用户
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:create", "创建在线用户")]
    [HttpPost]
    public async Task<IActionResult> CreateOnlineAsync([FromBody] TaktOnlineCreateDto dto)
    {
        try
        {
            var result = await _onlineService.CreateOnlineAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:update", "更新在线用户")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOnlineAsync(long id, [FromBody] TaktOnlineUpdateDto dto)
    {
        try
        {
            var result = await _onlineService.UpdateOnlineAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:delete", "删除在线用户")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOnlineByIdAsync(long id)
    {
        try
        {
            await _onlineService.DeleteOnlineByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:delete", "批量删除在线用户")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteOnlineBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _onlineService.DeleteOnlineBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:update", "更新在线用户状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateOnlineStatusAsync([FromBody] TaktOnlineStatusDto dto)
    {
        try
        {
            var result = await _onlineService.UpdateOnlineStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前登录用户在线统计
    /// </summary>
    /// <returns>统计结果</returns>
    [TaktPermission("foundation:online:query", "当前用户在线统计")]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetOnlineStatisticsAsync()
    {
        try
        {
            var result = await _onlineService.GetOnlineStatisticsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:online:export", "导出在线用户")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportOnlineAsync([FromQuery] TaktOnlineQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _onlineService.ExportOnlineAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
