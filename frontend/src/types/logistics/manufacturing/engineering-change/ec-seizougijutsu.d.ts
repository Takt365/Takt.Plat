// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-seizougijutsu.d.ts
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
 * 设变制造技术课（D0630）部门执行表
 * 对应前端 TaktEcSeizougijutsuDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcSeizougijutsu
 * @description 对应后端 TaktEcSeizougijutsuDto
 */
export interface EcSeizougijutsu extends CompanyDtoBase {
  /**
   * EcSeizougijutsuID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecSeizougijutsuId: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期
   */
  confirmationDate?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizougijutsu 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcSeizougijutsuQuery
 * @description 对应后端 TaktEcSeizougijutsuQueryDto
 */
export interface EcSeizougijutsuQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期（范围查询-开始）
   */
  confirmationDateStart?: string;

  /**
   * 确认日期（范围查询-结束）
   */
  confirmationDateEnd?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated?: number;

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
 * 创建EcSeizougijutsu DTO
 * 对应前端 EcSeizougijutsuCreate
 * @description 对应后端 TaktEcSeizougijutsuCreateDto
 */
export interface EcSeizougijutsuCreate {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期
   */
  confirmationDate?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated: number;

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
 * 更新EcSeizougijutsu DTO
 * 继承 TaktEcSeizougijutsuCreateDto，添加 EcSeizougijutsuId 字段
 * 对应前端 EcSeizougijutsuUpdate
 * @description 对应后端 TaktEcSeizougijutsuUpdateDto
 */
export interface EcSeizougijutsuUpdate extends EcSeizougijutsuCreate {
  /**
   * EcSeizougijutsuID（标识要更新的实体）
   */
  ecSeizougijutsuId: string;

}


/**
 * EcSeizougijutsu 作废/撤销作废 DTO
 * 对应前端 EcSeizougijutsuObsolete
 * @description 对应后端 TaktEcSeizougijutsuObsoleteDto
 */
export interface EcSeizougijutsuObsolete {
  /**
   * EcSeizougijutsuID
   */
  ecSeizougijutsuId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizougijutsu 导入模板行 DTO
 * 对应前端 EcSeizougijutsuTemplate
 * @description 对应后端 TaktEcSeizougijutsuTemplateDto
 */
export interface EcSeizougijutsuTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期
   */
  confirmationDate?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated?: number;

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
 * EcSeizougijutsu 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcSeizougijutsuImport
 * @description 对应后端 TaktEcSeizougijutsuImportDto
 */
export interface EcSeizougijutsuImport {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期
   */
  confirmationDate?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated?: number;

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
 * EcSeizougijutsu 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcSeizougijutsuExport
 * @description 对应后端 TaktEcSeizougijutsuExportDto
 */
export interface EcSeizougijutsuExport {
  /**
   * EcSeizougijutsuID
   */
  ecSeizougijutsuId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0630）
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
   * 确认日期
   */
  confirmationDate?: string;

  /**
   * 是否更新 SOP（0=否 1=是，字典 sys_yes_no）
   */
  isSopUpdated: number;

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

