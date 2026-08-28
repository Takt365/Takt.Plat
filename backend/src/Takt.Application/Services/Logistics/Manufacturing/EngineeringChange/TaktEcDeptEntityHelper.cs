// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptEntityHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行表实体读取辅助（转置/视图/批次，公共字段走 ITaktEcDeptExecEntity）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行实体读取辅助
/// </summary>
public static class TaktEcDeptEntityHelper
{
    /// <summary>
    /// 转为部门执行接口（公共字段单链路）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>部门执行接口</returns>
    /// <exception cref="ArgumentException">非部门执行实体</exception>
    private static ITaktEcDeptExecEntity AsExec(object exec)
    {
        ArgumentNullException.ThrowIfNull(exec);
        return exec as ITaktEcDeptExecEntity
            ?? throw new ArgumentException("不支持的部门执行实体类型", nameof(exec));
    }

    /// <summary>
    /// 按部门编码在列表中查找执行实体
    /// </summary>
    /// <param name="deptList">部门执行实体列表</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>执行实体</returns>
    public static object? FindByDeptCode(IReadOnlyList<object> deptList, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptList.FirstOrDefault(x => GetDeptCode(x) == deptCode);
    }

    /// <summary>
    /// 读取部门编码
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>部门编码</returns>
    public static string GetDeptCode(object exec) => AsExec(exec).DeptCode;

    /// <summary>
    /// 读取是否实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否实施</returns>
    public static int GetIsImplemented(object exec) => AsExec(exec).IsImplemented;

    /// <summary>
    /// 写入是否实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isImplemented">是否实施</param>
    public static void SetIsImplemented(object exec, int isImplemented)
    {
        AsExec(exec).IsImplemented = isImplemented;
    }

    /// <summary>
    /// 写入执行内容（仅当目标为空或强制覆盖时；短文案规范为「管理区分-…」）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="content">执行内容</param>
    /// <param name="overwrite">是否覆盖已有内容</param>
    public static void SetExecContent(object exec, string? content, bool overwrite = false)
    {
        var entity = AsExec(exec);
        var normalized = TaktEcDistinctionConstants.NormalizeLegacyAutoExecContent(content);
        if (overwrite || string.IsNullOrWhiteSpace(entity.ExecContent))
        {
            entity.ExecContent = normalized;
        }
    }

    /// <summary>
    /// 读取执行内容
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>执行内容</returns>
    public static string? GetExecContent(object exec) => AsExec(exec).ExecContent;

    /// <summary>
    /// 读取是否作废
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否作废</returns>
    public static int GetIsObsolete(object exec) => AsExec(exec).IsObsolete;

    /// <summary>
    /// 部门执行行是否已有输入（实施=是，或执行内容非空）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否有输入</returns>
    public static bool HasDeptInput(object exec)
    {
        if (GetIsObsolete(exec) == 1)
        {
            return false;
        }
        if (GetIsImplemented(exec) == 1)
        {
            return true;
        }
        return !string.IsNullOrWhiteSpace(GetExecContent(exec));
    }

    /// <summary>
    /// 解析转置单元格完成日期
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>完成日期</returns>
    public static DateTime? ResolveTransposedCompletedDate(object exec)
    {
        var deptCode = GetDeptCode(exec);
        DateTime? scheduledProductionDate = null;
        DateTime? purchaseOrderIssueDate = null;
        DateTime? inspectionDate = null;
        DateTime? outboundDate = null;
        DateTime? productionDate = null;
        DateTime? updatedAt = null;
        var createdAt = DateTime.MinValue;
        switch (exec)
        {
            case TaktEcSeikan e:
                scheduledProductionDate = e.ScheduledProductionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcKoubai e:
                purchaseOrderIssueDate = e.PurchaseOrderIssueDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcUkeken e:
                inspectionDate = e.InspectionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcBukan e:
                outboundDate = e.OutboundDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcSeizounika e:
                productionDate = e.ProductionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcSeizouikka e:
                productionDate = e.ProductionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcHinkan e:
                inspectionDate = e.InspectionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcSeizougijutsu e:
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
        }
        return TaktEcExecTransposedHelper.ResolveCompletedDate(
            deptCode,
            scheduledProductionDate,
            purchaseOrderIssueDate,
            inspectionDate,
            outboundDate,
            productionDate,
            updatedAt,
            createdAt);
    }

    /// <summary>
    /// 解析品管检样日期（批次转置）
    /// </summary>
    /// <param name="qaDept">品管执行实体</param>
    /// <returns>检样日期</returns>
    public static DateTime? ResolveSampleInspectionDate(object? qaDept)
    {
        if (qaDept is not TaktEcHinkan qa || qa.IsImplemented != 1)
        {
            return null;
        }
        if (qa.UpdatedAt.HasValue)
        {
            return qa.UpdatedAt.Value.Date;
        }
        return qa.CreatedAt.Date;
    }

    /// <summary>
    /// 读取设变单号
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>设变单号</returns>
    public static string GetEcCode(object exec) => AsExec(exec).EcCode;

    /// <summary>
    /// 读取明细 ID
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>设变明细 ID</returns>
    public static long GetEcnDetailId(object exec) => AsExec(exec).EcnDetailId;

    /// <summary>
    /// 判断是否匹配是否实施筛选
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isImplemented">是否实施</param>
    /// <returns>是否匹配</returns>
    public static bool MatchesIsImplemented(object exec, int isImplemented) =>
        GetIsImplemented(exec) == isImplemented;
}
