// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktTranslationMessageService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：前端动态翻译消息应用服务接口（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 前端动态翻译消息应用服务接口
/// </summary>
public interface ITaktTranslationMessageService
{
    /// <summary>
    /// 获取指定区域文化的前端扁平翻译消息（ResourceType=Frontend）
    /// </summary>
    /// <param name="cultureCode">区域文化编码 BCP47（如 zh-CN）</param>
    /// <returns>扁平键值 DTO</returns>
    Task<TaktTranslationMessagesDto> GetTranslationMessagesAsync(string cultureCode);
}
