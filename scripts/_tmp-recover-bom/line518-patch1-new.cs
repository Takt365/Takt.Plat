        else if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var modelCode = queryDto.ModelCode.Trim();
            var materialCodes = await GetMaterialCodesByModelAsync(modelCode);
            if (materialCodes.Count == 0)
            {
                return new List<TaktBomMaterialCost>();
            }
            exp = exp.And(x =>
                (x.ModelCode != null && x.ModelCode == modelCode)
                || ((x.ModelCode == null || x.ModelCode == string.Empty)
                    && x.ProductCode != null
                    && materialCodes.Contains(x.ProductCode)));
        }