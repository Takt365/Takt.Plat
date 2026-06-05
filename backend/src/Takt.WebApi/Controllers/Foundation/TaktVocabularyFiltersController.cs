// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktVocabularyFiltersController.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词过滤控制器（检测/替换，与词库 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 敏感词过滤控制器
/// 提供 UGC 文本检测与替换 API，底层使用 <see cref="ITaktVocabularyFilter"/>
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "敏感词过滤")]
public class TaktVocabularyFiltersController : TaktControllerBase
{
    private readonly ITaktVocabularyFilter _vocabularyFilter;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vocabularyFilter">敏感词过滤器</param>
    public TaktVocabularyFiltersController(ITaktVocabularyFilter vocabularyFilter)
    {
        _vocabularyFilter = vocabularyFilter;
    }

    /// <summary>
    /// 检测文本是否包含敏感词
    /// </summary>
    /// <param name="dto">检测请求</param>
    /// <returns>检测结果</returns>
    [TaktPermission("foundation:vocabulary:filter", "敏感词检测")]
    [HttpPost("detect")]
    public async Task<IActionResult> DetectVocabularyTextAsync([FromBody] TaktVocabularyFilterRequestDto dto)
    {
        try
        {
            var filterResult = await _vocabularyFilter.FilterAsync(dto.Text, dto.MinFilterLevel);
            var result = new TaktVocabularyDetectResultDto
            {
                HasSensitiveWord = filterResult.HasSensitiveWord,
                MatchedWords = filterResult.MatchedWords,
            };
            return Success(result, "检测完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 过滤文本中的敏感词
    /// </summary>
    /// <param name="dto">过滤请求</param>
    /// <returns>过滤结果</returns>
    [TaktPermission("foundation:vocabulary:replace", "敏感词替换")]
    [HttpPost("filter")]
    public async Task<IActionResult> FilterVocabularyTextAsync([FromBody] TaktVocabularyFilterRequestDto dto)
    {
        try
        {
            var filterResult = await _vocabularyFilter.FilterAsync(dto.Text, dto.MinFilterLevel);
            var result = new TaktVocabularyFilterResultDto
            {
                OriginalText = filterResult.OriginalText,
                FilteredText = filterResult.FilteredText,
                HasSensitiveWord = filterResult.HasSensitiveWord,
                MatchedWords = filterResult.MatchedWords,
            };
            return Success(result, "过滤完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
