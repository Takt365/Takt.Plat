// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：material-cost-analysis.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析 API（转置 / 差异 / 月度涨跌；三页共用级联选项 URL）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktSelectOption } from '@/types/common';
import type {
  BomMaterialCostItemMonthlyTrendQuery,
  BomMaterialCostItemMonthlyTrendResult,
  BomMaterialCostItemTransposedQuery,
  BomMaterialCostItemTransposedResult,
  BomMaterialCostItemVarianceQuery,
  BomMaterialCostItemVarianceResult,
} from '@/types/logistics/manufacturing/bom/material-cost-analysis';

/** API 路由前缀（对应 TaktBomMaterialCostAnalysesController） */
const BOM_MATERIAL_COST_ANALYSIS_API_BASE = 'TaktBomMaterialCostAnalyses';

/**
 * 三页共用：工厂选项 URL（仅当前公司 RelatedPlant ∩ 本表 PlantCode；通常一项）
 * @returns {string} 相对 API 路径
 */
export function getBomMaterialCostAnalysisPlantOptionsUrl(): string {
  return `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/plant-options`;
}

/**
 * 三页共用：本表物料类型去重选项 URL（takt_bom_material_cost.MaterialType；须 plantCode）
 * @description 分析视图专用；❌ 非字典 logistics_material_type（CRUD 表单用）
 * @returns {string} 相对 API 路径
 */
export function getBomMaterialCostAnalysisMaterialTypeOptionsUrl(): string {
  return `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/material-type-options`;
}

/**
 * 拉取本表物料类型去重选项（须工厂；返回 FERT/HALB 等全部类型，不做默认截断）
 * @param {string} plantCode 工厂代码
 * @returns {Promise<TaktSelectOption[]>} 物料类型选项
 */
export function getBomMaterialCostAnalysisMaterialTypeOptions(
  plantCode: string,
): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/material-type-options`,
    method: 'get',
    params: { plantCode },
  });
}

/**
 * 三页共用：本表机种去重选项 URL（分析视图；takt_bom_material_cost.ModelCode；须 plantCode + materialType）
 * @description ❌ 非 CRUD 主数据 TaktBomMaterialCosts/model-options（TaktModelDestination）
 * @returns {string} 相对 API 路径
 */
export function getBomMaterialCostAnalysisModelOptionsUrl(): string {
  return `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/model-options`;
}

/**
 * 三页共用：本表产品编码去重选项 URL（takt_bom_material_cost.ProductCode；须 plantCode + materialType）
 * @description 仅本表真实 ProductCode，非 MaterialPlants、非物料类型字典
 * @returns {string} 相对 API 路径
 */
export function getBomMaterialCostAnalysisProductOptionsUrl(): string {
  return `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/product-options`;
}

/**
 * 拉取本表工厂去重选项（PlantCode）；供默认选中公司关联工厂前校验是否存在于本表
 * @returns {Promise<TaktSelectOption[]>} 本表工厂选项
 */
export function getBomMaterialCostAnalysisPlantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/plant-options`,
    method: 'get',
  });
}

/**
 * 获取 BOM 成本分析转置列表
 * @param {BomMaterialCostItemTransposedQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemTransposedResult>} 转置分页结果
 */
export function getBomMaterialCostItemTransposedList(
  queryDto: BomMaterialCostItemTransposedQuery
): Promise<BomMaterialCostItemTransposedResult> {
  return request<BomMaterialCostItemTransposedResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/transposed`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本分析转置报表
 * @param {Partial<BomMaterialCostItemTransposedQuery>} [query] 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemTransposed(
  query?: Partial<BomMaterialCostItemTransposedQuery>,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/transposed/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 获取 BOM 成本分析差异
 * @param {BomMaterialCostItemVarianceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemVarianceResult>} 差异结果
 */
export function getBomMaterialCostItemVarianceAnalysis(
  queryDto: BomMaterialCostItemVarianceQuery
): Promise<BomMaterialCostItemVarianceResult> {
  return request<BomMaterialCostItemVarianceResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/variance-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本分析差异报表
 * @param {BomMaterialCostItemVarianceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemVarianceAnalysis(
  query: BomMaterialCostItemVarianceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/variance-analysis/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
  });
}

/**
 * 获取 BOM 成本分析月度涨跌
 * @param {BomMaterialCostItemMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemMonthlyTrendResult>} 月度涨跌结果
 */
export function getBomMaterialCostItemMonthlyTrendAnalysis(
  queryDto: BomMaterialCostItemMonthlyTrendQuery
): Promise<BomMaterialCostItemMonthlyTrendResult> {
  return request<BomMaterialCostItemMonthlyTrendResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}
