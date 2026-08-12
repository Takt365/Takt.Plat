// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/sales/monthly-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月销售推移页静态文案；引用键 logistics.sales.monthly-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Monthly Sales Trend',
    periodRange: 'Period',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    summary: '{count} customer row(s) (monthly actual sales amount)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    export: 'Export',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
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
      varianceAmount: 'MoM Δ',
      variancePercent: 'MoM %',
    },
  },
};
