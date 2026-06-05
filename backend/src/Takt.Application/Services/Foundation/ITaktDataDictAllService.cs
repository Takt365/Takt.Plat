// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktDataDictAllService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：全部字典数据应用服务接口（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 全部字典数据应用服务接口
/// </summary>
public interface ITaktDataDictAllService
{
    /// <summary>
    /// 获取当前租户下全部字典数据（扁平列表，含 DictTypeCode）
    /// </summary>
    /// <returns>全部字典数据 DTO</returns>
    Task<TaktDataDictAllDto> GetDataDictAllAsync();
}
