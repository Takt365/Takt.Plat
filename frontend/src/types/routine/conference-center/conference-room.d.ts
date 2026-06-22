// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/conference-center
// 文件名称：conference-room.d.ts
// 创建时间：2026-06-21
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
   * ConferenceRoomID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  conferenceRoomId: string;

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

}


/**
 * ConferenceRoom 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ConferenceRoomQuery
 * @description 对应后端 TaktConferenceRoomQueryDto
 */
export interface ConferenceRoomQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

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
   * 排序号
   */
  sortOrder?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ConferenceRoom DTO
 * 对应前端 ConferenceRoomCreate
 * @description 对应后端 TaktConferenceRoomCreateDto
 */
export interface ConferenceRoomCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

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
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新ConferenceRoom DTO
 * 继承 TaktConferenceRoomCreateDto，添加 ConferenceRoomId 字段
 * 对应前端 ConferenceRoomUpdate
 * @description 对应后端 TaktConferenceRoomUpdateDto
 */
export interface ConferenceRoomUpdate extends ConferenceRoomCreate {
  /**
   * ConferenceRoomID（标识要更新的实体）
   */
  conferenceRoomId: string;

}


/**
 * ConferenceRoom 状态更新 DTO
 * 对应前端 ConferenceRoomStatus
 * @description 对应后端 TaktConferenceRoomStatusDto
 */
export interface ConferenceRoomStatus {
  /**
   * ConferenceRoomID
   */
  conferenceRoomId: string;

  /**
   * 会议室状态
   */
  roomStatus: number;

}


/**
 * ConferenceRoom 排序更新 DTO
 * 对应前端 ConferenceRoomSort
 * @description 对应后端 TaktConferenceRoomSortDto
 */
export interface ConferenceRoomSort {
  /**
   * ConferenceRoomID
   */
  conferenceRoomId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * ConferenceRoom 导入模板行 DTO
 * 对应前端 ConferenceRoomTemplate
 * @description 对应后端 TaktConferenceRoomTemplateDto
 */
export interface ConferenceRoomTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

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
 * ConferenceRoom 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ConferenceRoomImport
 * @description 对应后端 TaktConferenceRoomImportDto
 */
export interface ConferenceRoomImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

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

