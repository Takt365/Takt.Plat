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
 * @param profile 接口响应（已与 UserInfoResponse / 后端 camelCase 对齐）
 */
export function normalizeUserInfoProfile(profile: UserInfoResponse): UserInfoResponse {
  return {
    ...profile,
    roles: profile.roles ?? [],
    permissions: profile.permissions ?? [],
    routePaths: profile.routePaths ?? [],
    menus: profile.menus ?? [],
    accessibleCompanies: profile.accessibleCompanies ?? [],
  };
}
