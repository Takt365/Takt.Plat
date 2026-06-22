// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：file-upload.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 文件上传/分片协议类型（与 TaktFileUploadPolicyResult 对齐；勿放入自动生成 file.d.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 上传策略（GET TaktFiles/upload-policy）
 */
export interface FileUploadPolicy {
  /** 单文件最大字节数 */
  maxFileSizeBytes: string | number;
  /** 最大分片数 */
  maxChunkCount: number;
  /** 默认分片大小（字节） */
  defaultChunkSizeBytes: string | number;
  /** 分片上传阈值（字节） */
  chunkThresholdBytes: string | number;
  /** 分片临时目录（相对 wwwroot） */
  chunkRelativePath: string;
  /** 允许扩展名（小写、不含点） */
  allowedExtensions: string[];
  /** 禁止扩展名（小写、不含点） */
  deniedExtensions: string[];
  /** 传入 totalSize 时：是否应分片上传 */
  useChunkUpload?: boolean;
  /** 传入 totalSize 时：分片大小（字节） */
  chunkSizeBytes?: string | number;
  /** 传入 totalSize 时：总分片数 */
  totalChunks?: number;
  /** 传入 totalSize 时：文件总大小（字节） */
  totalSizeBytes?: string | number;
}

/**
 * 整文件/合并上传附带业务元数据
 */
export interface FileUploadMeta {
  fileDescription?: string;
  fileTags?: string;
  isPublic?: number;
  fileStatus?: number;
  fileUploadType?: number;
  targetFileName?: string;
  categoryPath?: string;
  storageType?: number;
  storageNaming?: number;
  storageConfig?: string;
}

/**
 * 上传完成结果（存储层 + 可选业务 fileId）
 */
export interface FileUploadResult {
  fileId?: string;
  fileCode?: string;
  fileName?: string;
  fileOriginalName?: string;
  filePath?: string;
  fileSize?: string | number;
  fileType?: string;
  fileExtension?: string;
  fileHash?: string;
  fileCategory?: number;
  storageType?: number;
  storageConfig?: string;
  accessUrl?: string;
}

/**
 * 分片存在性检查请求
 */
export interface FileChunkCheck {
  identifier: string;
  chunkNumber: number;
  chunkSize: string | number;
  totalSize: string | number;
  totalChunks?: number;
  fileName?: string;
}

/**
 * 分片存在性检查结果
 */
export interface FileChunkCheckResult {
  exists: boolean;
}

/**
 * 已上传分片列表查询
 */
export interface FileChunkList {
  identifier: string;
  totalChunks?: number;
  totalSize: string | number;
}

/**
 * 已上传分片列表结果
 */
export interface FileChunkListResult {
  uploadedChunkNumbers: number[];
}

/**
 * 分片上传表单元数据
 */
export interface FileChunkUpload {
  identifier: string;
  chunkNumber: number;
  totalChunks: number;
  chunkSize: string | number;
  totalSize: string | number;
  fileName: string;
}

/**
 * 分片合并请求（含业务元数据）
 */
export interface FileChunkMerge {
  identifier: string;
  fileName: string;
  totalChunks: number;
  totalSize: string | number;
  fileDescription?: string;
  fileTags?: string;
  isPublic?: number;
  fileUploadType?: number;
  targetFileName?: string;
  categoryPath?: string;
  storageType?: number;
  storageConfig?: string;
  storageNaming?: number;
  fileStatus?: number;
}

/**
 * 取消分片上传
 */
export interface FileChunkCancel {
  identifier: string;
}
