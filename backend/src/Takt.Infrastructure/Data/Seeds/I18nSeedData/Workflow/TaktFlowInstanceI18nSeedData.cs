// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowInstanceI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowInstance 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow;

/// <summary>
/// TaktFlowInstance 实体国际化翻译种子（键前缀 entity.flowInstance.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowInstanceI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktFlowInstance 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowInstance 实体翻译...", tenantCode);

        foreach (var item in GetFlowInstanceTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktFlowInstance 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowInstance 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowInstance._self / entity.flowInstance.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowInstanceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowInstance._self
            new TranslationSeedItem("entity.flowInstance._self", "en-US", "Flow Instance Information", "实体名称"),
            // entity.flowInstance._self
            new TranslationSeedItem("entity.flowInstance._self", "ja-JP", "流程实例信息", "实体名称"),
            // entity.flowInstance._self
            new TranslationSeedItem("entity.flowInstance._self", "zh-CN", "流程实例信息", "实体名称"),
            // entity.flowInstance._self
            new TranslationSeedItem("entity.flowInstance._self", "zh-HK", "流程实例信息", "实体名称"),

            // entity.flowInstance.instancecode
            new TranslationSeedItem("entity.flowInstance.instancecode", "en-US", "实例编码", "实例编码（对外业务单号）"),
            // entity.flowInstance.instancecode
            new TranslationSeedItem("entity.flowInstance.instancecode", "ja-JP", "实例编码", "实例编码（对外业务单号）"),
            // entity.flowInstance.instancecode
            new TranslationSeedItem("entity.flowInstance.instancecode", "zh-CN", "实例编码", "实例编码（对外业务单号）"),
            // entity.flowInstance.instancecode
            new TranslationSeedItem("entity.flowInstance.instancecode", "zh-HK", "实例编码", "实例编码（对外业务单号）"),

            // entity.flowInstance.processdefinitionid
            new TranslationSeedItem("entity.flowInstance.processdefinitionid", "en-US", "流程定义ID", "流程定义 ID（<see cref=\"TaktFlowScheme\"/> Id）"),
            // entity.flowInstance.processdefinitionid
            new TranslationSeedItem("entity.flowInstance.processdefinitionid", "ja-JP", "流程定义ID", "流程定义 ID（<see cref=\"TaktFlowScheme\"/> Id）"),
            // entity.flowInstance.processdefinitionid
            new TranslationSeedItem("entity.flowInstance.processdefinitionid", "zh-CN", "流程定义ID", "流程定义 ID（<see cref=\"TaktFlowScheme\"/> Id）"),
            // entity.flowInstance.processdefinitionid
            new TranslationSeedItem("entity.flowInstance.processdefinitionid", "zh-HK", "流程定义ID", "流程定义 ID（<see cref=\"TaktFlowScheme\"/> Id）"),

            // entity.flowInstance.processkey
            new TranslationSeedItem("entity.flowInstance.processkey", "en-US", "流程键", "流程键（冗余）"),
            // entity.flowInstance.processkey
            new TranslationSeedItem("entity.flowInstance.processkey", "ja-JP", "流程键", "流程键（冗余）"),
            // entity.flowInstance.processkey
            new TranslationSeedItem("entity.flowInstance.processkey", "zh-CN", "流程键", "流程键（冗余）"),
            // entity.flowInstance.processkey
            new TranslationSeedItem("entity.flowInstance.processkey", "zh-HK", "流程键", "流程键（冗余）"),

            // entity.flowInstance.processname
            new TranslationSeedItem("entity.flowInstance.processname", "en-US", "流程名称", "流程名称（冗余）"),
            // entity.flowInstance.processname
            new TranslationSeedItem("entity.flowInstance.processname", "ja-JP", "流程名称", "流程名称（冗余）"),
            // entity.flowInstance.processname
            new TranslationSeedItem("entity.flowInstance.processname", "zh-CN", "流程名称", "流程名称（冗余）"),
            // entity.flowInstance.processname
            new TranslationSeedItem("entity.flowInstance.processname", "zh-HK", "流程名称", "流程名称（冗余）"),

            // entity.flowInstance.definitionversion
            new TranslationSeedItem("entity.flowInstance.definitionversion", "en-US", "定义版本号", "发起时锁定的定义版本号"),
            // entity.flowInstance.definitionversion
            new TranslationSeedItem("entity.flowInstance.definitionversion", "ja-JP", "定义版本号", "发起时锁定的定义版本号"),
            // entity.flowInstance.definitionversion
            new TranslationSeedItem("entity.flowInstance.definitionversion", "zh-CN", "定义版本号", "发起时锁定的定义版本号"),
            // entity.flowInstance.definitionversion
            new TranslationSeedItem("entity.flowInstance.definitionversion", "zh-HK", "定义版本号", "发起时锁定的定义版本号"),

            // entity.flowInstance.processtitle
            new TranslationSeedItem("entity.flowInstance.processtitle", "en-US", "申请标题", "申请标题"),
            // entity.flowInstance.processtitle
            new TranslationSeedItem("entity.flowInstance.processtitle", "ja-JP", "申请标题", "申请标题"),
            // entity.flowInstance.processtitle
            new TranslationSeedItem("entity.flowInstance.processtitle", "zh-CN", "申请标题", "申请标题"),
            // entity.flowInstance.processtitle
            new TranslationSeedItem("entity.flowInstance.processtitle", "zh-HK", "申请标题", "申请标题"),

            // entity.flowInstance.instancestatus
            new TranslationSeedItem("entity.flowInstance.instancestatus", "en-US", "实例状态", "实例状态"),
            // entity.flowInstance.instancestatus
            new TranslationSeedItem("entity.flowInstance.instancestatus", "ja-JP", "实例状态", "实例状态"),
            // entity.flowInstance.instancestatus
            new TranslationSeedItem("entity.flowInstance.instancestatus", "zh-CN", "实例状态", "实例状态"),
            // entity.flowInstance.instancestatus
            new TranslationSeedItem("entity.flowInstance.instancestatus", "zh-HK", "实例状态", "实例状态"),

            // entity.flowInstance.currentactivityid
            new TranslationSeedItem("entity.flowInstance.currentactivityid", "en-US", "当前节点ID", "当前节点 ID（设计器 nodeId）"),
            // entity.flowInstance.currentactivityid
            new TranslationSeedItem("entity.flowInstance.currentactivityid", "ja-JP", "当前节点ID", "当前节点 ID（设计器 nodeId）"),
            // entity.flowInstance.currentactivityid
            new TranslationSeedItem("entity.flowInstance.currentactivityid", "zh-CN", "当前节点ID", "当前节点 ID（设计器 nodeId）"),
            // entity.flowInstance.currentactivityid
            new TranslationSeedItem("entity.flowInstance.currentactivityid", "zh-HK", "当前节点ID", "当前节点 ID（设计器 nodeId）"),

            // entity.flowInstance.currentactivityname
            new TranslationSeedItem("entity.flowInstance.currentactivityname", "en-US", "当前节点名称", "当前节点名称"),
            // entity.flowInstance.currentactivityname
            new TranslationSeedItem("entity.flowInstance.currentactivityname", "ja-JP", "当前节点名称", "当前节点名称"),
            // entity.flowInstance.currentactivityname
            new TranslationSeedItem("entity.flowInstance.currentactivityname", "zh-CN", "当前节点名称", "当前节点名称"),
            // entity.flowInstance.currentactivityname
            new TranslationSeedItem("entity.flowInstance.currentactivityname", "zh-HK", "当前节点名称", "当前节点名称"),

            // entity.flowInstance.startuserid
            new TranslationSeedItem("entity.flowInstance.startuserid", "en-US", "发起人ID", "发起人 ID"),
            // entity.flowInstance.startuserid
            new TranslationSeedItem("entity.flowInstance.startuserid", "ja-JP", "发起人ID", "发起人 ID"),
            // entity.flowInstance.startuserid
            new TranslationSeedItem("entity.flowInstance.startuserid", "zh-CN", "发起人ID", "发起人 ID"),
            // entity.flowInstance.startuserid
            new TranslationSeedItem("entity.flowInstance.startuserid", "zh-HK", "发起人ID", "发起人 ID"),

            // entity.flowInstance.startusername
            new TranslationSeedItem("entity.flowInstance.startusername", "en-US", "发起人姓名", "发起人姓名"),
            // entity.flowInstance.startusername
            new TranslationSeedItem("entity.flowInstance.startusername", "ja-JP", "发起人姓名", "发起人姓名"),
            // entity.flowInstance.startusername
            new TranslationSeedItem("entity.flowInstance.startusername", "zh-CN", "发起人姓名", "发起人姓名"),
            // entity.flowInstance.startusername
            new TranslationSeedItem("entity.flowInstance.startusername", "zh-HK", "发起人姓名", "发起人姓名"),

            // entity.flowInstance.starttime
            new TranslationSeedItem("entity.flowInstance.starttime", "en-US", "开始时间", "开始时间"),
            // entity.flowInstance.starttime
            new TranslationSeedItem("entity.flowInstance.starttime", "ja-JP", "开始时间", "开始时间"),
            // entity.flowInstance.starttime
            new TranslationSeedItem("entity.flowInstance.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.flowInstance.starttime
            new TranslationSeedItem("entity.flowInstance.starttime", "zh-HK", "开始时间", "开始时间"),

            // entity.flowInstance.endtime
            new TranslationSeedItem("entity.flowInstance.endtime", "en-US", "结束时间", "结束时间"),
            // entity.flowInstance.endtime
            new TranslationSeedItem("entity.flowInstance.endtime", "ja-JP", "结束时间", "结束时间"),
            // entity.flowInstance.endtime
            new TranslationSeedItem("entity.flowInstance.endtime", "zh-CN", "结束时间", "结束时间"),
            // entity.flowInstance.endtime
            new TranslationSeedItem("entity.flowInstance.endtime", "zh-HK", "结束时间", "结束时间"),

            // entity.flowInstance.durationms
            new TranslationSeedItem("entity.flowInstance.durationms", "en-US", "历时毫秒", "历时毫秒"),
            // entity.flowInstance.durationms
            new TranslationSeedItem("entity.flowInstance.durationms", "ja-JP", "历时毫秒", "历时毫秒"),
            // entity.flowInstance.durationms
            new TranslationSeedItem("entity.flowInstance.durationms", "zh-CN", "历时毫秒", "历时毫秒"),
            // entity.flowInstance.durationms
            new TranslationSeedItem("entity.flowInstance.durationms", "zh-HK", "历时毫秒", "历时毫秒"),

            // entity.flowInstance.businesskey
            new TranslationSeedItem("entity.flowInstance.businesskey", "en-US", "业务主键", "业务主键（关联业务单据 Id 等）"),
            // entity.flowInstance.businesskey
            new TranslationSeedItem("entity.flowInstance.businesskey", "ja-JP", "业务主键", "业务主键（关联业务单据 Id 等）"),
            // entity.flowInstance.businesskey
            new TranslationSeedItem("entity.flowInstance.businesskey", "zh-CN", "业务主键", "业务主键（关联业务单据 Id 等）"),
            // entity.flowInstance.businesskey
            new TranslationSeedItem("entity.flowInstance.businesskey", "zh-HK", "业务主键", "业务主键（关联业务单据 Id 等）"),

            // entity.flowInstance.businesstype
            new TranslationSeedItem("entity.flowInstance.businesstype", "en-US", "业务类型", "业务类型（由业务模块约定，用于回写）"),
            // entity.flowInstance.businesstype
            new TranslationSeedItem("entity.flowInstance.businesstype", "ja-JP", "业务类型", "业务类型（由业务模块约定，用于回写）"),
            // entity.flowInstance.businesstype
            new TranslationSeedItem("entity.flowInstance.businesstype", "zh-CN", "业务类型", "业务类型（由业务模块约定，用于回写）"),
            // entity.flowInstance.businesstype
            new TranslationSeedItem("entity.flowInstance.businesstype", "zh-HK", "业务类型", "业务类型（由业务模块约定，用于回写）"),

            // entity.flowInstance.superinstanceid
            new TranslationSeedItem("entity.flowInstance.superinstanceid", "en-US", "父流程实例ID", "父流程实例 ID（子流程场景）"),
            // entity.flowInstance.superinstanceid
            new TranslationSeedItem("entity.flowInstance.superinstanceid", "ja-JP", "父流程实例ID", "父流程实例 ID（子流程场景）"),
            // entity.flowInstance.superinstanceid
            new TranslationSeedItem("entity.flowInstance.superinstanceid", "zh-CN", "父流程实例ID", "父流程实例 ID（子流程场景）"),
            // entity.flowInstance.superinstanceid
            new TranslationSeedItem("entity.flowInstance.superinstanceid", "zh-HK", "父流程实例ID", "父流程实例 ID（子流程场景）"),

            // entity.flowInstance.deletereason
            new TranslationSeedItem("entity.flowInstance.deletereason", "en-US", "终止原因", "终止原因"),
            // entity.flowInstance.deletereason
            new TranslationSeedItem("entity.flowInstance.deletereason", "ja-JP", "终止原因", "终止原因"),
            // entity.flowInstance.deletereason
            new TranslationSeedItem("entity.flowInstance.deletereason", "zh-CN", "终止原因", "终止原因"),
            // entity.flowInstance.deletereason
            new TranslationSeedItem("entity.flowInstance.deletereason", "zh-HK", "终止原因", "终止原因"),

            // entity.flowInstance.frmdata
            new TranslationSeedItem("entity.flowInstance.frmdata", "en-US", "表单数据JSON", "表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref=\"TaktFlowVariable\"/>）"),
            // entity.flowInstance.frmdata
            new TranslationSeedItem("entity.flowInstance.frmdata", "ja-JP", "表单数据JSON", "表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref=\"TaktFlowVariable\"/>）"),
            // entity.flowInstance.frmdata
            new TranslationSeedItem("entity.flowInstance.frmdata", "zh-CN", "表单数据JSON", "表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref=\"TaktFlowVariable\"/>）"),
            // entity.flowInstance.frmdata
            new TranslationSeedItem("entity.flowInstance.frmdata", "zh-HK", "表单数据JSON", "表单数据 JSON（前端 frmData；细粒度字段可同步至 <see cref=\"TaktFlowVariable\"/>）"),

            // entity.flowInstance.formid
            new TranslationSeedItem("entity.flowInstance.formid", "en-US", "表单ID", "关联表单 ID"),
            // entity.flowInstance.formid
            new TranslationSeedItem("entity.flowInstance.formid", "ja-JP", "表单ID", "关联表单 ID"),
            // entity.flowInstance.formid
            new TranslationSeedItem("entity.flowInstance.formid", "zh-CN", "表单ID", "关联表单 ID"),
            // entity.flowInstance.formid
            new TranslationSeedItem("entity.flowInstance.formid", "zh-HK", "表单ID", "关联表单 ID"),

            // entity.flowInstance.formcode
            new TranslationSeedItem("entity.flowInstance.formcode", "en-US", "表单编码", "关联表单编码"),
            // entity.flowInstance.formcode
            new TranslationSeedItem("entity.flowInstance.formcode", "ja-JP", "表单编码", "关联表单编码"),
            // entity.flowInstance.formcode
            new TranslationSeedItem("entity.flowInstance.formcode", "zh-CN", "表单编码", "关联表单编码"),
            // entity.flowInstance.formcode
            new TranslationSeedItem("entity.flowInstance.formcode", "zh-HK", "表单编码", "关联表单编码"),

            // entity.flowInstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowInstance.processcontentsnapshot", "en-US", "流程设计快照", "流程设计快照（启动时复制，避免定义变更影响在途实例）"),
            // entity.flowInstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowInstance.processcontentsnapshot", "ja-JP", "流程设计快照", "流程设计快照（启动时复制，避免定义变更影响在途实例）"),
            // entity.flowInstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowInstance.processcontentsnapshot", "zh-CN", "流程设计快照", "流程设计快照（启动时复制，避免定义变更影响在途实例）"),
            // entity.flowInstance.processcontentsnapshot
            new TranslationSeedItem("entity.flowInstance.processcontentsnapshot", "zh-HK", "流程设计快照", "流程设计快照（启动时复制，避免定义变更影响在途实例）"),

            // entity.flowInstance.processdefinition
            new TranslationSeedItem("entity.flowInstance.processdefinition", "en-US", "流程定义", "流程定义"),
            // entity.flowInstance.processdefinition
            new TranslationSeedItem("entity.flowInstance.processdefinition", "ja-JP", "流程定义", "流程定义"),
            // entity.flowInstance.processdefinition
            new TranslationSeedItem("entity.flowInstance.processdefinition", "zh-CN", "流程定义", "流程定义"),
            // entity.flowInstance.processdefinition
            new TranslationSeedItem("entity.flowInstance.processdefinition", "zh-HK", "流程定义", "流程定义"),

            // entity.flowInstance.tasks
            new TranslationSeedItem("entity.flowInstance.tasks", "en-US", "待办任务", "待办任务"),
            // entity.flowInstance.tasks
            new TranslationSeedItem("entity.flowInstance.tasks", "ja-JP", "待办任务", "待办任务"),
            // entity.flowInstance.tasks
            new TranslationSeedItem("entity.flowInstance.tasks", "zh-CN", "待办任务", "待办任务"),
            // entity.flowInstance.tasks
            new TranslationSeedItem("entity.flowInstance.tasks", "zh-HK", "待办任务", "待办任务"),

            // entity.flowInstance.historicactivities
            new TranslationSeedItem("entity.flowInstance.historicactivities", "en-US", "流转历史", "流转历史"),
            // entity.flowInstance.historicactivities
            new TranslationSeedItem("entity.flowInstance.historicactivities", "ja-JP", "流转历史", "流转历史"),
            // entity.flowInstance.historicactivities
            new TranslationSeedItem("entity.flowInstance.historicactivities", "zh-CN", "流转历史", "流转历史"),
            // entity.flowInstance.historicactivities
            new TranslationSeedItem("entity.flowInstance.historicactivities", "zh-HK", "流转历史", "流转历史"),

            // entity.flowInstance.variables
            new TranslationSeedItem("entity.flowInstance.variables", "en-US", "流程变量", "流程变量"),
            // entity.flowInstance.variables
            new TranslationSeedItem("entity.flowInstance.variables", "ja-JP", "流程变量", "流程变量"),
            // entity.flowInstance.variables
            new TranslationSeedItem("entity.flowInstance.variables", "zh-CN", "流程变量", "流程变量"),
            // entity.flowInstance.variables
            new TranslationSeedItem("entity.flowInstance.variables", "zh-HK", "流程变量", "流程变量"),

            // entity.flowInstance.addsigns
            new TranslationSeedItem("entity.flowInstance.addsigns", "en-US", "加签记录", "加签记录"),
            // entity.flowInstance.addsigns
            new TranslationSeedItem("entity.flowInstance.addsigns", "ja-JP", "加签记录", "加签记录"),
            // entity.flowInstance.addsigns
            new TranslationSeedItem("entity.flowInstance.addsigns", "zh-CN", "加签记录", "加签记录"),
            // entity.flowInstance.addsigns
            new TranslationSeedItem("entity.flowInstance.addsigns", "zh-HK", "加签记录", "加签记录"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = TaktModule.Workflow;
        translation.ResourceType = TaktAppSide.Frontend;
        translation.ContextNote = item.ContextNote;
        translation.ExtFieldJson = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
