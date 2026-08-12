// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktCompanyScopeFillHelper.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级/审批级实体创建时注入 CultureCode、PlantCode（PlantCode 空则取 TaktCompany.RelatedPlant）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Accounting.Financial;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 公司上下文写入辅助：按当前公司主档强制写入 CultureCode / PlantCode。
/// </summary>
public static class TaktCompanyScopeFillHelper
{
    /// <summary>
    /// 按公司主档解析并写入 CultureCode / PlantCode。
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">公司级或审批级实体</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    public static async Task ApplyCompanyScopeFromMasterAsync(
        ISqlSugarClient db,
        object entity,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
        {
            return;
        }
        var row = await db.Queryable<TaktCompany>()
            .Where(c => c.TenantCode == tenantCode && c.CompanyCode == companyCode && c.IsDeleted == 0)
            .Select(c => new { c.CultureCode, c.RelatedPlant })
            .FirstAsync();
        if (row == null)
        {
            return;
        }
        ApplyCompanyScope(
            entity,
            (row.CultureCode ?? string.Empty).Trim(),
            (row.RelatedPlant ?? string.Empty).Trim());
    }

    /// <summary>
    /// 按公司主档写入 CultureCode；PlantCode 仅在当前为空时用公司 RelatedPlant 填充。
    /// </summary>
    /// <param name="entity">公司级或审批级实体</param>
    /// <param name="cultureCode">当前公司 CultureCode</param>
    /// <param name="relatedPlant">当前公司 RelatedPlant（用作默认工厂）</param>
    public static void ApplyCompanyScope(object entity, string cultureCode, string relatedPlant)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var plant = (relatedPlant ?? string.Empty).Trim();
        switch (entity)
        {
            case TaktCompanyEntityBase companyEntity:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyEntity.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyEntity.PlantCode) && plant.Length > 0)
                {
                    companyEntity.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityBase approvalEntity:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalEntity.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalEntity.PlantCode) && plant.Length > 0)
                {
                    approvalEntity.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityGuidBase companyGuid:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyGuid.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyGuid.PlantCode) && plant.Length > 0)
                {
                    companyGuid.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityGuidBase approvalGuid:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalGuid.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalGuid.PlantCode) && plant.Length > 0)
                {
                    approvalGuid.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityIncrementBase companyIncrement:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyIncrement.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(companyIncrement.PlantCode) && plant.Length > 0)
                {
                    companyIncrement.PlantCode = plant;
                }
                break;
            case TaktApprovalEntityIncrementBase approvalIncrement:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalIncrement.CultureCode = cultureCode.Trim();
                }
                if (string.IsNullOrWhiteSpace(approvalIncrement.PlantCode) && plant.Length > 0)
                {
                    approvalIncrement.PlantCode = plant;
                }
                break;
            case TaktCompanyEntityScopeBase companyScope:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    companyScope.CultureCode = cultureCode.Trim();
                }
                break;
            case TaktApprovalEntityScopeBase approvalScope:
                if (!string.IsNullOrWhiteSpace(cultureCode))
                {
                    approvalScope.CultureCode = cultureCode.Trim();
                }
                break;
            default:
                return;
        }
    }
}
