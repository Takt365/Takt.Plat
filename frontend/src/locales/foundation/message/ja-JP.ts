// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：ja-JP.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：オンラインメッセージページ静的文案；参照キー foundation.message.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    listScope: {
      all: 'すべてのメッセージ',
      unread: '未読（自分）',
      read: '既読（自分）',
    },
    recipient: {
      all: '現在の会社の全ユーザー',
      list: '指定ユーザー',
      listSelect: '受信ユーザー',
      listPlaceholder: '受信ユーザーを選択（最大5人）',
      listRequired: '受信ユーザーを1人以上選択してください',
      listMax: '受信ユーザーは最大 {max} 人まで選択できます',
      sendToAllForbidden: '全員送信はスーパー管理者のみ利用できます',
      broadcastSuccess: 'ブロードキャストを送信しました',
    },
    upload: {
      select: 'ファイルを選択',
      imageHint: '画像をアップロードすると添付リンクが自動入力されます',
      fileHint: 'ファイルをアップロード（大容量は分割アップロード）',
      videoHint: '動画ファイルをアップロード',
      voiceHint: '音声ファイルをアップロード',
      success: '添付のアップロードに成功しました',
      failed: '添付のアップロードに失敗しました',
      required: '先に添付をアップロードしてください',
      contentOptional: 'テキスト説明（任意）',
    },
  },
};
