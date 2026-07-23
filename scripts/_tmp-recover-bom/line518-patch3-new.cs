        if (!string.IsNullOrEmpty(queryDto?.ProductDescription))
        {
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(queryDto.ProductDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelName))
        {
            exp = exp.And(x => x.ModelName != null && x.ModelName.Contains(queryDto.ModelName));
        }

        if (!string.IsNullOrEmpty(queryDto?.BomLevel))