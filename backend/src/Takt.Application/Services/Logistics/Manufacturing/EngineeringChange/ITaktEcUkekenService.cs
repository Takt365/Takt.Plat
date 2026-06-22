// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcUkekenService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变受检部门视图应用服务接口（DeptCode=Iqc）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变受检部门视图应用服务接口
/// </summary>
public interface ITaktEcUkekenService
{
    /// <summary>获取受检部门列表（分页）</summary>
    Task<TaktPagedResult<TaktEcDeptViewDto>> GetEcUkekenListAsync(TaktEcDeptViewQueryDto queryDto);
    /// <summary>根据设变明细 ID 获取受检部门行</summary>
    Task<TaktEcDeptViewDto?> GetEcUkekenByEcDetailIdAsync(long ecDetailId);
    /// <summary>更新受检部门</summary>
    Task<TaktEcDeptViewDto> UpdateEcUkekenAsync(long ecDetailId, TaktEcDeptViewUpdateDto dto);
    /// <summary>导出受检部门</summary>
    Task<(string fileName, byte[] fileContent)> ExportEcUkekenAsync(TaktEcDeptViewQueryDto? query = null, string? sheetName = null, string? fileName = null);
}
