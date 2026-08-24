/**
 * Split material moving/model trend modules (DTOs, services, controllers).
 * Run: node scripts/split-material-trend-modules.cjs
 */
const fs = require('fs');
const path = require('path');

const root = path.join(__dirname, '..');
const materials = path.join(root, 'backend/src/Takt.Application');
const webApi = path.join(root, 'backend/src/Takt.WebApi/Controllers/Logistics/Materials');

function walk(dir, acc = []) {
  for (const name of fs.readdirSync(dir)) {
    const p = path.join(dir, name);
    const st = fs.statSync(p);
    if (st.isDirectory()) walk(p, acc);
    else if (p.endsWith('.cs')) acc.push(p);
  }
  return acc;
}

function renameFile(oldRel, newRel) {
  const oldPath = path.join(root, oldRel);
  const newPath = path.join(root, newRel);
  if (!fs.existsSync(oldPath)) {
    console.warn('skip missing', oldRel);
    return;
  }
  if (fs.existsSync(newPath)) fs.unlinkSync(newPath);
  fs.renameSync(oldPath, newPath);
  console.log('renamed', oldRel, '->', newRel);
}

// DTOs
const movingDto = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Materials;

/// <summary>
/// 物料 × 月份移动单价转置分析查询
/// </summary>
public class TaktMaterialMovingTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 移动价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 评估类别（可选；为空时按物料+估值分行）
    /// </summary>
    public string? Valuation { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料移动价格推移转置行（行=物料+估值，列=各月单价 MovingPrice÷PriceUnit）
/// </summary>
public class TaktMaterialMovingTrendDto
{
    public string PlantCode { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string Valuation { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);
    public string Trend { get; set; } = "none";
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public decimal? VarianceAmount { get; set; }
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 物料移动价格推移转置分析结果
/// </summary>
public class TaktMaterialMovingTrendResultDto
{
    public TaktPagedResult<TaktMaterialMovingTrendDto> Paged { get; set; } = null!;
    public List<string> PeriodOrder { get; set; } = new();
    public int MaterialCount { get; set; }
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public int UpCount { get; set; }
    public int DownCount { get; set; }
    public int FlatCount { get; set; }
    public int NoneCount { get; set; }
}
`;

const modelDto = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialModelTrendDtos.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Materials;

/// <summary>
/// 物料机种推移转置分析查询
/// </summary>
public class TaktMaterialModelTrendQueryDto : TaktPagedQuery
{
    public string PlantCode { get; set; } = string.Empty;
    public DateTime? PeriodDateStart { get; set; }
    public DateTime? PeriodDateEnd { get; set; }
    public string? FocusPeriod { get; set; }
    public string? Valuation { get; set; }
    /// <summary>
    /// 产品物料类型（机种推移必填；用于 BOM 产品组过滤，如 FERT）
    /// </summary>
    public string? MaterialType { get; set; }
    public string? MaterialCode { get; set; }
    /// <summary>
    /// 涨跌筛选：空/leading=领涨领跌各 50；all=全部；up/down/changed
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料机种推移行（物料×机种组×产品组 + 各月单价）
/// </summary>
public class TaktMaterialModelTrendDto : TaktMaterialMovingTrendDto
{
    public string ModelGroup { get; set; } = string.Empty;
    public string ProductGroup { get; set; } = string.Empty;
    public List<string> ModelCodes { get; set; } = new();
    public List<string> ProductCodes { get; set; } = new();
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 物料机种推移分析结果
/// </summary>
public class TaktMaterialModelTrendResultDto
{
    public TaktPagedResult<TaktMaterialModelTrendDto> Paged { get; set; } = null!;
    public List<string> PeriodOrder { get; set; } = new();
    public int MaterialCount { get; set; }
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public int UpCount { get; set; }
    public int DownCount { get; set; }
    public int FlatCount { get; set; }
    public int NoneCount { get; set; }
}
`;

fs.writeFileSync(
  path.join(materials, 'Dtos/Logistics/Materials/TaktMaterialMovingTrendDtos.cs'),
  movingDto,
  'utf8'
);
fs.writeFileSync(
  path.join(materials, 'Dtos/Logistics/Materials/TaktMaterialModelTrendDtos.cs'),
  modelDto,
  'utf8'
);
const oldDto = path.join(materials, 'Dtos/Logistics/Materials/TaktMaterialMovingPriceTrendDtos.cs');
if (fs.existsSync(oldDto)) fs.unlinkSync(oldDto);
console.log('wrote DTO files');

renameFile(
  'backend/src/Takt.Application/Services/Logistics/Materials/TaktMaterialMovingPriceTrendService.cs',
  'backend/src/Takt.Application/Services/Logistics/Materials/TaktMaterialTrendAnalysisService.cs'
);
renameFile(
  'backend/src/Takt.Application/Services/Logistics/Materials/ITaktMaterialMovingPriceTrendService.cs',
  'backend/src/Takt.Application/Services/Logistics/Materials/ITaktMaterialTrendAnalysisService.cs'
);

const replacements = [
  ['TaktMaterialMovingPriceMonthlyTrendQueryDto', 'TaktMaterialMovingTrendQueryDto'],
  ['TaktMaterialMovingPriceMonthlyTrendResultDto', 'TaktMaterialMovingTrendResultDto'],
  ['TaktMaterialMovingPriceMonthlyTrendDto', 'TaktMaterialMovingTrendDto'],
  ['TaktMaterialMovingPriceModelTrendResultDto', 'TaktMaterialModelTrendResultDto'],
  ['TaktMaterialMovingPriceModelTrendDto', 'TaktMaterialModelTrendDto'],
  ['GetMaterialMovingPriceMonthlyTrendAnalysisAsync', 'GetMaterialMovingTrendAnalysisAsync'],
  ['ExportMaterialMovingPriceMonthlyTrendAnalysisAsync', 'ExportMaterialMovingTrendAnalysisAsync'],
  ['GetMaterialMovingPriceModelTrendAnalysisAsync', 'GetMaterialModelTrendAnalysisAsync'],
  ['ExportMaterialMovingPriceModelTrendAnalysisAsync', 'ExportMaterialModelTrendAnalysisAsync'],
  ['GetMaterialMovingPriceTrendPlantOptionsAsync', 'GetMaterialMovingTrendPlantOptionsAsync'],
  ['GetMaterialMovingPriceTrendValuationOptionsAsync', 'GetMaterialMovingTrendValuationOptionsAsync'],
  ['GetMaterialMovingPriceTrendMaterialOptionsAsync', 'GetMaterialMovingTrendMaterialOptionsAsync'],
  ['ITaktMaterialMovingPriceTrendService', 'ITaktMaterialTrendAnalysisService'],
  ['TaktMaterialMovingPriceTrendService', 'TaktMaterialTrendAnalysisService'],
  ['物料月移动价格推移 / 机种推移', '物料移动价格/机种推移分析核心'],
  ['logistics:materials:model:moving:trend', 'logistics:materials:material:model:trend'],
];

for (const file of walk(path.join(root, 'backend/src'))) {
  let text = fs.readFileSync(file, 'utf8');
  let changed = false;
  for (const [from, to] of replacements) {
    if (text.includes(from)) {
      text = text.split(from).join(to);
      changed = true;
    }
  }
  if (changed) {
    fs.writeFileSync(file, text, 'utf8');
    console.log('patched', path.relative(root, file));
  }
}

// Core analysis interface
const coreIface = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialTrendAnalysisService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格/机种推移分析核心服务接口（内部编排）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料推移分析核心服务（移动价格转置 + 机种 BOM 扩展；供 Moving/Model 门面服务调用）
/// </summary>
public interface ITaktMaterialTrendAnalysisService
{
    Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync();
    Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string plantCode);
    Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string plantCode, string? valuation = null);
    Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(TaktMaterialMovingTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query, string? sheetName = null, string? fileName = null);
    Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(TaktMaterialModelTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(
        TaktMaterialModelTrendQueryDto query, string? sheetName = null, string? fileName = null);
}
`;

fs.writeFileSync(
  path.join(materials, 'Services/Logistics/Materials/ITaktMaterialTrendAnalysisService.cs'),
  coreIface,
  'utf8'
);

// Facade: moving trend service
const movingSvc = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialMovingTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移分析服务（门面）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料移动价格推移分析服务
/// </summary>
public class TaktMaterialMovingTrendService : TaktServiceBase, ITaktMaterialMovingTrendService
{
    private readonly ITaktMaterialTrendAnalysisService _analysisService;

    public TaktMaterialMovingTrendService(
        ITaktMaterialTrendAnalysisService analysisService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _analysisService = analysisService;
    }

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync() =>
        _analysisService.GetMaterialMovingTrendPlantOptionsAsync();

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string plantCode) =>
        _analysisService.GetMaterialMovingTrendValuationOptionsAsync(plantCode);

    public Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string plantCode, string? valuation = null) =>
        _analysisService.GetMaterialMovingTrendMaterialOptionsAsync(plantCode, valuation);

    public Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(TaktMaterialMovingTrendQueryDto queryDto) =>
        _analysisService.GetMaterialMovingTrendAnalysisAsync(queryDto);

    public Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query, string? sheetName = null, string? fileName = null) =>
        _analysisService.ExportMaterialMovingTrendAnalysisAsync(query, sheetName, fileName);
}
`;

const movingIface = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialMovingTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料移动价格推移分析服务
/// </summary>
public interface ITaktMaterialMovingTrendService
{
    Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync();
    Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string plantCode);
    Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string plantCode, string? valuation = null);
    Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(TaktMaterialMovingTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query, string? sheetName = null, string? fileName = null);
}
`;

const modelSvc = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移分析服务（门面）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料机种推移分析服务
/// </summary>
public class TaktMaterialModelTrendService : TaktServiceBase, ITaktMaterialModelTrendService
{
    private readonly ITaktMaterialTrendAnalysisService _analysisService;

    public TaktMaterialModelTrendService(
        ITaktMaterialTrendAnalysisService analysisService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _analysisService = analysisService;
    }

    public Task<List<TaktSelectOption>> GetMaterialModelTrendPlantOptionsAsync() =>
        _analysisService.GetMaterialMovingTrendPlantOptionsAsync();

    public Task<List<TaktSelectOption>> GetMaterialModelTrendValuationOptionsAsync(string plantCode) =>
        _analysisService.GetMaterialMovingTrendValuationOptionsAsync(plantCode);

    public Task<List<TaktSelectOption>> GetMaterialModelTrendMaterialOptionsAsync(string plantCode, string? valuation = null) =>
        _analysisService.GetMaterialMovingTrendMaterialOptionsAsync(plantCode, valuation);

    public Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(TaktMaterialModelTrendQueryDto queryDto) =>
        _analysisService.GetMaterialModelTrendAnalysisAsync(queryDto);

    public Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(
        TaktMaterialModelTrendQueryDto query, string? sheetName = null, string? fileName = null) =>
        _analysisService.ExportMaterialModelTrendAnalysisAsync(query, sheetName, fileName);
}
`;

const modelIface = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料机种推移分析服务
/// </summary>
public interface ITaktMaterialModelTrendService
{
    Task<List<TaktSelectOption>> GetMaterialModelTrendPlantOptionsAsync();
    Task<List<TaktSelectOption>> GetMaterialModelTrendValuationOptionsAsync(string plantCode);
    Task<List<TaktSelectOption>> GetMaterialModelTrendMaterialOptionsAsync(string plantCode, string? valuation = null);
    Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(TaktMaterialModelTrendQueryDto queryDto);
    Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(
        TaktMaterialModelTrendQueryDto query, string? sheetName = null, string? fileName = null);
}
`;

fs.writeFileSync(path.join(materials, 'Services/Logistics/Materials/ITaktMaterialMovingTrendService.cs'), movingIface, 'utf8');
fs.writeFileSync(path.join(materials, 'Services/Logistics/Materials/TaktMaterialMovingTrendService.cs'), movingSvc, 'utf8');
fs.writeFileSync(path.join(materials, 'Services/Logistics/Materials/ITaktMaterialModelTrendService.cs'), modelIface, 'utf8');
fs.writeFileSync(path.join(materials, 'Services/Logistics/Materials/TaktMaterialModelTrendService.cs'), modelSvc, 'utf8');

// Fix core service class implements interface + model method signature
const corePath = path.join(materials, 'Services/Logistics/Materials/TaktMaterialTrendAnalysisService.cs');
let core = fs.readFileSync(corePath, 'utf8');
core = core.replace(
  'public class TaktMaterialTrendAnalysisService : TaktServiceBase, ITaktMaterialTrendAnalysisService',
  'public class TaktMaterialTrendAnalysisService : TaktServiceBase, ITaktMaterialTrendAnalysisService'
);
// Model methods need TaktMaterialModelTrendQueryDto - add adapter at method entry
core = core.replace(
  'public async Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(\n        TaktMaterialMovingTrendQueryDto queryDto)',
  'public async Task<TaktMaterialModelTrendResultDto> GetMaterialModelTrendAnalysisAsync(\n        TaktMaterialModelTrendQueryDto queryDto)'
);
core = core.replace(
  'public async Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(\n        TaktMaterialMovingTrendQueryDto query,',
  'public async Task<(string fileName, byte[] fileContent)> ExportMaterialModelTrendAnalysisAsync(\n        TaktMaterialModelTrendQueryDto query,'
);
core = core.replace(
  'private async Task<ModelTrendAnalysisBuilt> BuildModelTrendAnalysisAsync(\n        TaktMaterialMovingTrendQueryDto queryDto)',
  'private async Task<ModelTrendAnalysisBuilt> BuildModelTrendAnalysisAsync(\n        TaktMaterialModelTrendQueryDto queryDto)'
);
core = core.replace(
  'private async Task<MonthlyTrendAnalysisBuilt> BuildMonthlyTrendAnalysisAsync(\n        TaktMaterialMovingTrendQueryDto queryDto)',
  'private async Task<MonthlyTrendAnalysisBuilt> BuildMonthlyTrendAnalysisAsync(\n        TaktMaterialMovingTrendQueryDto queryDto)'
);
// Model build calls monthly with same shape - add ToMovingQuery helper if needed
if (!core.includes('ToMovingTrendQuery')) {
  const helper = `
    private static TaktMaterialMovingTrendQueryDto ToMovingTrendQuery(TaktMaterialModelTrendQueryDto queryDto) =>
        new()
        {
            PlantCode = queryDto.PlantCode,
            PeriodDateStart = queryDto.PeriodDateStart,
            PeriodDateEnd = queryDto.PeriodDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            Valuation = queryDto.Valuation,
            MaterialCode = queryDto.MaterialCode,
            TrendFilter = queryDto.TrendFilter,
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
        };
`;
  core = core.replace(
    'private async Task<ModelTrendAnalysisBuilt> BuildModelTrendAnalysisAsync(',
    helper + '\n    private async Task<ModelTrendAnalysisBuilt> BuildModelTrendAnalysisAsync('
  );
  core = core.replace(
    'var monthly = await BuildMonthlyTrendAnalysisAsync(queryDto);',
    'var monthly = await BuildMonthlyTrendAnalysisAsync(ToMovingTrendQuery(queryDto));',
    1
  );
}
fs.writeFileSync(corePath, core, 'utf8');

// Controllers
const movingCtrl = fs.readFileSync(path.join(webApi, 'TaktMaterialMovingPriceTrendsController.cs'), 'utf8');
let mc = movingCtrl
  .replace(/TaktMaterialMovingPriceTrendsController/g, 'TaktMaterialMovingTrendsController')
  .replace(/ITaktMaterialMovingPriceTrendService/g, 'ITaktMaterialMovingTrendService')
  .replace(/_materialMovingPriceTrendService/g, '_materialMovingTrendService')
  .replace(/materialMovingPriceTrendService/g, 'materialMovingTrendService')
  .replace(/TaktMaterialMovingPriceMonthlyTrendQueryDto/g, 'TaktMaterialMovingTrendQueryDto')
  .replace(/GetMaterialMovingPriceMonthlyTrendAnalysisAsync/g, 'GetMaterialMovingTrendAnalysisAsync')
  .replace(/ExportMaterialMovingPriceMonthlyTrendAnalysisAsync/g, 'ExportMaterialMovingTrendAnalysisAsync')
  .replace(/monthly-trend-analysis/g, 'trend-analysis')
  .replace(/物料月移动价格推移 \/ 机种推移转置分析控制器[\s\S]*?public class/, '物料移动价格推移转置分析控制器\\n/// </summary>\\n[ApiModule(4, \"后勤管理\")]\\n[Route(\"api/[controller]\", Name = \"物料移动价格推移\")]\\npublic class');
// Remove model endpoints block
mc = mc.replace(/\s*\/\/\/ <summary>[\s\S]*?GetMaterialMovingPriceModelTrendAnalysisAsync[\s\S]*?ExportMaterialMovingPriceModelTrendAnalysisAsync[\s\S]*?\}\s*\}\s*$/m, '\n}\n');
fs.writeFileSync(path.join(webApi, 'TaktMaterialMovingTrendsController.cs'), mc, 'utf8');
fs.unlinkSync(path.join(webApi, 'TaktMaterialMovingPriceTrendsController.cs'));

const modelCtrl = `// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialModelTrendsController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料机种推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料机种推移")]
public class TaktMaterialModelTrendsController : TaktControllerBase
{
    private readonly ITaktMaterialModelTrendService _materialModelTrendService;

    public TaktMaterialModelTrendsController(ITaktMaterialModelTrendService materialModelTrendService)
    {
        _materialModelTrendService = materialModelTrendService;
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetMaterialModelTrendPlantOptionsAsync()
    {
        try
        {
            var result = await _materialModelTrendService.GetMaterialModelTrendPlantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移评估类别选项")]
    [HttpGet("valuation-options")]
    public async Task<IActionResult> GetMaterialModelTrendValuationOptionsAsync([FromQuery] string plantCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode)) return Success(new List<TaktSelectOption>(), "查询成功");
            var result = await _materialModelTrendService.GetMaterialModelTrendValuationOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetMaterialModelTrendMaterialOptionsAsync(
        [FromQuery] string plantCode, [FromQuery] string? valuation = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(valuation))
                return Success(new List<TaktSelectOption>(), "查询成功");
            var result = await _materialModelTrendService.GetMaterialModelTrendMaterialOptionsAsync(plantCode, valuation);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetMaterialModelTrendAnalysisAsync([FromQuery] TaktMaterialModelTrendQueryDto queryDto)
    {
        try
        {
            var result = await _materialModelTrendService.GetMaterialModelTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:export", "清单导出物料机种推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportMaterialModelTrendAnalysisAsync(
        [FromQuery] TaktMaterialModelTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialModelTrendService.ExportMaterialModelTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
`;
fs.writeFileSync(path.join(webApi, 'TaktMaterialModelTrendsController.cs'), modelCtrl, 'utf8');

// Menu seed patch hints in console
console.log('done - update menu seeds manually if script missed them');
