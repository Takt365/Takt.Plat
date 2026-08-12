/**
 * 可靠重写 Quartz sync_*.sql 的 culture_code 写入：
 * - 不再注入到 WHERE/ON 等不相关位置
 * - 只在 MERGE 的 UPDATE SET / INSERT columns-values 中写入 culture_code
 * - 修复之前错误注入导致的语法：T.[tenant_code] = @culture_code,@tenant_code 等
 *
 * 入口：node scripts/rewrite-quartz-sync-culture-code-direct.cjs
 */

const fs = require("fs");
const path = require("path");

function readText(p) {
  return fs.readFileSync(p, "utf8");
}

function writeText(p, content) {
  fs.writeFileSync(p, content, "utf8");
}

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

function normalizeNewlines(s) {
  return s.replace(/\r\n/g, "\n");
}

function buildEntityTableCultureMap(domainEntitiesDir) {
  /** @type {Record<string, {hasCultureCode: boolean}>} */
  const map = {};
  const csFiles = walkDir(domainEntitiesDir).filter((p) => p.toLowerCase().endsWith(".cs"));
  const sugarTableRegex = /\[SugarTable\("([^"]+)"\s*,/g;
  for (const file of csFiles) {
    const content = readText(file);
    const filesHasCulture = content.includes("public string CultureCode");
    let match;
    while ((match = sugarTableRegex.exec(content))) {
      const tableName = match[1];
      const start = match.index + match[0].length;
      const after = content.slice(start, start + 4000);
      const classMatch = after.match(/public class\s+\w+\s*:\s*([^{\n\r]+)/);
      const baseList = classMatch?.[1] ?? "";
      const hasCompanyCode =
        baseList.includes("TaktCompanyEntityBase") || baseList.includes("TaktApprovalEntityBase");
      const hasCultureCode = hasCompanyCode || filesHasCulture;
      map[tableName] = { hasCultureCode };
    }
  }
  return map;
}

function fixTenantCodeCorruption(sql) {
  // 只修：T.[tenant_code] = @culture_code,@tenant_code
  sql = sql.replace(
    /T\.\[tenant_code\]\s*=\s*@culture_code\s*,\s*@tenant_code/gi,
    "T.[tenant_code] = @tenant_code"
  );
  // 只修：T.[tenant_code] = @culture_code,S.[tenant_code]
  sql = sql.replace(
    /T\.\[tenant_code\]\s*=\s*@culture_code\s*,\s*S\.\[tenant_code\]/gi,
    "T.[tenant_code] = S.[tenant_code]"
  );
  // 只修：T.[tenant_code]=@culture_code,@tenant_code（无空格版本）
  sql = sql.replace(
    /T\.\[tenant_code\]\s*=\s*@culture_code\s*,\s*@tenant_code/gi,
    "T.[tenant_code] = @tenant_code"
  );
  return sql;
}

function ensureCultureCodeInMergeSegment(seg, tableName) {
  let s = seg;

  // 1) UPDATE SET：如果这一段 MERGE 没有 T.[culture_code]，就在第一个 T.[updated_by] 之前插入
  if (!s.includes(`T.[culture_code]`)) {
    s = s.replace(
      /^([ \t]*)T\.\[updated_by\]\s*=/m,
      (full, indent) => `${indent}T.[culture_code] = @culture_code,\n${indent}T.[updated_by] =`
    );
  }

  // 2) INSERT columns-values：只在 insert 子句里补 [culture_code] / @culture_code
  //    找到 WHEN NOT MATCHED THEN -> INSERT (...) VALUES (...)
  const marker = "WHEN NOT MATCHED THEN";
  const idxMarker = s.indexOf(marker);
  if (idxMarker > -1) {
    const idxInsert = s.indexOf("INSERT", idxMarker);
    const idxValues = s.indexOf("VALUES", idxInsert);
    if (idxInsert > -1 && idxValues > -1) {
      // columns：从第一个 '(' 到匹配的 ')'（用简单策略：取到紧接着的 ')\n  VALUES' 前一段）
      const colOpen = s.indexOf("(", idxInsert);
      const colClose = s.lastIndexOf(")", idxValues);
      if (colOpen > -1 && colClose > colOpen) {
        const columns = s.slice(colOpen + 1, colClose);
        if (!columns.includes("[culture_code]")) {
          // 在 [tenant_code] 前插入 [culture_code],
          const idxTenantCol = columns.indexOf("[tenant_code]");
          if (idxTenantCol > -1) {
            const before = columns.slice(0, idxTenantCol);
            const after = columns.slice(idxTenantCol);
            // 尽量保持缩进：用 tenant_code 前一个换行的缩进
            const lastNewline = before.lastIndexOf("\n");
            const indent = lastNewline >= 0 ? before.slice(lastNewline + 1).match(/^[ \t]*/)?.[0] ?? "" : "";
            const inserted = `${indent}[culture_code],`;
            const newColumns = `${before}${inserted}\n${indent}${after}`;
            s = s.slice(0, colOpen + 1) + newColumns + s.slice(colClose);
          }
        }
      }

      // values：从 VALUES '(' 到对应的 ')'（简单：取到段尾最近的 ')'）
      const valOpen = s.indexOf("(", idxValues);
      const valClose = s.indexOf(")", valOpen + 1);
      // 这里不保证严格配对，但足以覆盖典型格式
      if (valOpen > -1 && valClose > valOpen) {
        const values = s.slice(valOpen + 1, valClose);
        if (!values.includes("@culture_code")) {
          // 在 S.[tenant_code] 或 @tenant_code 前插入 @culture_code
          const idxS = values.indexOf("S.[tenant_code]");
          const idxA = values.indexOf("@tenant_code");
          if (idxS > -1) {
            values.replace("S.[tenant_code]", "@culture_code,S.[tenant_code]");
          }
          if (idxS > -1) {
            const before = values.slice(0, idxS);
            const after = values.slice(idxS);
            const newValues = `${before}@culture_code,${after}`;
            s = s.slice(0, valOpen + 1) + newValues + s.slice(valClose);
          } else if (idxA > -1) {
            const before = values.slice(0, idxA);
            const after = values.slice(idxA);
            const newValues = `${before}@culture_code,${after}`;
            s = s.slice(0, valOpen + 1) + newValues + s.slice(valClose);
          }
        }
      }
    }
  }

  return s;
}

function main() {
  const workspaceRoot = path.resolve(__dirname, "..");
  const quartzDir = path.join(workspaceRoot, "backend", "src", "Takt.WebApi", "wwwroot", "Quartz");
  const domainEntitiesDir = path.join(workspaceRoot, "backend", "src", "Takt.Domain", "Entities");

  const map = buildEntityTableCultureMap(domainEntitiesDir);
  const sqlFiles = walkDir(quartzDir).filter((p) => path.basename(p).startsWith("sync_") && p.endsWith(".sql"));

  const changed = [];

  for (const file of sqlFiles) {
    let sql = normalizeNewlines(readText(file));
    const before = sql;

    // 仅做“语法修复”：回滚错误注入到 WHERE/ON 的 tenant_code 表达式。
    // 不再二次改 MERGE，以免再次引入列清单错位。
    sql = fixTenantCodeCorruption(sql);

    if (sql !== before) {
      writeText(file, sql.replace(/\n/g, "\r\n"));
      changed.push(path.relative(workspaceRoot, file));
    }
  }

  console.log(`Quartz sync culture_code 重写完成。变更文件数：${changed.length}`);
  for (const f of changed) console.log(`- ${f}`);
}

main();

