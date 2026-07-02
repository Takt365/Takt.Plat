// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/routine/news-center/news-comment/composables
// 文件名称：use-news-comment-master-context.ts
// 功能描述：新闻中心评论实体 支持多级回复主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { NewsComment } from '@/types/routine/news-center/news-comment'

/** 主表选中行上下文 */
export interface NewsCommentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<NewsComment | null>
}

const newsCommentMasterContextKey: InjectionKey<NewsCommentMasterContext> = Symbol('news-commentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {NewsCommentMasterContext} 主表上下文
 */
export function provideNewsCommentMasterContext(): NewsCommentMasterContext {
  const selectedMasterRow = ref<NewsComment | null>(null)
  const ctx: NewsCommentMasterContext = { selectedMasterRow }
  provide(newsCommentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {NewsCommentMasterContext} 主表上下文
 */
export function useNewsCommentMasterContext(): NewsCommentMasterContext {
  const ctx = inject(newsCommentMasterContextKey)
  if (!ctx) {
    throw new Error('useNewsCommentMasterContext must be used within news-comment index')
  }
  return ctx
}
