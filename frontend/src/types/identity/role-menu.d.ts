// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/identity
// 文件名称：role-menu.d.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktRoleMenu RBAC 关联类型（仅列表，分配见 rbac.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TenantDtoBase } from '@/types/common';

/**
 * 角色-菜单关联列表（对应后端 TaktRoleMenuDto）
 */
export interface RoleMenu extends TenantDtoBase {
  /** 关联主键 */
  roleMenuId: string;
  /** 角色ID */
  roleId: string;
  /** 角色名称（填充） */
  roleName?: string;
  /** 菜单ID */
  menuId: string;
  /** 菜单名称（填充） */
  menuName?: string;
}
