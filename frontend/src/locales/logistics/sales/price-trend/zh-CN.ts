// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/sales/price-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移页静态文案；引用键 logistics.sales.price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '销售价格推移',
    periodRange: '期间年月',
    materialCode: '物料编码',
    customerCode: '客户编码',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '物料×客户行 {count} 条（按月展示有效销售价；无有效价月份留空）',
    summaryModel: '物料×客户行 {count} 条（机种/产品组来自 BOM：组件→产品→机种）',
    trendSummary: '环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      price: '销售价格推移',
      model: '机种价格推移',
    },
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
      modelGroup: '机种组',
      productGroup: '产品组',
      materialText: '物料描述',
    },
  },
};
