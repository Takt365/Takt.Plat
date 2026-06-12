// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：en-US.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Flow form page static copy (keys workflow.form.page.*)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此 software uses MIT License.
// ========================================

export default {
  page: {
    category: {
      business: 'Business form',
      system: 'System form',
      general: 'General form',
    },
    type: {
      static: 'Static form',
      custom: 'Custom form',
      dynamic: 'Dynamic form',
    },
    versionPlaceholder: 'Enter version',
    dataSourcePlaceholder: 'Enter data source',
    dataTablePlaceholder: 'Select data table',
    entityTableHint: 'Entity table columns are used in the form designer',
    isDatasourceLabel: 'Bind business data source',
    isDatasourceHint: 'Maps physical table columns and enables approval write-back',
    businessBindingTitle: 'Business status & submit rules',
    businessStatusColumn: 'Business status column',
    businessStatusColumnPlaceholder: 'Select or enter snake_case column (e.g. trip_status)',
    statusInProgress: 'In-progress status value',
    statusApproved: 'Approved status value',
    statusRejected: 'Rejected status value',
    statusCancelled: 'Cancelled status value',
    submitAllowedStatuses: 'Statuses allowed to submit',
    submitAllowedStatusesPlaceholder: 'Type a value and press Enter (e.g. 0, 3)',
    requireDataTable: 'Select a data table and load columns',
    requireFormConfig: 'Complete the form designer',
    publishSuccess: 'Form published',
    disableSuccess: 'Form disabled',
    loadDetailFailed: 'Failed to load form detail',
    loadFormConfigFailed: 'Failed to load form configuration',
    step: {
      formInfo: 'Form info',
      dataSource: 'Data source',
      dataTableList: 'Data tables',
      formDesign: 'Form design',
      prev: 'Previous',
      next: 'Next',
      done: 'Done',
      validateFail: 'Step {step} validation failed',
      completeRequired: 'Complete all steps before saving',
      dataTableLoaded: 'Columns loaded. Continue to restore the form.',
    },
  },
}
