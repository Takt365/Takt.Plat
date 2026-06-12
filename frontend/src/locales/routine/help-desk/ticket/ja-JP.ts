// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/routine/help-desk/ticket
// 文件名称：ja-JP.ts
// 功能描述：チケットページ静的文案；引用键 routine.help-desk.ticket.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    workflowTitle: 'チケット処理',
    ticketNoAuto: '空欄の場合は自動採番',
    replies: '会話履歴',
    replyPlaceholder: '返信内容を入力',
    internalNote: '内部メモ（ユーザー非表示）',
    status: {
      open: '新規',
      assigned: '割当済',
      inprogress: '処理中',
      waiting: 'ユーザー返信待ち',
      resolved: '解決済',
      closed: 'クローズ',
      reopened: '再オープン',
    },
    action: {
      pick: '受取して開始',
      assign: '割当',
      start: '処理開始',
      wait: 'ユーザー返信待ち',
      resolve: '解決済にする',
      confirmClose: 'クローズ確認',
      reopen: '再オープン',
      reply: '返信送信',
    },
    author: {
      agent: '担当者',
      requester: 'ユーザー',
      system: 'システム',
    },
  },
};
