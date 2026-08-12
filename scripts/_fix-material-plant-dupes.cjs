'use strict';

const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');

/** @param {string} rel */
function fixFile(rel, replacer) {
  const full = path.join(root, rel);
  const raw = fs.readFileSync(full, 'utf8');
  const next = replacer(raw);
  if (next !== raw) {
    fs.writeFileSync(full, next, 'utf8');
    console.log('fixed', rel);
  } else {
    console.log('unchanged', rel);
  }
}

const csDupBlock = [
  /    \/\*\*[\s\S]*?\*\/\r?\n    public string MaterialDescription \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\*\*[\s\S]*?\*\/\r?\n    public string\? MaterialSpecification \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\*\*[\s\S]*?\*\/\r?\n    public string\? MaterialDescription \{ get; set; \} = string\.Empty;\r?\n/g,
  /    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string MaterialDescription \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料规格\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialSpecification \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialDescription \{ get; set; \} = string\.Empty;\r?\n/g,
];

const csReplacement = `    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;
`;

// Query DTOs often use string? for former MaterialName too
const csDupNullableBoth = /    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialDescription \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料规格\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialSpecification \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialDescription \{ get; set; \} = string\.Empty;\r?\n/g;

fixFile('backend/src/Takt.Application/Dtos/Logistics/Materials/TaktMaterialPlantDtos.cs', (raw) => {
  let s = raw;
  s = s.replace(csDupBlock[1], csReplacement);
  s = s.replace(csDupNullableBoth, csReplacement);
  // Import/Export may use non-null MaterialDescription first
  s = s.replace(
    /    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string MaterialDescription \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料规格\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialSpecification \{ get; set; \} = string\.Empty;\r?\n\r?\n    \/\/\/ <summary>\r?\n    \/\/\/ 物料描述\r?\n    \/\/\/ <\/summary>\r?\n    public string\? MaterialDescription \{ get; set; \} = string\.Empty;\r?\n/g,
    csReplacement
  );
  return s;
});

const tsDup = /  \/\*\*\r?\n   \* 物料描述\r?\n   \*\/\r?\n  materialDescription: string;\r?\n\r?\n  \/\*\*\r?\n   \* 物料规格\r?\n   \*\/\r?\n  materialSpecification\?: string;\r?\n\r?\n  \/\*\*\r?\n   \* 物料描述\r?\n   \*\/\r?\n  materialDescription\?: string;\r?\n/g;

const tsDupOpt = /  \/\*\*\r?\n   \* 物料描述\r?\n   \*\/\r?\n  materialDescription\?: string;\r?\n\r?\n  \/\*\*\r?\n   \* 物料规格\r?\n   \*\/\r?\n  materialSpecification\?: string;\r?\n\r?\n  \/\*\*\r?\n   \* 物料描述\r?\n   \*\/\r?\n  materialDescription\?: string;\r?\n/g;

const tsReplacement = `  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;
`;

fixFile('frontend/src/types/logistics/materials/material-plant.d.ts', (raw) => {
  let s = raw.replace(tsDup, tsReplacement);
  s = s.replace(tsDupOpt, tsReplacement);
  return s;
});

// Service: collapse duplicate MaterialDescription keyword / query filters
fixFile('backend/src/Takt.Application/Services/Logistics/Materials/TaktMaterialPlantService.cs', (raw) => {
  let s = raw;
  s = s.replace(
    / \|\| \(x\.MaterialDescription != null && x\.MaterialDescription\.Contains\(keywords\)\)\r?\n                \|\| \(x\.MaterialDescription != null && x\.MaterialDescription\.Contains\(keywords\)\)/g,
    ' || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))'
  );
  s = s.replace(
    /        if \(!string\.IsNullOrEmpty\(queryDto\?\.MaterialDescription\)\)\r?\n        \{\r?\n            exp = exp\.And\(x => x\.MaterialDescription != null && x\.MaterialDescription\.Contains\(queryDto\.MaterialDescription\)\);\r?\n        \}\r?\n\r?\n        if \(!string\.IsNullOrEmpty\(queryDto\?\.MaterialDescription\)\)\r?\n        \{\r?\n            exp = exp\.And\(x => x\.MaterialDescription != null && x\.MaterialDescription\.Contains\(queryDto\.MaterialDescription\)\);\r?\n        \}/g,
    `        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }`
  );
  return s;
});

// Validators: former MaterialName required → optional MaterialDescription max 80
fixFile('backend/src/Takt.Application/Validators/Logistics/Materials/TaktMaterialPlantValidators.cs', (raw) => {
  return raw.replace(
    /RuleFor\(x => x\.MaterialDescription\)\r?\n            \.NotEmpty\(\)\.WithMessage\("物料描述不能为空"\)\r?\n            \.MaximumLength\(40\)\.WithMessage\("物料描述长度不能超过40个字符"\);/g,
    `RuleFor(x => x.MaterialDescription)
            .MaximumLength(80).WithMessage("物料描述长度不能超过80个字符");`
  );
});
