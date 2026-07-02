'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');

/** @type {[string, string][]} 顺序敏感：长匹配优先 */
const REPLACEMENTS = [
  ['TaktMaterialTransactionItem', 'TaktMaterialDocumentItem'],
  ['TaktMaterialTransactions', 'TaktMaterialDocuments'],
  ['TaktMaterialTransaction', 'TaktMaterialDocument'],
  ['GetMaterialTransactionItem', 'GetMaterialDocumentItem'],
  ['GetMaterialTransaction', 'GetMaterialDocument'],
  ['CreateMaterialTransactionItem', 'CreateMaterialDocumentItem'],
  ['CreateMaterialTransaction', 'CreateMaterialDocument'],
  ['UpdateMaterialTransactionItem', 'UpdateMaterialDocumentItem'],
  ['UpdateMaterialTransaction', 'UpdateMaterialDocument'],
  ['DeleteMaterialTransactionItem', 'DeleteMaterialDocumentItem'],
  ['DeleteMaterialTransaction', 'DeleteMaterialDocument'],
  ['ImportMaterialTransactionItem', 'ImportMaterialDocumentItem'],
  ['ImportMaterialTransaction', 'ImportMaterialDocument'],
  ['ExportMaterialTransactionItem', 'ExportMaterialDocumentItem'],
  ['ExportMaterialTransaction', 'ExportMaterialDocument'],
  ['StampMaterialTransactionItem', 'StampMaterialDocumentItem'],
  ['StampMaterialTransaction', 'StampMaterialDocument'],
  ['FillMaterialTransaction', 'FillMaterialDocument'],
  ['SaveMaterialTransaction', 'SaveMaterialDocument'],
  ['MaterialTransactionId', 'MaterialDocumentId'],
  ['_materialTransactionItem', '_materialDocumentItem'],
  ['_materialTransaction', '_materialDocument'],
  ['materialTransactionItem', 'materialDocumentItem'],
  ['materialTransaction', 'materialDocument'],
  ['material_transaction_item', 'material_document_item'],
  ['material_transaction', 'material_document'],
  ['material-transaction-item', 'material-document-item'],
  ['material-transaction', 'material-document'],
  ['materialtransactionitem', 'materialdocumentitem'],
  ['materialtransaction', 'materialdocument'],
  ['menu.logistics.materials.material.transaction', 'menu.logistics.materials.material.document'],
  ['logistics:materials:material:transaction:item', 'logistics:materials:material:document:item'],
  ['logistics:materials:material:transaction', 'logistics:materials:material:document'],
  ['ix_material_transaction_item', 'ix_material_document_item'],
  ['ix_material_transaction', 'ix_material_document'],
  ['use-material-transaction-master-context', 'use-material-document-master-context'],
];

const TEXT_EXTENSIONS = new Set([
  '.cs', '.ts', '.vue', '.d.ts', '.cjs', '.mdc', '.txt', '.json',
]);

const SKIP_DIRS = new Set([
  'node_modules', '.git', 'bin', 'obj', 'dist', '.cursor',
]);

function walk(dir, files = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (SKIP_DIRS.has(entry.name)) continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      walk(full, files);
    } else {
      const ext = path.extname(entry.name);
      if (TEXT_EXTENSIONS.has(ext) || entry.name.endsWith('.d.ts')) {
        files.push(full);
      }
    }
  }
  return files;
}

function applyReplacements(content) {
  let result = content;
  for (const [from, to] of REPLACEMENTS) {
    result = result.split(from).join(to);
  }
  return result;
}

function renamePath(oldPath) {
  let newPath = oldPath;
  for (const [from, to] of REPLACEMENTS) {
    newPath = newPath.split(from).join(to);
  }
  return newPath === oldPath ? null : newPath;
}

function main() {
  const files = walk(ROOT);
  let updated = 0;
  for (const file of files) {
    if (file.includes('rename-material-transaction-to-document.cjs')) continue;
    const raw = fs.readFileSync(file, 'utf8');
    const next = applyReplacements(raw);
    if (next !== raw) {
      fs.writeFileSync(file, next, 'utf8');
      updated += 1;
    }
  }
  console.log(`Content updated in ${updated} files`);

  const allPaths = walk(ROOT).concat(
    walk(path.join(ROOT, 'frontend', 'src', 'views', 'logistics', 'materials')).filter(() => false),
  );
  const dirsToRename = [];
  const filesToRename = [];

  function collectRenames(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      if (SKIP_DIRS.has(entry.name)) continue;
      const full = path.join(dir, entry.name);
      const renamed = renamePath(full);
      if (entry.isDirectory()) {
        collectRenames(full);
        if (renamed) dirsToRename.push({ from: full, to: renamed });
      } else if (renamed) {
        filesToRename.push({ from: full, to: renamed });
      }
    }
  }
  collectRenames(ROOT);

  dirsToRename.sort((a, b) => b.from.length - a.from.length);
  filesToRename.sort((a, b) => b.from.length - a.from.length);

  for (const { from, to } of filesToRename) {
    if (!fs.existsSync(from)) continue;
    fs.mkdirSync(path.dirname(to), { recursive: true });
    fs.renameSync(from, to);
    console.log(`Renamed file: ${path.relative(ROOT, from)} -> ${path.relative(ROOT, to)}`);
  }
  for (const { from, to } of dirsToRename) {
    if (!fs.existsSync(from)) continue;
    fs.mkdirSync(path.dirname(to), { recursive: true });
    fs.renameSync(from, to);
    console.log(`Renamed dir: ${path.relative(ROOT, from)} -> ${path.relative(ROOT, to)}`);
  }
}

main();
