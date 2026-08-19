// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-document.d.ts
// 创建时间：2026-08-10
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt物料凭证主表实体（公司级）
 * 对应前端 TaktMaterialDocumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialDocument
 * @description 对应后端 TaktMaterialDocumentDto
 */
export interface MaterialDocument extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 物料凭证
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear?: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 用户名（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * MaterialDocument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDocumentExport
 * @description 对应后端 TaktMaterialDocumentExportDto
 */
export interface MaterialDocumentExport {
  /**
   * MaterialDocumentID
   */
  materialDocumentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 物料凭证
   */
  materialDocumentCode: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 用户名（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  postedBy?: string;

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

