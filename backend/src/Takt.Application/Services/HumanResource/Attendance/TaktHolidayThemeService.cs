// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktHolidayThemeService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：假日主题应用服务（租户 + 公司下的当日假日）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 假日主题应用服务
/// </summary>
public class TaktHolidayThemeService : TaktServiceBase, ITaktHolidayThemeService
{
    private readonly ITaktCompanyRepository<TaktHoliday> _holidayRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="holidayRepository">假日信息仓储</param>
    /// <param name="companyRepository">公司仓储（校验租户下公司存在且启用）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktHolidayThemeService(
        ITaktCompanyRepository<TaktHoliday> holidayRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _holidayRepository = holidayRepository;
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// 获取服务器当日、指定租户与公司下的假日主题色与问候信息
    /// </summary>
    /// <param name="tenantCode">租户编码（登录页显式传入；禁止依赖配置默认租户）</param>
    /// <param name="companyCode">公司编码（由登录预览语言接口解析后传入）</param>
    /// <returns>假日主题 DTO</returns>
    public async Task<TaktHolidayThemeDto> GetHolidayThemeAsync(string tenantCode, string companyCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            ThrowBusinessException("租户编码不能为空");
        }

        if (string.IsNullOrWhiteSpace(companyCode))
        {
            ThrowBusinessException("公司编码不能为空");
        }

        var effectiveTenant = tenantCode.Trim();
        var effectiveCompany = companyCode.Trim();
        var empty = new TaktHolidayThemeDto();

        var company = await _companyRepository.FirstAsync(c =>
            c.TenantCode == effectiveTenant
            && c.CompanyCode == effectiveCompany
            && c.CompanyStatus == TaktCommonStatus.Enabled);
        if (company == null)
        {
            return empty;
        }

        var today = DateTime.Now.Date;
        var holidays = await _holidayRepository.GetListAsync(
            h => h.TenantCode == effectiveTenant
                && h.CompanyCode == effectiveCompany
                && h.StartDate.Date <= today
                && h.EndDate.Date >= today,
            h => h.StartDate,
            true);

        var holiday = holidays.FirstOrDefault();
        if (holiday == null)
        {
            return empty;
        }

        var dto = holiday.Adapt<TaktHolidayThemeDto>();
        dto.IsHolidayToday = holiday.IsWorkingDay == TaktHolidayWorkingDay.NonWorkingDay;
        return dto;
    }
}
