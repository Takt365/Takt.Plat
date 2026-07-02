// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcUkekensController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变受检部门视图控制器
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
/// 设变受检部门视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变受检部门")]
public class TaktEcUkekensController : TaktControllerBase
{
    private readonly ITaktEcUkekenService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcUkekensController(ITaktEcUkekenService service) => _service = service;

    /// <summary>获取受检部门列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:ukeken:list", "受检部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcUkekenListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcUkekenListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取受检部门行</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:ukeken:query", "受检部门详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcUkekenByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcUkekenByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("受检部门不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新受检部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:ukeken:update", "更新受检部门")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcUkekenAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcUkekenAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出受检部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:ukeken:export", "导出受检部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcUkekenAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcUkekenAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
