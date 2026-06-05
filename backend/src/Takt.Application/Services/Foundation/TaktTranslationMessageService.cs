// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktTranslationMessageService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：前端动态翻译消息应用服务实现（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 前端动态翻译消息应用服务
/// </summary>
public class TaktTranslationMessageService : TaktServiceBase, ITaktTranslationMessageService
{
    private readonly ITaktTenantRepository<TaktTranslation> _translationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="translationRepository">翻译仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTranslationMessageService(
        ITaktTenantRepository<TaktTranslation> translationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _translationRepository = translationRepository;
    }

    /// <summary>
    /// 获取指定区域文化的前端扁平翻译消息（ResourceType=Frontend）
    /// </summary>
    /// <param name="cultureCode">区域文化编码 BCP47</param>
    /// <returns>扁平键值 DTO</returns>
    public async Task<TaktTranslationMessagesDto> GetTranslationMessagesAsync(string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            ThrowBusinessException("区域文化编码不能为空");
        }

        var trimmedCulture = cultureCode.Trim();
        var list = await _translationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CultureCode == trimmedCulture
                && x.ResourceType == TaktAppSide.Frontend);

        var messages = list
            .Where(x => !string.IsNullOrWhiteSpace(x.I18nKey))
            .GroupBy(x => x.I18nKey)
            .ToDictionary(g => g.Key, g => g.First().TranslationText ?? string.Empty);

        return new TaktTranslationMessagesDto
        {
            CultureCode = trimmedCulture,
            Messages = messages,
        };
    }
}
