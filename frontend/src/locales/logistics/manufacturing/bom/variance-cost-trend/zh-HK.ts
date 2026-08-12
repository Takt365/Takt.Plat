// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/variance-cost-trend
// 文件名称：zh-HK.ts
// 创建時間：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：差異成本推移；引用键 logistics.manufacturing.bom.variance-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '差異成本推移',
    periodRange: '核算年月',
    modelCodeRequired: '機種編碼（必選）',
    modelCodesRequired: '機種編碼（可選，可多選；空=全部機種）',
    modelCodesOptional: '機種編碼（可選，可多選；空=全部機種）',
    productCodesOptional: '產品編碼（可選，隨機種聯動）',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇核算期間',
    selectMaterialTypeRequired: '請選擇物料類型',
    selectModelRequired: '請選擇機種編碼（可多選；可留空表示全部）',
    summary:
      '{plant} / 機種 {model} — 產品 {productCount} 個，差異組件 {componentCount} 條（僅有無/版本差異，非全量組件）',
    trendSummary:
      '對比 {base} → {compare}：新增 {newCount} · 剔除 {removed} · 版本變更 {version}',
    productCodes: '產品組',
    productCount: '產品數',
    previousComponentCode: '基準月組件',
    export: '導出差異成本推移',
    exportSuccess: '差異成本推移導出成功',
    exportFailed: '差異成本推移導出失敗',
    queryFailed: '差異成本推移查詢失敗',
    filter: {
      all: '全部差異',
      changed: '有無差異',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
      new: '新增',
      removed: '剔除',
      version: '版本變更',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '新增',
      removed: '剔除',
      version: '版本',
      up: '漲',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '差異',
      varianceAmount: '移動價格差額',
      variancePercent: '環比%',
    },
  },
}
