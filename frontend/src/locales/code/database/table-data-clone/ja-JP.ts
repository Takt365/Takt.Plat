// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：ja-JP.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：テナント間テーブルクローンページ文言（code.database.tableDataClone.page.*）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'テナント間テーブルクローン',
    subtitle: 'テナント間のみ；1 回 1～5 表；クローン前にターゲット表を全量バックアップして TRUNCATE します',
    section: {
      scope: 'テナントとデータベース',
      tables: 'テーブルマッピング（1～5）',
      options: 'クローンオプション',
      result: '実行結果',
    },
    field: {
      sourceTenant: 'ソーステナント',
      targetTenant: 'ターゲットテナント',
      sourceDatabase: 'ソース DB',
      targetDatabase: 'ターゲット DB',
      sourceTable: 'ソース表',
      targetTable: 'ターゲット表',
      preserveIdentity: 'IDENTITY 列の値を保持（IDENTITY_INSERT）',
    },
    tableMapping: {
      addRow: '表を追加',
      removeRow: '削除',
      maxHint: '1 回最大 5 表',
      actionColumn: '操作',
    },
    crossTenantRequired: 'テーブルクローンはテナント間のみ。ソースとターゲットのテナントは同一にできません',
    tableRequired: '少なくとも 1 つの表マッピングを設定してください',
    action: {
      startClone: 'クローン開始',
      confirmExecute: '確認して実行',
    },
    backupModalTitle: 'バックアップウィンドウ — 実行前に確認',
    cloneSuccess: 'クローン成功',
    cloneFailed: 'クローン失敗',
    previewRequired: 'バックアップウィンドウで確認してから実行してください',
    result: {
      backupTable: 'バックアップ表',
      backedUpRows: 'バックアップ行数',
      clearedRows: '削除行数',
      sourceRows: 'ソース行数',
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
