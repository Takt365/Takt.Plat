// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：zh-CN.ts
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
      business: '业务表单',
      system: '系统表单',
      general: '通用表单',
    },
    type: {
      static: '静态表单',
      custom: '自定义表单',
      dynamic: '动态表单',
    },
    versionPlaceholder: '请输入版本号',
    dataSourcePlaceholder: '请输入数据源标识',
    dataTablePlaceholder: '请选择数据表',
    entityTableHint: '实体表字段将用于表单设计器',
    isDatasourceLabel: '绑定业务数据源',
    isDatasourceHint: '绑定后按物理表映射字段，并支持审批回写',
    businessBindingTitle: '业务状态与提交规则',
    businessStatusColumn: '业务状态列',
    businessStatusColumnPlaceholder: '选择或输入蛇形列名（如 trip_status）',
    statusInProgress: '审批中状态值',
    statusApproved: '已通过状态值',
    statusRejected: '已驳回状态值',
    statusCancelled: '已撤销状态值',
    submitAllowedStatuses: '允许提交审批的业务状态',
    submitAllowedStatusesPlaceholder: '输入状态值后回车（如 0、3）',
    requireDataTable: '请选择数据表并加载字段',
    requireFormConfig: '请完成表单设计',
    publishSuccess: '表单已发布',
    disableSuccess: '表单已停用',
    loadDetailFailed: '加载表单详情失败',
    loadFormConfigFailed: '获取表单配置失败',
    step: {
      formInfo: '表单信息',
      dataSource: '数据源',
      dataTableList: '数据表',
      formDesign: '表单设计',
      prev: '上一步',
      next: '下一步',
      done: '完成',
      validateFail: '第 {step} 步校验未通过',
      completeRequired: '请完成全部步骤后再保存',
      dataTableLoaded: '已获取所有数据列项，下一步可还原表单',
    },
  },
}
