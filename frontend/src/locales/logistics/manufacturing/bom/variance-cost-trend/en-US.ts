// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/variance-cost-trend
// 文件名称：en-US.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：Variance cost trend page copy; keys logistics.manufacturing.bom.variance-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Variance Cost Trend',
    periodRange: 'Costing months',
    modelCodeRequired: 'Model (required)',
    modelCodesRequired: 'Models (optional, multi-select; empty = all)',
    modelCodesOptional: 'Models (optional, multi-select; empty = all)',
    productCodesOptional: 'Products (optional, linked to models)',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select costing period',
    selectMaterialTypeRequired: 'Please select a material type',
    selectModelRequired: 'Select models (optional; leave empty for all)',
    summary:
      '{plant} / model {model} — {productCount} products, {componentCount} variance components (presence/version only, not all BOM lines)',
    trendSummary:
      'Compare {base} → {compare}: new {newCount} · removed {removed} · version {version}',
    productCodes: 'Product group',
    productCount: 'Products',
    previousComponentCode: 'Base-month component',
    export: 'Export variance cost trend',
    exportSuccess: 'Variance cost trend exported',
    exportFailed: 'Failed to export variance cost trend',
    queryFailed: 'Variance cost trend query failed',
    filter: {
      all: 'All variances',
      changed: 'Presence only',
    },
    sort: {
      trend: 'Variance type (all rows)',
      varianceDesc: 'Abs. variance ↓ (all rows)',
      componentCode: 'Component code (all rows)',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
      new: 'New',
      removed: 'Removed',
      version: 'Version change',
    },
    periodChange: {
      present: 'Yes',
      absent: 'No',
      new: 'New',
      removed: 'Removed',
      version: 'Version',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Variance',
      varianceAmount: 'Moving price delta',
      variancePercent: 'Variance %',
    },
  },
}
