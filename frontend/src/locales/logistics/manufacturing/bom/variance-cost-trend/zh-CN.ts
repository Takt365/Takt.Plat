// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/variance-cost-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：差异成本推移页静态文案；引用键 logistics.manufacturing.bom.variance-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '差异成本推移',
    periodRange: '核算年月',
    modelCodeRequired: '机种编码（必选）',
    modelCodesRequired: '机种编码（可选，可多选；空=全部机种）',
    modelCodesOptional: '机种编码（可选，可多选；空=全部机种）',
    productCodesOptional: '产品编码（可选，随机种联动）',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择核算期间',
    selectMaterialTypeRequired: '请选择物料类型',
    selectModelRequired: '请选择机种编码（可多选；可留空表示全部）',
    summary:
      '{plant} / 机种 {model} — 产品 {productCount} 个，差异组件 {componentCount} 条（仅有无/版本差异，非全量组件）',
    trendSummary:
      '对比 {base} → {compare}：新增 {newCount} · 剔除 {removed} · 版本变更 {version}',
    productCodes: '产品组',
    productCount: '产品数',
    previousComponentCode: '基准月组件',
    export: '导出差异成本推移',
    exportSuccess: '差异成本推移导出成功',
    exportFailed: '差异成本推移导出失败',
    queryFailed: '差异成本推移查询失败',
    filter: {
      all: '全部差异',
      changed: '有无差异',
    },
    sort: {
      trend: '差异类型（全表）',
      varianceDesc: '差额绝对值降序（全表）',
      componentCode: '组件编码（全表）',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
      new: '新增',
      removed: '剔除',
      version: '版本变更',
    },
    periodChange: {
      present: '有',
      absent: '无',
      new: '新增',
      removed: '剔除',
      version: '版本',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '差异',
      varianceAmount: '移动价格差额',
      variancePercent: '环比%',
    },
  },
}
