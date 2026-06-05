// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/utils
// 文件名称：takt-signalr.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 双 Hub 连接管理（TaktConnectHub / TaktNotificationHub）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import * as signalR from '@microsoft/signalr';
import type {
  BroadcastMessage,
  ForceLogoutEvent,
  MessageReadEvent,
  MessageSentEvent,
  OnlineMessageEvent,
  OnlineUser,
  SignalRErrorEvent,
  SignalRMessage,
  UserConnectedEvent,
  UserDisconnectedEvent,
} from '@/types/foundation/signal-r';
import type { MessageStatistics } from '@/types/foundation/message';
import type { OnlineStatistics } from '@/types/foundation/online';
import { TaktMessageGroup, TaktMessageType } from '@/utils/foundation-enums';
import { useUserStore } from '@/stores/identity/user';
import { useTenantStore } from '@/stores/identity/tenant';
import { getAppOrigin, joinOriginPath, requireViteEnv } from '@/config/vite-env';
import { refreshOAuthTokens } from '@/utils/oauth';
import { createLogger } from '@/utils/logger';

const signalrLogger = createLogger('signalr');

/**
 * SignalR 事件回调
 */
export interface TaktSignalRCallbacks {
  /**
   * 用户连接
   */
  onUserConnected?: (event: UserConnectedEvent) => void;

  /**
   * 用户断开
   */
  onUserDisconnected?: (event: UserDisconnectedEvent) => void;

  /**
   * 收到私信
   */
  onReceiveMessage?: (message: SignalRMessage) => void;

  /**
   * 收到广播
   */
  onReceiveBroadcast?: (message: BroadcastMessage) => void;

  /**
   * 消息已发送
   */
  onMessageSent?: (event: MessageSentEvent) => void;

  /**
   * 消息已读
   */
  onMessageRead?: (event: MessageReadEvent) => void;

  /**
   * Hub 错误
   */
  onError?: (error: SignalRErrorEvent) => void;

  /**
   * 上线通知
   */
  onOnlineMessage?: (event: OnlineMessageEvent) => void;

  /**
   * 强退下线
   */
  onForceLogout?: (event: ForceLogoutEvent) => void;

  /**
   * 在线统计实时更新
   */
  onOnlineStatisticsUpdated?: (statistics: OnlineStatistics) => void;

  /**
   * 消息统计实时更新
   */
  onMessageStatisticsUpdated?: (statistics: MessageStatistics) => void;
}

/**
 * 拼接 SignalR Hub 完整 URL（VITE_APP_ORIGIN + Hub 路径环境变量）
 * @param hubPathEnvKey Hub 路径环境变量名
 */
function buildSignalRHubUrl(hubPathEnvKey: 'VITE_SIGNALR_HUB_CONNECT_PATH' | 'VITE_SIGNALR_HUB_NOTIFICATION_PATH'): string {
  return joinOriginPath(getAppOrigin(), requireViteEnv(hubPathEnvKey));
}

/**
 * 构建 SignalR 请求头（negotiate / long polling）
 * @returns 租户与公司请求头
 */
function buildSignalRContextHeaders(): Record<string, string> {
  const tenantStore = useTenantStore();
  const headers: Record<string, string> = {};
  const tenantCode = tenantStore.tenantCode.trim();
  const companyCode = tenantStore.companyCode.trim();
  if (tenantCode) {
    headers['X-Tenant-Code'] = tenantCode;
  }
  if (companyCode) {
    headers['X-Company-Code'] = companyCode;
  }
  return headers;
}

/**
 * 为 Hub URL 附加 tenant_code / company_code 查询参数（WebSocket 无法自定义 Header）
 * @param hubUrl Hub 完整 URL
 * @returns 带上下文查询参数的 URL
 */
function appendSignalRContextQuery(hubUrl: string): string {
  const tenantStore = useTenantStore();
  const url = new URL(hubUrl);
  const tenantCode = tenantStore.tenantCode.trim();
  const companyCode = tenantStore.companyCode.trim();
  if (tenantCode) {
    url.searchParams.set('tenant_code', tenantCode);
  }
  if (companyCode) {
    url.searchParams.set('company_code', companyCode);
  }
  return url.toString();
}

/**
 * SignalR 连接管理器
 */
export class TaktSignalRManager {
  private connectHub: signalR.HubConnection | null = null;

  private notificationHub: signalR.HubConnection | null = null;

  private heartbeatTimer: number | null = null;

  private readonly heartbeatIntervalMs = 30_000;

  /**
   * 创建 Hub 连接
   * @param hubPath Hub 路径
   */
  private createHubConnection(hubUrl: string): signalR.HubConnection {
    const contextualUrl = appendSignalRContextQuery(hubUrl);
    const headers = buildSignalRContextHeaders();

    return new signalR.HubConnectionBuilder()
      .withUrl(contextualUrl, {
        withCredentials: true,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        accessTokenFactory: () => {
          const token = useUserStore().token;
          return Promise.resolve(token || '');
        },
        headers,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  /**
   * 注册 Connect Hub 事件
   * @param callbacks 回调
   */
  private registerConnectHubEvents(callbacks?: TaktSignalRCallbacks): void {
    if (!this.connectHub) {
      return;
    }

    this.connectHub.on('OnlineMessage', (event: OnlineMessageEvent) => {
      callbacks?.onOnlineMessage?.(event);
    });

    this.connectHub.on('UserConnected', (event: UserConnectedEvent) => {
      callbacks?.onUserConnected?.(event);
    });

    this.connectHub.on('UserDisconnected', (event: UserDisconnectedEvent) => {
      callbacks?.onUserDisconnected?.(event);
    });

    this.connectHub.on('ForceLogout', (event: ForceLogoutEvent) => {
      callbacks?.onForceLogout?.(event);
    });

    this.connectHub.on('OnlineStatisticsUpdated', (statistics: OnlineStatistics) => {
      callbacks?.onOnlineStatisticsUpdated?.(statistics);
    });

    this.connectHub.on('MessageStatisticsUpdated', (statistics: MessageStatistics) => {
      callbacks?.onMessageStatisticsUpdated?.(statistics);
    });

    this.connectHub.onclose(async (error) => {
      this.stopHeartbeat();

      if (error?.message?.includes('401') || error?.message?.includes('Unauthorized')) {
        await refreshOAuthTokens();
      }
    });
  }

  /**
   * 注册 Notification Hub 事件
   * @param callbacks 回调
   */
  private registerNotificationHubEvents(callbacks?: TaktSignalRCallbacks): void {
    if (!this.notificationHub) {
      return;
    }

    this.notificationHub.on('OnlineMessage', (event: OnlineMessageEvent) => {
      callbacks?.onOnlineMessage?.(event);
    });

    this.notificationHub.on('ReceiveMessage', (message: SignalRMessage) => {
      callbacks?.onReceiveMessage?.(message);
    });

    this.notificationHub.on('ReceiveBroadcast', (message: BroadcastMessage) => {
      callbacks?.onReceiveBroadcast?.(message);
    });

    this.notificationHub.on('MessageSent', (event: MessageSentEvent) => {
      callbacks?.onMessageSent?.(event);
    });

    this.notificationHub.on('MessageRead', (event: MessageReadEvent) => {
      callbacks?.onMessageRead?.(event);
    });

    this.notificationHub.on('Error', (error: SignalRErrorEvent) => {
      callbacks?.onError?.(error);
    });

    this.notificationHub.on('ForceLogout', (event: ForceLogoutEvent) => {
      callbacks?.onForceLogout?.(event);
    });

    this.notificationHub.on('OnlineStatisticsUpdated', (statistics: OnlineStatistics) => {
      callbacks?.onOnlineStatisticsUpdated?.(statistics);
    });

    this.notificationHub.on('MessageStatisticsUpdated', (statistics: MessageStatistics) => {
      callbacks?.onMessageStatisticsUpdated?.(statistics);
    });
  }

  /**
   * 启动心跳
   */
  private startHeartbeat(): void {
    if (this.heartbeatTimer) {
      return;
    }

    this.heartbeatTimer = window.setInterval(() => {
      if (this.connectHub?.state === signalR.HubConnectionState.Connected) {
        void this.connectHub.invoke('Heartbeat').catch((error: unknown) => {
          signalrLogger.warn('SignalR 心跳失败', { action: 'heartbeat' }, error);
        });
      }
    }, this.heartbeatIntervalMs);
  }

  /**
   * 停止心跳
   */
  private stopHeartbeat(): void {
    if (this.heartbeatTimer) {
      window.clearInterval(this.heartbeatTimer);
      this.heartbeatTimer = null;
    }
  }

  /**
   * 连接所有 Hub
   * @param callbacks 事件回调
   */
  async connectSignalRHubsAsync(callbacks?: TaktSignalRCallbacks): Promise<void> {
    await this.disconnectSignalRHubsAsync();
    this.connectHub = this.createHubConnection(buildSignalRHubUrl('VITE_SIGNALR_HUB_CONNECT_PATH'));
    this.notificationHub = this.createHubConnection(buildSignalRHubUrl('VITE_SIGNALR_HUB_NOTIFICATION_PATH'));

    this.registerConnectHubEvents(callbacks);
    this.registerNotificationHubEvents(callbacks);

    await this.connectHub.start();
    await this.notificationHub.start();

    this.startHeartbeat();
    signalrLogger.info('SignalR Hub 已全部连接');
  }

  /**
   * 断开所有 Hub
   */
  async disconnectSignalRHubsAsync(): Promise<void> {
    this.stopHeartbeat();

    const tasks: Promise<void>[] = [];

    if (this.connectHub) {
      tasks.push(this.connectHub.stop());
      this.connectHub = null;
    }

    if (this.notificationHub) {
      tasks.push(this.notificationHub.stop());
      this.notificationHub = null;
    }

    await Promise.allSettled(tasks);
    signalrLogger.info('SignalR Hub 已全部断开');
  }

  /**
   * 获取在线用户列表
   */
  async getOnlineUsersAsync(): Promise<OnlineUser[]> {
    if (!this.connectHub || this.connectHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Connect Hub 未连接');
    }

    const users = await this.connectHub.invoke<OnlineUser[]>('GetOnlineUsers');
    return users ?? [];
  }

  /**
   * 获取未读消息数量
   */
  async getUnreadCountAsync(): Promise<number> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    const count = await this.notificationHub.invoke<number>('GetUnreadCount');
    return count ?? 0;
  }

  /**
   * 发送私信
   */
  async sendMessageAsync(
    toUserName: string,
    messageContent: string,
    messageTitle?: string,
    messageType: TaktMessageType = TaktMessageType.UserMessage,
    messageGroup?: TaktMessageGroup,
    messageExtData?: string
  ): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke(
      'SendMessage',
      toUserName,
      messageContent,
      messageTitle,
      messageType,
      messageGroup,
      messageExtData
    );
  }

  /**
   * 发送广播
   */
  async broadcastMessageAsync(
    messageContent: string,
    messageTitle?: string,
    messageType: TaktMessageType = TaktMessageType.SystemNotice,
    messageGroup: TaktMessageGroup = TaktMessageGroup.Notification
  ): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke(
      'BroadcastMessage',
      messageContent,
      messageTitle,
      messageType,
      messageGroup
    );
  }

  /**
   * 标记消息已读
   * @param messageId 消息 ID
   */
  async markMessageAsReadAsync(messageId: number): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke('MarkAsRead', messageId);
  }

  /**
   * 获取 Hub 连接状态
   */
  getConnectionState(): {
    connectHub: signalR.HubConnectionState;
    notificationHub: signalR.HubConnectionState;
  } {
    return {
      connectHub: this.connectHub?.state ?? signalR.HubConnectionState.Disconnected,
      notificationHub: this.notificationHub?.state ?? signalR.HubConnectionState.Disconnected,
    };
  }

  /**
   * 是否已全部连接
   */
  isConnected(): boolean {
    const state = this.getConnectionState();

    return (
      state.connectHub === signalR.HubConnectionState.Connected &&
      state.notificationHub === signalR.HubConnectionState.Connected
    );
  }
}

/** SignalR 管理器单例 */
export const taktSignalRManager = new TaktSignalRManager();
