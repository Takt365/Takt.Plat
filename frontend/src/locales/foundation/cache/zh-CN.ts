// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：zh-CN.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理页面静态文案；引用键 foundation.cache.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '缓存管理',
    description: '查看运行时缓存配置与统计，并按键检查或移除缓存项',
    section: {
      config: '缓存配置',
      statistics: '缓存统计',
      keyOps: '按键操作',
    },
    field: {
      provider: '缓存提供者',
      defaultExpirationMinutes: '默认过期（分钟）',
      enableSlidingExpiration: '滑动过期',
      enableMultiLevelCache: '多级缓存',
      redisInstanceName: 'Redis 实例前缀',
      note: '说明',
      currentEntryCount: '当前条目数',
      totalHits: '命中次数',
      totalMisses: '未命中次数',
      hitRate: '命中率',
      estimatedSizeBytes: '估算占用（字节）',
      cacheKey: '缓存键',
    },
    placeholder: {
      cacheKey: '请输入完整缓存键',
    },
    button: {
      checkExists: '检查存在',
      remove: '移除键',
    },
    rule: {
      cacheKeyRequired: '请输入缓存键',
    },
    message: {
      loadFail: '加载缓存信息失败',
      checkFail: '检查缓存键失败',
      removeSuccess: '缓存键已移除',
      removeFail: '移除缓存键失败',
      loadingHint: '加载中…',
    },
    alert: {
      keyExists: '缓存键存在',
      keyNotExists: '缓存键不存在',
    },
  },
};
