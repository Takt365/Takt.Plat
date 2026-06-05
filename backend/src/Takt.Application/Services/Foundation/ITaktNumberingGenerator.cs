// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktNumberingGenerator.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：业务单据编号生成器接口（按编号规则分配流水号）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 业务单据编号生成器（按 <see cref="Takt.Domain.Entities.Foundation.TaktNumbering"/> 规则生成并持久化流水号）
/// </summary>
public interface ITaktNumberingGenerator
{
    /// <summary>
    /// 预览编号（不占用流水号、不写库）
    /// </summary>
    /// <param name="request">预览参数（规则 Id 或规则编码 + 规则字段）</param>
    /// <returns>预览结果</returns>
    Task<TaktNumberingPreviewResultDto> PreviewNumberingAsync(TaktNumberingPreviewRequestDto request);

    /// <summary>
    /// 生成下一个业务编号（递增 CurrentSequence 并写回规则）
    /// </summary>
    /// <param name="request">生成参数</param>
    /// <returns>生成结果</returns>
    Task<TaktNumberingGenerateResultDto> GenerateNumberingAsync(TaktNumberingGenerateRequestDto request);
}
