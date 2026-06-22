// ========================================
// 项目名称：节拍工厂·Takt Plat (TDF)
// 命名空间：frontend/src/locales
// 文件名称：index.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：国际化配置（vue-i18n）
//           · 本地静态：import.meta.glob 按目录路径嵌套（如 locales/dashboard/workspace → dashboard.workspace.page.*）
//           · 语言文件内顶级节点必须为 page（引用键：业务目录.page.*，勿重复 common/login 等目录名）
//           · 后端动态：登录后或登录页租户校验通过后由 useTranslationStore 合并
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createI18n } from 'vue-i18n';
import { TAKT_SUPPORTED_LOCALES } from '@/utils/common';
import { readStoredLocale } from '@/utils/takt-locale-sync';
import { buildNestedLocaleMessages, resolveLocaleMessageFromTree } from '@/utils/takt-i18n-message';
import {
  deepMergeLocaleMessages,
  nestLocalePayloadUnderNamespace,
  resolveLocaleNamespaceSegments,
  type TaktLocaleMessageTree,
} from '@/utils/takt-locale-merge';

/** 单语言包消息结构 */
type TaktLocaleMessages = TaktLocaleMessageTree;

/** 语言文件后缀匹配（如 common/zh-CN.ts、dashboard/workspace/en-US.ts） */
const LOCALE_FILE_PATTERN = /\/(zh-CN|zh-HK|en-US|ja-JP)\.ts$/;

/** 构建期收集 locales 目录下全部静态翻译模块（排除本文件） */
const localeModules = import.meta.glob<{ default: TaktLocaleMessages }>('./**/*.ts', {
  eager: true,
});

/**
 * 由 glob 结果按语言编码合并本地静态消息（路径排序保证合并顺序稳定）
 * @returns vue-i18n 静态 messages 表
 */
export function buildStaticLocaleMessages(): Record<string, TaktLocaleMessages> {
  const messages = Object.fromEntries(
    TAKT_SUPPORTED_LOCALES.map((locale) => [locale, {} as TaktLocaleMessages])
  ) as Record<string, TaktLocaleMessages>;

  Object.keys(localeModules)
    .sort()
    .forEach((filePath) => {
      if (filePath === './index.ts') {
        return;
      }

      const localeMatch = filePath.match(LOCALE_FILE_PATTERN);
      if (!localeMatch) {
        return;
      }

      const locale = localeMatch[1];
      if (!(locale in messages)) {
        return;
      }

      const payload = localeModules[filePath]?.default;
      if (payload && typeof payload === 'object') {
        const segments = resolveLocaleNamespaceSegments(filePath, LOCALE_FILE_PATTERN);
        const nested = nestLocalePayloadUnderNamespace(segments, payload);
        messages[locale] = deepMergeLocaleMessages(messages[locale], nested);
      }
    });

  return messages;
}

/**
 * 将后端扁平翻译键合并到 vue-i18n（供 useTranslationStore 调用；同名键覆盖静态文案）
 * @param cultureCode 区域文化编码（zh-CN / zh-HK / en-US / ja-JP）
 * @param flatMessages 扁平键值（如 entity.user.name）
 */
export function mergeDynamicLocaleMessages(
  cultureCode: string,
  flatMessages: Record<string, string>
): void {
  if (Object.keys(flatMessages).length === 0) {
    return;
  }

  const nestedMessages = buildNestedLocaleMessages(flatMessages);
  i18n.global.mergeLocaleMessage(cultureCode, nestedMessages);
}

const staticMessages = buildStaticLocaleMessages();

/**
 * 创建 i18n 实例（仅含本地静态包；动态包在登录后由 TranslationStore 增量合并）
 */
const i18n = createI18n({
  legacy: false,
  locale: readStoredLocale(),
  /** 禁用语言回退：当前 locale 无翻译时由 missing 返回资源键 */
  fallbackLocale: false,
  fallbackWarn: false,
  missingWarn: false,
  missing: (locale, key) => {
    const messages = (i18n.global as { messages: { value: Record<string, TaktLocaleMessageTree> } }).messages.value;
    const tree = messages[locale];
    if (tree) {
      const text = resolveLocaleMessageFromTree(tree, key);
      if (text !== undefined) {
        return text;
      }
    }
    return key;
  },
  // glob 合并结果为运行时对象，与 vue-i18n 泛型 LocaleMessage 对齐
  messages: staticMessages as never,
});

export default i18n;
