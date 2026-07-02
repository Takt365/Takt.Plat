// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-gijutsu
// 文件名称：ja-JP.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設変主画面静的文案；参照キー logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: 'ソース設変取込',
      openButton: 'ソース取込',
      searchPlaceholder: '設変番号 / タイトル / 機種',
      detailCount: '明細行数',
      selectRequired: 'ソース設変を1件選択してください',
      loadToForm: 'フォームへ読込',
      plantFromCompany: '会社 {company} → 工場 {plant}',
      companyRequired: '会社を選択してからソース設変を取込んでください',
      formTitle: 'ソース設変取込（担当者・管理区分・添付を入力して保存）',
      attachmentRequired: '添付を1件以上追加し、ファイルをアップロードしてください',
      attachmentUploadRequired: '{row} 行目：添付ファイルが未アップロードです',
      importSelected: '選択取込（{count}）',
      importSuccess: '{count} 件の設変を取込しました',
      importPartial: '取込完了：成功 {success} 件、失敗 {fail} 件',
    },
    tabs: {
      oldNewMaterial: '旧新部品',
    },
  },
};
