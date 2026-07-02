/**
 * 验证主题色优先级：系统默认 → 假日适配 → 用户自定义（最高）
 * 运行：node frontend/scripts/verify-theme-priority.cjs
 */
const themeColorMap = {
  'klein-blue': '#002fa7',
  'chinese-red': '#ff0000',
  'mars-green': '#2e8b57',
};

const legacyMap = {};

function resolveThemeColorPreset(stored) {
  const key = stored?.trim();
  if (!key) return null;
  if (key in themeColorMap) return key;
  return legacyMap[key] ?? null;
}

function resolveThemeColorConfigHex(config) {
  if (config.type === 'custom' && config.customColor) return config.customColor;
  const preset = { blue: 'klein-blue', red: 'chinese-red', green: 'mars-green' }[config.type];
  return preset ? themeColorMap[preset] : themeColorMap['klein-blue'];
}

function resolveEffectiveColorPrimary(setting, holiday, systemDefaultColor = { type: 'blue' }) {
  if (setting.appearanceUserOverride) {
    if (setting.themeColor.type === 'custom' && setting.themeColor.customColor) {
      return setting.themeColor.customColor;
    }
    return resolveThemeColorConfigHex(setting.themeColor);
  }
  if (holiday?.isHolidayToday) {
    const preset = resolveThemeColorPreset(holiday.holidayTheme);
    if (preset) return themeColorMap[preset];
  }
  return resolveThemeColorConfigHex(systemDefaultColor);
}

const baseSetting = {
  appearanceUserOverride: false,
  themeColor: { type: 'blue' },
};

const cases = [
  {
    name: '仅系统默认',
    setting: baseSetting,
    holiday: null,
    expect: '#002fa7',
  },
  {
    name: '假日覆盖默认',
    setting: baseSetting,
    holiday: { isHolidayToday: true, holidayTheme: 'chinese-red' },
    expect: '#ff0000',
  },
  {
    name: '非假日不生效',
    setting: baseSetting,
    holiday: { isHolidayToday: false, holidayTheme: 'chinese-red' },
    expect: '#002fa7',
  },
  {
    name: '用户自定义覆盖假日',
    setting: { appearanceUserOverride: true, themeColor: { type: 'green' } },
    holiday: { isHolidayToday: true, holidayTheme: 'chinese-red' },
    expect: '#2e8b57',
  },
  {
    name: '用户自定义 hex',
    setting: {
      appearanceUserOverride: true,
      themeColor: { type: 'custom', customColor: '#abcdef' },
    },
    holiday: { isHolidayToday: true, holidayTheme: 'chinese-red' },
    expect: '#abcdef',
  },
];

let failed = 0;
for (const c of cases) {
  const actual = resolveEffectiveColorPrimary(c.setting, c.holiday);
  const ok = actual === c.expect;
  if (!ok) {
    failed += 1;
    console.error(`FAIL ${c.name}: expected ${c.expect}, got ${actual}`);
  } else {
    console.log(`OK   ${c.name}: ${actual}`);
  }
}

if (failed > 0) {
  process.exit(1);
}
console.log('\n全部通过：系统默认 → 假日适配 → 用户自定义（最高）');
