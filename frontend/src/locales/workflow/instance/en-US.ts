// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：en-US.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Flow instance runtime page static copy (keys workflow.instance.page.*)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    status: {
      0: 'Running',
      1: 'Completed',
      2: 'Rejected',
      3: 'Suspended',
      4: 'Terminated',
      5: 'Draft',
      unknown: 'Unknown',
    },
    noHistory: 'No transition history',
    taskFormContent: 'Form content',
    formDataEmpty: '(empty)',
    suspendReason: 'Suspend reason',
    suspendReasonPlaceholder: 'Enter suspend reason (optional)',
    terminateReason: 'Terminate reason',
    terminateReasonPlaceholder: 'Enter terminate reason (optional)',
    confirmResume: 'Resume process "{name}"?',
    confirmRevoke: 'Revoke process "{name}"?',
    msg: {
      suspendSuccess: 'Suspended successfully',
      resumeSuccess: 'Resumed successfully',
      terminateSuccess: 'Terminated successfully',
      reduceSignSuccess: 'Sign-off reduced successfully',
      reduceSignFail: 'Failed to reduce sign-off',
    },
  },
}
