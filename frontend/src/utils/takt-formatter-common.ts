// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-formatter-common.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：日志/事件格式化公共序列化与 batch 上报
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 日志/事件条目共用的运行时元数据 */
export interface TaktFormatterRuntimeMeta {
  timestamp: string;
  appName: string;
  appVersion: string;
  environment: string;
  url: string;
}

/**
 * 构建日志/事件条目共用的运行时元数据
 * @param config 应用标识配置
 * @returns 运行时元数据
 */
export function buildFormatterRuntimeMeta(config: {
  appName: string;
  appVersion: string;
  environment: string;
}): TaktFormatterRuntimeMeta {
  return {
    timestamp: new Date().toISOString(),
    appName: config.appName,
    appVersion: config.appVersion,
    environment: config.environment,
    url: typeof window !== 'undefined' ? window.location.href : '',
  };
}

/**
 * JSON 安全序列化（避免循环引用）
 * @param payload 原始载荷
 * @returns 可序列化载荷
 */
export function serializeJsonSafe(payload: unknown): unknown {
  if (payload === undefined) {
    return undefined;
  }
  try {
    return JSON.parse(JSON.stringify(payload));
  } catch {
    return String(payload);
  }
}

/**
 * 将条目列表格式化为上报 JSON（含 batchId、reportedAt）
 * @param entries 条目列表
 * @returns JSON 字符串
 */
export function formatEntriesBatchForReport<T>(entries: T[]): string {
  return JSON.stringify({
    batchId: crypto.randomUUID(),
    reportedAt: new Date().toISOString(),
    entries,
  });
}
