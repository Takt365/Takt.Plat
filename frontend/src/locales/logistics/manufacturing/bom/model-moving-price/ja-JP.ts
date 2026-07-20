// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-moving-price
// 文件名称：ja-JP.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：機種コスト推移ページ文案；引用键 logistics.manufacturing.bom.model-moving-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '機種コスト推移',
    periodRange: '原価年月',
    selectPlantRequired: '工場を選択してください',
    selectModelRequired: '機種を選択してください',
    selectMasterFirst: '機種を選択して検索してください',
    summary: '{plant} / {model} — 製品グループ {productCount}、分析行 {componentCount}（工場+部品+生産関連+購買タイプで月次材料コスト統合、欠月は埋めない）',
    modelTrendSummary: '機種月次材料コスト {base} → {compare}：{cost}（{trend}、差額 {variance}、{percent}）',
    trendSummary: '分析行前月比 {base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat}',
    productCodes: '製品グループ',
    productCount: '製品数',
    export: '機種コスト推移を出力',
    exportSuccess: '機種コスト推移の出力に成功',
    exportFailed: '機種コスト推移の出力に失敗',
    filter: {
      all: 'すべて',
      changed: '騰落のみ',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
    },
    columns: {
      trend: '騰落',
      varianceAmount: '差額',
      variancePercent: '変動率%',
    },
  },
};
