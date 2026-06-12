// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-file-chunk-upload.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 文件分片上传/断点续传（对接 api/TaktFiles check/chunk/merge/chunk-list）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  cancelFileChunks,
  checkFileChunk,
  listFileChunks,
  mergeFileChunks,
  uploadFile,
  uploadFileChunk,
} from '@/api/foundation/file';
import type {
  FileChunkMerge,
  FileUploadMeta,
  FileUploadResult,
} from '@/types/foundation/file';
import { generateFileIdentifier } from '@/utils/upload';

/** 默认分片大小：2MB */
export const TAKT_FILE_DEFAULT_CHUNK_SIZE = 2 * 1024 * 1024;

/** 超过此大小走分片上传（5MB） */
export const TAKT_FILE_CHUNK_THRESHOLD = 5 * 1024 * 1024;

/** 默认最大并发分片数 */
export const TAKT_FILE_DEFAULT_CONCURRENCY = 3;

/** 默认分片重试次数 */
export const TAKT_FILE_DEFAULT_MAX_RETRIES = 3;

/**
 * 分片上传状态
 */
export enum TaktFileChunkUploadStatus {
  /** 等待中 */
  Waiting = 'waiting',
  /** 计算 MD5 */
  Hashing = 'hashing',
  /** 上传中 */
  Uploading = 'uploading',
  /** 合并中 */
  Merging = 'merging',
  /** 已暂停 */
  Paused = 'paused',
  /** 成功 */
  Success = 'success',
  /** 失败 */
  Error = 'error',
  /** 已取消 */
  Cancelled = 'cancelled',
}

/**
 * 分片上传进度
 */
export interface TaktFileChunkUploadProgress {
  /** 状态 */
  status: TaktFileChunkUploadStatus;
  /** 总进度 0-100 */
  percent: number;
  /** 已上传分片数 */
  uploadedChunks: number;
  /** 总分片数 */
  totalChunks: number;
  /** 文件 MD5 标识 */
  identifier: string;
}

/**
 * 分片上传选项
 */
export interface TaktFileChunkUploadOptions {
  /** 分片大小（字节） */
  chunkSize?: number;
  /** 并发数 */
  concurrency?: number;
  /** 分片最大重试次数 */
  maxRetries?: number;
  /** 业务元数据（合并时写入） */
  meta?: FileUploadMeta;
  /** 进度回调 */
  onProgress?: (progress: TaktFileChunkUploadProgress) => void;
}

interface InternalChunkState {
  index: number;
  blob: Blob;
  uploaded: boolean;
  uploading: boolean;
  retries: number;
  abortController?: AbortController;
}

/**
 * 是否应使用分片上传
 * @param fileSize 文件大小（字节）
 * @returns 是否分片
 */
export function shouldUseTaktFileChunkUpload(fileSize: number): boolean {
  if (!Number.isFinite(fileSize) || fileSize <= 0) {
    return false;
  }
  return fileSize > TAKT_FILE_CHUNK_THRESHOLD;
}

/** 分片上传暂停信号（非错误，用于 UI 保持弹窗） */
export class TaktFileChunkUploadPausedError extends Error {
  /**
   * @param message 说明
   */
  constructor(message = 'PAUSED') {
    super(message);
    this.name = 'TaktFileChunkUploadPausedError';
  }
}

/**
 * Foundation 文件分片上传器（支持断点续传、暂停/恢复、取消）
 */
export class TaktFileChunkUploader {
  private readonly file: globalThis.File;
  private readonly options: Required<Pick<TaktFileChunkUploadOptions, 'chunkSize' | 'concurrency' | 'maxRetries'>> &
    Pick<TaktFileChunkUploadOptions, 'meta' | 'onProgress'>;
  private chunks: InternalChunkState[] = [];
  private identifier = '';
  private status: TaktFileChunkUploadStatus = TaktFileChunkUploadStatus.Waiting;
  private paused = false;
  private cancelled = false;
  private uploadPromise: Promise<FileUploadResult> | null = null;

  /**
   * @param file 待上传文件
   * @param options 上传选项
   */
  constructor(file: globalThis.File, options: TaktFileChunkUploadOptions = {}) {
    if (!file || file.size <= 0) {
      throw new Error('文件不能为空');
    }
    this.file = file;
    this.options = {
      chunkSize: options.chunkSize ?? TAKT_FILE_DEFAULT_CHUNK_SIZE,
      concurrency: options.concurrency ?? TAKT_FILE_DEFAULT_CONCURRENCY,
      maxRetries: options.maxRetries ?? TAKT_FILE_DEFAULT_MAX_RETRIES,
      meta: options.meta,
      onProgress: options.onProgress,
    };
    this.chunks = this.createChunks();
  }

  /**
   * 开始上传（可 await）
   * @returns 上传结果
   */
  start(): Promise<FileUploadResult> {
    if (this.uploadPromise) {
      return this.uploadPromise;
    }
    this.uploadPromise = this.runUpload();
    return this.uploadPromise;
  }

  /**
   * 暂停上传
   */
  pause(): void {
    if (this.status !== TaktFileChunkUploadStatus.Uploading) {
      return;
    }
    this.paused = true;
    this.chunks.forEach((chunk) => {
      chunk.abortController?.abort();
      chunk.uploading = false;
    });
    this.setStatus(TaktFileChunkUploadStatus.Paused);
  }

  /**
   * 恢复上传
   */
  resume(): Promise<FileUploadResult> {
    if (this.status !== TaktFileChunkUploadStatus.Paused && this.status !== TaktFileChunkUploadStatus.Error) {
      return this.start();
    }
    this.paused = false;
    this.cancelled = false;
    this.uploadPromise = this.runUpload();
    return this.uploadPromise;
  }

  /**
   * 取消上传并清理服务端临时分片
   */
  async cancel(): Promise<void> {
    this.cancelled = true;
    this.paused = false;
    this.chunks.forEach((chunk) => {
      chunk.abortController?.abort();
      chunk.uploading = false;
    });
    this.setStatus(TaktFileChunkUploadStatus.Cancelled);
    if (this.identifier) {
      try {
        await cancelFileChunks({ identifier: this.identifier });
      } catch {
        // 取消清理失败不阻断 UI
      }
    }
  }

  /**
   * 执行上传主流程
   * @returns 上传结果
   */
  private async runUpload(): Promise<FileUploadResult> {
    try {
      this.setStatus(TaktFileChunkUploadStatus.Hashing);
      if (!this.identifier) {
        this.identifier = await generateFileIdentifier(this.file);
      }
      await this.restoreUploadedChunks();
      this.setStatus(TaktFileChunkUploadStatus.Uploading);
      const allUploaded = await this.uploadAllChunks();
      if (this.cancelled) {
        throw new Error('上传已取消');
      }
      if (this.paused || !allUploaded) {
        throw new TaktFileChunkUploadPausedError();
      }
      this.setStatus(TaktFileChunkUploadStatus.Merging);
      const mergeDto: FileChunkMerge = {
        identifier: this.identifier,
        fileName: this.file.name,
        totalChunks: this.chunks.length,
        totalSize: String(this.file.size),
        fileDescription: this.options.meta?.fileDescription,
        fileTags: this.options.meta?.fileTags,
        isPublic: this.options.meta?.isPublic,
        fileUploadType: this.options.meta?.fileUploadType,
        targetFileName: this.options.meta?.targetFileName,
      };
      const result = await mergeFileChunks(mergeDto);
      this.setStatus(TaktFileChunkUploadStatus.Success);
      this.emitProgress(100);
      return result;
    } catch (error) {
      if (error instanceof TaktFileChunkUploadPausedError) {
        this.setStatus(TaktFileChunkUploadStatus.Paused);
        throw error;
      }
      if (this.cancelled) {
        this.setStatus(TaktFileChunkUploadStatus.Cancelled);
      } else if (this.paused) {
        this.setStatus(TaktFileChunkUploadStatus.Paused);
      } else {
        this.setStatus(TaktFileChunkUploadStatus.Error);
      }
      throw error;
    }
  }

  /**
   * 从服务端恢复已上传分片（断点续传）
   */
  private async restoreUploadedChunks(): Promise<void> {
    const listResult = await listFileChunks({
      identifier: this.identifier,
      totalChunks: this.chunks.length,
    });
    const uploadedSet = new Set(listResult.uploadedChunkNumbers ?? []);
    this.chunks.forEach((chunk) => {
      const chunkNumber = chunk.index + 1;
      if (uploadedSet.has(chunkNumber)) {
        chunk.uploaded = true;
      }
    });
    this.emitProgress(this.calcPercent());
  }

  /**
   * 并发上传未完成分片
   * @returns 是否全部分片已上传
   */
  private async uploadAllChunks(): Promise<boolean> {
    const pending = (): InternalChunkState | undefined =>
      this.chunks.find((c) => !c.uploaded && !c.uploading && !this.paused && !this.cancelled);

    const workers: Promise<void>[] = [];
    const worker = async (): Promise<void> => {
      while (!this.paused && !this.cancelled) {
        const chunk = pending();
        if (!chunk) {
          break;
        }
        await this.uploadSingleChunk(chunk);
      }
    };
    const concurrency = Math.max(1, Math.min(this.options.concurrency, this.chunks.length));
    for (let i = 0; i < concurrency; i++) {
      workers.push(worker());
    }
    await Promise.all(workers);
    if (this.paused || this.cancelled) {
      return false;
    }
    if (this.chunks.some((c) => !c.uploaded)) {
      throw new Error('部分分片上传失败');
    }
    return true;
  }

  /**
   * 上传单个分片（含重试与断点检查）
   * @param chunk 分片状态
   */
  private async uploadSingleChunk(chunk: InternalChunkState): Promise<void> {
    if (chunk.uploaded || chunk.uploading || this.paused || this.cancelled) {
      return;
    }
    const chunkNumber = chunk.index + 1;
    const checkResult = await checkFileChunk({
      identifier: this.identifier,
      chunkNumber,
      chunkSize: String(chunk.blob.size),
      totalSize: String(this.file.size),
      fileName: this.file.name,
    });
    if (checkResult.exists) {
      chunk.uploaded = true;
      this.emitProgress(this.calcPercent());
      return;
    }
    chunk.uploading = true;
    chunk.abortController = new AbortController();
    try {
      const chunkFile = new File([chunk.blob], this.file.name, { type: this.file.type });
      await uploadFileChunk(chunkFile, {
        identifier: this.identifier,
        chunkNumber,
        totalChunks: this.chunks.length,
        chunkSize: String(chunk.blob.size),
        totalSize: String(this.file.size),
        fileName: this.file.name,
      });
      chunk.uploaded = true;
      chunk.retries = 0;
      this.emitProgress(this.calcPercent());
    } catch (error) {
      if (this.paused || this.cancelled) {
        return;
      }
      if (chunk.retries < this.options.maxRetries) {
        chunk.retries += 1;
        chunk.uploading = false;
        await this.uploadSingleChunk(chunk);
        return;
      }
      throw error;
    } finally {
      chunk.uploading = false;
      chunk.abortController = undefined;
    }
  }

  /**
   * 切分文件为分片
   * @returns 分片状态列表
   */
  private createChunks(): InternalChunkState[] {
    const size = this.options.chunkSize;
    const total = Math.ceil(this.file.size / size);
    const chunks: InternalChunkState[] = [];
    for (let i = 0; i < total; i++) {
      const start = i * size;
      const end = Math.min(start + size, this.file.size);
      chunks.push({
        index: i,
        blob: this.file.slice(start, end),
        uploaded: false,
        uploading: false,
        retries: 0,
      });
    }
    return chunks;
  }

  /**
   * 计算总进度
   * @returns 0-100
   */
  private calcPercent(): number {
    if (this.chunks.length === 0) {
      return 0;
    }
    const uploaded = this.chunks.filter((c) => c.uploaded).length;
    return Math.round((uploaded * 100) / this.chunks.length);
  }

  /**
   * 更新状态并通知
   * @param status 新状态
   */
  private setStatus(status: TaktFileChunkUploadStatus): void {
    this.status = status;
    this.emitProgress(this.calcPercent());
  }

  /**
   * 触发进度回调
   * @param percent 进度
   */
  private emitProgress(percent: number): void {
    this.options.onProgress?.({
      status: this.status,
      percent,
      uploadedChunks: this.chunks.filter((c) => c.uploaded).length,
      totalChunks: this.chunks.length,
      identifier: this.identifier,
    });
  }
}

/**
 * 智能上传：小文件整传，大文件分片+断点续传
 * @param file 文件
 * @param meta 业务元数据
 * @param options 分片选项
 * @returns 上传结果
 */
export async function uploadTaktFileSmart(
  file: globalThis.File,
  meta?: FileUploadMeta,
  options?: TaktFileChunkUploadOptions
): Promise<FileUploadResult> {
  if (!shouldUseTaktFileChunkUpload(file.size)) {
    return uploadFile(file, meta);
  }
  const uploader = new TaktFileChunkUploader(file, { ...options, meta });
  return uploader.start();
}

/**
 * 强制分片上传（含断点续传）
 * @param file 文件
 * @param meta 业务元数据
 * @param options 分片选项
 * @returns 上传结果
 */
export async function uploadTaktFileWithChunks(
  file: globalThis.File,
  meta?: FileUploadMeta,
  options?: TaktFileChunkUploadOptions
): Promise<FileUploadResult> {
  const uploader = new TaktFileChunkUploader(file, { ...options, meta });
  return uploader.start();
}
