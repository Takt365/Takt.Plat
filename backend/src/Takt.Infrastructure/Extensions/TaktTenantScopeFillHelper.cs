// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktTenantScopeFillHelper.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：租户级实体创建/种子时注入 RelatedPlant、CultureCode（空则取租户下首个公司；TaktPlant 跳过 RelatedPlant）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Logistics.Materials;
namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 租户上下文写入辅助：RelatedPlant / CultureCode 为空时按公司主档补齐。
/// </summary>
public static class TaktTenantScopeFillHelper
{
    /// <summary>
    /// 按公司主档解析并写入 RelatedPlant、CultureCode（仅当前为空时填充）。
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">租户级实体</param>
    public static async Task ApplyRelatedPlantAsync(ISqlSugarClient db, TaktTenantEntityBase entity)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        await ApplyTenantScopeFieldsAsync(db, entity).ConfigureAwait(false);
    }

    /// <summary>
    /// 按公司主档解析并写入 RelatedPlant、CultureCode（仅当前为空时填充）。
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="entity">租户级实体</param>
    public static async Task ApplyTenantScopeFieldsAsync(ISqlSugarClient db, TaktTenantEntityBase entity)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(entity);
        var needPlant = entity is not TaktPlant && string.IsNullOrWhiteSpace(entity.RelatedPlant);
        var needCulture = NeedsCultureFill(entity);
        if (!needPlant && !needCulture)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(entity.TenantCode))
        {
            return;
        }
        var row = await db.Queryable<TaktCompany>()
            .Where(c => c.TenantCode == entity.TenantCode && c.IsDeleted == 0)
            .OrderBy(c => c.SortOrder)
            .Select(c => new { c.RelatedPlant, c.CultureCode })
            .FirstAsync()
            .ConfigureAwait(false);
        if (row == null)
        {
            return;
        }
        if (needPlant && !string.IsNullOrWhiteSpace(row.RelatedPlant))
        {
            entity.RelatedPlant = row.RelatedPlant.Trim();
        }
        if (needCulture && !string.IsNullOrWhiteSpace(row.CultureCode))
        {
            entity.CultureCode = row.CultureCode.Trim();
        }
    }

    /// <summary>
    /// 是否需补 CultureCode（DictData 空串=全局通用，禁止用公司码覆盖）。
    /// </summary>
    /// <param name="entity">租户实体</param>
    /// <returns>是否需要填充</returns>
    private static bool NeedsCultureFill(TaktTenantEntityBase entity)
    {
        if (entity is TaktDictData)
        {
            return false;
        }
        return string.IsNullOrWhiteSpace(entity.CultureCode);
    }
}
