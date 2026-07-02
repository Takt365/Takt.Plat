// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/news-center/news/composables
// 文件名称：use-news-master-context.ts
// 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { News } from '@/types/routine/news-center/news'

/** 主表选中行上下文 */
export interface NewsMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<News | null>
}

const newsMasterContextKey: InjectionKey<NewsMasterContext> = Symbol('newsMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {NewsMasterContext} 主表上下文
 */
export function provideNewsMasterContext(): NewsMasterContext {
  const selectedMasterRow = ref<News | null>(null)
  const ctx: NewsMasterContext = { selectedMasterRow }
  provide(newsMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {NewsMasterContext} 主表上下文
 */
export function useNewsMasterContext(): NewsMasterContext {
  const ctx = inject(newsMasterContextKey)
  if (!ctx) {
    throw new Error('useNewsMasterContext must be used within news index')
  }
  return ctx
}
