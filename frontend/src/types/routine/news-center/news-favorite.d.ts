// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-favorite.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 新闻中心收藏记录实体
 * 对应前端 TaktNewsFavoriteDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsFavorite
 * @description 对应后端 TaktNewsFavoriteDto
 */
export interface NewsFavorite extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 新闻 ID
   */
  newsId?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 收藏时间
   */
  favoriteTime?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * NewsFavorite 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsFavoriteExport
 * @description 对应后端 TaktNewsFavoriteExportDto
 */
export interface NewsFavoriteExport {
  /**
   * NewsFavoriteID
   */
  newsFavoriteId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 收藏时间
   */
  favoriteTime: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

