// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsusController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术部门视图控制器
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
/// 设变技术部门视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变技术部门")]
public class TaktEcGijutsusController : TaktControllerBase
{
    private readonly ITaktEcGijutsuService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcGijutsusController(ITaktEcGijutsuService service) => _service = service;

    /// <summary>获取技术部门列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:gijutsu:list", "技术部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcGijutsuListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcGijutsuListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取技术部门行</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:gijutsu:query", "技术部门详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcGijutsuByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcGijutsuByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("技术部门不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新技术部门</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:gijutsu:update", "更新技术部门")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcGijutsuAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcGijutsuAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出技术部门</summary>
    [TaktPermission("logistics:manufacturing:engineeringchange:gijutsu:export", "导出技术部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcGijutsuAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcGijutsuAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
