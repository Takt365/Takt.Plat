// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-delegation.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工代理关系实体 独立记录所有代理场景（部门代理、岗位代理、审批代理等） 参考 SAP HR 设计： - Infotype 0001 (组织分配) 中的代理字段 - T77UA 代理表 - SWAC 工作流代理模块
 * 对应前端 TaktEmployeeDelegationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeDelegation
 * @description 对应后端 TaktEmployeeDelegationDto
 */
export interface EmployeeDelegation extends CompanyDtoBase {
  /**
   * EmployeeDelegationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeDelegationId: string;

  /**
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId: string;

  /**
   * 代理人名称（填充字段）
   */
  proxyEmployeeName?: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId: string;

  /**
   * 被代理人名称（填充字段）
   */
  originalEmployeeName?: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理范围名称（填充字段）
   */
  scopeName?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除
   */
  endDate?: string;

}


/**
 * EmployeeDelegation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeDelegationQuery
 * @description 对应后端 TaktEmployeeDelegationQueryDto
 */
export interface EmployeeDelegationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId?: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId?: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType?: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType?: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason?: string;

  /**
   * 代理开始时间（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 代理开始时间（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除（范围查询-结束）
   */
  endDateEnd?: string;

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
 * 创建EmployeeDelegation DTO
 * 对应前端 EmployeeDelegationCreate
 * @description 对应后端 TaktEmployeeDelegationCreateDto
 */
export interface EmployeeDelegationCreate {
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
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除
   */
  endDate?: string;

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
 * 更新EmployeeDelegation DTO
 * 继承 TaktEmployeeDelegationCreateDto，添加 EmployeeDelegationId 字段
 * 对应前端 EmployeeDelegationUpdate
 * @description 对应后端 TaktEmployeeDelegationUpdateDto
 */
export interface EmployeeDelegationUpdate extends EmployeeDelegationCreate {
  /**
   * EmployeeDelegationID（标识要更新的实体）
   */
  employeeDelegationId: string;

}


/**
 * EmployeeDelegation 导入模板行 DTO
 * 对应前端 EmployeeDelegationTemplate
 * @description 对应后端 TaktEmployeeDelegationTemplateDto
 */
export interface EmployeeDelegationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId?: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId?: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType?: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType?: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason?: string;

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
 * EmployeeDelegation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeDelegationImport
 * @description 对应后端 TaktEmployeeDelegationImportDto
 */
export interface EmployeeDelegationImport {
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
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId?: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId?: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType?: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType?: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason?: string;

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
 * EmployeeDelegation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeDelegationExport
 * @description 对应后端 TaktEmployeeDelegationExportDto
 */
export interface EmployeeDelegationExport {
  /**
   * EmployeeDelegationID
   */
  employeeDelegationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 代理人ID（代替别人处理工作的人）
   */
  proxyEmployeeId: string;

  /**
   * 被代理人ID（需要别人代替的人）
   */
  originalEmployeeId: string;

  /**
   * 代理类型 1 = 完全代理（代理人拥有被代理人的所有权限） 2 = 部分代理（仅代理特定部门/岗位的权限） 3 = 审批代理（仅代理审批流程）
   */
  delegationType: number;

  /**
   * 代理范围类型 1 = 部门级别（代理被代理人在特定部门的所有权限） 2 = 岗位级别（代理被代理人在特定岗位的所有权限） 3 = 全局代理（代理被代理人的所有权限） 4 = 特定业务（仅代理特定业务流程）
   */
  scopeType: number;

  /**
   * 代理范围ID 当 ScopeType=1 时，表示部门ID 当 ScopeType=2 时，表示岗位ID 当 ScopeType=4 时，表示业务ID（如：工作流定义ID）
   */
  scopeId?: string;

  /**
   * 代理原因 如：休假、出差、培训、岗位空缺、病假等
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除
   */
  endDate?: string;

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

