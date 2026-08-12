'use strict';

/**
 * Deduplicate MaterialDescription property blocks that appeared after MaterialName rename
 * when files already had MaterialDescription (e.g. MaterialPlant DTOs / frontend).
 */
const fs = require('fs');
const path = require('path');

const files = [
  'backend/src/Takt.Application/Dtos/Logistics/Materials/TaktMaterialPlantDtos.cs',
  'frontend/src/types/logistics/materials/material-plant.d.ts',
];

/**
 * Remove consecutive duplicate property declarations named MaterialDescription / materialDescription.
 * Keeps the second occurrence when first is non-nullable string and second is string? (entity alignment),
 * otherwise keeps the first.
 * @param {string} content
 * @param {'cs' | 'ts'} kind
 */
function dedupeCsOrTsProps(content, kind) {
  if (kind === 'cs') {
    // Remove first of each adjacent pair of MaterialDescription property blocks within a class section
    // Pattern: summary+attr+property then later another MaterialDescription - remove the renamed former MaterialName
    // which is typically the one with ColumnDescription 物料描述 that was 物料名称 and Length 40 non-null
    // Simpler approach: within file, for each class, if two MaterialDescription props appear, remove the first block of each pair when they are consecutive field groups.
    const propRe =
      /(\/\*\*[\s\S]*?\*\/\s*)?(\/\/\/ <summary>[\s\S]*?\/\/\/ <\/summary>\s*)?(\[[^\]]+\]\s*)*public string\?? MaterialDescription \{ get; set; \}[^\r\n]*\r?\n/g;
    const matches = [...content.matchAll(propRe)];
    if (matches.length < 2) return content;
    // Remove odd-indexed duplicates that immediately follow a MaterialSpecification or MaterialCode block
    // Actually: pairs are (old MaterialName→Description, original Description). Keep nullable one (string?).
    let next = content;
    // Process from end to start
    const all = [...content.matchAll(propRe)];
    const toRemove = [];
    for (let i = 0; i < all.length - 1; i++) {
      const a = all[i];
      const b = all[i + 1];
      if (a.index == null || b.index == null) continue;
      // if close together (< 800 chars) treat as duplicate pair
      if (b.index - a.index < 800) {
        const aText = a[0];
        const bText = b[0];
        // prefer keeping string? MaterialDescription
        if (aText.includes('string?') && !bText.includes('string?')) {
          toRemove.push({ start: b.index, end: b.index + b[0].length });
        } else if (!aText.includes('string?') && bText.includes('string?')) {
          toRemove.push({ start: a.index, end: a.index + a[0].length });
        } else {
          // same nullability: remove first (renamed MaterialName)
          toRemove.push({ start: a.index, end: a.index + a[0].length });
        }
        i += 1; // skip pair
      }
    }
    toRemove.sort((x, y) => y.start - x.start);
    for (const r of toRemove) {
      next = next.slice(0, r.start) + next.slice(r.end);
    }
    return next;
  }

  // TS interfaces: remove duplicate materialDescription fields that are adjacent-ish
  const tsRe =
    /(\/\*\*[\s\S]*?\*\/\s*)?materialDescription\??: string;?\r?\n/g;
  const all = [...content.matchAll(tsRe)];
  let next = content;
  const toRemove = [];
  for (let i = 0; i < all.length - 1; i++) {
    const a = all[i];
    const b = all[i + 1];
    if (a.index == null || b.index == null) continue;
    if (b.index - a.index < 500) {
      // keep optional one if mixed
      const aOpt = a[0].includes('?:');
      const bOpt = b[0].includes('?:');
      if (!aOpt && bOpt) toRemove.push({ start: a.index, end: a.index + a[0].length });
      else if (aOpt && !bOpt) toRemove.push({ start: b.index, end: b.index + b[0].length });
      else toRemove.push({ start: a.index, end: a.index + a[0].length });
      i += 1;
    }
  }
  toRemove.sort((x, y) => y.start - x.start);
  for (const r of toRemove) {
    next = next.slice(0, r.start) + next.slice(r.end);
  }
  return next;
}

const root = path.resolve(__dirname, '..');
for (const rel of files) {
  const full = path.join(root, rel);
  const raw = fs.readFileSync(full, 'utf8');
  const kind = rel.endsWith('.cs') ? 'cs' : 'ts';
  const next = dedupeCsOrTsProps(raw, kind);
  fs.writeFileSync(full, next, 'utf8');
  const before = (raw.match(/MaterialDescription|materialDescription/g) || []).length;
  const after = (next.match(/MaterialDescription|materialDescription/g) || []).length;
  console.log(rel, { before, after, changed: raw !== next });
}
