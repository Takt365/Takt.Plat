// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Application.Services.Logistics.Quality.Complaint;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Complaint;

/// <summary>
/// 客诉主控制器
/// 提供客诉主的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "客诉主")]
public class TaktCustomerComplaintsController : TaktControllerBase
{
    private readonly ITaktCustomerComplaintService _customerComplaintService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintService">客诉主服务</param>
    public TaktCustomerComplaintsController(ITaktCustomerComplaintService customerComplaintService)
    {
        _customerComplaintService = customerComplaintService;
    }

    /// <summary>
    /// 获取客诉主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:list", "客诉主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerComplaintListAsync([FromQuery] TaktCustomerComplaintQueryDto queryDto)
    {
        try
        {
            var result = await _customerComplaintService.GetCustomerComplaintListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>客诉主DTO</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:query", "客诉主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerComplaintByIdAsync(long id)
    {
        try
        {
            var result = await _customerComplaintService.GetCustomerComplaintByIdAsync(id);
            if (result == null)
            {
                return NotFound("客诉主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客诉主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:query", "客诉主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerComplaintOptionsAsync()
    {
        try
        {
            var result = await _customerComplaintService.GetCustomerComplaintOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客诉主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客诉主DTO</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:create", "创建客诉主")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerComplaintAsync([FromBody] TaktCustomerComplaintCreateDto dto)
    {
        try
        {
            var result = await _customerComplaintService.CreateCustomerComplaintAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客诉主DTO</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:update", "更新客诉主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerComplaintAsync(long id, [FromBody] TaktCustomerComplaintUpdateDto dto)
    {
        try
        {
            var result = await _customerComplaintService.UpdateCustomerComplaintAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:delete", "删除客诉主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerComplaintByIdAsync(long id)
    {
        try
        {
            await _customerComplaintService.DeleteCustomerComplaintByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客诉主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:delete", "批量删除客诉主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerComplaintBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerComplaintService.DeleteCustomerComplaintBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>客诉主DTO</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:update", "更新客诉主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerComplaintStatusAsync([FromBody] TaktCustomerComplaintStatusDto dto)
    {
        try
        {
            var result = await _customerComplaintService.UpdateCustomerComplaintStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客诉主DTO</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:update", "更新客诉主排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerComplaintSortAsync([FromBody] TaktCustomerComplaintSortDto dto)
    {
        try
        {
            var result = await _customerComplaintService.UpdateCustomerComplaintSortAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:customercomplaint:import", "获取客诉主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerComplaintTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerComplaintService.GetCustomerComplaintTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客诉主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:import", "导入客诉主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerComplaintAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerComplaintService.ImportCustomerComplaintAsync(stream, sheetName);
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
    /// 导出客诉主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:customercomplaint:export", "导出客诉主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerComplaintAsync([FromQuery] TaktCustomerComplaintQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerComplaintService.ExportCustomerComplaintAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
