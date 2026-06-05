// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：ITaktHolidayThemeService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：假日主题应用服务接口（用户默认公司 + 当日假日）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Attendance;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 假日主题应用服务接口
/// </summary>
public interface ITaktHolidayThemeService
{
    /// <summary>
    /// 获取服务器当日、指定租户与公司下的假日主题色与问候信息
    /// </summary>
    /// <param name="tenantCode">租户编码（登录页须显式传入，与 X-Tenant-Code 一致）</param>
    /// <param name="companyCode">公司编码（登录预览语言接口解析的默认公司）</param>
    /// <returns>假日主题 DTO；无匹配记录时 IsHolidayToday 为 false、HolidayTheme 为空</returns>
    Task<TaktHolidayThemeDto> GetHolidayThemeAsync(string tenantCode, string companyCode);
}
