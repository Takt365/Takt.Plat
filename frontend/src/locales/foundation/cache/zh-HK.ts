// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：zh-HK.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：緩存管理頁面靜態文案；引用鍵 foundation.cache.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '緩存管理',
    description: '查看運行時緩存配置與統計，並按鍵檢查或移除緩存項',
    section: {
      config: '緩存配置',
      statistics: '緩存統計',
      keyOps: '按鍵操作',
    },
    field: {
      provider: '緩存提供者',
      defaultExpirationMinutes: '預設過期（分鐘）',
      enableSlidingExpiration: '滑動過期',
      enableMultiLevelCache: '多級緩存',
      redisInstanceName: 'Redis 實例前綴',
      note: '說明',
      currentEntryCount: '目前條目數',
      totalHits: '命中次數',
      totalMisses: '未命中次數',
      hitRate: '命中率',
      estimatedSizeBytes: '估算佔用（位元組）',
      cacheKey: '緩存鍵',
    },
    placeholder: {
      cacheKey: '請輸入完整緩存鍵',
    },
    button: {
      checkExists: '檢查存在',
      remove: '移除鍵',
    },
    rule: {
      cacheKeyRequired: '請輸入緩存鍵',
    },
    message: {
      loadFail: '載入緩存資訊失敗',
      checkFail: '檢查緩存鍵失敗',
      removeSuccess: '緩存鍵已移除',
      removeFail: '移除緩存鍵失敗',
      loadingHint: '載入中…',
    },
    alert: {
      keyExists: '緩存鍵存在',
      keyNotExists: '緩存鍵不存在',
    },
  },
};
