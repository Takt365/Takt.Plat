// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：numbering.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 编号规则实体 定义系统中各类业务单据的编号生成规则，如：订单号、合同号、发票号等 支持灵活的前缀、日期格式、流水号组合 编码顺序：单据类型-公司-部门-前缀-日期-流水号 示例：order-1000-DEPT01-SO-20250120-000001
 * 对应前端 TaktNumberingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Numbering
 * @description 对应后端 TaktNumberingDto
 */
export interface Numbering extends CompanyDtoBase {
  /**
   * NumberingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  numberingId: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型
   */
  documentType: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

  /**
   * 分隔符（默认 -，也可用 _ 或 /）
   */
  separator: string;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  status: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
   */
  description?: string;

}


/**
 * Numbering 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NumberingQuery
 * @description 对应后端 TaktNumberingQueryDto
 */
export interface NumberingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型
   */
  documentType?: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode?: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod?: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

  /**
   * 分隔符（默认 -，也可用 _ 或 /）
   */
  separator?: string;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  status?: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
   */
  description?: string;

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
 * 创建Numbering DTO
 * 对应前端 NumberingCreate
 * @description 对应后端 TaktNumberingCreateDto
 */
export interface NumberingCreate {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型
   */
  documentType: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

  /**
   * 分隔符（默认 -，也可用 _ 或 /）
   */
  separator: string;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  status: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
   */
  description?: string;

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
 * 更新Numbering DTO
 * 继承 TaktNumberingCreateDto，添加 NumberingId 字段
 * 对应前端 NumberingUpdate
 * @description 对应后端 TaktNumberingUpdateDto
 */
export interface NumberingUpdate extends NumberingCreate {
  /**
   * NumberingID（标识要更新的实体）
   */
  numberingId: string;

}


/**
 * Numbering 状态更新 DTO
 * 对应前端 NumberingStatus
 * @description 对应后端 TaktNumberingStatusDto
 */
export interface NumberingStatus {
  /**
   * NumberingID
   */
  numberingId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  status: number;

}


/**
 * Numbering 导入模板行 DTO
 * 对应前端 NumberingTemplate
 * @description 对应后端 TaktNumberingTemplateDto
 */
export interface NumberingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型
   */
  documentType?: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode?: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod?: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

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
 * Numbering 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NumberingImport
 * @description 对应后端 TaktNumberingImportDto
 */
export interface NumberingImport {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型
   */
  documentType?: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode?: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod?: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

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
 * Numbering 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NumberingExport
 * @description 对应后端 TaktNumberingExportDto
 */
export interface NumberingExport {
  /**
   * NumberingID
   */
  numberingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型
   */
  documentType: number;

  /**
   * 部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode
   */
  departmentCode: string;

  /**
   * 前缀（如：SO-, PO-, INV-）
   */
  prefix?: string;

  /**
   * 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期
   */
  dateFormat?: string;

  /**
   * 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水号步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀（如：-CN, -USD, -V2）
   */
  suffix?: string;

  /**
   * 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
   */
  resetPeriod: string;

  /**
   * 当前流水号（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001
   */
  exampleCode?: string;

  /**
   * 分隔符（默认 -，也可用 _ 或 /）
   */
  separator: string;

  /**
   * 是否内置（0=否，1=是，系统内置的不可删除）
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  status: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
   */
  description?: string;

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

