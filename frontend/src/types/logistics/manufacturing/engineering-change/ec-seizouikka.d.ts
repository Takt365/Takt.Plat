// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-seizouikka.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设变制造1课（D0610）部门执行表
 * 对应前端 TaktEcSeizouikkaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcSeizouikka
 * @description 对应后端 TaktEcSeizouikkaDto
 */
export interface EcSeizouikka extends CompanyDtoBase {
  /**
   * EcSeizouikkaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecSeizouikkaId: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId: string;

  /**
   * 设变明细 名称（填充字段）
   */
  ecnDetailName?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizouikka 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcSeizouikkaQuery
 * @description 对应后端 TaktEcSeizouikkaQueryDto
 */
export interface EcSeizouikkaQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建EcSeizouikka DTO
 * 对应前端 EcSeizouikkaCreate
 * @description 对应后端 TaktEcSeizouikkaCreateDto
 */
export interface EcSeizouikkaCreate {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新EcSeizouikka DTO
 * 继承 TaktEcSeizouikkaCreateDto，添加 EcSeizouikkaId 字段
 * 对应前端 EcSeizouikkaUpdate
 * @description 对应后端 TaktEcSeizouikkaUpdateDto
 */
export interface EcSeizouikkaUpdate extends EcSeizouikkaCreate {
  /**
   * EcSeizouikkaID（标识要更新的实体）
   */
  ecSeizouikkaId: string;

}


/**
 * EcSeizouikka 作废/撤销作废 DTO
 * 对应前端 EcSeizouikkaObsolete
 * @description 对应后端 TaktEcSeizouikkaObsoleteDto
 */
export interface EcSeizouikkaObsolete {
  /**
   * EcSeizouikkaID
   */
  ecSeizouikkaId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizouikka 导入模板行 DTO
 * 对应前端 EcSeizouikkaTemplate
 * @description 对应后端 TaktEcSeizouikkaTemplateDto
 */
export interface EcSeizouikkaTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * EcSeizouikka 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcSeizouikkaImport
 * @description 对应后端 TaktEcSeizouikkaImportDto
 */
export interface EcSeizouikkaImport {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * EcSeizouikka 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcSeizouikkaExport
 * @description 对应后端 TaktEcSeizouikkaExportDto
 */
export interface EcSeizouikkaExport {
  /**
   * EcSeizouikkaID
   */
  ecSeizouikkaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizouikka 导航）
   */
  ecnDetailId: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

