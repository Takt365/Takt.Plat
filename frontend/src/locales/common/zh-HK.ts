// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/common
// 文件名称：zh-HK.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：通用香港繁體語言包（仅登录壳/主题等前端静态文案；业务通用项见后端动态种子 common.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    app: {
      title: '節拍工廠',
      name: '節拍工廠·Takt Plat',
      productcode: 'TP-MES-PRO',
      slogan: '節拍驅動智造',
      tagline: '實用 · 簡潔 · 靈活',
    },
    api: {
      connectFail: '無法連接服務器',
      connectFailDescription: '請檢查網絡連接或稍後重試；若持續失敗請聯繫管理員。',
    },
    signalr: {
      connectFail: '實時消息連接失敗',
      onlineNotify: '實時消息連接已恢復',
    },
    theme: {
      switch: '切換主題',
      switchToLight: '切換為淺色模式',
      switchToDark: '切換為深色模式',
      light: '淺色',
      dark: '深色',
      system: '跟隨系統',
    },
    locale: {
      switch: '切換語言',
    },
    tenant: {
      switch: '切換租戶',
    },
    company: {
      switch: '切換公司',
    },
    color: {
      title: '主題色',
      switch: '切換主題色',
      'mars-green': '馬爾斯綠',
      'tiffany-blue': '蒂芙尼藍',
      'chinese-red': '中國紅',
      'titian-red': '提香紅',
      burgundy: '勃艮第酒紅',
      bordeaux: '波爾多紅',
      'klein-blue': '克萊因藍',
      'van-dyke-brown': '凡戴克棕',
      'prussian-blue': '普魯士藍',
      'senelier-yellow': '申內利爾黃',
      'memorial-gray': '紀念灰',
      custom: '自定義',
    },
    layout: {
      switch: '切換登錄佈局',
      position: {
        left: '左對齊',
        center: '居中',
        right: '右對齊',
      },
    },
    entity: {
      culturelist: '語言列表',
      menulist: '菜單',
      tenantlist: '租戶列表',
    },
    button: {
      ok: '確定',
      cancel: '取消',
      logout: '退出登錄',
      profile: '個人中心',
    },
  },
  feedback: {
    load: {
      empty: '未獲取到可用的{target}',
      failed: '加載{target}失敗',
    },
    connect: {
      success: '連接成功',
    },
    signalr: {
      error: '實時連接發生錯誤',
    },
  },
  tip: {
    session: {
      expired: '登錄已過期，請重新登錄',
      idle: {
        logout: '長時間未操作，已自動退出登錄',
      },
    },
    force: {
      logout: '您已被強制下線',
    },
    confirm: {
      action: {
        title: '確認{action}',
        question: '確定要{action}嗎？',
      },
    },
  },
};
