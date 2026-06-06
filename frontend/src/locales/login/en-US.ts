// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/login
// 文件名称：en-US.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Login module English locale (keys login.page.*; login/register fields and validation are static)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenantCode: 'Tenant',
    companyCode: 'Company',
    forgotPassword: 'Forgot password?',
    field: {
      tenantCode: {
        placeholder: 'Enter tenant code',
      },
      username: {
        label: 'Username',
        placeholder: 'Enter username',
      },
      password: {
        label: 'Password',
        placeholder: 'Enter password',
      },
      culture: {
        label: 'Language',
        placeholder: 'Enter a valid tenant code first',
      },
      email: {
        label: 'Email',
        placeholder: 'Enter email',
      },
      phone: {
        label: 'Phone',
        placeholder: 'Enter phone number',
      },
      usernameOrEmail: {
        label: 'Username or email',
        placeholder: 'Enter username or email',
      },
    },
    validate: {
      tenantRequired: 'Please enter tenant code',
      tenantInvalid: 'Tenant code must be 3 digits',
      usernameRequired: 'Please enter username',
      usernameInvalid: 'Invalid username format',
      passwordRequired: 'Please enter password',
      passwordWeak: 'Password is too weak',
      emailRequired: 'Please enter email',
      emailInvalid: 'Invalid email format',
      emailTooShort: 'Email must be at least {min} characters',
      emailTooLong: 'Email must not exceed {max} characters',
      phoneRequired: 'Please enter phone number',
      phoneInvalid: 'Invalid phone number format',
      usernameOrEmailRequired: 'Please enter username or email',
      usernameOrEmailInvalid: 'Invalid username or email format',
      usernameOrEmailTooShort: 'Username or email must be at least {min} characters',
      usernameOrEmailTooLong: 'Username or email must not exceed {max} characters',
      captchaRequired: 'Please complete security verification',
    },
    login: {
      title: 'Welcome',
      rememberme: 'Remember Me',
      login: 'Sign In',
      logout: 'Sign Out',
      noaccountregister: 'New to Takt365? {register}',
    },
    forgot: {
      title: 'Forgot Password',
      subtitle: 'Enter your email address and we will send you a password reset link',
      submit: 'Reset Password',
      backtologin: 'Back to Sign In',
      fail: 'Failed to send reset email. Please check your email address',
      emailnotregistered: 'This email address is not registered. Please confirm!',
      emailSent: 'Reset email sent. Please check your inbox',
      resetUnavailable: 'Self-service password reset is not available for this account. Please contact an administrator',
      steps: {
        email: 'Enter Email',
        captcha: 'Security Check',
        done: 'Email Sent',
      },
    },
    sign: {
      title: 'Sign up',
      subtitle: 'Create your account to get started',
      hasaccount: 'Already have an account? Sign in',
      success: 'Registration successful. Please sign in',
      successinitialpassword: 'Registration successful. Please check your email for the initial password.',
      fail: 'Registration failed',
      steps: {
        info: 'Account Info',
        captcha: 'Security Check',
        done: 'Complete',
      },
    },
    message: {
      fail: 'Login failed',
      credentialsIncorrect: 'Incorrect username or password',
      tenantNoAccess: 'You do not have permission to sign in to the selected tenant',
      tenantNotFound: 'Tenant does not exist',
      userNotFound: 'User does not exist or is disabled',
      defaultCompanyNotFound: 'No default login company configured',
      tenantValidateFail: 'Failed to validate tenant. Please try again later.',
      tenantDatabaseMissing: 'Tenant {tenantCode} database does not exist ({databaseName}). Set Init.InitDb to true and restart the backend.',
      tenantTableMissing: 'Tenant {tenantCode} database {databaseName} is reachable but business tables are missing. Enable Init.InitDb and SeedData as needed.',
      tenantDatabaseLoginFailed: 'Cannot log in to SQL Server for tenant {tenantCode} ({databaseName}). Check the connection string and sa password.',
      tenantOptionsFail: 'Failed to load tenant list. Please try again later.',
      publicKeyMissing: 'Unable to load login encryption key. Please refresh and try again.',
      encryptFail: 'Failed to encrypt password. Please try again.',
    },
    log: {
      signInSessionStart: 'Establishing cookie session',
      signInSessionSuccess: 'Cookie session ready; starting OAuth authorization',
      verifyPasswordStart: 'Verifying password',
      verifyPasswordSuccess: 'Password verified',
      verifyPasswordCaptchaRequired: 'Password verified; captcha required',
    },
    captcha: {
      title: 'Security Verification',
      dragHint: 'Drag the slider to complete the puzzle',
      behaviorHint: 'Drag the slider to the correct position',
      slideToTarget: 'Drag the slider to target position {position}%',
      success: 'Verified',
      cancel: 'Cancel',
      confirm: 'Confirm',
      typerequired: 'Captcha type not returned from server, please check backend configuration',
    },
    callback: {
      title: 'Login Callback',
      processing: 'Completing sign-in...',
      missingCode: 'Authorization code is missing',
      stateMismatch: 'State validation failed',
      pkceMissing: 'PKCE verifier is missing',
      fail: 'Login failed',
      log: {
        oauthError: 'Authorization callback error: {description}',
        exchangeStart: 'Exchanging authorization code for tokens',
        exchangeSuccess: 'Tokens received; user profile loaded',
        rbacReady: 'RBAC ready after OAuth callback',
      },
    },
  },
};
