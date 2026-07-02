// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktIsoCodeSeedData.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：ISO 编码种子数据，标准单字母编码清单（11 项），名称与组织架构 DeptCode 对照
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// ISO 编码种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// IsoName 为业务标准名称（与 TaktDept 对照见 Description）；IsoCode 为单字母编码、租户+类别内唯一；清单 11 项均为内置
/// </summary>
public class TaktIsoCodeSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在部门种子之后、岗位之前）
    /// </summary>
    public int Order => 31;

    /// <summary>
    /// 初始化 ISO 编码种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 ISO 编码种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过 ISO 编码种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktIsoCode>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ISO 编码数据...", tenantCode);

        foreach (var seed in GetStandardIsoCodes())
        {
            var (_, i, u) = await CreateOrUpdateIsoCodeAsync(repository, tenantCode, seed);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("ISO 编码种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准 ISO 编码（11 项，按 SourceDeptCode 排序）：D0100总务 … D0800品保
    /// SourceDeptCode 为主对照 DeptCode；RelatedDeptCode 可选，写入 Description 便于幂等更新
    /// </summary>
    private static List<TaktIsoCodeSeedItem> GetStandardIsoCodes()
    {
        return
        [
            new TaktIsoCodeSeedItem("R", "总务部", 1, "D0100"),
            new TaktIsoCodeSeedItem("F", "财务部", 2, "D0200"),
            new TaktIsoCodeSeedItem("D", "IT部", 3, "D0300"),
            new TaktIsoCodeSeedItem("M", "文管中心", 4, "D0410"),
            new TaktIsoCodeSeedItem("S", "生管课", 5, "D0420"),
            new TaktIsoCodeSeedItem("B", "部管课", 6, "D0430"),
            new TaktIsoCodeSeedItem("C", "资材部", 7, "D0500"),
            new TaktIsoCodeSeedItem("Z", "制造部", 8, "D0600"),
            new TaktIsoCodeSeedItem("P", "制技部", 9, "D0630"),
            new TaktIsoCodeSeedItem("T", "技术部", 10, "D0700"),
            new TaktIsoCodeSeedItem("Q", "品保部", 11, "D0800"),
        ];
    }

    /// <summary>
    /// 创建或更新 ISO 编码
    /// </summary>
    private static async Task<(TaktIsoCode IsoCode, int InsertCount, int UpdateCount)> CreateOrUpdateIsoCodeAsync(
        ITaktTenantSeedRepository<TaktIsoCode> repository,
        string tenantCode,
        TaktIsoCodeSeedItem seed)
    {
        var description = BuildDescription(seed.SourceDeptCode, seed.RelatedDeptCode);

        var isoCode = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.IsoCodeCategory == seed.IsoCodeCategory
            && x.IsoCodeDescription == description);

        isoCode ??= await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.IsoCodeCategory == seed.IsoCodeCategory
            && x.IsoCode == seed.IsoCode);

        isoCode ??= await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.IsoCodeCategory == seed.IsoCodeCategory
            && x.IsoName == seed.IsoName);

        if (isoCode == null && !string.IsNullOrWhiteSpace(seed.RelatedDeptCode))
        {
            var relatedDescription = BuildDescription(seed.RelatedDeptCode);
            isoCode = await repository.FirstAsync(x =>
                x.TenantCode == tenantCode
                && x.IsoCodeCategory == seed.IsoCodeCategory
                && x.IsoCodeDescription == relatedDescription);
        }

        if (isoCode == null)
        {
            isoCode = new TaktIsoCode
            {
                TenantCode = tenantCode,
            };
            ApplySeedFields(isoCode, seed, description);
            await repository.CreateAsync(isoCode);
            return (isoCode, 1, 0);
        }

        ApplySeedFields(isoCode, seed, description);
        await repository.UpdateAsync(isoCode);
        return (isoCode, 0, 1);
    }

    /// <summary>
    /// 写入标准种子字段（清单项均为内置、启用）
    /// </summary>
    private static void ApplySeedFields(TaktIsoCode isoCode, TaktIsoCodeSeedItem seed, string description)
    {
        isoCode.IsoCodeCategory = seed.IsoCodeCategory;
        isoCode.IsoCode = seed.IsoCode;
        isoCode.IsoName = seed.IsoName;
        isoCode.SortOrder = seed.SortOrder;
        isoCode.IsoCodeDescription = description;
        isoCode.IsBuiltIn = 1;
        isoCode.IsoCodeStatus = 1;
    }

    /// <summary>
    /// 构建与组织架构的对照说明
    /// </summary>
    /// <param name="sourceDeptCode">主对照 DeptCode</param>
    /// <param name="relatedDeptCode">关联 DeptCode（如技术部/制技部共用编码 T）</param>
    private static string BuildDescription(string sourceDeptCode, string? relatedDeptCode = null)
    {
        if (string.IsNullOrWhiteSpace(relatedDeptCode))
        {
            return $"对应 TaktDept.DeptCode={sourceDeptCode}";
        }
        return $"对应 TaktDept.DeptCode={sourceDeptCode}；关联={relatedDeptCode}";
    }

    /// <summary>
    /// ISO 编码种子项
    /// </summary>
    /// <param name="IsoCode">ISO 单字母标准编码（大写、租户+类别内唯一）</param>
    /// <param name="IsoName">业务标准名称（文管中心、制造部、业务部等与组织架构对照见 Description）</param>
    /// <param name="SortOrder">排序号</param>
    /// <param name="SourceDeptCode">主对照组织架构 DeptCode</param>
    /// <param name="IsoCodeCategory">编码类别（字典 sys_iso_code_category；部门类默认为 1）</param>
    /// <param name="RelatedDeptCode">关联 DeptCode（可选）</param>
    private sealed record TaktIsoCodeSeedItem(
        string IsoCode,
        string IsoName,
        int SortOrder,
        string SourceDeptCode,
        int IsoCodeCategory = 1,
        string? RelatedDeptCode = null);
}
