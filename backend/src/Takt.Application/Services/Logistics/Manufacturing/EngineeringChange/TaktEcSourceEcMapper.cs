// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSourceEcMapper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主/子表映射为设变主/明细创建 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源数据映射为设变创建 DTO
/// </summary>
public static class TaktEcSourceEcMapper
{
    /// <summary>
    /// 将设变来源主表及明细映射为设变创建 DTO
    /// </summary>
    /// <param name="sourceEc">设变来源主表</param>
    /// <param name="sourceDetails">设变来源明细列表</param>
    /// <param name="plantCode">目标工厂代码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="materialsByCode">目标工厂物料字典（物料编码 → TaktMaterialPlant）；为空时不补全物料衍生字段</param>
    /// <returns>设变创建 DTO</returns>
    public static TaktEcGijutsuCreateDto ToCreateDto(
        TaktSourceEc sourceEc,
        IReadOnlyList<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture,
        IReadOnlyDictionary<string, TaktMaterialPlant>? materialsByCode = null)
    {
        ArgumentNullException.ThrowIfNull(sourceEc);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var culture = companyDefaultCulture ?? string.Empty;
        var today = DateTime.Today;
        var ecCode = sourceEc.SourceEcCode ?? string.Empty;
        var createDto = new TaktEcGijutsuCreateDto
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            CultureCode = culture,
            PlantCode = plantCode,
            EcCode = ecCode,
            EcIssueDate = sourceEc.SourceIssueDate,
            EcTitle = sourceEc.SourceTitle ?? string.Empty,
            EcContent = sourceEc.SourceEcContent ?? string.Empty,
            EcLeader = string.Empty,
            EcLossAmount = 0,
            EcDistinction = 1,
            EcEntryDate = today,
            ChangeStatus = TaktEcSourceStatusMapper.MapToChangeStatusOrThrow(sourceEc.SourceStatus),
            EcStatus = 1,
            EcDetails = MapDetails(sourceDetails, ecCode, tenantCode, companyCode, culture, today, materialsByCode),
        };
        return createDto;
    }

    /// <summary>
    /// 映射设变来源明细为设变明细创建 DTO 列表
    /// </summary>
    /// <param name="sourceDetails">设变来源明细</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="entryDate">录入日期</param>
    /// <param name="materialsByCode">目标工厂物料字典</param>
    /// <returns>设变明细创建 DTO 列表</returns>
    private static List<TaktEcDetailCreateDto> MapDetails(
        IReadOnlyList<TaktSourceEcDetail> sourceDetails,
        string ecCode,
        string tenantCode,
        string companyCode,
        string companyDefaultCulture,
        DateTime entryDate,
        IReadOnlyDictionary<string, TaktMaterialPlant>? materialsByCode)
    {
        if (sourceDetails == null || sourceDetails.Count == 0)
        {
            return new List<TaktEcDetailCreateDto>();
        }
        var lineNumber = 0;
        var result = new List<TaktEcDetailCreateDto>(sourceDetails.Count);
        foreach (var detail in sourceDetails)
        {
            lineNumber += 10;
            var bomDate = detail.SourceBomEffectiveDate ?? entryDate;
            var dto = new TaktEcDetailCreateDto
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                CultureCode = companyDefaultCulture ?? string.Empty,
                EcCode = ecCode,
                LineNumber = lineNumber,
                EcModel = string.IsNullOrWhiteSpace(detail.SourceFinishedProduct)
                    ? (detail.SourceParentPart ?? string.Empty)
                    : detail.SourceFinishedProduct,
                EcBomItem = detail.SourceFinishedProduct,
                EcBomSubItem = detail.SourceParentPart,
                EcOldItem = detail.SourceLegacyPartCode,
                EcOldText = detail.SourceLegacyPartName,
                EcOldUsage = detail.SourceLegacyUsage,
                EcOldPosition = detail.SourceLegacyMountingPosition,
                EcNewItem = detail.SourceReplacementPartCode,
                EcNewText = detail.SourceReplacementPartName,
                EcNewUsage = detail.SourceReplacementUsage,
                EcNewPosition = detail.SourceReplacementMountingPosition,
                EcBomDate = bomDate,
            };
            if (materialsByCode != null)
            {
                TaktEcDetailMaterialPlantMapper.EnrichCreateDto(dto, materialsByCode);
            }
            result.Add(dto);
        }
        return result;
    }
}
