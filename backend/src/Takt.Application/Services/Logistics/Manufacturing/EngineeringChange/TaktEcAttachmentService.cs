// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件应用服务
/// </summary>
public class TaktEcAttachmentService : TaktServiceBase, ITaktEcAttachmentService
{
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;
    private readonly ITaktCompanyRepository<TaktEc> _ecRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecAttachmentRepository">设变附件仓储</param>
    /// <param name="ecRepository">设变主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcAttachmentService(
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktCompanyRepository<TaktEc> ecRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecAttachmentRepository = ecAttachmentRepository;
        _ecRepository = ecRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcAttachmentDto>> GetEcAttachmentListAsync(TaktEcAttachmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecAttachmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcAttachmentDto>.Create(
            data.Adapt<List<TaktEcAttachmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto?> GetEcAttachmentByIdAsync(long id)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcAttachmentDto>();
    }

    /// <summary>
    /// 获取设变附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcAttachmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecAttachmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FileName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto> CreateEcAttachmentAsync(TaktEcAttachmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcAttachment>();
                await StampEcAttachmentEcAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecAttachmentRepository,
            x => x.EcId == entity.EcId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique)
        {
            throw new TaktBusinessException("设变附件的EcId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.EcId,
                x => x.LineNumber);
            var businessCode = entity.EcId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecAttachmentRepository.CreateAsync(entity);
        return await GetEcAttachmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEcAttachmentDto>();
    }

    /// <summary>
    /// 更新设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto> UpdateEcAttachmentAsync(long id, TaktEcAttachmentUpdateDto dto)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变附件不存在");
        }
        dto.Adapt(entity);
                await StampEcAttachmentEcAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique = await _uniqueValidator.IsUniqueAsync(
            _ecAttachmentRepository,
            x => x.EcId == entity.EcId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique)
        {
            throw new TaktBusinessException("设变附件的EcId、LineNumber已存在");
        }
        await _ecAttachmentRepository.UpdateAsync(entity);
        return await GetEcAttachmentByIdAsync(id) ?? throw new TaktBusinessException("设变附件不存在");
    }

    /// <summary>
    /// 删除设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcAttachmentByIdAsync(long id)
    {
        var deleted = await _ecAttachmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变附件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcAttachmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcAttachmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcAttachmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcAttachmentTemplateDto>(
            sheetName ?? "设变附件导入模板",
            fileName ?? "设变附件导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcAttachmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcAttachmentImportDto>(fileStream, sheetName ?? "设变附件导入模板");
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
                var entity = rows[i].Adapt<TaktEcAttachment>();
                var importDto = rows[i].Adapt<TaktEcAttachmentCreateDto>();
                await StampEcAttachmentEcAsync(entity, importDto);
                var importKey = $"{entity.EcId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecAttachmentRepository,
                    x => x.EcId == entity.EcId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique)
                {
                    throw new TaktBusinessException("设变附件的EcId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.EcId,
                        x => x.LineNumber);
                    var businessCode = entity.EcId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecAttachmentRepository.CreateAsync(entity);
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
    /// 导出设变附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcAttachmentAsync(TaktEcAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcAttachmentQueryDto());
        var list = await _ecAttachmentRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcAttachmentExportDto>(),
                sheetName ?? "设变附件数据",
                fileName ?? "设变附件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcAttachmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变附件数据",
            fileName ?? "设变附件导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设变附件主表外键（ManyToOne → 设变主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcAttachmentEcAsync(TaktEcAttachment entity, TaktEcAttachmentCreateDto dto)
    {
        if (dto.EcId <= 0)
        {
            return;
        }
        var master = await _ecRepository.GetByIdAsync(dto.EcId);
        if (master == null)
        {
            throw new TaktBusinessException("设变主不存在");
        }
        entity.EcId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变附件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcAttachment, bool>> QueryExpression(TaktEcAttachmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcAttachment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcId).Contains(keywords)
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.AttachmentType != null && x.AttachmentType.Contains(keywords))
                || (x.DocNo != null && x.DocNo.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EcId.HasValue == true)
        {
            exp = exp.And(x => x.EcId == queryDto.EcId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.AttachmentType))
        {
            exp = exp.And(x => x.AttachmentType != null && x.AttachmentType.Contains(queryDto.AttachmentType));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocNo))
        {
            exp = exp.And(x => x.DocNo != null && x.DocNo.Contains(queryDto.DocNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileName))
        {
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(queryDto.FileName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccessUrl))
        {
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(queryDto.AccessUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
}
