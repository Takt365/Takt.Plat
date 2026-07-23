    public async Task<TaktBomMaterialCostTransposedResultDto> GetBomMaterialCostTransposedListAsync(TaktBomMaterialCostTransposedQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var rows = await LoadAnalysisRowsAsync(queryDto, externalPurchaseOnly: true);
        var periodOrder = BuildPeriodOrder(rows, queryDto.ProcessedDateStart, queryDto.ProcessedDateEnd);
        var plantCode = queryDto.PlantCode?.Trim() ?? string.Empty;
        var productGroups = rows
            .Where(TaktBomMaterialCostLineCostHelper.CountsTowardBomMaterialCost)
            .GroupBy(r => r.ProductCode ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        List<(string ProductCode, string ProductDescription)> productCatalog;
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode) && string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            productCatalog = await GetOrderedModelMaterialsAsync(queryDto.ModelCode.Trim());
        }
        else
        {
            productCatalog = productGroups
                .Select(kv => (kv.Key, kv.Value.FirstOrDefault()?.ProductDescription?.Trim() ?? string.Empty))
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .ToList();
        }
        var total = productCatalog.Count;
        var pageCatalog = productCatalog.Skip(skip).Take(pageSize).ToList();
        var transposedRows = pageCatalog
            .Select(item => BuildTransposedRowForCatalogItem(plantCode, item.ProductCode, item.ProductDescription, productGroups, periodOrder))
            .ToList();
        return new TaktBomMaterialCostTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostTransposedDto>.Create(transposedRows, total, pageIndex, pageSize),
            PeriodOrder = periodOrder,
        };
    }