// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktValidationI18nKeys.cs
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：抽象校验 I18n 键常量（common.validation.* / common.field.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 抽象校验与通用字段标签 I18n 键（配合 <c>{field}</c> 与 <c>entity.*</c> / <c>common.field.*</c>）。
/// </summary>
public static class TaktValidationI18nKeys
{
    /// <summary>不能为空</summary>
    public const string Required = "common.validation.required";

    /// <summary>不合法</summary>
    public const string Invalid = "common.validation.invalid";

    /// <summary>格式不正确</summary>
    public const string InvalidFormat = "common.validation.invalid.format";

    /// <summary>已存在</summary>
    public const string Duplicate = "common.validation.duplicate";

    /// <summary>不存在</summary>
    public const string NotFound = "common.validation.not.found";

    /// <summary>不存在或已过期</summary>
    public const string NotFoundOrExpired = "common.validation.not.found.or.expired";

    /// <summary>已过期</summary>
    public const string Expired = "common.validation.expired";

    /// <summary>不一致</summary>
    public const string NotMatch = "common.validation.not.match";

    /// <summary>过短</summary>
    public const string TooShort = "common.validation.too.short";

    /// <summary>过长</summary>
    public const string TooLong = "common.validation.too.long";

    /// <summary>长度区间</summary>
    public const string LengthBetween = "common.validation.length.between";

    /// <summary>不足</summary>
    public const string Insufficient = "common.validation.insufficient";

    /// <summary>不正确</summary>
    public const string Incorrect = "common.validation.incorrect";

    /// <summary>登录凭证（用户名或密码）</summary>
    public const string FieldLoginCredentials = "common.field.login.credentials";

    /// <summary>功能不可用</summary>
    public const string TipFeatureUnavailable = "common.tip.feature.unavailable";

    /// <summary>在线重置密码</summary>
    public const string FeatureResetPassword = "common.tip.reset.password";

    /// <summary>Excel 导入导出</summary>
    public const string FieldExcelHelper = "common.field.excel.helper";

    /// <summary>工作表</summary>
    public const string FieldSheet = "common.field.sheet";

    /// <summary>工作表名称</summary>
    public const string FieldSheetName = "common.field.sheet.name";

    /// <summary>工作表数据</summary>
    public const string FieldSheetData = "common.field.sheet.data";

    /// <summary>未配置</summary>
    public const string SystemNotConfigured = "common.system.not.configured";

    /// <summary>导入行数超限</summary>
    public const string DataImportRowLimitExceeded = "common.data.import.row.limit.exceeded";

    /// <summary>导入 Sheet 数量超限</summary>
    public const string DataImportSheetLimitExceeded = "common.data.import.sheet.limit.exceeded";

    /// <summary>导入单 Sheet 行数超限</summary>
    public const string DataImportSheetRowLimitExceeded = "common.data.import.sheet.row.limit.exceeded";

    /// <summary>导出 Sheet 数量超限</summary>
    public const string DataExportSheetLimitExceeded = "common.data.export.sheet.limit.exceeded";

    /// <summary>导出单 Sheet 行数超限</summary>
    public const string DataExportSheetRowLimitExceeded = "common.data.export.sheet.row.limit.exceeded";

    /// <summary>操作</summary>
    public const string ActionOperation = "common.action.operation";

    /// <summary>账号已停用</summary>
    public const string StatusAccountDisabled = "common.status.account.disabled";

    /// <summary>无权登录所选租户</summary>
    public const string PermissionTenantNoAccess = "common.permission.tenant.no.access";

    /// <summary>无权访问任何公司</summary>
    public const string PermissionCompanyNoAccess = "common.permission.company.no.access";

    /// <summary>功能未启用</summary>
    public const string SystemFeatureDisabled = "common.system.feature.disabled";

    /// <summary>请先选择目标</summary>
    public const string TipSelectFirst = "common.tip.select.first";

    /// <summary>不支持在线重置密码</summary>
    public const string TipResetPasswordUnavailable = "common.tip.reset.password.unavailable";

    /// <summary>邮件已发送</summary>
    public const string FeedbackEmailSent = "common.feedback.email.sent";

    /// <summary>无权执行指定操作</summary>
    public const string PermissionDeniedWithAction = "common.permission.denied.with.action";

    /// <summary>租户上下文缺失</summary>
    public const string FieldTenantContext = "common.field.tenant.context";

    /// <summary>记录</summary>
    public const string FieldRecord = "common.field.record";

    /// <summary>操作过快，请重试</summary>
    public const string TooFastRetry = "common.validation.too.fast.retry";

    /// <summary>验证未通过</summary>
    public const string VerifyFailed = "common.validation.verify.failed";

    /// <summary>请拖动滑块并稍候再提交</summary>
    public const string TipDragWaitSubmit = "common.tip.drag.wait.submit";

    /// <summary>请稍后再提交</summary>
    public const string TipWaitBeforeSubmit = "common.tip.wait.before.submit";

    /// <summary>操作成功</summary>
    public const string FeedbackSuccess = "common.feedback.success";

    /// <summary>验证码</summary>
    public const string FieldCaptcha = "common.field.captcha";

    /// <summary>验证码 ID</summary>
    public const string FieldCaptchaId = "common.field.captcha.id";

    /// <summary>验证码数据</summary>
    public const string FieldCaptchaPayload = "common.field.captcha.payload";

    /// <summary>滑块位置</summary>
    public const string FieldSliderPosition = "common.field.slider.position";

    /// <summary>行为验证数据</summary>
    public const string FieldBehaviorData = "common.field.behavior.data";

    /// <summary>鼠标轨迹</summary>
    public const string FieldMouseTrajectory = "common.field.mouse.trajectory";

    /// <summary>用户名（entity.user.name）</summary>
    public const string EntityUserName = "entity.user.name";

    /// <summary>密码（entity.user.password）</summary>
    public const string EntityUserPassword = "entity.user.password";

    /// <summary>用户实体（entity.user._self）</summary>
    public const string EntityUserSelf = "entity.user._self";

    /// <summary>租户实体（entity.tenant._self）</summary>
    public const string EntityTenantSelf = "entity.tenant._self";

    /// <summary>员工 ID（entity.user.employeeid）</summary>
    public const string EntityUserEmployeeId = "entity.user.employeeid";

    /// <summary>租户编码</summary>
    public const string FieldTenantCode = "common.field.tenant.code";

    /// <summary>公司编码</summary>
    public const string FieldCompanyCode = "common.field.company.code";

    /// <summary>区域文化编码</summary>
    public const string FieldCultureCode = "common.field.culture.code";

    /// <summary>登录票据</summary>
    public const string FieldLoginTicket = "common.field.login.ticket";

    /// <summary>密码传输密文</summary>
    public const string FieldPasswordCipher = "common.field.password.cipher";

    /// <summary>密码加密传输</summary>
    public const string FieldPasswordEncryption = "common.field.password.encryption";

    /// <summary>刷新令牌</summary>
    public const string FieldRefreshToken = "common.field.refresh.token";

    /// <summary>客户端 ID</summary>
    public const string FieldClientId = "common.field.client.id";

    /// <summary>授权码</summary>
    public const string FieldAuthorizationCode = "common.field.authorization.code";

    /// <summary>应用</summary>
    public const string FieldApplication = "common.field.application";

    /// <summary>用户名或邮箱</summary>
    public const string FieldUsernameOrEmail = "common.field.username.or.email";

    /// <summary>文件</summary>
    public const string FieldFile = "common.field.file";
}
