// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：en-US.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Cross-tenant table clone page copy (code.database.tableDataClone.page.*)
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担 any use risk.
// ========================================

export default {
  page: {
    title: 'Cross-Tenant Table Clone',
    subtitle: 'Cross-tenant only; 1–5 tables per request; target tables are fully backed up then TRUNCATE cleared before clone',
    section: {
      scope: 'Tenants and Databases',
      tables: 'Table Mapping (1–5)',
      options: 'Clone Options',
      result: 'Execution Result',
    },
    field: {
      sourceTenant: 'Source Tenant',
      targetTenant: 'Target Tenant',
      sourceDatabase: 'Source Database',
      targetDatabase: 'Target Database',
      sourceTable: 'Source Table',
      targetTable: 'Target Table',
      preserveIdentity: 'Preserve identity column values (IDENTITY_INSERT)',
    },
    tableMapping: {
      addRow: 'Add Table',
      removeRow: 'Remove',
      maxHint: 'Up to 5 tables per request',
      actionColumn: 'Action',
    },
    crossTenantRequired: 'Table clone requires different source and target tenants',
    tableRequired: 'Configure at least one table mapping',
    action: {
      startClone: 'Start Clone',
      confirmExecute: 'Confirm and Execute',
    },
    backupModalTitle: 'Backup Window — Confirm Before Execute',
    cloneSuccess: 'Clone succeeded',
    cloneFailed: 'Clone failed',
    previewRequired: 'Open the backup window and confirm before executing',
    result: {
      backupTable: 'Backup Table',
      backedUpRows: 'Backed Up Rows',
      clearedRows: 'Cleared Rows',
      sourceRows: 'Source Rows',
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
