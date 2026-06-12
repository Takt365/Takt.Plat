// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：zh-CN.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例运行时页面静态文案（引用键 workflow.instance.page.*；字段标签走 entity.flowInstance.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    status: {
      0: '运行中',
      1: '已完成',
      2: '已驳回',
      3: '已挂起',
      4: '已终止',
      5: '草稿',
      unknown: '未知状态',
    },
    noHistory: '暂无流转记录',
    taskFormContent: '表单内容',
    formDataEmpty: '（空）',
    suspendReason: '挂起原因',
    suspendReasonPlaceholder: '请输入挂起原因（选填）',
    terminateReason: '终止原因',
    terminateReasonPlaceholder: '请输入终止原因（选填）',
    confirmResume: '确定恢复流程「{name}」吗？',
    confirmRevoke: '确定撤回流程「{name}」吗？',
    msg: {
      suspendSuccess: '挂起成功',
      resumeSuccess: '恢复成功',
      terminateSuccess: '终止成功',
      reduceSignSuccess: '减签成功',
      reduceSignFail: '减签失败',
    },
  },
}
