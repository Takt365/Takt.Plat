// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz
// 文件名称：TaktBomMaterialCostSumJobHandler.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 成本合计（仅 CostingDate 当月；完成后由 Executor 落库消息并推送）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.Json;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;

/// <summary>
/// Quartz：BOM 物料成本合计处理器（依赖 Job 入口已注入的租户/公司上下文）
/// </summary>
public sealed class TaktBomMaterialCostSumJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktBomMaterialCostItemService _bomMaterialCostItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemService">BOM 物料成本明细服务</param>
    public TaktBomMaterialCostSumJobHandler(ITaktBomMaterialCostItemService bomMaterialCostItemService)
    {
        ArgumentNullException.ThrowIfNull(bomMaterialCostItemService);
        _bomMaterialCostItemService = bomMaterialCostItemService;
    }

    /// <inheritdoc />
    public string HandlerKey => nameof(TaktBomMaterialCostSumJobHandler);

    /// <inheritdoc />
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        ArgumentException.ThrowIfNullOrWhiteSpace(task.TenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.CompanyCode);
        var result = await _bomMaterialCostItemService.RunScheduledBomMaterialCostSumAsync(
            force: TryParseForce(context.ExecuteParams),
            asOfDate: DateTime.Today);
        if (result == null)
        {
            context.ExecuteMessage = "成本合计无结果（当月无明细或上下文无效）";
            TaktLogger.Warning(
                "[BomMaterialCost] Quartz 合计无结果 TaskCode={TaskCode} Tenant={Tenant} Company={Company}",
                task.TaskCode,
                task.TenantCode,
                task.CompanyCode);
            return;
        }
        context.ExecuteMessage =
            $"成本合计完成（当月 {result.ProcessedMonth}）：扫描 {result.ScannedRowCount}，刷新 {result.RefreshedGroupCount}，跳过 {result.SkippedGroupCount}";
        TaktLogger.Information(
            "[BomMaterialCost] Quartz 合计完成 Month={Month} Scanned={Scanned} Refreshed={Refreshed} Skipped={Skipped} Tenant={Tenant} Company={Company}",
            result.ProcessedMonth,
            result.ScannedRowCount,
            result.RefreshedGroupCount,
            result.SkippedGroupCount,
            task.TenantCode,
            task.CompanyCode);
    }

    /// <summary>
    /// 解析 ExecuteParams 中的 force（JSON {\"force\":true} 或裸 force）
    /// </summary>
    /// <param name="executeParams">执行参数</param>
    /// <returns>是否强制（兼容旧参数；当前无额外门禁）</returns>
    private static bool TryParseForce(string? executeParams)
    {
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return false;
        }
        var raw = executeParams.Trim();
        if (string.Equals(raw, "force", StringComparison.OrdinalIgnoreCase)
            || string.Equals(raw, "true", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.ValueKind == JsonValueKind.Object
                && doc.RootElement.TryGetProperty("force", out var forceProp))
            {
                return forceProp.ValueKind == JsonValueKind.True
                    || (forceProp.ValueKind == JsonValueKind.String
                        && bool.TryParse(forceProp.GetString(), out var parsed)
                        && parsed);
            }
        }
        catch (JsonException)
        {
            // 非 JSON 参数忽略
        }
        return false;
    }
}
