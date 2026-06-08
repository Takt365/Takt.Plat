// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：dept.d.ts
// 创建时间：2026-06-08
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
   * DeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  deptId: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门名称
   */
  deptName: string;

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
   * 部门负责人名称（填充字段）
   */
  headUserName?: string;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder: number;

  /**
   * 部门描述
   */
  description: string;

  /**
   * 角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept） （子表：TaktRoleDept）
   */
  roleDepts?: RoleDept[];

  /**
   * 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept） （子表：TaktEmployeeDept）
   */
  employeeDepts?: EmployeeDept[];

}


/**
 * Dept 树形列表/树选择 DTO（含子节点）
 * 对应 GetDeptTreeAsync 等接口
 * 对应前端 DeptTree
 * @description 对应后端 TaktDeptTreeDto
 */
export interface DeptTree extends Dept {
  /**
   * 子节点
   */
  children: DeptTree[];

}


/**
 * Dept 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DeptQuery
 * @description 对应后端 TaktDeptQueryDto
 */
export interface DeptQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 父部门ID（0表示根部门）
   */
  parentId?: string;

  /**
   * 层级（1=一级部门，2=二级部门，以此类推）
   */
  level?: number;

  /**
   * 部门路径（如：/1/3/5/，用于快速查询子部门）
   */
  deptPath?: string;

  /**
   * 是否叶子节点（0=否，1=是）
   */
  isLeaf?: number;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder?: number;

  /**
   * 部门描述
   */
  description?: string;

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
 * 创建Dept DTO
 * 对应前端 DeptCreate
 * @description 对应后端 TaktDeptCreateDto
 */
export interface DeptCreate {
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
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门名称
   */
  deptName: string;

  /**
   * 父部门ID（0表示根部门）
   */
  parentId: string;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder: number;

  /**
   * 部门描述
   */
  description: string;

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
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Dept DTO
 * 继承 TaktDeptCreateDto，添加 DeptId 字段
 * 对应前端 DeptUpdate
 * @description 对应后端 TaktDeptUpdateDto
 */
export interface DeptUpdate extends DeptCreate {
  /**
   * DeptID（标识要更新的实体）
   */
  deptId: string;

}


/**
 * Dept 状态更新 DTO
 * 对应前端 DeptStatus
 * @description 对应后端 TaktDeptStatusDto
 */
export interface DeptStatus {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  deptStatus: number;

}


/**
 * Dept 排序更新 DTO
 * 对应前端 DeptSort
 * @description 对应后端 TaktDeptSortDto
 */
export interface DeptSort {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder: number;

}


/**
 * Dept 导入模板行 DTO
 * 对应前端 DeptTemplate
 * @description 对应后端 TaktDeptTemplateDto
 */
export interface DeptTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门名称
   */
  deptName?: string;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder?: number;

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
 * Dept 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DeptImport
 * @description 对应后端 TaktDeptImportDto
 */
export interface DeptImport {
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
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门名称
   */
  deptName?: string;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder?: number;

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
   * 是否内置（1=是，0=否） 种子部门为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 排序号（同级部门排序）
   */
  sortOrder: number;

  /**
   * 部门描述
   */
  description: string;

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

