    /// <summary>
    /// 从查询条件解析批量重算需逐月处理的日历月序列（须已指定起止处理月份）
    /// </summary>
    /// <param name="queryDto">已规范月份的查询 DTO</param>
    /// <returns>按时间升序的 (年, 月) 列表</returns>
    private static List<(int Year, int Month)> BuildRecalculateMonthSequenceFromQuery(TaktBomMaterialCostQueryDto queryDto)
    {
        if (!queryDto.ProcessedDateStart.HasValue || !queryDto.ProcessedDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择处理月份范围后再重算");
        }
        var rangeStartMonth = ToProcessedMonthStart(queryDto.ProcessedDateStart.Value);
        var rangeEndMonth = ToProcessedMonthStart(queryDto.ProcessedDateEnd.Value);
        if (rangeEndMonth < rangeStartMonth)
        {
            throw new TaktBusinessException("处理月份结束不能早于开始");
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