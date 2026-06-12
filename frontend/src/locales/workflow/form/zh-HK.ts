// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：zh-HK.ts
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
      business: '業務表單',
      system: '系統表單',
      general: '通用表單',
    },
    type: {
      static: '靜態表單',
      custom: '自定義表單',
      dynamic: '動態表單',
    },
    versionPlaceholder: '請輸入版本號',
    dataSourcePlaceholder: '請輸入數據源標識',
    dataTablePlaceholder: '請選擇數據表',
    entityTableHint: '實體表字段將用於表單設計器',
    isDatasourceLabel: '綁定業務數據源',
    isDatasourceHint: '按物理表映射字段，並支持審批回寫',
    businessBindingTitle: '業務狀態與提交規則',
    businessStatusColumn: '業務狀態列',
    businessStatusColumnPlaceholder: '選擇或輸入蛇形列名（如 trip_status）',
    statusInProgress: '審批中狀態值',
    statusApproved: '已通過狀態值',
    statusRejected: '已駁回狀態值',
    statusCancelled: '已撤銷狀態值',
    submitAllowedStatuses: '允許提交審批的業務狀態',
    submitAllowedStatusesPlaceholder: '輸入狀態值後回車（如 0、3）',
    requireDataTable: '請選擇數據表並加載字段',
    requireFormConfig: '請完成表單設計',
    publishSuccess: '表單已發布',
    disableSuccess: '表單已停用',
    loadDetailFailed: '加載表單詳情失敗',
    loadFormConfigFailed: '獲取表單配置失敗',
    step: {
      formInfo: '表單信息',
      dataSource: '數據源',
      dataTableList: '數據表',
      formDesign: '表單設計',
      prev: '上一步',
      next: '下一步',
      done: '完成',
      validateFail: '第 {step} 步校驗未通過',
      completeRequired: '請完成全部步驟後再保存',
      dataTableLoaded: '已獲取所有數據列項，下一步可還原表單',
    },
  },
}
