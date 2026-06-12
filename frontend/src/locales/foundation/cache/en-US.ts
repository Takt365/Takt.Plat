// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：en-US.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Cache management page static copy; keys foundation.cache.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Cache Management',
    description: 'Inspect runtime cache configuration and statistics; check or remove entries by key',
    section: {
      config: 'Configuration',
      statistics: 'Statistics',
      keyOps: 'Key Operations',
    },
    field: {
      provider: 'Provider',
      defaultExpirationMinutes: 'Default expiration (minutes)',
      enableSlidingExpiration: 'Sliding expiration',
      enableMultiLevelCache: 'Multi-level cache',
      redisInstanceName: 'Redis instance prefix',
      note: 'Note',
      currentEntryCount: 'Current entries',
      totalHits: 'Hits',
      totalMisses: 'Misses',
      hitRate: 'Hit rate',
      estimatedSizeBytes: 'Estimated size (bytes)',
      cacheKey: 'Cache key',
    },
    placeholder: {
      cacheKey: 'Enter the full cache key',
    },
    button: {
      checkExists: 'Check exists',
      remove: 'Remove key',
    },
    rule: {
      cacheKeyRequired: 'Cache key is required',
    },
    message: {
      loadFail: 'Failed to load cache information',
      checkFail: 'Failed to check cache key',
      removeSuccess: 'Cache key removed',
      removeFail: 'Failed to remove cache key',
      loadingHint: 'Loading…',
    },
    alert: {
      keyExists: 'Cache key exists',
      keyNotExists: 'Cache key does not exist',
    },
  },
};
