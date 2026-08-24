// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料机种推移分析服务
/// </summary>
public interface ITaktMaterialModelTrendService
{
    Task<List<TaktSelectOption>> GetMaterialModelTrendPlantOptionsAsync();
    Task<List<TaktSelectOption>> GetMaterialModelTrendValuationOptionsAsync(string plantCode);
    Task<List<TaktSelectOption>> GetMaterialModelTrendMaterialOptionsAsync(string plantCode, string? valuation = null);
    Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(TaktMaterialModelTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(
        TaktMaterialModelTrendQueryDto query, string? sheetName = null, string? fileName = null);
}
