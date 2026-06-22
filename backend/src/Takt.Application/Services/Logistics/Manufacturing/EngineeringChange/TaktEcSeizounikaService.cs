// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizounikaService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制造二课视图应用服务（DeptCode=Pcba）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变制造二课视图应用服务
/// </summary>
public class TaktEcSeizounikaService : TaktEcDeptViewServiceBase, ITaktEcSeizounikaService
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcSeizounikaService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcDept> ecDeptRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(TaktEcDeptCodes.Pcba, ecDetailRepository, ecDeptRepository, lineNumberGenerator, userContext, localizationService)
    {
    }

    /// <summary>获取制造二课列表（分页）</summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public Task<TaktPagedResult<TaktEcDeptViewDto>> GetEcSeizounikaListAsync(TaktEcDeptViewQueryDto queryDto) => GetDeptViewListAsync(queryDto);

    /// <summary>根据设变明细 ID 获取制造二课行</summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>部门视图 DTO</returns>
    public Task<TaktEcDeptViewDto?> GetEcSeizounikaByEcDetailIdAsync(long ecDetailId) => GetDeptViewByEcDetailIdAsync(ecDetailId);

    /// <summary>更新制造二课</summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>部门视图 DTO</returns>
    public Task<TaktEcDeptViewDto> UpdateEcSeizounikaAsync(long ecDetailId, TaktEcDeptViewUpdateDto dto) => UpdateDeptViewAsync(ecDetailId, dto);

    /// <summary>导出制造二课</summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public Task<(string fileName, byte[] fileContent)> ExportEcSeizounikaAsync(TaktEcDeptViewQueryDto? query = null, string? sheetName = null, string? fileName = null) => ExportDeptViewAsync(query, sheetName, fileName);
}
