// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：inspection-standard.d.ts
// 创建时间：2026-06-06
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
 * 检验标准实体（IQC/IPQC/FQC通用）
 * 对应前端 TaktInspectionStandardDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 InspectionStandard
 * @description 对应后端 TaktInspectionStandardDto
 */
export interface InspectionStandard extends CompanyDtoBase {
  /**
   * InspectionStandardID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  inspectionStandardId: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode: string;

  /**
   * 检验标准名称
   */
  standardName: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode: string;

  /**
   * 物料类别名称
   */
  materialCategoryName: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

  /**
   * 检验标准明细列表（主子表关系） （子表：TaktInspectionStandardItem）
   */
  items?: InspectionStandardItem[];

}


/**
 * InspectionStandard 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 InspectionStandardQuery
 * @description 对应后端 TaktInspectionStandardQueryDto
 */
export interface InspectionStandardQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode?: string;

  /**
   * 检验标准名称
   */
  standardName?: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType?: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode?: string;

  /**
   * 物料类别名称
   */
  materialCategoryName?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled?: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus?: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

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
 * 创建InspectionStandard DTO
 * 对应前端 InspectionStandardCreate
 * @description 对应后端 TaktInspectionStandardCreateDto
 */
export interface InspectionStandardCreate {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode: string;

  /**
   * 检验标准名称
   */
  standardName: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode: string;

  /**
   * 物料类别名称
   */
  materialCategoryName: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

  /**
   * 检验标准明细列表（主子表关系）（子表，级联保存）
   */
  items?: InspectionStandardItemCreate[];

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
 * 更新InspectionStandard DTO
 * 继承 TaktInspectionStandardCreateDto，添加 InspectionStandardId 字段
 * 对应前端 InspectionStandardUpdate
 * @description 对应后端 TaktInspectionStandardUpdateDto
 */
export interface InspectionStandardUpdate extends InspectionStandardCreate {
  /**
   * InspectionStandardID（标识要更新的实体）
   */
  inspectionStandardId: string;

}


/**
 * InspectionStandard 状态更新 DTO
 * 对应前端 InspectionStandardStatus
 * @description 对应后端 TaktInspectionStandardStatusDto
 */
export interface InspectionStandardStatus {
  /**
   * InspectionStandardID
   */
  inspectionStandardId: string;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus: number;

}


/**
 * InspectionStandard 导入模板行 DTO
 * 对应前端 InspectionStandardTemplate
 * @description 对应后端 TaktInspectionStandardTemplateDto
 */
export interface InspectionStandardTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode?: string;

  /**
   * 检验标准名称
   */
  standardName?: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType?: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode?: string;

  /**
   * 物料类别名称
   */
  materialCategoryName?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled?: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus?: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

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
 * InspectionStandard 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 InspectionStandardImport
 * @description 对应后端 TaktInspectionStandardImportDto
 */
export interface InspectionStandardImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode?: string;

  /**
   * 检验标准名称
   */
  standardName?: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType?: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode?: string;

  /**
   * 物料类别名称
   */
  materialCategoryName?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled?: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus?: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

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
 * InspectionStandard 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 InspectionStandardExport
 * @description 对应后端 TaktInspectionStandardExportDto
 */
export interface InspectionStandardExport {
  /**
   * InspectionStandardID
   */
  inspectionStandardId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode: string;

  /**
   * 检验标准名称
   */
  standardName: string;

  /**
   * 检验类型（0=IQC来料检验，1=IPQC过程检验，2=FQC最终检验）
   */
  inspectionType: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode: string;

  /**
   * 物料类别名称
   */
  materialCategoryName: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 是否启用（0=否，1=是）
   */
  isEnabled: number;

  /**
   * 检验标准状态（0=草稿，1=已发布，2=已停用）
   */
  standardStatus: number;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

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

