// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/utils
// 文件名称：takt-module-root-menu.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：一级目录菜单解析（业务域、上传路径 uploads/{RoutePath首段} 与侧栏菜单联动）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TaktMenuType } from '@/utils/common'
import type { MenuTree } from '@/types/identity/menu'
import { translateLocaleMessage } from '@/utils/takt-i18n-message'
import { normalizeFileTagList } from '@/utils/takt-file-tags'

/**
 * 一级目录 MenuCode → reportDomain（TaktModule 整型，不含仪表盘）
 */
export const REPORT_DOMAIN_MENU_CODES: Readonly<Record<string, number>> = {
  IDENTITY: 1,
  ROUTINE: 2,
  ACCOUNTING: 3,
  LOGISTICS: 4,
  HUMAN_RESOURCE: 5,
  WORKFLOW: 6,
  CODE: 7,
  FOUNDATION: 8,
  STATISTICS: 9,
}

/** reportDomain → 一级目录 MenuCode */
export const MENU_CODE_BY_REPORT_DOMAIN: Readonly<Record<number, string>> = Object.fromEntries(
  Object.entries(REPORT_DOMAIN_MENU_CODES).map(([code, domain]) => [domain, code])
) as Record<number, string>

/**
 * 菜单展示文案（优先 i18nKey）
 * @param menu 菜单节点
 * @returns {string} 展示文本
 */
export function getRootMenuLabel(menu: MenuTree): string {
  if (menu.i18nKey) {
    return translateLocaleMessage(menu.i18nKey)
  }
  return menu.menuName
}

/**
 * 一级目录菜单（ParentId=0、MenuType=目录、可映射业务模块）
 * @param menus 菜单树根列表
 * @returns {MenuTree[]} 排序后的一级目录
 */
export function listRootModuleDirectoryMenus(menus: readonly MenuTree[]): MenuTree[] {
  return menus
    .filter(
      (menu) =>
        menu.menuType === TaktMenuType.Directory &&
        REPORT_DOMAIN_MENU_CODES[menu.menuCode] != null
    )
    .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
}

/**
 * 由一级菜单 RoutePath / ComponentPath 首段生成上传相对路径（保留连字符，如 uploads/human-resource）
 * @param menu 一级目录菜单
 * @returns {string} 上传路径
 */
export function buildMenuUploadPath(menu: MenuTree): string {
  const route = menu.routePath?.trim() || menu.componentPath?.trim() || ''
  const firstSegment = route.replace(/^\/+/, '').split('/').filter(Boolean)[0] ?? ''
  const slug = firstSegment.toLowerCase()
  if (!slug) {
    return `uploads/${menu.menuCode.replace(/_/g, '-').toLowerCase()}`
  }
  return `uploads/${slug}`
}

/**
 * 上传路径规范化（用于兼容旧数据 uploads/humanresource 与 uploads/human-resource）
 * @param uploadPath 上传路径
 * @returns {string} 去连字符小写
 */
function normalizeUploadPathForMatch(uploadPath: string): string {
  return uploadPath.trim().toLowerCase().replace(/-/g, '')
}

/**
 * 上传路径下拉选项（一级目录菜单）
 * @param menus 菜单树根列表
 * @returns 选项列表
 */
export function buildUploadPathSelectOptions(menus: readonly MenuTree[]): { label: string; value: string }[] {
  return listRootModuleDirectoryMenus(menus).map((menu) => {
    const path = buildMenuUploadPath(menu)
    return {
      label: `${getRootMenuLabel(menu)} · ${path}`,
      value: path,
    }
  })
}

/**
 * 解析上传路径展示名
 * @param uploadPath 上传路径（如 uploads/foundation）
 * @param menus 菜单树根列表
 * @returns {string} 展示文本；无法解析时回退路径本身
 */
export function resolveUploadPathLabel(
  uploadPath?: string | null,
  menus?: readonly MenuTree[]
): string {
  const path = uploadPath?.trim()
  if (!path || !menus?.length) {
    return path ?? ''
  }
  const normalizedPath = normalizeUploadPathForMatch(path)
  const matched = listRootModuleDirectoryMenus(menus).find(
    (menu) =>
      buildMenuUploadPath(menu) === path ||
      normalizeUploadPathForMatch(buildMenuUploadPath(menu)) === normalizedPath
  )
  return matched ? `${getRootMenuLabel(matched)} · ${path}` : path
}

/**
 * 上传路径 uploads/{slug} 首段 slug（如 routine、human-resource）
 * @param uploadPath 上传路径
 * @returns {string} 路径 slug
 */
export function extractUploadPathSlug(uploadPath?: string | null): string {
  if (!uploadPath?.trim()) {
    return ''
  }
  const withoutPrefix = uploadPath.trim().replace(/^uploads\/?/i, '').replace(/^\/+|\/+$/g, '')
  const segments = withoutPrefix.split('/').filter(Boolean)
  return segments[0]?.toLowerCase() ?? ''
}

/**
 * 根据上传路径生成默认文件标签：一级菜单展示名（如日常事务）+ 路径 slug（如 routine）
 * @param uploadPath 上传路径（如 uploads/routine）
 * @param menus 菜单树根列表
 * @returns 默认标签数组
 */
export function buildDefaultFileTagsFromUploadPath(
  uploadPath?: string | null,
  menus?: readonly MenuTree[]
): string[] {
  const path = uploadPath?.trim()
  const slug = extractUploadPathSlug(path)
  if (!slug) {
    return []
  }
  let menuLabel = ''
  if (path && menus?.length) {
    const normalizedPath = normalizeUploadPathForMatch(path)
    const matched = listRootModuleDirectoryMenus(menus).find(
      (menu) =>
        buildMenuUploadPath(menu) === path ||
        normalizeUploadPathForMatch(buildMenuUploadPath(menu)) === normalizedPath
    )
    if (matched) {
      menuLabel = getRootMenuLabel(matched)
    }
  }
  const tags: string[] = []
  if (menuLabel) {
    tags.push(menuLabel)
  }
  tags.push(slug)
  return normalizeFileTagList(tags)
}

/**
 * 从路由路径取末段作为 reportSubCategory（与实体注释对齐）
 * @param routePath 菜单 RoutePath
 * @returns {string} 末段路径
 */
export function extractReportSubCategoryFromRoute(routePath?: string): string {
  if (!routePath?.trim()) {
    return ''
  }
  const parts = routePath.split('/').filter(Boolean)
  return parts[parts.length - 1] ?? ''
}

/**
 * 根据 reportDomain 解析一级模块菜单
 * @param menus 菜单树根列表
 * @param reportDomain 报表业务域
 * @returns {MenuTree | undefined} 一级目录菜单
 */
export function findReportModuleMenu(menus: readonly MenuTree[], reportDomain?: number | null): MenuTree | undefined {
  if (reportDomain == null) {
    return undefined
  }
  const menuCode = MENU_CODE_BY_REPORT_DOMAIN[reportDomain]
  if (!menuCode) {
    return undefined
  }
  return menus.find((item) => item.menuCode === menuCode)
}

/**
 * 解析子分类展示名
 * @param reportDomain 报表业务域
 * @param reportSubCategory 子分类路由末段
 * @param menus 菜单树
 * @returns {string} 展示文本；无法解析时回退子分类码
 */
export function resolveReportSubCategoryLabel(
  reportDomain?: number | null,
  reportSubCategory?: string | null,
  menus?: readonly MenuTree[]
): string {
  if (!reportSubCategory?.trim() || !menus?.length) {
    return reportSubCategory?.trim() ?? ''
  }
  const moduleMenu = findReportModuleMenu(menus, reportDomain)
  if (!moduleMenu?.children?.length) {
    return reportSubCategory
  }
  const matched = moduleMenu.children.find(
    (child) =>
      child.menuType === TaktMenuType.Directory &&
      extractReportSubCategoryFromRoute(child.routePath) === reportSubCategory
  )
  return matched ? getRootMenuLabel(matched) : reportSubCategory
}

/**
 * 解析业务域（一级模块菜单）展示名
 * @param reportDomain 报表业务域
 * @param menus 菜单树
 * @returns {string} 展示文本；无法解析时回退数字字符串
 */
export function resolveReportDomainLabel(
  reportDomain?: number | null,
  menus?: readonly MenuTree[]
): string {
  if (reportDomain == null) {
    return ''
  }
  if (!menus?.length) {
    return String(reportDomain)
  }
  const moduleMenu = findReportModuleMenu(menus, reportDomain)
  if (moduleMenu) {
    return getRootMenuLabel(moduleMenu)
  }
  return String(reportDomain)
}
