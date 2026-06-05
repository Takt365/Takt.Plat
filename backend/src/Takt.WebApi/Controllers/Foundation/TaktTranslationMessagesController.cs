// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktTranslationMessagesController.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：前端动态翻译消息控制器（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 前端动态翻译消息控制器
/// 登录后按区域文化拉取扁平 i18n 键值，供 vue-i18n mergeLocaleMessage
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "翻译消息")]
public class TaktTranslationMessagesController : TaktControllerBase
{
    private readonly ITaktTranslationMessageService _translationMessageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="translationMessageService">翻译消息服务</param>
    public TaktTranslationMessagesController(ITaktTranslationMessageService translationMessageService)
    {
        _translationMessageService = translationMessageService;
    }

    /// <summary>
    /// 获取指定区域文化的前端扁平翻译消息
    /// </summary>
    /// <param name="cultureCode">区域文化编码 BCP47（如 zh-CN）</param>
    /// <returns>扁平 i18n 键值</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTranslationMessagesAsync([FromQuery] string cultureCode)
    {
        try
        {
            var result = await _translationMessageService.GetTranslationMessagesAsync(cultureCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
