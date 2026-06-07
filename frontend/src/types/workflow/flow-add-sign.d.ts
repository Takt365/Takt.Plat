// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/workflow
// 文件名称：flow-add-sign.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 流程加签记录实体
 * 对应前端 TaktFlowAddSignDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowAddSign
 * @description 对应后端 TaktFlowAddSignDto
 */
export interface FlowAddSign extends CompanyDtoBase {
  /**
   * FlowAddSignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  flowAddSignId: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 流程实例 名称（填充字段）
   */
  instanceName?: string;

  /**
   * 加签节点 ID
   */
  nodeId: string;

  /**
   * 加签节点 名称（填充字段）
   */
  nodeName?: string;

  /**
   * 加签人 ID
   */
  signUserId: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled: number;

  /**
   * 所属流程实例 （主表：TaktFlowInstance）
   */
  instance?: FlowInstance;

}


/**
 * FlowAddSign 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FlowAddSignQuery
 * @description 对应后端 TaktFlowAddSignQueryDto
 */
export interface FlowAddSignQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 加签节点 ID
   */
  nodeId?: string;

  /**
   * 加签人 ID
   */
  signUserId?: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType?: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode?: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled?: number;

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
 * 创建FlowAddSign DTO
 * 对应前端 FlowAddSignCreate
 * @description 对应后端 TaktFlowAddSignCreateDto
 */
export interface FlowAddSignCreate {
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
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 加签节点 ID
   */
  nodeId: string;

  /**
   * 加签人 ID
   */
  signUserId: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled: number;

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
 * 更新FlowAddSign DTO
 * 继承 TaktFlowAddSignCreateDto，添加 FlowAddSignId 字段
 * 对应前端 FlowAddSignUpdate
 * @description 对应后端 TaktFlowAddSignUpdateDto
 */
export interface FlowAddSignUpdate extends FlowAddSignCreate {
  /**
   * FlowAddSignID（标识要更新的实体）
   */
  flowAddSignId: string;

}


/**
 * FlowAddSign 导入模板行 DTO
 * 对应前端 FlowAddSignTemplate
 * @description 对应后端 TaktFlowAddSignTemplateDto
 */
export interface FlowAddSignTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 加签节点 ID
   */
  nodeId?: string;

  /**
   * 加签人 ID
   */
  signUserId?: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType?: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode?: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled?: number;

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
 * FlowAddSign 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FlowAddSignImport
 * @description 对应后端 TaktFlowAddSignImportDto
 */
export interface FlowAddSignImport {
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
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 加签节点 ID
   */
  nodeId?: string;

  /**
   * 加签人 ID
   */
  signUserId?: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType?: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode?: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled?: number;

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
 * FlowAddSign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowAddSignExport
 * @description 对应后端 TaktFlowAddSignExportDto
 */
export interface FlowAddSignExport {
  /**
   * FlowAddSignID
   */
  flowAddSignId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 加签节点 ID
   */
  nodeId: string;

  /**
   * 加签人 ID
   */
  signUserId: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled: number;

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

