// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference-room.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/conference-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会议室实体 维护线下会议室编码、位置、容量与设施，供会议排期预约
 * 对应前端 TaktConferenceRoomDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConferenceRoom
 * @description 对应后端 TaktConferenceRoomDto
 */
export interface ConferenceRoom extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 会议室编码（租户+公司内唯一）
   */
  roomCode?: string;

  /**
   * 会议室名称
   */
  roomName?: string;

  /**
   * 楼栋/建筑
   */
  building?: string;

  /**
   * 楼层
   */
  floor?: string;

  /**
   * 详细位置说明
   */
  locationDetail?: string;

  /**
   * 容纳人数（0 表示不限）
   */
  capacity?: number;

  /**
   * 设施说明（投影、视频会议设备等）
   */
  facilities?: string;

  /**
   * 会议室状态
   */
  roomStatus?: number;

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
 * ConferenceRoom 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConferenceRoomExport
 * @description 对应后端 TaktConferenceRoomExportDto
 */
export interface ConferenceRoomExport {
  /**
   * ConferenceRoomID
   */
  conferenceRoomId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 会议室编码（租户+公司内唯一）
   */
  roomCode: string;

  /**
   * 会议室名称
   */
  roomName: string;

  /**
   * 楼栋/建筑
   */
  building?: string;

  /**
   * 楼层
   */
  floor?: string;

  /**
   * 详细位置说明
   */
  locationDetail?: string;

  /**
   * 容纳人数（0 表示不限）
   */
  capacity: number;

  /**
   * 设施说明（投影、视频会议设备等）
   */
  facilities?: string;

  /**
   * 会议室状态
   */
  roomStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

