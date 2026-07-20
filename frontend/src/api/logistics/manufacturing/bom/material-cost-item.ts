// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：material-cost-item.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common';
import type {
  BomMaterialCostItem,
  BomMaterialCostItemCreate,
  BomMaterialCostItemQuery,
  BomMaterialCostItemRecalculateSubmitted,
  BomMaterialCostItemUpdate
} from '@/types/logistics/manufacturing/bom/material-cost-item';
import type {
  BomMaterialCostItemMonthlyTrendQuery,
  BomMaterialCostItemMonthlyTrendResult,
  BomMaterialCostItemComponentMovingPriceQuery,
  BomMaterialCostItemComponentMovingPriceResult,
  BomMaterialCostItemModelMovingPriceQuery,
  BomMaterialCostItemModelMovingPriceResult,
  BomMaterialCostItemZeroMovingPriceQuery,
  BomMaterialCostItemZeroMovingPriceResult,
  BomMaterialCostItemTransposedQuery,
  BomMaterialCostItemTransposedResult,
  BomMaterialCostItemVarianceQuery,
  BomMaterialCostItemVarianceResult
} from '@/types/logistics/manufacturing/bom/material-cost-trend';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBomMaterialCostItems
 */
const BOM_MATERIAL_COST_ITEM_API_BASE = 'TaktBomMaterialCostItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取BOM物料成本明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BomMaterialCostItem>>} 分页结果
 */
export function getBomMaterialCostItemList(queryDto: any): Promise<TaktPagedResult<BomMaterialCostItem>> {
  return request<TaktPagedResult<BomMaterialCostItem>>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function getBomMaterialCostItemById(id: string): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建BOM物料成本明细
 * @param {BomMaterialCostItemCreate} dto 创建DTO
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function createBomMaterialCostItem(dto: BomMaterialCostItemCreate): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @param {BomMaterialCostItemUpdate} dto 更新DTO
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function updateBomMaterialCostItem(id: string, dto: BomMaterialCostItemUpdate): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostItemById(id: string): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除BOM物料成本明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 提交后台重算（按明细 Sync 汇总表；完成后 SignalR 通知触发用户）
 * @param {BomMaterialCostItemQuery} queryDto 与明细列表相同的筛选（须单个核算月；忽略分页）
 * @param {boolean} [forceRecalculate=false] 为 true 时按重置成本路径排队
 * @param {number} [processRecordCount=5000] 处理记录数上限（工厂+产品组；0=全部）
 * @returns {Promise<BomMaterialCostItemRecalculateSubmitted>} 已提交回执
 */
export function recalculateBomMaterialCostItemModelAverage(
  queryDto: BomMaterialCostItemQuery,
  forceRecalculate = false,
  processRecordCount = 5000
): Promise<BomMaterialCostItemRecalculateSubmitted> {
  return request<BomMaterialCostItemRecalculateSubmitted>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/recalculate-model-average`,
    method: 'put',
    params: {
      ...queryDto,
      forceRecalculate,
      processRecordCount,
    },
  });
}

/**
 * 提交后台重算（与控制器方法名对齐的别名）
 * @param {BomMaterialCostItemQuery} queryDto 筛选条件（须单个核算月）
 * @param {boolean} [forceRecalculate=false] 是否重置后重算
 * @param {number} [processRecordCount=5000] 处理记录数上限（0=全部）
 * @returns {Promise<BomMaterialCostItemRecalculateSubmitted>} 已提交回执
 */
export function recalculateBomMaterialCostItemModelMonthlyAverage(
  queryDto: BomMaterialCostItemQuery,
  forceRecalculate = false,
  processRecordCount = 5000
): Promise<BomMaterialCostItemRecalculateSubmitted> {
  return recalculateBomMaterialCostItemModelAverage(queryDto, forceRecalculate, processRecordCount);
}

// ========================================
// 选项
// ========================================

/**
 * 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
 * @param {string} [plantCode] 工厂代码
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBomMaterialCostItemOptions(plantCode?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/options`,
    method: 'get',
    params: plantCode ? { plantCode } : undefined,
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getBomMaterialCostItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入BOM物料成本明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBomMaterialCostItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出BOM物料成本明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBomMaterialCostItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

/**
 * 导出 BOM 成本分析明细清单（权限 logistics:…:analysis:export）
 * @param {any} [query] 与明细列表相同的筛选
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemAnalysisList(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/analysis-items/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  });
}

// ========================================
// 转置 / 差异 / 月度涨跌分析
// ========================================

/**
 * 获取 BOM 物料成本转置列表（行=产品，列=月份总成本）
 * @param {BomMaterialCostItemTransposedQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemTransposedResult>} 转置分页结果
 */
export function getBomMaterialCostItemTransposedList(
  queryDto: BomMaterialCostItemTransposedQuery
): Promise<BomMaterialCostItemTransposedResult> {
  return request<BomMaterialCostItemTransposedResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/transposed`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 物料成本转置报表
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
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/transposed/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 获取 BOM 物料成本差异分析（两期间组件级对比）
 * @param {BomMaterialCostItemVarianceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemVarianceResult>} 差异分析结果
 */
export function getBomMaterialCostItemVarianceAnalysis(
  queryDto: BomMaterialCostItemVarianceQuery
): Promise<BomMaterialCostItemVarianceResult> {
  return request<BomMaterialCostItemVarianceResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/variance-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 物料成本差异分析报表
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
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/variance-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  });
}

/**
 * 获取 BOM 物料成本月度涨跌分析
 * @param {BomMaterialCostItemMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemMonthlyTrendResult>} 月度涨跌结果
 */
export function getBomMaterialCostItemMonthlyTrendAnalysis(
  queryDto: BomMaterialCostItemMonthlyTrendQuery
): Promise<BomMaterialCostItemMonthlyTrendResult> {
  return request<BomMaterialCostItemMonthlyTrendResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 物料成本月度涨跌分析报表
 * @param {BomMaterialCostItemMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemMonthlyTrendAnalysis(
  query: BomMaterialCostItemMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/monthly-trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  });
}

/**
 * BOM 成本推移：单个产品下明细组件 × 月材料成本转置分析
 * @param {BomMaterialCostItemComponentMovingPriceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemComponentMovingPriceResult>} 明细组件月材料成本结果
 */
export function getBomMaterialCostItemComponentMovingPriceAnalysis(
  queryDto: BomMaterialCostItemComponentMovingPriceQuery
): Promise<BomMaterialCostItemComponentMovingPriceResult> {
  return request<BomMaterialCostItemComponentMovingPriceResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/component-moving-price-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本推移（单个产品×月材料成本）
 * @param {BomMaterialCostItemComponentMovingPriceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemComponentMovingPriceAnalysis(
  query: BomMaterialCostItemComponentMovingPriceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/component-moving-price-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 机种成本推移：按组件编码合并后核算月单价转置分析
 * @param {BomMaterialCostItemModelMovingPriceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemModelMovingPriceResult>} 合并组件单价转置结果
 */
export function getBomMaterialCostItemModelMovingPriceAnalysis(
  queryDto: BomMaterialCostItemModelMovingPriceQuery
): Promise<BomMaterialCostItemModelMovingPriceResult> {
  return request<BomMaterialCostItemModelMovingPriceResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/model-moving-price-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出机种成本推移（组件合并）分析报表
 * @param {BomMaterialCostItemModelMovingPriceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemModelMovingPriceAnalysis(
  query: BomMaterialCostItemModelMovingPriceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/model-moving-price-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 机种零价格合并清单（X+F、移动平均价=0，按组件合并产品）
 * @param {BomMaterialCostItemZeroMovingPriceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemZeroMovingPriceResult>} 合并结果
 */
export function getBomMaterialCostItemZeroMovingPriceMerged(
  queryDto: BomMaterialCostItemZeroMovingPriceQuery
): Promise<BomMaterialCostItemZeroMovingPriceResult> {
  return request<BomMaterialCostItemZeroMovingPriceResult>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/zero-moving-price-merged`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出机种零价格合并清单
 * @param {BomMaterialCostItemZeroMovingPriceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemZeroMovingPriceMerged(
  query: BomMaterialCostItemZeroMovingPriceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/zero-moving-price-merged/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  });
}

/**
 * 按机种获取产品下拉选项（汇总表 ProductCode 去重）
 * @param {string} modelCode 机种编码
 * @param {string} [plantCode] 工厂代码（可选）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBomMaterialCostItemProductOptionsByModel(
  modelCode: string,
  plantCode?: string
): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/product-options-by-model`,
    method: 'get',
    params: plantCode ? { modelCode, plantCode } : { modelCode },
  });
}

/**
 * 按产品编码反查机种编码
 * @param {string} productCode 产品编码
 * @param {string} [plantCode] 工厂代码（可选）
 * @returns {Promise<string | null>} 机种编码
 */
export function getBomMaterialCostItemModelCodeByProduct(
  productCode: string,
  plantCode?: string
): Promise<string | null> {
  return request<string | null>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/model-by-product`,
    method: 'get',
    params: plantCode ? { productCode, plantCode } : { productCode },
  });
}
