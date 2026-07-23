/**
 * Reorder triad to: MinOrderQuantity → RoundingValue → PlannedDeliveryTimeDays
 * (match SourceOfSupply / MaterialPlant)
 */
const fs = require('fs');
const path = require('path');
const root = path.resolve(__dirname, '..');

const files = [
  'backend/src/Takt.Application/Dtos/Logistics/Procurement/TaktPurchasePriceItemDtos.cs',
  'backend/src/Takt.Application/Dtos/Logistics/Sales/TaktSalesPriceItemDtos.cs',
  'frontend/src/types/logistics/procurement/purchase-price-item.d.ts',
  'frontend/src/types/logistics/sales/price-item.d.ts',
  'frontend/src/views/logistics/procurement/purchase-price/components/purchase-price-item-form.vue',
  'frontend/src/views/logistics/procurement/purchase-price-item/components/purchase-price-item-form.vue',
  'frontend/src/views/logistics/sales/price/components/price-item-form.vue',
  'frontend/src/views/logistics/sales/price-item/components/price-item-form.vue',
  'frontend/src/views/logistics/procurement/purchase-price/composables/use-purchase-price-item-i18n.ts',
  'frontend/src/views/logistics/sales/price/composables/use-price-item-i18n.ts',
  'frontend/src/views/logistics/sales/price-item/composables/use-price-item-i18n.ts',
  'frontend/src/views/logistics/procurement/purchase-price-item/composables/use-purchase-price-item-i18n.ts',
  'frontend/src/views/logistics/procurement/purchase-price/components/purchase-price-items-panel.vue',
  'frontend/src/views/logistics/sales/price/components/price-items-panel.vue',
];

const swaps = [
  // PascalCase property triad in DTOs (nullable and non-nullable variants)
  [
    `public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;`,
    `public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数；SAP BSTRF）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;`,
  ],
  [
    `public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }`,
    `public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数；SAP BSTRF）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }`,
  ],
  // Also handle DTO blocks that still have summary before PlannedDelivery
  [
    `/// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;`,
    `/// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数；SAP BSTRF）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;`,
  ],
  [
    `/// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }`,
    `/// <summary>
    /// 最小起订量（计量单位数量，整数；SAP MINBM）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数；SAP BSTRF）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数；SAP PLIFZ）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }`,
  ],
  // camelCase string sequences
  ['plannedDeliveryTimeDays","minOrderQuantity","roundingValue', 'minOrderQuantity","roundingValue","plannedDeliveryTimeDays'],
  ["plannedDeliveryTimeDays','minOrderQuantity','roundingValue", "minOrderQuantity','roundingValue','plannedDeliveryTimeDays"],
  ['| \'plannedDeliveryTimeDays\' | \'minOrderQuantity\' | \'roundingValue\'', "| 'minOrderQuantity' | 'roundingValue' | 'plannedDeliveryTimeDays'"],
  ['plannedDeliveryTimeDays", "minOrderQuantity", "roundingValue', 'minOrderQuantity", "roundingValue", "plannedDeliveryTimeDays'],
];

for (const rel of files) {
  const full = path.join(root, rel);
  if (!fs.existsSync(full)) {
    console.log('skip missing', rel);
    continue;
  }
  let c = fs.readFileSync(full, 'utf8');
  const before = c;
  for (const [from, to] of swaps) {
    if (c.includes(from)) c = c.split(from).join(to);
  }
  // TS interface field blocks (planned → min → rounding)
  c = c.replace(
    /(\/\*\*[\s\S]*?\*\/\s*\n\s*plannedDeliveryTimeDays\??: number;\s*\n\s*)(\/\*\*[\s\S]*?\*\/\s*\n\s*minOrderQuantity\??: number;\s*\n\s*)(\/\*\*[\s\S]*?\*\/\s*\n\s*roundingValue\??: number;)/g,
    (m, a, b, d) => {
      // rebuild with min, rounding, planned - keep comments from each
      const plannedComment = (a.match(/\/\*\*[\s\S]*?\*\//) || [''])[0];
      const minComment = (b.match(/\/\*\*[\s\S]*?\*\//) || [''])[0];
      const roundComment = (d.match(/\/\*\*[\s\S]*?\*\//) || [''])[0];
      const opt = a.includes('plannedDeliveryTimeDays?:') || b.includes('minOrderQuantity?:') ? '?' : '';
      return `${minComment}\n  minOrderQuantity${opt}: number;\n\n  ${roundComment}\n  roundingValue${opt}: number;\n\n  ${plannedComment}\n  plannedDeliveryTimeDays${opt}: number;`;
    }
  );
  if (c !== before) {
    fs.writeFileSync(full, c, 'utf8');
    console.log('updated', rel);
  } else {
    console.log('no change', rel);
  }
}

/** Reorder Vue form a-col blocks by name= */
function reorderFormCols(rel, names) {
  const full = path.join(root, rel);
  if (!fs.existsSync(full)) return;
  let c = fs.readFileSync(full, 'utf8');
  // Find contiguous region containing these three name= fields
  const idxs = names.map((n) => {
    const i = c.indexOf(`name="${n}"`);
    return i;
  });
  if (idxs.some((i) => i < 0)) {
    console.log('form fields missing', rel);
    return;
  }
  // Expand each to enclosing a-col
  function colAround(pos) {
    const start = c.lastIndexOf('<a-col', pos);
    const end = c.indexOf('</a-col>', pos) + '</a-col>'.length;
    return { start, end, text: c.slice(start, end) };
  }
  const cols = names.map((n) => colAround(c.indexOf(`name="${n}"`)));
  // Sort by current start to get region
  const orderedByPos = [...cols].sort((a, b) => a.start - b.start);
  const regionStart = orderedByPos[0].start;
  const regionEnd = orderedByPos[orderedByPos.length - 1].end;
  // Emit in desired name order
  const byName = Object.fromEntries(
    names.map((n) => [n, colAround(c.indexOf(`name="${n}"`)).text])
  );
  // Keep interstitial between cols? If they are adjacent with newlines only, just join.
  const newRegion = names.map((n) => byName[n]).join('\n');
  const next = c.slice(0, regionStart) + newRegion + c.slice(regionEnd);
  if (next !== c) {
    fs.writeFileSync(full, next, 'utf8');
    console.log('reordered form', rel);
  }
}

const formFiles = [
  'frontend/src/views/logistics/procurement/purchase-price/components/purchase-price-item-form.vue',
  'frontend/src/views/logistics/procurement/purchase-price-item/components/purchase-price-item-form.vue',
  'frontend/src/views/logistics/sales/price/components/price-item-form.vue',
  'frontend/src/views/logistics/sales/price-item/components/price-item-form.vue',
];
for (const f of formFiles) {
  reorderFormCols(f, ['minOrderQuantity', 'roundingValue', 'plannedDeliveryTimeDays']);
}

console.log('done');
