// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：机种成本推移页静态文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '机种成本推移',
    periodRange: '核算年月',
    selectPlantRequired: '请选择工厂代码',
    selectModelRequired: '请选择机种编码',
    selectProductRequired: '请选择产品编码',
    selectMasterFirst: '请先选择机种并查询',
    tabs: {
      summary: '机种成本推移',
      detail: '差异组件推移',
    },
    summary: '{plant} / {model} — 产品组 {productCount} 个，展开行 {componentCount} 条（Item 按工厂+组件+生产相关+采购类型合并月材料成本）',
    summaryDetail: '{plant} / {model} — 产品组 {productCount} 个，差异组件 {componentCount} 条（按组件编码+描述+数量+批量标识+生产相关+采购类型+特殊采购类+利润中心对齐；期间取核算日期）',
    modelTrendSummary: '机种月材料成本 {base} → {compare}：{cost}（{trend}，差额 {variance}，{percent}）',
    trendSummary: '分析行环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
    productCodes: '产品组',
    productCount: '产品数',
    export: '导出机种成本推移',
    exportSuccess: '机种成本推移导出成功',
    exportFailed: '机种成本推移导出失败',
    filter: {
      all: '全部',
      changed: '仅涨跌',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
  },
};
