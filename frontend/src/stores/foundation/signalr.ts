// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/foundation
// 文件名称：signalr.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 状态管理（连接、消息、在线用户、强退）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import * as signalR from '@microsoft/signalr';
import { notification } from 'ant-design-vue';
import type { SignalRMessageWithId } from '@/types/common';
import type {
  BroadcastMessage,
  ForceLogoutEvent,
  OnlineUser,
  SignalRMessage,
} from '@/types/foundation/signal-r';
import type { MessageStatistics } from '@/types/foundation/message';
import type { OnlineStatistics } from '@/types/foundation/online';
import { TaktMessageReadStatus } from '@/utils/foundation-enums';
import { getMessageStatistics } from '@/api/foundation/message';
import { getOnlineStatistics } from '@/api/foundation/online';
import { useTenantStore } from '@/stores/identity/tenant';
import { taktSignalRManager } from '@/utils/takt-signalr';
import { EventBus } from '@/utils/event-bus';
import { createLogger } from '@/utils/logger';
import { translateLocaleMessage } from '@/utils/takt-i18n-message';
import {
  STORE_I18N_FEEDBACK_CONNECT_SUCCESS,
  STORE_I18N_FEEDBACK_SIGNALR_ERROR,
  STORE_I18N_TIP_FORCE_LOGOUT,
} from '@/utils/takt-store-i18n';

const signalrStoreLogger = createLogger('signalr-store');

/**
 * SignalR 状态管理
 */
export const useSignalRStore = defineStore('signalr', () => {
  const connectHubState = ref<signalR.HubConnectionState>(signalR.HubConnectionState.Disconnected);
  const notificationHubState = ref<signalR.HubConnectionState>(signalR.HubConnectionState.Disconnected);
  const onlineUsers = ref<OnlineUser[]>([]);
  const unreadCount = ref(0);
  const onlineStatistics = ref<OnlineStatistics | null>(null);
  const messageStatistics = ref<MessageStatistics | null>(null);
  const messages = ref<SignalRMessage[]>([]);
  const broadcastMessages = ref<BroadcastMessage[]>([]);
  const connecting = ref(false);

  /**
   * 双 Hub 是否均已连接
   */
  const isConnected = computed(
    () =>
      connectHubState.value === signalR.HubConnectionState.Connected &&
      notificationHubState.value === signalR.HubConnectionState.Connected
  );

  /**
   * 同步 Hub 连接状态
   */
  function syncConnectionState(): void {
    const state = taktSignalRManager.getConnectionState();
    connectHubState.value = state.connectHub;
    notificationHubState.value = state.notificationHub;
  }

  /**
   * 刷新在线用户
   */
  async function refreshOnlineUsersAsync(): Promise<void> {
    onlineUsers.value = await taktSignalRManager.getOnlineUsersAsync();
  }

  /**
   * 刷新未读消息数
   */
  async function refreshUnreadCountAsync(): Promise<void> {
    unreadCount.value = await taktSignalRManager.getUnreadCountAsync();
  }

  /**
   * 应用在线统计（SignalR 推送或 HTTP 拉取）
   * @param statistics 统计 DTO
   */
  function applyOnlineStatistics(statistics: OnlineStatistics): void {
    onlineStatistics.value = statistics;
  }

  /**
   * 应用消息统计（SignalR 推送或 HTTP 拉取）
   * @param statistics 统计 DTO
   */
  function applyMessageStatistics(statistics: MessageStatistics): void {
    messageStatistics.value = statistics;
    unreadCount.value = statistics.unreadCount;
  }

  /**
   * 通过 HTTP 拉取在线统计（Hub 未连接时的兜底）
   */
  async function refreshOnlineStatisticsAsync(): Promise<void> {
    applyOnlineStatistics(await getOnlineStatistics());
  }

  /**
   * 通过 HTTP 拉取消息统计（Hub 未连接时的兜底）
   */
  async function refreshMessageStatisticsAsync(): Promise<void> {
    applyMessageStatistics(await getMessageStatistics());
  }

  /**
   * 处理强退事件
   */
  function handleForceLogout(event: ForceLogoutEvent): void {
    EventBus.emit('notification:show', {
      type: 'warning',
      message: event.message || translateLocaleMessage(STORE_I18N_TIP_FORCE_LOGOUT),
    });
    EventBus.emit('user:logout', undefined);
  }

  /**
   * 连接 SignalR
   */
  async function connectSignalRAsync(): Promise<void> {
    if (connecting.value || isConnected.value) {
      return;
    }

    const tenantStore = useTenantStore();
    if (!tenantStore.tenantCode.trim() || !tenantStore.companyCode.trim()) {
      signalrStoreLogger.warn('SignalR 跳过连接：租户或公司上下文未就绪', {
        action: 'connect',
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
      });
      return;
    }

    connecting.value = true;

    try {
      await taktSignalRManager.connectSignalRHubsAsync({
        onReceiveMessage: (msg) => {
          messages.value.push(msg);

          EventBus.emit('notification:show', {
            type: 'info',
            message: `${msg.fromUserName}: ${msg.messageContent}`,
          });
        },
        onReceiveBroadcast: (msg) => {
          broadcastMessages.value.push(msg);

          EventBus.emit('notification:show', {
            type: 'info',
            message: msg.messageContent,
            description: msg.messageTitle,
          });
        },
        onMessageRead: (event) => {
          const target = messages.value.find(
            (item) => String((item as SignalRMessageWithId).messageId) === String(event.messageId)
          );

          if (target) {
            target.readStatus = TaktMessageReadStatus.Read;
          }
        },
        onOnlineStatisticsUpdated: applyOnlineStatistics,
        onMessageStatisticsUpdated: applyMessageStatistics,
        onOnlineMessage: (event) => {
          notification.success({
            message: translateLocaleMessage(STORE_I18N_FEEDBACK_CONNECT_SUCCESS),
            description: String(event.message ?? ''),
            placement: 'topRight',
            duration: 5,
          });
        },
        onUserConnected: () => {
          void refreshOnlineUsersAsync().catch(() => undefined);
        },
        onUserDisconnected: () => {
          void refreshOnlineUsersAsync().catch(() => undefined);
        },
        onForceLogout: handleForceLogout,
        onError: (error) => {
          EventBus.emit('notification:show', {
            type: 'error',
            message: error.message || translateLocaleMessage(STORE_I18N_FEEDBACK_SIGNALR_ERROR),
          });
        },
      });

      syncConnectionState();

      void refreshOnlineUsersAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取在线用户失败', { action: 'refreshOnlineUsers' }, error);
      });

      void refreshUnreadCountAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取未读消息数失败', { action: 'refreshUnreadCount' }, error);
      });

      void refreshOnlineStatisticsAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取在线统计失败', { action: 'refreshOnlineStatistics' }, error);
      });

      void refreshMessageStatisticsAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取消息统计失败', { action: 'refreshMessageStatistics' }, error);
      });
    } catch (error) {
      EventBus.emit('notification:show', {
        type: 'error',
        message: error instanceof Error ? error.message : 'SignalR 连接失败',
      });

      throw error;
    } finally {
      connecting.value = false;
    }
  }

  /**
   * 公司切换后重连 SignalR（携带新的 X-Company-Code / company_code）
   */
  async function reconnectSignalRAsync(): Promise<void> {
    await disconnectSignalRAsync();
    await connectSignalRAsync();
  }

  /**
   * 断开 SignalR
   */
  async function disconnectSignalRAsync(): Promise<void> {
    await taktSignalRManager.disconnectSignalRHubsAsync();
    syncConnectionState();
    onlineUsers.value = [];
    messages.value = [];
    broadcastMessages.value = [];
    unreadCount.value = 0;
    onlineStatistics.value = null;
    messageStatistics.value = null;
  }

  /**
   * 发送私信
   */
  async function sendSignalRMessageAsync(
    toUserName: string,
    messageContent: string,
    messageTitle?: string
  ): Promise<void> {
    await taktSignalRManager.sendMessageAsync(toUserName, messageContent, messageTitle);
  }

  /**
   * 发送广播
   */
  async function broadcastSignalRMessageAsync(messageContent: string, messageTitle?: string): Promise<void> {
    await taktSignalRManager.broadcastMessageAsync(messageContent, messageTitle);
  }

  /**
   * 标记消息已读
   * @param messageId 消息 ID
   */
  async function markSignalRMessageAsReadAsync(messageId: number): Promise<void> {
    await taktSignalRManager.markMessageAsReadAsync(messageId);
  }

  /**
   * 重置 SignalR 状态
   */
  function resetSignalRState(): void {
    connectHubState.value = signalR.HubConnectionState.Disconnected;
    notificationHubState.value = signalR.HubConnectionState.Disconnected;
    onlineUsers.value = [];
    unreadCount.value = 0;
    onlineStatistics.value = null;
    messageStatistics.value = null;
    messages.value = [];
    broadcastMessages.value = [];
    connecting.value = false;
  }

  return {
    connectHubState,
    notificationHubState,
    onlineUsers,
    unreadCount,
    onlineStatistics,
    messageStatistics,
    messages,
    broadcastMessages,
    connecting,
    isConnected,
    connectSignalRAsync,
    reconnectSignalRAsync,
    disconnectSignalRAsync,
    refreshOnlineUsersAsync,
    refreshUnreadCountAsync,
    refreshOnlineStatisticsAsync,
    refreshMessageStatisticsAsync,
    sendSignalRMessageAsync,
    broadcastSignalRMessageAsync,
    markSignalRMessageAsReadAsync,
    resetSignalRState,
    syncConnectionState,
  };
});
