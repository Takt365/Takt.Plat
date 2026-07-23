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