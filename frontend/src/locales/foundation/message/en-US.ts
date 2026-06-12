// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：en-US.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Online message page static copy; keys foundation.message.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    listScope: {
      all: 'All Messages',
      unread: 'My Unread',
      read: 'My Read',
    },
    recipient: {
      all: 'All users in current company',
      list: 'Selected users',
      listSelect: 'Recipients',
      listPlaceholder: 'Select recipients (up to 5)',
      listRequired: 'Select at least one recipient',
      listMax: 'You can select at most {max} recipients',
      sendToAllForbidden: 'Broadcast is available to super administrators only',
      broadcastSuccess: 'Broadcast sent',
    },
    upload: {
      select: 'Select file',
      imageHint: 'Upload an image; the attachment URL will be filled automatically',
      fileHint: 'Upload a file; large files use chunked upload',
      videoHint: 'Upload a video file',
      voiceHint: 'Upload an audio file',
      success: 'Attachment uploaded',
      failed: 'Attachment upload failed',
      required: 'Please upload an attachment first',
      contentOptional: 'Optional text description',
    },
  },
};
