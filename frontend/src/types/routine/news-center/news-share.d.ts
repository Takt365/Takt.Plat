// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news-share.d.ts
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
 * 新闻中心分享记录实体
 * 对应前端 TaktNewsShareDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsShare
 * @description 对应后端 TaktNewsShareDto
 */
export interface NewsShare extends CompanyDtoBase {
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
   * 分享人 ID
   */
  userId?: string;

  /**
   * 分享人姓名
   */
  userName?: string;

  /**
   * 分享渠道（如 wechat、link 等）
   */
  shareChannel?: string;

  /**
   * 分享时间
   */
  shareTime?: string;

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
 * NewsShare 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsShareExport
 * @description 对应后端 TaktNewsShareExportDto
 */
export interface NewsShareExport {
  /**
   * NewsShareID
   */
  newsShareId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 新闻 ID
   */
  newsId: string;

  /**
   * 分享人 ID
   */
  userId: string;

  /**
   * 分享人姓名
   */
  userName: string;

  /**
   * 分享渠道（如 wechat、link 等）
   */
  shareChannel?: string;

  /**
   * 分享时间
   */
  shareTime: string;

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

