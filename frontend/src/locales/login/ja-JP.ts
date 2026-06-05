// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/locales/login
// 文件名称：ja-JP.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：ログインモジュール日本語（キー login.page.*；ログイン/登録フィールドと検証は静的）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenantCode: 'テナント',
    companyCode: '会社',
    forgotPassword: 'パスワードをお忘れですか？',
    field: {
      tenantCode: {
        placeholder: 'テナントコードを入力',
      },
      username: {
        label: 'ユーザー名',
        placeholder: 'ユーザー名を入力',
      },
      password: {
        label: 'パスワード',
        placeholder: 'パスワードを入力',
      },
      culture: {
        label: '言語',
        placeholder: '有効なテナントコードを先に入力してください',
      },
      email: {
        label: 'メールアドレス',
        placeholder: 'メールアドレスを入力',
      },
      phone: {
        label: '携帯電話番号',
        placeholder: '携帯電話番号を入力',
      },
      usernameOrEmail: {
        label: 'ユーザー名またはメールアドレス',
        placeholder: 'ユーザー名またはメールアドレスを入力',
      },
    },
    validate: {
      tenantRequired: 'テナントコードを入力してください',
      tenantInvalid: 'テナントコードの形式が正しくありません',
      usernameRequired: 'ユーザー名を入力してください',
      usernameInvalid: 'ユーザー名の形式が正しくありません',
      passwordRequired: 'パスワードを入力してください',
      passwordWeak: 'パスワードの強度が不足しています',
      emailRequired: 'メールアドレスを入力してください',
      emailInvalid: 'メールアドレスの形式が正しくありません',
      emailTooShort: 'メールアドレスは {min} 文字以上である必要があります',
      emailTooLong: 'メールアドレスは {max} 文字以内である必要があります',
      phoneRequired: '携帯電話番号を入力してください',
      phoneInvalid: '携帯電話番号の形式が正しくありません',
      usernameOrEmailRequired: 'ユーザー名またはメールアドレスを入力してください',
      usernameOrEmailInvalid: 'ユーザー名またはメールアドレスの形式が正しくありません',
      usernameOrEmailTooShort: 'ユーザー名またはメールアドレスは {min} 文字以上である必要があります',
      usernameOrEmailTooLong: 'ユーザー名またはメールアドレスは {max} 文字以内である必要があります',
      captchaRequired: 'セキュリティ認証を完了してください',
    },
    login: {
      title: 'ようこそ',
      rememberme: 'ログイン状態を保持',
      login: 'ログイン',
      logout: 'ログアウト',
      noaccountregister: 'Takt365 は初めてですか？{register}',
    },
    forgot: {
      title: 'パスワードをお忘れの方',
      subtitle: 'メールアドレスを入力してください。パスワード再設定リンクをお送りします',
      submit: 'パスワードをリセット',
      backtologin: 'ログインに戻る',
      fail: 'リセットメールの送信に失敗しました。メールアドレスを確認してください',
      emailnotregistered: 'このメールアドレスは登録されていません。ご確認ください',
      emailSent: 'リセットメールを送信しました。受信トレイをご確認ください',
      resetUnavailable: 'このアカウントは自助パスワードリセットに対応していません。管理者にお問い合わせください',
      steps: {
        email: 'メール入力',
        captcha: 'セキュリティ確認',
        done: '送信完了',
      },
    },
    sign: {
      title: '新規登録',
      subtitle: 'アカウントを作成して始めましょう',
      hasaccount: 'すでにアカウントをお持ちですか？ログイン',
      success: '登録が完了しました。ログインしてください',
      successinitialpassword: '登録が完了しました。初期パスワードはメールをご確認ください',
      fail: '登録に失敗しました',
      steps: {
        info: '情報入力',
        captcha: 'セキュリティ確認',
        done: '登録完了',
      },
    },
    message: {
      fail: 'ログインに失敗しました',
      credentialsIncorrect: 'ユーザー名またはパスワードが正しくありません',
      tenantNoAccess: '選択したテナントにログインする権限がありません',
      tenantNotFound: 'テナントが存在しません',
      userNotFound: 'ユーザーが存在しないか、無効になっています',
      defaultCompanyNotFound: 'デフォルトのログイン会社が設定されていません',
      tenantValidateFail: 'テナントの検証に失敗しました。しばらくしてから再試行してください',
      tenantDatabaseMissing: 'テナント {tenantCode} の業務 DB が存在しません（{databaseName}）。Init.InitDb を true にして再起動してください。',
      tenantTableMissing: 'テナント {tenantCode} の DB {databaseName} には接続できますが、業務テーブルがありません。Init.InitDb / SeedData を有効にしてください。',
      tenantDatabaseLoginFailed: 'テナント {tenantCode} で SQL Server にログインできません（{databaseName}）。接続文字列と sa パスワードを確認してください。',
      tenantOptionsFail: 'テナント一覧の読み込みに失敗しました。しばらくしてから再試行してください',
      publicKeyMissing: 'ログイン暗号化公開鍵を取得できません。ページを更新してください',
      encryptFail: 'パスワードの暗号化に失敗しました。再試行してください',
    },
    log: {
      signInSessionStart: 'Cookie セッションの確立を開始',
      signInSessionSuccess: 'Cookie セッション確立完了。OAuth 認可を開始',
      verifyPasswordStart: 'パスワード検証を開始',
      verifyPasswordSuccess: 'パスワード検証成功',
      verifyPasswordCaptchaRequired: 'パスワード検証成功。認証コードが必要',
    },
    captcha: {
      title: 'セキュリティ確認',
      dragHint: 'スライダーをドラッグしてパズルを完成',
      behaviorHint: 'スライダーを正しい位置までドラッグ',
      slideToTarget: 'スライダーを目標位置 {position}% までドラッグしてください',
      success: '確認完了',
      cancel: 'キャンセル',
      confirm: '確認',
      typerequired: 'サーバーから認証コードタイプが返されませんでした。バックエンド設定を確認してください',
    },
    callback: {
      title: 'ログインコールバック',
      processing: 'ログインを完了しています...',
      missingCode: '認可コードがありません',
      stateMismatch: 'state の検証に失敗しました',
      pkceMissing: 'PKCE 検証パラメータが失われました',
      fail: 'ログインに失敗しました',
      log: {
        oauthError: '認可コールバックエラー：{description}',
        exchangeStart: '認可コードをトークンに交換開始',
        exchangeSuccess: 'トークン取得完了。ユーザープロファイルを読み込み',
        rbacReady: 'OAuth コールバック後 RBAC 準備完了',
      },
    },
  },
};
