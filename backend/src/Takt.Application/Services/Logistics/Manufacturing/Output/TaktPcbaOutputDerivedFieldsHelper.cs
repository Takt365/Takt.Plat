// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDerivedFieldsHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报主表派生字段计算（标准工时/标准点数/标准产能回填）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报主表派生字段计算辅助
/// </summary>
internal static class TaktPcbaOutputDerivedFieldsHelper
{
    /// <summary>
    /// 汇总标准工序时间得到 PCBA 标准点数
    /// </summary>
    /// <param name="operationTimes">标准工序时间列表</param>
    /// <returns>标准点数</returns>
    public static int CalculateStdShortsFromOperationTimes(IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        var effectiveRows = TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(operationTimes);
        return TaktStandardOperationTimeEffectiveResolver.CalculateStdShorts(effectiveRows);
    }

    /// <summary>
    /// 回填标准工时、标准点数并计算标准产能
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工序时间仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="entity">PCBA日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task ApplyDerivedFieldsAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        TaktPcbaOutput entity,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var operationTimes = await TaktAssyOutputDerivedFieldsHelper.ResolveStandardOperationTimesByMaterialAsync(
            standardOperationTimeRepository,
            tenantCode,
            companyCode,
            entity.MaterialCode,
            entity.PlantCode,
            entity.ProdDate);
        entity.StdMinutes = TaktAssyOutputDerivedFieldsHelper.CalculateStdMinutesFromOperationTimes(operationTimes);
        entity.StdShorts = CalculateStdShortsFromOperationTimes(operationTimes);
        var operationRatePercent = await TaktAssyOutputDerivedFieldsHelper.ResolvePersonnelOperationRatePercentAsync(
            standardOperationRateRepository,
            tenantCode,
            companyCode,
            entity.PlantCode,
            entity.ProdDate);
        entity.StdCapacity = TaktProductionStatHelper.CalculateAssyStdCapacity(
            entity.DirectLabor,
            entity.StdMinutes,
            operationRatePercent);
    }
}
