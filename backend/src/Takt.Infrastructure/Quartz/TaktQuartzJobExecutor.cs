// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobExecutor.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行器（按 TaktQuartzTask 全字段执行、写日志并 SignalR 推送执行结果）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Quartz;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 任务执行器（按任务类型分发并写执行日志）
/// </summary>
public sealed class TaktQuartzJobExecutor
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEnumerable<ITaktQuartzJobHandler> _handlers;
    private readonly ITaktQuartzJobSignalRPushService _quartzJobSignalRPushService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="httpClientFactory">HTTP 客户端工厂</param>
    /// <param name="handlers">程序集任务处理器集合</param>
    /// <param name="quartzJobSignalRPushService">Quartz Job 执行 SignalR 推送</param>
    public TaktQuartzJobExecutor(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IEnumerable<ITaktQuartzJobHandler> handlers,
        ITaktQuartzJobSignalRPushService quartzJobSignalRPushService)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(handlers);
        ArgumentNullException.ThrowIfNull(quartzJobSignalRPushService);
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _handlers = handlers;
        _quartzJobSignalRPushService = quartzJobSignalRPushService;
    }

    /// <summary>
    /// 执行 Quartz Job
    /// </summary>
    /// <param name="context">Quartz 执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ExecuteAsync(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var data = context.MergedJobDataMap;
        var tenantCode = data.GetString(TaktQuartzJobDataKeys.TenantCode) ?? string.Empty;
        var companyCode = data.GetString(TaktQuartzJobDataKeys.CompanyCode) ?? string.Empty;
        var taskId = data.GetLong(TaktQuartzJobDataKeys.QuartzTaskId);
        var userName = data.GetString(TaktQuartzJobDataKeys.UserName);
        var triggerExecuteParams = data.GetString(TaktQuartzJobDataKeys.ExecuteParams);
        var manualTrigger = data.GetInt(TaktQuartzJobDataKeys.ManualTrigger) == 1;
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode) || taskId <= 0)
        {
            throw new InvalidOperationException("Quartz JobDataMap 缺少 TenantCode/CompanyCode/QuartzTaskId");
        }
        using var seedContext = new TaktSeedContext(_configuration, tenantCode);
        var task = await seedContext.Db.Queryable<TaktQuartzTask>()
            .Where(x => x.Id == taskId
                && x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsDeleted == 0)
            .FirstAsync(cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException($"定时任务不存在：Id={taskId}, Tenant={tenantCode}, Company={companyCode}");
        }
        var stopwatch = Stopwatch.StartNew();
        var executeTime = DateTime.Now;
        var executeMessage = string.Empty;
        var errorInfo = string.Empty;
        var executeStatus = TaktExecuteStatus.Success;
        var effectiveExecuteParams = TaktQuartzSchedulingHelper.ResolveEffectiveExecuteParams(task, triggerExecuteParams);
        Exception? executionException = null;
        TaktQuartzLog? executionLog = null;
        try
        {
            TaktQuartzSchedulingHelper.ValidateQuartzTaskForExecution(task, manualTrigger);
            executeMessage = await DispatchAsync(
                task,
                effectiveExecuteParams,
                userName,
                seedContext,
                cancellationToken);
        }
        catch (Exception ex)
        {
            executeStatus = TaktExecuteStatus.Failed;
            errorInfo = ex.Message;
            executeMessage = string.IsNullOrWhiteSpace(executeMessage) ? "执行失败" : executeMessage;
            executionException = ex;
            TaktLogger.Error(ex, "[Quartz] 任务执行失败 TaskId={TaskId}, Code={TaskCode}", task.Id, task.TaskCode);
        }
        finally
        {
            stopwatch.Stop();
            executionLog = await PersistExecutionResultAsync(
                seedContext,
                task,
                executeTime,
                stopwatch.ElapsedMilliseconds,
                effectiveExecuteParams,
                executeMessage,
                errorInfo,
                executeStatus,
                context,
                cancellationToken);
        }
        await _quartzJobSignalRPushService.PushTaskExecutedAsync(task, executionLog, userName);
        if (executionException != null)
        {
            throw new JobExecutionException(executionException, refireImmediately: false);
        }
    }

    /// <summary>
    /// 按任务类型分发执行逻辑
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="executeParams">有效执行参数</param>
    /// <param name="userName">触发用户</param>
    /// <param name="seedContext">租户种子上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要消息</returns>
    private async Task<string> DispatchAsync(
        TaktQuartzTask task,
        string? executeParams,
        string? userName,
        TaktSeedContext seedContext,
        CancellationToken cancellationToken)
    {
        var taskType = (task.TaskType ?? string.Empty).Trim().ToLowerInvariant();
        return taskType switch
        {
            "assembly" => await ExecuteAssemblyAsync(
                task, executeParams, userName, cancellationToken),
            "http" => await ExecuteHttpAsync(task, executeParams, cancellationToken),
            "sql" => await ExecuteSqlAsync(task, seedContext, cancellationToken),
            _ => throw new InvalidOperationException($"不支持的任务类型：{task.TaskType}"),
        };
    }

    /// <summary>
    /// 执行程序集任务（HandlerKey = ClassName，校验 AssemblyName）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">执行参数</param>
    /// <param name="userName">触发用户</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private async Task<string> ExecuteAssemblyAsync(
        TaktQuartzTask task,
        string? executeParams,
        string? userName,
        CancellationToken cancellationToken)
    {
        var handler = _handlers.FirstOrDefault(x =>
            string.Equals(x.HandlerKey, task.ClassName, StringComparison.OrdinalIgnoreCase));
        if (handler == null)
        {
            throw new InvalidOperationException($"未找到 Quartz 任务处理器：{task.ClassName}");
        }
        TaktQuartzSchedulingHelper.EnsureAssemblyNameMatchesHandler(task, handler);
        var jobContext = new TaktQuartzJobContext
        {
            Task = task,
            ExecuteParams = executeParams,
            UserName = userName,
        };
        await handler.ExecuteAsync(jobContext, cancellationToken);
        var assemblyHint = string.IsNullOrWhiteSpace(task.AssemblyName)
            ? handler.GetType().Assembly.GetName().Name
            : task.AssemblyName;
        return $"程序集任务已执行：{assemblyHint}/{handler.HandlerKey}";
    }

    /// <summary>
    /// 执行 HTTP 任务（携带租户/公司请求头）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">执行参数（请求体）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private async Task<string> ExecuteHttpAsync(
        TaktQuartzTask task,
        string? executeParams,
        CancellationToken cancellationToken)
    {
        var method = string.IsNullOrWhiteSpace(task.RequestMethod) ? "GET" : task.RequestMethod.Trim().ToUpperInvariant();
        using var request = new HttpRequestMessage(new HttpMethod(method), task.ApiUrl);
        request.Headers.TryAddWithoutValidation("X-Tenant-Code", task.TenantCode);
        request.Headers.TryAddWithoutValidation("X-Company-Code", task.CompanyCode);
        if (!string.IsNullOrWhiteSpace(executeParams)
            && method is "POST" or "PUT" or "PATCH")
        {
            request.Content = new StringContent(executeParams);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }
        var client = _httpClientFactory.CreateClient(TaktQuartzConstants.HttpClientName);
        using var response = await client.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"HTTP 任务失败：{(int)response.StatusCode} {response.ReasonPhrase}，URL={task.ApiUrl}");
        }
        return $"HTTP {method} 成功，状态码 {(int)response.StatusCode}，URL={task.ApiUrl}，响应长度 {body.Length}";
    }

    /// <summary>
    /// 执行只读 SQL 任务
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="seedContext">租户上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private static async Task<string> ExecuteSqlAsync(
        TaktQuartzTask task,
        TaktSeedContext seedContext,
        CancellationToken cancellationToken)
    {
        TaktSqlExecutorValidator.Validate(task.SqlScript!, TaktSqlExecuteOptions.ReadOnlyDefault);
        var rows = await TaktRepositoryReadOnlySql.QueryAsync(seedContext.Db, task.SqlScript!, null, cancellationToken);
        return $"SQL 执行成功，返回 {rows.Count} 行";
    }

    /// <summary>
    /// 持久化执行日志并更新任务统计字段
    /// </summary>
    /// <param name="seedContext">租户上下文</param>
    /// <param name="task">定时任务</param>
    /// <param name="executeTime">执行时间</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="executeParams">执行参数快照</param>
    /// <param name="executeMessage">执行消息</param>
    /// <param name="errorInfo">错误信息</param>
    /// <param name="executeStatus">执行状态</param>
    /// <param name="context">Quartz 上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>写入后的执行日志</returns>
    private static async Task<TaktQuartzLog> PersistExecutionResultAsync(
        TaktSeedContext seedContext,
        TaktQuartzTask task,
        DateTime executeTime,
        long durationMs,
        string? executeParams,
        string executeMessage,
        string errorInfo,
        TaktExecuteStatus executeStatus,
        IJobExecutionContext context,
        CancellationToken cancellationToken)
    {
        var (executeIp, executeHost) = TaktServerHostHelper.ResolveQuartzExecuteEndpoint();
        var log = TaktQuartzSchedulingHelper.BuildQuartzLog(
            task,
            executeTime,
            durationMs,
            executeParams,
            executeMessage,
            string.IsNullOrWhiteSpace(errorInfo) ? null : errorInfo,
            executeStatus,
            executeIp,
            executeHost);
        log.Id = await TaktPrimaryKeyInsertHelper.InsertEntityReturnInt64Async(
            seedContext.Db,
            log,
            TaktPrimaryKeyInsertHelper.RuntimeOptions,
            cancellationToken);
        task.ExecuteCount = checked(task.ExecuteCount + 1);
        task.LastRunAt = executeTime;
        task.NextRunAt = TaktQuartzSchedulingHelper.ResolveNextRunAt(context);
        await seedContext.Db.Updateable(task)
            .UpdateColumns(x => new { x.ExecuteCount, x.LastRunAt, x.NextRunAt })
            .ExecuteCommandAsync(cancellationToken);
        return log;
    }
}
