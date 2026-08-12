// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：機種コスト推移ページ文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '機種コスト推移',
    periodRange: '原価年月',
    componentCodeOptional: '部品（任意、空＝すべて）',
    componentCodesOptional: '部品（複数可、空＝期間最終月すべて）',
    modelCodesOptional: '機種（複数可、空＝期間最終月すべて）',
    componentAll: '全部品',
    modelAll: '全機種',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '原価期間を選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectModelRequired: '機種を選択してください',
    selectProductRequired: '製品を選択してください',
    selectMasterFirst: '工場を選択して検索してください',
    tabs: {
      summary: '機種コスト推移',
      detail: '差異部品推移',
    },
    summary:
      '{plant} / 機種 {model} / 部品 {component} — 製品グループ {productCount}、材料行 {componentCount}（工場+機種+部品+生産関連+購買タイプで月次材料コスト統合）',
    summaryDetail:
      '{plant} / 機種 {model} / 部品 {component} — 製品グループ {productCount}、差異部品 {componentCount}（機種内の月次有無を比較）',
    modelTrendSummary: '機種月次材料コスト {base} → {compare}：{cost}（{trend}、差額 {variance}、{percent}）',
    trendSummary: '分析行前月比 {base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat}',
    productCodes: '製品グループ',
    modelGroup: '機種グループ',
    productCount: '製品数',
    export: '機種コスト推移を出力',
    exportSuccess: '機種コスト推移の出力に成功',
    exportFailed: '機種コスト推移の出力に失敗',
    queryFailed: '機種コスト推移の照会に失敗しました',
    filter: {
      all: 'すべて',
      changed: '変動のみ',
    },
    sort: {
      productCountDesc: '製品数降順（全件）',
      productCountAsc: '製品数昇順（全件）',
      trend: '騰落優先（全件）',
      varianceDesc: '差額絶対値降順（全件）',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
      new: '新規',
      removed: '削除',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '新規',
      removed: '削除',
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
