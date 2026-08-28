// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcDistinctionConstants.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：设变区分 logistics_manufacturing_ec_distinction_category 与执行生成判定常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变管理区分（字典 logistics_manufacturing_ec_distinction_category）及部门执行生成判定常量
/// </summary>
public static class TaktEcDistinctionConstants
{
    /// <summary>
    /// 全仕向
    /// </summary>
    public const int AllDestination = 1;

    /// <summary>
    /// 部管
    /// </summary>
    public const int MaterialControl = 2;

    /// <summary>
    /// 内部
    /// </summary>
    public const int Internal = 3;

    /// <summary>
    /// 技术
    /// </summary>
    public const int Technical = 4;

    /// <summary>
    /// 新采购类型：外部采购（需采购/受检填写）
    /// </summary>
    public const string PurchaseTypeExternal = "F";

    /// <summary>
    /// 新品仓库：原料电子保税仓等（区分部管时走制造二课门禁）
    /// </summary>
    public const string NewWarehousePcbaGate = "C003";

    /// <summary>
    /// 计划物料停产状态（字典 logistics_materials_material_discontinued_status；非此值视为 EOL）
    /// </summary>
    public const string PlannedMaterialStatus = "Z0";

    /// <summary>
    /// 区分=全仕向时，无需人工填写部门的执行内容
    /// </summary>
    public const string AllDestinationExecContent = "管理区分-全仕向";

    /// <summary>
    /// 区分=部管时，非采购/受检/部管/制二课的执行内容
    /// </summary>
    public const string MaterialControlExecContent = "管理区分-部管";

    /// <summary>
    /// 区分=内部时各部门执行内容（不做采购类型等条件判断）
    /// </summary>
    public const string InternalExecContent = "管理区分-内部";

    /// <summary>
    /// 区分=技术时各部门执行内容（不做采购类型等条件判断）
    /// </summary>
    public const string TechnicalExecContent = "管理区分-技术";

    /// <summary>
    /// 历史自动完成文案；再生成时替换为区分文案
    /// </summary>
    public const string AutoCompletedExecContent = "系统自动完成";

    /// <summary>
    /// 完成品物料状态≠Z0 时自动填充的执行内容（各部门可人工清空以恢复）
    /// </summary>
    public const string EolExecContent = "EOL";

    /// <summary>
    /// 是否按完成品物料状态视为 EOL（空或 Z0 为否）
    /// </summary>
    /// <param name="discontinuedStatus">完成品物料状态</param>
    /// <returns>是否 EOL</returns>
    public static bool IsEolDiscontinued(string? discontinuedStatus)
    {
        var status = discontinuedStatus?.Trim();
        if (string.IsNullOrEmpty(status))
        {
            return false;
        }
        return !string.Equals(status, PlannedMaterialStatus, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否外部采购类型 F（采购课列表可见、采购/受检待填）
    /// </summary>
    /// <param name="purchaseType">采购类型（空视为否）</param>
    /// <returns>是否为 F</returns>
    public static bool IsExternalPurchaseType(string? purchaseType)
    {
        return string.Equals(
            purchaseType?.Trim(),
            PurchaseTypeExternal,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 新品仓库是否为制二门禁仓 C003
    /// </summary>
    /// <param name="warehouse">新品仓库编码</param>
    /// <returns>是否为 C003</returns>
    public static bool IsPcbaGateWarehouse(string? warehouse)
    {
        return string.Equals(
            warehouse?.Trim(),
            NewWarehousePcbaGate,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 部管课是否需人工填写（新采购类型 F 且新品仓库非 C003）
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>是否部管课可见待填</returns>
    public static bool IsBukanVisible(string? purchaseType, string? newWarehouse)
    {
        return IsExternalPurchaseType(purchaseType) && !IsPcbaGateWarehouse(newWarehouse);
    }

    /// <summary>
    /// 制二课 C003 页签：新采购类型 F 且新品仓库为 C003
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>是否属于制二 C003 页签</returns>
    public static bool IsPcbaC003ExternalGroup(string? purchaseType, string? newWarehouse)
    {
        return IsExternalPurchaseType(purchaseType) && IsPcbaGateWarehouse(newWarehouse);
    }

    /// <summary>
    /// 按区分得到自动填写的执行内容（管理区分-全仕向/部管/内部/技术）
    /// </summary>
    /// <param name="ecDistinction">设变区分</param>
    /// <returns>自动填写文案</returns>
    public static string ResolveAutoExecContent(int ecDistinction)
    {
        return ecDistinction switch
        {
            AllDestination => AllDestinationExecContent,
            MaterialControl => MaterialControlExecContent,
            Internal => InternalExecContent,
            Technical => TechnicalExecContent,
            _ => TechnicalExecContent
        };
    }

    /// <summary>
    /// 将历史短文案规范为「管理区分-…」（落库唯一口径）
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>规范后的执行内容</returns>
    public static string? NormalizeLegacyAutoExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content;
        }
        var value = content.Trim();
        return value switch
        {
            "全仕向" => AllDestinationExecContent,
            "部管" or "部管为止" => MaterialControlExecContent,
            "内部" or "内部管理" => InternalExecContent,
            "技术" or "技术为止" => TechnicalExecContent,
            _ => content
        };
    }

    /// <summary>
    /// 区分=部管时，采购/受检/部管/制二课是否需按条件人工填写
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>是否待填</returns>
    public static bool IsMaterialControlNeedFillDept(
        string? deptCode,
        string? purchaseType,
        string? newWarehouse)
    {
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Mp => IsExternalPurchaseType(purchaseType),
            TaktEcDeptCodes.Iqc => IsExternalPurchaseType(purchaseType),
            TaktEcDeptCodes.Mc => IsBukanVisible(purchaseType, newWarehouse),
            TaktEcDeptCodes.Pcba => IsPcbaC003ExternalGroup(purchaseType, newWarehouse),
            _ => false
        };
    }

    /// <summary>
    /// 是否为系统按区分/历史规则写入的执行内容（可被再生成覆盖）
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否为自动生成文案</returns>
    public static bool IsDistinctionGeneratedExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return true;
        }
        var value = content.Trim();
        return string.Equals(value, AutoCompletedExecContent, StringComparison.Ordinal)
            || string.Equals(value, AllDestinationExecContent, StringComparison.Ordinal)
            || string.Equals(value, MaterialControlExecContent, StringComparison.Ordinal)
            || string.Equals(value, InternalExecContent, StringComparison.Ordinal)
            || string.Equals(value, TechnicalExecContent, StringComparison.Ordinal)
            || string.Equals(value, "全仕向", StringComparison.Ordinal)
            || string.Equals(value, "部管", StringComparison.Ordinal)
            || string.Equals(value, "内部", StringComparison.Ordinal)
            || string.Equals(value, "技术", StringComparison.Ordinal)
            || string.Equals(value, "部管为止", StringComparison.Ordinal)
            || string.Equals(value, "内部管理", StringComparison.Ordinal)
            || string.Equals(value, "技术为止", StringComparison.Ordinal)
            || string.Equals(value, TaktEcKoubaiConstants.NotPurchasingRelatedExecContent, StringComparison.Ordinal)
            || string.Equals(value, TaktEcUkekenConstants.NotRelatedToIqcExecContent, StringComparison.Ordinal)
            || string.Equals(value, TaktEcBukanConstants.NotRelatedToMaterialControlExecContent, StringComparison.Ordinal);
    }
}
