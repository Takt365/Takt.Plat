/**
 * 将 Quartz sync_*.sql 中 culture_code 的“后置 UPDATE 强制块”改为“直接写入 MERGE”。
 *
 * 前置：
 * - 已经支持占位符 {{CultureCode}}，脚本里通常已存在 DECLARE @culture_code。
 * - 允许脚本存在多个 MERGE（但通常每个目标表只出现一次）。
 *
 * 处理：
 * 1) 删除形如 `-- 强制区域文化：同步 culture_code` 到 `INSERT INTO [takt_statistics_logging_oper_log]` 前的后置块
 * 2) 对每个目标表（实体确实有 CultureCode）：
 *    - 在 MERGE 的 UPDATE SET 里插入：T.[culture_code] = @culture_code
 *    - 在 MERGE 的 INSERT columns/values 里插入：[culture_code] / @culture_code
 *
 * 入口：
 * node scripts/fix-quartz-sync-culture-code-direct.cjs
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
  /** @type {Record<string, {hasCultureCode: boolean, hasCompanyCode: boolean}>} */
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
      map[tableName] = { hasCultureCode, hasCompanyCode };
    }
  }
  return map;
}

function stripPostUpdateCultureBlocks(sqlContent) {
  const marker = "-- 强制区域文化：同步 culture_code";
  const idx = sqlContent.indexOf(marker);
  if (idx < 0) return sqlContent;
  const logInsert = "INSERT INTO [takt_statistics_logging_oper_log]";
  const idxLog = sqlContent.indexOf(logInsert);
  if (idxLog < 0) {
    // 保底：删到文件末尾
    return sqlContent.slice(0, idx);
  }
  return sqlContent.slice(0, idx) + "\n" + sqlContent.slice(idxLog);
}

function applyCultureCodeToMerge(sqlContent, tableName) {
  const mergeStart = `MERGE INTO [${tableName}]`;
  const startIdx = sqlContent.indexOf(mergeStart);
  if (startIdx < 0) return sqlContent;

  const nextMergeIdx = sqlContent.indexOf("\nMERGE INTO [", startIdx + mergeStart.length);
  // 只处理这一段，避免误伤后续
  const segmentEnd = nextMergeIdx > -1 ? nextMergeIdx : sqlContent.length;
  const seg = sqlContent.slice(startIdx, segmentEnd);

  let seg2 = seg;

  // 1) UPDATE SET：在 T.[updated_by] 之前插入 culture_code（如果没插过）
  if (!seg2.includes("T.[culture_code]")) {
    seg2 = seg2.replace(
      /^([ \t]*)T\.\[updated_by\]\s*=/m,
      (full, indent) => `${indent}T.[culture_code] = @culture_code,\n${indent}T.[updated_by] =`
    );
  }

  // 2) INSERT columns：在 [tenant_code] 之前插入 [culture_code]（如果没插过）
  //    仅在 MERGE 的 INSERT 子句附近生效；这里用局部替换：把第一次出现的 [tenant_code] 切开。
  if (!seg2.includes("[culture_code]") && seg2.includes("[tenant_code]")) {
    seg2 = seg2.replace(
      /\[tenant_code\]/,
      "[culture_code],[tenant_code]"
    );
  }

  // 3) INSERT values：在 S.[tenant_code] / @tenant_code 之前插入 @culture_code（如果没插过）
  //    用 @culture_code 插入到第一个 tenant_code 对应值前。
  if (!seg2.includes("@culture_code,S.[tenant_code]") && seg2.includes("S.[tenant_code]")) {
    seg2 = seg2.replace("S.[tenant_code]", "@culture_code,S.[tenant_code]");
  }
  if (!seg2.includes("@culture_code,@tenant_code") && seg2.includes("@tenant_code")) {
    // 少数写法：直接用 @tenant_code
    seg2 = seg2.replace("@tenant_code", "@culture_code,@tenant_code");
  }

  // 替换回原文
  if (seg2 !== seg) {
    return sqlContent.slice(0, startIdx) + seg2 + sqlContent.slice(segmentEnd);
  }
  return sqlContent;
}

function main() {
  const workspaceRoot = path.resolve(__dirname, "..");
  const quartzDir = path.join(
    workspaceRoot,
    "backend",
    "src",
    "Takt.WebApi",
    "wwwroot",
    "Quartz"
  );
  const domainEntitiesDir = path.join(workspaceRoot, "backend", "src", "Takt.Domain", "Entities");

  const map = buildEntityTableCultureMap(domainEntitiesDir);

  const sqlFiles = walkDir(quartzDir).filter((p) => {
    const name = path.basename(p).toLowerCase();
    return name.startsWith("sync_") && name.endsWith(".sql");
  });

  const changedFiles = [];

  for (const file of sqlFiles) {
    let sql = normalizeNewlines(readText(file));
    const before = sql;

    // 删除后置 UPDATE 块
    sql = stripPostUpdateCultureBlocks(sql);

    // 对出现过的目标表执行 MERGE 注入
    for (const [tableName, info] of Object.entries(map)) {
      if (!info.hasCultureCode) continue;
      if (!sql.includes(`MERGE INTO [${tableName}]`)) continue;
      sql = applyCultureCodeToMerge(sql, tableName);
    }

    if (sql !== before) {
      writeText(file, sql.replace(/\n/g, "\r\n"));
      changedFiles.push(path.relative(workspaceRoot, file));
    }
  }

  console.log(
    `Quartz sync culture_code 直接写入 MERGE 完成。变更文件数：${changedFiles.length}`
  );
  for (const f of changedFiles) console.log(`- ${f}`);
}

main();

