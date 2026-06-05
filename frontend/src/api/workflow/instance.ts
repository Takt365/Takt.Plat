// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/workflow
// 文件名称：instance.ts
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例运行时 API（引擎：发起/待办/审批/加签等）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type {
  FlowInstance,
  FlowInstanceQuery,
  FlowInstanceUpdate,
  FlowStart,
  FlowCompleteTask,
  FlowTransfer,
  FlowAddApprovers,
  FlowReduceApproval,
  FlowInstanceOperate
} from '@/types/workflow/flow-instance';
import type {
  FlowInstanceDetail,
  FlowTodoItem,
  FlowTodoQuery,
  FlowMyInstanceQuery
} from '@/types/workflow/flow-engine';

/**
 * 流程引擎 API 前缀（对应 TaktFlowEngineController，运行时）
 */
const FLOW_ENGINE_API_BASE = 'TaktFlowEngine';

/**
 * 流程实例 CRUD API 前缀（对应 TaktFlowInstancesController，脚本生成）
 */
const FLOW_INSTANCE_CRUD_API_BASE = 'TaktFlowInstances';

/**
 * 将后端列表项映射为前端 instanceId 字段
 * @param row 原始行
 * @returns 映射后的行
 */
function mapListRow<T extends { flowInstanceId?: string; instanceId?: string }>(row: T): T & { instanceId: string } {
  const id = row.instanceId ?? row.flowInstanceId ?? '';
  return { ...row, instanceId: String(id) };
}

// ========================================
// 详情与列表（引擎）
// ========================================

/**
 * 根据 ID 获取流程实例详情
 * @param id 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 详情
 */
export function getFlowInstanceById(id: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取待办列表（分页）
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowTodoItem>>} 分页结果
 */
/**
 * 构建待办/已办列表查询参数（关键词走基类 keyWords）
 * @param pageIndex 页码
 * @param pageSize 每页条数
 * @param keyword 关键词
 * @returns 查询 DTO
 */
export function buildFlowTodoQuery(pageIndex: number, pageSize: number, keyword?: string): FlowTodoQuery {
  const params: FlowTodoQuery = { pageIndex, pageSize };
  const kw = keyword?.trim();
  if (kw) params.keyWords = kw;
  return params;
}

/**
 * 待办行映射为前端表格字段
 * @param row 后端待办项
 * @returns 表格行
 */
function mapTodoListRow(row: FlowTodoItem & { flowInstanceId?: string; flowTaskId?: string; taskName?: string }): FlowTodoItem & { instanceId: string; taskId: string; nodeName?: string } {
  const instanceId = String(row.flowInstanceId ?? row.instanceId ?? '');
  return {
    ...row,
    instanceId,
    taskId: String(row.flowTaskId ?? row.taskId ?? ''),
    nodeName: row.taskName ?? row.nodeName,
  };
}

export function getFlowInstanceTodoList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowTodoItem>> {
  return request<TaktPagedResult<FlowTodoItem>>({
    url: `${FLOW_ENGINE_API_BASE}/todo/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapTodoListRow(row as FlowTodoItem & { flowInstanceId?: string; flowTaskId?: string; taskName?: string })),
  }));
}

/**
 * 获取我发起的流程列表
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowInstance>>} 分页结果
 */
/**
 * 构建我的流程列表查询参数
 * @param pageIndex 页码
 * @param pageSize 每页条数
 * @param keyword 关键词
 * @returns 查询 DTO
 */
export function buildFlowMyQuery(pageIndex: number, pageSize: number, keyword?: string): FlowMyInstanceQuery {
  const params: FlowMyInstanceQuery = { pageIndex, pageSize, myStartedOnly: true };
  const kw = keyword?.trim();
  if (kw) params.keyWords = kw;
  return params;
}

export function getFlowInstanceMyList(queryDto: FlowMyInstanceQuery): Promise<TaktPagedResult<FlowInstance>> {
  return request<TaktPagedResult<FlowInstance>>({
    url: `${FLOW_ENGINE_API_BASE}/my/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapListRow(row as FlowInstance & { flowInstanceId?: string })),
  }));
}

/**
 * 获取已办流程列表
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowInstance>>} 分页结果
 */
export function getFlowInstanceProcessedList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowInstance>> {
  return request<TaktPagedResult<FlowInstance>>({
    url: `${FLOW_ENGINE_API_BASE}/processed/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapListRow(row as FlowInstance & { flowInstanceId?: string })),
  }));
}

// ========================================
// 发起与草稿
// ========================================

/**
 * 发起流程
 * @param dto 发起参数
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function startFlowInstance(dto: FlowStart): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/start`,
    method: 'post',
    data: dto,
  });
}

/**
 * 保存草稿
 * @param dto 发起参数
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function createFlowInstanceDraft(dto: FlowStart): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/draft`,
    method: 'post',
    data: dto,
  });
}

/**
 * 从草稿启动
 * @param instanceId 实例 ID
 * @returns {Promise<FlowInstanceDetail>} 实例详情
 */
export function startFlowInstanceFromDraft(instanceId: string): Promise<FlowInstanceDetail> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/${instanceId}/start-from-draft`,
    method: 'post',
  });
}

// ========================================
// 审批操作
// ========================================

/**
 * 办结任务（通过/驳回）
 * @param dto 办结参数
 * @returns {Promise<void>}
 */
export function completeFlowInstanceTask(dto: FlowCompleteTask): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/complete`,
    method: 'post',
    data: dto,
  });
}

/**
 * 撤回流程
 * @param instanceCode 实例编码
 * @returns {Promise<void>}
 */
export function revokeFlowInstance(instanceCode: string): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/revoke`,
    method: 'post',
    params: { instanceCode },
  });
}

/**
 * 转办
 * @param dto 转办参数
 * @returns {Promise<void>}
 */
export function transferFlowInstance(dto: FlowTransfer): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/transfer`,
    method: 'post',
    data: dto,
  });
}

/**
 * 加签
 * @param dto 加签参数
 * @returns {Promise<void>}
 */
export function addFlowInstanceApprovers(dto: FlowAddApprovers): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/add-approvers`,
    method: 'post',
    data: dto,
  });
}

/**
 * 减签
 * @param dto 减签参数
 * @returns {Promise<void>}
 */
export function reduceFlowInstanceApproval(dto: FlowReduceApproval): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/reduce-sign`,
    method: 'post',
    data: dto,
  });
}

/**
 * 挂起流程
 * @param dto 参数
 * @returns {Promise<void>}
 */
export function suspendFlowInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/suspend`,
    method: 'post',
    data: dto,
  });
}

/**
 * 恢复流程
 * @param dto 参数
 * @returns {Promise<void>}
 */
export function resumeFlowInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/resume`,
    method: 'post',
    data: dto,
  });
}

/**
 * 终止流程
 * @param dto 参数
 * @returns {Promise<void>}
 */
export function terminateFlowInstance(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/terminate`,
    method: 'post',
    data: dto,
  });
}

/**
 * 撤销当前节点审批
 * @param dto 参数
 * @returns {Promise<void>}
 */
export function undoFlowInstanceVerification(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/undo-verification`,
    method: 'post',
    data: dto,
  });
}

// ========================================
// 管理端 CRUD（复用 flow-instance 能力）
// ========================================

/**
 * 获取流程实例列表（分页）
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowInstance>>} 分页结果
 */
export function getFlowInstanceList(queryDto: FlowInstanceQuery): Promise<TaktPagedResult<FlowInstance>> {
  return request<TaktPagedResult<FlowInstance>>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapListRow(row as FlowInstance & { flowInstanceId?: string })),
  }));
}

/**
 * 更新流程实例（草稿编辑等）
 * @param payload 含 id 与更新字段
 * @returns {Promise<FlowInstance>} 更新结果
 */
export function updateFlowInstance(payload: FlowInstanceUpdate & { id: string }): Promise<FlowInstance> {
  const { id, ...dto } = payload;
  return request<FlowInstance>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程实例
 * @param id 实例 ID
 * @returns {Promise<void>}
 */
export function deleteFlowInstanceById(id: string): Promise<void> {
  return request({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程实例
 * @param ids ID 列表
 * @returns {Promise<void>}
 */
export function deleteFlowInstanceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 导出流程实例（管理端）
 * @param query 查询条件
 * @returns {Promise<Blob>} Excel
 */
export function exportFlowInstanceData(query?: FlowInstanceQuery): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/export`,
    method: 'get',
    params: query,
    responseType: 'blob',
  });
}

/**
 * 导出待办
 * @param query 查询条件
 * @returns {Promise<Blob>} Excel
 */
export function exportFlowInstanceTodo(query?: FlowTodoQuery): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/export`,
    method: 'get',
    params: { ...query, sheetName: 'FlowTodo' },
    responseType: 'blob',
  });
}

/**
 * 导出我的流程
 * @param query 查询条件
 * @returns {Promise<Blob>} Excel
 */
export function exportFlowInstanceMy(query?: FlowInstanceQuery): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/export`,
    method: 'get',
    params: query,
    responseType: 'blob',
  });
}

/**
 * 导出已办流程
 * @param query 查询条件
 * @returns {Promise<Blob>} Excel
 */
export function exportFlowInstanceProcessed(query?: FlowTodoQuery): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_CRUD_API_BASE}/export`,
    method: 'get',
    params: query,
    responseType: 'blob',
  });
}
