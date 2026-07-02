// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：source-ec-detail.d.ts
// 创建时间：2026-06-27
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
 * 设变来源子表实体。
 * 对应前端 TaktSourceEcDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SourceEcDetail
 * @description 对应后端 TaktSourceEcDetailDto
 */
export interface SourceEcDetail extends CompanyDtoBase {
  /**
   * SourceEcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sourceEcDetailId: string;

  /**
   * 主ID
   */
  sourceEcId: string;

  /**
   * 主名称（填充字段）
   */
  sourceEcName?: string;

  /**
   * 完成品
   */
  sourceFinishedProduct: string;

  /**
   * 上阶物料
   */
  sourceParentPart: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

  /**
   * 设变来源主表 （主表：TaktSourceEc）
   */
  sourceEc?: SourceEc;

}


/**
 * SourceEcDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SourceEcDetailQuery
 * @description 对应后端 TaktSourceEcDetailQueryDto
 */
export interface SourceEcDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 主ID
   */
  sourceEcId?: string;

  /**
   * 完成品
   */
  sourceFinishedProduct?: string;

  /**
   * 上阶物料
   */
  sourceParentPart?: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期（范围查询-开始）
   */
  sourceBomEffectiveDateStart?: string;

  /**
   * BOM生效日期（范围查询-结束）
   */
  sourceBomEffectiveDateEnd?: string;

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
 * 创建SourceEcDetail DTO
 * 对应前端 SourceEcDetailCreate
 * @description 对应后端 TaktSourceEcDetailCreateDto
 */
export interface SourceEcDetailCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 主ID
   */
  sourceEcId: string;

  /**
   * 完成品
   */
  sourceFinishedProduct: string;

  /**
   * 上阶物料
   */
  sourceParentPart: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * 更新SourceEcDetail DTO
 * 继承 TaktSourceEcDetailCreateDto，添加 SourceEcDetailId 字段
 * 对应前端 SourceEcDetailUpdate
 * @description 对应后端 TaktSourceEcDetailUpdateDto
 */
export interface SourceEcDetailUpdate extends SourceEcDetailCreate {
  /**
   * SourceEcDetailID（标识要更新的实体）
   */
  sourceEcDetailId: string;

}


/**
 * SourceEcDetail 导入模板行 DTO
 * 对应前端 SourceEcDetailTemplate
 * @description 对应后端 TaktSourceEcDetailTemplateDto
 */
export interface SourceEcDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 主ID
   */
  sourceEcId?: string;

  /**
   * 完成品
   */
  sourceFinishedProduct?: string;

  /**
   * 上阶物料
   */
  sourceParentPart?: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * SourceEcDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SourceEcDetailImport
 * @description 对应后端 TaktSourceEcDetailImportDto
 */
export interface SourceEcDetailImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 主ID
   */
  sourceEcId?: string;

  /**
   * 完成品
   */
  sourceFinishedProduct?: string;

  /**
   * 上阶物料
   */
  sourceParentPart?: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * SourceEcDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SourceEcDetailExport
 * @description 对应后端 TaktSourceEcDetailExportDto
 */
export interface SourceEcDetailExport {
  /**
   * SourceEcDetailID
   */
  sourceEcDetailId: string;

  /**
   * 主ID
   */
  sourceEcId: string;

  /**
   * 完成品
   */
  sourceFinishedProduct: string;

  /**
   * 上阶物料
   */
  sourceParentPart: string;

  /**
   * 旧物料号
   */
  sourceLegacyPartNo?: string;

  /**
   * 旧物料
   */
  sourceLegacyPartName?: string;

  /**
   * 旧物料用量
   */
  sourceLegacyUsage?: number;

  /**
   * 旧物料安装位置
   */
  sourceLegacyMountingPosition?: string;

  /**
   * 新物料
   */
  sourceReplacementPartNo?: string;

  /**
   * 新物料
   */
  sourceReplacementPartName?: string;

  /**
   * 新物料用量
   */
  sourceReplacementUsage?: number;

  /**
   * 新物料安装位置
   */
  sourceReplacementMountingPosition?: string;

  /**
   * BOM番号
   */
  sourceBomNo?: string;

  /**
   * 互换性
   */
  sourceInterchangeability?: string;

  /**
   * 区分
   */
  sourceDistinction?: string;

  /**
   * 安排指示
   */
  sourceArrangementInstruction?: string;

  /**
   * 旧物料处理
   */
  sourceLegacyPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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

