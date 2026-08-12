// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/variance-cost-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：差異コスト推移；引用键 logistics.manufacturing.bom.variance-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '差異コスト推移',
    periodRange: '原価計算年月',
    modelCodeRequired: '機種（必須）',
    modelCodesRequired: '機種（任意・複数可；空=全機種）',
    modelCodesOptional: '機種（任意・複数可；空=全機種）',
    productCodesOptional: '製品（任意・機種連動）',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '原価計算期間を選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectModelRequired: '機種を選択（任意；空で全機種）',
    summary:
      '{plant} / 機種 {model} — 製品 {productCount}、差異部品 {componentCount}（有無/版数差異のみ、全BOM行ではない）',
    trendSummary:
      '比較 {base} → {compare}：新規 {newCount} · 削除 {removed} · 版数変更 {version}',
    productCodes: '製品グループ',
    productCount: '製品数',
    previousComponentCode: '基準月部品',
    export: '差異コスト推移をエクスポート',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    queryFailed: '照会失敗',
    filter: {
      all: 'すべての差異',
      changed: '有無のみ',
    },
    sort: {
      trend: '差異種別（全件）',
      varianceDesc: '差額絶対値降順（全件）',
      componentCode: '部品コード（全件）',
    },
    trend: {
      none: '—',
      up: '増',
      down: '減',
      flat: '横ばい',
      new: '新規',
      removed: '削除',
      version: '版数変更',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '新規',
      removed: '削除',
      version: '版数',
      up: '増',
      down: '減',
      flat: '横ばい',
    },
    columns: {
      trend: '差異',
      varianceAmount: '移動単価差額',
      variancePercent: '前月比%',
    },
  },
}
