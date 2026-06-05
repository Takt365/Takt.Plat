// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：strip-auto-imports-in-vue.mjs
// 功能描述：移除 .vue 中已由 unplugin-auto-import 注入的 vue / vue-router / pinia 重复 import
// ========================================

import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..', 'src');

/** unplugin-auto-import 已注入的 vue 运行时 API（与 auto-imports.d.ts 一致） */
const AUTO_VUE = new Set([
  'EffectScope', 'computed', 'createApp', 'customRef', 'defineAsyncComponent', 'defineComponent',
  'effectScope', 'getCurrentInstance', 'getCurrentScope', 'getCurrentWatcher', 'h', 'inject',
  'isProxy', 'isReactive', 'isReadonly', 'isRef', 'isShallow', 'markRaw', 'nextTick',
  'onActivated', 'onBeforeMount', 'onBeforeUnmount', 'onBeforeUpdate', 'onDeactivated',
  'onErrorCaptured', 'onMounted', 'onRenderTracked', 'onRenderTriggered', 'onScopeDispose',
  'onServerPrefetch', 'onUnmounted', 'onUpdated', 'onWatcherCleanup', 'provide', 'reactive',
  'readonly', 'ref', 'resolveComponent', 'shallowReactive', 'shallowReadonly', 'shallowRef',
  'toRaw', 'toRef', 'toRefs', 'toValue', 'triggerRef', 'unref', 'useAttrs', 'useCssModule',
  'useCssVars', 'useId', 'useModel', 'useSlots', 'useTemplateRef', 'watch', 'watchEffect',
  'watchPostEffect', 'watchSyncEffect',
]);

const AUTO_VUE_ROUTER = new Set([
  'onBeforeRouteLeave', 'onBeforeRouteUpdate', 'useLink', 'useRoute', 'useRouter',
]);

const AUTO_PINIA = new Set([
  'acceptHMRUpdate', 'createPinia', 'defineStore', 'getActivePinia', 'mapActions', 'mapGetters',
  'mapState', 'mapStores', 'mapWritableState', 'setActivePinia', 'setMapStoreSuffix', 'storeToRefs',
]);

/** vue 全局类型（auto-imports.d.ts 已 re-export，.vue 内可省略 type import） */
const AUTO_VUE_TYPES = new Set([
  'Component', 'Slot', 'Slots', 'ComponentPublicInstance', 'ComputedRef', 'DirectiveBinding',
  'ExtractDefaultPropTypes', 'ExtractPropTypes', 'ExtractPublicPropTypes', 'InjectionKey',
  'PropType', 'Ref', 'ShallowRef', 'MaybeRef', 'MaybeRefOrGetter', 'VNode', 'WritableComputedRef',
]);

const MODULE_MAP = {
  vue: AUTO_VUE,
  'vue-router': AUTO_VUE_ROUTER,
  pinia: AUTO_PINIA,
};

/**
 * 解析 import 子句中的标识符
 * @param {string} clause 如 "computed, type Ref, type Component"
 * @returns {{ values: string[], types: string[] }}
 */
function parseImportClause(clause) {
  const values = [];
  const types = [];
  let isType = false;
  for (const part of clause.split(',')) {
    const token = part.trim();
    if (!token) continue;
    if (token === 'type') {
      isType = true;
      continue;
    }
    const name = token.replace(/^type\s+/, '').split(/\s+as\s+/)[0].trim();
    if (token.startsWith('type ') || isType) {
      types.push(name);
      isType = false;
    } else {
      values.push(name);
    }
  }
  return { values, types };
}

/**
 * 处理单条 import 语句
 * @param {string} line
 * @returns {string|null} 替换行；null 表示删除整行
 */
function processImportLine(line) {
  const m = line.match(/^import\s+(type\s+)?\{([^}]+)\}\s+from\s+['"](vue-router|vue|pinia)['"];?\s*$/);
  if (!m) return line;

  const moduleName = m[3];
  const autoSet = MODULE_MAP[moduleName];
  const { values, types } = parseImportClause(m[2]);

  const remainValues = values.filter((n) => !autoSet.has(n));
  let remainTypes = types;
  if (moduleName === 'vue') {
    remainTypes = types.filter((n) => !AUTO_VUE_TYPES.has(n));
  }

  if (remainValues.length === 0 && remainTypes.length === 0) {
    return null;
  }

  const parts = [];
  if (remainValues.length) parts.push(remainValues.join(', '));
  for (const t of remainTypes) parts.push(`type ${t}`);

  return `import { ${parts.join(', ')} } from '${moduleName}';`;
}

/**
 * @param {string} filePath
 */
function processFile(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const lines = content.split(/\r?\n/);
  let changed = false;
  const out = [];

  for (const line of lines) {
    const next = processImportLine(line);
    if (next === null) {
      changed = true;
      continue;
    }
    if (next !== line) changed = true;
    out.push(next ?? line);
  }

  if (changed) {
    fs.writeFileSync(filePath, out.join('\n'), 'utf8');
    console.log('updated:', path.relative(ROOT, filePath));
  }
}

/**
 * @param {string} dir
 */
function walk(dir) {
  for (const ent of fs.readdirSync(dir, { withFileTypes: true })) {
    const p = path.join(dir, ent.name);
    if (ent.isDirectory()) walk(p);
    else if (ent.name.endsWith('.vue')) processFile(p);
  }
}

walk(ROOT);
