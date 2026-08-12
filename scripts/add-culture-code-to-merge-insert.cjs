/**
 * 给 Quartz sync_*.sql 的 MERGE INSERT 子句补写 culture_code。
 *
 * 仅在模式：
 *  WHEN NOT MATCHED THEN
 *    INSERT ( ...columns... )
 *    VALUES ( ...values... )
 *    OUTPUT ...
 *
 * 中修改：
 * - columns 若缺少 [culture_code]：在 [tenant_code] 前插入 [culture_code],
 * - values 若缺少 @culture_code：在 S.[tenant_code] / @tenant_code 前插入 @culture_code,
 *
 * 不处理 UPDATE SET（已有则保留），也不触及 WHERE/ON。
 *
 * 入口：
 * node scripts/add-culture-code-to-merge-insert.cjs
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

function main() {
  const workspaceRoot = path.resolve(__dirname, "..");
  const quartzDir = path.join(workspaceRoot, "backend", "src", "Takt.WebApi", "wwwroot", "Quartz");
  const sqlFiles = walkDir(quartzDir).filter((p) => path.basename(p).startsWith("sync_") && p.endsWith(".sql"));

  let changedFiles = 0;

  const mergeInsertRe = /WHEN NOT MATCHED THEN\s*INSERT\s*\(\s*([\s\S]*?)\s*\)\s*VALUES\s*\(\s*([\s\S]*?)\s*\)\s*OUTPUT/gi;

  for (const file of sqlFiles) {
    let sql = normalizeNewlines(readText(file));
    const before = sql;

    sql = sql.replace(mergeInsertRe, (full, cols, vals) => {
      let newCols = cols;
      let newVals = vals;

      const hasCultureCol = newCols.includes("[culture_code]");
      if (!hasCultureCol) {
        const idxTenant = newCols.indexOf("[tenant_code]");
        if (idxTenant > -1) {
          const beforeTenant = newCols.slice(0, idxTenant);
          const afterTenant = newCols.slice(idxTenant);
          // 直接插入，依赖原本列清单本身已有逗号分隔
          newCols = `${beforeTenant}[culture_code],${afterTenant}`;
        }
      }

      const hasCultureVal = newVals.includes("@culture_code");
      if (!hasCultureVal) {
        const idxS = newVals.indexOf("S.[tenant_code]");
        if (idxS > -1) {
          const beforeTenantVal = newVals.slice(0, idxS);
          const afterTenantVal = newVals.slice(idxS);
          newVals = `${beforeTenantVal}@culture_code,${afterTenantVal}`;
        } else {
          const idxA = newVals.indexOf("@tenant_code");
          if (idxA > -1) {
            const beforeTenantVal = newVals.slice(0, idxA);
            const afterTenantVal = newVals.slice(idxA);
            newVals = `${beforeTenantVal}@culture_code,${afterTenantVal}`;
          }
        }
      }

      // rebuild
      return full.replace(cols, newCols).replace(vals, newVals);
    });

    if (sql !== before) {
      writeText(file, sql.replace(/\n/g, "\r\n"));
      changedFiles++;
    }
  }

  console.log(`Quartz sync culture_code MERGE INSERT 补写完成。变更文件数：${changedFiles}`);
}

main();

