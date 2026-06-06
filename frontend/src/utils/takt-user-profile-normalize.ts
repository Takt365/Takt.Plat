// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-user-profile-normalize.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：GET /me 用户资料兜底（空数组等，不做菜单字段映射）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { UserInfoResponse } from '@/types/identity/login';

/**
 * 归一化 GET /me 用户资料（仅补齐可能为空的数组字段）
 * @param {UserInfoResponse} profile 接口响应（已与 UserInfoResponse / 后端 camelCase 对齐）
 * @returns {UserInfoResponse} 数组字段非 null 的用户资料
 */
export function normalizeUserInfoProfile(profile: UserInfoResponse): UserInfoResponse {
  // 展开原对象并兜底可能为 null/undefined 的集合字段
  return {
    ...profile,
    // 角色列表缺省为空数组
    roles: profile.roles ?? [],
    // 权限码列表缺省为空数组
    permissions: profile.permissions ?? [],
    // 可访问路由路径缺省为空数组
    routePaths: profile.routePaths ?? [],
    // 菜单树缺省为空数组
    menus: profile.menus ?? [],
    // 可访问公司列表缺省为空数组
    accessibleCompanies: profile.accessibleCompanies ?? [],
  };
}
