// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialTrendAnalysisService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格/机种推移分析核心服务接口（内部编排）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料推移分析核心服务（移动价格转置 + 机种 BOM 扩展；供 Moving/Model 门面服务调用）
/// </summary>
public interface ITaktMaterialTrendAnalysisService
{
    Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync();
    Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string plantCode);
    Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string plantCode, string? valuation = null);
    Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(TaktMaterialMovingTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query, string? sheetName = null, string? fileName = null);
    Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(TaktMaterialModelTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(
        TaktMaterialModelTrendQueryDto query, string? sheetName = null, string? fileName = null);
}
