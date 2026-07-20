// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services.Code.Database.Quartz
// 文件名称：TaktDatabaseBackupJobHandler.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 数据库备份处理器（ExecuteParams: { backupId }）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.Json;
using Takt.Application.Services.Code.Database;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Services.Code.Database.Quartz;

/// <summary>
/// Quartz：数据库备份处理器
/// </summary>
public sealed class TaktDatabaseBackupJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktDatabaseBackupService _databaseBackupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="databaseBackupService">备份服务</param>
    public TaktDatabaseBackupJobHandler(ITaktDatabaseBackupService databaseBackupService)
    {
        ArgumentNullException.ThrowIfNull(databaseBackupService);
        _databaseBackupService = databaseBackupService;
    }

    /// <inheritdoc />
    public string HandlerKey => TaktDatabaseBackupService.QuartzHandlerClassName;

    /// <inheritdoc />
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var backupId = TryParseBackupId(context.ExecuteParams);
        if (backupId <= 0)
        {
            context.ExecuteMessage = "ExecuteParams 缺少有效 backupId";
            TaktLogger.Warning("[DatabaseBackup] Quartz 参数无效 Params={Params}", context.ExecuteParams);
            return;
        }
        var summary = await _databaseBackupService.ExecuteScheduledDatabaseBackupAsync(backupId);
        context.ExecuteMessage = summary;
        TaktLogger.Information("[DatabaseBackup] Quartz 结束 BackupId={BackupId} Message={Message}", backupId, summary);
    }

    /// <summary>
    /// 解析 backupId（JSON {\"backupId\":\"…\"} 或纯数字）
    /// </summary>
    private static long TryParseBackupId(string? executeParams)
    {
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return 0;
        }
        var raw = executeParams.Trim();
        if (long.TryParse(raw, out var direct))
        {
            return direct;
        }
        try
        {
            using var doc = JsonDocument.Parse(raw);
            if (doc.RootElement.TryGetProperty("backupId", out var prop))
            {
                if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt64(out var n))
                {
                    return n;
                }
                if (prop.ValueKind == JsonValueKind.String && long.TryParse(prop.GetString(), out var s))
                {
                    return s;
                }
            }
        }
        catch
        {
            return 0;
        }
        return 0;
    }
}
