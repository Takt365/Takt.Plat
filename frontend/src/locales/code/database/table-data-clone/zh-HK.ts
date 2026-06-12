// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：zh-HK.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租戶整表克隆頁靜態文案（引用鍵 code.database.tableDataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '跨租戶整表克隆',
    subtitle: '僅支持跨租戶；一次 1~5 張表；克隆前先備份再 TRUNCATE 清空目標整表',
    section: {
      scope: '租戶與數據庫',
      tables: '表映射（1~5 張）',
      options: '克隆選項',
      result: '執行結果',
    },
    field: {
      sourceTenant: '源租戶',
      targetTenant: '目標租戶',
      sourceDatabase: '源數據庫',
      targetDatabase: '目標數據庫',
      sourceTable: '源表',
      targetTable: '目標表',
      preserveIdentity: '保留自增列原值（IDENTITY_INSERT）',
    },
    tableMapping: {
      addRow: '添加表',
      removeRow: '刪除',
      maxHint: '一次最多 5 張表',
      actionColumn: '操作',
    },
    crossTenantRequired: '整表克隆僅支持跨租戶，源租戶與目標租戶不能相同',
    tableRequired: '請至少配置一張表映射',
    action: {
      startClone: '開始克隆',
      confirmExecute: '確認並執行克隆',
    },
    backupModalTitle: '備份窗口 — 請先確認再執行',
    cloneSuccess: '克隆成功',
    cloneFailed: '克隆失敗',
    previewRequired: '請先打開備份窗口並完成確認',
    result: {
      backupTable: '備份表',
      backedUpRows: '備份行數',
      clearedRows: '清空行數',
      sourceRows: '源表行數',
      clonedRows: '克隆行數',
      commonColumns: '同名列數',
      summary: '備份與清空摘要',
      targetRows: '目標現有行數',
      plannedBackupTable: '計劃備份表',
      totalSourceRows: '源表行數合計',
      totalClonedRows: '克隆行數合計',
      tableCount: '克隆表數量',
    },
  },
};
