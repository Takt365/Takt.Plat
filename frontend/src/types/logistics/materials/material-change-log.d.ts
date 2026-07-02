// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-change-log.d.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * 全局物料变更记录实体
 * 对应前端 TaktMaterialChangeLogDto
 * 继承 TaktTenantDtoBase
 * 对应前端 MaterialChangeLog
 * @description 对应后端 TaktMaterialChangeLogDto
 */
export interface MaterialChangeLog extends TenantDtoBase {
  /**
   * MaterialChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialChangeLogId: string;

  /**
   * 全局物料 ID（关联 TaktMaterial.Id，选项 TaktMaterials/options）
   */
  materialId: string;

  /**
   * 全局物料 名称（填充字段）
   */
  materialName?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，冗余；选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 全局物料主表 （主表：TaktMaterial）
   */
  material?: Material;

}


/**
 * MaterialChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialChangeLogQuery
 * @description 对应后端 TaktMaterialChangeLogQueryDto
 */
export interface MaterialChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 全局物料 ID（关联 TaktMaterial.Id，选项 TaktMaterials/options）
   */
  materialId?: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，冗余；选项 TaktMaterials/options）
   */
  materialCode?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

  /**
   * 变更人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 创建MaterialChangeLog DTO
 * 对应前端 MaterialChangeLogCreate
 * @description 对应后端 TaktMaterialChangeLogCreateDto
 */
export interface MaterialChangeLogCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 全局物料 ID（关联 TaktMaterial.Id，选项 TaktMaterials/options）
   */
  materialId: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，冗余；选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 更新MaterialChangeLog DTO
 * 继承 TaktMaterialChangeLogCreateDto，添加 MaterialChangeLogId 字段
 * 对应前端 MaterialChangeLogUpdate
 * @description 对应后端 TaktMaterialChangeLogUpdateDto
 */
export interface MaterialChangeLogUpdate extends MaterialChangeLogCreate {
  /**
   * MaterialChangeLogID（标识要更新的实体）
   */
  materialChangeLogId: string;

}


/**
 * MaterialChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialChangeLogExport
 * @description 对应后端 TaktMaterialChangeLogExportDto
 */
export interface MaterialChangeLogExport {
  /**
   * MaterialChangeLogID
   */
  materialChangeLogId: string;

  /**
   * 全局物料 ID（关联 TaktMaterial.Id，选项 TaktMaterials/options）
   */
  materialId: string;

  /**
   * 物料编码（关联 TaktMaterial.MaterialCode，冗余；选项 TaktMaterials/options）
   */
  materialCode: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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

