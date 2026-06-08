// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：numbering-generator.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 编号预览请求 DTO（规则 Id、规则编码或草稿字段）
 * 对应前端 NumberingPreviewRequest
 * @description 对应后端 TaktNumberingPreviewRequestDto
 */
export interface NumberingPreviewRequest {
  /**
   * 编号规则 Id（优先）
   */
  numberingId: string;

  /**
   * 规则编码
   */
  ruleCode?: string;

  /**
   * 规则名称（草稿预览）
   */
  ruleName?: string;

  /**
   * 单据类型（草稿预览）
   */
  documentType: number;

  /**
   * 部门编码（草稿预览必填）
   */
  departmentCode?: string;

  /**
   * 前缀
   */
  prefix?: string;

  /**
   * 日期格式
   */
  dateFormat?: string;

  /**
   * 流水号位数
   */
  sequenceLength: number;

  /**
   * 流水号步长
   */
  sequenceStep: number;

  /**
   * 后缀
   */
  suffix?: string;

  /**
   * 重置周期
   */
  resetPeriod?: string;

  /**
   * 当前流水号（草稿预览）
   */
  currentSequence: number;

  /**
   * 分隔符
   */
  separator?: string;

  /**
   * 覆盖预览流水号（不传则按规则推算下一号）
   */
  sequenceOverride?: number;

}


/**
 * 编号预览结果 DTO
 * 对应前端 NumberingPreviewResult
 * @description 对应后端 TaktNumberingPreviewResultDto
 */
export interface NumberingPreviewResult {
  /**
   * 预览业务编号
   */
  businessCode: string;

  /**
   * 预览所用流水号
   */
  nextSequence: number;

  /**
   * 规则编码
   */
  ruleCode: string;

}


/**
 * 编号生成请求 DTO
 * 对应前端 NumberingGenerateRequest
 * @description 对应后端 TaktNumberingGenerateRequestDto
 */
export interface NumberingGenerateRequest {
  /**
   * 规则编码
   */
  ruleCode: string;

}


/**
 * 编号生成结果 DTO
 * 对应前端 NumberingGenerateResult
 * @description 对应后端 TaktNumberingGenerateResultDto
 */
export interface NumberingGenerateResult {
  /**
   * 业务编号
   */
  businessCode: string;

  /**
   * 更新后的当前流水号
   */
  currentSequence: number;

  /**
   * 规则编码
   */
  ruleCode: string;

}

