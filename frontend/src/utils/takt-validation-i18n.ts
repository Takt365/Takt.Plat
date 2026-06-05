// ========================================
// 项目名称：节节拍工厂·Takt Plat
// 命名空间：@/utils/takt-validation-i18n
// 文件名称：takt-validation-i18n.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：校验类抽象 I18n 键与组装辅助（common.validation.* + entity.* 字段名）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 格式不正确 */
export const VALIDATION_INVALID_FORMAT_I18N_KEY = 'common.validation.invalid.format';

/** 不能为空 */
export const VALIDATION_REQUIRED_I18N_KEY = 'common.validation.required';

/** 长度过短 */
export const VALIDATION_TOO_SHORT_I18N_KEY = 'common.validation.too.short';

/** 长度过长 */
export const VALIDATION_TOO_LONG_I18N_KEY = 'common.validation.too.long';

/** 强度不足 */
export const VALIDATION_TOO_WEAK_I18N_KEY = 'common.validation.too.weak';

type TaktTranslateFn = (key: string, params?: Record<string, unknown>) => string;

/**
 * 组装「{field}格式不正确」校验提示
 * @param t vue-i18n 翻译函数
 * @param fieldLabel 字段显示名（已翻译）
 * @returns 完整校验提示
 */
export function formatValidationInvalidFormat(t: TaktTranslateFn, fieldLabel: string): string {
  return t(VALIDATION_INVALID_FORMAT_I18N_KEY, { field: fieldLabel });
}

/**
 * 组装「{field}不能为空」校验提示
 * @param t vue-i18n 翻译函数
 * @param fieldLabel 字段显示名（已翻译）
 * @returns 完整校验提示
 */
export function formatValidationRequired(t: TaktTranslateFn, fieldLabel: string): string {
  return t(VALIDATION_REQUIRED_I18N_KEY, { field: fieldLabel });
}

/**
 * 组装「{field}长度过短」校验提示
 * @param t vue-i18n 翻译函数
 * @param fieldLabel 字段显示名（已翻译）
 * @param min 最少字符数
 * @returns 完整校验提示
 */
export function formatValidationTooShort(t: TaktTranslateFn, fieldLabel: string, min: number): string {
  return t(VALIDATION_TOO_SHORT_I18N_KEY, { field: fieldLabel, min });
}

/**
 * 组装「{field}长度过长」校验提示
 * @param t vue-i18n 翻译函数
 * @param fieldLabel 字段显示名（已翻译）
 * @param max 最多字符数
 * @returns 完整校验提示
 */
export function formatValidationTooLong(t: TaktTranslateFn, fieldLabel: string, max: number): string {
  return t(VALIDATION_TOO_LONG_I18N_KEY, { field: fieldLabel, max });
}

/**
 * 组装「{field}强度不足」校验提示
 * @param t vue-i18n 翻译函数
 * @param fieldLabel 字段显示名（已翻译）
 * @returns 完整校验提示
 */
export function formatValidationTooWeak(t: TaktTranslateFn, fieldLabel: string): string {
  return t(VALIDATION_TOO_WEAK_I18N_KEY, { field: fieldLabel });
}

/**
 * 组装「请输入{field}」占位提示（common.page.form.placeholder.required）
 * @param t vue-i18n 翻译函数
 * @param fieldKey 字段 I18n 键（如 entity.user.name）
 * @returns 完整占位提示
 */
export function formatFormPlaceholderRequired(t: TaktTranslateFn, fieldKey: string): string {
  return t('common.page.form.placeholder.required', { field: t(fieldKey) });
}
