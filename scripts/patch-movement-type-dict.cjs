/**
 * 一次性脚本：向 TaktDictDataSeedData 注入 logistics_movement_type 字典项
 */
const fs = require('fs')
const path = require('path')
const items = require('./data/logistics-movement-type.cjs')

const target = path.join(__dirname, '../backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs')
let content = fs.readFileSync(target, 'utf8')

const marker = '            ("logistics_outbound_type","其他","6","dict.logistics.outbound.type.6",7,7,0,"出库类型.其他",7),'
if (!content.includes(marker)) {
  console.error('Marker not found')
  process.exit(1)
}
if (content.includes('logistics_movement_type')) {
  console.log('logistics_movement_type already present, skip')
  process.exit(0)
}

function escapeCs(str) {
  return str.replace(/\\/g, '\\\\').replace(/"/g, '\\"')
}

const lines = items.map(([code, label], idx) => {
  const sort = idx + 1
  const i18n = `dict.logistics.movement.type.${code.toLowerCase()}`
  const isDefault = code === '101' ? 1 : 0
  const remark = `移动类型.${code} ${label}`
  return `            ("logistics_movement_type","${escapeCs(label)}","${code}","${i18n}",${sort},${sort},${isDefault},"${escapeCs(remark)}",${sort}),`
})

content = content.replace(marker, `${marker}\n${lines.join('\n')}`)
fs.writeFileSync(target, content, 'utf8')
console.log(`Inserted ${lines.length} logistics_movement_type dict entries`)
