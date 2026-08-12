// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bom-export-file-name.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析/推移导出文件名：标准名 + 选中条件后缀
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 清理导出文件名片段中的非法字符
 * @param {string} segment 原始片段
 * @returns {string} 安全片段
 */
function sanitizeExportSegment(segment: string): string {
  return segment.trim().replace(/[/\\?%*:|"<>]/g, '_');
}

/**
 * 标准导出名 + 选中条件（如机种 202MK7）组成下载基名
 * @param {string} standardName 标准名，如「DTA BOM成本推移表」
 * @param {ReadonlyArray<string | undefined | null>} selectedParts 选中信息（工厂/机种/产品等）
 * @returns {string} 基名（不含扩展名），如「DTA BOM成本推移表_202MK7」
 */
export function buildBomExportBaseName(
  standardName: string,
  selectedParts: ReadonlyArray<string | undefined | null>,
): string {
  const base = sanitizeExportSegment(standardName || '')
  if (!base) {
    return 'export'
  }
  const suffix = selectedParts
    .map((part) => (part == null ? '' : sanitizeExportSegment(String(part))))
    .filter((part) => part.length > 0)
    .join('_')
  return suffix ? `${base}_${suffix}` : base
}

/**
 * 带 .xlsx 的完整导出文件名
 * @param {string} standardName 标准名
 * @param {ReadonlyArray<string | undefined | null>} selectedParts 选中信息
 * @returns {string} 如「DTA BOM通用组件成本推移表_202MK7.xlsx」
 */
export function buildBomExportFileName(
  standardName: string,
  selectedParts: ReadonlyArray<string | undefined | null>,
): string {
  return `${buildBomExportBaseName(standardName, selectedParts)}.xlsx`
}
