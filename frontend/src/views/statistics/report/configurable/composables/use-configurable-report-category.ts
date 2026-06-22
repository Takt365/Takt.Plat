// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/views/statistics/report/configurable/composables
// 文件名称：use-configurable-report-category.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：SQVI 报表业务域/子分类与侧栏菜单联动（根目录=模块，下级目录=子分类）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed, watch, type Ref } from 'vue'
import { useMenuStore } from '@/stores/identity/menu'
import { TaktMenuType } from '@/utils/common'
import type { MenuTree } from '@/types/identity/menu'
import {
  REPORT_DOMAIN_MENU_CODES,
  MENU_CODE_BY_REPORT_DOMAIN,
  getRootMenuLabel,
  findReportModuleMenu,
  extractReportSubCategoryFromRoute,
  resolveReportSubCategoryLabel,
  resolveReportDomainLabel,
} from '@/utils/takt-module-root-menu'

export { REPORT_DOMAIN_MENU_CODES, MENU_CODE_BY_REPORT_DOMAIN }

/** 菜单展示文案（兼容导出） */
export function getConfigurableReportMenuLabel(menu: MenuTree): string {
  return getRootMenuLabel(menu)
}

export { extractReportSubCategoryFromRoute, findReportModuleMenu, resolveReportSubCategoryLabel, resolveReportDomainLabel }

/**
 * SQVI 报表业务域与子分类联动（基于登录菜单树，reportDomain 存 TaktModule 整型）
 * @param reportDomainRef 报表业务域
 * @param reportSubCategoryRef 报表子分类（路由末段）；传入时在模块切换时校验并清空无效值
 * @returns 模块/子分类下拉选项与解析函数
 */
export function useConfigurableReportCategory(
  reportDomainRef: Ref<number | undefined | null>,
  reportSubCategoryRef?: Ref<string | undefined | null>
) {
  const menuStore = useMenuStore()

  /** 一级目录（ParentId=0、MenuType=目录）→ 模块选项 */
  const moduleOptions = computed(() =>
    menuStore.menuList
      .filter(
        (menu) =>
          menu.menuType === TaktMenuType.Directory &&
          REPORT_DOMAIN_MENU_CODES[menu.menuCode] != null
      )
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .map((menu) => ({
        label: getRootMenuLabel(menu),
        value: REPORT_DOMAIN_MENU_CODES[menu.menuCode],
      }))
  )

  /** 当前选中模块对应的一级菜单 */
  const selectedModuleMenu = computed(() =>
    findReportModuleMenu(menuStore.menuList, reportDomainRef.value ?? undefined)
  )

  /** 选中模块下直接子级目录（MenuType=0）→ 子分类选项 */
  const subCategoryOptions = computed(() => {
    const moduleMenu = selectedModuleMenu.value
    if (!moduleMenu?.children?.length) {
      return []
    }
    return moduleMenu.children
      .filter((child) => child.menuType === TaktMenuType.Directory)
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .map((child) => ({
        label: getRootMenuLabel(child),
        value: extractReportSubCategoryFromRoute(child.routePath),
      }))
      .filter((option) => option.value)
  })

  if (reportSubCategoryRef) {
    watch(reportDomainRef, () => {
      const sub = reportSubCategoryRef.value?.trim()
      if (!sub) {
        return
      }
      const valid = subCategoryOptions.value.some((option) => option.value === sub)
      if (!valid) {
        reportSubCategoryRef.value = ''
      }
    })
  }

  /**
   * 解析子分类展示名（当前菜单树）
   * @param reportDomain 报表业务域
   * @param reportSubCategory 子分类路由末段
   * @returns {string} 展示文本
   */
  function resolveSubCategoryLabel(reportDomain?: number | null, reportSubCategory?: string | null): string {
    return resolveReportSubCategoryLabel(reportDomain, reportSubCategory, menuStore.menuList)
  }

  /**
   * 解析业务域展示名（当前菜单树）
   * @param reportDomain 报表业务域
   * @returns {string} 展示文本
   */
  function resolveDomainLabel(reportDomain?: number | null): string {
    return resolveReportDomainLabel(reportDomain, menuStore.menuList)
  }

  return {
    moduleOptions,
    subCategoryOptions,
    resolveSubCategoryLabel,
    resolveDomainLabel,
  }
}
