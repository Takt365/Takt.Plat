// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：zh-CN.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表克隆页静态文案（引用键 code.database.tableDataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '跨租户整表克隆',
    subtitle: '仅支持跨租户；一次 1~5 张表；克隆前先备份再 TRUNCATE 清空目标整表',
    section: {
      scope: '租户与数据库',
      tables: '表映射（1~5 张）',
      options: '克隆选项',
      result: '执行结果',
    },
    field: {
      sourceTenant: '源租户',
      targetTenant: '目标租户',
      sourceDatabase: '源数据库',
      targetDatabase: '目标数据库',
      sourceTable: '源表',
      targetTable: '目标表',
      preserveIdentity: '保留自增列原值（IDENTITY_INSERT）',
    },
    tableMapping: {
      addRow: '添加表',
      removeRow: '删除',
      maxHint: '一次最多 5 张表',
      actionColumn: '操作',
    },
    crossTenantRequired: '整表克隆仅支持跨租户，源租户与目标租户不能相同',
    tableRequired: '请至少配置一张表映射',
    action: {
      startClone: '开始克隆',
      confirmExecute: '确认并执行克隆',
    },
    backupModalTitle: '备份窗口 — 请先确认再执行',
    cloneSuccess: '克隆成功',
    cloneFailed: '克隆失败',
    previewRequired: '请先打开备份窗口并完成确认',
    result: {
      backupTable: '备份表',
      backedUpRows: '备份行数',
      clearedRows: '清空行数',
      sourceRows: '源表行数',
      clonedRows: '克隆行数',
      commonColumns: '同名列数',
      summary: '备份与清空摘要',
      targetRows: '目标现有行数',
      plannedBackupTable: '计划备份表',
      totalSourceRows: '源表行数合计',
      totalClonedRows: '克隆行数合计',
      tableCount: '克隆表数量',
    },
  },
};
