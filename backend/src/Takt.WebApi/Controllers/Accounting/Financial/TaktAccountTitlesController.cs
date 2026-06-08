// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktAccountTitlesController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 会计科目控制器
/// 提供会计科目的 REST API
/// </summary>
[ApiModule(TaktModule.Accounting, "财务核算")]
[Route("api/[controller]", Name = "会计科目")]
public class TaktAccountTitlesController : TaktControllerBase
{
    private readonly ITaktAccountTitleService _accountTitleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="accountTitleService">会计科目服务</param>
    public TaktAccountTitlesController(ITaktAccountTitleService accountTitleService)
    {
        _accountTitleService = accountTitleService;
    }

    /// <summary>
    /// 获取会计科目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:accounttitle:list", "会计科目列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAccountTitleListAsync([FromQuery] TaktAccountTitleQueryDto queryDto)
    {
        try
        {
            var result = await _accountTitleService.GetAccountTitleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>会计科目DTO</returns>
    [TaktPermission("accounting:financial:accounttitle:query", "会计科目详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountTitleByIdAsync(long id)
    {
        try
        {
            var result = await _accountTitleService.GetAccountTitleByIdAsync(id);
            if (result == null)
            {
                return NotFound("会计科目不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会计科目树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    [TaktPermission("accounting:financial:accounttitle:query", "会计科目树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetAccountTitleTreeOptionsAsync()
    {
        try
        {
            var result = await _accountTitleService.GetAccountTitleTreeOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会计科目树形列表
    /// </summary>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 枚举字段，如 TaktCommonStatus.Enabled）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("accounting:financial:accounttitle:query", "会计科目树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetAccountTitleTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _accountTitleService.GetAccountTitleTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会计科目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会计科目DTO</returns>
    [TaktPermission("accounting:financial:accounttitle:create", "创建会计科目")]
    [HttpPost]
    public async Task<IActionResult> CreateAccountTitleAsync([FromBody] TaktAccountTitleCreateDto dto)
    {
        try
        {
            var result = await _accountTitleService.CreateAccountTitleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会计科目DTO</returns>
    [TaktPermission("accounting:financial:accounttitle:update", "更新会计科目")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccountTitleAsync(long id, [FromBody] TaktAccountTitleUpdateDto dto)
    {
        try
        {
            var result = await _accountTitleService.UpdateAccountTitleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会计科目
    /// </summary>
    /// <param name="id">会计科目ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:accounttitle:delete", "删除会计科目")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountTitleByIdAsync(long id)
    {
        try
        {
            await _accountTitleService.DeleteAccountTitleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会计科目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:accounttitle:delete", "批量删除会计科目")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAccountTitleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _accountTitleService.DeleteAccountTitleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会计科目状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>会计科目DTO</returns>
    [TaktPermission("accounting:financial:accounttitle:update", "更新会计科目状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAccountTitleStatusAsync([FromBody] TaktAccountTitleStatusDto dto)
    {
        try
        {
            var result = await _accountTitleService.UpdateAccountTitleStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会计科目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>会计科目DTO</returns>
    [TaktPermission("accounting:financial:accounttitle:update", "更新会计科目排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateAccountTitleSortAsync([FromBody] TaktAccountTitleSortDto dto)
    {
        try
        {
            var result = await _accountTitleService.UpdateAccountTitleSortAsync(dto);
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
    [TaktPermission("accounting:financial:accounttitle:import", "获取会计科目导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAccountTitleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _accountTitleService.GetAccountTitleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会计科目
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:accounttitle:import", "导入会计科目")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAccountTitleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _accountTitleService.ImportAccountTitleAsync(stream, sheetName);
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
    /// 导出会计科目
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:accounttitle:export", "导出会计科目")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAccountTitleAsync([FromQuery] TaktAccountTitleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _accountTitleService.ExportAccountTitleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
