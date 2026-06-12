// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：zh-HK.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司級數據克隆頁靜態文案（引用鍵 code.database.dataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '公司級數據克隆',
    subtitle: '一次一個源公司、一張表 → 一個目標公司、一張表；克隆前先備份再清空目標公司數據',
    section: {
      source: '源範圍',
      target: '目標範圍',
      options: '克隆選項',
      result: '執行結果',
    },
    field: {
      tenant: '租戶',
      database: '數據庫',
      table: '數據表',
      company: '公司編碼',
      preserveIdentity: '保留自增列原值（IDENTITY_INSERT）',
    },
    action: {
      startClone: '開始克隆',
      confirmExecute: '確認並執行克隆',
    },
    backupModalTitle: '備份窗口 — 請先確認再執行',
    backupSummary: '備份與清空說明',
    sameScopeError: '源與目標的租戶、數據庫、數據表、公司編碼不能完全相同',
    previewRequired: '請先打開備份窗口並完成確認',
    cloneSuccess: '克隆成功',
    cloneFailed: '克隆失敗',
    result: {
      backupTable: '備份表',
      backedUpRows: '備份行數',
      clearedRows: '清空行數',
      sourceRows: '源公司行數',
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
