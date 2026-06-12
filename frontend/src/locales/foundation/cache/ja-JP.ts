// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：ja-JP.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：キャッシュ管理ページ静的文案；参照キー foundation.cache.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'キャッシュ管理',
    description: '実行時キャッシュの設定と統計を表示し、キー単位で存在確認・削除を行います',
    section: {
      config: 'キャッシュ設定',
      statistics: 'キャッシュ統計',
      keyOps: 'キー操作',
    },
    field: {
      provider: 'プロバイダー',
      defaultExpirationMinutes: 'デフォルト有効期限（分）',
      enableSlidingExpiration: 'スライディング有効期限',
      enableMultiLevelCache: '多段キャッシュ',
      redisInstanceName: 'Redis インスタンス接頭辞',
      note: '説明',
      currentEntryCount: '現在のエントリ数',
      totalHits: 'ヒット数',
      totalMisses: 'ミス数',
      hitRate: 'ヒット率',
      estimatedSizeBytes: '推定サイズ（バイト）',
      cacheKey: 'キャッシュキー',
    },
    placeholder: {
      cacheKey: 'キャッシュキーを入力',
    },
    button: {
      checkExists: '存在確認',
      remove: 'キー削除',
    },
    rule: {
      cacheKeyRequired: 'キャッシュキーを入力してください',
    },
    message: {
      loadFail: 'キャッシュ情報の読み込みに失敗しました',
      checkFail: 'キャッシュキーの確認に失敗しました',
      removeSuccess: 'キャッシュキーを削除しました',
      removeFail: 'キャッシュキーの削除に失敗しました',
      loadingHint: '読み込み中…',
    },
    alert: {
      keyExists: 'キャッシュキーは存在します',
      keyNotExists: 'キャッシュキーは存在しません',
    },
  },
};
