// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：Model cost trend page copy; keys logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Model Cost Trend',
    periodRange: 'Costing months',
    componentCodeOptional: 'Component (optional, empty = all)',
    componentCodesOptional: 'Components (multi, empty = all in last period month)',
    modelCodesOptional: 'Models (multi, empty = all in last period month)',
    componentAll: 'All components',
    modelAll: 'All models',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select costing period',
    selectMaterialTypeRequired: 'Please select a material type',
    selectModelRequired: 'Please select a model',
    selectProductRequired: 'Please select a product',
    selectMasterFirst: 'Select a plant and search first',
    tabs: {
      summary: 'Model cost trend',
      detail: 'Component variance trend',
    },
    summary:
      '{plant} / model {model} / component {component} — product group {productCount}, material rows {componentCount} (scope = models & components in last period month)',
    summaryDetail:
      '{plant} / model {model} / component {component} — product group {productCount}, variance components {componentCount} (month-by-month presence)',
    modelTrendSummary: 'Model monthly material cost {base} → {compare}: {cost} ({trend}, variance {variance}, {percent})',
    trendSummary: 'Analysis MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    productCodes: 'Product group',
    modelGroup: 'Model group',
    productCount: 'Products',
    export: 'Export model cost trend',
    exportSuccess: 'Model cost trend exported',
    exportFailed: 'Failed to export model cost trend',
    queryFailed: 'Model cost trend query failed',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    sort: {
      productCountDesc: 'Product count ↓ (all rows)',
      productCountAsc: 'Product count ↑ (all rows)',
      trend: 'Trend first (all rows)',
      varianceDesc: 'Abs. variance ↓ (all rows)',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
      new: 'New',
      removed: 'Removed',
    },
    periodChange: {
      present: 'Yes',
      absent: 'No',
      new: 'New',
      removed: 'Removed',
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
