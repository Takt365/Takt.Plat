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
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode?: string;

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
  ecCode: string;

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

