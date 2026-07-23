    /// <summary>
    /// 按查询条件批量重算并回填机种月平均材料成本（同工厂+机种+处理月份算术平均）
    /// </summary>
    /// <param name="queryDto">与列表相同的筛选条件（忽略分页）</param>
    /// <returns>重算统计</returns>
    public async Task<TaktBomMaterialCostRecalculateModelAverageResultDto> RecalculateBomMaterialCostModelMonthlyAverageAsync(TaktBomMaterialCostQueryDto queryDto)