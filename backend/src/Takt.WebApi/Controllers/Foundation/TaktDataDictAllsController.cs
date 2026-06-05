// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktDataDictAllsController.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：全部字典数据控制器（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 全部字典数据控制器
/// 登录后一次性拉取当前租户全部字典项，供前端按 dictTypeCode 分组缓存
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "全部字典")]
public class TaktDataDictAllsController : TaktControllerBase
{
    private readonly ITaktDataDictAllService _dataDictAllService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dataDictAllService">全部字典数据服务</param>
    public TaktDataDictAllsController(ITaktDataDictAllService dataDictAllService)
    {
        _dataDictAllService = dataDictAllService;
    }

    /// <summary>
    /// 获取当前租户下全部字典数据（登录即可访问，供前端全局字典缓存）
    /// </summary>
    /// <returns>扁平字典项列表</returns>
    [HttpGet]
    public async Task<IActionResult> GetDataDictAllAsync()
    {
        try
        {
            var result = await _dataDictAllService.GetDataDictAllAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
