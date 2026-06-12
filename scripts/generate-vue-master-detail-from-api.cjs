// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/scripts
// 文件名称：generate-vue-master-detail-from-api.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：主子表 Vue（展开行 + 表单 Tab），仅此一种模板
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { buildSingleIndexStateRefs, buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  VUE_TEMPLATE,
  runVueGeneratorCli,
  FORM_TAB_FIELDS_PER_TAB,
  buildMenuIndex,
  buildMasterDetailChildRegistry,
  loadVueModuleContext,
  writeVueModuleOutputs,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderQueryFormItem,
  renderFormControl,
  computeFormTabCount,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  hasScopeContextFormFields,
  pascalToCamel,
} = require('./generate-vue-common.cjs');

/**
 * 生成 index.vue 主子表展开区模板
 * @param {object[]} children
 * @returns {string}
 */
function generateExpandedRowTemplate(children) {
  if (!children.length) {
    return '';
  }
  const tables = children.map((child) => `          <div class="mb-2 text-sm font-medium">{{ t('entity.${child.childI18nSlug}._self') }}</div>
          <a-table
            v-if="has${child.childPascal}Rows(record)"
            :columns="${child.childCamel}ExpandColumns"
            :data-source="get${child.childPascal}Rows(record)"
            :row-key="(row: ${child.childType}, index?: number) => row?.${child.childIdField} || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />`).join('\n');
  return `      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
${tables}
        </div>
      </template>`;
}

/**
 * 生成 index.vue 主子表 script 片段（对齐 foundation/dict：展开行懒加载子表 list API）
 * @param {object} ctx
 * @returns {{ state: string, columns: string, helpers: string, handlers: string, childApiImports: string, expandProps: string, expandTemplate: string }}
 */
function generateMasterDetailIndexScript(ctx) {
  const { entityPascal, caps, fields, modulePath } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length) {
    return {
      state: '',
      columns: '',
      helpers: '',
      handlers: '',
      childApiImports: '',
      expandProps: '',
      expandTemplate: '',
    };
  }
  const state = '/** 主子表展开行 keys（手风琴，仅一行展开） */\nconst expandedRowKeys = ref<string[]>([])\n';
  const columns = children.map((child) => {
    const cols = child.listFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    ellipsis: true,
  },`).join('\n');
    return `/** 展开行预览：${child.childCamel} 列 */
const ${child.childCamel}ExpandColumns = computed(() => [
${cols}
])`;
  }).join('\n\n');
  const helpers = children.map((child) => `/** 读取主表行上的 ${child.childCamel} 子表缓存 */
function get${child.childPascal}Rows(record: ${entityPascal}): ${child.childType}[] {
  return (record as any)?.${child.fieldName} ?? []
}

/** 主表行是否已加载 ${child.childCamel} 子表 */
function has${child.childPascal}Rows(record: ${entityPascal}): boolean {
  return get${child.childPascal}Rows(record).length > 0
}`).join('\n\n');
  const loadChildFns = children.map((child) => {
    if (child.apiGetList) {
      const childQueryType = `${child.childType}Query`;
      return `/** 懒加载 ${child.childCamel} 子表（${childQueryType} + ${child.childCamel}Api，与主表 ${entityPascal}Query 分离） */
async function load${child.childPascal}For${entityPascal}(record: ${entityPascal}): Promise<${child.childType}[]> {
  const masterId = get${entityPascal}Id(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ${childQueryType} = {
      pageIndex: 1,
      pageSize: 500,
      ${child.masterFkField}: masterId,
    }
    const result = await ${child.childCamel}Api.${child.apiGetList}(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => get${entityPascal}Id(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, ${child.fieldName}: rows } as ${entityPascal}
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}`;
    }
    return `/** 通过主表详情接口加载 ${child.childCamel} 子表 */
async function load${child.childPascal}For${entityPascal}(record: ${entityPascal}): Promise<${child.childType}[]> {
  const detail = await load${entityPascal}Detail(record)
  return detail?.${child.fieldName} ?? []
}`;
  }).join('\n\n');
  const ensureLoaded = children.map((child) => `  if (!has${child.childPascal}Rows(record)) {
    await load${child.childPascal}For${entityPascal}(record)
  }`).join('\n');
  const detailFn = caps.hasGetById ? `
/** 加载主表详情并回填当前页 dataSource */
async function load${entityPascal}Detail(record: ${entityPascal}): Promise<${entityPascal} | null> {
  const id = get${entityPascal}Id(record)
  if (!id) {
    return null
  }
  try {
    const detail = await ${caps.apiGetById}(id)
    const index = dataSource.value.findIndex((row) => get${entityPascal}Id(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ${entityPascal}
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}` : '';
  const handlers = `
${detailFn}
${loadChildFns}

/** 展开前确保各子表已懒加载 */
async function ensure${entityPascal}ChildrenLoaded(record: ${entityPascal}) {
${ensureLoaded}
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: ${entityPascal}) {
  const key = get${entityPascal}Id(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensure${entityPascal}ChildrenLoaded(record)
  expandedRowKeys.value = [key]
}`;
  const childApiImports = children
    .filter((child) => child.apiGetList)
    .map((child) => `import * as ${child.childCamel}Api from '@/api/${modulePath}/${child.childKebab}'`)
    .join('\n');
  return {
    state,
    columns,
    helpers,
    handlers,
    childApiImports,
    expandProps: `
      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"`,
    expandTemplate: generateExpandedRowTemplate(children),
  };
}

/**
 * 生成 *-form.vue 主子表 Tab 与 script
 * @param {object} ctx
 * @returns {{ tabs: string, script: string, needsTaktSelect: boolean }}
 */
function generateMasterDetailFormParts(ctx) {
  const { entityPascal, entityCamel, fields } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length) {
    return { tabs: '', script: '', needsTaktSelect: false };
  }
  const tabs = children.map((child) => {
    const cols = child.formFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 140,
  },`).join('\n');
    const bodyCells = child.formFields.map((f, fieldIndex) => {
      const cond = fieldIndex === 0 ? 'v-if' : 'v-else-if';
      return `            <template ${cond}="column.key === '${f.name}'">
${renderFormControl(f, 'record.', '              ')}
            </template>`;
    }).join('\n');
    return `      <!-- 子表：${child.childCamel} -->
      <a-tab-pane
        key="child-${child.fieldName}"
        :tab="t('entity.${child.childI18nSlug}._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAdd${child.childPascal}Row">
            {{ t('common.page.button.create') }}{{ t('entity.${child.childI18nSlug}._self') }}
          </a-button>
        </div>
        <a-table
          :columns="${child.childCamel}FormColumns"
          :data-source="child${child.childPascal}Rows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
${bodyCells}
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemove${child.childPascal}Row(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>`;
  }).join('\n');
  const columnDefs = children.map((child) => {
    const cols = child.formFields.map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 140,
  },`).join('\n');
    return `/** 子表 ${child.childCamel} 表单列定义 */
const ${child.childCamel}FormColumns = computed(() => [
${cols}
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])`;
  }).join('\n\n');
  const rowRefs = children.map((child) => `/** ${child.childCamel} 子表行（表单 Tab 内嵌） */
const child${child.childPascal}Rows = ref<Record<string, unknown>[]>([])`).join('\n');
  const syncFromForm = children.map((child) => `  child${child.childPascal}Rows.value = ((val as any)?.${child.fieldName} ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.${child.childIdField} ?? \`new-\${index}\`,
  }))`).join('\n');
  const clearRows = children.map((child) => `  child${child.childPascal}Rows.value = []`).join('\n');
  const addHandlers = children.map((child) => {
    const defaults = child.formFields.map((f) => {
      if (f.readOnly && f.name === 'tenantCode') {
        return '      tenantCode: tenantStore.tenantCode,';
      }
      if (f.readOnly && f.name === 'companyCode') {
        return '      companyCode: tenantStore.companyCode,';
      }
      if (f.readOnly && f.name === 'companyDefaultCulture') {
        return "      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',";
      }
      const val = f.type === 'number' ? '0' : "''";
      return `      ${f.name}: ${val},`;
    }).join('\n');
    return `/** 表单 Tab 内新增 ${child.childCamel} 行 */
function handleAdd${child.childPascal}Row() {
  child${child.childPascal}Rows.value.push({
    __rowKey: \`new-\${Date.now()}\`,
${defaults}
  })
}

/** 表单 Tab 内删除 ${child.childCamel} 行 */
function handleRemove${child.childPascal}Row(index: number) {
  child${child.childPascal}Rows.value.splice(index, 1)
}`;
  }).join('\n\n');
  const getValuesMerge = children.map((child) => `    ${child.fieldName}: child${child.childPascal}Rows.value.map(({ __rowKey, ...rest }) => rest),`).join('\n');
  const script = `
${rowRefs}

${columnDefs}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<${entityPascal}Create & { ${pascalToCamel(entityPascal)}Id?: string }> | null | undefined) {
${syncFromForm}
}

${addHandlers}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
${getValuesMerge}
  }
}`;
  const needsTaktSelect = children.some((c) => c.formFields.some((f) => f.htmlType === 'select' && f.dictType));
  return { tabs, script, needsTaktSelect };
}

const EMPTY_MD_INDEX_PARTS = {
  state: '',
  columns: '',
  helpers: '',
  handlers: '',
  childApiImports: '',
  expandProps: '',
  expandTemplate: '',
};

/**
 * 生成 index.vue（单表 CRUD 或主子表，由 assemblyOptions 区分）
 * @param {object} ctx
 * @param {{ mdParts?: object, hasMasterDetail?: boolean, generatorScript?: string }} [assemblyOptions]
 */
function generateMasterDetailIndexVue(ctx) {
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    modulePath,
    viewModulePath,
    permissionPrefix,
    cssRootClass,
    caps,
    fields,
    comment,
  } = ctx;
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  const mdParts = generateMasterDetailIndexScript(ctx);
  const hasMasterDetail = true;
  const entityScope = fields.entityScope || 'company';
  const importApiNames = [
    caps.apiGetList,
    caps.apiGetById,
    caps.apiCreate,
    caps.apiUpdate,
    caps.apiDelete,
    caps.apiDeleteBatch,
    caps.apiGetTemplate,
    caps.apiImport,
    caps.apiExport,
  ].filter(Boolean);
  const typeImports = [`${entityPascal}`, `${entityPascal}Query`, `${entityPascal}Create`, `${entityPascal}Update`]
    .filter((name, idx, arr) => arr.indexOf(name) === idx);
  const childTypeImportLines = hasMasterDetail
    ? (fields.masterDetailChildren || [])
      .map((child) => {
        const typeNames = [child.childType];
        if (child.apiGetList) {
          typeNames.push(`${child.childType}Query`);
        }
        return `import type { ${typeNames.join(', ')} } from '@/types/${modulePath}/${child.childKebab}'`;
      })
      .join('\n')
    : '';
  const listCols = fields.listFields.filter((f) => f.name !== caps.entityIdName);
  const dictListCols = listCols.filter((f) => f.dictType);
  const dictBodyCellBlock = dictListCols.length > 0
    ? `      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
${dictListCols.map((f, i) => `        <template ${i === 0 ? 'v-if' : 'v-else-if'}="column.key === '${f.name}'">
          <TaktDictTag
            :value="get${entityPascal}Field(record, '${f.name}')"
            dict-type="${f.dictType}"
          />
        </template>`).join('\n')}
      </template>
`
    : '';
  const queryItems = fields.queryFields.map((f) => renderQueryFormItem(f)).join('\n');
  const queryFieldsMetaBlock = fields.queryFields.map((f) => `  { key: '${f.name}', label: ${fieldLabelTExpr(f)} },`).join('\n');
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}`;
  const queryInit = fields.queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const columnBlocks = listCols.map((f) => {
    if (f.dictType) {
      return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
  },`;
    }
    return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${f.name}') ?? ''
  },`;
  }).join('\n');
  const actionItems = [];
  if (caps.hasUpdate) {
    actionItems.push(`      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${permissionPrefix}:update',
        onClick: (record: ${entityPascal}) => handleEdit(record)
      },`);
  }
  if (caps.hasDelete) {
    actionItems.push(`      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${permissionPrefix}:delete',
        onClick: (record: ${entityPascal}) => handleDeleteOne(record)
      }`);
  }
  const formBlock = (caps.hasCreate || caps.hasUpdate) ? `
    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <${entityPascal}Form
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>` : '';
  const importBlock = (caps.hasImport && caps.hasGetTemplate) ? `
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.${entityI18nSlug}._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.${entityI18nSlug}._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>` : '';
  const formImports = (caps.hasCreate || caps.hasUpdate)
    ? `import ${entityPascal}Form from './components/${entityKebab}-form.vue'\n`
    : '';
  const iconImports = (caps.hasUpdate || caps.hasDelete)
    ? "import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'\n"
    : '';
  const excelImport = (caps.hasImport || caps.hasExport)
    ? "import { taktExcelEntityNames } from '@/utils/naming'\n"
    : '';
  const exportImport = caps.hasExport
    ? "import { resolveExportDownloadFileName } from '@/utils/export-download-name'\n"
    : '';
  const excelConst = (caps.hasImport || caps.hasExport)
    ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${caps.entityClassName}')
`
    : '';
  const singleStateBlock = buildSingleIndexStateRefs(entityPascal, {
    hasForm: caps.hasCreate || caps.hasUpdate,
    hasImport: caps.hasImport && caps.hasGetTemplate,
    hasUpdate: caps.hasUpdate,
    hasDelete: caps.hasDelete || caps.hasDeleteBatch,
    queryInit,
    queryFieldsMetaBlock,
    entityIdName: caps.entityIdName,
    entityCamel,
    entityI18nSlug,
    excelConst,
  });
  const formStateBlock = '';
  const createHandler = caps.hasCreate ? `
/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.${entityI18nSlug}._self') })
  formData.value = {}
  formVisible.value = true
}` : '';
  const updateHandler = caps.hasUpdate ? (hasMasterDetail && caps.hasGetById ? `
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ${entityPascal}) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.${entityI18nSlug}._self') })
  formLoading.value = true
  try {
    const detail = await load${entityPascal}Detail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.${entityI18nSlug}._self') }))
  }
}` : `
/** 打开编辑弹窗 */
function handleEdit(record: ${entityPascal}) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.${entityI18nSlug}._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.${entityI18nSlug}._self') }))
  }
}`) : '';
  const formSubmitHandler = (caps.hasCreate || caps.hasUpdate) ? `
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
${caps.hasUpdate ? `      await ${caps.apiUpdate}(id, payload as any)\n      message.success(t('common.feedback.updated', { target: t('entity.${entityI18nSlug}._self') }))` : ''}
    } else {
${caps.hasCreate ? `      await ${caps.apiCreate}(payload as any)\n      message.success(t('common.feedback.created', { target: t('entity.${entityI18nSlug}._self') }))` : ''}
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}` : '';
  const importHandlers = (caps.hasImport && caps.hasGetTemplate) ? `
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await ${caps.apiGetTemplate}(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await ${caps.apiImport}(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}` : '';
  const exportHandler = caps.hasExport ? `
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: ${entityPascal}Query = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await ${caps.apiExport}(exportQuery, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = \`\${excelNames.fileBase}_\${ts.getFullYear()}\${pad(ts.getMonth() + 1)}\${pad(ts.getDate())}\${pad(ts.getHours())}\${pad(ts.getMinutes())}\${pad(ts.getSeconds())}\`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.${entityI18nSlug}._self') }))
  } catch (error: any) {
    logger.error('[${entityPascal}] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.${entityI18nSlug}._self') }))
  } finally {
    loading.value = false
  }
}` : '';
  const deleteOneHandler = caps.hasDelete ? `
/** 删除单行 */
async function handleDeleteOne(record: ${entityPascal}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.${entityI18nSlug}._self'), name: t('common.tip.this.target', { target: t('entity.${entityI18nSlug}._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${caps.apiDelete}((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.${entityI18nSlug}._self') }))
      loadData()
    }
  })
}` : '';
  const deleteBatchHandler = caps.hasDeleteBatch ? `
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.${entityI18nSlug}._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.${entityI18nSlug}._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await ${caps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.${entityI18nSlug}._self') }))
      loadData()
    }
  })
}` : '';
  const loadDataBody = caps.hasGetList ? `    const kw = (queryKeyword.value ?? '').trim()
    const params: ${entityPascal}Query = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await ${caps.apiGetList}(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0` : `    dataSource.value = []
    total.value = 0`;
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：${comment}管理页面，含查询、增删改，由 ${generatorScript} 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="${cssRootClass}">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
${caps.hasCreate ? `      create-permission="${permissionPrefix}:create"` : ''}
${caps.hasUpdate ? `      update-permission="${permissionPrefix}:update"` : ''}
${caps.hasDelete ? `      delete-permission="${permissionPrefix}:delete"` : ''}
${caps.hasImport ? `      import-permission="${permissionPrefix}:import"` : ''}
${caps.hasExport ? `      export-permission="${permissionPrefix}:export"` : ''}
      :show-create="${caps.hasCreate}"
      :show-update="${caps.hasUpdate}"
      :show-delete="${caps.hasDelete || caps.hasDeleteBatch}"
      :show-import="${caps.hasImport && caps.hasGetTemplate}"
      :show-export="${caps.hasExport}"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
${caps.hasCreate ? '      :create-disabled="false"\n      :create-loading="loading"' : ''}
${caps.hasUpdate ? '      :update-disabled="updateDisabled"\n      :update-loading="loading"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      :delete-disabled="deleteDisabled"\n      :delete-loading="loading"' : ''}
      :refresh-loading="loading"
${caps.hasCreate ? '      @create="handleCreate"' : ''}
${caps.hasUpdate ? '      @update="handleUpdate"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      @delete="handleDelete"' : ''}
${caps.hasImport && caps.hasGetTemplate ? '      @import="handleImport"' : ''}
${caps.hasExport ? '      @export="handleExport"' : ''}
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      :columns="columns"
      entity-scope="${entityScope}"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'${caps.entityIdName}'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="get${entityPascal}Id"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
${mdParts.expandProps}
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
${dictBodyCellBlock}${mdParts.expandTemplate}
    </TaktSingleTable>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
${formBlock}
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'${queryFieldStorageKey}'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
${queryItems}
      </template>
    </TaktQueryDrawer>
${importBlock}
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'${caps.entityIdName}'"
      :action-column-key="'action'"
      entity-scope="${entityScope}"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}管理页 · 由 ${generatorScript} 根据 types/api 生成
 * @module views/${viewModulePath}
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
${formImports}import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${entityKebab}'
${mdParts.childApiImports ? `${mdParts.childApiImports}\n` : ''}${childTypeImportLines ? `${childTypeImportLines}\n` : ''}import type { ${typeImports.join(', ')} } from '@/types/${modulePath}/${entityKebab}'
${excelImport}${exportImport}${iconImports}
${singleStateBlock}
${mdParts.state}
/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

${mdParts.columns}

${mdParts.helpers}
${mdParts.handlers}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${caps.entityIdName}',
    key: '${caps.entityIdName}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${caps.entityIdName}') ?? ''
  },
${columnBlocks}
  CreateActionColumn({
    actions: [
${actionItems.join('\n')}
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const get${entityPascal}Id = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const get${entityPascal}Field = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${entityPascal}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ${entityPascal}, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (get${entityPascal}Id(selectedRow.value) === get${entityPascal}Id(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${entityPascal}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ${entityPascal}) => ({
  onClick: () => {
    const key = get${entityPascal}Id(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(get${entityPascal}Id(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
${loadDataBody}
  } catch (error: any) {
    logger.error('[${entityPascal}] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
${queryInit}
  }
  currentPage.value = 1
  loadData()
}
${createHandler}${updateHandler}${formSubmitHandler}${importHandlers}${exportHandler}${deleteOneHandler}${deleteBatchHandler}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
${queryInit}
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.${cssRootClass} {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
`;
}

const EMPTY_MD_FORM_PARTS = { tabs: '', script: '', needsTaktSelect: false };

/**
 * 生成 *-form.vue（单表 CRUD 或主子表，由 assemblyOptions 区分）
 * @param {object} ctx
 * @param {{ mdFormParts?: object, hasMasterDetail?: boolean, generatorScript?: string }} [assemblyOptions]
 */
function generateMasterDetailFormVue(ctx) {
  const { entityPascal, entityCamel, entityKebab, modulePath, viewModulePath, fields, comment } = ctx;
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  const mdFormParts = generateMasterDetailFormParts(ctx);
  const hasMasterDetail = true;
  const formFields = fields.formFields;
  const tabCount = computeFormTabCount(formFields.length);
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const tabs = [];
  for (let tabIndex = 1; tabIndex <= tabCount; tabIndex += 1) {
    const start = (tabIndex - 1) * FORM_TAB_FIELDS_PER_TAB;
    const end = tabIndex * FORM_TAB_FIELDS_PER_TAB;
    const tabFields = formFields.slice(start, end);
    const items = tabFields.map((f) => {
      const colSpan = f.htmlType === 'textarea' ? 24 : 12;
      const control = renderFormControl(f, 'formState.', '                ');
      return `            <a-col :span="${colSpan}">
              <a-form-item
                :label="${fieldLabelTExpr(f)}"
                name="${f.name}"
              >
${control}
              </a-form-item>
            </a-col>`;
    }).join('\n');
    const tabLabel = buildFormTabLabelAttr(tabIndex, tabCount);
    const tabComment = tabIndex === 1 && hasMasterDetail ? '      <!-- 主表 -->\n' : '';
    tabs.push(`${tabComment}      <a-tab-pane
        key="tab-${tabIndex - 1}"
        ${tabLabel}
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
${items}
          </a-row>
        </div>
      </a-tab-pane>`);
  }
  const needsTaktSelect = formFields.some((f) => f.htmlType === 'select' && f.dictType) || mdFormParts.needsTaktSelect;
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasScopeContextFields = hasScopeContextFormFields(formFields, masterDetailChildren);
  const entityIdField = `${pascalToCamel(entityPascal)}Id`;
  const scopeStoreImports = hasScopeContextFields
    ? "import { useTenantStore } from '@/stores/identity/tenant'\nimport { useUserStore } from '@/stores/identity/user'\n"
    : '';
  const scopeStoreScript = hasScopeContextFields ? `
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
` : '';
  const scopeContextWatch = hasScopeContextFields ? `
/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.${entityIdField}
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)
` : '';
  const childTypeImports = hasMasterDetail
    ? [...new Set((fields.masterDetailChildren || []).flatMap((c) => [c.childCreateType, c.childType]))]
    : [];
  const typeImportLine = [`${entityPascal}Create`, ...childTypeImports]
    .filter((name, idx, arr) => arr.indexOf(name) === idx)
    .join(', ');
  const childFieldStrip = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `    delete (next as any).${c.fieldName}`).join('\n')
    : '';
  const watchSyncChild = hasMasterDetail ? '    syncChildRowsFromFormData(val)\n' : '';
  const resetChildRows = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `  child${c.childPascal}Rows.value = []`).join('\n')
    : '';
  const getValuesBody = hasMasterDetail ? '  return buildSubmitPayload()' : '  return { ...formState }';
  const taktSelectImport = needsTaktSelect
    ? "import TaktSelect from '@/components/business/takt-select/index.vue'\n"
    : '';
  const requiredRules = formFields
    .filter((f) => !f.optional && f.name !== 'remark' && !f.readOnly)
    .map((f) => {
      const trigger = f.htmlType === 'select' || f.htmlType === 'date' || f.htmlType === 'switch' ? 'change' : 'blur';
      const placeholderKey = f.htmlType === 'select' || f.htmlType === 'date'
        ? 'common.page.form.placeholder.select'
        : 'common.page.form.placeholder.required';
      return `  ${f.name}: [
    {
      required: true,
      message: ${fieldPlaceholderTExpr(f, placeholderKey)},
      trigger: '${trigger}'
    }
  ],`;
    }).join('\n');
  const formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(formFields.map((f) => f.name)),
    mdScript: mdFormParts.script,
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal,
    entityIdField,
  });
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${entityKebab}-form.vue -->
<!-- 功能描述：${comment}维护弹窗内嵌表单。由 ${generatorScript} 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="${entityKebab}-form-tabs"
    >
${tabs.join('\n')}
${mdFormParts.tabs}
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * ${comment}维护表单 · 由 ${generatorScript} 根据 types/api 生成
 * @module views/${viewModulePath}/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ${typeImportLine} } from '@/types/${modulePath}/${entityKebab}'
${taktSelectImport}${scopeStoreImports}
${formScriptState}

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
${childFieldStrip}
${hasScopeContextFields ? '    applyScopeDefaults(next)\n' : ''}    Object.assign(formState, next)
${watchSyncChild}  },
  { immediate: true, deep: true }
)
${scopeContextWatch}
/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
${requiredRules}
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
${getValuesBody}
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
${resetChildRows}
  activeTab.value = 'tab-0'
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
`;
}
/**
 * 处理主子表 API 模块
 */
function processMasterDetailApiModule(apiFilePath, options, registry) {
  const bundle = loadVueModuleContext(apiFilePath, options, registry);
  if (bundle.skipped) {
    return bundle;
  }
  if (!bundle.isMasterDetailEntity) {
    console.log(`⏭️  跳过（非主子表主实体）: ${bundle.rel}`);
    return { skipped: true };
  }
  const children = bundle.fullCtx.fields.masterDetailChildren || [];
  console.log(`  主子表: ${children.map((c) => c.childPascal).join(', ')}（展开行 + 表单 Tab）`);
  console.log(`  entityScope: ${bundle.fullCtx.fields.entityScope} ← Takt${bundle.entityShort}`);
  const indexContent = generateMasterDetailIndexVue(bundle.fullCtx);
  const formContent = bundle.needsForm ? generateMasterDetailFormVue(bundle.fullCtx) : '';
  return writeVueModuleOutputs(bundle, indexContent, formContent, options);
}

function printMasterDetailUsage() {
  console.log(`
用法: node scripts/generate-vue-master-detail-from-api.cjs [参数]

模板: **主子表 Master-Detail**（OneToMany 展开行 + 表单 Tab）

参数:
  --<实体名>            如 --DictType
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体

示例:
  node scripts/generate-vue-master-detail-from-api.cjs --DictType
`);
}

if (require.main === module) {
  runVueGeneratorCli({
    banner: '🚀 主子表 Vue（generate-vue-master-detail-from-api.cjs）...\n',
    printUsage: printMasterDetailUsage,
    templateType: VUE_TEMPLATE.MASTER_DETAIL,
    buildRegistry: buildMasterDetailChildRegistry,
    onInit: buildMenuIndex,
    processModule: processMasterDetailApiModule,
  });
}

module.exports = {
  processMasterDetailApiModule,
  generateMasterDetailIndexVue,
  generateMasterDetailFormVue,
};
