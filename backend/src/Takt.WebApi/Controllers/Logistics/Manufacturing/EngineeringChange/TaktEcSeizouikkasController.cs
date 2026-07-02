// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizouikkasController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造一课视图控制器
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
/// 设变制造一课视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变制造一课")]
public class TaktEcSeizouikkasController : TaktControllerBase
{
    private readonly ITaktEcSeizouikkaService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcSeizouikkasController(ITaktEcSeizouikkaService service) => _service = service;

    /// <summary>获取制造一课列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:list", "制造一课列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeizouikkaListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcSeizouikkaListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取制造一课行</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:query", "制造一课详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcSeizouikkaByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcSeizouikkaByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("制造一课不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新制造一课</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:update", "更新制造一课")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcSeizouikkaAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcSeizouikkaAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出制造一课</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:export", "导出制造一课")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeizouikkaAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcSeizouikkaAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
