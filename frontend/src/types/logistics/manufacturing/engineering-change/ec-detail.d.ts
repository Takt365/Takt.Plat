// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-detail.d.ts
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
 * 设变明细实体（技术阶段一 ③，隶属 TaktEcGijutsu）。技术维护 BOM/料号变更行；存在明细时保存主表后系统自动生成 TaktEcNotification， 阶段二各部门在 TaktEcSeikan/Mp 等表按明细行（EcnDetailId）填报执行，本实体通过 OneToOne 导航直接关联各课部门执行表。
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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck: number;

  /**
   * BOM生效日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 设变技术课主表（多对一） （主表：TaktEcGijutsu）
   */
  ecGijutsu?: EcGijutsu;

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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel?: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine?: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement?: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck?: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement?: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck?: number;

  /**
   * BOM生效日期（Ec_bomdate）（范围查询-开始）
   */
  ecBomDateStart?: string;

  /**
   * BOM生效日期（Ec_bomdate）（范围查询-结束）
   */
  ecBomDateEnd?: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

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
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck: number;

  /**
   * BOM生效日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

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
 * EcDetail 作废/撤销作废 DTO
 * 对应前端 EcDetailObsolete
 * @description 对应后端 TaktEcDetailObsoleteDto
 */
export interface EcDetailObsolete {
  /**
   * EcDetailID
   */
  ecDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel?: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine?: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement?: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck?: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement?: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck?: number;

  /**
   * BOM生效日期（Ec_bomdate）
   */
  ecBomDate?: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

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
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel?: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine?: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement?: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck?: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement?: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck?: number;

  /**
   * BOM生效日期（Ec_bomdate）
   */
  ecBomDate?: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

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
   * BOM行号（Ec_bom_line_no）
   */
  ecBomLineNo?: string;

  /**
   * 机种（Ec_model）
   */
  ecModel: string;

  /**
   * 完成品（Ec_bomitem）
   */
  ecBomItem?: string;

  /**
   * 完成品描述（Ec_bomitemtext）
   */
  ecBomItemText?: string;

  /**
   * 上阶物料（Ec_bomsubitem）
   */
  ecBomSubItem?: string;

  /**
   * 上阶物料描述（Ec_bomsubitemtext）
   */
  ecBomSubItemText?: string;

  /**
   * 完成品EOL（End of Line，0=否 1=是）
   */
  isEndOfLine: number;

  /**
   * 旧料号（Ec_olditem）
   */
  ecOldItem?: string;

  /**
   * 旧料号描述（Ec_oldtext）
   */
  ecOldText?: string;

  /**
   * 旧用量（Ec_oldusage）
   */
  ecOldUsage?: number;

  /**
   * 旧位置（Ec_oldposition）
   */
  ecOldPosition?: string;

  /**
   * 旧在库数量（Ec_oldstock）
   */
  ecOldStock?: number;

  /**
   * 旧品仓库（Ec_oldwarehouse）
   */
  ecOldWarehouse?: string;

  /**
   * 旧品是否采购（0=否 1=是）
   */
  isOldProcurement: number;

  /**
   * 旧品是否检查（0=否 1=是）
   */
  isOldCheck: number;

  /**
   * 新料号（Ec_newitem）
   */
  ecNewItem?: string;

  /**
   * 新料号描述（Ec_newtext）
   */
  ecNewText?: string;

  /**
   * 新用量（Ec_newusage）
   */
  ecNewUsage?: number;

  /**
   * 新位置（Ec_newposition）
   */
  ecNewPosition?: string;

  /**
   * 新在库数量（Ec_newstock）
   */
  ecNewStock?: number;

  /**
   * 新品仓库（Ec_newwarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品是否采购（0=否 1=是）
   */
  isNewProcurement: number;

  /**
   * 新品是否检查（0=否 1=是）
   */
  isNewCheck: number;

  /**
   * BOM生效日期（Ec_bomdate）
   */
  ecBomDate: string;

  /**
   * 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
   */
  ecIsCompatible?: string;

  /**
   * 二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
   */
  ecSecondDistinction?: string;

  /**
   * 生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecLegacyPartDisposition?: string;

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

