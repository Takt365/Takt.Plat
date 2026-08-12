// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost
// 文件名称：ja-JP.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM材料コスト3階層表示の静的文案；参照キー logistics.manufacturing.bom.material-cost.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    masterpanetitle: 'BOM材料コスト（機種集計）',
    detailpanetitle: '製品 / 明細',
    productpanetitle: '製品月次コスト',
    itempanetitle: 'BOM明細',
    selectmasterfirst: '先に機種集計行を選択してください',
    selectproductfirst: 'BOM明細を表示するには製品行を選択してください',
    periodRange: '原価年月',
    selectPlantRequired: '工場コードを選択してください',
    selectModelRequired: '機種を選択してください',
    itemFilterHint: '既定：生産関連=X、購買タイプ=F（クリアで全件表示）',
    productRowCount: '製品数',
    modalmasterhint: '左機種 → 中製品 → 右明細（エンティティ分割なし）。明細取込後に合計または再計算してください。',
    costSum: 'コスト合計',
    costRecalculate: '再計算コスト',
    backfillPurchasePrice: '購買価格を反映',
    refreshModelFields: '機種価格を反映',
    costingMonth: '原価月',
    costingMonthPlaceholder: '原価月を選択',
    processRecordCount: '処理件数',
    processRecordCountHint: '工場+製品グループ単位；0=全件、既定 5000',
    processRecordCountInvalid: '処理件数は 0 以上の整数にしてください',
    costNeedMonth: '原価月を選択してください',
    costSumSubmitted: '{month} のバックグラウンド合計を受付ました。完了後に通知します',
    costRecalculateSubmitted: '{month} の再計算（ゼロクリア後に再集計）を受付ました。完了後に通知します',
    costRecalculateCompleted: '{month} 処理完了（所要 {duration}、更新 {refreshed}、スキップ {skipped}）',
    costRecalculateFailed: 'コスト処理に失敗しました',
    costRecalculateConfirmTitle: 'コストを再計算しますか？',
    costRecalculateConfirmContent: '当該原価月をいったんゼロにして明細から再集計し、完了後に集計を更新します。',
    backfillPurchasePriceSuccess:
      '{month} 反映完了：更新 {updated} 行、スキップ {skipped} 行（スキャン {scanned} 行）',
    backfillPurchasePriceFailed: '購買価格の反映に失敗しました',
    refreshModelFieldsSuccess:
      '{month} 機種価格反映完了：スキャン {scanned}、機種更新 {modelUpdated}、平均更新 {averageUpdated}（{groups} グループ）',
    refreshModelFieldsFailed: '機種価格の反映に失敗しました',
    zeroPrice: {
      button: '部品ゼロ価格',
      monthTitle: '工場・原価月を選択',
      title: '部品ゼロ価格（{month}）',
      hint: '{month} · 製品 {productCount} · ゼロ価格部品 {componentCount}（全機種 · ProductionRelated=X · PurchaseType=F · 移動平均価格=0；代替候補=末尾版英字を逆引き例 D01446500B→A、原価月以前の直近有価月の移動価格）',
      productCodes: '共有製品',
      productCount: '製品数',
      suggestedComponentCode: '代替候補部品',
      suggestedMovingPrice: '代替移動価格',
      exportSuccess: '部品ゼロ価格一覧のエクスポートに成功しました',
      exportFailed: '部品ゼロ価格一覧のエクスポートに失敗しました',
    },
  },
};
