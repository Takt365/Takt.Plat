// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：file-upload.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 分片存在性检查请求（对应 upload.ts checkChunk）
 * 对应前端 FileChunkCheck
 * @description 对应后端 TaktFileChunkCheckDto
 */
export interface FileChunkCheck {
  /**
   * 文件唯一标识（通常为 MD5）
   */
  identifier: string;

  /**
   * 分片序号（从 1 开始）
   */
  chunkNumber: number;

  /**
   * 当前分片大小（字节）
   */
  chunkSize: string;

  /**
   * 文件总大小（字节）
   */
  totalSize: string;

  /**
   * 原始文件名
   */
  fileName?: string;

}


/**
 * 分片存在性检查结果
 * 对应前端 FileChunkCheckResult
 * @description 对应后端 TaktFileChunkCheckResultDto
 */
export interface FileChunkCheckResult {
  /**
   * 分片是否已存在
   */
  exists: boolean;

}


/**
 * 分片上传元数据（multipart 表单字段，不含 file 流）
 * 对应前端 FileChunkUpload
 * @description 对应后端 TaktFileChunkUploadDto
 */
export interface FileChunkUpload {
  /**
   * 文件唯一标识
   */
  identifier: string;

  /**
   * 分片序号（从 1 开始）
   */
  chunkNumber: number;

  /**
   * 总分片数
   */
  totalChunks: number;

  /**
   * 当前分片大小（字节）
   */
  chunkSize: string;

  /**
   * 文件总大小（字节）
   */
  totalSize: string;

  /**
   * 原始文件名
   */
  fileName: string;

}


/**
 * 分片合并请求（对应 upload.ts mergeChunks）
 * 对应前端 FileChunkMerge
 * @description 对应后端 TaktFileChunkMergeDto
 */
export interface FileChunkMerge {
  /**
   * 文件唯一标识
   */
  identifier: string;

  /**
   * 原始文件名
   */
  fileName: string;

  /**
   * 总分片数
   */
  totalChunks: number;

  /**
   * 文件总大小（字节）
   */
  totalSize: string;

  /**
   * 文件描述（可选）
   */
  fileDescription?: string;

  /**
   * 文件标签（可选）
   */
  fileTags?: string;

  /**
   * 是否公开（默认公开）→ <c>TaktFile.IsPublic</c>
   */
  isPublic?: number;

  /**
   * IP 地址（上传来源；未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
   */
  ipAddress?: string;

  /**
   * 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
   */
  location?: string;

}


/**
 * 整文件上传附加元数据（multipart 可选字段）
 * 对应前端 FileUploadMeta
 * @description 对应后端 TaktFileUploadMetaDto
 */
export interface FileUploadMeta {
  /**
   * 文件描述 → <c>TaktFile.FileDescription</c>
   */
  fileDescription?: string;

  /**
   * 文件标签 → <c>TaktFile.FileTags</c>
   */
  fileTags?: string;

  /**
   * 是否公开 → <c>TaktFile.IsPublic</c>
   */
  isPublic?: number;

  /**
   * IP 地址（未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
   */
  ipAddress?: string;

  /**
   * 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
   */
  location?: string;

}


/**
 * 文件公开范围更新 DTO
 * 对应前端 FilePublicAccess
 * @description 对应后端 TaktFilePublicAccessDto
 */
export interface FilePublicAccess {
  /**
   * 是否公开
   */
  isPublic: number;

}

