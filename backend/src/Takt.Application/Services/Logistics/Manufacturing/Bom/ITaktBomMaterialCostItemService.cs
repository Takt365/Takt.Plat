// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostItemService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM物料成本明细应用服务接口
/// </summary>
public interface ITaktBomMaterialCostItemService
{
    /// <summary>
    /// 获取BOM物料成本明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktBomMaterialCostItemDto>> GetBomMaterialCostItemListAsync(TaktBomMaterialCostItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto?> GetBomMaterialCostItemByIdAsync(long id);

    /// <summary>
    /// 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项（DictValue=产品编码）</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostItemOptionsAsync(string? plantCode = null);

    /// <summary>
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto> CreateBomMaterialCostItemAsync(TaktBomMaterialCostItemCreateDto dto);

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktBomMaterialCostItemDto> UpdateBomMaterialCostItemAsync(long id, TaktBomMaterialCostItemUpdateDto dto);

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostItemByIdAsync(long id);

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteBomMaterialCostItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetBomMaterialCostItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出BOM物料成本明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemAsync(TaktBomMaterialCostItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 成本合计/重算：明细按工厂+核算期间+产品分组合计后 Upsert 主表；产品须 MaterialType=FERT；再刷新机种月均
    /// </summary>
    /// <param name="queryDto">筛选（PlantCode/ModelCode 可选；CostingDateStart/End 须为同一自然月）</param>
    /// <param name="forceRecalculate">为 true 时走重置路径（Sync 范围同合计）</param>
    /// <param name="processRecordCount">处理明细产品组上限（0=全部；默认 5000）</param>
    /// <returns>重算统计</returns>
    Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto> RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
        TaktBomMaterialCostItemQueryDto queryDto,
        bool forceRecalculate = false,
        int processRecordCount = 5000);

    /// <summary>
    /// Quartz 成本合计：仅合计判定日所在自然月（CostingDate 当月；调度由 Cron 控制）
    /// </summary>
    /// <param name="force">保留参数（兼容旧调用；当前无额外门禁）</param>
    /// <param name="asOfDate">判定日；默认今天（目标月=该日所在月）</param>
    /// <param name="nthWorkingDay">保留参数（兼容旧调用）</param>
    /// <returns>合计统计</returns>
    Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto?> RunScheduledBomMaterialCostSumAsync(
        bool force = false,
        DateTime? asOfDate = null,
        int nthWorkingDay = 3);

    /// <summary>
    /// Quartz 重算成本：仅 force 重算判定日所在自然月（CostingDate 当月；调度由 Cron 控制）
    /// </summary>
    /// <param name="force">保留参数（兼容旧调用；当前无额外门禁）</param>
    /// <param name="asOfDate">判定日；默认今天（目标月=该日所在月）</param>
    /// <param name="nthWorkingDay">保留参数（兼容旧调用）</param>
    /// <returns>重算统计</returns>
    Task<TaktBomMaterialCostItemRecalculateModelAverageResultDto?> RunScheduledBomMaterialCostRecalculateAsync(
        bool force = false,
        DateTime? asOfDate = null,
        int nthWorkingDay = 3);

    /// <summary>
    /// 获取成本分析转置列表：产品行取自主表 TaktBomMaterialCost（工厂/机种/期间），列=各月 ProductMonthlyCost
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置结果</returns>
    Task<TaktBomMaterialCostItemTransposedResultDto> GetBomMaterialCostItemTransposedListAsync(
        TaktBomMaterialCostItemTransposedQueryDto queryDto);

    /// <summary>
    /// 导出 BOM 物料成本明细转置报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemTransposedAsync(
        TaktBomMaterialCostItemTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取涨跌差异分析：按工厂/产品/两期间从子表 TaktBomMaterialCostItem 对比组件成本
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>差异分析结果</returns>
    Task<TaktBomMaterialCostItemVarianceResultDto> GetBomMaterialCostItemVarianceAnalysisAsync(
        TaktBomMaterialCostItemVarianceQueryDto queryDto);

    /// <summary>
    /// 导出 BOM 物料成本明细差异分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemVarianceAnalysisAsync(
        TaktBomMaterialCostItemVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取 BOM 物料成本明细月度涨跌分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    Task<TaktBomMaterialCostItemMonthlyTrendResultDto> GetBomMaterialCostItemMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostItemMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出 BOM 物料成本明细月度涨跌分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostItemMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// BOM 成本推移：单个产品下明细组件×月材料成本并算环比（来自 TaktBomMaterialCostItem；不按机种跨产品合并）
    /// </summary>
    /// <param name="queryDto">查询 DTO（PlantCode + ProductCode 必填；ModelCode 可选）</param>
    /// <returns>明细组件×月材料成本结果</returns>
    Task<TaktBomMaterialCostItemComponentMovingPriceResultDto> GetBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemComponentMovingPriceQueryDto queryDto);

    /// <summary>
    /// 导出 BOM 成本推移（单个产品明细组件×月材料成本）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostItemComponentMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 机种成本推移：机种月材料成本（产品月成本平均）+ 按 Plant/Component/ProductionRelated/PurchaseType 合并分析（缺月不回填）
    /// </summary>
    /// <param name="queryDto">查询 DTO（PlantCode + ModelCode 必填）</param>
    /// <returns>机种月成本与合并键分析行</returns>
    Task<TaktBomMaterialCostItemModelCostTrendResultDto> GetBomMaterialCostItemModelCostTrendAnalysisAsync(
        TaktBomMaterialCostItemModelCostTrendQueryDto queryDto);

    /// <summary>
    /// 导出机种成本推移分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemModelCostTrendAnalysisAsync(
        TaktBomMaterialCostItemModelCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 机种下 X+F 且移动平均价=0 的 BOM 行按组件合并（机种 / 组件 / 共用产品列表）
    /// </summary>
    /// <param name="queryDto">工厂+机种+核算月</param>
    /// <returns>合并分页结果</returns>
    Task<TaktBomMaterialCostItemZeroMovingPriceResultDto> GetBomMaterialCostItemZeroMovingPriceMergedAsync(
        TaktBomMaterialCostItemZeroMovingPriceQueryDto queryDto);

    /// <summary>
    /// 导出机种零价格合并清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemZeroMovingPriceMergedAsync(
        TaktBomMaterialCostItemZeroMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取 BOM 物料成本机种下拉选项（型号目的地 ModelCode 去重，可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostItemModelOptionsAsync(string? plantCode = null);

    /// <summary>
    /// 按机种获取 BOM 物料成本产品下拉选项
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetBomMaterialCostItemProductOptionsByModelAsync(string? modelCode, string? plantCode = null);

    /// <summary>
    /// 根据产品编码反查机种编码（型号目的地）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <param name="plantCode">工厂代码（可选，保留参数与前端一致）</param>
    /// <returns>机种编码；未匹配时返回 null</returns>
    Task<string?> GetBomMaterialCostItemModelCodeByProductAsync(string productCode, string? plantCode = null);
}
