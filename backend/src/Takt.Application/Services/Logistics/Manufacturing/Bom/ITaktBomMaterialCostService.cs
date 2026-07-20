// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM物料成本应用服务接口
/// </summary>
public interface ITaktBomMaterialCostService
{
    /// <summary>
    /// 获取BOM物料成本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBomMaterialCostDto>> GetBomMaterialCostListAsync(TaktBomMaterialCostQueryDto queryDto);

    /// <summary>
    /// 获取机种维度汇总列表（分页；同一 takt_bom_material_cost 表按工厂+机种+核算期间聚合，非拆表）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBomMaterialCostModelGroupDto>> GetBomMaterialCostModelGroupListAsync(TaktBomMaterialCostQueryDto queryDto);

    /// <summary>
    /// 根据ID获取BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostDto?> GetBomMaterialCostByIdAsync(long id);

    /// <summary>
    /// 获取BOM物料成本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostOptionsAsync();

    /// <summary>
    /// 获取机种下拉选项（来自汇总表 takt_bom_material_cost 的 ModelCode 去重；可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项（DictValue=机种编码）</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostModelOptionsAsync(string? plantCode = null);

    /// <summary>
    /// 创建BOM物料成本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostDto> CreateBomMaterialCostAsync(TaktBomMaterialCostCreateDto dto);

    /// <summary>
    /// 更新BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostDto> UpdateBomMaterialCostAsync(long id, TaktBomMaterialCostUpdateDto dto);

    /// <summary>
    /// 删除BOM物料成本
    /// </summary>
    /// <param name="id">BOM物料成本ID</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostByIdAsync(long id);

    /// <summary>
    /// 批量删除BOM物料成本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBomMaterialCostTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入BOM物料成本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出BOM物料成本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAsync(TaktBomMaterialCostQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 按明细回算并新增/更新主表（同工厂+机种+产品+核算月仅一行；取该月最后核算日结果；同步机种编码与机种月平均）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="costingDate">核算日期（用于定位核算月）</param>
    /// <returns>主表 DTO；无明细且无既有主表时返回 null</returns>
    Task<TaktBomMaterialCostDto?> SyncBomMaterialCostFromItemsAsync(string plantCode, string productCode, DateTime costingDate);

    /// <summary>
    /// 批量按明细回算主表（导入后按工厂+产品+核算月去重）
    /// </summary>
    /// <param name="keys">工厂+产品+核算日期提示</param>
    /// <returns>任务</returns>
    Task SyncBomMaterialCostFromItemsBatchAsync(IEnumerable<(string PlantCode, string ProductCode, DateTime CostingDate)> keys);

}
