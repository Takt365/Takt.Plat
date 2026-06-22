// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：foundation-enums.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 域运行时枚举（与后端 Takt.Shared.Enums 数值对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 在线用户状态（与后端 TaktOnlineStatus 一致）
 */
export enum TaktOnlineStatus {
  /** 在线 */
  Online = 0,
  /** 离线 */
  Offline = 1,
  /** 离开（含强退） */
  Away = 2,
}

/**
 * 客户端设备类型（与后端 TaktDeviceType 一致）
 */
export enum TaktDeviceType {
  /** 未知 */
  Unknown = 0,
  /** PC */
  Pc = 1,
  /** 手机 */
  Mobile = 2,
  /** 平板 */
  Tablet = 3,
}

/**
 * 浏览器类型（与后端 TaktBrowserType 一致）
 */
export enum TaktBrowserType {
  /** 未知 */
  Unknown = 0,
  /** Chrome */
  Chrome = 1,
  /** Firefox */
  Firefox = 2,
  /** Safari */
  Safari = 3,
  /** Edge */
  Edge = 4,
}

/**
 * 操作系统类型（与后端 TaktOperatingSystem 一致）
 */
export enum TaktOperatingSystem {
  /** 未知 */
  Unknown = 0,
  /** Windows */
  Windows = 1,
  /** macOS */
  MacOS = 2,
  /** Linux */
  Linux = 3,
  /** Android */
  Android = 4,
  /** iOS */
  IOS = 5,
}

/**
 * 在线消息内容类型（与后端 TaktMessageType、字典 sys_message_type 一致）
 */
export enum TaktMessageType {
  /** 文本 */
  Text = 1,
  /** 图片 */
  Image = 2,
  /** 文件 */
  File = 3,
  /** 系统消息 */
  System = 4,
  /** 视频 */
  Video = 5,
  /** 语音 */
  Voice = 6,
}

/**
 * 在线消息分组（与后端 TaktMessageGroup、字典 sys_message_group_category 一致）
 */
export enum TaktMessageGroup {
  /** 协同 */
  Collaboration = 1,
  /** 公文 */
  OfficialDocument = 2,
  /** 文档 */
  Document = 3,
  /** 公告 */
  Announcement = 4,
  /** 其他 */
  Other = 5,
  /** 消息 */
  Message = 6,
  /** 提醒 */
  Reminder = 7,
}
