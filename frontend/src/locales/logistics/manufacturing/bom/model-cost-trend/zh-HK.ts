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
    selectPlantRequired: '請選擇工廠代碼',
    selectModelRequired: '請選擇機種編碼',
    selectMasterFirst: '請先選擇機種並查詢',
    tabs: {
      summary: '機種成本推移',
      detail: '差異組件推移',
    },
    summary: '{plant} / {model} — 產品組 {productCount} 個，分析行 {componentCount} 條（按工廠+組件+生產相關+採購類型合併月材料成本，缺月不回填）',
    summaryDetail: '{plant} / {model} — 產品組 {productCount} 個，差異組件 {componentCount} 條（按組件編碼+描述+數量+批量標識+生產相關+採購類型+特殊採購類+利潤中心對齊；期間取核算日期）',
    modelTrendSummary: '機種月材料成本 {base} → {compare}：{cost}（{trend}，差額 {variance}，{percent}）',
    trendSummary: '分析行環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    productCodes: '產品組',
    productCount: '產品數',
    export: '導出機種成本推移',
    exportSuccess: '機種成本推移導出成功',
    exportFailed: '機種成本推移導出失敗',
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
    },
  },
};
