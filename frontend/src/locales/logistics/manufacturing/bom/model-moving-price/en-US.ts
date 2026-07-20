// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-moving-price
// 文件名称：en-US.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：Model cost trend page copy; keys logistics.manufacturing.bom.model-moving-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Model Cost Trend',
    periodRange: 'Costing months',
    selectPlantRequired: 'Please select a plant',
    selectModelRequired: 'Please select a model',
    selectMasterFirst: 'Select a model and search first',
    summary: '{plant} / {model} — product group {productCount}, analysis rows {componentCount} (merge by plant+component+production-related+purchase type; monthly material cost; missing months not filled)',
    modelTrendSummary: 'Model monthly material cost {base} → {compare}: {cost} ({trend}, variance {variance}, {percent})',
    trendSummary: 'Analysis MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    productCodes: 'Product group',
    productCount: 'Products',
    export: 'Export model cost trend',
    exportSuccess: 'Model cost trend exported',
    exportFailed: 'Failed to export model cost trend',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'Variance',
      variancePercent: 'Variance %',
    },
  },
};
