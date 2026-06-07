// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.CompensationBenefits
// 文件名称：TaktSocialSecuritiesController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：社保缴纳控制器
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
/// 社保缴纳控制器
/// 提供社保缴纳的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "社保缴纳")]
public class TaktSocialSecuritiesController : TaktControllerBase
{
    private readonly ITaktSocialSecurityService _socialSecurityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="socialSecurityService">社保缴纳服务</param>
    public TaktSocialSecuritiesController(ITaktSocialSecurityService socialSecurityService)
    {
        _socialSecurityService = socialSecurityService;
    }

    /// <summary>
    /// 获取社保缴纳列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:list", "社保缴纳列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSocialSecurityListAsync([FromQuery] TaktSocialSecurityQueryDto queryDto)
    {
        try
        {
            var result = await _socialSecurityService.GetSocialSecurityListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <returns>社保缴纳DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:query", "社保缴纳详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSocialSecurityByIdAsync(long id)
    {
        try
        {
            var result = await _socialSecurityService.GetSocialSecurityByIdAsync(id);
            if (result == null)
            {
                return NotFound("社保缴纳不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取社保缴纳选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:query", "社保缴纳选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSocialSecurityOptionsAsync()
    {
        try
        {
            var result = await _socialSecurityService.GetSocialSecurityOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建社保缴纳
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>社保缴纳DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:create", "创建社保缴纳")]
    [HttpPost]
    public async Task<IActionResult> CreateSocialSecurityAsync([FromBody] TaktSocialSecurityCreateDto dto)
    {
        try
        {
            var result = await _socialSecurityService.CreateSocialSecurityAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>社保缴纳DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:update", "更新社保缴纳")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSocialSecurityAsync(long id, [FromBody] TaktSocialSecurityUpdateDto dto)
    {
        try
        {
            var result = await _socialSecurityService.UpdateSocialSecurityAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除社保缴纳
    /// </summary>
    /// <param name="id">社保缴纳ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:delete", "删除社保缴纳")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSocialSecurityByIdAsync(long id)
    {
        try
        {
            await _socialSecurityService.DeleteSocialSecurityByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除社保缴纳
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:delete", "批量删除社保缴纳")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSocialSecurityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _socialSecurityService.DeleteSocialSecurityBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新社保缴纳状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>社保缴纳DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:update", "更新社保缴纳状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSocialSecurityStatusAsync([FromBody] TaktSocialSecurityStatusDto dto)
    {
        try
        {
            var result = await _socialSecurityService.UpdateSocialSecurityStatusAsync(dto);
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
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:import", "获取社保缴纳导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSocialSecurityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _socialSecurityService.GetSocialSecurityTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入社保缴纳
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:import", "导入社保缴纳")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSocialSecurityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _socialSecurityService.ImportSocialSecurityAsync(stream, sheetName);
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
    /// 导出社保缴纳
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:compensationbenefits:socialsecurity:export", "导出社保缴纳")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSocialSecurityAsync([FromQuery] TaktSocialSecurityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _socialSecurityService.ExportSocialSecurityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
