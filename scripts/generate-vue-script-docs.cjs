// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-vue-script-docs.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Vue CRUD 生成脚本共享 JSDoc（index.vue / *-form.vue script setup 段）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 树表 index.vue：ref/computed 状态块（完整 JSDoc）
 * @param {string} entityPascal
 * @param {object} options
 */
function buildTreeIndexStateRefs(entityPascal, options) {
  const {
    hasForm,
    hasImport,
    idField,
    titleField,
    queryInit,
    queryFieldsMetaBlock,
    searchPlaceholderFields,
    needsUserStore,
    needsExcelNames,
    entityClassName,
  } = options;
  const formBlock = hasForm ? `
/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<${entityPascal}>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()` : '';
  const importLine = hasImport ? `/** 导入对话框是否打开 */
const importVisible = ref(false)
` : '';
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${needsUserStore ? `/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
` : ''}${needsExcelNames ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${entityClassName}')
` : ''}/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [${searchPlaceholderFields}].join(' / '),
  })
)

/** 左侧树关键字（客户端过滤，不重复请求 API） */
const treeQueryKeyword = ref('')
/** 右侧树表快捷查询关键字 */
const queryKeyword = ref('')
/** 左侧树工具栏「展开/收缩」状态 */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表 */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧表格展开状态（预留） */
const tableExpanded = ref(false)
/** 右侧拍平列表当前页码 */
const tableCurrentPage = ref(1)
/** 右侧拍平列表每页条数 */
const tablePageSize = ref(20)
/** 页面 loading（树加载、提交、导出等） */
const loading = ref(false)
/** 全量树表节点（左侧树与右侧表共用，不受右侧查询过滤） */
const fullTableTree = ref<Record<string, unknown>[]>([])
/** 左侧 a-tree 绑定数据（由 fullTableTree 映射 title/key） */
const entityTreeData = ref<TreeDataItem[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selectedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selectedRow = ref<${entityPascal} | null>(null)
/** 表格多选行 */
const selectedRows = ref<${entityPascal}[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])
${formBlock}
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
${queryInit}
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
${queryFieldsMetaBlock}
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
${importLine}/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = '${idField}'
/** 树节点标题字段名（左侧树 title 与缩进列） */
const treeTitleField = '${titleField}'
`;
}

/**
 * 单表/主子表 index.vue：ref/computed 状态块（完整 JSDoc）
 * @param {string} entityPascal
 * @param {object} options
 */
function buildSingleIndexStateRefs(entityPascal, options) {
  const {
    hasForm,
    hasImport,
    hasUpdate,
    hasDelete,
    queryInit,
    queryFieldsMetaBlock,
    entityIdName,
    excelConst,
  } = options;
  const formBlock = hasForm ? `
/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<${entityPascal}>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()` : '';
  const importLine = hasImport ? `/** 导入对话框是否打开 */
const importVisible = ref(false)
` : '';
  const updateDisabled = hasUpdate ? `/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
` : '';
  const deleteDisabled = hasDelete ? `/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)
` : '';
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${excelConst}/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.${options.entityI18nSlug || options.entityCamel}._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<${entityPascal}[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<${entityPascal} | null>(null)
/** 表格多选行 */
const selectedRows = ref<${entityPascal}[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])
${formBlock}/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
${queryInit}
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
${queryFieldsMetaBlock}
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
${importLine}/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = '${entityIdName}'
${updateDisabled}${deleteDisabled}`;
}

/**
 * *-form.vue script 段：表单状态与 props（完整 JSDoc）
 * @param {object} options
 */
function buildFormScriptStateBlock(options) {
  const { formContentClassExpr, formFieldsJson, mdScript, scopeStoreScript } = options;
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${scopeStoreScript || ''}/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = ${formContentClassExpr}
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ${formFieldsJson}
${mdScript}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<${options.entityPascal}Create & { ${options.entityIdField}?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})`;
}

module.exports = {
  buildTreeIndexStateRefs,
  buildSingleIndexStateRefs,
  buildFormScriptStateBlock,
};
