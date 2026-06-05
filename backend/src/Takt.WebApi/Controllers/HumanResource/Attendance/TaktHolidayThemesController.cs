// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktHolidayThemesController.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：假日主题控制器（登录前预览当日假日，非匿名登录）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.HumanResource.Attendance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Attendance;

/// <summary>
/// 假日主题控制器
/// 登录前预览：须已校验租户（X-Tenant-Code）；按租户 + 公司编码查询当日假日
/// </summary>
[ApiModule(TaktModule.HumanResource, "考勤管理")]
[Route("api/[controller]", Name = "假日主题")]
public class TaktHolidayThemesController : TaktControllerBase
{
    private readonly ITaktHolidayThemeService _holidayThemeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="holidayThemeService">假日主题服务</param>
    public TaktHolidayThemesController(ITaktHolidayThemeService holidayThemeService)
    {
        _holidayThemeService = holidayThemeService;
    }

    /// <summary>
    /// 登录前预览：指定租户与公司下的当日假日主题（未签发 OAuth 访问令牌，非匿名登录）
    /// </summary>
    /// <param name="tenantCode">租户编码（与 X-Tenant-Code、登录页输入一致）</param>
    /// <param name="companyCode">公司编码（由 session/login-preview-locale 解析）</param>
    /// <returns>假日主题 DTO（字段对齐 TaktHoliday 实体）</returns>
    [AllowAnonymous]
    [HttpGet("theme")]
    public async Task<IActionResult> GetHolidayThemeAsync([FromQuery] string tenantCode, [FromQuery] string companyCode)
    {
        try
        {
            var tenantError = ValidateLoginPreviewTenantHeader(tenantCode);
            if (tenantError != null)
            {
                return tenantError;
            }

            var result = await _holidayThemeService.GetHolidayThemeAsync(tenantCode, companyCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
