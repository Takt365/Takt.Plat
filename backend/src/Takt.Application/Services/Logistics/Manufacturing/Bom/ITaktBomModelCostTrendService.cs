// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomModelCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 机种成本推移分析服务（读 BOM 成本本表；与产品推移 / CRUD 分离）。
/// 工厂/机种选项可引用 ITaktBomMaterialCostAnalysisService；物料（X+F 组件）选项由本服务提供。
/// </summary>
public interface ITaktBomModelCostTrendService
{
    /// <summary>
    /// 机种成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    Task<TaktBomModelCostTrendResultDto> GetBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto);

    /// <summary>
    /// 导出机种成本推移分析
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 机种选项（分析：工厂 + 期间最后月头表机种去重；❌ 非 CRUD 主数据 TaktModelDestination）
    /// </summary>
    /// <param name="queryDto">工厂与 FocusPeriod（yyyy-MM）</param>
    /// <returns>下拉选项 DictValue=ModelCode</returns>
    Task<List<TaktSelectOption>> GetBomModelCostTrendModelOptionsAsync(
        TaktBomModelCostTrendOptionsQueryDto queryDto);

    /// <summary>
    /// 物料/组件选项（工厂 + 期间最后月 + ProductionRelated=X + PurchaseType=F + 未删除去重；支持 keyword 远程搜索）
    /// </summary>
    /// <param name="queryDto">工厂、FocusPeriod、可选 Keyword</param>
    /// <returns>下拉选项 DictValue=ComponentCode</returns>
    Task<List<TaktSelectOption>> GetBomModelCostTrendComponentOptionsAsync(
        TaktBomModelCostTrendOptionsQueryDto queryDto);
}
