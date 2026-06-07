// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.CompensationBenefits
// 文件名称：TaktTaxCalcsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：个税计算规则控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.CompensationBenefits;
using Takt.Application.Services.HumanResource.CompensationBenefits;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.CompensationBenefits;

/// <summary>
/// 个税计算规则控制器
/// 提供个税计算规则的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "个税计算规则")]
public class TaktTaxCalcsController : TaktControllerBase
{
    private readonly ITaktTaxCalcService _taxCalcService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxCalcService">个税计算规则服务</param>
    public TaktTaxCalcsController(ITaktTaxCalcService taxCalcService)
    {
        _taxCalcService = taxCalcService;
    }

    /// <summary>
    /// 获取个税计算规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:list", "个税计算规则列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTaxCalcListAsync([FromQuery] TaktTaxCalcQueryDto queryDto)
    {
        try
        {
            var result = await _taxCalcService.GetTaxCalcListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>个税计算规则DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:query", "个税计算规则详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaxCalcByIdAsync(long id)
    {
        try
        {
            var result = await _taxCalcService.GetTaxCalcByIdAsync(id);
            if (result == null)
            {
                return NotFound("个税计算规则不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取个税计算规则选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:query", "个税计算规则选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTaxCalcOptionsAsync()
    {
        try
        {
            var result = await _taxCalcService.GetTaxCalcOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建个税计算规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>个税计算规则DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:create", "创建个税计算规则")]
    [HttpPost]
    public async Task<IActionResult> CreateTaxCalcAsync([FromBody] TaktTaxCalcCreateDto dto)
    {
        try
        {
            var result = await _taxCalcService.CreateTaxCalcAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>个税计算规则DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:update", "更新个税计算规则")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaxCalcAsync(long id, [FromBody] TaktTaxCalcUpdateDto dto)
    {
        try
        {
            var result = await _taxCalcService.UpdateTaxCalcAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:delete", "删除个税计算规则")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaxCalcByIdAsync(long id)
    {
        try
        {
            await _taxCalcService.DeleteTaxCalcByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除个税计算规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:delete", "批量删除个税计算规则")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTaxCalcBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _taxCalcService.DeleteTaxCalcBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新个税计算规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>个税计算规则DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:update", "更新个税计算规则状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTaxCalcStatusAsync([FromBody] TaktTaxCalcStatusDto dto)
    {
        try
        {
            var result = await _taxCalcService.UpdateTaxCalcStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:import", "获取个税计算规则导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTaxCalcTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _taxCalcService.GetTaxCalcTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入个税计算规则
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:import", "导入个税计算规则")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTaxCalcAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _taxCalcService.ImportTaxCalcAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出个税计算规则
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:compensationbenefits:taxcalc:export", "导出个税计算规则")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTaxCalcAsync([FromQuery] TaktTaxCalcQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _taxCalcService.ExportTaxCalcAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
