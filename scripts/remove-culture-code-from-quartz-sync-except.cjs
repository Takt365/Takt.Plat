/**
 * 对 Quartz sync_*.sql 做“反向清理”：
 * - 只保留指定的 5 个文件：sync_matplt.sql / sync_mdl.sql / sync_ec.sql / sync_mo.sql / sync_st.sql
 * - 其它 sync_*.sql 移除：
 *   - DECLARE @culture_code ...
 *   - MERGE UPDATE SET / INSERT columns-values 中的 [culture_code] 与 @culture_code
 *
 * 不使用 git revert/restore/checkout，仅直接改工作区文件。
 *
 * 入口：
 * node scripts/remove-culture-code-from-quartz-sync-except.cjs
 */

const fs = require("fs");
const path = require("path");

const KEEP_FILES = new Set([
  "sync_matplt.sql",
  "sync_mdl.sql",
  "sync_ec.sql",
  "sync_mo.sql",
  "sync_st.sql",
]);

function walkDir(dir) {
  /** @type {string[]} */
  const out = [];
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, entry.name);
    if (entry.isDirectory()) out.push(...walkDir(p));
    else out.push(p);
  }
  return out;
}

function readText(p) {
  return fs.readFileSync(p, "utf8");
}

function writeText(p, content) {
  fs.writeFileSync(p, content, "utf8");
}

function normalizeNewlines(s) {
  return s.replace(/\r\n/g, "\n");
}

function cleanupCultureCodeTokens(sql) {
  let s = sql;

  // 1) 删除 culture_code 变量声明（整行）
  s = s.replace(/^\s*DECLARE\s+@culture_code[^\n]*\n/mg, "");

  // 2) 删除 UPDATE SET 中的 T.[culture_code] = @culture_code（行独立 / 行内拼接都覆盖）
  s = s.replace(/T\.\[culture_code\]\s*=\s*@culture_code\s*,?\s*\n/gm, "");
  s = s.replace(/T\.\[culture_code\]\s*=\s*@culture_code\s*,\s*/g, "");
  s = s.replace(/T\.\[culture_code\]\s*=\s*@culture_code\s*\n/gm, "");

  // 3) 删除 INSERT 列清单里的 [culture_code] token（包含逗号前后两种）
  //    先处理“前面是逗号”的情况：...,[culture_code],...
  s = s.replace(/,\s*\[culture_code\]\s*,/g, ",");
  s = s.replace(/,\s*\[culture_code\]\s*\)/g, ")");
  s = s.replace(/,\s*\[culture_code\]\s*/g, "");
  //    再处理 “后面是逗号”的情况：...[culture_code],...
  s = s.replace(/\[culture_code\]\s*,\s*/g, "");

  // 4) 删除 VALUES 列表里的 @culture_code token
  s = s.replace(/,\s*@culture_code\s*,/g, ",");
  s = s.replace(/,\s*@culture_code\s*\)/g, ")");
  s = s.replace(/,\s*@culture_code\s*/g, "");
  s = s.replace(/@culture_code\s*,\s*/g, "");

  // 5) 清理可能残留的多余逗号（只做局部常见模式）
  s = s.replace(/\(\s*,/g, "(");
  s = s.replace(/,\s*,/g, ",");
  s = s.replace(/,\s*\)/g, ")");

  return s;
}

function main() {
  const workspaceRoot = path.resolve(__dirname, "..");
  const quartzDir = path.join(workspaceRoot, "backend", "src", "Takt.WebApi", "wwwroot", "Quartz");
  const sqlFiles = walkDir(quartzDir).filter((p) => {
    const name = path.basename(p).toLowerCase();
    return name.startsWith("sync_") && name.endsWith(".sql");
  });

  let changed = 0;

  for (const file of sqlFiles) {
    const base = path.basename(file).toLowerCase();
    if (KEEP_FILES.has(base)) continue;

    const before = readText(file);
    const normalized = normalizeNewlines(before);
    const after = cleanupCultureCodeTokens(normalized);

    if (after !== normalized) {
      writeText(file, after.replace(/\n/g, "\r\n"));
      changed++;
    }
  }

  console.log(`Quartz sync_*.sql culture_code 反向清理完成。变更文件数：${changed}`);
}

main();

