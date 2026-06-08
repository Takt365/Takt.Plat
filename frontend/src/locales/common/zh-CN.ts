// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/common
// 文件名称：zh-CN.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：通用中文语言包（仅登录壳/主题等前端静态文案；业务通用项见后端动态种子 common.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    app: {
      title: '节拍工厂',
      name: '节拍工厂·Takt Plat',
      productcode: 'TP-MES-PRO',
      slogan: '节拍驱动智造',
      tagline: '实用 · 简洁 · 灵活',
    },
    api: {
      connectFail: '无法连接服务器',
      connectFailDescription: '请检查网络连接或稍后重试；若持续失败请联系管理员。',
    },
    signalr: {
      connectFail: '实时消息连接失败',
      onlineNotify: '实时消息连接已恢复',
    },
    theme: {
      switch: '切换主题',
      switchToLight: '切换为浅色模式',
      switchToDark: '切换为深色模式',
      light: '浅色',
      dark: '深色',
      system: '跟随系统',
    },
    locale: {
      switch: '切换语言',
    },
    tenant: {
      switch: '切换租户',
    },
    company: {
      switch: '切换公司',
    },
    color: {
      title: '主题色',
      switch: '切换主题色',
      'mars-green': '马尔斯绿',
      'tiffany-blue': '蒂芙尼蓝',
      'chinese-red': '中国红',
      'titian-red': '提香红',
      burgundy: '勃艮第酒红',
      bordeaux: '波尔多红',
      'klein-blue': '克莱因蓝',
      'van-dyke-brown': '凡戴克棕',
      'prussian-blue': '普鲁士蓝',
      'senelier-yellow': '申内利尔黄',
      'memorial-gray': '纪念灰',
      custom: '自定义',
    },
    layout: {
      switch: '切换登录布局',
      position: {
        left: '左对齐',
        center: '居中',
        right: '右对齐',
      },
    },
    entity: {
      culturelist: '语言列表',
      menulist: '菜单',
      tenantlist: '租户列表',
    },
    button: {
      ok: '确定',
      cancel: '取消',
      logout: '退出登录',
      profile: '个人中心',
    },
  },
  feedback: {
    load: {
      empty: '未获取到可用的{target}',
      failed: '加载{target}失败',
    },
    connect: {
      success: '连接成功',
    },
    signalr: {
      error: '实时连接发生错误',
    },
  },
  tip: {
    session: {
      expired: '登录已过期，请重新登录',
      idle: {
        logout: '长时间未操作，已自动退出登录',
      },
    },
    force: {
      logout: '您已被强制下线',
    },
    confirm: {
      action: {
        title: '确认{action}',
        question: '确定要{action}吗？',
      },
    },
  },
};
