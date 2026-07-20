// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostItemRecalculateBackgroundService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种月平均重算后台任务调度接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本机种月平均重算后台任务调度接口
/// </summary>
public interface ITaktBomMaterialCostItemRecalculateBackgroundService
{
    /// <summary>
    /// 提交后台重算任务（立即返回；完成后通过 SignalR 通知触发用户）
    /// </summary>
    /// <param name="queryDto">已校验的重算查询条件</param>
    /// <param name="forceRecalculate">是否先清零再重算</param>
    /// <param name="processRecordCount">处理记录数上限（工厂+产品组；0=全部；默认 5000）</param>
    /// <returns>任务</returns>
    Task EnqueueRecalculateAsync(
        TaktBomMaterialCostItemQueryDto queryDto,
        bool forceRecalculate = false,
        int processRecordCount = 5000);
}
