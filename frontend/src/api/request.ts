// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api
// 文件名称：request.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Axios 统一请求实例；自动携带鉴权与租户头；解析后端 TaktApiResult 并返回 data 载荷
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import axios from 'axios';
import type {
  AxiosError,
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from 'axios';
import { useUserStore } from '@/stores/identity/user';
import { useTenantStore } from '@/stores/identity/tenant';
import { resolveRequestLocale } from '@/stores/foundation/locale';
import type { TaktApiResult } from '@/types/common';
import { TaktResultCode } from '@/utils/common';
import { createLogger } from '@/utils/logger';
import { EventBus, emitNotification } from '@/utils/event-bus';
import { ensureValidAccessToken, refreshOAuthTokens } from '@/utils/oauth';

const requestLogger = createLogger('request');

/** Axios 配置扩展：无感刷新重试标记 */
declare module 'axios' {
  export interface InternalAxiosRequestConfig {
    /** 已为 401 / 业务未授权尝试过 refresh 并重试 */
    _retryAuth?: boolean;
    /** 跳过请求前主动刷新（登录等接口） */
    skipTokenRefresh?: boolean;
  }
}

/**
 * API 业务错误
 * @description HTTP 失败或响应体 code !== TaktResultCode.Success 时由拦截器抛出，供业务层 catch 区分处理
 */
export class TaktApiError extends Error {
  /**
   * 后端结果代码（对应 Takt.Shared.Enums.TaktResultCode）
   */
  code: TaktResultCode;

  /**
   * 后端 TaktApiResult.data 字段（失败时可能为 null）
   */
  data: unknown;

  /**
   * @param {string} message 错误提示（通常与后端 message 一致）
   * @param {TaktResultCode} code 结果代码
   * @param {unknown} [data=null] 业务 data 载荷
   */
  constructor(message: string, code: TaktResultCode, data: unknown = null) {
    super(message);
    this.name = 'TaktApiError';
    this.code = code;
    this.data = data;
  }
}

/**
 * 判断响应体是否为后端统一包装 TaktApiResult
 * @param {unknown} data 原始响应体
 * @returns {boolean} 是否为 TaktApiResult 结构
 */
function isTaktApiResult(data: unknown): data is TaktApiResult<unknown> {
  if (!data || typeof data !== 'object') {
    return false;
  }
  const body = data as Record<string, unknown>;
  return typeof body.code === 'number' && typeof body.message === 'string' && 'data' in body;
}

/**
 * 是否为二进制下载响应（跳过 JSON 解包，直接返回 Blob / ArrayBuffer）
 * @param {AxiosResponse} response Axios 原始响应
 * @returns {boolean} 是否按二进制处理
 */
function isBinaryResponse(response: AxiosResponse): boolean {
  const responseType = response.config.responseType;
  if (responseType === 'blob' || responseType === 'arraybuffer') {
    return true;
  }
  return response.data instanceof Blob || response.data instanceof ArrayBuffer;
}

/**
 * 解包成功响应：返回 TaktApiResult.data；业务失败时提示并抛出 TaktApiError
 * @template T 业务数据类型
 * @param {AxiosResponse} response Axios 原始响应
 * @returns {T} 解包后的业务 data
 * @throws {TaktApiError} code !== TaktResultCode.Success
 */
function unwrapTaktApiResult<T>(response: AxiosResponse): T {
  if (isBinaryResponse(response)) {
    return response.data as T;
  }

  const body = response.data;

  if (!isTaktApiResult(body)) {
    return body as T;
  }

  if (body.code !== TaktResultCode.Success) {
    const errMsg = body.message || '请求失败';
    if (body.code !== TaktResultCode.Unauthorized) {
      emitNotification('error', errMsg);
    }
    throw new TaktApiError(errMsg, body.code, body.data);
  }

  return body.data as T;
}

/**
 * 将 unknown 异常收窄为 AxiosError
 * @param {unknown} error 捕获的异常
 * @returns {AxiosError | null} AxiosError 或 null
 */
function toAxiosError(error: unknown): AxiosError | null {
  if (!axios.isAxiosError(error)) {
    return null;
  }
  return error as AxiosError;
}

/**
 * 未授权：仅广播事件，由 bootstrap 订阅方清状态并跳转
 */
function emitSessionExpired(message?: string): void {
  EventBus.emit('auth:session-expired', { message });
}

/**
 * 401 / 业务未授权：尝试 refresh_token 后重试原请求
 * @param {InternalAxiosRequestConfig} config 原请求配置
 * @param {string} [message] 失败时提示文案
 * @returns {Promise<AxiosResponse>} 重试响应或 reject
 */
async function retryRequestAfterTokenRefresh(
  config: InternalAxiosRequestConfig,
  message?: string
): Promise<AxiosResponse> {
  if (config._retryAuth) {
    emitSessionExpired(message || '登录已过期，请重新登录');
    return Promise.reject(new TaktApiError(message || '登录已过期', TaktResultCode.Unauthorized));
  }

  const refreshed = await refreshOAuthTokens();
  if (!refreshed) {
    emitSessionExpired(message || '登录已过期，请重新登录');
    return Promise.reject(new TaktApiError(message || '登录已过期', TaktResultCode.Unauthorized));
  }

  const userStore = useUserStore();
  config._retryAuth = true;
  config.headers.Authorization = `Bearer ${userStore.token}`;
  return axiosInstance.request(config);
}

/**
 * 统一处理 Axios 错误响应
 * @param {unknown} error 拦截器捕获的异常
 * @returns {Promise<unknown>} reject 或刷新成功后重试的响应
 */
function rejectFromAxiosError(error: unknown): Promise<unknown> {
  const axiosError = toAxiosError(error);
  if (!axiosError) {
    if (error instanceof Error) {
      requestLogger.warn('请求配置错误', { action: 'config' }, error);
      emitNotification('error', error.message || '请求配置错误');
    }
    return Promise.reject(error);
  }

  if (!axiosError.response) {
    if (axiosError.request) {
      requestLogger.warn('网络连接失败', { action: 'network' }, axiosError);
      emitNotification('error', '网络连接失败');
    }
    return Promise.reject(axiosError);
  }

  const { status, data, config: axiosConfig } = axiosError.response;

  if (isTaktApiResult(data)) {
    if (data.code === TaktResultCode.Unauthorized && axiosError.config) {
      return retryRequestAfterTokenRefresh(axiosError.config, data.message || '登录已过期，请重新登录');
    }
    if (data.code !== TaktResultCode.Unauthorized && !axiosConfig?.skipErrorNotification) {
      emitNotification('error', data.message || '请求失败');
    }
    return Promise.reject(new TaktApiError(data.message, data.code, data.data));
  }

  switch (status) {
    case 401:
      if (axiosError.config) {
        return retryRequestAfterTokenRefresh(axiosError.config, '登录已过期，请重新登录');
      }
      emitSessionExpired('登录已过期，请重新登录');
      break;
    case 403:
      emitNotification('error', '无权限访问');
      break;
    case 404:
      emitNotification('error', '请求的资源不存在');
      break;
    case 500:
      requestLogger.error('服务器错误', { action: 'http', status, url: axiosConfig?.url }, axiosError);
      emitNotification('error', '服务器错误');
      break;
    default:
      requestLogger.warn(
        '请求失败',
        { action: 'http', status, url: axiosConfig?.url },
        axiosError
      );
      emitNotification(
        'error',
        typeof data === 'object' && data !== null && 'message' in data
          ? String((data as { message?: string }).message)
          : '请求失败'
      );
  }

  return Promise.reject(axiosError);
}

/**
 * 业务 HTTP 客户端
 * @description 所有方法返回值均为拦截器解包后的业务 data，而非完整 TaktApiResult 包装
 */
export interface TaktHttpClient {
  /**
   * 配置式请求（推荐，与业务 API 生成模板一致）
   * @template T 业务 data 类型
   * @param {AxiosRequestConfig} config Axios 请求配置
   * @returns {Promise<T>} 解包后的业务 data
   */
  <T = unknown>(config: AxiosRequestConfig): Promise<T>;

  /**
   * GET 请求
   * @template T 业务 data 类型
   * @param {string} url 相对路径（相对 baseURL）
   * @param {AxiosRequestConfig} [config] 附加配置
   * @returns {Promise<T>} 解包后的业务 data
   */
  get<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T>;

  /**
   * POST 请求
   * @template T 业务 data 类型
   */
  post<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;

  /**
   * PUT 请求
   * @template T 业务 data 类型
   */
  put<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;

  /**
   * DELETE 请求
   * @template T 业务 data 类型
   */
  delete<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T>;

  /**
   * PATCH 请求
   * @template T 业务 data 类型
   */
  patch<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;
}

/**
 * Axios 底层实例（挂载请求/响应拦截器）
 */
const axiosInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  timeout: 30000,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(
  async (config: InternalAxiosRequestConfig) => {
    const userStore = useUserStore();
    const tenantStore = useTenantStore();

    if (!config.skipTokenRefresh && userStore.token && userStore.refreshToken) {
      await ensureValidAccessToken();
    }

    if (userStore.token) {
      config.headers.Authorization = `Bearer ${userStore.token}`;
    }
    if (tenantStore.tenantCode) {
      config.headers['X-Tenant-Code'] = tenantStore.tenantCode;
    }
    if (tenantStore.companyCode) {
      config.headers['X-Company-Code'] = tenantStore.companyCode;
    }

    config.headers['Accept-Language'] = resolveRequestLocale();

    if (config.data instanceof FormData) {
      delete config.headers['Content-Type'];
    }

    return config;
  },
  (error: unknown) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    try {
      return unwrapTaktApiResult(response);
    } catch (err: unknown) {
      if (
        err instanceof TaktApiError &&
        err.code === TaktResultCode.Unauthorized &&
        response.config
      ) {
        return retryRequestAfterTokenRefresh(response.config, err.message).then((retried) => {
          try {
            return unwrapTaktApiResult(retried);
          } catch (retryErr: unknown) {
            return Promise.reject(retryErr);
          }
        });
      }
      return Promise.reject(err);
    }
  },
  (error: unknown) => rejectFromAxiosError(error)
);

/**
 * 配置式请求入口
 * @template T 业务 data 类型
 */
function taktRequest<T = unknown>(config: AxiosRequestConfig): Promise<T> {
  return axiosInstance.request(config) as Promise<T>;
}

const request = Object.assign(taktRequest, {
  get: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    axiosInstance.get(url, config) as Promise<T>,
  post: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.post(url, data, config) as Promise<T>,
  put: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.put(url, data, config) as Promise<T>,
  delete: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    axiosInstance.delete(url, config) as Promise<T>,
  patch: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.patch(url, data, config) as Promise<T>,
}) as TaktHttpClient;

export default request;
