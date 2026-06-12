// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/common
// 文件名称：ja-JP.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：通用日文语言包（仅登录壳/主题等前端静态文案；业务通用项见后端动态种子 common.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    app: {
      title: 'Takt Plat',
      name: 'Takt Plat',
      productcode: 'TP-MES-PRO',
      slogan: 'タクト駆動のスマート製造',
      tagline: '実用 · シンプル · 柔軟',
    },
    api: {
      connectFail: 'サーバーに接続できません',
      connectFailDescription:
        'ネットワークを確認して再試行してください。解決しない場合は管理者にお問い合わせください。',
    },
    signalr: {
      connectFail: 'リアルタイム接続に失敗しました',
      onlineNotify: 'リアルタイム接続が復旧しました',
      newMessage: '新着メッセージ',
    },
    theme: {
      switch: 'テーマを切り替え',
      switchToLight: 'ライトモードに切り替え',
      switchToDark: 'ダークモードに切り替え',
      light: 'ライト',
      dark: 'ダーク',
      system: 'システムに従う',
    },
    locale: {
      switch: '言語を切り替え',
    },
    tenant: {
      switch: 'テナントを切り替え',
    },
    company: {
      switch: '会社を切り替え',
    },
    color: {
      title: 'テーマカラー',
      switch: 'テーマカラーを切り替え',
      'mars-green': 'マースグリーン',
      'tiffany-blue': 'ティファニーブルー',
      'chinese-red': 'チャイニーズレッド',
      'titian-red': 'ティツィアンレッド',
      burgundy: 'バーガンディ',
      bordeaux: 'ボルドー',
      'klein-blue': 'クラインズブルー',
      'van-dyke-brown': 'ヴァン・ダイクブラウン',
      'prussian-blue': 'プルシアンブルー',
      'senelier-yellow': 'サネリエイエロー',
      'memorial-gray': 'メモリアルグレー',
      custom: 'カスタム',
    },
    layout: {
      switch: 'ログインレイアウトを切り替え',
      position: {
        left: '左寄せ',
        center: '中央',
        right: '右寄せ',
      },
    },
    entity: {
      culturelist: '言語リスト',
      menulist: 'メニュー',
      tenantlist: 'テナントリスト',
    },
    button: {
      ok: 'OK',
      cancel: 'キャンセル',
      logout: 'ログアウト',
      profile: 'プロフィール',
    },
  },
  feedback: {
    load: {
      empty: '利用可能な{target}がありません',
      failed: '{target}の読み込みに失敗しました',
    },
    connect: {
      success: '接続しました',
    },
    signalr: {
      error: 'リアルタイム接続でエラーが発生しました',
    },
  },
  tip: {
    session: {
      expired: 'ログインの有効期限が切れました。再度ログインしてください。',
      idle: {
        logout: '長時間操作がなかったため、自動的にログアウトしました。',
      },
    },
    force: {
      logout: '管理者により強制ログアウトされました。',
    },
    confirm: {
      action: {
        title: '{action}を確認',
        question: '{action}しますか？',
      },
    },
  },
};
