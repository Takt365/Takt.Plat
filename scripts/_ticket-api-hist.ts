// ========================================
// 椤圭洰鍚嶇О锛氳妭鎷嶅伐鍘偮稵akt Plat
// 鍛藉悕绌洪棿锛歠rontend/src/api/routine/help-desk
// 鏂囦欢鍚嶇О锛歵icket.ts
// 鍒涘缓鏃堕棿锛?026-07-09
// 鍒涘缓浜猴細Takt365(Auto Generated)
// 鍔熻兘鎻忚堪锛歳outine/help-desk 妯″潡 API锛堣嚜鍔ㄧ敓鎴愶紝璇峰嬁鎵嬫敼璺敱甯搁噺锛?// 
// 鐗堟潈淇℃伅锛欳opyright (c) 2025 Takt  All rights reserved.
// 鍏嶈矗澹版槑锛氭杞欢浣跨敤 MIT License锛屼綔鑰呬笉鎵挎媴浠讳綍浣跨敤椋庨櫓銆?// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common';
import type {
  HelpDeskTicketStat
} from '@/types/routine/help-desk/help-desk-ticket-stat';
import type {
  HelpDeskTicketStatQuery
} from '@/types/routine/help-desk/help-desk-ticket-stat-query';
import type {
  MyTicketReply
} from '@/types/routine/help-desk/my-ticket-reply';
import type {
  Ticket,
  TicketCreate,
  TicketStatus,
  TicketUpdate
} from '@/types/routine/help-desk/ticket';
import type {
  TicketAssign
} from '@/types/routine/help-desk/ticket-assign';
import type {
  TicketCreateFromChannel
} from '@/types/routine/help-desk/ticket-create-from-channel';
import type {
  TicketMyAsset
} from '@/types/routine/help-desk/ticket-my-asset';
import type {
  TicketReply
} from '@/types/routine/help-desk/ticket-reply';
import type {
  TicketSessionReplyCreate
} from '@/types/routine/help-desk/ticket-session-reply-create';
import type {
  TicketSubmit
} from '@/types/routine/help-desk/ticket-submit';
import type {
  TicketWorkflowAction
} from '@/types/routine/help-desk/ticket-workflow-action';

/**
 * API 璺緞鍓嶇紑锛堢浉瀵?request baseURL锛屽搴斿悗绔?[controller]锛? * @description TaktTickets
 */
const TICKET_API_BASE = 'TaktTickets';

// ========================================
// 鍩虹 CRUD
// ========================================

/**
 * 鑾峰彇宸ュ崟鍒楄〃锛堝垎椤碉級
 * @param {any} queryDto 鏌ヨDTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 鍒嗛〉缁撴灉
 */
export function getTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 鑾峰彇鏈嶅姟鍙板伐鍗曠粺璁★紙鏁版嵁鐪嬫澘锛? * @param {HelpDeskTicketStatQuery} queryDto 鏌ヨ DTO
 * @returns {Promise<HelpDeskTicketStat>} 鏈嶅姟鍙板伐鍗曠粺璁? */
export function getHelpDeskTicketStat(queryDto: HelpDeskTicketStatQuery): Promise<HelpDeskTicketStat> {
  return request<HelpDeskTicketStat>({
    url: `${TICKET_API_BASE}/ticket-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 鑾峰彇褰撳墠鐢ㄦ埛鎻愪氦鐨勫伐鍗曞垪琛紙鍒嗛〉锛? * @param {any} queryDto 鏌ヨ DTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 鍒嗛〉缁撴灉
 */
export function getMyTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/my-tickets`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 鑾峰彇褰撳墠鐢ㄦ埛鎻愪氦鐨勫伐鍗曡鎯? * @param {string} id 宸ュ崟 ID
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function getMyTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 鑾峰彇褰撳墠鐢ㄦ埛宸ュ崟鐨勫洖澶嶅垪琛紙鍒嗛〉锛屼笉鍚唴閮ㄥ娉級
 * @param {string} id 宸ュ崟 ID
 * @param {any} queryDto 鍒嗛〉鏌ヨ
 * @returns {Promise<TaktPagedResult<MyTicketReply>>} 鍒嗛〉缁撴灉
 */
export function getMyTicketReplyList(id: string, queryDto: any): Promise<TaktPagedResult<MyTicketReply>> {
  return request<TaktPagedResult<MyTicketReply>>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}/replies`,
    method: 'get',
    params: {
      id,
      ...queryDto
    },
  });
}

/**
 * 闂ㄦ埛鐢ㄦ埛鍥炲鑷繁鐨勫伐鍗? * @param {string} id 宸ュ崟 ID
 * @param {TicketSessionReplyCreate} dto 鍥炲 DTO
 * @returns {Promise<TicketSessionReplyCreate>} 鍥炲 DTO
 */
export function replyMyTicket(id: string, dto: TicketSessionReplyCreate): Promise<TicketSessionReplyCreate> {
  return request<TicketSessionReplyCreate>({
    url: `${TICKET_API_BASE}/my-tickets/{id:long}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 鏍规嵁ID鑾峰彇宸ュ崟
 * @param {string} id 宸ュ崟ID
 * @returns {Promise<Ticket>} 宸ュ崟DTO
 */
export function getTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 鍒涘缓宸ュ崟
 * @param {TicketCreate} dto 鍒涘缓DTO
 * @returns {Promise<Ticket>} 宸ュ崟DTO
 */
export function createTicket(dto: TicketCreate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 鏇存柊宸ュ崟
 * @param {string} id 宸ュ崟ID
 * @param {TicketUpdate} dto 鏇存柊DTO
 * @returns {Promise<Ticket>} 宸ュ崟DTO
 */
export function updateTicket(id: string, dto: TicketUpdate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/{id:long}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 鍒犻櫎宸ュ崟
 * @param {string} id 宸ュ崟ID
 * @returns {Promise<void>} 鎿嶄綔缁撴灉
 */
export function deleteTicketById(id: string): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/{id:long}`,
    method: 'delete',
    params: {
      id
    },
  });
}

/**
 * 鎵归噺鍒犻櫎宸ュ崟
 * @param {string[]} ids ID鍒楄〃
 * @returns {Promise<void>} 鎿嶄綔缁撴灉
 */
export function deleteTicketBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 闂ㄦ埛鐢ㄦ埛鎻愪氦宸ュ崟
 * @param {TicketSubmit} dto 鎻愪氦 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function submitTicket(dto: TicketSubmit): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/submit`,
    method: 'post',
    data: dto,
  });
}

/**
 * 閭欢/API 娓犻亾寤哄崟
 * @param {TicketCreateFromChannel} dto 娓犻亾寤哄崟 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function createTicketFromChannel(dto: TicketCreateFromChannel): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/channel`,
    method: 'post',
    data: dto,
  });
}

/**
 * 寮€濮嬪鐞嗗伐鍗? * @param {TicketWorkflowAction} dto 鍔ㄤ綔 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function startTicketProgress(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/start`,
    method: 'post',
    data: dto,
  });
}

/**
 * 绛夊緟鐢ㄦ埛鍥炲
 * @param {TicketWorkflowAction} dto 鍔ㄤ綔 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function waitForRequester(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/wait`,
    method: 'post',
    data: dto,
  });
}

/**
 * 鏍囪宸ュ崟宸茶В鍐? * @param {TicketWorkflowAction} dto 鍔ㄤ綔 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function resolveTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/resolve`,
    method: 'post',
    data: dto,
  });
}

/**
 * 鐢ㄦ埛纭鍏抽棴宸ュ崟
 * @param {TicketWorkflowAction} dto 鍔ㄤ綔 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function confirmCloseTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/confirm-close`,
    method: 'post',
    data: dto,
  });
}

/**
 * 閲嶆柊鎵撳紑宸ュ崟
 * @param {TicketWorkflowAction} dto 鍔ㄤ綔 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function reopenTicket(dto: TicketWorkflowAction): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/reopen`,
    method: 'post',
    data: dto,
  });
}

/**
 * 娣诲姞宸ュ崟鍥炲锛堜細璇濓級
 * @param {TicketSessionReplyCreate} dto 鍥炲 DTO
 * @returns {Promise<TicketReply>} 鍥炲 DTO
 */
export function replyTicket(dto: TicketSessionReplyCreate): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_API_BASE}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 鑾峰彇宸ュ崟鍥炲鍒楄〃锛堝垎椤碉級
 * @param {any} queryDto 鏌ヨ DTO
 * @returns {Promise<TaktPagedResult<TicketReply>>} 鍒嗛〉缁撴灉
 */
export function getTicketReplyList(queryDto: any): Promise<TaktPagedResult<TicketReply>> {
  return request<TaktPagedResult<TicketReply>>({
    url: `${TICKET_API_BASE}/replies`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 鑾峰彇褰撳墠鐢ㄦ埛宸ュ崟鍏宠仈璧勪骇姹囨€? * @param {any} queryDto 鍒嗛〉鏌ヨ
 * @returns {Promise<TaktPagedResult<TicketMyAsset>>} 鍒嗛〉缁撴灉
 */
export function getMyAssetList(queryDto: any): Promise<TaktPagedResult<TicketMyAsset>> {
  return request<TaktPagedResult<TicketMyAsset>>({
    url: `${TICKET_API_BASE}/my-assets`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 鏇存柊宸ュ崟鐘舵€? * @param {TicketStatus} dto 鐘舵€?DTO
 * @returns {Promise<Ticket>} 宸ュ崟DTO
 */
export function updateTicketStatus(dto: TicketStatus): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 閫夐」
// ========================================

/**
 * 鑾峰彇宸ュ崟閫夐」鍒楄〃
 * @returns {Promise<TaktSelectOption[]>} 涓嬫媺閫夐」
 */
export function getTicketOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TICKET_API_BASE}/options`,
    method: 'get',
  });
}

// ========================================
// 鍏宠仈鍒嗛厤
// ========================================

/**
 * 鎸囨淳鎴栭鍙栧伐鍗? * @param {TicketAssign} dto 鎸囨淳 DTO
 * @returns {Promise<Ticket>} 宸ュ崟 DTO
 */
export function assignTicket(dto: TicketAssign): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/assign`,
    method: 'post',
    data: dto,
  });
}

// ========================================
// 瀵煎叆瀵煎嚭
// ========================================

/**
 * 鑾峰彇瀵煎叆妯℃澘
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel鏂囦欢
 */
export function getTicketTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 瀵煎叆宸ュ崟
 * @param {globalThis.File} file Excel鏂囦欢
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 瀵煎叆缁撴灉
 */
export function importTicket(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 瀵煎嚭宸ュ崟
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel鏂囦欢
 */
export function exportTicket(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
