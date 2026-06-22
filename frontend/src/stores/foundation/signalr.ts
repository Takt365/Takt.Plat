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
import { notify, showPrivateMessageNotify } from '@/utils/notification';
import {
  HEADER_ONLINE_AUTO_READ_MS,
  useHeaderNotificationStore,
} from '@/stores/navigation/header-notification';
import type { SignalRMessageWithId } from '@/types/common';
import type {
  BroadcastMessage,
  ForceLogoutEvent,
  OnlineUser,
  SignalRMessage,
} from '@/types/foundation/signal-r';
import type { MessageStatistics } from '@/types/foundation/message';
import type { OnlineStatistics } from '@/types/foundation/online';
import { TaktReadStatus } from '@/utils/common-enums';
import { getMessageStatistics } from '@/api/foundation/message';
import { getOnlineStatistics } from '@/api/foundation/online';
import { useTenantStore } from '@/stores/identity/tenant';
import { taktSignalRManager } from '@/utils/takt-signalr';
import { executeForceLogoutAsync } from '@/bootstrap/takt-logout-flow';
import { EventBus } from '@/utils/event-bus';
import { createLogger } from '@/utils/logger';
import { translateLocaleMessage } from '@/utils/takt-i18n-message';
import { useWorkflowTodoCountStore } from '@/stores/workflow/todo-count';
import { WORKFLOW_TABLE_NAMES } from '@/composables/use-workflow-signalr-refresh';
import { QUARTZ_TABLE_NAME } from '@/composables/use-quartz-signalr-refresh';
import type {
  FlowInstanceProgressedEvent,
  FlowSchemeChangedEvent,
  FlowTodoCountUpdatedEvent,
} from '@/types/workflow/signal-r';
import type {
  QuartzTaskChangedEvent,
  QuartzTaskExecutedEvent,
} from '@/types/foundation/quartz-signal-r';

const signalrStoreLogger = createLogger('signalr-store');

/**
 * 双 Hub 均已连接时补拉工作流待办数量（首连 / 断线重连）
 * @returns {Promise<void>}
 */
async function refreshWorkflowTodoCountAfterHubConnectedAsync(): Promise<void> {
  await useWorkflowTodoCountStore().refreshTodoCountAsync().catch((error: unknown) => {
    signalrStoreLogger.warn('重连后补拉待办数量失败', { action: 'refreshWorkflowTodoCount' }, error);
  });
}

/**
 * 分发工作流 SignalR 事件到 EventBus
 * @param schemeEvent 方案变更
 * @param instanceEvent 实例推进
 * @param todoCountEvent 待办计数
 * @returns {void}
 */
function dispatchWorkflowSignalREvents(
  schemeEvent?: FlowSchemeChangedEvent,
  instanceEvent?: FlowInstanceProgressedEvent,
  todoCountEvent?: FlowTodoCountUpdatedEvent,
): void {
  if (schemeEvent) {
    EventBus.emit('workflow:scheme:changed', schemeEvent);
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.scheme });
  }
  if (instanceEvent) {
    EventBus.emit('workflow:instance:progressed', instanceEvent);
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.todo });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.my });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.processed });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.instance });
  }
  if (todoCountEvent) {
    useWorkflowTodoCountStore().applyTodoCountFromSignalR(todoCountEvent);
    EventBus.emit('workflow:todo:count-updated', todoCountEvent);
  }
}

/**
 * 分发 Quartz SignalR 事件到 EventBus
 * @param changedEvent 定义变更
 * @param executedEvent 执行完成
 * @returns {void}
 */
function dispatchQuartzSignalREvents(
  changedEvent?: QuartzTaskChangedEvent,
  executedEvent?: QuartzTaskExecutedEvent,
): void {
  if (changedEvent) {
    EventBus.emit('foundation:quartz-task:changed', changedEvent);
    EventBus.emit('table:refresh', { tableName: QUARTZ_TABLE_NAME });
  }
  if (executedEvent) {
    EventBus.emit('foundation:quartz-task:executed', executedEvent);
    EventBus.emit('table:refresh', { tableName: QUARTZ_TABLE_NAME });
  }
}

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
    const message = event.message || translateLocaleMessage('common.tip.force.logout');
    void executeForceLogoutAsync(message);
  }

  /**
   * 连接 SignalR
   */
  async function connectSignalRAsync(): Promise<void> {
    if (connecting.value) {
      return;
    }

    const actualState = taktSignalRManager.getConnectionState();
    const actuallyConnected =
      actualState.connectHub === signalR.HubConnectionState.Connected
      && actualState.notificationHub === signalR.HubConnectionState.Connected;

    if (actuallyConnected) {
      syncConnectionState();
      void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('同步落库未读至通知中心失败', { action: 'hydratePersistedUnread' }, error);
      });
      return;
    }

    if (isConnected.value && !actuallyConnected) {
      signalrStoreLogger.warn('SignalR 缓存状态与 Hub 实际状态不一致，强制重连', {
        action: 'connect',
        cached: { connectHub: connectHubState.value, notificationHub: notificationHubState.value },
        actual: actualState,
      });
      await disconnectSignalRAsync();
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
        onConnectionStateChange: (state) => {
          connectHubState.value = state.connectHub;
          notificationHubState.value = state.notificationHub;
          if (
            state.connectHub === signalR.HubConnectionState.Disconnected
            || state.notificationHub === signalR.HubConnectionState.Disconnected
          ) {
            signalrStoreLogger.warn('SignalR Hub 已断开，等待自动重连或手动刷新', {
              action: 'connectionStateChange',
              state,
            });
            return;
          }
          if (
            state.connectHub === signalR.HubConnectionState.Connected
            && state.notificationHub === signalR.HubConnectionState.Connected
          ) {
            void refreshWorkflowTodoCountAfterHubConnectedAsync();
          }
        },
        onReceiveMessage: (msg) => {
          messages.value.push(msg);
          const sender = msg.fromUserName?.trim() || '?';
          const body = msg.messageContent?.trim() || '';
          if (!body && !msg.messageTitle?.trim()) {
            signalrStoreLogger.warn('收到空内容私信', { action: 'receiveMessage', msg });
          }
          showPrivateMessageNotify({
            sender,
            content: body,
            title: msg.messageTitle?.trim(),
            messageId: msg.messageId,
            sendTime: msg.sendTime,
          });
          EventBus.emit('foundation:message:received', msg);
          signalrStoreLogger.info('私信已送达客户端', {
            action: 'receiveMessage',
            fromUserName: msg.fromUserName,
            messageId: msg.messageId,
          });
        },
        onReceiveBroadcast: (msg) => {
          broadcastMessages.value.push(msg);
          notify({
            type: 'info',
            message: msg.messageTitle?.trim() || translateLocaleMessage('common.page.signalr.new.message'),
            description: msg.messageContent,
            duration: 5,
          });
        },
        onMessageRead: (event) => {
          const target = messages.value.find(
            (item) => String((item as SignalRMessageWithId).messageId) === String(event.messageId)
          );

          if (target) {
            target.readStatus = TaktReadStatus.Read;
          }
          useHeaderNotificationStore().markPersistedReadByMessageId(String(event.messageId));
        },
        onOnlineStatisticsUpdated: applyOnlineStatistics,
        onMessageStatisticsUpdated: applyMessageStatistics,
        onFlowSchemeChanged: (event) => {
          dispatchWorkflowSignalREvents(event);
        },
        onFlowInstanceProgressed: (event) => {
          dispatchWorkflowSignalREvents(undefined, event);
        },
        onFlowTodoCountUpdated: (event) => {
          dispatchWorkflowSignalREvents(undefined, undefined, event);
        },
        onQuartzTaskChanged: (event) => {
          dispatchQuartzSignalREvents(event);
        },
        onQuartzTaskExecuted: (event) => {
          dispatchQuartzSignalREvents(undefined, event);
        },
        onOnlineMessage: (event) => {
          notify({
            type: 'success',
            message: translateLocaleMessage('common.feedback.connect.success'),
            description: String(event.message ?? ''),
            duration: 5,
            center: {
              kind: 'online',
              autoMarkReadAfterMs: HEADER_ONLINE_AUTO_READ_MS,
            },
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
            message: error.message || translateLocaleMessage('common.feedback.signalr.error'),
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

      void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('同步落库未读至通知中心失败', { action: 'hydratePersistedUnread' }, error);
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
    useWorkflowTodoCountStore().resetTodoCount();
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
    useWorkflowTodoCountStore().resetTodoCount();
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
