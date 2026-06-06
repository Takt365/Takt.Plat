// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/table-columns
// 文件名称：table-columns.ts
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：表格列工具函数，按实体基类（租户/公司/审批）生成默认字段列
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue';
import type { ColumnGroupType, ColumnType } from 'ant-design-vue/es/table';

/** 表格行占位（默认实体列排序/过滤用） */
type RowRecord = Record<string, unknown>;

type ColumnItem = ColumnType<RowRecord> | ColumnGroupType<RowRecord>;

/**
 * 实体基类作用域（与 common.d.ts 中 TaktTenantEntityBase / TaktCompanyEntityBase / TaktApprovalEntityBase 对齐）
 */
export type TaktEntityScope = 'tenant' | 'company' | 'approval';

/** 各作用域专属字段（不含 extFieldJson / remark / 审计字段） */
const TENANT_SCOPE_FIELD_KEYS = ['tenantCode'] as const;
const COMPANY_EXTRA_FIELD_KEYS = ['companyCode'] as const;
const APPROVAL_EXTRA_FIELD_KEYS = [
  'approvalStatus',
  'initiatorId',
  'initiatedAt',
  'approvalOpinion',
  'approvedBy',
  'approvedAt',
] as const;

const COMMON_FIELD_KEYS = ['extFieldJson', 'remark'] as const;

/**
 * 审计字段（includeAuditFields 为 false 时过滤）
 * 与 TaktTenantEntityBase / TaktCompanyEntityBase / TaktApprovalEntityBase 的 camelCase 字段一致
 */
const AUDIT_FIELD_KEYS = [
  'createdBy',
  'createdAt',
  'updatedBy',
  'updatedAt',
  'isDeleted',
  'deletedBy',
  'deletedAt',
] as const;

type AuditFieldKey = (typeof AUDIT_FIELD_KEYS)[number];

/**
 * 获取列 key（key 或 dataIndex）
 * @param col 列配置
 */
export function getTableColumnKey(col: ColumnItem | Record<string, unknown>): string | undefined {
  const c = col as { key?: string | number; dataIndex?: string | number };
  const k = c.key ?? c.dataIndex;
  return k != null && k !== '' ? String(k) : undefined;
}

/**
 * 解析实体基座字段 i18n 键（common.page.entity.*，camelCase 转全小写，对齐 TaktCompanyEntityBase 种子）
 * @param field 实体基座字段名（如 tenantCode、companyCode）
 * @returns 动态翻译键（如 common.page.entity.tenantcode）
 */
export function resolveEntityBaseFieldI18nKey(field: string): string {
  return `common.page.entity.${field.toLowerCase()}`;
}

/**
 * 构建文本列
 * @param field 字段名
 * @param t 翻译函数
 * @param width 列宽
 */
function buildTextColumn(field: string, t: (key: string) => string, width = 120): ColumnType<RowRecord> {
  return {
    key: field,
    dataIndex: field,
    title: t(resolveEntityBaseFieldI18nKey(field)),
    width,
    ellipsis: true,
  };
}

/**
 * 构建日期时间列（带排序）
 * @param field 字段名
 * @param t 翻译函数
 * @param width 列宽
 */
function buildDateTimeColumn(field: string, t: (key: string) => string, width = 180): ColumnType<RowRecord> {
  return {
    key: field,
    dataIndex: field,
    title: t(resolveEntityBaseFieldI18nKey(field)),
    width,
    ellipsis: true,
    sorter: (a: RowRecord, b: RowRecord) => {
      const aTime = a[field] ? new Date(String(a[field])).getTime() : 0;
      const bTime = b[field] ? new Date(String(b[field])).getTime() : 0;
      return aTime - bTime;
    },
  };
}

/**
 * 构建数值列（带排序）
 * @param field 字段名
 * @param t 翻译函数
 * @param width 列宽
 */
function buildNumberColumn(field: string, t: (key: string) => string, width = 100): ColumnType<RowRecord> {
  return {
    key: field,
    dataIndex: field,
    title: t(resolveEntityBaseFieldI18nKey(field)),
    width,
    ellipsis: true,
    sorter: (a: RowRecord, b: RowRecord) => Number(a[field] ?? 0) - Number(b[field] ?? 0),
  };
}

/**
 * 按字段名构建单列
 * @param field 字段名
 * @param t 翻译函数
 */
function buildEntityFieldColumn(field: string, t: (key: string) => string): ColumnType<RowRecord> {
  if (field === 'createdAt' || field === 'updatedAt' || field === 'deletedAt' || field === 'initiatedAt' || field === 'approvedAt') {
    return buildDateTimeColumn(field, t);
  }
  if (field === 'isDeleted' || field === 'approvalStatus') {
    return buildNumberColumn(field, t);
  }
  if (field === 'extFieldJson' || field === 'approvalOpinion' || field === 'remark') {
    return buildTextColumn(field, t, 150);
  }
  if (field === 'tenantCode') {
    return buildTextColumn(field, t, 100);
  }
  if (field === 'companyCode') {
    return buildTextColumn(field, t, 100);
  }
  return buildTextColumn(field, t);
}

/**
 * 解析作用域下的默认字段顺序
 * @param entityScope 实体基类作用域
 */
function resolveScopeFieldKeys(entityScope: TaktEntityScope): readonly string[] {
  if (entityScope === 'tenant') {
    return [...TENANT_SCOPE_FIELD_KEYS, ...COMMON_FIELD_KEYS, ...AUDIT_FIELD_KEYS];
  }
  if (entityScope === 'company') {
    return [...TENANT_SCOPE_FIELD_KEYS, ...COMPANY_EXTRA_FIELD_KEYS, ...COMMON_FIELD_KEYS, ...AUDIT_FIELD_KEYS];
  }
  return [
    ...TENANT_SCOPE_FIELD_KEYS,
    ...COMPANY_EXTRA_FIELD_KEYS,
    ...APPROVAL_EXTRA_FIELD_KEYS,
    ...COMMON_FIELD_KEYS,
    ...AUDIT_FIELD_KEYS,
  ];
}

/**
 * 过滤审计字段列
 * @param columns 列配置
 * @param includeAuditFields 是否包含审计字段
 */
function filterAuditColumns(columns: TableColumnsType, includeAuditFields: boolean): TableColumnsType {
  if (includeAuditFields) {
    return columns;
  }
  return columns.filter((col) => {
    const key = getTableColumnKey(col as ColumnItem);
    return key != null && !AUDIT_FIELD_KEYS.includes(key as AuditFieldKey);
  });
}

/**
 * 从合并结果中移除审计字段（含分组列子列）
 * @param columns 列配置
 */
function stripAuditColumnsFromMerged(columns: TableColumnsType): TableColumnsType {
  return columns
    .map((col) => {
      const item = col as ColumnItem;
      const key = getTableColumnKey(item);
      if ('children' in item && item.children) {
        const filteredChildren = item.children.filter((childCol: ColumnItem) => {
          const childKey = getTableColumnKey(childCol);
          return childKey != null && !AUDIT_FIELD_KEYS.includes(childKey as AuditFieldKey);
        });
        if (filteredChildren.length === 0) {
          return null;
        }
        return { ...item, children: filteredChildren };
      }
      if (key != null && AUDIT_FIELD_KEYS.includes(key as AuditFieldKey)) {
        return null;
      }
      return col;
    })
    .filter((col): col is ColumnItem => col !== null) as TableColumnsType;
}

/**
 * 获取默认实体基座字段列（按 TaktTenantEntityBase / TaktCompanyEntityBase / TaktApprovalEntityBase 区分）
 * @param t 翻译函数
 * @param includeAuditFields 是否包含审计字段（默认 true）
 * @param entityScope 实体基类作用域（默认 company）
 */
export function getDefaultEntityColumns(
  t: (key: string) => string,
  includeAuditFields: boolean = true,
  entityScope: TaktEntityScope = 'company',
): TableColumnsType {
  const fieldKeys = resolveScopeFieldKeys(entityScope);
  const columns = fieldKeys.map((field) => buildEntityFieldColumn(field, t));
  return filterAuditColumns(columns, includeAuditFields);
}

/**
 * 合并默认实体基座字段到用户定义的列中（同 key/dataIndex 以用户列为准）
 * @param userColumns 用户定义的列
 * @param t 翻译函数
 * @param includeAuditFields 是否包含审计字段（默认 true）
 * @param entityScope 实体基类作用域（默认 company）
 */
export function mergeDefaultColumns(
  userColumns: TableColumnsType,
  t: (key: string) => string,
  includeAuditFields: boolean = true,
  entityScope: TaktEntityScope = 'company',
): TableColumnsType {
  const defaultColumns = getDefaultEntityColumns(t, includeAuditFields, entityScope);
  const userColumnKeys = new Set<string>();
  userColumns.forEach((col) => {
    if ('children' in col && col.children) {
      col.children.forEach((childCol: ColumnType<RowRecord>) => {
        const key = getTableColumnKey(childCol);
        if (key) {
          userColumnKeys.add(key);
        }
      });
    } else {
      const key = getTableColumnKey(col as ColumnItem);
      if (key) {
        userColumnKeys.add(key);
      }
    }
  });
  const missingDefaultColumns = defaultColumns.filter((col) => {
    const key = getTableColumnKey(col as ColumnItem);
    return key != null && !userColumnKeys.has(key);
  });
  const actionColumns: TableColumnsType = [];
  const otherUserColumns: TableColumnsType = [];
  userColumns.forEach((col) => {
    const item = col as ColumnItem;
    const key = getTableColumnKey(item);
    if (key === 'action' || item.fixed === 'right') {
      actionColumns.push(col);
    } else {
      otherUserColumns.push(col);
    }
  });
  const mergedColumns = [...otherUserColumns, ...missingDefaultColumns, ...actionColumns];
  if (!includeAuditFields) {
    return stripAuditColumnsFromMerged(mergedColumns);
  }
  return mergedColumns;
}

/**
 * 按可见列键过滤表格列（保持 mergedColumns 原始顺序）
 * @param mergedColumns 已合并实体基座的列
 * @param visibleKeys 可见列键；空数组时返回 fallbackColumns
 * @param fallbackColumns 列设置未初始化时的回退列（通常为业务列）
 */
export function filterTableColumnsByVisibleKeys(
  mergedColumns: TableColumnsType,
  visibleKeys: string[],
  fallbackColumns?: TableColumnsType,
): TableColumnsType {
  if (!visibleKeys.length) {
    return fallbackColumns ?? mergedColumns;
  }
  const keysSet = new Set(visibleKeys.map((k) => String(k)));
  return mergedColumns.filter((col) => {
    const colKey = getTableColumnKey(col as ColumnItem);
    return colKey != null && keysSet.has(colKey);
  });
}
