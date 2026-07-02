// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perf-scheme.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 绩效方案指标（方案维度 + 指标维度合一，每行表示某方案下的一条指标）
 * 对应前端 TaktPerfSchemeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfScheme
 * @description 对应后端 TaktPerfSchemeDto
 */
export interface PerfScheme extends CompanyDtoBase {
  /**
   * PerfSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  perfSchemeId: string;

  /**
   * 方案编码
   */
  schemeCode: string;

  /**
   * 方案名称
   */
  schemeName: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight: number;

  /**
   * 指标编码
   */
  metricCode: string;

  /**
   * 指标名称
   */
  metricName: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType: string;

  /**
   * 评分标准说明
   */
  scoringCriteria: string;

  /**
   * 标准权重（%）
   */
  standardWeight: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * PerfScheme 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PerfSchemeQuery
 * @description 对应后端 TaktPerfSchemeQueryDto
 */
export interface PerfSchemeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 方案编码
   */
  schemeCode?: string;

  /**
   * 方案名称
   */
  schemeName?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard?: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight?: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight?: number;

  /**
   * 指标编码
   */
  metricCode?: string;

  /**
   * 指标名称
   */
  metricName?: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category?: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType?: string;

  /**
   * 评分标准说明
   */
  scoringCriteria?: string;

  /**
   * 标准权重（%）
   */
  standardWeight?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 创建PerfScheme DTO
 * 对应前端 PerfSchemeCreate
 * @description 对应后端 TaktPerfSchemeCreateDto
 */
export interface PerfSchemeCreate {
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
   * 方案编码
   */
  schemeCode: string;

  /**
   * 方案名称
   */
  schemeName: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight: number;

  /**
   * 指标编码
   */
  metricCode: string;

  /**
   * 指标名称
   */
  metricName: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType: string;

  /**
   * 评分标准说明
   */
  scoringCriteria: string;

  /**
   * 标准权重（%）
   */
  standardWeight: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新PerfScheme DTO
 * 继承 TaktPerfSchemeCreateDto，添加 PerfSchemeId 字段
 * 对应前端 PerfSchemeUpdate
 * @description 对应后端 TaktPerfSchemeUpdateDto
 */
export interface PerfSchemeUpdate extends PerfSchemeCreate {
  /**
   * PerfSchemeID（标识要更新的实体）
   */
  perfSchemeId: string;

}


/**
 * PerfScheme 状态更新 DTO
 * 对应前端 PerfSchemeStatus
 * @description 对应后端 TaktPerfSchemeStatusDto
 */
export interface PerfSchemeStatus {
  /**
   * PerfSchemeID
   */
  perfSchemeId: string;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus: number;

}


/**
 * PerfScheme 排序更新 DTO
 * 对应前端 PerfSchemeSort
 * @description 对应后端 TaktPerfSchemeSortDto
 */
export interface PerfSchemeSort {
  /**
   * PerfSchemeID
   */
  perfSchemeId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * PerfScheme 导入模板行 DTO
 * 对应前端 PerfSchemeTemplate
 * @description 对应后端 TaktPerfSchemeTemplateDto
 */
export interface PerfSchemeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 方案编码
   */
  schemeCode?: string;

  /**
   * 方案名称
   */
  schemeName?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard?: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight?: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight?: number;

  /**
   * 指标编码
   */
  metricCode?: string;

  /**
   * 指标名称
   */
  metricName?: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category?: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType?: string;

  /**
   * 评分标准说明
   */
  scoringCriteria?: string;

  /**
   * 标准权重（%）
   */
  standardWeight?: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * PerfScheme 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PerfSchemeImport
 * @description 对应后端 TaktPerfSchemeImportDto
 */
export interface PerfSchemeImport {
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
   * 方案编码
   */
  schemeCode?: string;

  /**
   * 方案名称
   */
  schemeName?: string;

  /**
   * 适用部门
   */
  applicableDepartment?: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType?: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard?: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight?: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight?: number;

  /**
   * 指标编码
   */
  metricCode?: string;

  /**
   * 指标名称
   */
  metricName?: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category?: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType?: string;

  /**
   * 评分标准说明
   */
  scoringCriteria?: string;

  /**
   * 标准权重（%）
   */
  standardWeight?: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * PerfScheme 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfSchemeExport
 * @description 对应后端 TaktPerfSchemeExportDto
 */
export interface PerfSchemeExport {
  /**
   * PerfSchemeID
   */
  perfSchemeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 方案编码
   */
  schemeCode: string;

  /**
   * 方案名称
   */
  schemeName: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight: number;

  /**
   * 指标编码
   */
  metricCode: string;

  /**
   * 指标名称
   */
  metricName: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType: string;

  /**
   * 评分标准说明
   */
  scoringCriteria: string;

  /**
   * 标准权重（%）
   */
  standardWeight: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

