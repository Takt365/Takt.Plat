// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：quality-scrap-item.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 品质废弃明细 - 废弃零件明细行
 * 对应前端 TaktQualityScrapItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityScrapItem
 * @description 对应后端 TaktQualityScrapItemDto
 */
export interface QualityScrapItem extends CompanyDtoBase {
  /**
   * QualityScrapItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityScrapItemId: string;

  /**
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId: string;

  /**
   * 品质废弃主表名称（填充字段）
   */
  qualityScrapName?: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 品质废弃主表(导航属性) （主表：TaktQualityScrap）
   */
  scrap?: QualityScrap;

}


/**
 * QualityScrapItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityScrapItemQuery
 * @description 对应后端 TaktQualityScrapItemQueryDto
 */
export interface QualityScrapItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId?: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 废弃费用(元)
   */
  scrapCost?: number;

  /**
   * 废弃数量
   */
  scrapSize?: number;

  /**
   * 零件单价(元)
   */
  partPrice?: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost?: number;

  /**
   * 运费(元)
   */
  freightCharges?: number;

  /**
   * 其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 关税(元)
   */
  tax?: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建QualityScrapItem DTO
 * 对应前端 QualityScrapItemCreate
 * @description 对应后端 TaktQualityScrapItemCreateDto
 */
export interface QualityScrapItemCreate {
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
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新QualityScrapItem DTO
 * 继承 TaktQualityScrapItemCreateDto，添加 QualityScrapItemId 字段
 * 对应前端 QualityScrapItemUpdate
 * @description 对应后端 TaktQualityScrapItemUpdateDto
 */
export interface QualityScrapItemUpdate extends QualityScrapItemCreate {
  /**
   * QualityScrapItemID（标识要更新的实体）
   */
  qualityScrapItemId: string;

}


/**
 * QualityScrapItem 导入模板行 DTO
 * 对应前端 QualityScrapItemTemplate
 * @description 对应后端 TaktQualityScrapItemTemplateDto
 */
export interface QualityScrapItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId?: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * QualityScrapItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityScrapItemImport
 * @description 对应后端 TaktQualityScrapItemImportDto
 */
export interface QualityScrapItemImport {
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
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId?: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * QualityScrapItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityScrapItemExport
 * @description 对应后端 TaktQualityScrapItemExportDto
 */
export interface QualityScrapItemExport {
  /**
   * QualityScrapItemID
   */
  qualityScrapItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质废弃主表ID(主子表关系,序列化为string以避免Javascript精度问题)
   */
  qualityScrapId: string;

  /**
   * 品质废弃编码（冗余字段，便于查询）
   */
  qualityScrapCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

