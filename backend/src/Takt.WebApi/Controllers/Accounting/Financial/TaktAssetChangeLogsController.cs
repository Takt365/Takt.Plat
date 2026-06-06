// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktAssetChangeLogsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：资产变更记录控制器
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
/// 资产变更记录控制器
/// 提供资产变更记录的 REST API
/// </summary>
[ApiModule(TaktModule.Accounting, "财务核算")]
[Route("api/[controller]", Name = "资产变更记录")]
public class TaktAssetChangeLogsController : TaktControllerBase
{
    private readonly ITaktAssetChangeLogService _assetChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assetChangeLogService">资产变更记录服务</param>
    public TaktAssetChangeLogsController(ITaktAssetChangeLogService assetChangeLogService)
    {
        _assetChangeLogService = assetChangeLogService;
    }

    /// <summary>
    /// 获取资产变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:assetchangelog:list", "资产变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssetChangeLogListAsync([FromQuery] TaktAssetChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _assetChangeLogService.GetAssetChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取资产变更记录
    /// </summary>
    /// <param name="id">资产变更记录ID</param>
    /// <returns>资产变更记录DTO</returns>
    [TaktPermission("accounting:financial:assetchangelog:query", "资产变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _assetChangeLogService.GetAssetChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("资产变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取资产变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:assetchangelog:query", "资产变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssetChangeLogOptionsAsync()
    {
        try
        {
            var result = await _assetChangeLogService.GetAssetChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建资产变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>资产变更记录DTO</returns>
    [TaktPermission("accounting:financial:assetchangelog:create", "创建资产变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateAssetChangeLogAsync([FromBody] TaktAssetChangeLogCreateDto dto)
    {
        try
        {
            var result = await _assetChangeLogService.CreateAssetChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产变更记录
    /// </summary>
    /// <param name="id">资产变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>资产变更记录DTO</returns>
    [TaktPermission("accounting:financial:assetchangelog:update", "更新资产变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetChangeLogAsync(long id, [FromBody] TaktAssetChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _assetChangeLogService.UpdateAssetChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除资产变更记录
    /// </summary>
    /// <param name="id">资产变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:assetchangelog:delete", "删除资产变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetChangeLogByIdAsync(long id)
    {
        try
        {
            await _assetChangeLogService.DeleteAssetChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除资产变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:assetchangelog:delete", "批量删除资产变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssetChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assetChangeLogService.DeleteAssetChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出资产变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:assetchangelog:export", "导出资产变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssetChangeLogAsync([FromQuery] TaktAssetChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assetChangeLogService.ExportAssetChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
