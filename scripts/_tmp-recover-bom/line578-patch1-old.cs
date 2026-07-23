    /// <summary>
    /// 按查询条件批量重算并回填机种月平均材料成本
    /// </summary>
    /// <param name="queryDto">与列表相同的筛选条件（忽略分页）</param>
    /// <returns>重算统计</returns>
    Task<TaktBomMaterialCostRecalculateModelAverageResultDto> RecalculateBomMaterialCostModelMonthlyAverageAsync(TaktBomMaterialCostQueryDto queryDto);