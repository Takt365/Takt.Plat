// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktNumberingSeedData.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：内置业务编号规则种子（日常/财务/后勤等模块；按 Database:CompanyCodes 各公司写入）
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
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 内置编号规则种子（按 Database:CompanyCodes 各公司写入；幂等：存在则更新配置，保留 CurrentSequence/ExampleCode）
/// </summary>
public class TaktNumberingSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;
    private const string SegmentsWithDepartment =
        "segments:CompanyCode,DepartmentCode,PrefixCode,DateSequence";
    private const string SegmentsCompanyLevel =
        "segments:CompanyCode,PrefixCode,DateSequence";
    private const string SegmentsCompanyNoDate =
        "segments:CompanyCode,PrefixCode,Sequence";
    private const string DefaultSeparator = "-";
    private const string DomainRoutine = "Routine";
    private const string DomainAccounting = "Accounting";
    private const string DomainLogistics = "Logistics";

    // 与 TaktIsoCodeSeedData.GetStandardIsoCodes 中 IsoCode 一致（DictValue=IsoCode）
    private const string IsoR = "R"; // 总务部 D0100
    private const string IsoF = "F"; // 财务部 D0200
    private const string IsoD = "D"; // IT部 D0300
    private const string IsoS = "S"; // 生管课 D0420
    private const string IsoB = "B"; // 部管课 D0430
    private const string IsoC = "C"; // 资材部 D0500
    private const string IsoZ = "Z"; // 制造部 D0600

    /// <summary>
    /// 执行顺序（在字典与公司基础数据之后、Quartz 示例之前）
    /// </summary>
    public int Order => 48;

    /// <summary>
    /// 初始化内置编号规则种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化内置编号规则种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过编号规则种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktNumbering>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过编号规则种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过编号规则种子", tenantCode);
            return (0, 0);
        }
        var templates = GetBuiltInRuleTemplates();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化内置编号规则...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateNumberingAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "内置编号规则种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取内置编号规则模板
    /// </summary>
    /// <returns>规则模板列表</returns>
    private static List<NumberingSeedTemplate> GetBuiltInRuleTemplates()
    {
        return
        [
            new NumberingSeedTemplate(
                "RT-ANN",
                "公告编码",
                DomainRoutine,
                IsoR,
                "ANN",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：公告通知模块（公告编码）"),
            new NumberingSeedTemplate(
                "RT-NOT",
                "通知编码",
                DomainRoutine,
                IsoR,
                "NOT",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：公告通知模块（通知编码）"),
            new NumberingSeedTemplate(
                "RT-CONF",
                "会议编码",
                DomainRoutine,
                IsoR,
                "CONF",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：会议中心 TaktConference.ConferenceCode"),
            new NumberingSeedTemplate(
                "RT-NEWS",
                "新闻编码",
                DomainRoutine,
                IsoR,
                "NEWS",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：新闻中心 TaktNews.NewsCode"),
            new NumberingSeedTemplate(
                "HD-TICKET",
                "工单编码",
                DomainRoutine,
                IsoD,
                "TKT",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：服务台 TaktTicket.TicketNo"),
            new NumberingSeedTemplate(
                "AC-ASSET",
                "资产编码",
                DomainAccounting,
                IsoF,
                "AST",
                "yyyy",
                "year",
                SegmentsWithDepartment,
                "内置：财务资产 TaktAsset.AssetCode"),
            new NumberingSeedTemplate(
                "LG-SVC-CON",
                "服务合同编码",
                DomainLogistics,
                IsoB,
                "SVCN",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktServiceContract.ServiceContractCode"),
            new NumberingSeedTemplate(
                "LG-SVC-ORD",
                "服务订单编码",
                DomainLogistics,
                IsoB,
                "SVCO",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktServiceOrder.ServiceOrderCode"),
            new NumberingSeedTemplate(
                "LG-SVC-REQ",
                "服务请求单号",
                DomainLogistics,
                IsoB,
                "SVCR",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktServiceRequest.ServiceRequestCode"),
            new NumberingSeedTemplate(
                "LG-SVC-TKT",
                "服务工单编码",
                DomainLogistics,
                IsoB,
                "SVCT",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktServiceTicket.ServiceTicketCode"),
            new NumberingSeedTemplate(
                "LG-EQP",
                "设备编码",
                DomainLogistics,
                IsoZ,
                "EQP",
                "none",
                "none",
                SegmentsWithDepartment,
                "内置：TaktEquipment.EquipmentCode"),
            new NumberingSeedTemplate(
                "LG-SLS-INV",
                "销售发票编码",
                DomainLogistics,
                IsoS,
                "SINV",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktSalesInvoice.SalesInvoiceCode"),
            new NumberingSeedTemplate(
                "LG-SLS-ORD",
                "销售订单编码",
                DomainLogistics,
                IsoS,
                "SORD",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktSalesOrder.SalesOrderCode"),
            new NumberingSeedTemplate(
                "LG-SLS-PRC",
                "销售价格编码",
                DomainLogistics,
                IsoS,
                "SPRC",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktSalesPrice.SalesPriceCode"),
            new NumberingSeedTemplate(
                "LG-SLS-QUO",
                "销售报价编码",
                DomainLogistics,
                IsoS,
                "SQUO",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktSalesQuotation.SalesQuotationCode"),
            new NumberingSeedTemplate(
                "LG-PUR-ORD",
                "采购订单编码",
                DomainLogistics,
                IsoC,
                "PUR",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktPurchaseOrder.PurchaseOrderCode"),
            new NumberingSeedTemplate(
                "LG-PUR-PRC",
                "采购价格编码",
                DomainLogistics,
                IsoC,
                "PRC",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktPurchasePrice.PurchasePriceCode"),
            new NumberingSeedTemplate(
                "LG-PUR-REQ",
                "采购申请编码",
                DomainLogistics,
                IsoC,
                "REQ",
                "yyyy",
                "year",
                SegmentsCompanyLevel,
                "内置：TaktPurchaseRequest.PurchaseRequestCode"),
            new NumberingSeedTemplate(
                "LG-SUP",
                "供货商编码",
                DomainLogistics,
                IsoC,
                "SUP",
                "none",
                "none",
                SegmentsCompanyNoDate,
                "内置：TaktSupplier.SupplierCode"),
            new NumberingSeedTemplate(
                "LG-VND",
                "经销商编码",
                DomainLogistics,
                IsoC,
                "VND",
                "none",
                "none",
                SegmentsCompanyNoDate,
                "内置：TaktVendor.VendorCode"),
            new NumberingSeedTemplate(
                "LG-CLT",
                "客户端编码",
                DomainLogistics,
                IsoB,
                "CLT",
                "none",
                "none",
                SegmentsCompanyNoDate,
                "内置：TaktClient.ClientCode"),
            new NumberingSeedTemplate(
                "LG-CUS",
                "客户编码",
                DomainLogistics,
                IsoB,
                "CUS",
                "none",
                "none",
                SegmentsCompanyNoDate,
                "内置：TaktCustomer.CustomerCode"),
        ];
    }

    /// <summary>
    /// 创建或更新编号规则（更新时保留 CurrentSequence，按最新段配置重算 ExampleCode）
    /// </summary>
    /// <param name="repository">编号规则仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="template">规则模板</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktNumbering Rule, int InsertCount, int UpdateCount)> CreateOrUpdateNumberingAsync(
        ITaktCompanySeedRepository<TaktNumbering> repository,
        string tenantCode,
        string companyCode,
        NumberingSeedTemplate template)
    {
        var entity = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.RuleCode == template.RuleCode);
        if (entity == null)
        {
            entity = new TaktNumbering
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                RuleCode = template.RuleCode,
                RuleName = template.RuleName,
                DocumentType = template.DocumentType,
                DepartmentCode = template.DepartmentCode,
                PrefixCode = template.PrefixCode,
                DateFormat = template.DateFormat,
                SequenceLength = template.SequenceLength,
                SequenceStep = template.SequenceStep,
                SuffixCode = template.SuffixCode,
                ResetPeriod = template.ResetPeriod,
                Separator = template.Separator,
                Description = template.SegmentsDescription,
                IsBuiltIn = IsBuiltInYes,
                Status = StatusEnabled,
                Remark = template.Remark,
            };
            ApplyDateFormatResetPeriodAlignment(entity);
            var (exampleCode, currentSequence) = BuildInitialExampleCode(entity, DateTime.Now);
            entity.ExampleCode = exampleCode;
            entity.CurrentSequence = currentSequence;
            entity = await repository.CreateAsync(entity);
            return (entity, 1, 0);
        }
        entity.RuleName = template.RuleName;
        entity.DocumentType = template.DocumentType;
        entity.DepartmentCode = template.DepartmentCode;
        entity.PrefixCode = template.PrefixCode;
        entity.DateFormat = template.DateFormat;
        entity.SequenceLength = template.SequenceLength;
        entity.SequenceStep = template.SequenceStep;
        entity.SuffixCode = template.SuffixCode;
        entity.ResetPeriod = template.ResetPeriod;
        entity.Separator = template.Separator;
        entity.Description = template.SegmentsDescription;
        entity.IsBuiltIn = IsBuiltInYes;
        entity.Status = StatusEnabled;
        entity.Remark = template.Remark;
        ApplyDateFormatResetPeriodAlignment(entity);
        var sequenceForExample = entity.CurrentSequence <= 0
            ? (entity.SequenceStep <= 0 ? 1 : entity.SequenceStep)
            : entity.CurrentSequence;
        entity.ExampleCode = FormatBusinessCode(entity, sequenceForExample, DateTime.Now);
        await repository.UpdateAsync(entity);
        return (entity, 0, 1);
    }

    /// <summary>
    /// 按默认段顺序生成初始起始编码（与 TaktNumberingService 创建逻辑一致）
    /// </summary>
    /// <param name="rule">编号规则（须含 TenantCode、CompanyCode 等段字段）</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>起始编码与当前流水号</returns>
    private static (string ExampleCode, int CurrentSequence) BuildInitialExampleCode(
        TaktNumbering rule,
        DateTime referenceTime)
    {
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var code = FormatBusinessCode(rule, step, referenceTime);
        return (code, step);
    }

    /// <summary>
    /// 按 DateFormat 强制对齐 ResetPeriod（与 TaktNumberingService.NormalizeNumberingRule 一致）
    /// </summary>
    /// <param name="entity">编号规则</param>
    private static void ApplyDateFormatResetPeriodAlignment(TaktNumbering entity)
    {
        if (string.Equals(entity.DateFormat?.Trim(), "none", StringComparison.OrdinalIgnoreCase))
        {
            entity.DateFormat = null;
        }
        else
        {
            entity.DateFormat = TaktNumberingHelper.NormalizeSupportedDateFormat(entity.DateFormat);
        }
        entity.ResetPeriod = TaktNumberingHelper.ResolveRequiredResetPeriod(entity.DateFormat);
        entity.DocumentType = TaktNumberingHelper.NormalizeDocumentType(entity.DocumentType);
    }

    /// <summary>
    /// 按 Description 段配置拼接业务编号（与 TaktNumberingService 一致）
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <param name="sequence">流水号</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>业务编号</returns>
    private static string FormatBusinessCode(TaktNumbering rule, int sequence, DateTime referenceTime)
    {
        var length = rule.SequenceLength <= 0 ? 6 : rule.SequenceLength;
        var separator = string.IsNullOrWhiteSpace(rule.Separator) ? DefaultSeparator : rule.Separator.Trim();
        var effectiveDescription = ResolveEffectiveSegmentDescription(rule.Description);
        var parts = new List<string>();
        foreach (var segmentKey in ParseSegmentKeys(effectiveDescription))
        {
            var part = ResolveSegmentValue(rule, segmentKey, sequence, length, referenceTime);
            if (!string.IsNullOrWhiteSpace(part))
            {
                parts.Add(part);
            }
        }
        var code = string.IsNullOrEmpty(separator)
            ? string.Concat(parts)
            : string.Join(separator, parts);
        var suffixCode = rule.SuffixCode?.Trim();
        if (!string.IsNullOrWhiteSpace(suffixCode))
        {
            code += suffixCode;
        }
        return code;
    }

    /// <summary>
    /// 从 segments 配置解析段键列表
    /// </summary>
    /// <param name="segmentsDescription">segments: 配置全文</param>
    /// <returns>段键列表</returns>
    private static IReadOnlyList<string> ParseSegmentKeys(string segmentsDescription)
    {
        return TryParseSegmentKeysFromDescription(segmentsDescription) ?? new[]
        {
            nameof(TaktNumbering.CompanyCode),
            nameof(TaktNumbering.DepartmentCode),
            nameof(TaktNumbering.PrefixCode),
            "DateSequence",
        };
    }

    /// <summary>
    /// 将旧版 segments 解析为有效段配置（与 TaktNumberingService 一致）
    /// </summary>
    /// <param name="description">原始 Description</param>
    /// <returns>有效段配置</returns>
    private static string ResolveEffectiveSegmentDescription(string? description)
    {
        var text = description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text)
            || !text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return SegmentsWithDepartment;
        }
        var body = text["segments:".Length..];
        var isLegacy = body.Contains("DocumentType", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateFormat,Sequence", StringComparison.OrdinalIgnoreCase);
        if (!isLegacy)
        {
            return text;
        }
        var hasDepartment = body.Contains("DepartmentCode", StringComparison.OrdinalIgnoreCase);
        var hasDate = body.Contains("DateFormat", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateSequence", StringComparison.OrdinalIgnoreCase);
        if (hasDepartment)
        {
            return hasDate ? SegmentsWithDepartment : "segments:CompanyCode,DepartmentCode,PrefixCode,Sequence";
        }
        return hasDate ? SegmentsCompanyLevel : SegmentsCompanyNoDate;
    }

    /// <summary>
    /// 解析单个编码段文本
    /// </summary>
    private static string? ResolveSegmentValue(
        TaktNumbering rule,
        string segmentKey,
        int sequence,
        int sequenceLength,
        DateTime time)
    {
        if (segmentKey.Equals("Sequence", StringComparison.OrdinalIgnoreCase))
        {
            return sequence.ToString().PadLeft(sequenceLength, '0');
        }
        if (segmentKey.Equals("DateSequence", StringComparison.OrdinalIgnoreCase))
        {
            var datePart = FormatDateSegment(rule.DateFormat, time);
            var seqPart = sequence.ToString().PadLeft(sequenceLength, '0');
            var combined = string.Concat(datePart, seqPart);
            return string.IsNullOrWhiteSpace(combined) ? null : combined;
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DateFormat), StringComparison.OrdinalIgnoreCase))
        {
            return FormatDateSegment(rule.DateFormat, time);
        }
        if (segmentKey.Equals(nameof(TaktNumbering.PrefixCode), StringComparison.OrdinalIgnoreCase)
            || segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
        {
            var prefixCode = rule.PrefixCode?.Trim();
            return string.IsNullOrWhiteSpace(prefixCode) ? null : prefixCode.TrimEnd('-', '_', '/', ' ');
        }
        if (segmentKey.Equals(nameof(TaktNumbering.CompanyCode), StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.CompanyCode) ? null : rule.CompanyCode.Trim();
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DepartmentCode), StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.DepartmentCode) ? null : rule.DepartmentCode.Trim();
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DocumentType), StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.DocumentType) ? null : rule.DocumentType.Trim();
        }
        return null;
    }

    /// <summary>
    /// 从 Description 解析 segments: 段配置
    /// </summary>
    private static string[]? TryParseSegmentKeysFromDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }
        var text = description.Trim();
        const string prefix = "segments:";
        if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        var body = text[prefix.Length..].Trim();
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }
        var keys = body.Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return keys.Length == 0 ? null : keys;
    }

    /// <summary>
    /// 按 DateFormat 生成日期段
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="time">参考时间</param>
    /// <returns>日期段文本；不使用日期时返回空串</returns>
    private static string FormatDateSegment(string? dateFormat, DateTime time)
    {
        if (string.IsNullOrWhiteSpace(dateFormat)
            || dateFormat.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }
        return dateFormat.Trim() switch
        {
            "yyyy" => time.ToString("yyyy"),
            "yyyyMM" => time.ToString("yyyyMM"),
            "yyyyMMdd" => time.ToString("yyyyMMdd"),
            "yyyyMMddHH" => time.ToString("yyyyMMddHH"),
            _ => time.ToString(dateFormat.Trim()),
        };
    }

    /// <summary>
    /// 编号规则种子模板
    /// </summary>
    /// <param name="RuleCode">规则编码</param>
    /// <param name="RuleName">规则名称</param>
    /// <param name="DocumentType">业务领域（如 Routine、Accounting、Logistics）</param>
    /// <param name="DepartmentCode">ISO 单字母编码（与 TaktIsoCodeSeedData.IsoCode 一致）</param>
    /// <param name="PrefixCode">前缀编码</param>
    /// <param name="DateFormat">日期格式</param>
    /// <param name="ResetPeriod">重置周期</param>
    /// <param name="SegmentsDescription">编码段配置（segments:…）</param>
    /// <param name="Remark">备注</param>
    /// <param name="SequenceLength">流水位数</param>
    /// <param name="SequenceStep">流水步长</param>
    /// <param name="SuffixCode">后缀编码</param>
    /// <param name="Separator">段间分隔符</param>
    private sealed record NumberingSeedTemplate(
        string RuleCode,
        string RuleName,
        string DocumentType,
        string DepartmentCode,
        string PrefixCode,
        string DateFormat,
        string ResetPeriod,
        string SegmentsDescription,
        string Remark,
        int SequenceLength = 6,
        int SequenceStep = 1,
        string? SuffixCode = null,
        string Separator = DefaultSeparator);
}
