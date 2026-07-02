// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcSeizougijutsuService.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造技术课部门视图应用服务接口（DeptCode=Te）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制造技术课部门视图应用服务接口
/// </summary>
public interface ITaktEcSeizougijutsuService
{
    /// <summary>获取制造技术课部门列表（分页）</summary>
    Task<TaktPagedResult<TaktEcDeptViewDto>> GetEcSeizougijutsuListAsync(TaktEcDeptViewQueryDto queryDto);
    /// <summary>根据设变明细 ID 获取制造技术课部门行</summary>
    Task<TaktEcDeptViewDto?> GetEcSeizougijutsuByEcDetailIdAsync(long ecDetailId);
    /// <summary>更新制造技术课部门</summary>
    Task<TaktEcDeptViewDto> UpdateEcSeizougijutsuAsync(long ecDetailId, TaktEcDeptViewUpdateDto dto);
    /// <summary>导出制造技术课部门</summary>
    Task<(string fileName, byte[] fileContent)> ExportEcSeizougijutsuAsync(TaktEcDeptViewQueryDto? query = null, string? sheetName = null, string? fileName = null);
}
