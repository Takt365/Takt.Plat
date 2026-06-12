// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：zh-HK.ts
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
      0: '運行中',
      1: '已完成',
      2: '已駁回',
      3: '已掛起',
      4: '已終止',
      5: '草稿',
      unknown: '未知狀態',
    },
    noHistory: '暫無流轉記錄',
    taskFormContent: '表單內容',
    formDataEmpty: '（空）',
    suspendReason: '掛起原因',
    suspendReasonPlaceholder: '請輸入掛起原因（選填）',
    terminateReason: '終止原因',
    terminateReasonPlaceholder: '請輸入終止原因（選填）',
    confirmResume: '確定恢復流程「{name}」嗎？',
    confirmRevoke: '確定撤回流程「{name}」嗎？',
    msg: {
      suspendSuccess: '掛起成功',
      resumeSuccess: '恢復成功',
      terminateSuccess: '終止成功',
      reduceSignSuccess: '減簽成功',
      reduceSignFail: '減簽失敗',
    },
  },
}
