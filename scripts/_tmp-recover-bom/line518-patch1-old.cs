        else if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var materialCodes = await GetMaterialCodesByModelAsync(queryDto.ModelCode.Trim());
            if (materialCodes.Count == 0)
            {
                return new List<TaktBomMaterialCost>();
            }
            exp = exp.And(x => x.ProductCode != null && materialCodes.Contains(x.ProductCode));
        }