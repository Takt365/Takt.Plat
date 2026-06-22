// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeikansController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变生管部门视图控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变生管部门视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变生管部门")]
public class TaktEcSeikansController : TaktControllerBase
{
    private readonly ITaktEcSeikanService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcSeikansController(ITaktEcSeikanService service) => _service = service;

    /// <summary>获取生管部门列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:seikan:list", "生管部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeikanListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcSeikanListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取生管部门行</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:seikan:query", "生管部门详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcSeikanByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcSeikanByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("生管部门不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新生管部门</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:seikan:update", "更新生管部门")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcSeikanAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcSeikanAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出生管部门</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:seikan:export", "导出生管部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeikanAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcSeikanAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
