// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktBankService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：银行信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 银行信息应用服务
/// </summary>
public class TaktBankService : TaktServiceBase, ITaktBankService
{
    private readonly ITaktTenantRepository<TaktBank> _bankRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bankRepository">银行信息仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBankService(
        ITaktTenantRepository<TaktBank> bankRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bankRepository = bankRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取银行信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBankDto>> GetBankListAsync(TaktBankQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _bankRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBankDto>.Create(
            data.Adapt<List<TaktBankDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto?> GetBankByIdAsync(long id)
    {
        var entity = await _bankRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktBankDto>();
    }

    /// <summary>
    /// 获取银行信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBankOptionsAsync()
    {
        var list = await _bankRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.BankCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BankCode,
            DictLabel = e.BankCode,
        }).ToList();
    }

    /// <summary>
    /// 创建银行信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto> CreateBankAsync(TaktBankCreateDto dto)
    {
        var entity = dto.Adapt<TaktBank>();
        var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
            _bankRepository,
            x => x.CountryRegion == entity.CountryRegion
                && x.BankCode == entity.BankCode);
        if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
        {
            throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
        }
        entity = await _bankRepository.CreateAsync(entity);
        return await GetBankByIdAsync(entity.Id) ?? entity.Adapt<TaktBankDto>();
    }

    /// <summary>
    /// 更新银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBankDto> UpdateBankAsync(long id, TaktBankUpdateDto dto)
    {
        var entity = await _bankRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("银行信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
            _bankRepository,
            x => x.CountryRegion == entity.CountryRegion
                && x.BankCode == entity.BankCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
        {
            throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
        }
        await _bankRepository.UpdateAsync(entity);
        return await GetBankByIdAsync(id) ?? throw new TaktBusinessException("银行信息不存在");
    }

    /// <summary>
    /// 删除银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBankByIdAsync(long id)
    {
        var deleted = await _bankRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("银行信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除银行信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBankBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBankByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBankTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBankTemplateDto>(
            sheetName ?? "银行信息导入模板",
            fileName ?? "银行信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入银行信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBankAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBankImportDto>(fileStream, sheetName ?? "银行信息导入模板");
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
                var entity = rows[i].Adapt<TaktBank>();
                var importKey = $"{entity.CountryRegion}|{entity.BankCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountryRegion、BankCode）");
                }
                var isUnique_ix_takt_accounting_financial_bank_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _bankRepository,
                    x => x.CountryRegion == entity.CountryRegion
                        && x.BankCode == entity.BankCode);
                if (!isUnique_ix_takt_accounting_financial_bank_code_unique)
                {
                    throw new TaktBusinessException("银行信息的CountryRegion、BankCode已存在");
                }
                await _bankRepository.CreateAsync(entity);
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
    /// 导出银行信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBankAsync(TaktBankQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBankQueryDto());
        var list = await _bankRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBankExportDto>(),
                sheetName ?? "银行信息数据",
                fileName ?? "银行信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBankExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "银行信息数据",
            fileName ?? "银行信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建银行信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBank, bool>> QueryExpression(TaktBankQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBank>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CountryRegion != null && x.CountryRegion.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankName1 != null && x.BankName1.Contains(keywords))
                || (x.BankName2 != null && x.BankName2.Contains(keywords))
                || (x.Province != null && x.Province.Contains(keywords))
                || (x.Prefecture != null && x.Prefecture.Contains(keywords))
                || (x.District != null && x.District.Contains(keywords))
                || (x.Township != null && x.Township.Contains(keywords))
                || (x.Village != null && x.Village.Contains(keywords))
                || (x.Address1 != null && x.Address1.Contains(keywords))
                || (x.Address2 != null && x.Address2.Contains(keywords))
                || (x.SwiftBic != null && x.SwiftBic.Contains(keywords))
                || (x.BankGroup != null && x.BankGroup.Contains(keywords))
                || SqlFunc.ToString(x.PobkCurAc).Contains(keywords)
                || (x.BankNumber != null && x.BankNumber.Contains(keywords))
                || (x.PostalBank != null && x.PostalBank.Contains(keywords))
                || (x.AddressNumber != null && x.AddressNumber.Contains(keywords))
                || (x.Branch != null && x.Branch.Contains(keywords))
                || (x.BankMethod != null && x.BankMethod.Contains(keywords))
                || (x.BankFormat != null && x.BankFormat.Contains(keywords))
                || (x.IbanRule != null && x.IbanRule.Contains(keywords))
                || SqlFunc.ToString(x.SddB2b).Contains(keywords)
                || SqlFunc.ToString(x.SddCore).Contains(keywords)
                || SqlFunc.ToString(x.SddRtrans).Contains(keywords)
                || (x.BicPlusNumber != null && x.BicPlusNumber.Contains(keywords))
                || (x.PathCode != null && x.PathCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CountryRegion))
        {
            exp = exp.And(x => x.CountryRegion != null && x.CountryRegion.Contains(queryDto.CountryRegion));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankCode))
        {
            exp = exp.And(x => x.BankCode != null && x.BankCode.Contains(queryDto.BankCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankName1))
        {
            exp = exp.And(x => x.BankName1 != null && x.BankName1.Contains(queryDto.BankName1));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankName2))
        {
            exp = exp.And(x => x.BankName2 != null && x.BankName2.Contains(queryDto.BankName2));
        }

        if (!string.IsNullOrEmpty(queryDto?.Province))
        {
            exp = exp.And(x => x.Province != null && x.Province.Contains(queryDto.Province));
        }

        if (!string.IsNullOrEmpty(queryDto?.Prefecture))
        {
            exp = exp.And(x => x.Prefecture != null && x.Prefecture.Contains(queryDto.Prefecture));
        }

        if (!string.IsNullOrEmpty(queryDto?.District))
        {
            exp = exp.And(x => x.District != null && x.District.Contains(queryDto.District));
        }

        if (!string.IsNullOrEmpty(queryDto?.Township))
        {
            exp = exp.And(x => x.Township != null && x.Township.Contains(queryDto.Township));
        }

        if (!string.IsNullOrEmpty(queryDto?.Village))
        {
            exp = exp.And(x => x.Village != null && x.Village.Contains(queryDto.Village));
        }

        if (!string.IsNullOrEmpty(queryDto?.Address1))
        {
            exp = exp.And(x => x.Address1 != null && x.Address1.Contains(queryDto.Address1));
        }

        if (!string.IsNullOrEmpty(queryDto?.Address2))
        {
            exp = exp.And(x => x.Address2 != null && x.Address2.Contains(queryDto.Address2));
        }

        if (!string.IsNullOrEmpty(queryDto?.SwiftBic))
        {
            exp = exp.And(x => x.SwiftBic != null && x.SwiftBic.Contains(queryDto.SwiftBic));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankGroup))
        {
            exp = exp.And(x => x.BankGroup != null && x.BankGroup.Contains(queryDto.BankGroup));
        }

        if (queryDto?.PobkCurAc.HasValue == true)
        {
            exp = exp.And(x => x.PobkCurAc == queryDto.PobkCurAc);
        }

        if (!string.IsNullOrEmpty(queryDto?.BankNumber))
        {
            exp = exp.And(x => x.BankNumber != null && x.BankNumber.Contains(queryDto.BankNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.PostalBank))
        {
            exp = exp.And(x => x.PostalBank != null && x.PostalBank.Contains(queryDto.PostalBank));
        }

        if (!string.IsNullOrEmpty(queryDto?.AddressNumber))
        {
            exp = exp.And(x => x.AddressNumber != null && x.AddressNumber.Contains(queryDto.AddressNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.Branch))
        {
            exp = exp.And(x => x.Branch != null && x.Branch.Contains(queryDto.Branch));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankMethod))
        {
            exp = exp.And(x => x.BankMethod != null && x.BankMethod.Contains(queryDto.BankMethod));
        }

        if (!string.IsNullOrEmpty(queryDto?.BankFormat))
        {
            exp = exp.And(x => x.BankFormat != null && x.BankFormat.Contains(queryDto.BankFormat));
        }

        if (!string.IsNullOrEmpty(queryDto?.IbanRule))
        {
            exp = exp.And(x => x.IbanRule != null && x.IbanRule.Contains(queryDto.IbanRule));
        }

        if (queryDto?.SddB2b.HasValue == true)
        {
            exp = exp.And(x => x.SddB2b == queryDto.SddB2b);
        }

        if (queryDto?.SddCore.HasValue == true)
        {
            exp = exp.And(x => x.SddCore == queryDto.SddCore);
        }

        if (queryDto?.SddRtrans.HasValue == true)
        {
            exp = exp.And(x => x.SddRtrans == queryDto.SddRtrans);
        }

        if (!string.IsNullOrEmpty(queryDto?.BicPlusNumber))
        {
            exp = exp.And(x => x.BicPlusNumber != null && x.BicPlusNumber.Contains(queryDto.BicPlusNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.PathCode))
        {
            exp = exp.And(x => x.PathCode != null && x.PathCode.Contains(queryDto.PathCode));
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
}
