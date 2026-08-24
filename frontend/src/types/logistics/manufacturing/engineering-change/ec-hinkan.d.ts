// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-hinkan.d.ts
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
 * 设变品管课（D0820）部门执行表
 * 对应前端 TaktEcHinkanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcHinkan
 * @description 对应后端 TaktEcHinkanDto
 */
export interface EcHinkan extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcHinkan 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0820）
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
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingCode?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * EcHinkan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcHinkanExport
 * @description 对应后端 TaktEcHinkanExportDto
 */
export interface EcHinkanExport {
  /**
   * EcHinkanID
   */
  ecHinkanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcHinkan 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0820）
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
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingCode?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

