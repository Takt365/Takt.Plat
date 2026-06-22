// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/table-columns
// 文件名称：table-columns.ts
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：表格列工具；三个实体基类字段直接映射（对齐 common.d.ts，不含 id）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue';
import type { ColumnGroupType, ColumnType } from 'ant-design-vue/es/table';

type RowRecord = Record<string, unknown>;
type ColumnItem = ColumnType<RowRecord> | ColumnGroupType<RowRecord>;

/** 实体基类作用域 ↔ common.d.ts 三个 EntityBase */
export type TaktEntityScope = 'tenant' | 'company' | 'approval';

/** 单表 / 左树右表默认可见业务字段数（不含 id、action、基类字段） */
export type TaktTableLayoutMode = 'single' | 'tree';

export const DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT: Record<TaktTableLayoutMode, number> = {
  single: 8,
  tree: 4,
};

/**
 * 三个实体基类字段（不含 id；顺序与 common.d.ts 一致）
 * tenant   → TaktTenantEntityBase
 * company  → TaktCompanyEntityBase（在 tenant 基础上 + companyCode）
 * approval → TaktApprovalEntityBase（在 company 基础上 + 审批字段）
 */
export const ENTITY_BASE_FIELDS = {
  tenant: [
    'tenantCode',
    'ExtField',
    'remark',
    'createdBy',
    'createdAt',
    'updatedBy',
    'updatedAt',
    'isDeleted',
    'deletedBy',
    'deletedAt',
  ],
  company: [
    'tenantCode',
    'companyCode',
    'ExtField',
    'remark',
    'createdBy',
    'createdAt',
    'updatedBy',
    'updatedAt',
    'isDeleted',
    'deletedBy',
    'deletedAt',
  ],
  approval: [
    'tenantCode',
    'companyCode',
    'ExtField',
    'remark',
    'approvalStatus',
    'initiatorId',
    'initiatedAt',
    'approvalOpinion',
    'approvedBy',
    'approvedAt',
    'flowInstanceId',
    'createdBy',
    'createdAt',
    'updatedBy',
    'updatedAt',
    'isDeleted',
    'deletedBy',
    'deletedAt',
  ],
} as const satisfies Record<TaktEntityScope, readonly string[]>;

const AUDIT_FIELD_SET = new Set<string>([
  'createdBy',
  'createdAt',
  'updatedBy',
  'updatedAt',
  'isDeleted',
  'deletedBy',
  'deletedAt',
]);

const DATETIME_FIELDS = new Set(['createdAt', 'updatedAt', 'deletedAt', 'initiatedAt', 'approvedAt']);
const NUMBER_FIELDS = new Set(['isDeleted', 'approvalStatus']);

/**
 * 归一化页面 columns（解包 Ref/ComputedRef）
 * @param input 业务列
 */
export function normalizeUserTableColumns(input: unknown): TableColumnsType {
  if (Array.isArray(input)) {
    return input as TableColumnsType;
  }
  if (input != null && typeof input === 'object' && 'value' in input) {
    const inner = (input as { value: unknown }).value;
    if (Array.isArray(inner)) {
      return inner as TableColumnsType;
    }
  }
  return [];
}

/**
 * 取 entityScope 对应基类字段键
 * @param entityScope tenant | company | approval
 */
export function resolveEntityScopeBaseFieldKeys(entityScope: TaktEntityScope): readonly string[] {
  return ENTITY_BASE_FIELDS[entityScope];
}

/**
 * 获取列 key
 * @param col 列配置
 */
export function getTableColumnKey(col: ColumnItem | Record<string, unknown>): string | undefined {
  const c = col as { key?: string | number; dataIndex?: string | number };
  const k = c.key ?? c.dataIndex;
  return k != null && k !== '' ? String(k) : undefined;
}

/**
 * 基类字段 i18n 键（common.page.entity.*）
 * @param field 字段名
 */
export function resolveEntityBaseFieldI18nKey(field: string): string {
  return `common.page.entity.${field.toLowerCase()}`;
}

/**
 * 基类字段 → 表格列
 * @param field 字段名
 * @param t 翻译函数
 */
function buildBaseFieldColumn(field: string, t: (key: string) => string): ColumnType<RowRecord> {
  const title = t(resolveEntityBaseFieldI18nKey(field));
  if (DATETIME_FIELDS.has(field)) {
    return {
      key: field,
      dataIndex: field,
      title,
      width: 180,
      ellipsis: true,
      sorter: (a: RowRecord, b: RowRecord) =>
        new Date(String(a[field] ?? 0)).getTime() - new Date(String(b[field] ?? 0)).getTime(),
    };
  }
  if (NUMBER_FIELDS.has(field)) {
    return {
      key: field,
      dataIndex: field,
      title,
      width: 100,
      ellipsis: true,
      sorter: (a: RowRecord, b: RowRecord) => Number(a[field] ?? 0) - Number(b[field] ?? 0),
    };
  }
  const width = field === 'tenantCode' || field === 'companyCode' ? 100 : field === 'remark' || field === 'ExtField' || field === 'approvalOpinion' ? 150 : 120;
  return { key: field, dataIndex: field, title, width, ellipsis: true };
}

/**
 * 按 entityScope 生成基类列
 * @param t 翻译函数
 * @param includeAuditFields 是否含审计字段
 * @param entityScope 基类作用域
 */
export function getDefaultEntityColumns(
  t: (key: string) => string,
  includeAuditFields: boolean = true,
  entityScope: TaktEntityScope = 'company',
): TableColumnsType {
  const fields = resolveEntityScopeBaseFieldKeys(entityScope).filter(
    (field) => includeAuditFields || !AUDIT_FIELD_SET.has(field),
  );
  return fields.map((field) => buildBaseFieldColumn(field, t));
}

/**
 * 业务列 + 基类列合并（同 key 以业务列为准；基类列插在业务列与操作列之间）
 * @param userColumns 页面业务列
 * @param t 翻译函数
 * @param includeAuditFields 是否含审计字段
 * @param entityScope 基类作用域
 */
export function mergeDefaultColumns(
  userColumns: TableColumnsType,
  t: (key: string) => string,
  includeAuditFields: boolean = true,
  entityScope: TaktEntityScope = 'company',
): TableColumnsType {
  const userKeys = new Set<string>();
  for (const col of userColumns) {
    if ('children' in col && col.children) {
      for (const child of col.children) {
        const key = getTableColumnKey(child);
        if (key) userKeys.add(key);
      }
    } else {
      const key = getTableColumnKey(col as ColumnItem);
      if (key) userKeys.add(key);
    }
  }
  const baseColumns = getDefaultEntityColumns(t, includeAuditFields, entityScope).filter((col) => {
    const key = getTableColumnKey(col as ColumnItem);
    return key != null && !userKeys.has(key);
  });
  const body: TableColumnsType = [];
  const actions: TableColumnsType = [];
  for (const col of userColumns) {
    const item = col as ColumnItem;
    const key = getTableColumnKey(item);
    if (key === 'action' || item.fixed === 'right') {
      actions.push(col);
    } else {
      body.push(col);
    }
  }
  return [...body, ...baseColumns, ...actions];
}

/**
 * 按可见列键过滤
 * @param mergedColumns 已合并列
 * @param visibleKeys 可见键
 * @param fallbackColumns 空 keys 时的回退
 */
export function filterTableColumnsByVisibleKeys(
  mergedColumns: TableColumnsType,
  visibleKeys: string[],
  fallbackColumns?: TableColumnsType,
): TableColumnsType {
  if (!visibleKeys.length) {
    return fallbackColumns ?? mergedColumns;
  }
  const keySet = new Set(visibleKeys.map(String));
  return mergedColumns.filter((col) => {
    const key = getTableColumnKey(col as ColumnItem);
    return key != null && keySet.has(key);
  });
}

/**
 * 从业务 columns 提取自有字段键（排除 id、action、当前 scope 基类字段）
 */
export function extractBusinessColumnKeys(
  userColumns: TableColumnsType,
  idColumnKey: string | number = 'id',
  actionColumnKey: string | number = 'action',
  entityScope: TaktEntityScope = 'company',
): string[] {
  const idKey = String(idColumnKey);
  const actionKey = String(actionColumnKey);
  const baseKeys = new Set(resolveEntityScopeBaseFieldKeys(entityScope));
  const keys: string[] = [];
  for (const col of userColumns) {
    const key = getTableColumnKey(col as ColumnItem);
    if (!key || key === idKey || key === actionKey || baseKeys.has(key)) {
      continue;
    }
    keys.push(key);
  }
  return keys;
}

/**
 * 默认可见列：ID + 前 N 个业务字段 + 操作列
 */
export function resolveDefaultVisibleColumnKeys(
  userColumns: TableColumnsType,
  options: {
    idColumnKey?: string | number;
    actionColumnKey?: string | number;
    tableMode?: TaktTableLayoutMode;
    entityScope?: TaktEntityScope;
  } = {},
): string[] {
  const idKey = String(options.idColumnKey ?? 'id');
  const actionKey = String(options.actionColumnKey ?? 'action');
  const tableMode = options.tableMode ?? 'single';
  const entityScope = options.entityScope ?? 'company';
  const count = DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT[tableMode];
  const businessKeys = extractBusinessColumnKeys(userColumns, idKey, actionKey, entityScope);
  return [idKey, ...businessKeys.slice(0, Math.max(0, count)), actionKey];
}

/**
 * 按默认可见规则过滤已合并列
 */
export function filterMergedColumnsByDefaultVisible(
  mergedColumns: TableColumnsType,
  userColumns: TableColumnsType,
  options: {
    idColumnKey?: string | number;
    actionColumnKey?: string | number;
    tableMode?: TaktTableLayoutMode;
    entityScope?: TaktEntityScope;
  } = {},
): TableColumnsType {
  return filterTableColumnsByVisibleKeys(
    mergedColumns,
    resolveDefaultVisibleColumnKeys(userColumns, options),
    mergedColumns,
  );
}
