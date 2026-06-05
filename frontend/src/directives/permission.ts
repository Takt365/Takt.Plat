// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/directives
// 文件名称：permission.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：v-permission 指令（按钮级功能权限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { App, Directive, DirectiveBinding } from 'vue';
import { watch, type WatchStopHandle } from 'vue';
import { usePermissionStore } from '@/stores/identity/permission';
import type { TaktPermissionBindingValue } from '@/types/common';
import { hasAnyPermission } from '@/utils/permission';

/** 指令卸载时停止 store 监听的扩展属性 */
type PermissionElement = HTMLElement & {
  _permissionStop?: WatchStopHandle;
};

/**
 * 更新元素可见性
 * @param {HTMLElement} el 绑定元素
 * @param {DirectiveBinding<TaktPermissionBindingValue>} binding 指令绑定
 */
function updatePermissionVisibility(
  el: HTMLElement,
  binding: DirectiveBinding<TaktPermissionBindingValue>
): void {
  const permissionStore = usePermissionStore();
  const allowed = hasAnyPermission(permissionStore.permissions, binding.value);
  el.style.display = allowed ? '' : 'none';
}

/**
 * 功能权限指令
 */
const permissionDirective: Directive<PermissionElement, TaktPermissionBindingValue> = {
  mounted(el, binding) {
    updatePermissionVisibility(el, binding);
    const permissionStore = usePermissionStore();
    el._permissionStop = watch(
      () => permissionStore.permissions,
      () => updatePermissionVisibility(el, binding),
      { deep: true }
    );
  },
  updated(el, binding) {
    updatePermissionVisibility(el, binding);
  },
  unmounted(el) {
    el._permissionStop?.();
    delete el._permissionStop;
  },
};

/**
 * 注册 v-permission 指令
 * @param {App} app Vue 应用实例
 */
export function registerPermissionDirective(app: App): void {
  app.directive('permission', permissionDirective);
}
