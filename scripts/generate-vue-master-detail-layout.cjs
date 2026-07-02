// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：generate-vue-master-detail-layout.cjs
// 功能描述：主子表 Vue 布局：列表 TaktMasterDetailTableLr + 弹窗上主下从 TaktEditableTable
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

const path = require('path');
const fs = require('fs');
const { writeGeneratedFile } = require('./generate-script-common.cjs');
const { buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  CONFIG,
  FORM_TAB_FIELDS_PER_TAB,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderFormControl,
  renderFormItemOpening,
  buildFormFieldColItems,
  buildFormRowMarkup,
  buildGeneratedFormTemplateBody,
  buildFormTabsScopedStyleBlock,
  buildFormContentClassComputedExpr,
  isFixedLongTextareaField,
  isExtFieldField,
  pascalToCamel,
  renderQueryFormItem,
  buildServerPagedListQueryBlock,
  buildServerPagedExportApiCall,
  detectApiCapabilities,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  buildGeneratedFormVueScriptFragments,
  buildFormResetScopeDefaultsBlock,
  buildMasterDetailFormTypeImportLines,
  hasScopeContextFormFields,
  buildVueImportResultUtilImportLine,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  buildAdvancedQueryFactoryBlock,
  entityI18nHookName,
  entityI18nComposableFileName,
  entityRowRecordTypeName,
} = require('./generate-vue-common.cjs');

/**
 * 子表 Create 字段 → TaktEditableTableColumn 配置片段
 * @param {object} field
 * @returns {string}
 */
function mapFormFieldToEditableColumn(field, piVar = 'pi') {
  const title = fieldLabelTExpr(field, 'form', piVar);
  const width = 140;
  if (field.readOnly) {
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'readonly',
    width: ${width},
  }`;
  }
  if (field.htmlType === 'textarea') {
    const rows = isFixedLongTextareaField(field) ? 2 : 1;
    const placeholder = field.optional
      ? fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'form', piVar)
      : fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required', 'form', piVar);
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'textarea',
    rows: ${rows},
    placeholder: ${placeholder},
    width: ${width},
  }`;
  }
  if (field.htmlType === 'date') {
    const valueFormat = field.name.toLowerCase().includes('time') ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD';
    const showTime = field.name.toLowerCase().includes('time') ? ', showTime: true' : '';
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'datePicker',
    valueFormat: '${valueFormat}'${showTime},
    width: ${width},
  }`;
  }
  if (field.type === 'number') {
    const summary = field.name === 'lineNumber' ? ", summary: 'sum'" : '';
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'inputNumber',
    width: ${width}${summary},
  }`;
  }
  const optionalParts = [];
  if (field.optional) {
    optionalParts.push('allowClear: true');
    optionalParts.push(`placeholder: ${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'form', piVar)}`);
  } else if (field.name.endsWith('SerialNo') || field.name.endsWith('serialNo')) {
    optionalParts.push('required: true');
    optionalParts.push('unique: true');
  }
  const extra = optionalParts.length ? `, ${optionalParts.join(', ')}` : '';
  return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'input',
    width: ${width}${extra},
  }`;
}

/**
 * 生成 use-{entityKebab}-master-context.ts
 * @param {object} ctx
 * @returns {string}
 */
function generateMasterContextComposable(ctx) {
  const { entityPascal, entityKebab, viewEntityKebab, viewModulePath, comment, modulePath } = ctx;
  const entityCamel = pascalToCamel(entityPascal);
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  return `// ========================================
// 项目名称：节拍数字工厂 · Takt Plat (TDF)
// 命名空间：@/views/${viewModulePath}/composables
// 文件名称：use-${viewEntityKebab}-master-context.ts
// 功能描述：${comment}主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import { inject, provide, type InjectionKey, type Ref, ref } from 'vue'
import type { ${entityPascal} } from '@/types/${modulePath}/${entityKebab}'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ${rowRecordType} = ${entityPascal} | Record<string, unknown>

/** 主表选中行上下文 */
export interface ${entityPascal}MasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<${rowRecordType} | null>
}

const ${entityCamel}MasterContextKey: InjectionKey<${entityPascal}MasterContext> = Symbol('${viewEntityKebab}MasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {${entityPascal}MasterContext} 主表上下文
 */
export function provide${entityPascal}MasterContext(): ${entityPascal}MasterContext {
  const selectedMasterRow = ref<${rowRecordType} | null>(null)
  const ctx: ${entityPascal}MasterContext = { selectedMasterRow }
  provide(${entityCamel}MasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {${entityPascal}MasterContext} 主表上下文
 */
export function use${entityPascal}MasterContext(): ${entityPascal}MasterContext {
  const ctx = inject(${entityCamel}MasterContextKey)
  if (!ctx) {
    throw new Error('use${entityPascal}MasterContext must be used within ${viewEntityKebab} index')
  }
  return ctx
}
`;
}

/**
 * 生成子表独立 CRUD 弹窗表单（右栏明细 / 面板 Modal）
 * @param {object} ctx
 * @param {object} child
 * @returns {string}
 */
function generateChildDetailFormVue(ctx, child) {
  const { modulePath, viewModulePath, comment } = ctx;
  const viewChildKebab = child.viewChildKebab || child.childKebab;
  const childPascal = child.childPascal;
  const childIdField = child.childIdField;
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  const formFields = (child.formFields || []).filter((f) => !f.readOnly);
  const formCodeControlOptions = {
    entityIdField: childIdField,
    colSpanFieldCount: FORM_TAB_FIELDS_PER_TAB,
  };
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const formTemplate = buildGeneratedFormTemplateBody({
    formFields,
    formCodeControlOptions,
    hasMasterDetail: false,
    forceFormTabs: true,
    entityKebab: viewChildKebab,
  });
  const useFormTabs = formTemplate.useFormTabs;
  const formTemplateBody = useFormTabs
    ? formTemplate.body
    : `    <div :class="formContentClass">
${formTemplate.body}
    </div>`;
  const hasScopeContextFields = hasScopeContextFormFields(formFields, []);
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
    const isCreate = !props.formData?.${childIdField}
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)
` : '';
  const masterTypeImport = `import type { ${childPascal}Create } from '@/types/${modulePath}/${child.childKebab}'`;
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields,
    entityIdField: childIdField,
    childFieldStrip: '',
    hasScopeContextFields,
    watchSyncChild: '',
    useBuildSubmitPayload: false,
  });
  const getValuesBody = `${formScriptFragments.getValuesBody.replace(
    '  return payload',
    `  payload.${child.masterFkField} = props.masterId\n  return payload`,
  )}`;
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(childIdField, hasScopeContextFields);
  const needsTaktSelect = formFields.some((f) => f.htmlType === 'select' && f.dictType);
  const taktSelectImport = needsTaktSelect
    ? "import TaktSelect from '@/components/business/takt-select/index.vue'\n"
    : '';
  const extFieldIconImport = buildExtFieldIconImportLine(formFields);
  let formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(formFields.map((f) => f.name)),
    mdScript: '',
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal: childPascal,
    entityIdField: childIdField,
    useFormTabs,
  });
  formScriptState = formScriptState.replace(
    `  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}`,
    `  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}`,
  ).replace(
    `  loading: false,
})`,
    `  loading: false,
  masterId: '',
})`,
  );
  const activeTabReset = useFormTabs ? "  activeTab.value = 'tab-0'\n" : '';
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${viewChildKebab}-form.vue -->
<!-- 功能描述：${comment}子表 ${child.childCamel} 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 ${generatorScript} 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ${viewChildKebab}-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
${formTemplateBody}
  </a-form>
</template>

<script setup lang="ts">
/**
 * ${comment}子表 ${child.childCamel} 维护表单 · 由 ${generatorScript} 生成
 * @module views/${viewModulePath}/components
 */
${formScriptFragments.vueImportLine}
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
${buildEntityI18nFormImportBlock(childPascal, viewChildKebab)}
${masterTypeImport}
${taktSelectImport}${extFieldIconImport}${formScriptFragments.dictImportLine}${scopeStoreImports}
${formScriptState}
${formScriptFragments.defaultsBlock}
${formScriptFragments.normalizerBlock}
${formScriptFragments.dictBootstrap}
${formScriptFragments.watchBlock}
${scopeContextWatch}
/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
${formScriptFragments.requiredRules}
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 ${child.masterFkField}） */
function getValues(): Record<string, any> {
${getValuesBody}
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
${resetScopeDefaultsLine}${activeTabReset}  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
${buildFormTabsScopedStyleBlock(useFormTabs)}
`;
}

/**
 * 子表 buildListQuery（始终带主表外键）
 * @param {string} childPascal
 * @param {string} masterFkField
 * @param {string} entityPascal
 * @param {object[]} queryFields
 * @returns {string}
 */
function buildChildPanelListQueryBlock(childPascal, masterFkField, entityPascal, queryFields) {
  const block = buildServerPagedListQueryBlock(childPascal, queryFields);
  return block.replace(
    '    pageSize: pageSize.value,\n    ...overrides,',
    `    pageSize: pageSize.value,\n    ${masterFkField}: master${entityPascal}Id.value,\n    ...overrides,`,
  );
}

/**
 * 生成子表右栏 CRUD 面板（对齐 logistics/serial/inbound inbound-item-panel）
 * @param {object} ctx
 * @param {object} child
 * @returns {string}
 */
function generateChildDetailPanelVue(ctx, child) {
  const {
    entityPascal,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    comment,
    caps,
    permissionPrefix,
  } = ctx;
  const viewChildKebab = child.viewChildKebab || child.childKebab;
  const masterPerm = String(permissionPrefix || ctx.permissionPrefix || '').replace(/:list$/, '');
  const childCaps = child.childCaps || detectApiCapabilities(child.childPascal, {});
  const queryFields = child.queryFields || [];
  const queryInit = queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const queryFactoryBlock = buildAdvancedQueryFactoryBlock(child.childPascal, queryFields);
  const childI18nPrefix = child.childPascal.toUpperCase();
  const childEntityI18nImport = buildEntityI18nIndexImportBlock(
    child.childPascal,
    viewChildKebab,
    '../composables',
  );
  const queryItems = queryFields.map((f) => renderQueryFormItem(f)).join('\n');
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}-${viewChildKebab}`;
  const entityScope = ctx.fields?.entityScope || 'company';
  const remixIconImport = buildRemixIconImportLine({ includeActionIcons: true, queryFields });
  const listCols = (child.listFields || []).map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ${child.childType} }) =>
      String(get${child.childPascal}Field(record, '${f.name}') ?? ''),
  },`).join('\n');
  const importApiImports = [
    childCaps.apiGetTemplate,
    childCaps.apiImport,
    childCaps.apiExport,
  ].filter(Boolean);
  const baseApiImports = [
    childCaps.apiGetList,
    childCaps.apiGetById,
    childCaps.apiCreate,
    childCaps.apiUpdate,
    childCaps.apiDelete,
    childCaps.apiDeleteBatch,
  ].filter(Boolean);
  const apiImportBlock = [...new Set([...baseApiImports, ...importApiImports])].join(',\n  ');
  const listQueryBlock = buildChildPanelListQueryBlock(
    child.childPascal,
    child.masterFkField,
    entityPascal,
    queryFields,
  );
  const importHandlers = (childCaps.hasImport && childCaps.hasGetTemplate)
    ? buildImportHandlersScriptBlock({
      apiGetTemplate: childCaps.apiGetTemplate,
      apiImport: childCaps.apiImport,
      openHandlerPrefix: `if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }`,
      successBody: 'void loadData()',
    })
    : '';
  const exportHandler = childCaps.hasExport ? `
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
${buildServerPagedExportApiCall(childCaps.apiExport)}
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = \`\${excelNames.fileBase}_\${ts.getFullYear()}\${pad(ts.getMonth() + 1)}\${pad(ts.getDate())}\${pad(ts.getHours())}\${pad(ts.getMinutes())}\${pad(ts.getSeconds())}\`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}` : '';
  const importModalBlock = (childCaps.hasImport && childCaps.hasGetTemplate)
    ? buildImportModalVueBlock(child.childPascal)
    : '';
  const queryDrawerBlock = queryFields.length ? `
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="${queryFieldStorageKey}"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
${queryItems}
      </template>
    </TaktQueryDrawer>` : '';
  const toolsBarImportExport = `
      :show-import="${childCaps.hasImport && childCaps.hasGetTemplate}"
      :show-export="${Boolean(childCaps.hasExport)}"
      :show-advanced-query="${queryFields.length > 0}"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
${childCaps.hasImport && childCaps.hasGetTemplate ? '      @import="handleImport"' : ''}
${childCaps.hasExport ? '      @export="handleExport"' : ''}
${queryFields.length ? `      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"` : '      @column-setting="handleColumnSetting"'}`;
  const excelImportLine = (childCaps.hasImport || childCaps.hasExport)
    ? [
      "import { taktExcelEntityNames } from '@/utils/naming'",
      ...(childCaps.hasExport ? ["import { resolveExportDownloadFileName } from '@/utils/export-download-name'"] : []),
      ...(childCaps.hasImport && childCaps.hasGetTemplate
        ? ["import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'"]
        : []),
    ].join('\n')
    : '';
  const advancedQueryScript = queryFields.length ? `
const advancedQueryVisible = ref(false)
${queryFactoryBlock}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  ${childI18nPrefix}_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}` : '';
  const columnSettingScript = `
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}`;
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${viewChildKebab}-panel.vue -->
<!-- 功能描述：${comment}主表实体右侧明细 ${child.childCamel} 独立 CRUD（按主表选中 ${child.masterFkField} 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="${viewChildKebab}-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ pi.self() }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="${masterPerm}:create"
      update-permission="${masterPerm}:update"
      delete-permission="${masterPerm}:delete"
${childCaps.hasImport ? `      import-permission="${masterPerm}:import"` : ''}
${childCaps.hasExport ? `      export-permission="${masterPerm}:export"` : ''}
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"
${toolsBarImportExport}
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div class="${viewChildKebab}-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="${ctx.fields?.entityScope || 'company'}"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="get${child.childPascal}Id"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="${child.childIdField}"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <${child.childPascal}Form
        ref="formRef"
        :form-data="formData"
        :master-id="master${entityPascal}Id"
        :loading="formLoading"
      />
    </TaktModal>
${queryDrawerBlock}${importModalBlock}
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="${child.childIdField}"
      action-column-key="action"
      entity-scope="${ctx.fields?.entityScope || 'company'}"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}子表 ${child.childCamel} 右栏面板
 * @module views/${viewModulePath}/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
${excelImportLine ? `${excelImportLine}\n` : ''}import { CreateActionColumn } from '@/components/business/takt-action-column/index'
${remixIconImport}import ${child.childPascal}Form from './${viewChildKebab}-form.vue'
import { use${entityPascal}MasterContext } from '../composables/use-${viewEntityKebab}-master-context'
import {
  ${apiImportBlock},
} from '@/api/${modulePath}/${child.childKebab}'
import type { ${child.childType}, ${child.childPascal}Query } from '@/types/${modulePath}/${child.childKebab}'

${childEntityI18nImport}
const { t } = useI18n()
const { selectedMasterRow } = use${entityPascal}MasterContext()

${childCaps.hasImport || childCaps.hasExport ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${child.childEntityClassName || `Takt${child.childPascal}`}')
` : ''}/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
const dataSource = ref<${child.childType}[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<${child.childType} | null>(null)
const selectedRows = ref<${child.childType}[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<${child.childType}>>({})
const formLoading = ref(false)
const formRef = ref()
${advancedQueryScript}${columnSettingScript}
${childCaps.hasImport && childCaps.hasGetTemplate ? 'const importVisible = ref(false)\n' : ''}
const entityIdName = '${child.childIdField}'
const master${entityPascal}Id = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['${caps.entityIdName}']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => master${entityPascal}Id.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function get${child.childPascal}Id(record: ${child.childType} | Record<string, unknown>): string {
  return String((record as ${child.childType})?.[entityIdName] ?? '')
}

function get${child.childPascal}Field(record: ${child.childType} | Record<string, unknown>, field: string): unknown {
  return (record as ${child.childType})?.[field as keyof ${child.childType}]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${child.childIdField}',
    key: '${child.childIdField}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ${child.childType} }) =>
      String(get${child.childPascal}Field(record, '${child.childIdField}') ?? ''),
  },
${listCols}
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${masterPerm}:update',
        onClick: (record: ${child.childType}) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${masterPerm}:delete',
        onClick: (record: ${child.childType}) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${child.childType}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ${child.childType}, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && get${child.childPascal}Id(selectedRow.value) === get${child.childPascal}Id(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${child.childType}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ${child.childType}) {
  const key = get${child.childPascal}Id(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}
${listQueryBlock}
async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await ${childCaps.apiGetList}(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(master${entityPascal}Id, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ${child.childType}) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await ${childCaps.apiGetById}(get${child.childPascal}Id(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
  }
}

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
    const payload = refInst.getValues?.()
    const id = formData.value?.${child.childIdField}
    if (id) {
      await ${childCaps.apiUpdate}(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await ${childCaps.apiCreate}(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: ${child.childType}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${childCaps.apiDelete}(get${child.childPascal}Id(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => get${child.childPascal}Id(r)).filter(Boolean)
      await ${childCaps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}
${importHandlers}${exportHandler}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
`;
}

/**
 * index.vue 左右主子表 script 片段
 * @param {object} ctx
 * @returns {object}
 */
function generateMasterDetailLrIndexScript(ctx) {
  const { entityPascal, caps, fields } = ctx;
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const children = fields.masterDetailChildren || [];
  const panelRefs = children.map((c) => `const ${c.childCamel}PanelRef = ref<InstanceType<typeof ${c.childPascal}Panel> | null>(null)`).join('\n');
  const panelImports = children.map((c) => {
    const viewChildKebab = c.viewChildKebab || c.childKebab;
    return `import ${c.childPascal}Panel from './components/${viewChildKebab}-panel.vue'`;
  }).join('\n');
  const reloadPanels = children.map((c) => `  ${c.childCamel}PanelRef.value?.reload?.()`).join('\n');
  const detailSlot = children.length === 1
    ? `      <template #detail>
        <${children[0].childPascal}Panel
          ref="${children[0].childCamel}PanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>`
    : `      <template #detail>
        <div class="flex min-h-0 flex-1 flex-col gap-3">
${children.map((c) => `          <${c.childPascal}Panel ref="${c.childCamel}PanelRef" class="h-full min-h-0 flex-1" />`).join('\n')}
        </div>
      </template>`;
  const detailFn = caps.hasGetById ? `
/** 加载主表详情并回填当前页 dataSource */
async function load${entityPascal}Detail(record: ${rowRecordType}): Promise<${entityPascal} | null> {
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
  return {
    panelImports,
    composableImport: `import { provide${entityPascal}MasterContext, type ${rowRecordType} } from './composables/use-${ctx.viewEntityKebab}-master-context'`,
    composableSetup: `/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provide${entityPascal}MasterContext()`,
    panelRefs,
    detailSlot,
    detailFn,
    lrScript: `
/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: ${rowRecordType} | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? get${entityPascal}Id(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as ${rowRecordType}
  const key = get${entityPascal}Id(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}
${detailFn}`,
    rowSelectionPatch: `    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }`,
    deleteClearSelection: `      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)`,
    formSubmitReload: children.length ? `
    if (selectedMasterKey.value) {
${reloadPanels}
    }` : '',
  };
}

/**
 * *-form.vue 上主下从（TaktEditableTable）片段
 * @param {object} ctx
 * @returns {{ editableBlocks: string, script: string, tableRefs: string, validateLines: string, resetLines: string }}
 */
function generateMasterDetailEditableFormParts(ctx) {
  const { entityPascal, entityCamel, fields, caps } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length) {
    return { editableBlocks: '', script: '', tableRefs: '', validateLines: '', resetLines: '' };
  }
  const childI18nImportLines = children.map((child) => {
    const viewChildKebab = child.viewChildKebab || child.childKebab;
    const hookName = entityI18nHookName(child.childPascal);
    const composableStem = entityI18nComposableFileName(viewChildKebab).replace(/\.ts$/, '');
    return `import { ${hookName} } from '../composables/${composableStem}'`;
  }).join('\n');
  const childI18nSetupLines = children.map((child) => {
    const hookName = entityI18nHookName(child.childPascal);
    return `const ${child.childCamel}Pi = ${hookName}()`;
  }).join('\n');
  const editableBlocks = children.map((child) => `    <!-- 下：子表 ${child.fieldName} -->
    <TaktEditableTable
      ref="${child.childCamel}TableRef"
      v-model="child${child.childPascal}Rows"
      :columns="${child.childCamel}FormColumns"
      :title="${child.childCamel}Pi.self()"
      :add-button-entity="${child.childCamel}Pi.self()"
      id-field="${child.childIdField}"
      :default-row="createDefault${child.childPascal}Row"
      :disabled="loading"
      section-border
    />`).join('\n');
  const columnDefs = children.map((child) => {
    const cols = child.formFields
      .filter((f) => !f.readOnly || f.name === 'lineNumber')
      .map((f) => mapFormFieldToEditableColumn(f, `${child.childCamel}Pi`))
      .join(',\n');
    return `/** 子表 ${child.childCamel} 可编辑列 */
const ${child.childCamel}FormColumns = computed<TaktEditableTableColumn[]>(() => [
${cols},
])`;
  }).join('\n\n');
  const rowRefs = children.map((child) => `const child${child.childPascal}Rows = ref<Record<string, unknown>[]>([])`).join('\n');
  const tableRefs = children.map((child) => `const ${child.childCamel}TableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)`).join('\n');
  const syncFromForm = children.map((child) => `  child${child.childPascal}Rows.value = ((val as any)?.${child.fieldName} ?? []) as Record<string, unknown>[]`).join('\n');
  const defaultRows = children.map((child) => {
    const defaults = child.formFields
      .filter((f) => !f.readOnly)
      .map((f) => {
        if (f.name === 'lineNumber') {
          return `    lineNumber: (child${child.childPascal}Rows.value.length + 1) * 10,`;
        }
        const val = f.type === 'number' ? '0' : "''";
        return `    ${f.name}: ${val},`;
      })
      .join('\n');
    return `function createDefault${child.childPascal}Row(): Record<string, unknown> {
  return {
${defaults}
  }
}`;
  }).join('\n\n');
  const getValuesMerge = children.map((child) => {
    const itemRowsExpr = `${child.childCamel}TableRef.value?.getRows?.() ?? child${child.childPascal}Rows.value`;
    return `    ${child.fieldName}: ${itemRowsExpr}.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      ${child.masterFkField}: masterId,
    })),`;
  }).join('\n');
  const validateLines = children.map((child) => `  await ${child.childCamel}TableRef.value?.validate?.()`).join('\n');
  const resetLines = children.map((child) => `  ${child.childCamel}TableRef.value?.resetRows?.()`).join('\n');
  const script = `
import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
${childI18nImportLines}

${childI18nSetupLines}

${rowRefs}
${tableRefs}

${columnDefs}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<${entityPascal}Create & { ${entityCamel}Id?: string }> | null | undefined) {
${syncFromForm}
}

${defaultRows}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.${caps.entityIdName} ?? ''
  return {
    ...formState,
${getValuesMerge}
  }
}`;
  return { editableBlocks, script, tableRefs, validateLines, resetLines };
}

/**
 * 写入 composable / 子表面板 / 子表 form
 * @param {object} bundle
 * @param {object} ctx
 * @param {object} options
 */
function writeMasterDetailLayoutOutputs(bundle, ctx, options) {
  const viewDir = path.dirname(bundle.indexPath);
  const composableDir = path.join(viewDir, 'composables');
  const componentsDir = path.join(viewDir, 'components');
  const composablePath = path.join(composableDir, `use-${ctx.viewEntityKebab}-master-context.ts`);
  const composableContent = generateMasterContextComposable(ctx);
  if (options.dryRun) {
    console.log(`🔍 [dry-run] 将生成: ${composablePath}`);
  } else {
    writeGeneratedFile(composablePath, composableContent);
    console.log(`✅ 已生成: ${composablePath}`);
  }
  (ctx.fields.masterDetailChildren || []).forEach((child) => {
    const viewChildKebab = child.viewChildKebab || child.childKebab;
    const panelPath = path.join(componentsDir, `${viewChildKebab}-panel.vue`);
    const childFormPath = path.join(componentsDir, `${viewChildKebab}-form.vue`);
    const childI18nPath = path.join(composableDir, entityI18nComposableFileName(viewChildKebab));
    const childListCols = (child.listFields || []).filter((f) => f.name !== child.childIdField);
    const childI18nContent = buildEntityI18nComposableFile({
      entityPascal: child.childPascal,
      entityI18nSlug: child.childI18nSlug,
      entityKebab: child.childKebab,
      viewModulePath: ctx.viewModulePath,
      viewEntityKebab: viewChildKebab,
      modulePath: ctx.modulePath,
      listFields: childListCols,
      formFields: child.formFields || [],
      queryFields: child.queryFields || [],
      comment: child.childPascal,
    });
    const panelContent = generateChildDetailPanelVue(ctx, child);
    const childFormContent = generateChildDetailFormVue(ctx, child);
    if (options.dryRun) {
      console.log(`🔍 [dry-run] 将生成:\n  - ${childI18nPath}\n  - ${panelPath}\n  - ${childFormPath}`);
      return;
    }
    writeGeneratedFile(childI18nPath, childI18nContent);
    console.log(`✅ 已生成: ${childI18nPath}`);
    writeGeneratedFile(panelPath, panelContent);
    writeGeneratedFile(childFormPath, childFormContent);
    console.log(`✅ 已生成: ${panelPath}`);
    console.log(`✅ 已生成: ${childFormPath}`);
  });
}

function buildMasterDetailIndexStyleBlock() {
  return '';
}

module.exports = {
  generateMasterDetailLrIndexScript,
  generateMasterDetailEditableFormParts,
  writeMasterDetailLayoutOutputs,
  buildMasterDetailIndexStyleBlock,
  generateChildDetailPanelVue,
  generateChildDetailFormVue,
};
