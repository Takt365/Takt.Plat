// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-seizounika.d.ts
// 创建时间：2026-08-26
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
 * 设变制造2课-间接（D0626）部门执行表
 * 对应前端 TaktEcSeizounikaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcSeizounika
 * @description 对应后端 TaktEcSeizounikaDto
 */
export interface EcSeizounika extends CompanyDtoBase {
  /**
   * EcSeizounikaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecSeizounikaId: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
   */
  ecnDetailId: string;

  /**
   * 设变明细 名称（填充字段）
   */
  ecnDetailName?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizounika 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcSeizounikaQuery
 * @description 对应后端 TaktEcSeizounikaQueryDto
 */
export interface EcSeizounikaQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
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
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode?: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

  /**
   * 制二课页签（1=采购 F 且仓库 C003 2=其它；有值时按页签过滤明细）
   */
  pcbaTab?: number;

}


/**
 * 创建EcSeizounika DTO
 * 对应前端 EcSeizounikaCreate
 * @description 对应后端 TaktEcSeizounikaCreateDto
 */
export interface EcSeizounikaCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
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
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * 更新EcSeizounika DTO
 * 继承 TaktEcSeizounikaCreateDto，添加 EcSeizounikaId 字段
 * 对应前端 EcSeizounikaUpdate
 * @description 对应后端 TaktEcSeizounikaUpdateDto
 */
export interface EcSeizounikaUpdate extends EcSeizounikaCreate {
  /**
   * EcSeizounikaID（标识要更新的实体）
   */
  ecSeizounikaId: string;

}


/**
 * EcSeizounika 作废/撤销作废 DTO
 * 对应前端 EcSeizounikaObsolete
 * @description 对应后端 TaktEcSeizounikaObsoleteDto
 */
export interface EcSeizounikaObsolete {
  /**
   * EcSeizounikaID
   */
  ecSeizounikaId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizounika 导入模板行 DTO
 * 对应前端 EcSeizounikaTemplate
 * @description 对应后端 TaktEcSeizounikaTemplateDto
 */
export interface EcSeizounikaTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
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
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode?: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * EcSeizounika 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcSeizounikaImport
 * @description 对应后端 TaktEcSeizounikaImportDto
 */
export interface EcSeizounikaImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
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
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode?: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * EcSeizounika 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcSeizounikaExport
 * @description 对应后端 TaktEcSeizounikaExportDto
 */
export interface EcSeizounikaExport {
  /**
   * EcSeizounikaID
   */
  ecSeizounikaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）
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
   * 机种（冗余：来自 TaktEcDetail.EcModelCode）
   */
  ecModelCode: string;

  /**
   * 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
   */
  ecFinishedGoods?: string;

  /**
   * 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
   */
  ecFinishedGoodsDescription?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 完成品物料状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0626）
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
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 出库单号
   */
  outboundOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

