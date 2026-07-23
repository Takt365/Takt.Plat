/**
 * Move VariableKey to last among business fields (before Items) in Purchase/Sales Price DTOs, forms, i18n lists.
 */
const fs = require('fs');
const path = require('path');
const root = path.resolve(__dirname, '..');

function read(rel) {
  return fs.readFileSync(path.join(root, rel), 'utf8');
}
function write(rel, c) {
  fs.writeFileSync(path.join(root, rel), c, 'utf8');
  console.log('updated', rel);
}

/** Reorder VariableKey after quotation/inquiry code block in each DTO class section. */
function reorderDtoVariableKey(content, codeProp /* PurchaseInquiryCode | SalesQuotationCode */) {
  const vkBlock =
    /    \/\/\/ <summary>\r?\n    \/\/\/ 可变关键字\r?\n    \/\/\/ <\/summary>\r?\n    public string\? VariableKey \{ get; set; \}(?: = string\.Empty)?;\r?\n(?:\r?\n)?/;
  if (!vkBlock.test(content)) {
    console.warn('no VariableKey block');
    return content;
  }
  // Remove all VariableKey blocks first
  let c = content.replace(
    /    \/\/\/ <summary>\r?\n    \/\/\/ 可变关键字\r?\n    \/\/\/ <\/summary>\r?\n    public string\? VariableKey \{ get; set; \}(?: = string\.Empty)?;\r?\n(?:\r?\n)?/g,
    ''
  );
  const insert = `    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; } = string.Empty;

`;
  const insertBare = `    /// <summary>
    /// 可变关键字
    /// </summary>
    public string? VariableKey { get; set; }

`;
  // After *Code property that is inquiry/quotation (not SalesPriceCode etc.)
  const re = new RegExp(
    `(public string\\? ${codeProp} \\{ get; set; \\}(?: = string\\.Empty)?;\\r?\\n)`,
    'g'
  );
  c = c.replace(re, (m, prop, offset, full) => {
    // Query may use = string.Empty; Template may not
    const useBare = !m.includes('= string.Empty');
    return m + '\n' + (useBare ? insertBare : insert);
  });
  // Also handle non-nullable string? already covered. Export might have = string.Empty.
  // Fix doubles
  c = c.replace(/\r?\n\r?\n\r?\n+/g, '\n\n');
  return c;
}

write(
  'backend/src/Takt.Application/Dtos/Logistics/Procurement/TaktPurchasePriceDtos.cs',
  reorderDtoVariableKey(
    read('backend/src/Takt.Application/Dtos/Logistics/Procurement/TaktPurchasePriceDtos.cs'),
    'PurchaseInquiryCode'
  )
);
write(
  'backend/src/Takt.Application/Dtos/Logistics/Sales/TaktSalesPriceDtos.cs',
  reorderDtoVariableKey(
    read('backend/src/Takt.Application/Dtos/Logistics/Sales/TaktSalesPriceDtos.cs'),
    'SalesQuotationCode'
  )
);

// Query DTO has date ranges between ValidTo and VariableKey historically - for Query, VariableKey after TaxCode filters is ok,
// but user said always last among business fields. For QueryDto the "last" before CreatedAt is fine after inquiry code.

function moveListFieldLast(arrSrc, field) {
  const parts = arrSrc
    .replace(/'/g, '')
    .split(',')
    .map((s) => s.trim())
    .filter(Boolean);
  const filtered = parts.filter((p) => p !== field);
  // place before purchaseInquiry/salesQuotation? User wants variableKey last among business — before extField/remark in forms,
  // in LIST before end (after inquiry codes).
  const out = [...filtered, field];
  return out.map((p) => `'${p}'`).join(',\n  ');
}

function reorderI18nList(rel, listConst) {
  let c = read(rel);
  const m = c.match(
    new RegExp(`export const ${listConst} = \\[([\\s\\S]*?)\\] as const`)
  );
  if (!m) {
    console.warn('no list', listConst);
    return;
  }
  let fields = m[1]
    .split(',')
    .map((s) => s.trim().replace(/['\n\r]/g, ''))
    .filter(Boolean);
  fields = fields.filter((f) => f !== 'variableKey');
  // last business before nothing — after inquiry/quotation codes
  fields.push('variableKey');
  const body = fields.map((f) => `  '${f}'`).join(',\n');
  c = c.replace(m[0], `export const ${listConst} = [\n${body},\n] as const`);
  write(rel, c);
}

reorderI18nList(
  'frontend/src/views/logistics/procurement/purchase-price/composables/use-purchase-price-i18n.ts',
  'PURCHASEPRICE_LIST_FIELDS'
);
reorderI18nList(
  'frontend/src/views/logistics/sales/price/composables/use-price-i18n.ts',
  'SALESPRICE_LIST_FIELDS'
);

// formFields string arrays
function reorderFormFields(rel) {
  let c = read(rel);
  c = c.replace(/const formFields = \[([^\]]+)\]/, (all, inner) => {
    let fields = inner
      .split(',')
      .map((s) => s.trim().replace(/"/g, ''))
      .filter(Boolean);
    fields = fields.filter((f) => f !== 'variableKey');
    // insert before extField if present, else before remark, else end
    const ext = fields.indexOf('extField');
    if (ext >= 0) fields.splice(ext, 0, 'variableKey');
    else {
      const rem = fields.indexOf('remark');
      if (rem >= 0) fields.splice(rem, 0, 'variableKey');
      else fields.push('variableKey');
    }
    return `const formFields = [${fields.map((f) => `"${f}"`).join(',')}]`;
  });
  write(rel, c);
}
reorderFormFields(
  'frontend/src/views/logistics/procurement/purchase-price/components/purchase-price-form.vue'
);
reorderFormFields('frontend/src/views/logistics/sales/price/components/price-form.vue');

console.log('done');
