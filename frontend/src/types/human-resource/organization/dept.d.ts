// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：dept.d.ts
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
 * 部门实体 代表组织架构中的部门（树形结构） 参照 SAP Organizational Unit (ORGEH) 设计
 * 对应前端 TaktDeptDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Dept
 * @description 对应后端 TaktDeptDto
 */
export interface Dept extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 部门简称（必填；最多 6 个字母，如 FIN、ENG、PMC）
   */
  deptShortName?: string;

  /**
   * 父部门ID（0表示根部门）
   */
  parentId?: string;

  /**
   * 成本中心编码（关联财务成本中心）
   */
  costCenterCode?: string;

  /**
   * 费用类别（1=直接，2=间接）
   */
  costCategory?: number;

  /**
   * 部门负责人ID（关联TaktUser.Id）
   */
  headUserId?: string;

  /**
   * 联系电话
   */
  phone?: string;

  /**
   * 邮箱
   */
  email?: string;

  /**
   * 办公地点
   */
  location?: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  deptStatus?: number;

  /**
   * 内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 部门描述
   */
  deptDescription?: string;

  /**
   * 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
   */
  roleIds?: any;

  /**
   * 关联该部门的员工 ID 列表（RBAC 反向合并）
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
 * Dept 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DeptExport
 * @description 对应后端 TaktDeptExportDto
 */
export interface DeptExport {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门名称
   */
  deptName: string;

  /**
   * 部门简称（必填；最多 6 个字母，如 FIN、ENG、PMC）
   */
  deptShortName: string;

  /**
   * 父部门ID（0表示根部门）
   */
  parentId: string;

  /**
   * 层级（1=一级部门，2=二级部门，以此类推）
   */
  level: number;

  /**
   * 部门路径（如：/1/3/5/，用于快速查询子部门）
   */
  deptPath: string;

  /**
   * 是否叶子节点（0=否，1=是）
   */
  isLeaf: number;

  /**
   * 成本中心编码（关联财务成本中心）
   */
  costCenterCode: string;

  /**
   * 费用类别（1=直接，2=间接）
   */
  costCategory: number;

  /**
   * 部门负责人ID（关联TaktUser.Id）
   */
  headUserId: string;

  /**
   * 联系电话
   */
  phone: string;

  /**
   * 邮箱
   */
  email: string;

  /**
   * 办公地点
   */
  location: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  deptStatus: number;

  /**
   * 内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder: number;

  /**
   * 部门描述
   */
  deptDescription: string;

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

