/**
 * 布局与路由 · 香港繁体
 * 用于 layouts、router、会话过期等文案
 */
export default {
  page: {
      route: {
        logintitle: '登錄',
        loadfail: '路由模塊加載失敗，請檢查開發服務器是否正常運行，或刷新頁麵重試。'
      },
      session: {
        autologout: '由於長時間未操作，系統已自動登出',
        canceltext: '立即登出',
        content: '您已長時間未操作，系統將在 {minutes} 分鐘後自動登出。請點擊"繼續使用"保持登錄狀態。',
        oktext: '繼續使用',
        title: '會話即將過期'
      }
  }
}
