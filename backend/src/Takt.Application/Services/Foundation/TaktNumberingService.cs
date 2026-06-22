// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktNumberingService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：编号规则应用服务实现（CRUD + 预览/生成业务编号）
// · 新增/导入/更新：服务端按规则自动生成或刷新起始编码 ExampleCode（Create/Import 同时写入 CurrentSequence）
// · 运行时：GenerateNumberingAsync 递增流水并产出下一业务编号（服务核心）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 编号规则应用服务
/// </summary>
public class TaktNumberingService : TaktServiceBase, ITaktNumberingService
{
    private readonly ITaktCompanyRepository<TaktNumbering> _numberingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="numberingRepository">编号规则仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNumberingService(
        ITaktCompanyRepository<TaktNumbering> numberingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _numberingRepository = numberingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取编号规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNumberingDto>> GetNumberingListAsync(TaktNumberingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _numberingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNumberingDto>.Create(
            data.Adapt<List<TaktNumberingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto?> GetNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 获取编号规则选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNumberingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _numberingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.RuleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.RuleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建编号规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> CreateNumberingAsync(TaktNumberingCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktNumbering>();
        entity.TenantCode = CurrentTenantCode;
        entity.CompanyCode = CurrentCompanyCode;
        entity.IsBuiltIn = 0;
        NormalizeNumberingRule(entity);
        var (initialCode, initialSequence) = BuildInitialExampleCode(entity, DateTime.Now);
        entity.ExampleCode = initialCode;
        entity.CurrentSequence = initialSequence;
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编号规则的RuleCode已存在");
        }
        entity = await _numberingRepository.CreateAsync(entity);
        return await GetNumberingByIdAsync(entity.Id) ?? entity.Adapt<TaktNumberingDto>();
    }

    /// <summary>
    /// 更新编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingAsync(long id, TaktNumberingUpdateDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        var originalCurrentSequence = entity.CurrentSequence;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        entity.CurrentSequence = originalCurrentSequence;
        NormalizeNumberingRule(entity);
        var sequenceForExample = entity.CurrentSequence <= 0
            ? (entity.SequenceStep <= 0 ? 1 : entity.SequenceStep)
            : entity.CurrentSequence;
        entity.ExampleCode = FormatBusinessCode(entity, sequenceForExample, DateTime.Now);
        var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
            _numberingRepository,
            x => x.RuleCode == entity.RuleCode,
            id);
        if (!isUnique_ix_numbering_code_unique)
        {
            throw new TaktBusinessException("编号规则的RuleCode已存在");
        }
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(id) ?? throw new TaktBusinessException("编号规则不存在");
    }

    /// <summary>
    /// 删除编号规则
    /// </summary>
    /// <param name="id">编号规则ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingByIdAsync(long id)
    {
        var entity = await _numberingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置编号规则不允许删除");
        }
        var deleted = await _numberingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("编号规则不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除编号规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNumberingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _numberingRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置编号规则不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteNumberingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新编号规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNumberingDto> UpdateNumberingStatusAsync(TaktNumberingStatusDto dto)
    {
        var entity = await _numberingRepository.GetByIdAsync(dto.NumberingId);
        if (entity == null)
        {
            throw new TaktBusinessException("编号规则不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.Status != 1)
        {
            throw new TaktBusinessException("不允许禁用内置编号规则");
        }
        entity.Status = dto.Status;
        await _numberingRepository.UpdateAsync(entity);
        return await GetNumberingByIdAsync(dto.NumberingId) ?? throw new TaktBusinessException("编号规则不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNumberingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNumberingTemplateDto>(
            sheetName ?? "编号规则导入模板",
            fileName ?? "编号规则导入模板.xlsx");
    }

    /// <summary>
    /// 导入编号规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNumberingAsync(Stream fileStream, string? sheetName = null)
    {
        EnsureThreeLayerContext();
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNumberingImportDto>(fileStream, sheetName ?? "编号规则导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNumbering>();
                entity.IsBuiltIn = 0;
                entity.TenantCode = CurrentTenantCode;
                entity.CompanyCode = CurrentCompanyCode;
                NormalizeNumberingRule(entity);
                ApplyExampleCodeOnCreate(entity, rows[i].ExampleCode, autoGenerateWhenEmpty: true);
                var importKey = $"{entity.RuleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RuleCode）");
                }
                var isUnique_ix_numbering_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _numberingRepository,
                    x => x.RuleCode == entity.RuleCode);
                if (!isUnique_ix_numbering_code_unique)
                {
                    throw new TaktBusinessException("编号规则的RuleCode已存在");
                }
                await _numberingRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出编号规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNumberingAsync(TaktNumberingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNumberingQueryDto());
        var list = await _numberingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNumberingExportDto>(),
                sheetName ?? "编号规则数据",
                fileName ?? "编号规则导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNumberingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "编号规则数据",
            fileName ?? "编号规则导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建编号规则查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNumbering, bool>> QueryExpression(TaktNumberingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNumbering>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.RuleCode != null && x.RuleCode.Contains(keywords))
                || (x.RuleName != null && x.RuleName.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.DepartmentCode != null && x.DepartmentCode.Contains(keywords))
                || (x.PrefixCode != null && x.PrefixCode.Contains(keywords))
                || (x.DateFormat != null && x.DateFormat.Contains(keywords))
                || SqlFunc.ToString(x.SequenceLength).Contains(keywords)
                || SqlFunc.ToString(x.SequenceStep).Contains(keywords)
                || (x.SuffixCode != null && x.SuffixCode.Contains(keywords))
                || (x.ResetPeriod != null && x.ResetPeriod.Contains(keywords))
                || SqlFunc.ToString(x.CurrentSequence).Contains(keywords)
                || x.ExampleCode.Contains(keywords)
                || (x.Separator != null && x.Separator.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.Status).Contains(keywords)
                || (x.Description != null && x.Description.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleCode))
        {
            exp = exp.And(x => x.RuleCode != null && x.RuleCode.Contains(queryDto.RuleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RuleName))
        {
            exp = exp.And(x => x.RuleName != null && x.RuleName.Contains(queryDto.RuleName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentType))
        {
            exp = exp.And(x => x.DocumentType == queryDto.DocumentType.Trim());
        }

        if (!string.IsNullOrEmpty(queryDto?.DepartmentCode))
        {
            exp = exp.And(x => x.DepartmentCode != null && x.DepartmentCode.Contains(queryDto.DepartmentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PrefixCode))
        {
            exp = exp.And(x => x.PrefixCode != null && x.PrefixCode.Contains(queryDto.PrefixCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DateFormat))
        {
            exp = exp.And(x => x.DateFormat != null && x.DateFormat.Contains(queryDto.DateFormat));
        }

        if (queryDto?.SequenceLength.HasValue == true)
        {
            exp = exp.And(x => x.SequenceLength == queryDto.SequenceLength);
        }

        if (queryDto?.SequenceStep.HasValue == true)
        {
            exp = exp.And(x => x.SequenceStep == queryDto.SequenceStep);
        }

        if (!string.IsNullOrEmpty(queryDto?.SuffixCode))
        {
            exp = exp.And(x => x.SuffixCode != null && x.SuffixCode.Contains(queryDto.SuffixCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ResetPeriod))
        {
            exp = exp.And(x => x.ResetPeriod != null && x.ResetPeriod.Contains(queryDto.ResetPeriod));
        }

        if (queryDto?.CurrentSequence.HasValue == true)
        {
            exp = exp.And(x => x.CurrentSequence == queryDto.CurrentSequence);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExampleCode))
        {
            exp = exp.And(x => x.ExampleCode.Contains(queryDto.ExampleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Separator))
        {
            exp = exp.And(x => x.Separator != null && x.Separator.Contains(queryDto.Separator));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.Status.HasValue == true)
        {
            exp = exp.And(x => x.Status == queryDto.Status);
        }

        if (!string.IsNullOrEmpty(queryDto?.Description))
        {
            exp = exp.And(x => x.Description != null && x.Description.Contains(queryDto.Description));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }

    // ========================================
    // 预览/生成业务编号
    // ========================================

    /// <summary>
    /// 默认编码段顺序：公司、部门、前缀、日期+流水（DateSequence）
    /// </summary>
    private static readonly string[] DefaultSegmentKeys =
    {
        nameof(TaktCompanyEntityBase.CompanyCode),
        nameof(TaktNumbering.DepartmentCode),
        nameof(TaktNumbering.PrefixCode),
        "DateSequence",
    };

    /// <summary>
    /// 编号段属性反射缓存（段名 → PropertyInfo）
    /// </summary>
    private static readonly ConcurrentDictionary<string, PropertyInfo?> SegmentPropertyCache =
        new(StringComparer.OrdinalIgnoreCase);

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
    /// 生成下一个业务编号（服务核心：递增 CurrentSequence、写回 ExampleCode，供业务单据调用）
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
        var (code, nextSequence) = BuildNextBusinessCode(rule, now);
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
        if (!string.IsNullOrWhiteSpace(request.DepartmentCode))
        {
            var draft = new TaktNumbering
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                RuleCode = request.RuleCode ?? "PREVIEW",
                RuleName = request.RuleName ?? "PREVIEW",
                DocumentType = TaktNumberingHelper.NormalizeDocumentType(request.DocumentType),
                DepartmentCode = request.DepartmentCode,
                PrefixCode = request.PrefixCode,
                DateFormat = request.DateFormat,
                SequenceLength = request.SequenceLength <= 0 ? 6 : request.SequenceLength,
                SequenceStep = request.SequenceStep <= 0 ? 1 : request.SequenceStep,
                SuffixCode = request.SuffixCode,
                ResetPeriod = string.IsNullOrWhiteSpace(request.ResetPeriod) ? "none" : request.ResetPeriod,
                CurrentSequence = request.CurrentSequence,
                Separator = request.Separator,
                Status = 1,
            };
            NormalizeNumberingRule(draft);
            return draft;
        }
        if (!string.IsNullOrWhiteSpace(request.RuleCode))
        {
            return await LoadActiveRuleByCodeAsync(request.RuleCode.Trim());
        }
        throw new TaktBusinessException("预览编号需提供规则 Id、规则编码或完整规则字段");
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
        if (rule.Status != 1)
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
        var code = string.IsNullOrEmpty(separator)
            ? string.Concat(parts)
            : string.Join(separator, parts);
        var suffixCode = rule.SuffixCode?.Trim();
        if (!string.IsNullOrWhiteSpace(suffixCode))
        {
            code += suffixCode.StartsWith(separator, StringComparison.Ordinal) ? suffixCode : suffixCode;
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
    /// 默认编码段：公司、部门、前缀、日期+流水（DateSequence）
    /// </summary>
    private const string DefaultSegmentsDescription =
        "segments:CompanyCode,DepartmentCode,PrefixCode,DateSequence";

    private const string SegmentsCompanyLevelDescription =
        "segments:CompanyCode,PrefixCode,DateSequence";
    private const string SegmentsCompanyNoDateDescription =
        "segments:CompanyCode,PrefixCode,Sequence";
    private const string SegmentsWithDepartmentNoDateDescription =
        "segments:CompanyCode,DepartmentCode,PrefixCode,Sequence";

    /// <summary>
    /// 规范化编号规则默认值（流水位数、步长、重置周期、日期格式 none、段配置、分隔符）
    /// </summary>
    /// <param name="rule">编号规则</param>
    private static void NormalizeNumberingRule(TaktNumbering rule)
    {
        if (rule.SequenceLength <= 0)
        {
            rule.SequenceLength = 6;
        }
        if (rule.SequenceStep <= 0)
        {
            rule.SequenceStep = 1;
        }
        if (string.IsNullOrWhiteSpace(rule.ResetPeriod))
        {
            rule.ResetPeriod = "none";
        }
        if (string.Equals(rule.DateFormat?.Trim(), "none", StringComparison.OrdinalIgnoreCase))
        {
            rule.DateFormat = null;
        }
        else
        {
            rule.DateFormat = TaktNumberingHelper.NormalizeSupportedDateFormat(rule.DateFormat);
        }
        if (TaktNumberingHelper.TryResolveRequiredResetPeriod(rule.DateFormat, out var requiredResetPeriod))
        {
            rule.ResetPeriod = requiredResetPeriod;
        }
        else
        {
            rule.ResetPeriod = TaktNumberingHelper.NormalizeResetPeriod(rule.ResetPeriod);
        }
        MigrateLegacySegmentDescription(rule);
        rule.DocumentType = TaktNumberingHelper.NormalizeDocumentType(rule.DocumentType);
        if (string.IsNullOrWhiteSpace(rule.Separator))
        {
            rule.Separator = "-";
        }
    }

    /// <summary>
    /// 将旧版 segments（含 DocumentType 或 DateFormat+Sequence 分列）迁移为新段配置
    /// </summary>
    /// <param name="description">原始 Description</param>
    /// <returns>有效段配置</returns>
    private static string ResolveEffectiveSegmentDescription(string? description)
    {
        var text = description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text)
            || !text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return DefaultSegmentsDescription;
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
            return hasDate ? DefaultSegmentsDescription : SegmentsWithDepartmentNoDateDescription;
        }
        return hasDate ? SegmentsCompanyLevelDescription : SegmentsCompanyNoDateDescription;
    }

    /// <summary>
    /// 将旧版 segments 写入实体 Description
    /// </summary>
    /// <param name="rule">编号规则</param>
    private static void MigrateLegacySegmentDescription(TaktNumbering rule)
    {
        rule.Description = TaktNumberingHelper.NormalizeSegmentDescription(
            ResolveEffectiveSegmentDescription(rule.Description));
    }

    /// <summary>
    /// 创建/导入时写入起始编码 ExampleCode 与 CurrentSequence（更新规则时不走此方法）
    /// </summary>
    /// <param name="entity">待持久化的编号规则</param>
    /// <param name="exampleCodeInput">起始编码</param>
    /// <param name="autoGenerateWhenEmpty">为空时是否按规则自动生成初始编码（仅导入场景）</param>
    private static void ApplyExampleCodeOnCreate(
        TaktNumbering entity,
        string exampleCodeInput,
        bool autoGenerateWhenEmpty)
    {
        var now = DateTime.Now;
        if (string.IsNullOrWhiteSpace(exampleCodeInput))
        {
            if (!autoGenerateWhenEmpty)
            {
                throw new TaktBusinessException("起始编码不能为空");
            }
            var (code, sequence) = BuildInitialExampleCode(entity, now);
            entity.ExampleCode = code;
            entity.CurrentSequence = sequence;
            return;
        }
        entity.ExampleCode = exampleCodeInput.Trim();
        if (!TryParseCurrentSequenceFromExampleCode(
                entity.ExampleCode,
                entity.SequenceLength,
                entity.Separator,
                out var currentSequence))
        {
            throw new TaktBusinessException("起始编码末段须为有效流水号");
        }
        entity.CurrentSequence = currentSequence;
    }

    /// <summary>
    /// 按规则生成初始业务编号（首个流水号 = 步长，与预览逻辑一致）
    /// </summary>
    /// <param name="rule">编号规则</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>起始编码与当前流水号</returns>
    private static (string ExampleCode, int CurrentSequence) BuildInitialExampleCode(
        TaktNumbering rule,
        DateTime referenceTime)
    {
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var initialSequence = step;
        var code = FormatBusinessCode(rule, initialSequence, referenceTime);
        return (code, initialSequence);
    }

    /// <summary>
    /// 按规则生成下一个业务编号（含重置周期判断）
    /// </summary>
    /// <param name="rule">编号规则（会临时修改 CurrentSequence 以应用重置）</param>
    /// <param name="now">当前时间</param>
    /// <returns>业务编号与下一流水号</returns>
    private static (string BusinessCode, int NextSequence) BuildNextBusinessCode(TaktNumbering rule, DateTime now)
    {
        if (ShouldResetSequence(rule.ResetPeriod, rule.UpdatedAt, now))
        {
            rule.CurrentSequence = 0;
        }
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var nextSequence = checked(rule.CurrentSequence + step);
        var code = FormatBusinessCode(rule, nextSequence, now);
        return (code, nextSequence);
    }

    /// <summary>
    /// 从起始编码末段解析当前流水号（末段须为纯数字，如 SO-20250120-000001 → 1）
    /// </summary>
    /// <param name="exampleCode">起始编码</param>
    /// <param name="sequenceLength">流水位数（未传有效值时不校验位数）</param>
    /// <param name="separator">规则分隔符（单字符时优先按此分段）</param>
    /// <param name="currentSequence">解析出的流水号</param>
    /// <returns>是否解析成功</returns>
    private static bool TryParseCurrentSequenceFromExampleCode(
        string exampleCode,
        int sequenceLength,
        string? separator,
        out int currentSequence)
    {
        currentSequence = 0;
        if (string.IsNullOrWhiteSpace(exampleCode))
        {
            return false;
        }
        var trimmed = exampleCode.Trim();
        var lastSegment = trimmed;
        var sep = separator?.Trim();
        if (!string.IsNullOrEmpty(sep) && sep.Length == 1)
        {
            var idx = trimmed.LastIndexOf(sep[0]);
            if (idx >= 0)
            {
                lastSegment = trimmed[(idx + 1)..];
            }
        }
        else
        {
            var lastSeparator = Math.Max(trimmed.LastIndexOf('-'), trimmed.LastIndexOf('_'));
            lastSegment = lastSeparator >= 0 ? trimmed[(lastSeparator + 1)..] : trimmed;
        }
        if (string.IsNullOrWhiteSpace(lastSegment) || lastSegment.Length > 18)
        {
            return false;
        }
        for (var i = 0; i < lastSegment.Length; i++)
        {
            if (!char.IsDigit(lastSegment[i]))
            {
                return false;
            }
        }
        if (!int.TryParse(lastSegment, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
            || parsed < 0)
        {
            return false;
        }
        if (sequenceLength > 0 && lastSegment.Length > sequenceLength)
        {
            return false;
        }
        currentSequence = parsed;
        return true;
    }

    /// <summary>
    /// 是否应根据重置周期将流水号归零
    /// </summary>
    /// <param name="resetPeriod">重置周期（none/day/month/year/hour；兼容 daily/monthly/yearly/hourly）</param>
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
            "day" or "daily" => last.Date < now.Date,
            "month" or "monthly" => last.Year < now.Year || (last.Year == now.Year && last.Month < now.Month),
            "year" or "yearly" => last.Year < now.Year,
            "hour" or "hourly" => last.Year != now.Year
                || last.Month != now.Month
                || last.Day != now.Day
                || last.Hour != now.Hour,
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
        var fromDescription = TryParseSegmentKeysFromDescription(ResolveEffectiveSegmentDescription(rule.Description));
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
        if (raw is int intValue)
        {
            return intValue.ToString();
        }
        if (raw is string text)
        {
            var trimmed = text.Trim();
            if (segmentKey.Equals(nameof(TaktNumbering.PrefixCode), StringComparison.OrdinalIgnoreCase)
                || segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
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
    /// 按段名在 TaktNumbering 及其基类上查找属性
    /// </summary>
    /// <param name="segmentKey">段键（属性名，忽略大小写）</param>
    /// <returns>属性信息；未找到返回 null</returns>
    private static PropertyInfo? ResolveSegmentProperty(string segmentKey)
    {
        var normalizedKey = TaktNumberingHelper.NormalizeSegmentKey(segmentKey);
        return SegmentPropertyCache.GetOrAdd(normalizedKey, key =>
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
    /// 去掉前缀末尾的分隔符
    /// </summary>
    /// <param name="value">前缀原文</param>
    /// <returns>修剪后的前缀</returns>
    private static string TrimTrailingSeparators(string value)
    {
        return value.TrimEnd('-', '_', '/', ' ');
    }
}
