// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizounikasController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造二课视图控制器
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
/// 设变制造二课视图控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变制造二课")]
public class TaktEcSeizounikasController : TaktControllerBase
{
    private readonly ITaktEcSeizounikaService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcSeizounikasController(ITaktEcSeizounikaService service) => _service = service;

    /// <summary>获取制造二课列表（分页）</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizounika:list", "制造二课列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeizounikaListAsync([FromQuery] TaktEcDeptViewQueryDto queryDto)
    {
        try { var result = await _service.GetEcSeizounikaListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>根据设变明细 ID 获取制造二课行</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizounika:query", "制造二课详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcSeizounikaByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcSeizounikaByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("制造二课不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>更新制造二课</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizounika:update", "更新制造二课")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcSeizounikaAsync(long ecDetailId, [FromBody] TaktEcDeptViewUpdateDto dto)
    {
        try { var result = await _service.UpdateEcSeizounikaAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>导出制造二课</summary>
    [TaktPermission("logistics:manufacturing:engineering:change:seizounika:export", "导出制造二课")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeizounikaAsync([FromQuery] TaktEcDeptViewQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcSeizounikaAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
