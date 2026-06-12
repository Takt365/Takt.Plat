// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：ja-JP.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单页面静态文案（引用键 workflow.form.page.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    category: {
      business: '業務フォーム',
      system: 'システムフォーム',
      general: '汎用フォーム',
    },
    type: {
      static: '静的フォーム',
      custom: 'カスタムフォーム',
      dynamic: '動的フォーム',
    },
    versionPlaceholder: 'バージョンを入力',
    dataSourcePlaceholder: 'データソースを入力',
    dataTablePlaceholder: 'データテーブルを選択',
    entityTableHint: 'エンティティ列はフォーム設計で使用します',
    isDatasourceLabel: '業務データソースをバインド',
    isDatasourceHint: '物理テーブル列をマップし承認書き戻しを有効化',
    businessBindingTitle: '業務状態と提出ルール',
    businessStatusColumn: '業務状態列',
    businessStatusColumnPlaceholder: '蛇形列名を選択または入力（例 trip_status）',
    statusInProgress: '承認中の状態値',
    statusApproved: '承認済み状態値',
    statusRejected: '却下状態値',
    statusCancelled: '取消状態値',
    submitAllowedStatuses: '提出を許可する業務状態',
    submitAllowedStatusesPlaceholder: '状態値を入力して Enter（例 0、3）',
    requireDataTable: 'データテーブルを選択して列を読み込んでください',
    requireFormConfig: 'フォーム設計を完了してください',
    publishSuccess: 'フォームを公開しました',
    disableSuccess: 'フォームを無効化しました',
    loadDetailFailed: 'フォーム詳細の読み込みに失敗しました',
    loadFormConfigFailed: 'フォーム設定の取得に失敗しました',
    step: {
      formInfo: 'フォーム情報',
      dataSource: 'データソース',
      dataTableList: 'データテーブル',
      formDesign: 'フォーム設計',
      prev: '前へ',
      next: '次へ',
      done: '完了',
      validateFail: 'ステップ {step} の検証に失敗しました',
      completeRequired: '保存前に全ステップを完了してください',
      dataTableLoaded: '列項目を取得しました。次のステップでフォームを復元できます',
    },
  },
}
