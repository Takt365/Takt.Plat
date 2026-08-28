// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-koubai.d.ts
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
 * 设变采购课（D0510）部门执行表
 * 对应前端 TaktEcKoubaiDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcKoubai
 * @description 对应后端 TaktEcKoubaiDto
 */
export interface EcKoubai extends CompanyDtoBase {
  /**
   * EcKoubaiID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecKoubaiId: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcKoubai 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcKoubaiQuery
 * @description 对应后端 TaktEcKoubaiQueryDto
 */
export interface EcKoubaiQuery extends TaktPagedQuery {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期（范围查询-开始）
   */
  purchaseOrderIssueDateStart?: string;

  /**
   * 采购订单发行日期（范围查询-结束）
   */
  purchaseOrderIssueDateEnd?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

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

}


/**
 * 创建EcKoubai DTO
 * 对应前端 EcKoubaiCreate
 * @description 对应后端 TaktEcKoubaiCreateDto
 */
export interface EcKoubaiCreate {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

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
 * 更新EcKoubai DTO
 * 继承 TaktEcKoubaiCreateDto，添加 EcKoubaiId 字段
 * 对应前端 EcKoubaiUpdate
 * @description 对应后端 TaktEcKoubaiUpdateDto
 */
export interface EcKoubaiUpdate extends EcKoubaiCreate {
  /**
   * EcKoubaiID（标识要更新的实体）
   */
  ecKoubaiId: string;

}


/**
 * EcKoubai 作废/撤销作废 DTO
 * 对应前端 EcKoubaiObsolete
 * @description 对应后端 TaktEcKoubaiObsoleteDto
 */
export interface EcKoubaiObsolete {
  /**
   * EcKoubaiID
   */
  ecKoubaiId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcKoubai 导入模板行 DTO
 * 对应前端 EcKoubaiTemplate
 * @description 对应后端 TaktEcKoubaiTemplateDto
 */
export interface EcKoubaiTemplate {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

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
 * EcKoubai 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcKoubaiImport
 * @description 对应后端 TaktEcKoubaiImportDto
 */
export interface EcKoubaiImport {
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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

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
 * EcKoubai 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcKoubaiExport
 * @description 对应后端 TaktEcKoubaiExportDto
 */
export interface EcKoubaiExport {
  /**
   * EcKoubaiID
   */
  ecKoubaiId: string;

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
   * 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
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
   * 部门编码（TaktDept.DeptCode，5 位，如 D0510）
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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

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

