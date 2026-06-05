// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktNumberingGeneratorsController.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：业务单据编号生成器控制器（预览/生成，与编号规则 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 业务单据编号生成器控制器
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "编号生成器")]
public class TaktNumberingGeneratorsController : TaktControllerBase
{
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingGenerator">编号生成器</param>
    public TaktNumberingGeneratorsController(ITaktNumberingGenerator numberingGenerator)
    {
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 预览业务编号（不占用流水号）
    /// </summary>
    /// <param name="request">预览参数</param>
    /// <returns>预览结果</returns>
    [TaktPermission("foundation:numbering:query", "预览业务编号")]
    [HttpPost("preview")]
    public async Task<IActionResult> PreviewNumberingAsync([FromBody] TaktNumberingPreviewRequestDto request)
    {
        try
        {
            var result = await _numberingGenerator.PreviewNumberingAsync(request);
            return Success(result, "预览成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 生成下一个业务编号（占用流水号）
    /// </summary>
    /// <param name="request">生成参数</param>
    /// <returns>生成结果</returns>
    [TaktPermission("foundation:numbering:update", "生成业务编号")]
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateNumberingAsync([FromBody] TaktNumberingGenerateRequestDto request)
    {
        try
        {
            var result = await _numberingGenerator.GenerateNumberingAsync(request);
            return Success(result, "生成成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
