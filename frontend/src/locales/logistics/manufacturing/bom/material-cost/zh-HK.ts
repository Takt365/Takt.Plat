// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost
// 文件名称：zh-HK.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本三層瀏覽靜態文案；引用鍵 logistics.manufacturing.bom.material-cost.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    masterpanetitle: 'BOM物料成本（機種匯總）',
    detailpanetitle: '產品 / 明細',
    productpanetitle: '產品月成本',
    itempanetitle: 'BOM 明細',
    selectmasterfirst: '請先選擇一條機種彙總行',
    selectproductfirst: '請先選擇一條產品行以查看 BOM 明細',
    periodRange: '核算年月',
    selectPlantRequired: '請選擇工廠代碼',
    selectModelRequired: '請選擇機種',
    itemFilterHint: '預設：生產相關=X、採購類型=F（可清空查看全部）',
    productRowCount: '產品數',
    modalmasterhint: '左機種 → 中產品 → 右明細（不拆實體）；請匯入明細後合計或重算成本。',
    costSum: '成本合計',
    costRecalculate: '重算成本',
    backfillPurchasePrice: '回填採購價格',
    refreshModelFields: '回填機種價格',
    costingMonth: '核算月份',
    costingMonthPlaceholder: '請選擇核算月份',
    processRecordCount: '處理記錄數',
    processRecordCountHint: '按工廠+產品組計數；0 表示全部，預設 5000',
    processRecordCountInvalid: '處理記錄數須為大於等於 0 的整數',
    costNeedMonth: '請選擇核算月份',
    costSumSubmitted: '已提交 {month} 後台合計，完成後將通知您',
    costRecalculateSubmitted: '已提交 {month} 後台重算（先歸零再匯總），完成後將通知您',
    costRecalculateCompleted: '{month} 處理完成（耗時 {duration}，刷新 {refreshed} 組，跳過 {skipped} 組）',
    costRecalculateFailed: '成本處理失敗',
    costRecalculateConfirmTitle: '確認重算成本？',
    costRecalculateConfirmContent: '將先歸零再按明細重算該核算月成本，完成後刷新匯總。',
    backfillPurchasePriceSuccess: '{month} 回填完成：更新 {updated} 行，跳過 {skipped} 行（掃描 {scanned} 行）',
    backfillPurchasePriceFailed: '回填採購價格失敗',
    refreshModelFieldsSuccess:
      '{month} 回填機種價格完成：掃描 {scanned} 行，機種更新 {modelUpdated}，平均更新 {averageUpdated}（{groups} 組）',
    refreshModelFieldsFailed: '回填機種價格失敗',
    zeroPrice: {
      button: '組件零價格',
      monthTitle: '選擇工廠與核算月份',
      title: '組件零價格（{month}）',
      hint: '{month} · 產品 {productCount} · 零價組件 {componentCount}（全部機種 · ProductionRelated=X · PurchaseType=F · 移動平均價=0，按組件合併產品；建議代替=末尾版本字母逆推如 D01446500B→A，取該料最近有價月移動價）',
      productCodes: '共用產品',
      productCount: '產品數',
      suggestedComponentCode: '建議代替組件',
      suggestedMovingPrice: '建議移動價格',
      exportSuccess: '組件零價格清單匯出成功',
      exportFailed: '組件零價格清單匯出失敗',
    },
  },
};
