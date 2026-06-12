// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：ja-JP.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例运行时页面静态文案（引用键 workflow.instance.page.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    status: {
      0: '実行中',
      1: '完了',
      2: '却下',
      3: '保留',
      4: '終了',
      5: '下書き',
      unknown: '不明',
    },
    noHistory: '履歴がありません',
    taskFormContent: 'フォーム内容',
    formDataEmpty: '（空）',
    suspendReason: '保留理由',
    suspendReasonPlaceholder: '保留理由を入力（任意）',
    terminateReason: '終了理由',
    terminateReasonPlaceholder: '終了理由を入力（任意）',
    confirmResume: 'プロセス「{name}」を再開しますか？',
    confirmRevoke: 'プロセス「{name}」を取り消しますか？',
    msg: {
      suspendSuccess: '保留しました',
      resumeSuccess: '再開しました',
      terminateSuccess: '終了しました',
      reduceSignSuccess: '減签しました',
      reduceSignFail: '減签に失敗しました',
    },
  },
}
