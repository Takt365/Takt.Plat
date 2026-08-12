/**
 * 全量修复 Quartz sync 脚本的 culture_code 写入一致性。
 *
 * 规则（按用户要求）：
 * - 仅当目标实体（目标表）存在 CultureCode（即目标表有 culture_code 列）时，才在 sync_*.sql 中写 culture_code。
 * - 为避免破坏原有 MERGE/INSERT 列清单，这里采用“最简后置修正”：
 *   - 在脚本末尾追加 UPDATE ... SET [culture_code]=@culture_code，where 仅按 tenant/company 过滤
 *   - 只有当目标表映射到实体存在 CultureCode 才追加。
 * - culture_code 变量通过占位符绑定：@culture_code = N'{{CultureCode}}'
 *
 * 入口：node scripts/fix-quartz-sync-culture-code-all.cjs
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
    const filesHasCulture = content.includes("public string CultureCode") || content.includes("public string CultureCode {");

    let match;
    while ((match = sugarTableRegex.exec(content))) {
      const tableName = match[1];
      // 找离该 SugarTable 最近的 public class（向后搜索，直到遇到 public class）
      const start = match.index + match[0].length;
      const after = content.slice(start, start + 4000);
      const classMatch = after.match(/public class\s+\w+\s*:\s*([^{\n\r]+)/);

      const baseList = classMatch?.[1] ?? "";
      const hasCompanyCode = baseList.includes("TaktCompanyEntityBase") || baseList.includes("TaktApprovalEntityBase");
      const hasCultureCode = hasCompanyCode || filesHasCulture;

      map[tableName] = {
        hasCultureCode,
        hasCompanyCode,
      };
    }
  }

  return map;
}

function findSqlTargetTables(sqlContent) {
  /** @type {Set<string>} */
  const tables = new Set();
  const mergeRegex = /MERGE\s+INTO\s+\[([^\]]+)\]/gi;
  const insertRegex = /INSERT\s+INTO\s+\[([^\]]+)\]/gi;
  const updateRegex = /UPDATE\s+\[([^\]]+)\]/gi;

  for (const re of [mergeRegex, insertRegex, updateRegex]) {
    let m;
    while ((m = re.exec(sqlContent))) {
      const t = m[1].trim();
      if (!t.startsWith("#")) tables.add(t);
    }
  }
  return Array.from(tables);
}

function ensureDeclareCultureCode(sqlContent) {
  const cultureDeclareRegex = /DECLARE\s+@culture_code\s+NVARCHAR\(\s*5\s*\)\s*=\s*N'\{\{CultureCode\}\}'\s*;/i;
  if (cultureDeclareRegex.test(sqlContent)) return sqlContent;

  // 如果文件里已经声明了 @culture_code（但值不是占位符），就替换它
  const anyCultureDeclareRegex = /DECLARE\s+@culture_code\s+NVARCHAR\(\s*5\s*\)\s*=\s*[^;]+;/i;
  if (anyCultureDeclareRegex.test(sqlContent)) {
    return sqlContent.replace(
      anyCultureDeclareRegex,
      "DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';"
    );
  }

  // 否则插入到 @company_code 之后；若无 company_code 则插入到 @tenant_code 之后
  const cultureLine = "DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';";
  const companyLineRegex = /^(DECLARE\s+@company_code[^\n]*;)\s*$/m;
  const tenantLineRegex = /^(DECLARE\s+@tenant_code[^\n]*;)\s*$/m;

  let out = sqlContent;
  if (companyLineRegex.test(sqlContent)) {
    out = sqlContent.replace(companyLineRegex, `$1\n${cultureLine}`);
  } else if (tenantLineRegex.test(sqlContent)) {
    out = sqlContent.replace(tenantLineRegex, `$1\n${cultureLine}`);
  } else {
    // 极少情况：没找到 tenant/company 声明，直接追加在文件头（保底）
    out = `${cultureLine}\n${sqlContent}`;
  }
  return out;
}

function insertCultureUpdateBlock(sqlContent, updateStatements) {
  if (updateStatements.length === 0) return sqlContent;

  const marker = "INSERT INTO [takt_statistics_logging_oper_log]";
  const idx = sqlContent.lastIndexOf(marker);
  const insertAt = idx >= 0 ? idx : sqlContent.length;

  const block = "\n" + updateStatements.join("\n") + "\n";
  const out = sqlContent.slice(0, insertAt) + block + sqlContent.slice(insertAt);
  return out;
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

  const sqlFiles = walkDir(quartzDir)
    .filter((p) => path.basename(p).startsWith("sync_") && p.toLowerCase().endsWith(".sql"));

  /** @type {string[]} */
  const changedFiles = [];

  for (const file of sqlFiles) {
    let sqlContent = normalizeNewlines(readText(file));

    const targetTables = findSqlTargetTables(sqlContent);
    const need = [];
    for (const t of targetTables) {
      const info = map[t];
      if (!info || !info.hasCultureCode) continue;
      need.push({ table: t, hasCompanyCode: info.hasCompanyCode });
    }

    // 去重（按 table）
    const uniq = new Map();
    for (const n of need) uniq.set(n.table, n.hasCompanyCode);

    if (uniq.size === 0) continue;

    sqlContent = ensureDeclareCultureCode(sqlContent);

    /** @type {string[]} */
    const updates = [];
    for (const [table, hasCompanyCode] of uniq.entries()) {
      if (hasCompanyCode) {
        updates.push(
          `-- 强制区域文化：同步 culture_code（${table}）\nUPDATE T\nSET\n  T.[culture_code] = @culture_code\nFROM [${table}] T\nWHERE T.[tenant_code] = @tenant_code\n  AND T.[company_code] = @company_code;`
        );
      } else {
        updates.push(
          `-- 强制区域文化：同步 culture_code（${table}）\nUPDATE T\nSET\n  T.[culture_code] = @culture_code\nFROM [${table}] T\nWHERE T.[tenant_code] = @tenant_code;`
        );
      }
    }

    const old = readText(file);
    const newContent = insertCultureUpdateBlock(sqlContent, updates);
    if (newContent !== normalizeNewlines(old)) {
      writeText(file, newContent.replace(/\n/g, "\r\n"));
      changedFiles.push(path.relative(workspaceRoot, file));
    }
  }

  // 简单输出到终端（用户可见）
  console.log(`Quartz sync culture_code 修复完成。变更文件数：${changedFiles.length}`);
  for (const f of changedFiles) console.log(`- ${f}`);
}

main();

