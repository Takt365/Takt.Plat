// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/utils
// 文件名称：naming.ts
// 功能描述：与后端 TaktNamingHelper 对齐的 Excel 导入导出命名（sheet = 去 Takt 后的英文名，fileName = 实体类名）。
// ========================================

/**
 * 与后端 TaktNamingHelper.DefaultSheetNameEnglish 一致。
 * @param entityTypeName 领域实体类名，如 TaktUser
 * @returns 去掉 Takt 前缀后的工作表英文名；空输入返回 ''
 */
export function taktDefaultExcelSheetName(entityTypeName: string): string {
  const n = (entityTypeName || '').trim()
  if (!n) return n
  if (n.startsWith('Takt') && n.length > 4) return n.slice(4)
  return n
}

export type TaktExcelEntityNames = { sheet: string; fileBase: string }

/**
 * 与后端 ResolveExcelImportExport 在「仅传实体类名」时的默认一致。
 * @param entityTypeName 领域实体类名
 * @param sheetEnglishOverride 非标准列表导出等工作表名（如 FlowTodo）
 * @returns sheet 与 fileBase（不含扩展名）
 */
export function taktExcelEntityNames(
  entityTypeName: string,
  sheetEnglishOverride?: string | null
): TaktExcelEntityNames {
  const fileBase = (entityTypeName || '').trim()
  const sheet = (sheetEnglishOverride?.trim() || taktDefaultExcelSheetName(fileBase))
  return { sheet, fileBase }
}
