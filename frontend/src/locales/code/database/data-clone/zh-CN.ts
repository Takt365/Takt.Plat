// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：zh-CN.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆页静态文案（引用键 code.database.dataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '公司级数据克隆',
    subtitle: '一次一个源公司、一张表 → 一个目标公司、一张表；克隆前先备份再清空目标公司数据',
    section: {
      source: '源范围',
      target: '目标范围',
      options: '克隆选项',
      result: '执行结果',
    },
    field: {
      tenant: '租户',
      database: '数据库',
      table: '数据表',
      company: '公司编码',
      preserveIdentity: '保留自增列原值（IDENTITY_INSERT）',
    },
    action: {
      startClone: '开始克隆',
      confirmExecute: '确认并执行克隆',
    },
    backupModalTitle: '备份窗口 — 请先确认再执行',
    backupSummary: '备份与清空说明',
    sameScopeError: '源与目标的租户、数据库、数据表、公司编码不能完全相同',
    previewRequired: '请先打开备份窗口并完成确认',
    cloneSuccess: '克隆成功',
    cloneFailed: '克隆失败',
    result: {
      backupTable: '备份表',
      backedUpRows: '备份行数',
      clearedRows: '清空行数',
      sourceRows: '源公司行数',
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
