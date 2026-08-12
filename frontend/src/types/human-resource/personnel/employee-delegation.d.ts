// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-delegation.d.ts
// 创建时间：2026-06-23
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
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

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
   * 代理开始时间
   */
  startDate?: string;

  /**
   * 代理结束时间 null = 长期有效，直到手动删除
   */
  endDate?: string;

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

