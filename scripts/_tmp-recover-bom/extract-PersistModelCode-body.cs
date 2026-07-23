        return result;
    }

    /// <summary>
    /// 构建按月机种回填用的查询范围（保留工厂/产品等条件，忽略机种与分页；单 processing 月）
    /// </summary>
    /// <param name="queryDto">已规范月份的查询 DTO</param>
    /// <param name="periodStart">该月起始</param>
    /// <param name="periodEnd">该月结束</param>
    /// <returns>单月查询 DTO</returns>
    private static TaktBomMaterialCostQueryDto BuildMonthScopeQueryForModelPatch(
        TaktBomMaterialCostQueryDto queryDto,
        DateTime periodStart,
        DateTime periodEnd)
    {
        var scope = queryDto.Adapt<TaktBomMaterialCostQueryDto>();
        scope.ProcessedDateStart = periodStart;
        scope.ProcessedDateEnd = periodEnd;
        scope.ModelCode = null;
        scope.PageIndex = 1;
        scope.PageSize = 1;
        return scope;
    }

    /// <summary>
    /// 按查询范围逐月从 TaktModelDestination 回填 BOM 行 ModelCode（初次/批量重算前）
    /// </summary>
    /// <param name="normalizedQuery">规范月份的查询 DTO</param>
    /// <param name="monthSequence">处理月份序列</param>
    /// <returns>实际写回机种编码的行数</returns>
    private async Task<int> PatchModelCodeFromModelDestinationForMonthsAsync(
        TaktBomMaterialCostQueryDto normalizedQuery,
        IReadOnlyList<(int Year, int Month)> monthSequence)
    {
        if (monthSequence.Count == 0)
        {
            return 0;
        }
        var lookup = await BuildProductModelLookupAsync();
        var patchedRowCount = 0;
        var scannedRowCount = 0;
        foreach (var (year, month) in monthSequence)
        {
            var periodStart = new DateTime(year, month, 1);
            var periodEnd = ToProcessedMonthEnd(periodStart);
            var monthScopeQuery = BuildMonthScopeQueryForModelPatch(normalizedQuery, periodStart, periodEnd);
            var predicate = QueryExpression(monthScopeQuery);
            var monthRows = await _bomMaterialCostRepository.GetListForExportAsync(predicate, MaxRecalculateScanRows);
            scannedRowCount += monthRows.Count;
            if (scannedRowCount > MaxRecalculateScanRows)
            {
                throw new TaktBusinessException(
                    $"机种回填扫描行数为 {scannedRowCount}，超过单次上限 {MaxRecalculateScanRows}，请缩小查询范围。");
            }
            patchedRowCount += await PersistModelCodeFromModelDestinationAsync(monthRows, lookup);
        }
        return patchedRowCount;
    }

    /// <summary>
    /// 将型号目的地解析出的机种编码持久化到 BOM 行（有变更才更新）
    /// </summary>
    /// <param name="rows">待回填 BOM 行</param>
    /// <param name="lookup">MaterialCode→ModelCode 查找表</param>
    /// <returns>写回行数</returns>
    private async Task<int> PersistModelCodeFromModelDestinationAsync(
        IReadOnlyList<TaktBomMaterialCost> rows,
        IReadOnlyDictionary<string, string> lookup)
    {
        var patched = 0;
        foreach (var row in rows)
        {
            var previous = row.ModelCode?.Trim() ?? string.Empty;
            ApplyModelMetadata(row, lookup);
            var current = row.ModelCode?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(current)
                || string.Equals(previous, current, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            await _bomMaterialCostRepository.UpdateAsync(row);
            patched += 1;
        }
        return patched;
    }

    /// <summary>
    /// 重算并写回机种月平均材料成本（同工厂+机种+处理月份全部 BOM 行；已计算机种月平均时默认跳过）
    /// </summary>