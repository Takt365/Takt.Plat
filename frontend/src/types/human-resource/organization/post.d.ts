// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：post.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/organization 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 岗位实体 代表组织架构中的岗位/职位 参照 SAP Position (STELL) 设计
 * 对应前端 TaktPostDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Post
 * @description 对应后端 TaktPostDto
 */
export interface Post extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode?: string;

  /**
   * 岗位名称
   */
  postName?: string;

  /**
   * 所属部门ID
   */
  deptId?: string;

  /**
   * 岗位类别（字典 sys_post_category；DictValue：MGT=管理岗，PRO=专业岗，TEC=技术岗，SUP=支持岗，OPS=操作岗）
   */
  postCategory?: string;

  /**
   * 岗位职级（字典 sys_post_level_category；DictValue：P1~P4 专业序列，M1~M5 管理序列）
   */
  postLevel?: string;

  /**
   * 编制人数
   */
  headcount?: number;

  /**
   * 当前在职人数
   */
  currentCount?: number;

  /**
   * 岗位职责
   */
  responsibilities?: string;

  /**
   * 任职要求
   */
  requirements?: string;

  /**
   * 学历要求（字典 hr_education_level_category；1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
   */
  educationRequired?: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears?: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  postStatus?: number;

  /**
   * 内置（1=是，0=否） 种子岗位为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 关联该岗位的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

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
 * Post 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PostExport
 * @description 对应后端 TaktPostExportDto
 */
export interface PostExport {
  /**
   * PostID
   */
  postId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode: string;

  /**
   * 岗位名称
   */
  postName: string;

  /**
   * 所属部门ID
   */
  deptId: string;

  /**
   * 岗位类别（字典 sys_post_category；DictValue：MGT=管理岗，PRO=专业岗，TEC=技术岗，SUP=支持岗，OPS=操作岗）
   */
  postCategory: string;

  /**
   * 岗位职级（字典 sys_post_level_category；DictValue：P1~P4 专业序列，M1~M5 管理序列）
   */
  postLevel: string;

  /**
   * 编制人数
   */
  headcount: number;

  /**
   * 当前在职人数
   */
  currentCount: number;

  /**
   * 岗位职责
   */
  responsibilities: string;

  /**
   * 任职要求
   */
  requirements: string;

  /**
   * 学历要求（字典 hr_education_level_category；1=高中及以下，2=大专，3=本科，4=硕士，5=博士）
   */
  educationRequired: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  postStatus: number;

  /**
   * 内置（1=是，0=否） 种子岗位为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

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

