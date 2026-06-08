// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/table-scroll
// 文件名称：table-scroll.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：Ant Design Vue 表格 scroll.x / 列宽展示纯函数（与 takt-single-table、takt-tree-right-table 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue';
import type { ColumnGroupType, ColumnType } from 'ant-design-vue/es/table/interface';

type TableColumnItem = ColumnType<Record<string, unknown>> | ColumnGroupType<Record<string, unknown>>;

/** 行选择列占用宽度（与 Ant Design Table selection 列对齐） */
export const TAKT_TABLE_ROW_SELECTION_WIDTH = 48;

/** 未显式配置 width 时的默认列宽 */
export const TAKT_TABLE_DEFAULT_COLUMN_WIDTH = 120;

/** 表格 scroll 配置 */
export type TaktTableScrollConfig = {
  x?: number | string | true;
  y?: number | string;
};

/**
 * 解析单列数值宽度
 * @param column 列配置
 * @param fallback 无有效 width 时的回退宽度
 * @returns 像素宽度
 */
export function getTableColumnPixelWidth(
  column: { width?: string | number | null | undefined },
  fallback: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): number {
  const width = column.width;
  if (typeof width === 'number' && width > 0) {
    return width;
  }
  if (typeof width === 'string' && width.trim()) {
    const parsed = Number.parseInt(width, 10);
    if (!Number.isNaN(parsed) && parsed > 0) {
      return parsed;
    }
  }
  return fallback;
}

/**
 * 累计列宽（含分组列 children）
 * @param columns 展示列
 * @param fallbackWidth 叶子列无 width 时的回退宽度
 * @returns 总像素宽度
 */
export function sumTableColumnsPixelWidth(
  columns: TableColumnsType,
  fallbackWidth: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): number {
  if (!columns?.length) {
    return 0;
  }
  return columns.reduce((sum, column) => {
    const item = column as TableColumnItem;
    if ('children' in item && item.children?.length) {
      return sum + sumTableColumnsPixelWidth(item.children as TableColumnsType, fallbackWidth);
    }
    return sum + getTableColumnPixelWidth(item, fallbackWidth);
  }, 0);
}

/**
 * 为列补充默认 width 与 ellipsis（不修改原对象引用外的共享状态）
 * @param columns 列配置
 * @param defaultEllipsis 是否默认 ellipsis
 * @param defaultWidth 无 width 时的默认列宽
 * @returns 新列数组
 */
export function applyTableColumnPresentation(
  columns: TableColumnsType,
  defaultEllipsis: boolean,
  defaultWidth: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): TableColumnsType {
  if (!columns?.length) {
    return [];
  }
  return columns.map((column) => {
    const processedColumn = { ...column } as ColumnType<Record<string, unknown>>;
    if (!processedColumn.width) {
      processedColumn.width = defaultWidth;
    }
    if (defaultEllipsis && processedColumn.ellipsis === undefined) {
      processedColumn.ellipsis = true;
    }
    return processedColumn;
  });
}

/**
 * 解析 Ant Design Table 的 scroll 配置（横向必须为数值总和以启用固定列宽 + 横向滚动条）
 * @param options.columns 最终展示列（已含 width）
 * @param options.scroll 页面传入的 scroll 覆盖项
 * @param options.includeRowSelection 是否包含行选择列宽
 * @param options.enableVerticalScroll 是否配置 scroll.y
 * @param options.verticalScrollHeight 纵向滚动高度
 * @returns scroll 对象
 */
export function resolveTableScrollConfig(options: {
  columns: TableColumnsType;
  scroll?: TaktTableScrollConfig;
  includeRowSelection?: boolean;
  enableVerticalScroll?: boolean;
  verticalScrollHeight?: number | string;
}): TaktTableScrollConfig {
  const {
    columns,
    scroll,
    includeRowSelection = false,
    enableVerticalScroll = false,
    verticalScrollHeight = 600,
  } = options;
  const config: TaktTableScrollConfig = { ...scroll };
  if (config.x == null || config.x === true) {
    let totalWidth = sumTableColumnsPixelWidth(columns);
    if (includeRowSelection) {
      totalWidth += TAKT_TABLE_ROW_SELECTION_WIDTH;
    }
    config.x = totalWidth > 0 ? totalWidth : TAKT_TABLE_DEFAULT_COLUMN_WIDTH;
  }
  if (enableVerticalScroll && config.y == null) {
    config.y = verticalScrollHeight;
  }
  return config;
}
