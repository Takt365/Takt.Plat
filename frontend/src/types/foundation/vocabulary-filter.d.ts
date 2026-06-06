// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：vocabulary-filter.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 敏感词过滤/检测请求 DTO
 * 对应前端 VocabularyFilterRequest
 * @description 对应后端 TaktVocabularyFilterRequestDto
 */
export interface VocabularyFilterRequest {
  /**
   * 待检测或过滤的文本
   */
  text: string;

  /**
   * 最低过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）；为空时匹配全部启用词条
   */
  minFilterLevel?: number;

}


/**
 * 敏感词过滤结果 DTO
 * 对应前端 VocabularyFilterResult
 * @description 对应后端 TaktVocabularyFilterResultDto
 */
export interface VocabularyFilterResult {
  /**
   * 原始文本
   */
  originalText: string;

  /**
   * 过滤后的文本
   */
  filteredText: string;

  /**
   * 是否命中敏感词
   */
  hasSensitiveWord: boolean;

  /**
   * 命中的敏感词列表（去重）
   */
  matchedWords: string[];

}


/**
 * 敏感词检测结果 DTO（不返回替换后文本）
 * 对应前端 VocabularyDetectResult
 * @description 对应后端 TaktVocabularyDetectResultDto
 */
export interface VocabularyDetectResult {
  /**
   * 是否命中敏感词
   */
  hasSensitiveWord: boolean;

  /**
   * 命中的敏感词列表（去重）
   */
  matchedWords: string[];

}

