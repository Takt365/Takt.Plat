// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：inspection-standard-item.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 检验标准明细实体
 * 对应前端 TaktInspectionStandardItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 InspectionStandardItem
 * @description 对应后端 TaktInspectionStandardItemDto
 */
export interface InspectionStandardItem extends CompanyDtoBase {
  /**
   * InspectionStandardItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  inspectionStandardItemId: string;

  /**
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId: string;

  /**
   * 检验标准名称（填充字段）
   */
  inspectionStandardName?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 检验项目编码
   */
  itemCode: string;

  /**
   * 检验项目名称
   */
  itemName: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode: number;

  /**
   * 检验标准值
   */
  standardValue: string;

  /**
   * 检验上限值
   */
  upperLimit: string;

  /**
   * 检验下限值
   */
  lowerLimit: string;

  /**
   * 检验工具
   */
  inspectionTool: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription: string;

  /**
   * 接收标准（AC值）
   */
  acceptanceCriteria: string;

  /**
   * 拒收标准（RE值）
   */
  rejectionCriteria: string;

  /**
   * 是否合格判定项目（0=否，1=是）
   */
  isQualifiedBasis: number;

  /**
   * 检验标准（主表） （主表：TaktInspectionStandard）
   */
  standard?: InspectionStandard;

}


/**
 * InspectionStandardItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 InspectionStandardItemQuery
 * @description 对应后端 TaktInspectionStandardItemQueryDto
 */
export interface InspectionStandardItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 检验项目编码
   */
  itemCode?: string;

  /**
   * 检验项目名称
   */
  itemName?: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType?: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode?: number;

  /**
   * 检验标准值
   */
  standardValue?: string;

  /**
   * 检验上限值
   */
  upperLimit?: string;

  /**
   * 检验下限值
   */
  lowerLimit?: string;

  /**
   * 检验工具
   */
  inspectionTool?: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription?: string;

  /**
   * 接收标准（AC值）
   */
  acceptanceCriteria?: string;

  /**
   * 拒收标准（RE值）
   */
  rejectionCriteria?: string;

  /**
   * 是否合格判定项目（0=否，1=是）
   */
  isQualifiedBasis?: number;

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
 * 创建InspectionStandardItem DTO
 * 对应前端 InspectionStandardItemCreate
 * @description 对应后端 TaktInspectionStandardItemCreateDto
 */
export interface InspectionStandardItemCreate {
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
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 检验项目编码
   */
  itemCode: string;

  /**
   * 检验项目名称
   */
  itemName: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode: number;

  /**
   * 检验标准值
   */
  standardValue: string;

  /**
   * 检验上限值
   */
  upperLimit: string;

  /**
   * 检验下限值
   */
  lowerLimit: string;

  /**
   * 检验工具
   */
  inspectionTool: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription: string;

  /**
   * 接收标准（AC值）
   */
  acceptanceCriteria: string;

  /**
   * 拒收标准（RE值）
   */
  rejectionCriteria: string;

  /**
   * 是否合格判定项目（0=否，1=是）
   */
  isQualifiedBasis: number;

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
 * 更新InspectionStandardItem DTO
 * 继承 TaktInspectionStandardItemCreateDto，添加 InspectionStandardItemId 字段
 * 对应前端 InspectionStandardItemUpdate
 * @description 对应后端 TaktInspectionStandardItemUpdateDto
 */
export interface InspectionStandardItemUpdate extends InspectionStandardItemCreate {
  /**
   * InspectionStandardItemID（标识要更新的实体）
   */
  inspectionStandardItemId: string;

}


/**
 * InspectionStandardItem 导入模板行 DTO
 * 对应前端 InspectionStandardItemTemplate
 * @description 对应后端 TaktInspectionStandardItemTemplateDto
 */
export interface InspectionStandardItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 检验项目编码
   */
  itemCode?: string;

  /**
   * 检验项目名称
   */
  itemName?: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType?: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode?: number;

  /**
   * 检验标准值
   */
  standardValue?: string;

  /**
   * 检验上限值
   */
  upperLimit?: string;

  /**
   * 检验下限值
   */
  lowerLimit?: string;

  /**
   * 检验工具
   */
  inspectionTool?: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription?: string;

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
 * InspectionStandardItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 InspectionStandardItemImport
 * @description 对应后端 TaktInspectionStandardItemImportDto
 */
export interface InspectionStandardItemImport {
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
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 检验项目编码
   */
  itemCode?: string;

  /**
   * 检验项目名称
   */
  itemName?: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType?: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel?: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode?: number;

  /**
   * 检验标准值
   */
  standardValue?: string;

  /**
   * 检验上限值
   */
  upperLimit?: string;

  /**
   * 检验下限值
   */
  lowerLimit?: string;

  /**
   * 检验工具
   */
  inspectionTool?: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription?: string;

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
 * InspectionStandardItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 InspectionStandardItemExport
 * @description 对应后端 TaktInspectionStandardItemExportDto
 */
export interface InspectionStandardItemExport {
  /**
   * InspectionStandardItemID
   */
  inspectionStandardItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 检验标准ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inspectionStandardId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 检验项目编码
   */
  itemCode: string;

  /**
   * 检验项目名称
   */
  itemName: string;

  /**
   * 检验项目类型（0=外观，1=尺寸，2=性能，3=材质，4=功能，5=颜色，6=结构）
   */
  itemType: number;

  /**
   * 缺点等级（CR=严重，MA=主要，MI=次要）
   */
  defectLevel: string;

  /**
   * 检验方式（1=计数，2=计量）
   */
  inspectionMode: number;

  /**
   * 检验标准值
   */
  standardValue: string;

  /**
   * 检验上限值
   */
  upperLimit: string;

  /**
   * 检验下限值
   */
  lowerLimit: string;

  /**
   * 检验工具
   */
  inspectionTool: string;

  /**
   * 检验方法说明
   */
  inspectionMethodDescription: string;

  /**
   * 接收标准（AC值）
   */
  acceptanceCriteria: string;

  /**
   * 拒收标准（RE值）
   */
  rejectionCriteria: string;

  /**
   * 是否合格判定项目（0=否，1=是）
   */
  isQualifiedBasis: number;

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

