// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailMaterialPlantMapper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：按工厂物料 TaktMaterialPlant 补全设变明细物料描述、库存、仓库、采购/检验及 EOL 字段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细与工厂物料（TaktMaterialPlant）字段映射
/// </summary>
public static class TaktEcDetailMaterialPlantMapper
{
    /// <summary>
    /// 按物料编码从工厂物料字典补全设变明细创建 DTO 的物料衍生字段
    /// </summary>
    /// <param name="dto">设变明细创建 DTO</param>
    /// <param name="materialsByCode">物料编码 → 工厂物料（当前工厂）</param>
    public static void EnrichCreateDto(
        TaktEcDetailCreateDto dto,
        IReadOnlyDictionary<string, TaktMaterialPlant> materialsByCode)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(materialsByCode);
        if (TryGetMaterial(materialsByCode, dto.EcBomItem, out var bomItem))
        {
            dto.EcBomItemText = ResolveMaterialText(bomItem);
            dto.IsEndOfLine = ResolveIsEndOfLine(bomItem);
        }
        if (TryGetMaterial(materialsByCode, dto.EcBomSubItem, out var bomSubItem))
        {
            dto.EcBomSubItemText = ResolveMaterialText(bomSubItem);
        }
        if (TryGetMaterial(materialsByCode, dto.EcOldItem, out var oldMaterial))
        {
            dto.EcOldText = ResolveMaterialText(oldMaterial);
            dto.EcOldStock = oldMaterial.CurrentStock;
            dto.EcOldWarehouse = ResolveWarehouse(oldMaterial);
            dto.IsOldCheck = oldMaterial.IsInspection;
            dto.IsOldProcurement = ResolveIsProcurement(oldMaterial);
        }
        if (TryGetMaterial(materialsByCode, dto.EcNewItem, out var newMaterial))
        {
            dto.EcNewText = ResolveMaterialText(newMaterial);
            dto.EcNewStock = newMaterial.CurrentStock;
            dto.EcNewWarehouse = ResolveWarehouse(newMaterial);
            dto.IsNewCheck = newMaterial.IsInspection;
            dto.IsNewProcurement = ResolveIsProcurement(newMaterial);
        }
    }

    /// <summary>
    /// 收集来源明细行涉及的物料编码（完成品、上阶、旧料、新料）
    /// </summary>
    /// <param name="materialCode">单个物料编码</param>
    /// <param name="codes">收集目标</param>
    public static void CollectMaterialCode(string? materialCode, ISet<string> codes)
    {
        ArgumentNullException.ThrowIfNull(codes);
        if (string.IsNullOrWhiteSpace(materialCode))
        {
            return;
        }
        codes.Add(materialCode.Trim());
    }

    /// <summary>
    /// 解析物料显示文本（名称优先，其次描述）
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>物料描述</returns>
    private static string ResolveMaterialText(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        if (!string.IsNullOrWhiteSpace(material.MaterialName))
        {
            return material.MaterialName.Trim();
        }
        if (!string.IsNullOrWhiteSpace(material.MaterialDescription))
        {
            return material.MaterialDescription.Trim();
        }
        return material.MaterialCode;
    }

    /// <summary>
    /// 完成品 EOL：停产状态非 Z0（计划物料）视为 EOL
    /// </summary>
    /// <param name="material">完成品工厂物料</param>
    /// <returns>0 或 1</returns>
    private static int ResolveIsEndOfLine(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var eol = material.IsEndOfLife?.Trim();
        if (string.IsNullOrEmpty(eol))
        {
            return 0;
        }
        return string.Equals(eol, "Z0", StringComparison.OrdinalIgnoreCase) ? 0 : 1;
    }

    /// <summary>
    /// 是否采购：采购类型 F=1（外部采购），E=0（自制生产）
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>0 或 1</returns>
    private static int ResolveIsProcurement(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var purchaseType = material.PurchaseType?.Trim().ToUpperInvariant();
        return purchaseType == "F" ? 1 : 0;
    }

    /// <summary>
    /// 仓库：自制取生产仓储，外购取采购仓储
    /// </summary>
    /// <param name="material">工厂物料</param>
    /// <returns>仓库编码</returns>
    private static string ResolveWarehouse(TaktMaterialPlant material)
    {
        ArgumentNullException.ThrowIfNull(material);
        var purchaseType = material.PurchaseType?.Trim().ToUpperInvariant();
        if (purchaseType == "E" && !string.IsNullOrWhiteSpace(material.ProductionLocation))
        {
            return material.ProductionLocation.Trim();
        }
        if (!string.IsNullOrWhiteSpace(material.PurchasingLocation))
        {
            return material.PurchasingLocation.Trim();
        }
        return material.ProductionLocation?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 按物料编码（忽略大小写）查找工厂物料
    /// </summary>
    /// <param name="materialsByCode">物料字典</param>
    /// <param name="materialCode">物料编码</param>
    /// <param name="material">查到的工厂物料</param>
    /// <returns>是否找到</returns>
    private static bool TryGetMaterial(
        IReadOnlyDictionary<string, TaktMaterialPlant> materialsByCode,
        string? materialCode,
        out TaktMaterialPlant material)
    {
        material = null!;
        if (string.IsNullOrWhiteSpace(materialCode))
        {
            return false;
        }
        return materialsByCode.TryGetValue(materialCode.Trim(), out material!);
    }
}
