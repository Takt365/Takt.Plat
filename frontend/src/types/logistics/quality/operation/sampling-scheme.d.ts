// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：sampling-scheme.d.ts
// 创建时间：2026-06-30
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
 * Takt抽样方案实体
 * 对应前端 TaktSamplingSchemeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SamplingScheme
 * @description 对应后端 TaktSamplingSchemeDto
 */
export interface SamplingScheme extends CompanyDtoBase {
  /**
   * SamplingSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  samplingSchemeId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus: number;

}


/**
 * SamplingScheme 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SamplingSchemeQuery
 * @description 对应后端 TaktSamplingSchemeQueryDto
 */
export interface SamplingSchemeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType?: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard?: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel?: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue?: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin?: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax?: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize?: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber?: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber?: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness?: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled?: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus?: number;

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
 * 创建SamplingScheme DTO
 * 对应前端 SamplingSchemeCreate
 * @description 对应后端 TaktSamplingSchemeCreateDto
 */
export interface SamplingSchemeCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus: number;

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
 * 更新SamplingScheme DTO
 * 继承 TaktSamplingSchemeCreateDto，添加 SamplingSchemeId 字段
 * 对应前端 SamplingSchemeUpdate
 * @description 对应后端 TaktSamplingSchemeUpdateDto
 */
export interface SamplingSchemeUpdate extends SamplingSchemeCreate {
  /**
   * SamplingSchemeID（标识要更新的实体）
   */
  samplingSchemeId: string;

}


/**
 * SamplingScheme 状态更新 DTO
 * 对应前端 SamplingSchemeStatus
 * @description 对应后端 TaktSamplingSchemeStatusDto
 */
export interface SamplingSchemeStatus {
  /**
   * SamplingSchemeID
   */
  samplingSchemeId: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus: number;

}


/**
 * SamplingScheme 导入模板行 DTO
 * 对应前端 SamplingSchemeTemplate
 * @description 对应后端 TaktSamplingSchemeTemplateDto
 */
export interface SamplingSchemeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType?: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard?: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel?: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue?: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin?: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax?: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize?: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber?: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber?: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness?: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled?: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus?: number;

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
 * SamplingScheme 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SamplingSchemeImport
 * @description 对应后端 TaktSamplingSchemeImportDto
 */
export interface SamplingSchemeImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType?: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard?: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel?: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue?: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin?: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax?: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize?: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber?: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber?: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness?: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled?: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus?: number;

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
 * SamplingScheme 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SamplingSchemeExport
 * @description 对应后端 TaktSamplingSchemeExportDto
 */
export interface SamplingSchemeExport {
  /**
   * SamplingSchemeID
   */
  samplingSchemeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 抽样方案编码（唯一索引）
   */
  samplingSchemeCode: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName: string;

  /**
   * 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
   */
  samplingSchemeType: number;

  /**
   * 抽样标准（字典 logistics_quality_sampling_standard）
   */
  samplingStandard: number;

  /**
   * 检验水平（字典 logistics_quality_inspection_level）
   */
  inspectionLevel: number;

  /**
   * AQL值（可接受质量水平，0.010-1000，存储为小数）
   */
  aqlValue: number;

  /**
   * 批量范围最小值
   */
  lotSizeMin: number;

  /**
   * 批量范围最大值（0表示无上限）
   */
  lotSizeMax: number;

  /**
   * 样本量（抽样数量）
   */
  sampleSize: number;

  /**
   * 接收数（Ac，Acceptance Number）
   */
  acceptanceNumber: number;

  /**
   * 拒收数（Re，Rejection Number）
   */
  rejectionNumber: number;

  /**
   * 检验严格度（字典 logistics_quality_inspection_strictness）
   */
  inspectionStrictness: number;

  /**
   * 是否支持转移规则（0=否，1=是）
   */
  isTransferRuleEnabled: number;

  /**
   * 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
   */
  transferRuleConfig?: string;

  /**
   * 抽样方案描述
   */
  schemeDescription?: string;

  /**
   * 抽样方案状态（字典 logistics_quality_standard_status）
   */
  samplingSchemeStatus: number;

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

