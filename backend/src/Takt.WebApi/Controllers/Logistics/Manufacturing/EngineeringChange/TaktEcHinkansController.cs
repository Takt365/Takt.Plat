// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkansController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变品管部门视图控制器
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
/// 设变品管部门视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变品管部门")]
public class TaktEcHinkansController : TaktControllerBase
{
    private readonly ITaktEcHinkanService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcHinkansController(ITaktEcHinkanService service) => _service = service;

    /// <summary>获取品管部门列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:list", "品管部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcHinkanListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcHinkanListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取品管部门行</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:query", "品管部门详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcHinkanByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcHinkanByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("品管部门不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新品管部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:update", "更新品管部门")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcHinkanAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcHinkanAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出品管部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:export", "导出品管部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcHinkanAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcHinkanAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
