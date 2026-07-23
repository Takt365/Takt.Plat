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
        queryDto ??= new TaktBomMaterialCostQueryDto();
        EnsureThreeLayerContext();
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
            return new TaktBomMaterialCostRecalculateModelAverageResultDto();
        }