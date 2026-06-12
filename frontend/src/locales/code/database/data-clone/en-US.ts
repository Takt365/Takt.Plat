// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：en-US.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Company-scoped data clone page copy (code.database.dataClone.page.*)
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Company Data Clone',
    subtitle: 'One source company and one table to one target company and one table; target company data is backed up then cleared before clone',
    section: {
      source: 'Source Scope',
      target: 'Target Scope',
      options: 'Clone Options',
      result: 'Execution Result',
    },
    field: {
      tenant: 'Tenant',
      database: 'Database',
      table: 'Table',
      company: 'Company Code',
      preserveIdentity: 'Preserve identity column values (IDENTITY_INSERT)',
    },
    action: {
      startClone: 'Start Clone',
      confirmExecute: 'Confirm and Execute',
    },
    backupModalTitle: 'Backup Window — Confirm Before Execute',
    backupSummary: 'Backup and Clear Steps',
    sameScopeError: 'Source and target tenant, database, table, and company code cannot be identical',
    previewRequired: 'Open the backup window and confirm before executing',
    cloneSuccess: 'Clone succeeded',
    cloneFailed: 'Clone failed',
    result: {
      backupTable: 'Backup Table',
      backedUpRows: 'Backed Up Rows',
      clearedRows: 'Cleared Rows',
      sourceRows: 'Source Company Rows',
      clonedRows: 'Cloned Rows',
      commonColumns: 'Common Columns',
      summary: 'Backup and Clear Summary',
      targetRows: 'Target Existing Rows',
      plannedBackupTable: 'Planned Backup Table',
      totalSourceRows: 'Total Source Rows',
      totalClonedRows: 'Total Cloned Rows',
      tableCount: 'Table Count',
    },
  },
};
