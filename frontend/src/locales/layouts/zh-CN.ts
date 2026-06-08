/**
 * 布局与路由 · 中文
 * 引用键 layouts.page.*（前端静态；会话预警 Modal 等）
 */
export default {
  page: {
    route: {
      logintitle: '登录',
      loadfail: '路由模块加载失败，请检查开发服务器是否正常运行，或刷新页面重试。',
    },
    session: {
      canceltext: '立即登出',
      content: '您已长时间未操作，系统将在 {minutes} 分钟后自动登出。请点击"继续使用"保持登录状态。',
      oktext: '继续使用',
      title: '会话即将过期',
    },
  },
};
