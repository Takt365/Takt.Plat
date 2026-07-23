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