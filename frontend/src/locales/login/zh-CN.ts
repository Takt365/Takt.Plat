// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/login
// 文件名称：zh-CN.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：登录模块中文语言包（引用键 login.page.*；登录/注册页字段与校验均为静态，不依赖 entity.* 动态种子）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenantCode: '租户',
    companyCode: '公司',
    forgotPassword: '忘记密码？',
    field: {
      tenantCode: {
        placeholder: '请输入租户编码',
      },
      username: {
        label: '用户名',
        placeholder: '请输入用户名',
      },
      password: {
        label: '密码',
        placeholder: '请输入密码',
      },
      culture: {
        label: '语言',
        placeholder: '请先输入有效租户编码',
      },
      email: {
        label: '邮箱',
        placeholder: '请输入邮箱',
      },
      phone: {
        label: '手机号',
        placeholder: '请输入手机号',
      },
      usernameOrEmail: {
        label: '用户名或邮箱',
        placeholder: '请输入用户名或邮箱',
      },
    },
    validate: {
      tenantRequired: '请输入租户编码',
      tenantInvalid: '租户编码格式不正确',
      usernameRequired: '请输入用户名',
      usernameInvalid: '用户名格式不正确',
      passwordRequired: '请输入密码',
      passwordWeak: '密码强度不足',
      emailRequired: '请输入邮箱',
      emailInvalid: '邮箱格式不正确',
      emailTooShort: '邮箱长度不能少于 {min} 个字符',
      emailTooLong: '邮箱长度不能超过 {max} 个字符',
      phoneRequired: '请输入手机号',
      phoneInvalid: '手机号格式不正确',
      usernameOrEmailRequired: '请输入用户名或邮箱',
      usernameOrEmailInvalid: '用户名或邮箱格式不正确',
      usernameOrEmailTooShort: '用户名或邮箱长度不能少于 {min} 个字符',
      usernameOrEmailTooLong: '用户名或邮箱长度不能超过 {max} 个字符',
      captchaRequired: '请完成安全验证',
    },
    login: {
      title: '欢迎',
      rememberme: '记住我',
      login: '登录',
      logout: '退出登录',
      noaccountregister: '初识节拍365？{register}',
    },
    forgot: {
      title: '忘记密码',
      subtitle: '请输入邮箱地址，我们将向您发送密码重置链接',
      submit: '重置密码',
      backtologin: '返回登录',
      fail: '发送重置邮件失败，请检查邮箱地址',
      emailnotregistered: '该邮箱地址未注册，请确认！',
      emailSent: '重置邮件已发送，请查收邮箱',
      resetUnavailable: '该账号不支持自助重置密码，请联系管理员',
      steps: {
        email: '输入邮箱',
        captcha: '安全验证',
        done: '发送完成',
      },
    },
    sign: {
      title: '注册',
      subtitle: '创建您的账号以开始使用',
      hasaccount: '已有账号？去登录',
      success: '注册成功，请登录',
      successinitialpassword: '注册成功，初始密码请查收邮件',
      fail: '注册失败',
      steps: {
        info: '填写资料',
        captcha: '安全验证',
        done: '完成注册',
      },
    },
    message: {
      fail: '登录失败',
      credentialsIncorrect: '用户名或密码错误',
      tenantNoAccess: '当前用户无权登录所选租户',
      tenantNotFound: '租户不存在',
      userNotFound: '用户不存在或已禁用',
      defaultCompanyNotFound: '未配置默认登录公司',
      tenantValidateFail: '租户校验失败，请稍后重试',
      tenantDatabaseMissing: '租户 {tenantCode} 的业务数据库不存在（库名：{databaseName}）。请将 Init.InitDb 设为 true 后重启后端建库。',
      tenantTableMissing: '租户 {tenantCode} 的业务数据库 {databaseName} 已连接，但缺少业务数据表。请开启 Init.InitDb 建表并视需要 SeedData。',
      tenantDatabaseLoginFailed: '租户 {tenantCode} 无法登录 SQL Server（库名：{databaseName}）。请检查连接字符串中的服务器与 sa 密码。',
      tenantOptionsFail: '加载租户列表失败，请稍后重试',
      publicKeyMissing: '无法获取登录加密公钥，请刷新后重试',
      encryptFail: '密码加密失败，请重试',
    },
    log: {
      signInSessionStart: '开始建立 Cookie 会话',
      signInSessionSuccess: 'Cookie 会话已建立，准备 OAuth 授权',
      verifyPasswordStart: '开始校验密码',
      verifyPasswordSuccess: '密码校验通过',
      verifyPasswordCaptchaRequired: '密码校验通过，需验证码',
    },
    captcha: {
      title: '安全验证',
      dragHint: '拖动滑块完成拼图',
      behaviorHint: '拖动滑块至正确位置',
      slideToTarget: '请拖动滑块至目标位置 {position}%',
      success: '验证通过',
      cancel: '取消',
      confirm: '确认',
      typerequired: '服务端未返回验证码类型，请检查后端配置',
    },
    callback: {
      title: '登录回调',
      processing: '正在完成登录...',
      missingCode: '缺少授权码',
      stateMismatch: 'state 校验失败',
      pkceMissing: 'PKCE 校验参数丢失',
      fail: '登录失败',
      log: {
        oauthError: '授权回调错误：{description}',
        exchangeStart: '开始用授权码换取令牌',
        exchangeSuccess: '授权码换令牌成功，用户资料已加载',
        rbacReady: 'OAuth 回调后 RBAC 已就绪',
      },
    },
  },
};
