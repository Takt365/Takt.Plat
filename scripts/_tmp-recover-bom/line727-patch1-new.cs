    /// <summary>
    /// 按查询条件批量重算并回填机种月平均材料成本（须指定单个处理月份；工厂+机种+处理月份维度）
    /// </summary>
    /// <param name="queryDto">与列表相同的筛选条件（ProcessedDateStart/End 须为同一自然月；忽略分页）</param>
    /// <param name="forceRecalculate">为 true 时先清零再重算</param>
    /// <returns>重算统计</returns>
    public async Task<TaktBomMaterialCostRecalculateModelAverageResultDto> RecalculateBomMaterialCostModelMonthlyAverageAsync(
        TaktBomMaterialCostQueryDto queryDto,
        bool forceRecalculate = false)
    {
        var prepared = PrepareRecalculateModelAverageQuery(queryDto);
        return await RecalculateBomMaterialCostModelMonthlyAverageCoreAsync(prepared.Query, forceRecalculate);
    }

    /// <summary>
    /// 校验并规范化机种月平均重算查询（须单个处理月份）
    /// </summary>
    /// <param name="queryDto">原始查询</param>
    /// <returns>规范化查询与处理月份标签</returns>
    public static TaktBomMaterialCostRecalculatePreparedQueryDto PrepareRecalculateModelAverageQuery(
        TaktBomMaterialCostQueryDto queryDto)
    {
        queryDto ??= new TaktBomMaterialCostQueryDto();
        if (!queryDto.BomMaterialCostStatus.HasValue)
        {
            queryDto.BomMaterialCostStatus = 1;
        }
        var normalizedQuery = NormalizeQueryToProcessedMonthBoundaries(queryDto);
        if (!normalizedQuery.ProcessedDateStart.HasValue || !normalizedQuery.ProcessedDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择处理月份后再重算");
        }
        var monthSequence = BuildRecalculateMonthSequenceFromQuery(normalizedQuery);
        if (monthSequence.Count == 0)
        {
            throw new TaktBusinessException("请选择处理月份后再重算");
        }
        var (year, month) = monthSequence[0];
        return new TaktBomMaterialCostRecalculatePreparedQueryDto
        {
            Query = normalizedQuery,
            ProcessedMonth = $"{year:D4}-{month:D2}",
        };
    }

    /// <summary>
    /// 机种月平均材料成本重算核心逻辑
    /// </summary>
    /// <param name="normalizedQuery">已规范化的查询</param>
    /// <param name="forceRecalculate">是否强制重算</param>
    /// <returns>重算统计</returns>
    private async Task<TaktBomMaterialCostRecalculateModelAverageResultDto> RecalculateBomMaterialCostModelMonthlyAverageCoreAsync(
        TaktBomMaterialCostQueryDto normalizedQuery,
        bool forceRecalculate)
    {
        EnsureThreeLayerContext();
        var monthSequence = BuildRecalculateMonthSequenceFromQuery(normalizedQuery);
        if (monthSequence.Count == 0)
        {
            return new TaktBomMaterialCostRecalculateModelAverageResultDto();
        }