// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsusController.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造技术课部门视图控制器
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
/// 设变制造技术课部门视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变制造技术课部门")]
public class TaktEcSeizougijutsusController : TaktControllerBase
{
    private readonly ITaktEcSeizougijutsuService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcSeizougijutsusController(ITaktEcSeizougijutsuService service) => _service = service;

    /// <summary>获取制造技术课部门列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:list", "制造技术课部门列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeizougijutsuListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcSeizougijutsuListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取制造技术课部门行</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:query", "制造技术课部门详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcSeizougijutsuByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcSeizougijutsuByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("制造技术课部门不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新制造技术课部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:update", "更新制造技术课部门")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcSeizougijutsuAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcSeizougijutsuAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出制造技术课部门</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:export", "导出制造技术课部门")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeizougijutsuAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcSeizougijutsuAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
