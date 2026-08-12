// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：機種成本推移頁靜態文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '機種成本推移',
    periodRange: '核算年月',
    componentCodeOptional: '物料編碼（可選，空=全部）',
    componentCodesOptional: '物料編碼（可多選，空=期間最後月全部）',
    modelCodesOptional: '機種編碼（可多選，空=期間最後月全部）',
    componentAll: '全部物料',
    modelAll: '全部機種',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇核算期間',
    selectMaterialTypeRequired: '請選擇物料類型',
    selectModelRequired: '請選擇機種編碼',
    selectProductRequired: '請選擇產品編碼',
    selectMasterFirst: '請先選擇工廠並查詢',
    tabs: {
      summary: '機種成本推移',
      detail: '差異組件推移',
    },
    summary:
      '{plant} / 機種 {model} / 物料 {component} — 產品組 {productCount} 個，材料行 {componentCount} 條（按工廠+機種+組件+生產相關+採購類型合併月材料成本）',
    summaryDetail:
      '{plant} / 機種 {model} / 物料 {component} — 產品組 {productCount} 個，差異組件 {componentCount} 條（按月對比物料在機種中的有無）',
    modelTrendSummary: '機種月材料成本 {base} → {compare}：{cost}（{trend}，差額 {variance}，{percent}）',
    trendSummary: '分析行環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    productCodes: '產品組',
    modelGroup: '機種組',
    productCount: '產品數',
    export: '導出機種成本推移',
    exportSuccess: '機種成本推移導出成功',
    exportFailed: '機種成本推移導出失敗',
    queryFailed: '機種成本推移查詢失敗',
    filter: {
      all: '全部',
      changed: '僅變動',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
      new: '新增',
      removed: '剔除',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '新增',
      removed: '剔除',
      up: '漲',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
    },
  },
};
