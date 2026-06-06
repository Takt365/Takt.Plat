// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-detail.d.ts
// 创建时间：2026-06-06
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
 * 设变（ECN）子表实体
 * 对应前端 TaktEcDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcDetail
 * @description 对应后端 TaktEcDetailDto
 */
export interface EcDetail extends CompanyDtoBase {
  /**
   * EcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecDetailId: string;

  /**
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变主表名称（填充字段）
   */
  ecName?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 型号（Ec_model）
   */
  ecModel: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * BOM 日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 录入日期（Ec_entrydate）
   */
  ecEntryDate: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧数量（Ec_oldqty）
   */
  ecOldQty?: number;

  /**
   * 旧单位/设置（Ec_oldset）
   */
  ecOldSet?: string;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新数量（Ec_newqty）
   */
  ecNewQty?: number;

  /**
   * 新单位/设置（Ec_newset）
   */
  ecNewSet?: string;

  /**
   * 是否采购（0=否 1=是）
   */
  isProcurement: number;

  /**
   * 是否检查（0=否 1=是）
   */
  isCheck: number;

  /**
   * 仓库（Ec_warehouse）
   */
  ecWarehouse?: string;

  /**
   * EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

  /**
   * 设变主表 （主表：TaktEc）
   */
  ec?: Ec;

  /**
   * 设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng） （子表：TaktEcDept）
   */
  deptRecords?: EcDept[];

}


/**
 * EcDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcDetailQuery
 * @description 对应后端 TaktEcDetailQueryDto
 */
export interface EcDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 型号（Ec_model）
   */
  ecModel?: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * BOM 日期（Ec_bomdate）（范围查询-开始）
   */
  ecBomDateStart?: string;

  /**
   * BOM 日期（Ec_bomdate）（范围查询-结束）
   */
  ecBomDateEnd?: string;

  /**
   * 录入日期（Ec_entrydate）（范围查询-开始）
   */
  ecEntryDateStart?: string;

  /**
   * 录入日期（Ec_entrydate）（范围查询-结束）
   */
  ecEntryDateEnd?: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧数量（Ec_oldqty）
   */
  ecOldQty?: number;

  /**
   * 旧单位/设置（Ec_oldset）
   */
  ecOldSet?: string;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新数量（Ec_newqty）
   */
  ecNewQty?: number;

  /**
   * 新单位/设置（Ec_newset）
   */
  ecNewSet?: string;

  /**
   * 是否采购（0=否 1=是）
   */
  isProcurement?: number;

  /**
   * 是否检查（0=否 1=是）
   */
  isCheck?: number;

  /**
   * 仓库（Ec_warehouse）
   */
  ecWarehouse?: string;

  /**
   * EOL（End of Line，0=否 1=是）
   */
  isEndOfLine?: number;

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
 * 创建EcDetail DTO
 * 对应前端 EcDetailCreate
 * @description 对应后端 TaktEcDetailCreateDto
 */
export interface EcDetailCreate {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 型号（Ec_model）
   */
  ecModel: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * BOM 日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 录入日期（Ec_entrydate）
   */
  ecEntryDate: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧数量（Ec_oldqty）
   */
  ecOldQty?: number;

  /**
   * 旧单位/设置（Ec_oldset）
   */
  ecOldSet?: string;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新数量（Ec_newqty）
   */
  ecNewQty?: number;

  /**
   * 新单位/设置（Ec_newset）
   */
  ecNewSet?: string;

  /**
   * 是否采购（0=否 1=是）
   */
  isProcurement: number;

  /**
   * 是否检查（0=否 1=是）
   */
  isCheck: number;

  /**
   * 仓库（Ec_warehouse）
   */
  ecWarehouse?: string;

  /**
   * EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

  /**
   * 设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）（子表，级联保存）
   */
  deptRecords?: EcDeptCreate[];

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
 * 更新EcDetail DTO
 * 继承 TaktEcDetailCreateDto，添加 EcDetailId 字段
 * 对应前端 EcDetailUpdate
 * @description 对应后端 TaktEcDetailUpdateDto
 */
export interface EcDetailUpdate extends EcDetailCreate {
  /**
   * EcDetailID（标识要更新的实体）
   */
  ecDetailId: string;

}


/**
 * EcDetail 导入模板行 DTO
 * 对应前端 EcDetailTemplate
 * @description 对应后端 TaktEcDetailTemplateDto
 */
export interface EcDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 型号（Ec_model）
   */
  ecModel?: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

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
 * EcDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcDetailImport
 * @description 对应后端 TaktEcDetailImportDto
 */
export interface EcDetailImport {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 型号（Ec_model）
   */
  ecModel?: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

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
 * EcDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcDetailExport
 * @description 对应后端 TaktEcDetailExportDto
 */
export interface EcDetailExport {
  /**
   * EcDetailID
   */
  ecDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 型号（Ec_model）
   */
  ecModel: string;

  /**
   * BOM 主项料号（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * BOM 子项料号（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * BOM 编号（Ec_bomno）
   */
  ecBomNo?: string;

  /**
   * 变更内容（Ec_change）
   */
  ecChange?: string;

  /**
   * 本地/现场（Ec_local）
   */
  ecLocal?: string;

  /**
   * 备注（Ec_note）
   */
  ecNote?: string;

  /**
   * 工序（Ec_process）
   */
  ecProcess?: string;

  /**
   * BOM 日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 录入日期（Ec_entrydate）
   */
  ecEntryDate: string;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧数量（Ec_oldqty）
   */
  ecOldQty?: number;

  /**
   * 旧单位/设置（Ec_oldset）
   */
  ecOldSet?: string;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新数量（Ec_newqty）
   */
  ecNewQty?: number;

  /**
   * 新单位/设置（Ec_newset）
   */
  ecNewSet?: string;

  /**
   * 是否采购（0=否 1=是）
   */
  isProcurement: number;

  /**
   * 是否检查（0=否 1=是）
   */
  isCheck: number;

  /**
   * 仓库（Ec_warehouse）
   */
  ecWarehouse?: string;

  /**
   * EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

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

