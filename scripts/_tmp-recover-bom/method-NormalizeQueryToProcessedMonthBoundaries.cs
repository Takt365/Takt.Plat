        entity.ModelCode = string.Empty;
    }

    /// <summary>
    /// 将查询条件中的处理日期区间规范为整月起止（用于按月重算/筛选）
    /// </summary>
    /// <param name="queryDto">原始查询 DTO</param>
    /// <returns>规范后的副本</returns>
    private static TaktBomMaterialCostQueryDto NormalizeQueryToProcessedMonthBoundaries(TaktBomMaterialCostQueryDto queryDto)
    {
        var normalized = queryDto.Adapt<TaktBomMaterialCostQueryDto>();
        if (normalized.ProcessedDateStart.HasValue)
        {
            normalized.ProcessedDateStart = ToProcessedMonthStart(normalized.ProcessedDateStart.Value);
        }
        if (normalized.ProcessedDateEnd.HasValue)
        {
            normalized.ProcessedDateEnd = ToProcessedMonthEnd(normalized.ProcessedDateEnd.Value);
        }
        return normalized;
    }

    /// <summary>
    /// 解析批量重算需逐月处理的日历月序列
    /// </summary>
    /// <param name="queryDto">已规范月份的查询 DTO</param>
    /// <param name="rows">扫描到的 BOM 行</param>
    /// <returns>按时间升序的 (年, 月) 列表</returns>
    private static List<(int Year, int Month)> BuildRecalculateMonthSequence(
        TaktBomMaterialCostQueryDto queryDto,
        IReadOnlyList<TaktBomMaterialCost> rows)
    {
        DateTime rangeStartMonth;
        DateTime rangeEndMonth;
        if (queryDto.ProcessedDateStart.HasValue && queryDto.ProcessedDateEnd.HasValue)
        {
            rangeStartMonth = ToProcessedMonthStart(queryDto.ProcessedDateStart.Value);
            rangeEndMonth = ToProcessedMonthStart(queryDto.ProcessedDateEnd.Value);
            if (rangeEndMonth < rangeStartMonth)
            {
                throw new TaktBusinessException("处理月份结束不能早于开始");
            }
        }
        else if (queryDto.ProcessedDateStart.HasValue)
        {
            rangeStartMonth = ToProcessedMonthStart(queryDto.ProcessedDateStart.Value);
            rangeEndMonth = rows.Count > 0
                ? ToProcessedMonthStart(rows.Max(r => r.ProcessedDate))
                : rangeStartMonth;
        }
        else if (queryDto.ProcessedDateEnd.HasValue)
        {
            rangeEndMonth = ToProcessedMonthStart(queryDto.ProcessedDateEnd.Value);
            rangeStartMonth = rows.Count > 0
                ? ToProcessedMonthStart(rows.Min(r => r.ProcessedDate))
                : rangeEndMonth;
        }
        else if (rows.Count > 0)
        {
            rangeStartMonth = ToProcessedMonthStart(rows.Min(r => r.ProcessedDate));
            rangeEndMonth = ToProcessedMonthStart(rows.Max(r => r.ProcessedDate));
        }
        else
        {
            return new List<(int Year, int Month)>();
        }
        var months = EnumerateProcessedMonthsInclusive(rangeStartMonth, rangeEndMonth);
        if (months.Count > MaxRecalculateMonthSpan)
        {
            throw new TaktBusinessException(
                $"处理月份跨度为 {months.Count} 个月，超过单次上限 {MaxRecalculateMonthSpan}，请缩小月份范围");
        }
        return months;
    }

    /// <summary>
    /// 构建批量重算维度组：工厂+机种 × 月份序列（逐月独立重算）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="rows">扫描到的 BOM 行</param>
    /// <param name="monthSequence">处理月份序列</param>
    /// <returns>维度组集合</returns>
    private static HashSet<(string PlantCode, string ModelCode, int Year, int Month)> BuildRecalculateRefreshGroups(
        TaktBomMaterialCostQueryDto queryDto,
        IReadOnlyList<TaktBomMaterialCost> rows,
        IReadOnlyList<(int Year, int Month)> monthSequence)
    {
        var plantModelPairs = new HashSet<(string PlantCode, string ModelCode)>();
        foreach (var row in rows)
        {
            var plantCode = row.PlantCode?.Trim() ?? string.Empty;
            var modelCode = row.ModelCode?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(modelCode))
            {
                continue;
            }
            plantModelPairs.Add((plantCode, modelCode));
        }
        var plantFilter = queryDto.PlantCode?.Trim() ?? string.Empty;
        var modelFilter = queryDto.ModelCode?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(plantFilter) && !string.IsNullOrWhiteSpace(modelFilter))
        {
            plantModelPairs.Add((plantFilter, modelFilter));
        }
        var refreshGroups = new HashSet<(string PlantCode, string ModelCode, int Year, int Month)>();
        foreach (var (plantCode, modelCode) in plantModelPairs)
        {
            foreach (var (year, month) in monthSequence)
            {
                refreshGroups.Add((plantCode, modelCode, year, month));
            }
        }
        return refreshGroups;
    }

    /// <summary>
    /// 处理日期所在月的首日 00:00:00
    /// </summary>
    /// <param name="value">处理日期</param>
    /// <returns>月初</returns>
    private static DateTime ToProcessedMonthStart(DateTime value) =>
        new DateTime(value.Year, value.Month, 1);

    /// <summary>
    /// 处理日期所在月的末日 23:59:59.999
    /// </summary>
    /// <param name="value">处理日期</param>
    /// <returns>月末时刻</returns>
    private static DateTime ToProcessedMonthEnd(DateTime value)
    {
        var lastDay = DateTime.DaysInMonth(value.Year, value.Month);
        return new DateTime(value.Year, value.Month, lastDay, 23, 59, 59, 999);
    }

    /// <summary>
    /// 枚举起止月份之间的全部日历月（含首尾）
    /// </summary>
    /// <param name="startMonth">起始月（任意日，取所在月）</param>
    /// <param name="endMonth">结束月（任意日，取所在月）</param>
    /// <returns>(年, 月) 列表</returns>
    private static List<(int Year, int Month)> EnumerateProcessedMonthsInclusive(DateTime startMonth, DateTime endMonth)
    {
        var result = new List<(int Year, int Month)>();
        var cursor = ToProcessedMonthStart(startMonth);
        var end = ToProcessedMonthStart(endMonth);
        while (cursor <= end)
        {
            result.Add((cursor.Year, cursor.Month));
            cursor = cursor.AddMonths(1);
        }
        return result;
    }

    /// <summary>
    /// 重算并写回机种月平均材料成本（同工厂+机种+处理月份全部 BOM 行；已计算机种月平均时默认跳过）
    /// </summary>