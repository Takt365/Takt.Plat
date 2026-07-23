// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktQuartzTaskSeedData.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务示例种子（SQL/HTTP/Cron 三种类型，默认暂停，供管理页参考配置）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Services.Logistics.Manufacturing.Bom.Quartz;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Quartz 定时任务示例种子（按 Database:CompanyCodes 各公司写入；幂等：存在则更新，不存在则创建）
/// </summary>
public class TaktQuartzTaskSeedData : ITaktSeedDataCoordinator
{
    private const int TaskStatusPaused = 1;
    private const string TaskTypeAssembly = TaktConstants.QuartzTaskType.Assembly;
    private const string TaskTypeSql = TaktConstants.QuartzTaskType.Sql;
    private const string TaskTypeHttp = TaktConstants.QuartzTaskType.Http;
    private const int TriggerTypeSimple = 0;
    private const int TriggerTypeCron = 1;

    /// <summary>
    /// 执行顺序（在字典数据与公司基础数据之后）
    /// </summary>
    public int Order => 56;

    /// <summary>
    /// 初始化 Quartz 定时任务示例种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 Quartz 定时任务示例种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过 Quartz 定时任务种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktQuartzTask>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过 Quartz 定时任务种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过 Quartz 定时任务种子", tenantCode);
            return (0, 0);
        }
        var healthCheckUrl = ResolveHealthCheckUrl(configuration);
        var templates = GetDemoTaskTemplates(healthCheckUrl);
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化 Quartz 定时任务示例...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateQuartzTaskAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "Quartz 定时任务示例种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 解析本机健康检查地址（供 HTTP 示例任务使用）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>健康检查 URL</returns>
    private static string ResolveHealthCheckUrl(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var urls = configuration["ASPNETCORE_URLS"];
        if (!string.IsNullOrWhiteSpace(urls))
        {
            var first = urls.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault(u => u.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                ?? urls.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(first))
            {
                return $"{first.TrimEnd('/')}/health";
            }
        }
        return "http://localhost:60070/health";
    }

    /// <summary>
    /// 获取示例任务模板（默认暂停，避免启动自动调度）
    /// </summary>
    /// <param name="healthCheckUrl">HTTP 健康检查地址</param>
    /// <returns>任务模板列表</returns>
    private static List<QuartzTaskSeedTemplate> GetDemoTaskTemplates(string healthCheckUrl)
    {
        return new List<QuartzTaskSeedTemplate>
        {
            new(
                TaskCode: "QT_DEMO_SQL",
                TaskName: "示例：只读 SQL 统计",
                JobName: "demo_sql_dict_count",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/demo_sql_dict_count.sql",
                TriggerType: TriggerTypeSimple,
                IntervalSeconds: 3600,
                CronExpression: string.Empty,
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Simple 触发器 + wwwroot/Quartz/demo_sql_dict_count.sql（默认暂停）"),
            new(
                TaskCode: "QT_DEMO_HTTP",
                TaskName: "示例：HTTP 健康检查",
                JobName: "demo_http_health",
                TaskType: TaskTypeHttp,
                ApiUrl: healthCheckUrl,
                RequestMethod: "GET",
                TriggerType: TriggerTypeSimple,
                IntervalSeconds: 1800,
                CronExpression: string.Empty,
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Simple 触发器 + GET 健康检查（默认暂停，ApiUrl 随 ASPNETCORE_URLS 解析）"),
            new(
                TaskCode: "QT_DEMO_CRON_SQL",
                TaskName: "示例：Cron SQL",
                JobName: "demo_cron_sql_ping",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/demo_cron_sql_ping.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "种子示例：Cron 触发器（每日 02:00）+ wwwroot/Quartz/demo_cron_sql_ping.sql（默认暂停）"),
            new(
                TaskCode: "QT_EC_TASK_OVERDUE_SCAN",
                TaskName: "工程变更执行任务超时扫描",
                JobName: "ec_task_overdue_scan",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 */30 * * * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Application",
                ClassName: TaktEcFlowConstants.QuartzHandlerEcTaskOverdueScan,
                Description: "扫描设变执行任务超时/阻塞并 SignalR 预警（默认暂停，启用后每 30 分钟）"),
            // SAP 同步链：每日 07:30 起依次间隔 10 分钟（ma → md → st → ec → so → pp → sp）
            // 月度链（每月 3 日，自 03:00 起依次间隔 30 分钟）：mb → bc → 成本合计 → 重算
            new(
                TaskCode: "QT_SAP_SYNC_MA",
                TaskName: "SAP同步：物料主数据",
                JobName: "sap_sync_ma",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_ma.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 7 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 07:30 执行 wwwroot/Quartz/sap_sync_ma.sql（默认暂停；链首）"),
            new(
                TaskCode: "QT_SAP_SYNC_MD",
                TaskName: "SAP同步：机种目的地",
                JobName: "sap_sync_md",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_md.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 7 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 07:40 执行 wwwroot/Quartz/sap_sync_md.sql（默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_ST",
                TaskName: "SAP同步：标准工时",
                JobName: "sap_sync_st",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_st.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 7 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 07:50 执行 wwwroot/Quartz/sap_sync_st.sql（默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_EC",
                TaskName: "SAP同步：工程变更",
                JobName: "sap_sync_ec",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_ec.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 8 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 08:00 执行 wwwroot/Quartz/sap_sync_ec.sql（默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_SO",
                TaskName: "SAP同步：生产工单",
                JobName: "sap_sync_so",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_so.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 8 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 08:10 执行 wwwroot/Quartz/sap_sync_so.sql（默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_PP",
                TaskName: "SAP同步：采购价格",
                JobName: "sap_sync_pp",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_pp.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 8 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 08:20 执行 wwwroot/Quartz/sap_sync_pp.sql（Sap_Data.takt_logistics_materials_purchase_price* 四级→租户库；默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_SP",
                TaskName: "SAP同步：销售价格",
                JobName: "sap_sync_sp",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_sp.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 8 * * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每日 08:30 执行 wwwroot/Quartz/sap_sync_sp.sql（Sap_Data.takt_logistics_sales_price* 四级→租户库；默认暂停；日链尾）"),
            new(
                TaskCode: "QT_SAP_SYNC_AD",
                TaskName: "SAP同步：行政区划",
                JobName: "sap_sync_ad",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_ad.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 1 * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每月 1 日 02:00 执行 wwwroot/Quartz/sap_sync_ad.sql（Sap_Data.takt_foundation_admin_division → 租户库同名表；按 DivisionCode 同步并重映射父级；默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_MB",
                TaskName: "SAP同步：移动价格",
                JobName: "sap_sync_mb",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_mb.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每月 3 日 03:00 执行 wwwroot/Quartz/sap_sync_mb.sql（PP_Sap_Mbewh → 移动价格；月度链首；默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_BC",
                TaskName: "SAP同步：BOM物料成本明细",
                JobName: "sap_sync_bc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_bc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每月 3 日 03:30 执行 wwwroot/Quartz/sap_sync_bc.sql（PP_Sap_Zp002 → BOM物料成本明细；MB 后间隔 30 分钟；默认暂停）"),
            new(
                TaskCode: "QT_SAP_SYNC_BV",
                TaskName: "SAP同步：BOM物料成本汇总",
                JobName: "sap_sync_bv",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sap_sync_bv.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 45 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                Description: "每月 3 日 03:45 执行 wwwroot/Quartz/sap_sync_bv.sql（Sap_Data.takt_logistics_manufacturing_bom_material_cost → 租户库同名表；BC 后间隔 15 分钟；默认暂停）"),
            new(
                TaskCode: "QT_BOM_MATERIAL_COST_SUM",
                TaskName: "BOM物料成本合计",
                JobName: "bom_material_cost_sum",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 4 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomMaterialCostSumJobHandler),
                Description: "每月 3 日 04:00 仅合计 CostingDate 当月（完成后落库消息并推送；BV 后间隔 15 分钟；默认暂停）"),
            new(
                TaskCode: "QT_BOM_MATERIAL_COST_RECALC",
                TaskName: "BOM物料成本重算",
                JobName: "bom_material_cost_recalc",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 4 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomMaterialCostRecalculateJobHandler),
                Description: "每月 3 日 04:30 force 重算 CostingDate 当月（完成后落库消息并推送；合计后间隔 30 分钟；默认暂停；链尾）"),
        };
    }

    /// <summary>
    /// 创建或更新定时任务示例
    /// </summary>
    /// <param name="repository">定时任务仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="template">任务模板</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktQuartzTask Task, int InsertCount, int UpdateCount)> CreateOrUpdateQuartzTaskAsync(
        ITaktCompanySeedRepository<TaktQuartzTask> repository,
        string tenantCode,
        string companyCode,
        QuartzTaskSeedTemplate template)
    {
        var entity = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.TaskCode == template.TaskCode);
        if (entity == null)
        {
            entity = new TaktQuartzTask
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                TaskCode = template.TaskCode,
                TaskName = template.TaskName,
                JobName = template.JobName,
                JobGroup = "DEFAULT",
                TaskType = template.TaskType,
                AssemblyName = template.AssemblyName ?? string.Empty,
                ClassName = template.ClassName ?? string.Empty,
                ApiUrl = template.ApiUrl,
                RequestMethod = template.RequestMethod,
                SqlScript = template.SqlScript,
                TriggerType = template.TriggerType,
                CronExpression = template.CronExpression,
                IntervalSeconds = template.IntervalSeconds,
                ExecuteParams = template.ExecuteParams,
                TaskStatus = template.TaskStatus,
                Concurrent = 0,
                MisfirePolicy = 0,
                TaskDescription = template.Description,
                Remark = "系统内置示例任务种子",
            };
            entity = await repository.CreateAsync(entity);
            return (entity, 1, 0);
        }
        // 仅刷新定义字段；TaskStatus 由管理页启停维护，禁止种子每次启动写回「暂停」导致已启用任务丢失调度
        entity.TaskName = template.TaskName;
        entity.JobName = template.JobName;
        entity.JobGroup = "DEFAULT";
        entity.TaskType = template.TaskType;
        entity.AssemblyName = template.AssemblyName ?? string.Empty;
        entity.ClassName = template.ClassName ?? string.Empty;
        entity.ApiUrl = template.ApiUrl;
        entity.RequestMethod = template.RequestMethod;
        entity.SqlScript = template.SqlScript;
        entity.TriggerType = template.TriggerType;
        entity.CronExpression = template.CronExpression;
        entity.IntervalSeconds = template.IntervalSeconds;
        entity.ExecuteParams = template.ExecuteParams;
        entity.TaskDescription = template.Description;
        entity.Remark = "系统内置示例任务种子";
        await repository.UpdateAsync(entity);
        return (entity, 0, 1);
    }

    /// <summary>
    /// Quartz 任务种子模板
    /// </summary>
    /// <param name="TaskCode">任务编码</param>
    /// <param name="TaskName">任务名称</param>
    /// <param name="JobName">Job 名称</param>
    /// <param name="TaskType">任务类型</param>
    /// <param name="SqlScript">相对 wwwroot 的 .sql 路径（如 Quartz/sap_sync_ma.sql；禁止内联 SQL）</param>
    /// <param name="ApiUrl">API 地址</param>
    /// <param name="RequestMethod">请求方式</param>
    /// <param name="TriggerType">触发器类型</param>
    /// <param name="IntervalSeconds">间隔秒数</param>
    /// <param name="CronExpression">Cron 表达式</param>
    /// <param name="TaskStatus">任务状态</param>
    /// <param name="Description">任务描述</param>
    /// <param name="AssemblyName">程序集名</param>
    /// <param name="ClassName">Handler 类名</param>
    /// <param name="ExecuteParams">执行参数</param>
    private sealed record QuartzTaskSeedTemplate(
        string TaskCode,
        string TaskName,
        string JobName,
        string TaskType,
        string? SqlScript = null,
        string? ApiUrl = null,
        string? RequestMethod = null,
        int TriggerType = 0,
        int IntervalSeconds = 0,
        string CronExpression = "",
        int TaskStatus = 1,
        string? Description = null,
        string? AssemblyName = null,
        string? ClassName = null,
        string? ExecuteParams = null);
}
