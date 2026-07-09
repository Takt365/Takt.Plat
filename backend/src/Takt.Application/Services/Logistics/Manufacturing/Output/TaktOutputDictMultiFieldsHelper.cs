// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktOutputDictMultiFieldsHelper.cs
// 功能描述：产出日报多选字典字段规范化（停线/未达成原因等；组立/PCBA 共用）：sortOrder 排序、Label↔Value
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Foundation;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 产出日报多选字典排序映射（停线原因、未达成原因）
/// </summary>
internal sealed class TaktOutputDictMultiSortMaps
{
    /// <summary>
    /// 停线原因 DictValue → SortOrder
    /// </summary>
    public IReadOnlyDictionary<string, int> DowntimeReasonSortOrder { get; init; } =
        new Dictionary<string, int>(StringComparer.Ordinal);

    /// <summary>
    /// 未达成原因 DictValue → SortOrder
    /// </summary>
    public IReadOnlyDictionary<string, int> UnachievedReasonSortOrder { get; init; } =
        new Dictionary<string, int>(StringComparer.Ordinal);
}

/// <summary>
/// 产出日报停线/未达成原因等多选字典字段规范化（组立日报、PCBA 日报等共用）
/// </summary>
internal static class TaktOutputDictMultiFieldsHelper
{
    /// <summary>
    /// 加载停线/未达成字典快照与 DictValue 排序映射
    /// </summary>
    /// <param name="dictDataRepository">字典数据仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>快照与排序映射</returns>
    public static async Task<(TaktDictSnapshot Snapshot, TaktOutputDictMultiSortMaps Maps)> LoadAsync(
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentNullException.ThrowIfNull(dictDataRepository);
        var list = await dictDataRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && (x.DictTypeCode == TaktManufacturingOutputDictTypeConstants.StopReasonCategory
                    || x.DictTypeCode == TaktManufacturingOutputDictTypeConstants.NonachievementReasonCategory),
            x => x.SortOrder,
            false);
        var downtime = new Dictionary<string, int>(StringComparer.Ordinal);
        var unachieved = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.DictValue) || string.IsNullOrWhiteSpace(item.DictTypeCode))
            {
                continue;
            }
            var value = item.DictValue.Trim();
            if (item.DictTypeCode == TaktManufacturingOutputDictTypeConstants.StopReasonCategory)
            {
                downtime[value] = item.SortOrder;
            }
            else if (item.DictTypeCode == TaktManufacturingOutputDictTypeConstants.NonachievementReasonCategory)
            {
                unachieved[value] = item.SortOrder;
            }
        }
        var rows = list.Select(x => (x.DictTypeCode, x.DictValue, x.DictLabel));
        var snapshot = TaktDictSnapshot.CreateFromRows(
            rows,
            TaktManufacturingOutputDictTypeConstants.StopReasonCategory,
            TaktManufacturingOutputDictTypeConstants.NonachievementReasonCategory);
        var maps = new TaktOutputDictMultiSortMaps
        {
            DowntimeReasonSortOrder = downtime,
            UnachievedReasonSortOrder = unachieved,
        };
        return (snapshot, maps);
    }

    /// <summary>
    /// 加载停线/未达成两类字典的 DictValue 排序映射
    /// </summary>
    /// <param name="dictDataRepository">字典数据仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>排序映射</returns>
    public static async Task<TaktOutputDictMultiSortMaps> LoadSortMapsAsync(
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        string tenantCode)
    {
        var (_, maps) = await LoadAsync(dictDataRepository, tenantCode);
        return maps;
    }

    /// <summary>
    /// 规范化停线/未达成原因：统一为 DictLabel 逗号分隔并按 sortOrder 排序（输入可为 DictLabel 或 DictValue）
    /// </summary>
    /// <param name="downtimeReason">停线原因原始值</param>
    /// <param name="unachievedReason">未达成原因原始值</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="maps">排序映射</param>
    /// <returns>规范化后的字段对</returns>
    public static (string? DowntimeReason, string? UnachievedReason) NormalizeFields(
        string? downtimeReason,
        string? unachievedReason,
        TaktDictSnapshot snapshot,
        TaktOutputDictMultiSortMaps maps)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(maps);
        return (
            TaktDictMultiValueHelper.NormalizeCommaSeparatedDictStorage(
                downtimeReason,
                snapshot,
                TaktManufacturingOutputDictTypeConstants.StopReasonCategory,
                storeAsLabel: true,
                maps.DowntimeReasonSortOrder),
            TaktDictMultiValueHelper.NormalizeCommaSeparatedDictStorage(
                unachievedReason,
                snapshot,
                TaktManufacturingOutputDictTypeConstants.NonachievementReasonCategory,
                storeAsLabel: true,
                maps.UnachievedReasonSortOrder));
    }

    /// <summary>
    /// 停线/未达成原因转为 DictLabel 逗号分隔（导出或外部引用存 Label 时使用）
    /// </summary>
    /// <param name="downtimeReason">停线原因 DictValue 串</param>
    /// <param name="unachievedReason">未达成原因 DictValue 串</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="maps">排序映射</param>
    /// <returns>DictLabel 字段对</returns>
    public static (string? DowntimeReasonLabels, string? UnachievedReasonLabels) ToLabelFields(
        string? downtimeReason,
        string? unachievedReason,
        TaktDictSnapshot snapshot,
        TaktOutputDictMultiSortMaps maps)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(maps);
        return (
            TaktDictMultiValueHelper.ConvertCommaSeparatedToLabels(
                downtimeReason,
                snapshot,
                TaktManufacturingOutputDictTypeConstants.StopReasonCategory,
                maps.DowntimeReasonSortOrder),
            TaktDictMultiValueHelper.ConvertCommaSeparatedToLabels(
                unachievedReason,
                snapshot,
                TaktManufacturingOutputDictTypeConstants.NonachievementReasonCategory,
                maps.UnachievedReasonSortOrder));
    }
}
