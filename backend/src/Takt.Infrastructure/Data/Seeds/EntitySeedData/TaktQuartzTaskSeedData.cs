// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktQuartzTaskSeedData.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务种子（日链自动同步 + 月度以手动为主；默认暂停）
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
    /// <summary>Sap_Data 日链 / sync_desc：仅目标库</summary>
    private const string SyncTargetOnlyParams = "{\"targetDatabase\":\"zTakt_000_Dev\"}";
    /// <summary>zTakt_900 暂存同步：源库 + 目标库</summary>
    private const string SyncStagingParams =
        "{\"sourceDatabase\":\"zTakt_900_Dev\",\"targetDatabase\":\"zTakt_000_Dev\"}";

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
            // 旧码 QT_SAP_SYNC_* / 极旧短码 → QT_SYNC_*（去掉 SAP 前缀）
            foreach (var mig in GetLegacySyncTaskMigrations())
            {
                await MigrateLegacySyncTaskAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    mig.LegacyTaskCode,
                    mig.NewTaskCode,
                    mig.TaskName,
                    mig.JobName,
                    mig.SqlScript,
                    mig.CronExpression,
                    mig.Description);
            }
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateQuartzTaskAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
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
            // ========== 日链自动同步（①→⑤；默认暂停，启用后按 Cron）==========
            new(
                TaskCode: "QT_SYNC_MATPLT",
                TaskName: "源数据同步：工厂物料",
                JobName: "sync_matplt",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matplt.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 7 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 07:40 自动同步：工厂物料（日链①；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MDL",
                TaskName: "源数据同步：机种目的地",
                JobName: "sync_mdl",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mdl.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 7 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 07:50 自动同步：机种目的地（日链②；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_ST",
                TaskName: "源数据同步：标准工时",
                JobName: "sync_st",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_st.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:00 自动同步：标准工时（日链③；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_EC",
                TaskName: "源数据同步：工程变更",
                JobName: "sync_ec",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_ec.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:10 自动同步：工程变更（日链④；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MO",
                TaskName: "源数据同步：生产工单",
                JobName: "sync_mo",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mo.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 8 * * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每日 08:20 自动同步：生产工单（日链⑤；默认暂停）"),
            // ========== 月度同步（以手动为主；默认暂停）==========
            new(
                TaskCode: "QT_SYNC_MAT",
                TaskName: "源数据同步：物料主数据",
                JobName: "sync_mat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:00：物料主数据（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_CUS",
                TaskName: "源数据同步：客户信息",
                JobName: "sync_cus",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_cus.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:10：客户信息（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_SUP",
                TaskName: "源数据同步：供货商信息",
                JobName: "sync_sup",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sup.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:20：供货商信息（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PUP",
                TaskName: "源数据同步：采购价格",
                JobName: "sync_pup",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_pup.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:30：采购价格（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_SP",
                TaskName: "源数据同步：销售价格",
                JobName: "sync_sp",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sp.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 40 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:40：销售价格（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_DESC",
                TaskName: "源数据同步：采销/机种物料描述回填",
                JobName: "sync_desc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_desc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 45 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncTargetOnlyParams,
                Description: "每月 3 日 02:45：回填销售/采购价格空物料描述（zh-CN→Z1→ja-JP）；机种 MaterialDescription=ja-JP（按物料码）、ModelName=Z1（按机种码）；建议在 QT_SYNC_PUP/SP/MDL 之后；默认暂停"),
            new(
                TaskCode: "QT_SYNC_MATDOC",
                TaskName: "源数据同步：物料凭证",
                JobName: "sync_matdoc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matdoc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 50 2 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 02:50：物料凭证（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_PUINV",
                TaskName: "源数据同步：采购发票",
                JobName: "sync_puinv",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_puinv.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:00：采购发票（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_SDINV",
                TaskName: "源数据同步：销售发票",
                JobName: "sync_sdinv",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_sdinv.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:10：销售发票（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_FC",
                TaskName: "源数据同步：销售预测",
                JobName: "sync_fc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_fc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 20 1 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 01:20：销售预测（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_AD",
                TaskName: "源数据同步：行政区划",
                JobName: "sync_ad",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_ad.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 2 1 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 1 日 02:00：行政区划（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MAP",
                TaskName: "源数据同步：移动价格",
                JobName: "sync_map",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_map.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:00：移动价格（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MATPKG",
                TaskName: "源数据同步：包装物料",
                JobName: "sync_matpkg",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_matpkg.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 5 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:05：包装物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_MFRMAT",
                TaskName: "源数据同步：制造商物料",
                JobName: "sync_mfrmat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_mfrmat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 10 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:10：制造商物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_DISTMAT",
                TaskName: "源数据同步：销售商物料",
                JobName: "sync_distmat",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_distmat.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 15 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:15：销售商物料（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BC",
                TaskName: "源数据同步：BOM物料成本明细",
                JobName: "sync_bc",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bc.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 30 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:30：BOM物料成本明细（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_SYNC_BV",
                TaskName: "源数据同步：BOM物料成本汇总",
                JobName: "sync_bv",
                TaskType: TaskTypeSql,
                SqlScript: "Quartz/sync_bv.sql",
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 45 3 3 * ?",
                TaskStatus: TaskStatusPaused,
                ExecuteParams: SyncStagingParams,
                Description: "每月 3 日 03:45：BOM物料成本汇总（先回填机种+物料类型再同步，再按物料类型+机种重算月均；须先 QT_SYNC_MDL；月度；以手动为主；默认暂停）"),
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
                Description: "每月 3 日 04:00 合计 CostingDate 当月（月度；以手动为主；默认暂停）"),
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
                Description: "每月 3 日 04:30 force 重算 CostingDate 当月（月度；以手动为主；默认暂停）"),
            new(
                TaskCode: "QT_BOM_MODEL_AVG_COST",
                TaskName: "BOM机种平均成本",
                JobName: "bom_model_avg_cost",
                TaskType: TaskTypeAssembly,
                TriggerType: TriggerTypeCron,
                IntervalSeconds: 0,
                CronExpression: "0 0 5 3 * ?",
                TaskStatus: TaskStatusPaused,
                AssemblyName: "Takt.Infrastructure",
                ClassName: nameof(TaktBomModelAvgCostJobHandler),
                Description: "每月 3 日 05:00：先回填机种+物料类型，再按物料类型+机种+核算月份重算机种月均（须先 QT_SYNC_MDL/MATPLT 与主表有当月数据；月度；以手动为主；默认暂停）"),
        };
    }

    /// <summary>
    /// 旧任务编码 → QT_SYNC_* 迁移清单（含极旧短码与 QT_SAP_SYNC_*）
    /// </summary>
    /// <returns>迁移项</returns>
    private static IReadOnlyList<LegacySyncTaskMigration> GetLegacySyncTaskMigrations()
    {
        // 目标定义与模板一致；多旧码可指向同一新码
        return new List<LegacySyncTaskMigration>
        {
            new("QT_SAP_SYNC_MA", "QT_SYNC_MATPLT", "源数据同步：工厂物料", "sync_matplt", "Quartz/sync_matplt.sql", "0 40 7 * * ?", "每日 07:40 自动同步：工厂物料（日链①；默认暂停）"),
            new("QT_SAP_SYNC_MATPLT", "QT_SYNC_MATPLT", "源数据同步：工厂物料", "sync_matplt", "Quartz/sync_matplt.sql", "0 40 7 * * ?", "每日 07:40 自动同步：工厂物料（日链①；默认暂停）"),
            new("QT_SAP_SYNC_MD", "QT_SYNC_MDL", "源数据同步：机种目的地", "sync_mdl", "Quartz/sync_mdl.sql", "0 50 7 * * ?", "每日 07:50 自动同步：机种目的地（日链②；默认暂停）"),
            new("QT_SAP_SYNC_MDL", "QT_SYNC_MDL", "源数据同步：机种目的地", "sync_mdl", "Quartz/sync_mdl.sql", "0 50 7 * * ?", "每日 07:50 自动同步：机种目的地（日链②；默认暂停）"),
            new("QT_SAP_SYNC_ST", "QT_SYNC_ST", "源数据同步：标准工时", "sync_st", "Quartz/sync_st.sql", "0 0 8 * * ?", "每日 08:00 自动同步：标准工时（日链③；默认暂停）"),
            new("QT_SAP_SYNC_EC", "QT_SYNC_EC", "源数据同步：工程变更", "sync_ec", "Quartz/sync_ec.sql", "0 10 8 * * ?", "每日 08:10 自动同步：工程变更（日链④；默认暂停）"),
            new("QT_SAP_SYNC_SO", "QT_SYNC_MO", "源数据同步：生产工单", "sync_mo", "Quartz/sync_mo.sql", "0 20 8 * * ?", "每日 08:20 自动同步：生产工单（日链⑤；默认暂停）"),
            new("QT_SAP_SYNC_MO", "QT_SYNC_MO", "源数据同步：生产工单", "sync_mo", "Quartz/sync_mo.sql", "0 20 8 * * ?", "每日 08:20 自动同步：生产工单（日链⑤；默认暂停）"),
            new("QT_SAP_SYNC_MAT", "QT_SYNC_MAT", "源数据同步：物料主数据", "sync_mat", "Quartz/sync_mat.sql", "0 0 2 3 * ?", "每月 3 日 02:00：物料主数据（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_CUS", "QT_SYNC_CUS", "源数据同步：客户信息", "sync_cus", "Quartz/sync_cus.sql", "0 10 2 3 * ?", "每月 3 日 02:10：客户信息（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_SUP", "QT_SYNC_SUP", "源数据同步：供货商信息", "sync_sup", "Quartz/sync_sup.sql", "0 20 2 3 * ?", "每月 3 日 02:20：供货商信息（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_PP", "QT_SYNC_PUP", "源数据同步：采购价格", "sync_pup", "Quartz/sync_pup.sql", "0 30 2 3 * ?", "每月 3 日 02:30：采购价格（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_PUP", "QT_SYNC_PUP", "源数据同步：采购价格", "sync_pup", "Quartz/sync_pup.sql", "0 30 2 3 * ?", "每月 3 日 02:30：采购价格（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_SP", "QT_SYNC_SP", "源数据同步：销售价格", "sync_sp", "Quartz/sync_sp.sql", "0 40 2 3 * ?", "每月 3 日 02:40：销售价格（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_MATDOC", "QT_SYNC_MATDOC", "源数据同步：物料凭证", "sync_matdoc", "Quartz/sync_matdoc.sql", "0 50 2 3 * ?", "每月 3 日 02:50：物料凭证（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_PUINV", "QT_SYNC_PUINV", "源数据同步：采购发票", "sync_puinv", "Quartz/sync_puinv.sql", "0 0 1 3 * ?", "每月 3 日 01:00：采购发票（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_SDINV", "QT_SYNC_SDINV", "源数据同步：销售发票", "sync_sdinv", "Quartz/sync_sdinv.sql", "0 10 1 3 * ?", "每月 3 日 01:10：销售发票（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_FC", "QT_SYNC_FC", "源数据同步：销售预测", "sync_fc", "Quartz/sync_fc.sql", "0 20 1 3 * ?", "每月 3 日 01:20：销售预测（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_AD", "QT_SYNC_AD", "源数据同步：行政区划", "sync_ad", "Quartz/sync_ad.sql", "0 0 2 1 * ?", "每月 1 日 02:00：行政区划（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_MB", "QT_SYNC_MAP", "源数据同步：移动价格", "sync_map", "Quartz/sync_map.sql", "0 0 3 3 * ?", "每月 3 日 03:00：移动价格（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_MAP", "QT_SYNC_MAP", "源数据同步：移动价格", "sync_map", "Quartz/sync_map.sql", "0 0 3 3 * ?", "每月 3 日 03:00：移动价格（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_MATPKG", "QT_SYNC_MATPKG", "源数据同步：包装物料", "sync_matpkg", "Quartz/sync_matpkg.sql", "0 5 3 3 * ?", "每月 3 日 03:05：包装物料（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_MFRMAT", "QT_SYNC_MFRMAT", "源数据同步：制造商物料", "sync_mfrmat", "Quartz/sync_mfrmat.sql", "0 10 3 3 * ?", "每月 3 日 03:10：制造商物料（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_DISTMAT", "QT_SYNC_DISTMAT", "源数据同步：销售商物料", "sync_distmat", "Quartz/sync_distmat.sql", "0 15 3 3 * ?", "每月 3 日 03:15：销售商物料（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_BC", "QT_SYNC_BC", "源数据同步：BOM物料成本明细", "sync_bc", "Quartz/sync_bc.sql", "0 30 3 3 * ?", "每月 3 日 03:30：BOM物料成本明细（月度；以手动为主；默认暂停）"),
            new("QT_SAP_SYNC_BV", "QT_SYNC_BV", "源数据同步：BOM物料成本汇总", "sync_bv", "Quartz/sync_bv.sql", "0 45 3 3 * ?", "每月 3 日 03:45：BOM物料成本汇总（先回填机种+物料类型再同步，再按物料类型+机种重算月均；须先 QT_SYNC_MDL；月度；以手动为主；默认暂停）"),
        };
    }

    /// <summary>
    /// 将旧 TaskCode 迁移为新 TaskCode（目标不存在则改名，已存在则软删旧任务）
    /// </summary>
    private static async Task MigrateLegacySyncTaskAsync(
        ITaktCompanySeedRepository<TaktQuartzTask> repository,
        string tenantCode,
        string companyCode,
        string legacyTaskCode,
        string newTaskCode,
        string taskName,
        string jobName,
        string sqlScript,
        string cronExpression,
        string description)
    {
        var legacy = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.TaskCode == legacyTaskCode
            && x.IsDeleted == 0);
        if (legacy == null)
        {
            return;
        }
        var target = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.TaskCode == newTaskCode
            && x.IsDeleted == 0);
        if (target == null)
        {
            legacy.TaskCode = newTaskCode;
            legacy.TaskName = taskName;
            legacy.JobName = jobName;
            legacy.SqlScript = sqlScript;
            legacy.CronExpression = cronExpression;
            legacy.TaskDescription = description;
            await repository.UpdateAsync(legacy);
            TaktLogger.Information(
                "已迁移 Quartz 任务 {Legacy} → {New}（Tenant={TenantCode}, Company={CompanyCode}）",
                legacyTaskCode,
                newTaskCode,
                tenantCode,
                companyCode);
            return;
        }
        legacy.IsDeleted = 1;
        legacy.DeletedAt = DateTime.Now;
        legacy.TaskDescription = $"已废弃：请使用 {newTaskCode}";
        await repository.UpdateAsync(legacy);
        TaktLogger.Information(
            "已软删重复旧任务 {Legacy}（Tenant={TenantCode}, Company={CompanyCode}）",
            legacyTaskCode,
            tenantCode,
            companyCode);
    }

    /// <summary>
    /// 旧同步任务迁移项
    /// </summary>
    private sealed record LegacySyncTaskMigration(
        string LegacyTaskCode,
        string NewTaskCode,
        string TaskName,
        string JobName,
        string SqlScript,
        string CronExpression,
        string Description);

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
        string cultureCode,
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
                CultureCode = cultureCode
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
    /// <param name="SqlScript">相对 wwwroot 的 .sql 路径（如 Quartz/sync_mat.sql；禁止内联 SQL）</param>
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
