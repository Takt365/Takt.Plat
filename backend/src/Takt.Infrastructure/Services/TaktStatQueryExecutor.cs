// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktStatQueryExecutor.cs
// 创建时间：2026-06-19
// 创建人：Takt365(Cursor AI)
// 功能描述：SQVI 自定义报表 SqlSugar Queryable 原生执行器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Statistics;
using Takt.Shared.Validation;

namespace Takt.Infrastructure.Services;

/// <summary>
/// SQVI 自定义报表 SqlSugar Queryable 原生执行器（ToPageListAsync / Take + ToDataTableAsync）
/// </summary>
public sealed class TaktStatQueryExecutor : ITaktStatQueryExecutor
{
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">租户 SqlSugar 上下文</param>
    public TaktStatQueryExecutor(TaktSqlSugarContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<TaktStatQueryPageResult> ExecutePagedAsync(
        TaktStatQueryBuildRequest request,
        int pageIndex,
        int pageSize,
        int maxPageSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Min(maxPageSize, Math.Max(1, pageSize));
        var (queryable, metadata) = TaktStatQueryBuilder.Compile(_dbContext.Db, request);
        ValidateQueryableSql(queryable);
        RefAsync<int> totalCount = 0;
        var dynamicRows = await queryable.ToPageListAsync(pageIndex, pageSize, totalCount);
        cancellationToken.ThrowIfCancellationRequested();
        var rows = TaktStatQueryRowConverter.FromDynamicRows(dynamicRows, metadata.OutputKeys);
        return new TaktStatQueryPageResult
        {
            Total = totalCount.Value,
            Rows = rows,
            OutputKeys = metadata.OutputKeys,
            OutputLabels = metadata.OutputLabels,
        };
    }

    /// <inheritdoc />
    public async Task<TaktStatQueryPageResult> ExecuteTopAsync(
        TaktStatQueryBuildRequest request,
        int maxRows,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (maxRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRows), "maxRows 必须大于 0");
        }
        cancellationToken.ThrowIfCancellationRequested();
        var (queryable, metadata) = TaktStatQueryBuilder.Compile(_dbContext.Db, request);
        ValidateQueryableSql(queryable);
        var dataTable = await queryable.Take(maxRows).ToDataTableAsync();
        cancellationToken.ThrowIfCancellationRequested();
        var rows = TaktStatQueryRowConverter.FromDataTable(dataTable, metadata.OutputKeys);
        return new TaktStatQueryPageResult
        {
            Total = rows.Count,
            Rows = rows,
            OutputKeys = metadata.OutputKeys,
            OutputLabels = metadata.OutputLabels,
        };
    }

    /// <summary>
    /// 校验 SqlSugar 生成的 SQL 符合只读策略
    /// </summary>
    /// <param name="queryable">已编译 Queryable</param>
    private static void ValidateQueryableSql(ISugarQueryable<object> queryable)
    {
        var sqlInfo = queryable.ToSql();
        TaktSqlExecutorValidator.Validate(sqlInfo.Key);
    }
}
