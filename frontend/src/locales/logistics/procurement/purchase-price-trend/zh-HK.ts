// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：採購價格推移頁靜態文案；引用鍵 logistics.procurement.purchase-price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '採購價格推移',
    periodRange: '期間年月',
    materialCode: '物料編碼',
    supplierCode: '供應商編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '物料×供應商行 {count} 條（按月展示有效採購價；無有效價月份留空）',
    summaryModel: '物料×供應商行 {count} 條（機種/產品組來自 BOM：組件→產品→機種）',
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    export: '清單匯出',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
    tabs: {
      price: '採購價格推移',
      model: '機種價格推移',
    },
    filter: {
      all: '全部',
      changed: '僅漲跌',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
      modelGroup: '機種組',
      productGroup: '產品組',
      materialText: '物料描述',
    },
  },
};
