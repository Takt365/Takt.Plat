// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/announcement
// 文件名称：announcement-stat.d.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知件数统计（数据看板 announcement-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 公告通知件数统计（按发布时间）
 * @description 对应后端 TaktAnnouncementStatDto
 */
export interface AnnouncementStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 通知件数 */
  monthAnnouncementCount: number
}
