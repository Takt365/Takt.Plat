// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：log-formatter.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端日志统一格式化（错误序列化、条目构建、控制台输出）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogContext, LogEntry, LogErrorInfo, LoggerConfig } from '@/types/logger';
import {
  buildFormatterRuntimeMeta,
  formatEntriesBatchForReport,
} from '@/utils/takt-formatter-common';
import { maskForLogging } from '@/utils/mask';

/**
 * 前端日志级别（数值越大优先级越高）
 */
export enum LogLevel {
  /** 调试信息（开发环境） */
  Debug = 0,
  /** 一般信息 */
  Info = 1,
  /** 警告（可恢复异常） */
  Warn = 2,
  /** 错误（业务或运行时失败） */
  Error = 3,
  /** 致命错误（需立即上报） */
  Fatal = 4,
}

/**
 * 日志级别名称映射
 */
const LOG_LEVEL_LABELS: Record<LogLevel, string> = {
  [LogLevel.Debug]: 'DEBUG',
  [LogLevel.Info]: 'INFO',
  [LogLevel.Warn]: 'WARN',
  [LogLevel.Error]: 'ERROR',
  [LogLevel.Fatal]: 'FATAL',
};

/**
 * 对日志上下文脱敏（password / token / phone 等敏感字段）
 * @param {LogContext} [context] 原始上下文
 * @returns {LogContext | undefined} 脱敏后的上下文
 */
export function sanitizeLogContext(context?: LogContext): LogContext | undefined {
  if (!context) {
    return undefined;
  }

  return maskForLogging(context) as LogContext;
}

/**
 * 对错误信息脱敏
 * @param {LogErrorInfo} [error] 原始错误信息
 * @returns {LogErrorInfo | undefined} 脱敏后的错误信息
 */
export function sanitizeLogErrorInfo(error?: LogErrorInfo): LogErrorInfo | undefined {
  if (!error) {
    return undefined;
  }

  return maskForLogging(error) as LogErrorInfo;
}

/**
 * 将 unknown 错误序列化为统一结构
 * @param {unknown} error 原始错误
 * @returns {LogErrorInfo | undefined} 序列化结果
 */
export function serializeLogError(error: unknown): LogErrorInfo | undefined {
  if (error === undefined || error === null) {
    return undefined;
  }

  if (error instanceof Error) {
    return sanitizeLogErrorInfo({
      name: error.name,
      message: error.message,
      stack: error.stack,
    });
  }

  if (typeof error === 'string') {
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: error,
    });
  }

  try {
    const maskedPayload = maskForLogging(error);
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: JSON.stringify(maskedPayload),
    });
  } catch {
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: String(error),
    });
  }
}

/**
 * 构建标准日志条目
 * @param {LogLevel} level 日志级别
 * @param {string} message 主消息
 * @param {LoggerConfig} config 日志器配置
 * @param {LogContext} [context] 业务上下文
 * @param {unknown} [error] 关联错误
 * @param {string[]} [tags] 标签
 * @returns {LogEntry} 标准日志条目
 */
export function buildLogEntry(
  level: LogLevel,
  message: string,
  config: LoggerConfig,
  context?: LogContext,
  error?: unknown,
  tags?: string[]
): LogEntry {
  const serializedError = serializeLogError(error);
  const runtimeMeta = buildFormatterRuntimeMeta(config);

  return {
    level,
    message,
    ...runtimeMeta,
    userAgent: typeof navigator !== 'undefined' ? navigator.userAgent : '',
    context: sanitizeLogContext(context),
    error: sanitizeLogErrorInfo(serializedError),
    tags,
  };
}

/**
 * 格式化日志条目为控制台可读字符串
 * @param {LogEntry} entry 日志条目
 * @returns {string} 格式化文本
 */
export function formatLogEntryForConsole(entry: LogEntry): string {
  const levelLabel = LOG_LEVEL_LABELS[entry.level] ?? 'LOG';
  const moduleLabel = entry.context?.module ? `[${entry.context.module}]` : '';
  const actionLabel = entry.context?.action ? `(${entry.context.action})` : '';
  const base = `[${levelLabel}]${moduleLabel}${actionLabel} ${entry.message}`;

  if (!entry.error) {
    return base;
  }

  return `${base} | ${entry.error.name}: ${entry.error.message}`;
}

/**
 * 将日志条目格式化为上报 JSON
 * @param {LogEntry[]} entries 日志条目列表
 * @returns {string} JSON 字符串
 */
export function formatLogEntriesForReport(entries: LogEntry[]): string {
  const sanitizedEntries = maskForLogging(entries) as LogEntry[];
  return formatEntriesBatchForReport(sanitizedEntries);
}

/**
 * 解析环境变量中的日志级别
 * @param {string | undefined} value 环境变量值
 * @param {LogLevel} defaultLevel 默认级别
 * @returns {LogLevel} 解析结果
 */
export function parseLogLevelFromEnv(value: string | undefined, defaultLevel: LogLevel): LogLevel {
  switch ((value ?? '').toLowerCase()) {
    case 'debug':
      return LogLevel.Debug;
    case 'info':
      return LogLevel.Info;
    case 'warn':
    case 'warning':
      return LogLevel.Warn;
    case 'error':
      return LogLevel.Error;
    case 'fatal':
      return LogLevel.Fatal;
    default:
      return defaultLevel;
  }
}

/**
 * 是否应输出指定级别日志
 * @param {LogLevel} level 当前级别
 * @param {LogLevel} minLevel 最低级别
 * @returns {boolean} 是否输出
 */
export function shouldLogLevel(level: LogLevel, minLevel: LogLevel): boolean {
  return level >= minLevel;
}

/**
 * 输出到浏览器控制台
 * @param {LogEntry} entry 日志条目
 */
export function writeLogEntryToConsole(entry: LogEntry): void {
  const text = formatLogEntryForConsole(entry);
  const detail =
    entry.context || entry.error
      ? (maskForLogging({ context: entry.context, error: entry.error }) as {
          context?: LogContext;
          error?: LogErrorInfo;
        })
      : undefined;

  switch (entry.level) {
    case LogLevel.Debug:
      console.debug(text, detail);
      break;
    case LogLevel.Info:
      console.info(text, detail);
      break;
    case LogLevel.Warn:
      console.warn(text, detail);
      break;
    case LogLevel.Error:
    case LogLevel.Fatal:
      console.error(text, detail);
      break;
    default:
      console.log(text, detail);
  }
}
