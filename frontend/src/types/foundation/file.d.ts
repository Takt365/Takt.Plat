// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：file.d.ts
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 文件实体 公司级实体：文件元数据按租户+公司隔离；字段与前端 entity.file.* 及业务附件 JSON 结构对齐
 * 对应前端 TaktFileDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 File
 * @description 对应后端 TaktFileDto
 */
export interface File extends CompanyDtoBase {
  /**
   * FileID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  fileId: string;

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件 MIME 类型
   */
  fileType: string;

  /**
   * 文件扩展名
   */
  fileExtension: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl: string;

  /**
   * 下载次数
   */
  downloadCount: number;

  /**
   * 最后下载时间
   */
  lastDownloadTime?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  fileStatus: number;

  /**
   * 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
   */
  isPublic: number;

  /**
   * 文件描述
   */
  fileDescription: string;

  /**
   * 文件标签（多个标签用逗号分隔）
   */
  fileTags: string;

  /**
   * IP 地址（上传或访问来源）
   */
  ipAddress: string;

  /**
   * 位置（IP 对应地理位置）
   */
  location: string;

}


/**
 * File 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FileQuery
 * @description 对应后端 TaktFileQueryDto
 */
export interface FileQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode?: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName?: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName?: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件 MIME 类型
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash?: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory?: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType?: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

  /**
   * 下载次数
   */
  downloadCount?: number;

  /**
   * 最后下载时间（范围查询-开始）
   */
  lastDownloadTimeStart?: string;

  /**
   * 最后下载时间（范围查询-结束）
   */
  lastDownloadTimeEnd?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  fileStatus?: number;

  /**
   * 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
   */
  isPublic?: number;

  /**
   * 文件描述
   */
  fileDescription?: string;

  /**
   * 文件标签（多个标签用逗号分隔）
   */
  fileTags?: string;

  /**
   * IP 地址（上传或访问来源）
   */
  ipAddress?: string;

  /**
   * 位置（IP 对应地理位置）
   */
  location?: string;

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
 * 创建File DTO
 * 对应前端 FileCreate
 * @description 对应后端 TaktFileCreateDto
 */
export interface FileCreate {
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
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件 MIME 类型
   */
  fileType: string;

  /**
   * 文件扩展名
   */
  fileExtension: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl: string;

  /**
   * 下载次数
   */
  downloadCount: number;

  /**
   * 最后下载时间
   */
  lastDownloadTime?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  fileStatus: number;

  /**
   * 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
   */
  isPublic: number;

  /**
   * 文件描述
   */
  fileDescription: string;

  /**
   * 文件标签（多个标签用逗号分隔）
   */
  fileTags: string;

  /**
   * IP 地址（上传或访问来源）
   */
  ipAddress: string;

  /**
   * 位置（IP 对应地理位置）
   */
  location: string;

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
 * 更新File DTO
 * 继承 TaktFileCreateDto，添加 FileId 字段
 * 对应前端 FileUpdate
 * @description 对应后端 TaktFileUpdateDto
 */
export interface FileUpdate extends FileCreate {
  /**
   * FileID（标识要更新的实体）
   */
  fileId: string;

}


/**
 * File 状态更新 DTO
 * 对应前端 FileStatus
 * @description 对应后端 TaktFileStatusDto
 */
export interface FileStatus {
  /**
   * FileID
   */
  fileId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  fileStatus: number;

}


/**
 * File 导入模板行 DTO
 * 对应前端 FileTemplate
 * @description 对应后端 TaktFileTemplateDto
 */
export interface FileTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode?: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName?: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName?: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件 MIME 类型
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash?: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory?: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType?: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

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
 * File 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FileImport
 * @description 对应后端 TaktFileImportDto
 */
export interface FileImport {
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
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode?: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName?: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName?: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件 MIME 类型
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash?: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory?: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType?: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

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
 * File 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FileExport
 * @description 对应后端 TaktFileExportDto
 */
export interface FileExport {
  /**
   * FileID
   */
  fileId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode: string;

  /**
   * 文件名称（存储文件名）
   */
  fileName: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName: string;

  /**
   * 文件路径（相对路径或完整路径）
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件 MIME 类型
   */
  fileType: string;

  /**
   * 文件扩展名
   */
  fileExtension: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash: string;

  /**
   * 文件分类（字典 sys_file_category）
   */
  fileCategory: number;

  /**
   * 存储方式（字典 sys_storage_type）
   */
  storageType: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl: string;

  /**
   * 下载次数
   */
  downloadCount: number;

  /**
   * 最后下载时间
   */
  lastDownloadTime?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  fileStatus: number;

  /**
   * 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
   */
  isPublic: number;

  /**
   * 文件描述
   */
  fileDescription: string;

  /**
   * 文件标签（多个标签用逗号分隔）
   */
  fileTags: string;

  /**
   * IP 地址（上传或访问来源）
   */
  ipAddress: string;

  /**
   * 位置（IP 对应地理位置）
   */
  location: string;

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

