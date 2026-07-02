// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktPlantSeedData.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂种子数据初始化
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 工厂种子数据初始化
/// 每个租户按 <c>Database:PlantCodes</c> 顺序初始化工厂主档
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktPlantSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在公司种子之后）
    /// </summary>
    public int Order => 6;

    /// <summary>
    /// 初始化工厂种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化工厂种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过工厂种子数据初始化");
            return (0, 0);
        }
        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktPlant>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var configuredPlantCodes = configuration.RequireDatabase().PlantCodes;
        var seedByCode = GetStandardPlants().ToDictionary(s => s.PlantCode, StringComparer.Ordinal);
        int insertCount = 0;
        int updateCount = 0;
        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化工厂数据（顺序: {PlantCodes}）...",
            tenantCode,
            string.Join(", ", configuredPlantCodes));
        for (var index = 0; index < configuredPlantCodes.Count; index++)
        {
            var plantCode = configuredPlantCodes[index];
            if (!seedByCode.TryGetValue(plantCode, out var seedDefinition))
            {
                throw new InvalidOperationException(
                    $"Database:PlantCodes 中的工厂编码 {plantCode} 未定义种子数据");
            }
            var seed = seedDefinition with { SortOrder = index + 1 };
            var (_, inserted, updated) = await CreateOrUpdatePlantAsync(repository, tenantCode, seed);
            insertCount += inserted;
            updateCount += updated;
        }
        TaktLogger.Information("工厂种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准工厂种子目录（实际初始化范围由 Database:PlantCodes 决定）
    /// </summary>
    private static List<TaktPlantSeedItem> GetStandardPlants()
    {
        return
        [
            new TaktPlantSeedItem(
                "0001",
                "Takt Technologies Co., Ltd.",
                "TKC",
                "TKC",
                "zh-CN",
                "0001",
                "150",
                "L",
                "L",
                "软件开发与信息技术服务",
                "中国",
                "北京市",
                "海淀区",
                "北京市海淀区中关村",
                1000000m,
                new DateTime(2010, 1, 1),
                1),
            new TaktPlantSeedItem(
                "1000",
                "ティアック株式会社",
                "TCJ",
                "TCJ",
                "ja-JP",
                "1000",
                "330",
                "C",
                "L",
                "音響機器（ハイエンドオーディオ機器、プレミアムオーディオ機器、音楽制作?業務用オーディオ機器 TASCAM ブランド）の開発?製造?販売並びに情報機器（計測機器、医用画像記録再生機器、機内エンターテインメント機器、産業用光ドライブ、データレコーダー等）の開発?製造?販売及びこれらに関するアフターサービス、ソリューションビジネス",
                "日本",
                "东京都",
                "多摩市",
                "〒206-8530　東京都多摩市落合1丁目47番地",
                2000000m,
                new DateTime(1953, 8, 1),
                2),
            new TaktPlantSeedItem(
                "C100",
                "东莞蒂雅克电子有限公司",
                "DTA",
                "DTA",
                "zh-CN",
                "2300",
                "150",
                "C",
                "L",
                "电子元器件制造；电子元器件批发；电子元器件零售；音响设备制造；音响设备销售；影视录放设备制造；家用视听设备销售；电气信号设备装置制造；电气信号设备装置销售；日用电器修理。",
                "中国",
                "广东省",
                "东莞市",
                "广东省东莞市长安镇上角社区上兴路1号",
                1000000m,
                new DateTime(1996, 3, 16),
                3),
            new TaktPlantSeedItem(
                "H100",
                "TEAC AUDIO (CHINA) CO., LTD.",
                "TAC",
                "TAC",
                "zh-HK",
                "2400",
                "230",
                "F",
                "M",
                "Import/Export of Audio Equipment, Hi-Fi Equipment, Cassette Recorders, TV & Video Equipment and Electronic Components",
                "中国",
                "香港特别行政区",
                "沙田",
                "沙田小瀝源安心街19號匯貿中心8樓9室",
                500000m,
                new DateTime(1995, 12, 1),
                4),
            new TaktPlantSeedItem(
                "2700",
                "蒂雅克商贸(深圳)有限公司",
                "TSZ",
                "TSZ",
                "zh-CN",
                "2700",
                "150",
                "F",
                "M",
                "音响设备及其零配件、电子产品及其零配件、电子元器件的研发、批发、佣金代理（拍卖除外）、进出口及相关配套服务。",
                "中国",
                "广东省",
                "深圳市",
                "深圳市福田区深南大道南泰然九路西喜年中心A座817房",
                500000m,
                new DateTime(2012, 1, 1),
                5),
            new TaktPlantSeedItem(
                "3000",
                "TEAC AMERICA, INC.",
                "TCA",
                "TCA",
                "en-US",
                "3000",
                "330",
                "L",
                "L",
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
    /// 创建或更新工厂
    /// </summary>
    private static async Task<(TaktPlant Plant, int InsertCount, int UpdateCount)> CreateOrUpdatePlantAsync(
        ITaktTenantSeedRepository<TaktPlant> repository,
        string tenantCode,
        TaktPlantSeedItem seed)
    {
        var plant = await repository.FirstAsync(p => p.TenantCode == tenantCode && p.PlantCode == seed.PlantCode);
        if (plant == null)
        {
            plant = new TaktPlant { TenantCode = tenantCode };
            ApplySeed(plant, seed);
            await repository.CreateAsync(plant);
            return (plant, 1, 0);
        }
        ApplySeed(plant, seed);
        await repository.UpdateAsync(plant);
        return (plant, 0, 1);
    }

    /// <summary>
    /// 将种子项写入实体（除 ClosingDate、地址2/3 外均为必填）
    /// </summary>
    private static void ApplySeed(TaktPlant plant, TaktPlantSeedItem seed)
    {
        plant.PlantCode = seed.PlantCode;
        plant.PlantName = seed.PlantName;
        plant.PlantShortName = seed.PlantShortName;
        plant.CodeAlias = seed.CodeAlias;
        plant.DefaultCulture = seed.DefaultCulture;
        plant.EnterpriseNature = seed.EnterpriseNature;
        plant.IndustryAttribute = seed.IndustryAttribute;
        plant.EnterpriseScale = seed.EnterpriseScale;
        plant.BusinessScope = seed.BusinessScope;
        plant.RegistrationRegion = seed.RegistrationRegion;
        plant.RegistrationProvince = seed.RegistrationProvince;
        plant.RegistrationCity = seed.RegistrationCity;
        plant.RegistrationAddress1 = seed.RegistrationAddress1;
        plant.RegistrationAddress2 = null;
        plant.RegistrationAddress3 = null;
        plant.BusinessRegion = seed.RegistrationRegion;
        plant.BusinessProvince = seed.RegistrationProvince;
        plant.BusinessCity = seed.RegistrationCity;
        plant.BusinessAddress1 = seed.RegistrationAddress1;
        plant.BusinessAddress2 = null;
        plant.BusinessAddress3 = null;
        plant.PlantPhone = seed.PlantPhone;
        plant.PlantEmail = seed.PlantEmail;
        plant.PlantFax = seed.PlantFax;
        plant.PlantWebsite = seed.PlantWebsite;
        plant.UnifiedSocialCreditCode = seed.UnifiedSocialCreditCode;
        plant.TaxRegistrationNumber = seed.TaxRegistrationNumber;
        plant.LegalRepresentative = seed.LegalRepresentative;
        plant.PlantManager = seed.PlantManager;
        plant.RegisteredCapital = seed.RegisteredCapital;
        plant.EstablishmentDate = seed.EstablishmentDate;
        plant.ClosingDate = null;
        plant.PlantExistence = 1;
        plant.RelatedCompany = seed.RelatedCompany;
        plant.PlantStatus = 1;
        plant.SortOrder = seed.SortOrder;
    }

    /// <summary>
    /// 工厂种子项（字段与公司种子项对称）
    /// </summary>
    private sealed record TaktPlantSeedItem(
        string PlantCode,
        string PlantName,
        string PlantShortName,
        string CodeAlias,
        string DefaultCulture,
        string RelatedCompany,
        string EnterpriseNature,
        string IndustryAttribute,
        string EnterpriseScale,
        string BusinessScope,
        string RegistrationRegion,
        string RegistrationProvince,
        string RegistrationCity,
        string RegistrationAddress1,
        decimal RegisteredCapital,
        DateTime EstablishmentDate,
        int SortOrder,
        string PlantPhone = "",
        string PlantEmail = "",
        string PlantFax = "",
        string PlantWebsite = "",
        string UnifiedSocialCreditCode = "",
        string TaxRegistrationNumber = "",
        string LegalRepresentative = "",
        string PlantManager = "");
}
