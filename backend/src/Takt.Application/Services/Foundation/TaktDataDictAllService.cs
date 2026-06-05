// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktDataDictAllService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：全部字典数据应用服务实现（独立模块，非 CRUD 脚本生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 全部字典数据应用服务
/// </summary>
public class TaktDataDictAllService : TaktServiceBase, ITaktDataDictAllService
{
    private readonly ITaktTenantRepository<TaktDictData> _dictDataRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictDataRepository">字典数据仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDataDictAllService(
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _dictDataRepository = dictDataRepository;
    }

    /// <summary>
    /// 获取当前租户下全部字典数据（扁平列表，含 DictTypeCode）
    /// </summary>
    /// <returns>全部字典数据 DTO</returns>
    public async Task<TaktDataDictAllDto> GetDataDictAllAsync()
    {
        var list = await _dictDataRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder,
            false);

        var items = list
            .OrderBy(x => x.DictTypeCode)
            .ThenBy(x => x.SortOrder)
            .Select(MapToSelectOption)
            .ToList();

        return new TaktDataDictAllDto
        {
            Items = items,
        };
    }

    /// <summary>
    /// 将字典数据实体映射为下拉选项
    /// </summary>
    /// <param name="entity">字典数据实体</param>
    /// <returns>下拉选项</returns>
    private static TaktSelectOption MapToSelectOption(TaktDictData entity)
    {
        return new TaktSelectOption
        {
            DictLabel = entity.DictLabel,
            DictValue = entity.DictValue,
            I18nKey = entity.I18nKey,
            DictTypeCode = entity.DictTypeCode,
            ExtLabel = entity.ExtLabel,
            ExtValue = entity.ExtValue,
            CssClass = entity.CssClass,
            ListClass = entity.ListClass,
            SortOrder = entity.SortOrder,
        };
    }
}
