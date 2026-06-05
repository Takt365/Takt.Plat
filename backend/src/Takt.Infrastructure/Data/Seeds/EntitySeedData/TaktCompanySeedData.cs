// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktCompanySeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：公司种子数据初始化（幂等性：存在则更新，不存在则创建）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 公司种子数据初始化
/// 各租户（<c>TenantCodes</c> 如 000/100/500）均按 <c>Database:CompanyCodes</c> 顺序初始化全部公司主档
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktCompanySeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在租户之后，员工档案之前）
    /// </summary>
    public int Order => 5;

    /// <summary>
    /// 初始化公司种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化公司种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过公司种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var configuredCompanyCodes = configuration.RequireDatabase().CompanyCodes;
        var seedByCode = GetStandardCompanies().ToDictionary(s => s.CompanyCode, StringComparer.Ordinal);
        int insertCount = 0;
        int updateCount = 0;
        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化公司数据（顺序: {CompanyCodes}）...",
            tenantCode,
            string.Join(", ", configuredCompanyCodes));
        for (var index = 0; index < configuredCompanyCodes.Count; index++)
        {
            var companyCode = configuredCompanyCodes[index];
            if (!seedByCode.TryGetValue(companyCode, out var seedDefinition))
            {
                throw new InvalidOperationException(
                    $"Database:CompanyCodes 中的公司编码 {companyCode} 未定义种子数据");
            }
            var seed = seedDefinition with { SortOrder = index + 1 };
            var (_, inserted, updated) = await CreateOrUpdateCompanyAsync(repository, tenantCode, seed);
            insertCount += inserted;
            updateCount += updated;
        }
        TaktLogger.Information("公司种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准公司种子目录（实际初始化范围由 Database:CompanyCodes 决定）
    /// </summary>
    private static List<TaktCompanySeedItem> GetStandardCompanies()
    {
        return
        [
            new TaktCompanySeedItem(
                "0001",
                "Takt Technologies Co., Ltd.",
                "TKC",
                "TKC",
                "zh-CN",
                TaktCompanyType.Research,
                "0001",
                TaktEnterpriseNature.DomesticLimitedLiability,
                TaktIndustryAttribute.L,
                TaktEnterpriseScale.Large,
                "软件开发与信息技术服务",
                "中国",
                "北京市",
                "海淀区",
                "北京市海淀区中关村",
                1000000m,
                new DateTime(2010, 1, 1),
                1),
            new TaktCompanySeedItem(
                "1000",
                "ティアック株式会社",
                "TCJ",
                "TCJ",
                "ja-JP",
                TaktCompanyType.Manufacturing,
                "1000",
                TaktEnterpriseNature.ForeignWhollyOwned,
                TaktIndustryAttribute.C,
                TaktEnterpriseScale.Large,
                "音響機器（ハイエンドオーディオ機器、プレミアムオーディオ機器、音楽制作?業務用オーディオ機器 TASCAM ブランド）の開発?製造?販売並びに情報機器（計測機器、医用画像記録再生機器、機内エンターテインメント機器、産業用光ドライブ、データレコーダー等）の開発?製造?販売及びこれらに関するアフターサービス、ソリューションビジネス",
                "日本",
                "东京都",
                "多摩市",
                "〒206-8530　東京都多摩市落合1丁目47番地",
                2000000m,
                new DateTime(1953, 8, 1),
                2),
            new TaktCompanySeedItem(
                "2300",
                "东莞蒂雅克电子有限公司",
                "DTA",
                "DTA",
                "zh-CN",
                TaktCompanyType.Manufacturing,
                "C100",
                TaktEnterpriseNature.DomesticLimitedLiability,
                TaktIndustryAttribute.C,
                TaktEnterpriseScale.Large,
                "电子元器件制造；电子元器件批发；电子元器件零售；音响设备制造；音响设备销售；影视录放设备制造；家用视听设备销售；电气信号设备装置制造；电气信号设备装置销售；日用电器修理。",
                "中国",
                "广东省",
                "东莞市",
                "广东省东莞市长安镇上角社区上兴路1号",
                1000000m,
                new DateTime(1996, 3, 16),
                3),
            new TaktCompanySeedItem(
                "2400",
                "TEAC AUDIO (CHINA) CO., LTD.",
                "TAC",
                "TAC",
                "zh-HK",
                TaktCompanyType.Sales,
                "H100",
                TaktEnterpriseNature.HmtWhollyOwned,
                TaktIndustryAttribute.F,
                TaktEnterpriseScale.Medium,
                "Import/Export of Audio Equipment, Hi-Fi Equipment, Cassette Recorders, TV & Video Equipment and Electronic Components",
                "中国",
                "香港特别行政区",
                "沙田",
                "沙田小瀝源安心街19號匯貿中心8樓9室",
                500000m,
                new DateTime(1995, 12, 1),
                4),
            new TaktCompanySeedItem(
                "2700",
                "蒂雅克商贸(深圳)有限公司",
                "TSZ",
                "TSZ",
                "zh-CN",
                TaktCompanyType.Sales,
                "2700",
                TaktEnterpriseNature.DomesticLimitedLiability,
                TaktIndustryAttribute.F,
                TaktEnterpriseScale.Medium,
                "音响设备及其零配件、电子产品及其零配件、电子元器件的研发、批发、佣金代理（拍卖除外）、进出口及相关配套服务。",
                "中国",
                "广东省",
                "深圳市",
                "深圳市福田区深南大道南泰然九路西喜年中心A座817房",
                500000m,
                new DateTime(2012, 1, 1),
                5),
            new TaktCompanySeedItem(
                "3000",
                "TEAC AMERICA, INC.",
                "TCA",
                "TCA",
                "en-US",
                TaktCompanyType.Headquarters,
                "3000",
                TaktEnterpriseNature.ForeignWhollyOwned,
                TaktIndustryAttribute.L,
                TaktEnterpriseScale.Large,
                "Engaged in the import, distribution, marketing and sales of high-fidelity audio/video electronics, consumer electronics, professional audio recording equipment (TASCAM brand), high-end audiophile components (Esoteric brand), computer data storage devices, disc publishing products, and instrumentation/data recorders. Also acts as the North American sales and service headquarters for TEAC Group products.",
                "美国",
                "California",
                "Santa Fe Springs",
                "14525 Valley View Ave., Suite I, Santa Fe Springs, California 90670, U.S.A",
                3000000m,
                new DateTime(1967, 5, 1),
                6),
        ];
    }

    /// <summary>
    /// 创建或更新公司
    /// </summary>
    private static async Task<(TaktCompany Company, int InsertCount, int UpdateCount)> CreateOrUpdateCompanyAsync(
        ITaktTenantSeedRepository<TaktCompany> repository,
        string tenantCode,
        TaktCompanySeedItem seed)
    {
        var company = await repository.FirstAsync(c => c.TenantCode == tenantCode && c.CompanyCode == seed.CompanyCode);

        if (company == null)
        {
            company = new TaktCompany { TenantCode = tenantCode };
            ApplySeed(company, seed);
            await repository.CreateAsync(company);
            return (company, 1, 0);
        }

        ApplySeed(company, seed);
        await repository.UpdateAsync(company);
        return (company, 0, 1);
    }

    /// <summary>
    /// 将种子项写入实体（除 ClosingDate、地址2/3 外均为必填）
    /// </summary>
    private static void ApplySeed(TaktCompany company, TaktCompanySeedItem seed)
    {
        company.CompanyCode = seed.CompanyCode;
        company.CompanyName = seed.CompanyName;
        company.CompanyShortName = seed.CompanyShortName;
        company.CodeAlias = seed.CodeAlias;
        company.DefaultCulture = seed.DefaultCulture;
        company.CompanyType = seed.CompanyType;
        company.RelatedPlant = seed.RelatedPlant;
        company.EnterpriseNature = seed.EnterpriseNature;
        company.IndustryAttribute = seed.IndustryAttribute;
        company.EnterpriseScale = seed.EnterpriseScale;
        company.BusinessScope = seed.BusinessScope;
        company.RegistrationRegion = seed.RegistrationRegion;
        company.RegistrationProvince = seed.RegistrationProvince;
        company.RegistrationCity = seed.RegistrationCity;
        company.RegistrationAddress1 = seed.RegistrationAddress1;
        company.RegistrationAddress2 = null;
        company.RegistrationAddress3 = null;
        company.BusinessRegion = seed.RegistrationRegion;
        company.BusinessProvince = seed.RegistrationProvince;
        company.BusinessCity = seed.RegistrationCity;
        company.BusinessAddress1 = seed.RegistrationAddress1;
        company.BusinessAddress2 = null;
        company.BusinessAddress3 = null;
        company.CompanyPhone = seed.CompanyPhone;
        company.CompanyEmail = seed.CompanyEmail;
        company.CompanyFax = seed.CompanyFax;
        company.CompanyWebsite = seed.CompanyWebsite;
        company.UnifiedSocialCreditCode = seed.UnifiedSocialCreditCode;
        company.TaxRegistrationNumber = seed.TaxRegistrationNumber;
        company.LegalRepresentative = seed.LegalRepresentative;
        company.CompanyManager = seed.CompanyManager;
        company.RegisteredCapital = seed.RegisteredCapital;
        company.EstablishmentDate = seed.EstablishmentDate;
        company.ClosingDate = null;
        company.CompanyExistence = TaktCompanyExistenceStatus.Subsisting;
        company.CompanyStatus = TaktCommonStatus.Enabled;
        company.SortOrder = seed.SortOrder;
    }

    /// <summary>
    /// 公司种子项
    /// </summary>
    private sealed record TaktCompanySeedItem(
        string CompanyCode,
        string CompanyName,
        string CompanyShortName,
        string CodeAlias,
        string DefaultCulture,
        TaktCompanyType CompanyType,
        string RelatedPlant,
        TaktEnterpriseNature EnterpriseNature,
        TaktIndustryAttribute IndustryAttribute,
        TaktEnterpriseScale EnterpriseScale,
        string BusinessScope,
        string RegistrationRegion,
        string RegistrationProvince,
        string RegistrationCity,
        string RegistrationAddress1,
        decimal RegisteredCapital,
        DateTime EstablishmentDate,
        int SortOrder,
        string CompanyPhone = "",
        string CompanyEmail = "",
        string CompanyFax = "",
        string CompanyWebsite = "",
        string UnifiedSocialCreditCode = "",
        string TaxRegistrationNumber = "",
        string LegalRepresentative = "",
        string CompanyManager = "");
}
