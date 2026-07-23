    public async Task<TaktBomMaterialCostMonthlyTrendResultDto> GetBomMaterialCostMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ModelCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = queryDto.ModelCode.Trim();
        var productCode = string.IsNullOrWhiteSpace(queryDto.ProductCode) ? null : queryDto.ProductCode.Trim();
        var allMaterialsUnderModel = productCode == null;
        var (rangeStart, rangeEnd) = ResolvePeriodRangeBounds(queryDto.PeriodStart, queryDto.PeriodEnd);
        var loadQuery = new TaktBomMaterialCostTransposedQueryDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode,
            ProcessedDateStart = rangeStart,
            ProcessedDateEnd = rangeEnd,
            BomMaterialCostStatus = 1,
            PageIndex = 1,
            PageSize = TaktPagedClamp.DefaultPageSize,
        };
        var rows = await LoadAnalysisRowsAsync(loadQuery, externalPurchaseOnly: true);
        var periodOrder = BuildPeriodOrder(rows, rangeStart, rangeEnd);
        var productCodesInScope = ResolveProductCodesInScope(rows, productCode);
        var productDescription = allMaterialsUnderModel
            ? string.Empty
            : rows.FirstOrDefault(r => TaktBomMaterialCostLineCostHelper.ProductCodeMatches(r.ProductCode, productCode!))?.ProductDescription ?? string.Empty;
        var trendLines = new List<TaktBomMaterialCostMonthlyTrendLineDto>();
        decimal? previousCost = null;
        string? previousPeriod = null;
        foreach (var period in periodOrder)
        {
            decimal totalCost = 0;
            var hasData = false;
            foreach (var scopedProductCode in productCodesInScope)
            {
                var snapshot = TaktBomMaterialCostLineCostHelper.ResolvePeriodSnapshot(
                    rows, plantCode, scopedProductCode, period);
                if (snapshot.Count == 0)
                {
                    continue;
                }
                hasData = true;
                totalCost += TaktBomMaterialCostLineCostHelper.SumSnapshotCost(snapshot);
            }
            if (!hasData)
            {
                continue;
            }
            decimal? varianceAmount = null;
            decimal? variancePercent = null;
            string trend = "none";
            if (previousCost.HasValue && previousPeriod != null)
            {
                varianceAmount = totalCost - previousCost.Value;
                if (previousCost.Value != 0m)
                {
                    variancePercent = Math.Round(varianceAmount.Value / previousCost.Value * 100m, 2);
                }
                if (totalCost > previousCost.Value)
                {
                    trend = "up";
                }
                else if (totalCost < previousCost.Value)
                {
                    trend = "down";
                }
                else
                {
                    trend = "flat";
                }
            }
            trendLines.Add(new TaktBomMaterialCostMonthlyTrendLineDto
            {
                Period = period,
                TotalCost = totalCost,
                BasePeriod = previousPeriod,
                BaseTotalCost = previousCost,
                VarianceAmount = varianceAmount,
                VariancePercent = variancePercent,
                Trend = trend,
            });
            previousCost = totalCost;
            previousPeriod = period;
        }
        return new TaktBomMaterialCostMonthlyTrendResultDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode ?? string.Empty,
            ProductDescription = productDescription,
            AllMaterialsUnderModel = allMaterialsUnderModel,
            Lines = trendLines,
        };
    }