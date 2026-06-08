// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/code/database
// 文件名称：database-info.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  DatabaseInfo,
  DatabaseTableColumnInfo,
  DatabaseTableInfo
} from '@/types/code/database/database-info';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDatabaseInfos
 */
const DATABASE_INFO_API_BASE = 'TaktDatabaseInfos';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取可 introspect 的租户业务库列表
 * @returns {Promise<DatabaseInfo[]>} 数据库摘要列表
 */
export function getDatabaseInfoList(): Promise<DatabaseInfo[]> {
  return request<DatabaseInfo[]>({
    url: `${DATABASE_INFO_API_BASE}/list`,
    method: 'get',
  });
}

/**
 * 获取指定租户库下所有用户表摘要
 * @param {string} tenantCode 租户编码（3 位）
 * @returns {Promise<DatabaseTableInfo[]>} 表摘要列表
 */
export function getDatabaseTableInfoList(tenantCode: string): Promise<DatabaseTableInfo[]> {
  return request<DatabaseTableInfo[]>({
    url: `${DATABASE_INFO_API_BASE}/tables`,
    method: 'get',
    params: {
      tenantCode
    },
  });
}

/**
 * 获取指定物理表的列摘要
 * @param {string} tenantCode 租户编码（3 位）
 * @param {string} tableName 表名
 * @returns {Promise<DatabaseTableColumnInfo[]>} 列摘要列表
 */
export function getDatabaseTableColumnInfoList(tenantCode: string, tableName: string): Promise<DatabaseTableColumnInfo[]> {
  return request<DatabaseTableColumnInfo[]>({
    url: `${DATABASE_INFO_API_BASE}/columns`,
    method: 'get',
    params: {
      tenantCode,
      tableName
    },
  });
}
