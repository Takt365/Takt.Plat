// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostAnalysisService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析服务接口（含三页共用工厂/机种/物料级联选项）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析服务（转置 / 差异 / 月度涨跌；
/// 工厂→机种→物料级联选项供成本分析 / 产品推移 / 机种推移三页共用）
/// </summary>
public interface ITaktBomMaterialCostAnalysisService
{
    /// <summary>
    /// 查询栏工厂选项（级联第 1 级）：仅当前公司 RelatedPlant，且须存在于本表 PlantCode
    /// </summary>
    /// <returns>下拉选项（通常 0～1 项；DictValue=PlantCode）</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisPlantOptionsAsync();

    /// <summary>
    /// 查询栏物料类型去重选项（本表 MaterialType；须工厂）
    /// <para>返回该工厂下全部类型（FERT/HALB/…），不做默认截断；前端拉全量后再默认选中 FERT。</para>
    /// <para>❌ 非字典 logistics_material_type（CRUD 表单专用）。</para>
    /// </summary>
    /// <param name="queryDto">须 PlantCode</param>
    /// <returns>DictValue/DictLabel=MaterialType；PlantCode 空则空列表</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisMaterialTypeOptionsAsync(
        TaktBomMaterialCostAnalysisMaterialTypeOptionsQueryDto queryDto);

    /// <summary>
    /// 查询栏机种去重选项（本表 ModelCode；须工厂）
    /// <para>MaterialType 有值才按类型过滤，空=该工厂全部机种。DictLabel 优先型号目的地机种名。</para>
    /// <para>❌ 非 CRUD 主数据 TaktModelDestination / TaktBomMaterialCosts/model-options。</para>
    /// </summary>
    /// <param name="queryDto">机种选项查询（PlantCode 必填；MaterialType 可选）</param>
    /// <returns>下拉选项（DictValue=ModelCode；DictLabel=机种名或编码）</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisModelOptionsAsync(
        TaktBomMaterialCostAnalysisModelOptionsQueryDto queryDto);

    /// <summary>
    /// 查询栏产品编码去重选项（本表 ProductCode；须工厂）
    /// <para>仅本表 ProductCode；❌ 非 MaterialPlant、非物料类型字典。MaterialType/ModelCode 可空。</para>
    /// </summary>
    /// <param name="queryDto">产品选项查询（PlantCode 必填）</param>
    /// <returns>DictValue=ProductCode；ExtValue=ModelCode；ExtLabel=MaterialType</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostAnalysisProductOptionsAsync(
        TaktBomMaterialCostAnalysisProductOptionsQueryDto queryDto);

    /// <summary>
    /// 获取成本分析转置列表（产品 × 核算月成本矩阵 + 环比涨跌）
    /// </summary>
    /// <param name="queryDto">转置查询 DTO</param>
    /// <returns>分页转置行、期间列、可选机种汇总与合计</returns>
    Task<TaktBomMaterialCostAnalysisTransposedResultDto> GetBomMaterialCostAnalysisTransposedListAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto);

    /// <summary>
    /// 导出成本分析转置 Excel（筛选命中的全部行，不截断）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisTransposedAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取成本分析差异（单产品两核算月组件级对比）
    /// </summary>
    /// <param name="queryDto">差异查询 DTO</param>
    /// <returns>汇总与组件差异明细</returns>
    Task<TaktBomMaterialCostAnalysisVarianceResultDto> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto queryDto);

    /// <summary>
    /// 导出成本分析差异 Excel（汇总 + 明细双表）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">明细工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取成本分析月度涨跌（机种下单产品或产品平均月成本序列）
    /// </summary>
    /// <param name="queryDto">月度涨跌查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    Task<TaktBomMaterialCostAnalysisMonthlyTrendResultDto> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出成本分析月度涨跌 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
