            fileName ?? "BOM成本差异分析.xlsx");
    }

    /// <summary>
    /// 导出 BOM 物料成本月度涨跌分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "productCode", "productDescription", "period", "totalCost",
            "basePeriod", "baseTotalCost", "varianceAmount", "variancePercent", "trend",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "产品编码", "产品描述", "年月", "材料总成本",
            "对比基准月", "基准月成本", "环比差额", "环比%", "涨跌",
        };
        var exportRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = result.PlantCode,
            ["productCode"] = result.ProductCode,
            ["productDescription"] = result.ProductDescription,
            ["period"] = line.Period,
            ["totalCost"] = line.TotalCost,
            ["basePeriod"] = line.BasePeriod,
            ["baseTotalCost"] = line.BaseTotalCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = line.VariancePercent,
            ["trend"] = line.Trend,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "BOM材料月度涨跌",
            fileName ?? $"BOM材料月度涨跌_{result.ProductCode}.xlsx");
    }

    /// <summary>
    /// 加载转置/差异分析用成本行（租户+公司范围内）
    /// </summary>