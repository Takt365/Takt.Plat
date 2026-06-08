// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/api/workflow
// 文件名称：flow-engine.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时 API（对应 TaktFlowEngineController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common';
import type {
  FlowStart,
  FlowCompleteTask,
  FlowTransfer,
  FlowAddApprovers,
  FlowReduceApproval,
  FlowInstanceOperate,
  FlowInstanceDetail,
  FlowInstanceDetailView,
  FlowTodoItem,
  FlowTodoQuery,
  FlowMyInstanceQuery,
  FlowInstanceListItem,
  FlowEngineListRow,
  FlowTodoTableRow
} from '@/types/workflow/flow-engine';

/**
 * API 路径前缀（对应 TaktFlowEngineController）
 */
const FLOW_ENGINE_API_BASE = 'TaktFlowEngine';

type ListRowLike = {
  flowInstanceId?: string;
  instanceId?: string;
  currentActivityName?: string;
  currentNodeName?: string;
};

/**
 * 将引擎列表项映射为视图表格字段（instanceId / currentNodeName 别名）
 * @param row 原始行
 * @returns 映射后的行
 */
function mapEngineListRow<T extends ListRowLike>(row: T): T & { instanceId: string; currentNodeName?: string } {
  const id = row.flowInstanceId ?? row.instanceId ?? '';
  const currentNodeName = row.currentNodeName ?? row.currentActivityName;
  return {
    ...row,
    instanceId: String(id),
    ...(currentNodeName != null ? { currentNodeName: String(currentNodeName) } : {})
  };
}

/**
 * 映射引擎实例详情为前端视图字段
 * @param detail 引擎详情 DTO
 * @returns 含 instanceId、currentNodeName 等别名的详情
 */
function mapEngineDetail(detail: FlowInstanceDetail): FlowInstanceDetailView {
  const instanceId = String(detail.flowInstanceId ?? '');
  const currentNodeName = detail.currentActivityName;
  return {
    ...detail,
    instanceId,
    flowInstanceId: instanceId,
    ...(currentNodeName != null ? { currentNodeName: String(currentNodeName) } : {})
  };
}

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
function mapTodoListRow(row: FlowTodoItem & { instanceId?: string; taskId?: string; nodeName?: string }): FlowTodoTableRow {
  const instanceId = String(row.flowInstanceId ?? row.instanceId ?? '');
  return {
    ...row,
    instanceId,
    flowInstanceId: instanceId,
    taskId: String(row.flowTaskId ?? row.taskId ?? ''),
    flowTaskId: String(row.flowTaskId ?? row.taskId ?? ''),
    nodeName: row.taskName ?? row.nodeName
  };
}

/** 流程引擎运行时详情入口（与控制器路由及 workflow:*:query 权限对齐） */
export type FlowEngineDetailScope = 'todo' | 'my' | 'processed' | 'instance';

/**
 * 根据 ID 获取流程实例详情（引擎）
 * @param id 实例 ID
 * @param scope 调用入口：todo / my / processed / instance，对应 workflow:{scope}:query
 * @returns {Promise<FlowInstanceDetailView>} 详情
 */
export function getFlowEngineInstanceById(
  id: string,
  scope: FlowEngineDetailScope = 'instance'
): Promise<FlowInstanceDetailView> {
  const path = scope === 'instance' ? `${FLOW_ENGINE_API_BASE}/${id}` : `${FLOW_ENGINE_API_BASE}/${scope}/${id}`;
  return request<FlowInstanceDetail>({
    url: path,
    method: 'get',
  }).then(mapEngineDetail);
}

/**
 * 获取待办列表（分页）
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowTodoTableRow>>} 分页结果
 */
export function getFlowEngineTodoList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowTodoTableRow>> {
  return request<TaktPagedResult<FlowTodoItem>>({
    url: `${FLOW_ENGINE_API_BASE}/todo/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapTodoListRow(row as FlowTodoItem & { instanceId?: string; taskId?: string; nodeName?: string })),
  }));
}

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

/**
 * 获取我发起的流程列表
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowEngineListRow>>} 分页结果
 */
export function getFlowEngineMyList(queryDto: FlowMyInstanceQuery): Promise<TaktPagedResult<FlowEngineListRow>> {
  return request<TaktPagedResult<FlowInstanceListItem>>({
    url: `${FLOW_ENGINE_API_BASE}/my/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapEngineListRow(row as FlowInstanceListItem & ListRowLike)),
  }));
}

/**
 * 获取已办流程列表
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<FlowEngineListRow>>} 分页结果
 */
export function getFlowEngineProcessedList(queryDto: FlowTodoQuery): Promise<TaktPagedResult<FlowEngineListRow>> {
  return request<TaktPagedResult<FlowInstanceListItem>>({
    url: `${FLOW_ENGINE_API_BASE}/processed/list`,
    method: 'get',
    params: queryDto,
  }).then((res) => ({
    ...res,
    data: (res.data ?? []).map((row) => mapEngineListRow(row as FlowInstanceListItem & ListRowLike)),
  }));
}

/**
 * 发起流程
 * @param dto 发起参数
 * @returns {Promise<FlowInstanceDetailView>} 实例详情
 */
export function startFlowEngineInstance(dto: FlowStart): Promise<FlowInstanceDetailView> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/start`,
    method: 'post',
    data: dto,
  }).then(mapEngineDetail);
}

/**
 * 保存草稿
 * @param dto 发起参数
 * @returns {Promise<FlowInstanceDetailView>} 实例详情
 */
export function createFlowEngineDraft(dto: FlowStart): Promise<FlowInstanceDetailView> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/draft`,
    method: 'post',
    data: dto,
  }).then(mapEngineDetail);
}

/**
 * 从草稿启动
 * @param instanceId 实例 ID
 * @returns {Promise<FlowInstanceDetailView>} 实例详情
 */
export function startFlowEngineFromDraft(instanceId: string): Promise<FlowInstanceDetailView> {
  return request<FlowInstanceDetail>({
    url: `${FLOW_ENGINE_API_BASE}/${instanceId}/start-from-draft`,
    method: 'post',
  }).then(mapEngineDetail);
}

/**
 * 办结任务（通过/驳回）
 * @param dto 办结参数
 * @returns {Promise<void>}
 */
export function completeFlowEngineTask(dto: FlowCompleteTask): Promise<void> {
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
export function revokeFlowEngineInstance(instanceCode: string): Promise<void> {
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
export function transferFlowEngineTask(dto: FlowTransfer): Promise<void> {
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
export function addFlowEngineApprovers(dto: FlowAddApprovers): Promise<void> {
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
export function reduceFlowEngineSign(dto: FlowReduceApproval): Promise<void> {
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
export function suspendFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
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
export function resumeFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
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
export function terminateFlowEngineInstance(dto: FlowInstanceOperate): Promise<void> {
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
export function undoFlowEngineVerification(dto: FlowInstanceOperate): Promise<void> {
  return request({
    url: `${FLOW_ENGINE_API_BASE}/undo-verification`,
    method: 'post',
    data: dto,
  });
}
