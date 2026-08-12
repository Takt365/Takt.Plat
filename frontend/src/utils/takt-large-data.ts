// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/takt-large-data
// 文件名称：takt-large-data.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：前端大数据自动处理阈值（下拉虚拟+远程、表格虚拟）；❌ 不是业务数据条数上限、禁止据此截断数据
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 大数据自动处理阈值（条）
 * - TaktSelect：达到后自动 virtual + remote-search（apiUrl）
 * - takt-*-table：达到后即使页面传 virtual=false 也强制 virtual
 * ❌ 禁止用本常量对 options / list 做 Take / slice 截断
 */
export const TAKT_LARGE_DATA_AUTO_THRESHOLD = 3000
