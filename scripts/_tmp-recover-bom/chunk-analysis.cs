            fileName ?? "BOM物料成本导出.xlsx");
    }

    // ========================================
    // 转置分析（产品 × 月份成本）
    // ========================================

    /// <summary>
    /// 获取 BOM 物料成本转置列表（行=产品，列=月份总成本）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置结果</returns>
    public async Task<TaktBomMaterialCostTransposedResultDto> GetBomMaterialCostTransposedListAsync(TaktBomMaterialCostTransposedQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var rows = await LoadAnalysisRowsAsync(queryDto);
        var periodOrder = BuildPeriodOrder(rows, queryDto.ProcessedDateStart, queryDto.ProcessedDateEnd);
        var productGroups = rows
            .GroupBy(r => r.ProductCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key)
            .ToList();
        var total = productGroups.Count;
        var pageGroups = productGroups.Skip(skip).Take(pageSize).ToList();
        var transposedRows = pageGroups
            .Select(g => BuildTransposedRow(g.Key, g.ToList(), periodOrder))
            .ToList();
        return new TaktBomMaterialCostTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostTransposedDto>.Create(transposedRows, total, pageIndex, pageSize),
            PeriodOrder = periodOrder,
        };
    }

    /// <summary>
    /// 获取 BOM 物料成本差异分析（两期间组件级对比）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>差异分析结果</returns>
    public async Task<TaktBomMaterialCostVarianceResultDto> GetBomMaterialCostVarianceAnalysisAsync(TaktBomMaterialCostVarianceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ProductCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.BasePeriod);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ComparePeriod);
        EnsureThreeLayerContext();
        var (baseStart, baseEnd) = ResolvePeriodDateRange(queryDto.BasePeriod);
        var (compareStart, compareEnd) = ResolvePeriodDateRange(queryDto.ComparePeriod);
        var rangeStart = baseStart < compareStart ? baseStart : compareStart;
        var rangeEnd = baseEnd > compareEnd ? baseEnd : compareEnd;
        var query = new TaktBomMaterialCostTransposedQueryDto
        {
            ProductCode = queryDto.ProductCode,
            ProcessedDateStart = rangeStart,
            ProcessedDateEnd = rangeEnd,
            BomMaterialCostStatus = 1,
            PageIndex = 1,
            PageSize = TaktPagedClamp.DefaultPageSize,
        };
        var rows = await LoadAnalysisRowsAsync(query);
        var baseSnapshot = TaktBomMaterialCostLineCostHelper.ResolvePeriodSnapshot(rows, queryDto.ProductCode, queryDto.BasePeriod);
        var compareSnapshot = TaktBomMaterialCostLineCostHelper.ResolvePeriodSnapshot(rows, queryDto.ProductCode, queryDto.ComparePeriod);
        var productDescription = compareSnapshot.FirstOrDefault()?.ProductDescription
            ?? baseSnapshot.FirstOrDefault()?.ProductDescription
            ?? string.Empty;
        var baseMap = baseSnapshot.ToDictionary(TaktBomMaterialCostLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var compareMap = compareSnapshot.ToDictionary(TaktBomMaterialCostLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var componentKeys = baseMap.Keys.Union(compareMap.Keys, StringComparer.Ordinal).ToList();
        var lines = new List<TaktBomMaterialCostVarianceLineDto>();
        foreach (var key in componentKeys)
        {
            baseMap.TryGetValue(key, out var baseRow);
            compareMap.TryGetValue(key, out var compareRow);
            lines.Add(BuildVarianceLine(baseRow, compareRow));
        }
        lines = lines.OrderByDescending(l => Math.Abs(l.VarianceAmount)).ToList();
        var baseTotal = TaktBomMaterialCostLineCostHelper.SumSnapshotCost(baseSnapshot);
        var compareTotal = TaktBomMaterialCostLineCostHelper.SumSnapshotCost(compareSnapshot);
        return new TaktBomMaterialCostVarianceResultDto
        {
            ProductCode = queryDto.ProductCode,
            ProductDescription = productDescription,
            BasePeriod = queryDto.BasePeriod,
            ComparePeriod = queryDto.ComparePeriod,
            BaseTotalCost = baseTotal,
            CompareTotalCost = compareTotal,
            TotalVariance = compareTotal - baseTotal,
            Lines = lines,
        };
    }

    /// <summary>
    /// 导出 BOM 物料成本转置报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostTransposedAsync(
        TaktBomMaterialCostTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        query ??= new TaktBomMaterialCostTransposedQueryDto();
        query.PageIndex = 1;
        query.PageSize = TaktPagedClamp.MaxPageSize;
        var result = await GetBomMaterialCostTransposedListAsync(query);
        var periodOrder = result.PeriodOrder;
        var columnKeys = new List<string> { "productCode", "productDescription" };
        var columnLabels = new List<string> { "产品编码", "产品描述" };
        foreach (var period in periodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        var exportRows = result.Paged.Data.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["productCode"] = row.ProductCode,
                ["productDescription"] = row.ProductDescription,
            };
            foreach (var period in periodOrder)
            {
                row.PeriodCosts.TryGetValue(period, out var cost);
                dict[$"period_{period}"] = cost;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "BOM成本转置分析",
            fileName ?? "BOM成本转置分析.xlsx");
    }

    /// <summary>
    /// 导出 BOM 物料成本差异分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostVarianceAnalysisAsync(
        TaktBomMaterialCostVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostVarianceAnalysisAsync(query);
        var summaryRows = new List<IReadOnlyDictionary<string, object?>>
        {
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "产品编码",
                ["value"] = result.ProductCode,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "产品描述",
                ["value"] = result.ProductDescription,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "基准期间",
                ["value"] = result.BasePeriod,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "对比期间",
                ["value"] = result.ComparePeriod,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "基准总成本",
                ["value"] = result.BaseTotalCost,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "对比总成本",
                ["value"] = result.CompareTotalCost,
            },
            new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["field"] = "总差异",
                ["value"] = result.TotalVariance,
            },
        };
        var detailKeys = new[]
        {
            "bomItemNo", "componentCode", "componentDescription", "purchaseType", "currency",
            "baseCost", "compareCost", "varianceAmount", "variancePercent",
            "baseUnitPrice", "compareUnitPrice", "unitPriceVariance",
            "baseQuantity", "compareQuantity", "quantityVariance",
            "priceEffectAmount", "quantityEffectAmount", "changeType",
        };
        var detailLabels = new[]
        {
            "BOM项目号", "组件编码", "组件描述", "采购类型", "货币",
            "基准成本", "对比成本", "成本差异", "差异率%",
            "基准单价", "对比单价", "单价差异",
            "基准数量", "对比数量", "数量差异",
            "价格影响额", "数量影响额", "变动类型",
        };
        var detailRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["bomItemNo"] = line.BomItemNo,
            ["componentCode"] = line.ComponentCode,
            ["componentDescription"] = line.ComponentDescription,
            ["purchaseType"] = line.PurchaseType,
            ["currency"] = line.Currency,
            ["baseCost"] = line.BaseCost,
            ["compareCost"] = line.CompareCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = line.VariancePercent,
            ["baseUnitPrice"] = line.BaseUnitPrice,
            ["compareUnitPrice"] = line.CompareUnitPrice,
            ["unitPriceVariance"] = line.UnitPriceVariance,
            ["baseQuantity"] = line.BaseQuantity,
            ["compareQuantity"] = line.CompareQuantity,
            ["quantityVariance"] = line.QuantityVariance,
            ["priceEffectAmount"] = line.PriceEffectAmount,
            ["quantityEffectAmount"] = line.QuantityEffectAmount,
            ["changeType"] = line.ChangeType,
        }).ToList();
        using var package = new OfficeOpenXml.ExcelPackage();
        var summarySheet = package.Workbook.Worksheets.Add("汇总");
        summarySheet.Cells[1, 1].LoadFromArrays(new[] { new[] { "字段", "值" } });
        summarySheet.Cells[2, 1].LoadFromArrays(summaryRows.Select(r => new object[] { r["field"]!, r["value"]! }).ToArray());
        var detailSheet = package.Workbook.Worksheets.Add(sheetName ?? "差异明细");
        detailSheet.Cells[1, 1].LoadFromArrays(new[] { detailLabels });
        if (detailRows.Count > 0)
        {
            var dataArray = detailRows.Select(row => detailKeys.Select(k => row.TryGetValue(k, out var v) ? v ?? DBNull.Value : DBNull.Value).ToArray()).ToList();
            detailSheet.Cells[2, 1].LoadFromArrays(dataArray);
        }
        if (detailSheet.Dimension != null)
        {
            detailSheet.Cells[detailSheet.Dimension.Address].AutoFitColumns();
        }
        var actualFileName = TaktNamingHelper.DefaultExcelFileName(fileName ?? "BOM成本差异分析");
        var content = await package.GetAsByteArrayAsync();
        return (actualFileName, content);
    }

    /// <summary>
    /// 加载转置/差异分析用成本行（租户+公司范围内）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>成本行列表</returns>
    private async Task<List<TaktBomMaterialCost>> LoadAnalysisRowsAsync(TaktBomMaterialCostTransposedQueryDto queryDto)
    {
        var status = queryDto.BomMaterialCostStatus ?? 1;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        exp = exp.And(x => x.BomMaterialCostStatus == status);
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(queryDto.ProductCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords)));
        }
        if (queryDto.ProcessedDateStart.HasValue)
        {
            exp = exp.And(x => x.ProcessedDate >= queryDto.ProcessedDateStart);
        }
        if (queryDto.ProcessedDateEnd.HasValue)
        {
            exp = exp.And(x => x.ProcessedDate <= queryDto.ProcessedDateEnd);
        }
        return await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
    }

    /// <summary>
    /// 构建期间列顺序
    /// </summary>
    /// <param name="rows">成本行</param>
    /// <param name="start">起始日期</param>
    /// <param name="end">结束日期</param>
    /// <returns>期间键列表</returns>
    private static List<string> BuildPeriodOrder(IReadOnlyList<TaktBomMaterialCost> rows, DateTime? start, DateTime? end)
    {
        var periods = rows
            .Select(r => TaktBomMaterialCostLineCostHelper.ToPeriodKey(r.ProcessedDate))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
        if (start.HasValue && end.HasValue && start.Value <= end.Value)
        {
            var cursor = new DateTime(start.Value.Year, start.Value.Month, 1);
            var endMonth = new DateTime(end.Value.Year, end.Value.Month, 1);
            var rangePeriods = new List<string>();
            while (cursor <= endMonth)
            {
                rangePeriods.Add(TaktBomMaterialCostLineCostHelper.ToPeriodKey(cursor));
                cursor = cursor.AddMonths(1);
            }
            foreach (var period in periods)
            {
                if (!rangePeriods.Contains(period, StringComparer.Ordinal))
                {
                    rangePeriods.Add(period);
                }
            }
            rangePeriods.Sort(StringComparer.Ordinal);
            return rangePeriods;
        }
        return periods;
    }

    /// <summary>
    /// 构建单产品转置行
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <param name="productRows">产品相关行</param>
    /// <param name="periodOrder">期间顺序</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostTransposedDto BuildTransposedRow(
        string productCode,
        List<TaktBomMaterialCost> productRows,
        IReadOnlyList<string> periodOrder)
    {
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var snapshot = TaktBomMaterialCostLineCostHelper.ResolvePeriodSnapshot(productRows, productCode, period);
            if (snapshot.Count > 0)
            {
                periodCosts[period] = TaktBomMaterialCostLineCostHelper.SumSnapshotCost(snapshot);
            }
        }
        return new TaktBomMaterialCostTransposedDto
        {
            ProductCode = productCode,
            ProductDescription = productRows.FirstOrDefault()?.ProductDescription ?? string.Empty,
            PeriodCosts = periodCosts,
        };
    }

    /// <summary>
    /// 构建组件差异行
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <returns>差异行 DTO</returns>
    private static TaktBomMaterialCostVarianceLineDto BuildVarianceLine(
        TaktBomMaterialCost? baseRow,
        TaktBomMaterialCost? compareRow)
    {
        var baseCost = baseRow != null ? TaktBomMaterialCostLineCostHelper.CalculateLineCost(baseRow) : 0m;
        var compareCost = compareRow != null ? TaktBomMaterialCostLineCostHelper.CalculateLineCost(compareRow) : 0m;
        var baseUnitPrice = baseRow != null ? TaktBomMaterialCostLineCostHelper.ResolveEffectiveUnitPrice(baseRow) : 0m;
        var compareUnitPrice = compareRow != null ? TaktBomMaterialCostLineCostHelper.ResolveEffectiveUnitPrice(compareRow) : 0m;
        var baseQty = baseRow?.ComponentQuantity ?? 0m;
        var compareQty = compareRow?.ComponentQuantity ?? 0m;
        var priceUnit = baseRow != null
            ? TaktBomMaterialCostLineCostHelper.ResolvePriceUnit(baseRow)
            : (compareRow != null ? TaktBomMaterialCostLineCostHelper.ResolvePriceUnit(compareRow) : 1);
        if (priceUnit <= 0)
        {
            priceUnit = 1;
        }
        var unitPriceVariance = compareUnitPrice - baseUnitPrice;
        var quantityVariance = compareQty - baseQty;
        var priceEffect = unitPriceVariance * baseQty / priceUnit;
        var quantityEffect = quantityVariance * baseUnitPrice / priceUnit;
        var varianceAmount = compareCost - baseCost;
        decimal? variancePercent = null;
        if (baseCost != 0m)
        {
            variancePercent = Math.Round(varianceAmount / baseCost * 100m, 2);
        }
        var changeType = ResolveChangeType(baseRow, compareRow, unitPriceVariance, quantityVariance);
        return new TaktBomMaterialCostVarianceLineDto
        {
            BomItemNo = compareRow?.BomItemNo ?? baseRow?.BomItemNo ?? string.Empty,
            ComponentCode = compareRow?.ComponentCode ?? baseRow?.ComponentCode ?? string.Empty,
            ComponentDescription = compareRow?.ComponentDescription ?? baseRow?.ComponentDescription ?? string.Empty,
            PurchaseType = compareRow?.PurchaseType ?? baseRow?.PurchaseType ?? string.Empty,
            Currency = compareRow != null
                ? TaktBomMaterialCostLineCostHelper.ResolveCurrency(compareRow)
                : (baseRow != null ? TaktBomMaterialCostLineCostHelper.ResolveCurrency(baseRow) : string.Empty),
            BaseCost = baseCost,
            CompareCost = compareCost,
            VarianceAmount = varianceAmount,
            VariancePercent = variancePercent,
            BaseUnitPrice = baseUnitPrice,
            CompareUnitPrice = compareUnitPrice,
            UnitPriceVariance = unitPriceVariance,
            BaseQuantity = baseQty,
            CompareQuantity = compareQty,
            QuantityVariance = quantityVariance,
            PriceEffectAmount = priceEffect,
            QuantityEffectAmount = quantityEffect,
            ChangeType = changeType,
        };
    }

    /// <summary>
    /// 解析组件变动类型
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <param name="unitPriceVariance">单价差异</param>
    /// <param name="quantityVariance">数量差异</param>
    /// <returns>变动类型码</returns>
    private static string ResolveChangeType(
        TaktBomMaterialCost? baseRow,
        TaktBomMaterialCost? compareRow,
        decimal unitPriceVariance,
        decimal quantityVariance)
    {
        if (baseRow == null && compareRow != null)
        {
            return "new";
        }
        if (baseRow != null && compareRow == null)
        {
            return "removed";
        }
        var hasPrice = unitPriceVariance != 0m;
        var hasQty = quantityVariance != 0m;
        if (hasPrice && hasQty)
        {
            return "mixed";
        }
        if (hasPrice)
        {
            return "price";
        }
        if (hasQty)
        {
            return "quantity";
        }
        return "unchanged";
    }

    /// <summary>
    /// 将 yyyy-MM 期间键解析为日期范围
    /// </summary>
    /// <param name="periodKey">期间键</param>
    /// <returns>起止日期</returns>
    private static (DateTime Start, DateTime End) ResolvePeriodDateRange(string periodKey)
    {
        if (!DateTime.TryParseExact(periodKey, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var monthStart))
        {
            throw new TaktBusinessException($"无效的期间格式：{periodKey}，应为 yyyy-MM");
        }
        var start = new DateTime(monthStart.Year, monthStart.Month, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }

    // ========================================
    // 查询表达式
    // ========================================