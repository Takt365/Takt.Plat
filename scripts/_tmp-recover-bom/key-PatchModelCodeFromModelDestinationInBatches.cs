    /// <summary>
    /// 构建批量重算维度组：工厂+机种 × 月份序列（逐月独立重算）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="plantModelPairs">工厂+机种去重集合</param>
    /// <param name="monthSequence">处理月份序列</param>
    /// <returns>维度组集合</returns>
    private static HashSet<(string PlantCode, string ModelCode, int Year, int Month)> BuildRecalculateRefreshGroupsFromPairs(
        TaktBomMaterialCostQueryDto queryDto,
        HashSet<(string PlantCode, string ModelCode)> plantModelPairs,
        IReadOnlyList<(int Year, int Month)> monthSequence)
    {
        var pairs = new HashSet<(string PlantCode, string ModelCode)>(plantModelPairs);
        var plantFilter = queryDto.PlantCode?.Trim() ?? string.Empty;
        var modelFilter = queryDto.ModelCode?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(plantFilter) && !string.IsNullOrWhiteSpace(modelFilter))
        {
            pairs.Add((plantFilter, modelFilter));
        }
        var refreshGroups = new HashSet<(string PlantCode, string ModelCode, int Year, int Month)>();
        foreach (var (plantCode, modelCode) in pairs)
        {
            foreach (var (year, month) in monthSequence)
            {
                refreshGroups.Add((plantCode, modelCode, year, month));
            }
        }
        return refreshGroups;
    }

    /// <summary>
    /// 分页从 TaktModelDestination 回填 BOM 行 ModelCode（避免单月全量加载）
    /// </summary>
    /// <param name="predicate">单月查询条件</param>
    /// <param name="lookup">MaterialCode→ModelCode 查找表</param>
    /// <returns>写回行数</returns>
    private async Task<int> PatchModelCodeFromModelDestinationInBatchesAsync(
        Expression<Func<TaktBomMaterialCost, bool>> predicate,
        IReadOnlyDictionary<string, string> lookup)
    {
        var patchedRowCount = 0;
        var pageIndex = 1;
        while (true)
        {
            var (items, total) = await _bomMaterialCostRepository.GetPagedAsync(
                predicate,
                pageIndex,
                RecalculateBatchPageSize,
                x => x.Id,
                isDesc: false);
            if (items.Count == 0)
            {
                break;
            }
            patchedRowCount += await PersistModelCodeFromModelDestinationAsync(items, lookup);
            if (checked(pageIndex * RecalculateBatchPageSize) >= total)
            {
                break;
            }
            pageIndex += 1;
        }
        return patchedRowCount;
    }

    /// <summary>
    /// 分页收集单月内工厂+机种去重对（用于批量重算维度组）
    /// </summary>
    /// <param name="predicate">单月查询条件</param>
    /// <returns>工厂+机种集合</returns>
    private async Task<HashSet<(string PlantCode, string ModelCode)>> CollectRecalculatePlantModelPairsAsync(
        Expression<Func<TaktBomMaterialCost, bool>> predicate)
    {
        var pairs = new HashSet<(string PlantCode, string ModelCode)>();
        var pageIndex = 1;
        while (true)
        {
            var (items, total) = await _bomMaterialCostRepository.GetPagedAsync(
                predicate,
                pageIndex,
                RecalculateBatchPageSize,
                x => x.Id,
                isDesc: false);
            if (items.Count == 0)
            {
                break;
            }
            foreach (var row in items)
            {
                var plantCode = row.PlantCode?.Trim() ?? string.Empty;
                var modelCode = row.ModelCode?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(modelCode))
                {
                    continue;
                }
                pairs.Add((plantCode, modelCode));
            }
            if (checked(pageIndex * RecalculateBatchPageSize) >= total)
            {
                break;
            }
            pageIndex += 1;
        }
        return pairs;
    }

    /// <summary>
    /// 处理日期所在月的首日 00:00:00
    /// </summary>