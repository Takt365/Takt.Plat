// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：common-enums.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：全项目通用运行时枚举（与后端 Takt.Shared.Enums.TaktCommonEnums 数值对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 已读状态（与后端 TaktReadStatus 一致；消息、新闻、邮件等通用）
 */
export enum TaktReadStatus {
  /** 未读 */
  Unread = 0,
  /** 已读 */
  Read = 1,
}
