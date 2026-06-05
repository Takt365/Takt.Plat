// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktNumberingGenerator.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：业务单据编号生成器实现（基于 TaktNumbering 实体反射拼接）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Concurrent;
using System.Reflection;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// <see cref="ITaktNumberingGenerator"/> 实现
/// 按 <see cref="TaktNumbering"/> 规则字段反射取值拼接业务编号；段顺序默认与实体注释一致，可由 Description 中 <c>segments:…</c> 覆盖
/// </summary>
public sealed class TaktNumberingGenerator : TaktServiceBase, ITaktNumberingGenerator
{
    /// <summary>
    /// 默认编码段顺序：单据类型、公司、部门、前缀、日期、流水号
    /// </summary>
    private static readonly string[] DefaultSegmentKeys =
    {
        nameof(TaktNumbering.DocumentType),
        nameof(TaktCompanyEntityBase.CompanyCode),
        nameof(TaktNumbering.DepartmentCode),
        nameof(TaktNumbering.Prefix),
        nameof(TaktNumbering.DateFormat),
        "Sequence",
    };

    /// <summary>
    /// 编号段属性反射缓存（段名 → PropertyInfo）
    /// </summary>
    private static readonly ConcurrentDictionary<string, PropertyInfo?> SegmentPropertyCache =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>编号规则仓储</summary>
    private readonly ITaktCompanyRepository<TaktNumbering> _numberingRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编号规则仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNumberingGenerator(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _numberingRepository = numberingRepository;
    }

    /// <summary>
    /// 预览业务编号（不占用流水号、不写库）
    /// </summary>
    /// <param name="request">预览参数（规则 Id、规则编码或草稿字段）</param>
    /// <returns>预览结果</returns>
    public async Task<TaktNumberingPreviewResultDto> PreviewNumberingAsync(TaktNumberingPreviewRequestDto request)
    {
        EnsureThreeLayerContext();
        var rule = await ResolveRuleForPreviewAsync(request);
        var now = DateTime.Now;
        var sequence = request.SequenceOverride ?? ComputePreviewSequence(rule, now);
        return new TaktNumberingPreviewResultDto
        {
            BusinessCode = FormatBusinessCode(rule, sequence, now),
            NextSequence = sequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 生成下一个业务编号（递增 CurrentSequence 并写回规则）
    /// </summary>
    /// <param name="request">生成参数（须含 RuleCode）</param>
    /// <returns>生成结果</returns>
    public async Task<TaktNumberingGenerateResultDto> GenerateNumberingAsync(TaktNumberingGenerateRequestDto request)
    {
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(request.RuleCode))
        {
            throw new TaktBusinessException("规则编码不能为空");
        }
        var rule = await LoadActiveRuleByCodeAsync(request.RuleCode.Trim());
        var now = DateTime.Now;
        if (ShouldResetSequence(rule.ResetPeriod, rule.UpdatedAt, now))
        {
            rule.CurrentSequence = 0;
        }
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var nextSequence = rule.CurrentSequence + step;
        var code = FormatBusinessCode(rule, nextSequence, now);
        rule.CurrentSequence = nextSequence;
        rule.ExampleCode = code;
        rule.UpdatedAt = now;
        await _numberingRepository.UpdateAsync(rule);
        return new TaktNumberingGenerateResultDto
        {
            BusinessCode = code,
            CurrentSequence = nextSequence,
            RuleCode = rule.RuleCode,
        };
    }

    /// <summary>
    /// 解析预览/生成所用的编号规则实体
    /// </summary>
    /// <param name="request">预览请求</param>
    /// <returns>编号规则实体</returns>
    private async Task<TaktNumbering> ResolveRuleForPreviewAsync(TaktNumberingPreviewRequestDto request)
    {
        if (request.NumberingId > 0)
        {
            var byId = await _numberingRepository.GetByIdAsync(request.NumberingId);
            if (byId == null
                || byId.TenantCode != CurrentTenantCode
                || byId.CompanyCode != CurrentCompanyCode)
            {
                throw new TaktBusinessException("编号规则不存在");
            }
            return byId;
        }
        if (!string.IsNullOrWhiteSpace(request.RuleCode))
        {
            return await LoadActiveRuleByCodeAsync(request.RuleCode.Trim());
        }
        if (string.IsNullOrWhiteSpace(request.DepartmentCode))
        {
            throw new TaktBusinessException("预览编号需提供规则 Id、规则编码或完整规则字段");
        }
        return new TaktNumbering
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            RuleCode = request.RuleCode ?? "PREVIEW",
            RuleName = request.RuleName ?? "PREVIEW",
            DocumentType = request.DocumentType,
            DepartmentCode = request.DepartmentCode,
            Prefix = request.Prefix,
            DateFormat = request.DateFormat,
            SequenceLength = request.SequenceLength <= 0 ? 6 : request.SequenceLength,
            SequenceStep = request.SequenceStep <= 0 ? 1 : request.SequenceStep,
            Suffix = request.Suffix,
            ResetPeriod = string.IsNullOrWhiteSpace(request.ResetPeriod) ? "none" : request.ResetPeriod,
            CurrentSequence = request.CurrentSequence,
            Separator = string.IsNullOrWhiteSpace(request.Separator) ? "-" : request.Separator,
            Status = TaktCommonStatus.Enabled,
        };
    }

    /// <summary>
    /// 按规则编码加载当前租户/公司下已启用的编号规则
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <returns>编号规则实体</returns>
    private async Task<TaktNumbering> LoadActiveRuleByCodeAsync(string ruleCode)
    {
        var list = await _numberingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.RuleCode == ruleCode);
        var rule = list.FirstOrDefault();
        if (rule == null)
        {
            throw new TaktBusinessException($"编号规则「{ruleCode}」不存在");
        }
        if (rule.Status != TaktCommonStatus.Enabled)
        {
            throw new TaktBusinessException($"编号规则「{ruleCode}」已禁用");
        }
        return rule;
    }

    /// <summary>
    /// 按编号规则反射拼接业务编号
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <param name="sequence">流水号（已含步长后的值）</param>
    /// <param name="referenceTime">参考时间；默认当前本地时间</param>
    /// <returns>业务编号</returns>
    private static string FormatBusinessCode(TaktNumbering rule, int sequence, DateTime? referenceTime = null)
    {
        if (sequence < 0)
        {
            throw new TaktBusinessException("流水号不能小于 0");
        }
        var time = referenceTime ?? DateTime.Now;
        var length = rule.SequenceLength <= 0 ? 6 : rule.SequenceLength;
        var separator = string.IsNullOrWhiteSpace(rule.Separator) ? "-" : rule.Separator.Trim();
        var parts = new List<string>();
        foreach (var segmentKey in ResolveSegmentKeys(rule))
        {
            var part = ResolveSegmentValue(rule, segmentKey, sequence, length, time);
            if (!string.IsNullOrWhiteSpace(part))
            {
                parts.Add(part);
            }
        }
        var code = string.Join(separator, parts);
        var suffix = rule.Suffix?.Trim();
        if (!string.IsNullOrWhiteSpace(suffix))
        {
            code += suffix.StartsWith(separator, StringComparison.Ordinal) ? suffix : suffix;
        }
        return code;
    }

    /// <summary>
    /// 计算预览用下一个流水号（不持久化）
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <param name="now">当前时间</param>
    /// <returns>预览流水号</returns>
    private static int ComputePreviewSequence(TaktNumbering rule, DateTime now)
    {
        if (ShouldResetSequence(rule.ResetPeriod, rule.UpdatedAt, now))
        {
            return rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        }
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        return rule.CurrentSequence <= 0 ? step : rule.CurrentSequence + step;
    }

    /// <summary>
    /// 是否应根据重置周期将流水号归零
    /// </summary>
    /// <param name="resetPeriod">重置周期（daily/monthly/yearly/none）</param>
    /// <param name="lastUpdatedAt">规则上次更新时间</param>
    /// <param name="now">当前时间</param>
    /// <returns>是否重置</returns>
    private static bool ShouldResetSequence(string resetPeriod, DateTime? lastUpdatedAt, DateTime now)
    {
        var period = resetPeriod?.Trim().ToLowerInvariant() ?? "none";
        if (period == "none" || !lastUpdatedAt.HasValue)
        {
            return false;
        }
        var last = lastUpdatedAt.Value;
        return period switch
        {
            "daily" => last.Date < now.Date,
            "monthly" => last.Year < now.Year || (last.Year == now.Year && last.Month < now.Month),
            "yearly" => last.Year < now.Year,
            _ => false,
        };
    }

    /// <summary>
    /// 解析编码段顺序
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <returns>段键列表（属性名或 Sequence）</returns>
    private static IReadOnlyList<string> ResolveSegmentKeys(TaktNumbering rule)
    {
        var fromDescription = TryParseSegmentKeysFromDescription(rule.Description);
        return fromDescription ?? DefaultSegmentKeys;
    }

    /// <summary>
    /// 反射解析单个编码段的文本值
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <param name="segmentKey">段键（实体属性名或 Sequence）</param>
    /// <param name="sequence">流水号</param>
    /// <param name="sequenceLength">流水号位数</param>
    /// <param name="time">参考时间</param>
    /// <returns>段文本；空段返回 null</returns>
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
        if (segmentKey.Equals(nameof(TaktNumbering.DateFormat), StringComparison.OrdinalIgnoreCase))
        {
            return FormatDateSegment(rule.DateFormat, time);
        }
        var property = ResolveSegmentProperty(segmentKey);
        if (property == null)
        {
            return null;
        }
        var raw = property.GetValue(rule);
        if (raw == null)
        {
            return null;
        }
        if (raw is TaktDocumentType documentType)
        {
            return documentType.ToString().ToLowerInvariant();
        }
        if (raw is string text)
        {
            var trimmed = text.Trim();
            if (segmentKey.Equals(nameof(TaktNumbering.Prefix), StringComparison.OrdinalIgnoreCase))
            {
                return string.IsNullOrWhiteSpace(trimmed) ? null : TrimTrailingSeparators(trimmed);
            }
            return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
        }
        if (raw is Enum enumValue)
        {
            return enumValue.ToString().ToLowerInvariant();
        }
        var formatted = Convert.ToString(raw, System.Globalization.CultureInfo.InvariantCulture)?.Trim();
        return string.IsNullOrWhiteSpace(formatted) ? null : formatted;
    }

    /// <summary>
    /// 按段名在 <see cref="TaktNumbering"/> 及其基类上查找属性
    /// </summary>
    /// <param name="segmentKey">段键（属性名，忽略大小写）</param>
    /// <returns>属性信息；未找到返回 null</returns>
    private static PropertyInfo? ResolveSegmentProperty(string segmentKey)
    {
        return SegmentPropertyCache.GetOrAdd(segmentKey, key =>
        {
            for (var type = typeof(TaktNumbering); type != null; type = type.BaseType)
            {
                var property = type.GetProperty(
                    key,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    return property;
                }
            }
            return null;
        });
    }

    /// <summary>
    /// 从 Description 解析 <c>segments:段1,段2,…</c> 配置
    /// </summary>
    /// <param name="description">描述字段</param>
    /// <returns>段键数组；未配置返回 null</returns>
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
    /// 按 DateFormat 字段生成日期段
    /// </summary>
    /// <param name="dateFormat">日期格式（为空则不输出日期段）</param>
    /// <param name="time">参考时间</param>
    /// <returns>日期段文本</returns>
    private static string FormatDateSegment(string? dateFormat, DateTime time)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
        {
            return string.Empty;
        }
        return dateFormat.Trim() switch
        {
            "yyyy" => time.ToString("yyyy"),
            "yyyyMM" => time.ToString("yyyyMM"),
            "yyyyMMdd" => time.ToString("yyyyMMdd"),
            "yyyyMMddHH" => time.ToString("yyyyMMddHH"),
            "yyyyMMddHHmm" => time.ToString("yyyyMMddHHmm"),
            _ => time.ToString(dateFormat.Trim()),
        };
    }

    /// <summary>
    /// 去掉前缀末尾的分隔符
    /// </summary>
    /// <param name="value">前缀原文</param>
    /// <returns>修剪后的前缀</returns>
    private static string TrimTrailingSeparators(string value)
    {
        return value.TrimEnd('-', '_', '/', ' ');
    }
}
