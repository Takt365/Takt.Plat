// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktDataDictAllDtos.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：租户下全部字典数据响应 DTO（供前端按 dictTypeCode 分组缓存）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Options;

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 租户下全部字典数据响应 DTO
/// 对应前端 TaktDataDictAllDto；Items 为扁平列表，含 DictTypeCode 供前端分组
/// </summary>
public class TaktDataDictAllDto
{
    /// <summary>
    /// 字典项列表（已按 DictTypeCode、SortOrder 排序）
    /// </summary>
    public List<TaktSelectOption> Items { get; set; } = new();
}
