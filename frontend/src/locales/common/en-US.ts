// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/common
// 文件名称：en-US.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：通用英文语言包（仅登录壳/主题等前端静态文案；业务通用项见后端动态种子 common.*）
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
      slogan: 'Takt-driven smart manufacturing',
      tagline: 'Practical · Simple · Flexible',
    },
    api: {
      connectFail: 'Unable to connect to the server',
      connectFailDescription:
        'Check your network and try again. Contact an administrator if the problem persists.',
    },
    signalr: {
      connectFail: 'Real-time connection failed',
      onlineNotify: 'Real-time connection restored',
    },
    theme: {
      switch: 'Switch theme',
      switchToLight: 'Switch to light mode',
      switchToDark: 'Switch to dark mode',
      light: 'Light',
      dark: 'Dark',
      system: 'System',
    },
    locale: {
      switch: 'Switch language',
    },
    tenant: {
      switch: 'Switch tenant',
    },
    company: {
      switch: 'Switch company',
    },
    color: {
      title: 'Theme color',
      switch: 'Switch theme color',
      'mars-green': 'Mars Green',
      'tiffany-blue': 'Tiffany Blue',
      'chinese-red': 'Chinese Red',
      'titian-red': 'Titian Red',
      burgundy: 'Burgundy',
      bordeaux: 'Bordeaux',
      'klein-blue': 'Klein Blue',
      'van-dyke-brown': 'Van Dyke Brown',
      'prussian-blue': 'Prussian Blue',
      'senelier-yellow': 'Sennelier Yellow',
      'memorial-gray': 'Memorial Gray',
      custom: 'Custom',
    },
    layout: {
      switch: 'Switch login layout',
      position: {
        left: 'Align left',
        center: 'Center',
        right: 'Align right',
      },
    },
    entity: {
      culturelist: 'language list',
      menulist: 'menu',
      tenantlist: 'tenant list',
    },
    button: {
      ok: 'OK',
      cancel: 'Cancel',
      logout: 'Logout',
      profile: 'Profile',
    },
  },
  feedback: {
    load: {
      empty: 'No available {target}',
      failed: 'Failed to load {target}',
    },
    connect: {
      success: 'Connected successfully',
    },
    signalr: {
      error: 'Real-time connection error',
    },
  },
  tip: {
    session: {
      expired: 'Your session has expired. Please sign in again.',
      idle: {
        logout: 'You have been signed out due to inactivity.',
      },
    },
    force: {
      logout: 'You have been signed out by an administrator.',
    },
    confirm: {
      action: {
        title: 'Confirm {action}',
        question: 'Are you sure you want to {action}?',
      },
    },
  },
};
