// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：ja-JP.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会社単位データクローンページ文言（code.database.dataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '会社単位データクローン',
    subtitle: '1 ソース会社・1 表 → 1 ターゲット会社・1 表。クローン前にターゲット会社データをバックアップしてから削除します',
    section: {
      source: 'ソース範囲',
      target: 'ターゲット範囲',
      options: 'クローンオプション',
      result: '実行結果',
    },
    field: {
      tenant: 'テナント',
      database: 'データベース',
      table: 'テーブル',
      company: '会社コード',
      preserveIdentity: 'IDENTITY 列の値を保持（IDENTITY_INSERT）',
    },
    action: {
      startClone: 'クローン開始',
      confirmExecute: '確認して実行',
    },
    backupModalTitle: 'バックアップウィンドウ — 実行前に確認',
    backupSummary: 'バックアップと削除の手順',
    sameScopeError: 'ソースとターゲットのテナント・DB・表・会社コードをすべて同一にできません',
    previewRequired: 'バックアップウィンドウで確認してから実行してください',
    cloneSuccess: 'クローン成功',
    cloneFailed: 'クローン失敗',
    result: {
      backupTable: 'バックアップ表',
      backedUpRows: 'バックアップ行数',
      clearedRows: '削除行数',
      sourceRows: 'ソース会社行数',
      clonedRows: 'クローン行数',
      commonColumns: '共通列数',
      summary: 'バックアップ・削除サマリー',
      targetRows: 'ターゲット既存行数',
      plannedBackupTable: '予定バックアップ表',
      totalSourceRows: 'ソース行数合計',
      totalClonedRows: 'クローン行数合計',
      tableCount: 'クローン表数',
    },
  },
};
