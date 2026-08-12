// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz
// 文件名称：TaktBomModelAvgCostJobHandler.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 机种平均成本（ExecuteParams 可指定 costingPeriod；空则当月）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;

/// <summary>
/// Quartz：BOM 机种平均成本处理器（依赖 Job 入口已注入的租户/公司上下文）
/// </summary>
public sealed class TaktBomModelAvgCostJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktBomMaterialCostAnalysisService _bomMaterialCostAnalysisService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostAnalysisService">BOM 成本分析服务（机种月均）</param>
    public TaktBomModelAvgCostJobHandler(ITaktBomMaterialCostAnalysisService bomMaterialCostAnalysisService)
    {
        ArgumentNullException.ThrowIfNull(bomMaterialCostAnalysisService);
        _bomMaterialCostAnalysisService = bomMaterialCostAnalysisService;
    }

    /// <inheritdoc />
    public string HandlerKey => nameof(TaktBomModelAvgCostJobHandler);

    /// <inheritdoc />
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        ArgumentException.ThrowIfNullOrWhiteSpace(task.TenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.CompanyCode);
        var asOfDate = TaktBomQuartzExecuteParamsHelper.ResolveAsOfDate(context.ExecuteParams);
        var result = await _bomMaterialCostAnalysisService.RunScheduledBomModelAvgCostAsync(asOfDate: asOfDate);
        if (result == null)
        {
            context.ExecuteMessage = "机种平均成本无结果（目标月无主表行或上下文无效）";
            TaktLogger.Warning(
                "[BomModelAvgCost] Quartz 无结果 TaskCode={TaskCode} Tenant={Tenant} Company={Company}",
                task.TaskCode,
                task.TenantCode,
                task.CompanyCode);
            return;
        }
        context.ExecuteMessage =
            $"机种平均成本完成（期间 {result.CostingPeriod}）：扫描 {result.ScannedRowCount}，机种更新 {result.ModelCodeUpdatedCount}，物料类型更新 {result.MaterialTypeUpdatedCount}，月均更新 {result.AverageUpdatedCount}，机种组 {result.ModelGroupCount}";
        TaktLogger.Information(
            "[BomModelAvgCost] Quartz 完成 Period={Period} Scanned={Scanned} ModelUpdated={ModelUpdated} TypeUpdated={TypeUpdated} AvgUpdated={AvgUpdated} Groups={Groups} Tenant={Tenant} Company={Company}",
            result.CostingPeriod,
            result.ScannedRowCount,
            result.ModelCodeUpdatedCount,
            result.MaterialTypeUpdatedCount,
            result.AverageUpdatedCount,
            result.ModelGroupCount,
            task.TenantCode,
            task.CompanyCode);
    }
}
