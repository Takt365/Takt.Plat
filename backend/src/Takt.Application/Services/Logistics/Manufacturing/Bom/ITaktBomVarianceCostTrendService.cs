// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomVarianceCostTrendService.cs
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 差异成本推移（工厂+期间+机种可多选；有无差异组件的移动单价月度推移）
/// </summary>
public interface ITaktBomVarianceCostTrendService
{
    /// <summary>
    /// 差异成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    Task<TaktBomVarianceCostTrendResultDto> GetBomVarianceCostTrendAnalysisAsync(
        TaktBomVarianceCostTrendQueryDto queryDto);

    /// <summary>
    /// 导出差异成本推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomVarianceCostTrendAnalysisAsync(
        TaktBomVarianceCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 机种选项（工厂 + 期间最后月 + MaterialType）
    /// </summary>
    /// <param name="queryDto">选项查询</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomVarianceCostTrendModelOptionsAsync(
        TaktBomVarianceCostTrendOptionsQueryDto queryDto);

    /// <summary>
    /// 产品选项（工厂 + 期间最后月 + MaterialType + 已选机种；与机种联动）
    /// </summary>
    /// <param name="queryDto">选项查询（须含 ModelCodes/ModelCode）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomVarianceCostTrendProductOptionsAsync(
        TaktBomVarianceCostTrendOptionsQueryDto queryDto);
}
