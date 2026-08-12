/**
 * Restore Notification/Notifier corrupted by No→Code rename.
 * Codetification→Notification, Codetifier→Notifier; delete duplicate Codetif* files.
 */
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const SKIP = new Set(['node_modules', 'bin', 'obj', 'dist', '.git', 'artifacts', 'coverage']);
const EXT = new Set(['.cs', '.ts', '.vue', '.tsx', '.js', '.cjs', '.json', '.md', '.mdc', '.d.ts']);

/** @param {string} dir */
function walk(dir, out = []) {
  for (const ent of fs.readdirSync(dir, { withFileTypes: true })) {
    if (SKIP.has(ent.name)) continue;
    const p = path.join(dir, ent.name);
    if (ent.isDirectory()) walk(p, out);
    else out.push(p);
  }
  return out;
}

const pairs = [
  // longest first
  ['EcCodetificationDelivery', 'EcNotificationDelivery'],
  ['ecCodetificationDelivery', 'ecNotificationDelivery'],
  ['eccodetificationdelivery', 'ecnotificationdelivery'],
  ['EcCodetification', 'EcNotification'],
  ['ecCodetification', 'ecNotification'],
  ['eccodetification', 'ecnotification'],
  ['Codetification', 'Notification'],
  ['codetification', 'notification'],
  ['Codetifier', 'Notifier'],
  ['codetifier', 'notifier'],
  ['CODETIFICATION', 'NOTIFICATION'],
];

const dirs = [
  path.join(ROOT, 'backend', 'src'),
  path.join(ROOT, 'frontend', 'src'),
];

let changed = 0;
for (const d of dirs) {
  for (const f of walk(d)) {
    const ext = path.extname(f);
    if (!EXT.has(ext) && !f.endsWith('.d.ts')) continue;
    let s = fs.readFileSync(f, 'utf8');
    const before = s;
    for (const [a, b] of pairs) {
      if (s.includes(a)) s = s.split(a).join(b);
    }
    if (s !== before) {
      fs.writeFileSync(f, s, 'utf8');
      changed++;
      console.log('content', path.relative(ROOT, f).replace(/\\/g, '/'));
    }
  }
}

// Delete duplicate Codetif* files (content already fixed into Notification* siblings)
const dupes = [
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing/EngineeringChange/TaktEcCodetificationDeliveryI18nSeedData.cs',
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/Logistics/Manufacturing/EngineeringChange/TaktEcCodetificationI18nSeedData.cs',
  'backend/src/Takt.Application/Validators/Logistics/Manufacturing/EngineeringChange/TaktEcCodetificationDeliveryValidators.cs',
  'backend/src/Takt.Application/Validators/Logistics/Manufacturing/EngineeringChange/TaktEcCodetificationValidators.cs',
];
for (const rel of dupes) {
  const f = path.join(ROOT, rel);
  if (fs.existsSync(f)) {
    fs.unlinkSync(f);
    console.log('deleted', rel);
  }
}

console.log('files content-fixed:', changed);
