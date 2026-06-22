// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：equipment.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt工厂设备实体
 * 对应前端 TaktEquipmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Equipment
 * @description 对应后端 TaktEquipmentDto
 */
export interface Equipment extends CompanyDtoBase {
  /**
   * EquipmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  equipmentId: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 安装日期
   */
  installationDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修结束日期
   */
  warrantyEndDate?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus: number;

  /**
   * 维护通知单列表 （子表：TaktMaintenanceNotification）
   */
  maintenanceNotifications?: MaintenanceNotification[];

  /**
   * 维护工单列表 （子表：TaktMaintenanceWorkOrder）
   */
  maintenanceWorkOrders?: MaintenanceWorkOrder[];

  /**
   * 维护履历列表（由维护工单完工归档生成，只读） （子表：TaktMaintenanceHistory）
   */
  maintenanceHistories?: MaintenanceHistory[];

}


/**
 * Equipment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EquipmentQuery
 * @description 对应后端 TaktEquipmentQueryDto
 */
export interface EquipmentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType?: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期（范围查询-开始）
   */
  purchaseDateStart?: string;

  /**
   * 购买日期（范围查询-结束）
   */
  purchaseDateEnd?: string;

  /**
   * 安装日期（范围查询-开始）
   */
  installationDateStart?: string;

  /**
   * 安装日期（范围查询-结束）
   */
  installationDateEnd?: string;

  /**
   * 启用日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 启用日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 保修开始日期（范围查询-开始）
   */
  warrantyStartDateStart?: string;

  /**
   * 保修开始日期（范围查询-结束）
   */
  warrantyStartDateEnd?: string;

  /**
   * 保修结束日期（范围查询-开始）
   */
  warrantyEndDateStart?: string;

  /**
   * 保修结束日期（范围查询-结束）
   */
  warrantyEndDateEnd?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue?: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical?: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus?: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus?: number;

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
 * 创建Equipment DTO
 * 对应前端 EquipmentCreate
 * @description 对应后端 TaktEquipmentCreateDto
 */
export interface EquipmentCreate {
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
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 安装日期
   */
  installationDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修结束日期
   */
  warrantyEndDate?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus: number;

  /**
   * 维护通知单列表（子表，级联保存）
   */
  maintenanceNotifications?: MaintenanceNotificationCreate[];

  /**
   * 维护工单列表（子表，级联保存）
   */
  maintenanceWorkOrders?: MaintenanceWorkOrderCreate[];

  /**
   * 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
   */
  maintenanceHistories?: MaintenanceHistoryCreate[];

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
 * 更新Equipment DTO
 * 继承 TaktEquipmentCreateDto，添加 EquipmentId 字段
 * 对应前端 EquipmentUpdate
 * @description 对应后端 TaktEquipmentUpdateDto
 */
export interface EquipmentUpdate extends EquipmentCreate {
  /**
   * EquipmentID（标识要更新的实体）
   */
  equipmentId: string;

}


/**
 * Equipment 状态更新 DTO
 * 对应前端 EquipmentStatus
 * @description 对应后端 TaktEquipmentStatusDto
 */
export interface EquipmentStatus {
  /**
   * EquipmentID
   */
  equipmentId: string;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus: number;

}


/**
 * Equipment 导入模板行 DTO
 * 对应前端 EquipmentTemplate
 * @description 对应后端 TaktEquipmentTemplateDto
 */
export interface EquipmentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType?: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

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
 * Equipment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EquipmentImport
 * @description 对应后端 TaktEquipmentImportDto
 */
export interface EquipmentImport {
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
   * 工厂代码（不可空）
   */
  plantCode?: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType?: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

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
 * Equipment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EquipmentExport
 * @description 对应后端 TaktEquipmentExportDto
 */
export interface EquipmentExport {
  /**
   * EquipmentID
   */
  equipmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  equipmentCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 设备类型（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  equipmentSpecification?: string;

  /**
   * 设备品牌
   */
  equipmentBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编号
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 安装日期
   */
  installationDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修结束日期
   */
  warrantyEndDate?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus: number;

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

