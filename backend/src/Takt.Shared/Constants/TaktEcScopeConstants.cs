// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcScopeConstants.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施范围 logistics_manufacturing_ec_scope_category 与执行生成判定常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变实施范围（字典 logistics_manufacturing_ec_scope_category）及部门执行生成判定常量。
/// 执行内容：停产≠Z0→「实施范围-{范围}-EOL」；全仕向空白；内部/技术自动「实施范围-内部/技术」；部管时生管/采购/受检/部管/制二待填，其余「实施范围-部管」。
/// </summary>
public static class TaktEcScopeConstants
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
    /// 新品仓库：原料电子保税仓等（实施范围=部管时走制造二课门禁）
    /// </summary>
    public const string NewWarehousePcbaGate = "C003";

    /// <summary>
    /// 计划物料停产状态（字典 logistics_materials_material_discontinued_status；非此值视为 EOL）
    /// </summary>
    public const string PlannedMaterialStatus = "Z0";

    /// <summary>
    /// 停产操作默认写入的根物料停产状态（字典 logistics_materials_material_discontinued_status；生产结束）
    /// </summary>
    public const string EolMaterialStatus = "ZQ";

    /// <summary>
    /// 执行内容标准前缀（统一口径；历史「管理区分-」读入时替换为此前缀）
    /// </summary>
    public const string ExecContentPrefix = "实施范围-";

    /// <summary>
    /// 历史执行内容前缀（仅识别/规范用，不得再写入）
    /// </summary>
    public const string LegacyExecContentPrefix = "管理区分-";

    /// <summary>
    /// 历史误写「实施范围-全仕向」（全仕向须空白由各部门填写；仅用于识别可覆盖旧值）
    /// </summary>
    public const string AllDestinationExecContent = ExecContentPrefix + "全仕向";

    /// <summary>
    /// 实施范围=部管时，非生管/采购/受检/部管/制二课的自动执行内容
    /// </summary>
    public const string MaterialControlExecContent = ExecContentPrefix + "部管";

    /// <summary>
    /// 实施范围=内部时各部门自动执行内容
    /// </summary>
    public const string InternalExecContent = ExecContentPrefix + "内部";

    /// <summary>
    /// 实施范围=技术时各部门自动执行内容
    /// </summary>
    public const string TechnicalExecContent = ExecContentPrefix + "技术";

    /// <summary>
    /// 历史自动完成文案；再生成时替换为实施范围文案
    /// </summary>
    public const string AutoCompletedExecContent = "系统自动完成";

    /// <summary>
    /// 停产状态≠Z0 时执行内容后缀（完整文案为「实施范围-{范围}-EOL」）
    /// </summary>
    public const string EolExecContent = "EOL";

    /// <summary>
    /// 按实施范围生成停产执行内容（实施范围-全仕向/部管/内部/技术-EOL）
    /// </summary>
    /// <param name="ecScope">设变实施范围</param>
    /// <returns>带实施范围前缀的 EOL 文案</returns>
    public static string ResolveEolExecContent(int ecScope)
    {
        var scopeLabel = ecScope switch
        {
            AllDestination => AllDestinationExecContent,
            MaterialControl => MaterialControlExecContent,
            Internal => InternalExecContent,
            Technical => TechnicalExecContent,
            _ => TechnicalExecContent
        };
        return $"{scopeLabel}-{EolExecContent}";
    }

    /// <summary>
    /// 是否为停产自动文案（含历史裸「EOL」、以及「实施范围-*-EOL」/历史「管理区分-*-EOL」）
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否 EOL 类文案</returns>
    public static bool IsEolExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        var value = ReplaceLegacyExecContentPrefix(content.Trim());
        if (string.Equals(value, EolExecContent, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return value.EndsWith($"-{EolExecContent}", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 新物料编码是否有效（空或占位「0」视为无新物料；采购/受检/部管不得因此生成执行行）
    /// </summary>
    /// <param name="ecNewMaterialCode">新物料编码</param>
    /// <returns>是否有效新物料</returns>
    public static bool HasEffectiveNewMaterialCode(string? ecNewMaterialCode)
    {
        var code = ecNewMaterialCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(code))
        {
            return false;
        }
        return !string.Equals(code, "0", StringComparison.Ordinal);
    }

    /// <summary>
    /// 是否采购/受检/部管课（依赖有效新物料编码才生成执行行）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>是否为上述三课</returns>
    public static bool IsNewMaterialDependentDept(string? deptCode)
    {
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Mp => true,
            TaktEcDeptCodes.Iqc => true,
            TaktEcDeptCodes.Mc => true,
            _ => false
        };
    }

    /// <summary>
    /// 是否按停产状态视为 EOL（空或 Z0 为否）
    /// </summary>
    /// <param name="discontinuedStatus">停产状态</param>
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
    /// 制二课其它页签：新采购类型不是 F
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <returns>是否属于制二 F以外页签</returns>
    public static bool IsPcbaOtherPurchaseGroup(string? purchaseType)
    {
        return !IsExternalPurchaseType(purchaseType);
    }

    /// <summary>
    /// 按实施范围解析自动执行内容：内部/技术→「实施范围-内部/技术」；部管→「实施范围-部管」（仅非待填部门使用）；全仕向→空（各部门人工填写）
    /// </summary>
    /// <param name="ecScope">设变实施范围</param>
    /// <returns>自动填写文案；全仕向返回空串</returns>
    public static string ResolveAutoExecContent(int ecScope)
    {
        return ecScope switch
        {
            AllDestination => string.Empty,
            MaterialControl => MaterialControlExecContent,
            Internal => InternalExecContent,
            Technical => TechnicalExecContent,
            _ => string.Empty
        };
    }

    /// <summary>
    /// 将历史短文案与「管理区分-」前缀规范为当前口径（前缀→「实施范围-」；全仕向→空；内部/技术/部管→标准文案）
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>规范后的执行内容</returns>
    public static string? NormalizeLegacyAutoExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content;
        }
        var value = ReplaceLegacyExecContentPrefix(content.Trim());
        return value switch
        {
            "全仕向" or AllDestinationExecContent => string.Empty,
            "部管" or "部管为止" or MaterialControlExecContent => MaterialControlExecContent,
            "内部" or "内部管理" or InternalExecContent => InternalExecContent,
            "技术" or "技术为止" or TechnicalExecContent => TechnicalExecContent,
            _ => value
        };
    }

    /// <summary>
    /// 历史前缀「管理区分-」统一替换为「实施范围-」（无此前缀则原样返回）
    /// </summary>
    /// <param name="content">已 Trim 的执行内容</param>
    /// <returns>前缀规范后的文案</returns>
    public static string ReplaceLegacyExecContentPrefix(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
        if (content.StartsWith(LegacyExecContentPrefix, StringComparison.Ordinal))
        {
            return ExecContentPrefix + content[LegacyExecContentPrefix.Length..];
        }
        return content;
    }

    /// <summary>
    /// 实施范围=部管时，须人工填写执行内容的部门（生管/采购/受检/部管/制二SMT）；其余部门自动写「实施范围-部管」
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="purchaseType">新采购类型（保留参数；部管待填名单不再按此过滤）</param>
    /// <param name="newWarehouse">新品仓库（保留参数；部管待填名单不再按此过滤）</param>
    /// <returns>是否待填</returns>
    public static bool IsMaterialControlNeedFillDept(
        string? deptCode,
        string? purchaseType = null,
        string? newWarehouse = null)
    {
        _ = purchaseType;
        _ = newWarehouse;
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => true,
            TaktEcDeptCodes.Mp => true,
            TaktEcDeptCodes.Iqc => true,
            TaktEcDeptCodes.Mc => true,
            TaktEcDeptCodes.Pcba => true,
            _ => false
        };
    }

    /// <summary>
    /// 是否为系统按实施范围/历史规则写入的执行内容（可被再生成覆盖）。
    /// 含历史「管理区分-」前缀（先规范为「实施范围-」再判定）。
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否为自动生成文案</returns>
    public static bool IsScopeGeneratedExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return true;
        }
        var value = ReplaceLegacyExecContentPrefix(content.Trim());
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
            || string.Equals(value, TaktEcBukanConstants.NotRelatedToMaterialControlExecContent, StringComparison.Ordinal)
            || IsEolExecContent(value);
    }
}
