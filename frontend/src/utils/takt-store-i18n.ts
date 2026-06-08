// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-store-i18n.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：Store / bootstrap 层 I18n 键常量与组装（common.* 动态种子 + locales/common 静态兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { translateLocaleMessage } from '@/utils/takt-i18n-message';

/** 语言列表（common.page.entity.culturelist） */
export const STORE_I18N_ENTITY_CULTURE_LIST = 'common.page.entity.culturelist';

/** 菜单（common.page.entity.menulist） */
export const STORE_I18N_ENTITY_MENU_LIST = 'common.page.entity.menulist';

/** 租户列表（common.page.entity.tenantlist） */
export const STORE_I18N_ENTITY_TENANT_LIST = 'common.page.entity.tenantlist';

/** 未获取到可用资源（common.feedback.load.empty） */
export const STORE_I18N_FEEDBACK_LOAD_EMPTY = 'common.feedback.load.empty';

/** 加载失败（common.feedback.load.failed） */
export const STORE_I18N_FEEDBACK_LOAD_FAILED = 'common.feedback.load.failed';

/** 会话过期（common.tip.session.expired） */
export const STORE_I18N_TIP_SESSION_EXPIRED = 'common.tip.session.expired';

/** 空闲自动登出（common.tip.session.idle.logout） */
export const STORE_I18N_TIP_SESSION_IDLE_LOGOUT = 'common.tip.session.idle.logout';

/** 强制下线（common.tip.force.logout） */
export const STORE_I18N_TIP_FORCE_LOGOUT = 'common.tip.force.logout';

/** 连接成功（common.feedback.connect.success） */
export const STORE_I18N_FEEDBACK_CONNECT_SUCCESS = 'common.feedback.connect.success';

/** SignalR 错误（common.feedback.signalr.error） */
export const STORE_I18N_FEEDBACK_SIGNALR_ERROR = 'common.feedback.signalr.error';

/** 路由模块加载失败（layouts.page.route.loadfail，前端静态） */
export const STORE_I18N_LAYOUT_ROUTE_LOAD_FAIL = 'layouts.page.route.loadfail';

/** 会话即将过期标题（layouts.page.session.title） */
export const STORE_I18N_LAYOUT_SESSION_TITLE = 'layouts.page.session.title';

/** 会话即将过期正文（layouts.page.session.content，含 {minutes}） */
export const STORE_I18N_LAYOUT_SESSION_CONTENT = 'layouts.page.session.content';

/** 继续使用（layouts.page.session.oktext） */
export const STORE_I18N_LAYOUT_SESSION_OK = 'layouts.page.session.oktext';

/** 立即登出（layouts.page.session.canceltext） */
export const STORE_I18N_LAYOUT_SESSION_CANCEL = 'layouts.page.session.canceltext';

/**
 * 组装「未获取到可用的{target}」
 * @param targetKey 目标名称 i18n 键（如 common.page.entity.culturelist）
 * @returns 本地化文案
 */
export function translateLoadEmpty(targetKey: string): string {
  return translateLocaleMessage(STORE_I18N_FEEDBACK_LOAD_EMPTY, {
    target: translateLocaleMessage(targetKey),
  });
}

/**
 * 组装「加载{target}失败」
 * @param targetKey 目标名称 i18n 键
 * @returns 本地化文案
 */
export function translateLoadFailed(targetKey: string): string {
  return translateLocaleMessage(STORE_I18N_FEEDBACK_LOAD_FAILED, {
    target: translateLocaleMessage(targetKey),
  });
}
