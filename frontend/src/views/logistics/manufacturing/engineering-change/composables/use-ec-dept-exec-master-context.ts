// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：use-ec-dept-exec-master-context.ts
// 功能描述：执行部门页左栏设变明细主表选中行上下文（供右侧部门执行行面板读取）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ec-detail'

/** 执行部门页主表选中行上下文 */
export interface EcDeptExecMasterContext {
  /** 当前选中的设变明细行（TaktEcDetail） */
  selectedMasterRow: Ref<EcDetail | null>
}

const ecDeptExecMasterContextKey: InjectionKey<EcDeptExecMasterContext> = Symbol('ecDeptExecMasterContext')

/**
 * 在执行部门列表页 provide 选中主表行
 * @returns {EcDeptExecMasterContext} 主表上下文
 */
export function provideEcDeptExecMasterContext(): EcDeptExecMasterContext {
  const selectedMasterRow = ref<EcDetail | null>(null)
  const ctx: EcDeptExecMasterContext = { selectedMasterRow }
  provide(ecDeptExecMasterContextKey, ctx)
  return ctx
}

/**
 * 在右侧执行行面板 inject 主表选中行
 * @returns {EcDeptExecMasterContext} 主表上下文
 */
export function useEcDeptExecMasterContext(): EcDeptExecMasterContext {
  const ctx = inject(ecDeptExecMasterContextKey)
  if (!ctx) {
    throw new Error('useEcDeptExecMasterContext must be used within ec-dept-exec-lr-page')
  }
  return ctx
}
