// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/table-scroll
// 文件名称：table-scroll.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：Ant Design Vue 表格 scroll 纯函数；scroll.y 为布局高度（与数据行数无关），空表亦保持固定高度
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

/** 表格默认 scroll.y 布局场景（全局唯一配置入口） */
export type TaktTableScrollLayout =
  | 'page'
  | 'treeRight'
  | 'masterDetailLr'
  | 'masterDetailTbMaster'
  | 'masterDetailTbDetail'
  | 'editable';

/**
 * 固定顶栏 TaktHeader 默认高度（px），与 layouts/side|top|mix 及 takt-header 默认 height=40 对齐
 */
export const TAKT_LAYOUT_HEADER_HEIGHT_PX = 40;

/**
 * 固定底栏 TaktFooter 默认高度（px），与 layouts 中 TaktFooter :height="40" 对齐
 */
export const TAKT_LAYOUT_FOOTER_HEIGHT_PX = 40;

/**
 * 顶栏与底栏之外、表格主体之上的页面壳预留（px）：多标签 TaktTabs(40) + 查询栏 + 工具栏 + 外置分页 + p-4 等
 * 与 layouts contentMaxHeight 中除 header 外的 84px（footer+标签）及 CRUD 页壳分区对齐
 */
export const TAKT_TABLE_PAGE_SHELL_BELOW_HEADER_PX = 220;

/**
 * 整页列表 scroll.y 视口总预留 = 固定 header + 固定 footer + 页面壳（等价历史 calc(100vh - 300px)）
 */
export const TAKT_TABLE_VIEWPORT_CHROME_RESERVE =
  TAKT_LAYOUT_HEADER_HEIGHT_PX + TAKT_LAYOUT_FOOTER_HEIGHT_PX + TAKT_TABLE_PAGE_SHELL_BELOW_HEADER_PX;

/**
 * @deprecated 请使用 computeTableScrollYPx；仅保留给仍传 :scroll="{ y: TAKT_VIEWPORT_TABLE_SCROLL_Y }" 的旧页面
 */
export const TAKT_VIEWPORT_TABLE_SCROLL_Y = `calc(100vh - ${TAKT_TABLE_VIEWPORT_CHROME_RESERVE}px)`;

/** 表单内嵌可编辑子表默认纵向滚动高度（px） */
export const TAKT_EDITABLE_TABLE_DEFAULT_SCROLL_Y = 240;

/** scroll.y 像素下限（避免视口过小时表格不可滚动） */
export const TAKT_TABLE_SCROLL_Y_MIN = 200;

/** 无 window 时的视口高度回退（SSR/单测） */
export const TAKT_TABLE_VIEWPORT_HEIGHT_FALLBACK = 800;

/** 左树虚拟滚动测量失败时的回退高度（px） */
export const TAKT_TREE_LEFT_VIRTUAL_HEIGHT_FALLBACK = 400;

/**
 * 解析用于计算的视口高度
 * @param viewportHeight 调用方传入的视口高度；缺省时读 window.innerHeight
 * @returns 有效视口像素高度
 */
export function resolveTableViewportHeight(viewportHeight?: number): number {
  if (typeof viewportHeight === 'number' && Number.isFinite(viewportHeight) && viewportHeight > 0) {
    return viewportHeight;
  }
  if (typeof window !== 'undefined' && typeof window.innerHeight === 'number' && window.innerHeight > 0) {
    return window.innerHeight;
  }
  return TAKT_TABLE_VIEWPORT_HEIGHT_FALLBACK;
}

/** 左右主子表明细侧标题/工具栏/外置分页等占用（px，视口 fallback；组件内优先实测 chrome） */
export const TAKT_TABLE_MASTER_DETAIL_LR_DETAIL_CHROME_PX = 152;

/** TaktPagination 默认占用高度（px，与 takt-pagination size=middle 对齐） */
export const TAKT_TABLE_PAGINATION_HEIGHT_PX = 40;

/** Ant Design Table 表头高度回退（size=middle） */
export const TAKT_TABLE_HEADER_FALLBACK_PX = 56;

/** Ant Design Table 汇总行高度回退（size=middle，含 fixed summary） */
export const TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX = 48;

/**
 * 左右主子表共享 scroll.y（以组件 pane 高度与两侧 chrome 实测为准，保证左右表体默认等高）
 * @param paneHeight 组件根或 pane 可用高度（px）
 * @param options.masterChromePx 左侧 master-toolbar 实测高度
 * @param options.detailChromePx 右侧 detailTitle + detail-toolbar 实测高度
 * @param options.paginationPx 表内分页占用（主表 showPagination 时）
 * @param options.tableHeaderPx 表头高度回退
 * @returns 共享 scroll.y（px）
 */
export function computeMasterDetailLrSharedScrollYPx(
  paneHeight: number,
  options: {
    masterChromePx?: number;
    detailChromePx?: number;
    paginationPx?: number;
    tableHeaderPx?: number;
  } = {},
): number {
  const masterChrome = options.masterChromePx ?? 0;
  const detailChrome = options.detailChromePx ?? 0;
  const chrome = Math.max(masterChrome, detailChrome);
  const pagination = options.paginationPx ?? 0;
  const header = options.tableHeaderPx ?? TAKT_TABLE_HEADER_FALLBACK_PX;
  const raw = paneHeight - chrome - pagination - header;
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, raw);
}

/**
 * 按布局场景与视口高度计算 scroll.y（像素，Ant Design Table 推荐写法）
 * @param layout 表格所在布局场景
 * @param viewportHeight 视口高度；缺省为 window.innerHeight
 * @returns scroll.y 像素值
 */
export function computeTableScrollYPx(
  layout: TaktTableScrollLayout = 'page',
  viewportHeight?: number,
): number {
  const vh = resolveTableViewportHeight(viewportHeight);
  let raw: number;
  switch (layout) {
    case 'masterDetailTbMaster':
      raw = Math.floor(vh * 0.4 - 72);
      break;
    case 'masterDetailTbDetail':
      raw = Math.floor(vh * 0.55 - 96);
      break;
    case 'masterDetailLr':
      // 左右主子表：扣除页壳与右栏明细标题/工具栏/分页，避免 scroll.y 过大导致表头表体横向错位
      raw = vh - TAKT_TABLE_VIEWPORT_CHROME_RESERVE - TAKT_TABLE_MASTER_DETAIL_LR_DETAIL_CHROME_PX;
      break;
    case 'editable':
      raw = TAKT_EDITABLE_TABLE_DEFAULT_SCROLL_Y;
      break;
    default:
      // innerHeight 减去固定 header/footer 与列表页壳，与 calc(100vh - 300px) 语义一致
      raw = vh - TAKT_TABLE_VIEWPORT_CHROME_RESERVE;
      break;
  }
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, raw);
}

/**
 * 合并页面显式 scroll.y 与视口动态计算结果
 * @param scrollYOverride 页面 :scroll.y 覆盖值
 * @param viewportScrollYPx useTaktTableViewportScrollY 输出
 * @returns 传给 a-table 的 scroll.y
 */
export function resolveVerticalScrollY(
  scrollYOverride: number | string | undefined | null,
  viewportScrollYPx: number,
): number | string {
  if (scrollYOverride != null && scrollYOverride !== '') {
    return scrollYOverride;
  }
  return viewportScrollYPx;
}

/**
 * 按布局场景解析默认 scroll.y（像素）
 * @param layout 表格所在布局场景
 * @returns 默认纵向滚动高度（px）
 */
export function resolveDefaultTableScrollY(layout: TaktTableScrollLayout = 'page'): number {
  return computeTableScrollYPx(layout);
}

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
 * @param options.enableVerticalScroll 是否配置 scroll.y（为 true 时与数据行数无关，空表亦保持固定高度）
 * @param options.scrollLayout 布局场景（未传 verticalScrollHeight 时用于默认 scroll.y）
 * @param options.verticalScrollHeight 显式覆盖默认 scroll.y（优先于 scrollLayout）
 * @returns scroll 对象
 */
export function resolveTableScrollConfig(options: {
  columns: TableColumnsType;
  scroll?: TaktTableScrollConfig;
  includeRowSelection?: boolean;
  enableVerticalScroll?: boolean;
  scrollLayout?: TaktTableScrollLayout;
  verticalScrollHeight?: number | string;
}): TaktTableScrollConfig {
  const {
    columns,
    scroll,
    includeRowSelection = false,
    enableVerticalScroll = false,
    scrollLayout = 'page',
    verticalScrollHeight,
  } = options;
  const resolvedVerticalHeight = verticalScrollHeight ?? resolveDefaultTableScrollY(scrollLayout);
  const config: TaktTableScrollConfig = { ...scroll };
  if (config.x == null || config.x === true) {
    let totalWidth = sumTableColumnsPixelWidth(columns);
    if (includeRowSelection) {
      totalWidth += TAKT_TABLE_ROW_SELECTION_WIDTH;
    }
    config.x = totalWidth > 0 ? totalWidth : TAKT_TABLE_DEFAULT_COLUMN_WIDTH;
  }
  if (enableVerticalScroll) {
    // 企业级规范：scroll.y 表示布局高度，与 dataSource 行数无关；空数据亦须固定高度避免页面抖动
    if (config.y == null || config.y === '') {
      config.y = resolvedVerticalHeight;
    } else if (typeof config.y === 'string') {
      const trimmed = config.y.trim();
      if (/^\d+(\.\d+)?$/.test(trimmed)) {
        config.y = Number.parseInt(trimmed, 10);
      } else if (/^\d+(\.\d+)?px$/i.test(trimmed)) {
        config.y = Number.parseInt(trimmed, 10);
      }
    }
  }
  return config;
}
