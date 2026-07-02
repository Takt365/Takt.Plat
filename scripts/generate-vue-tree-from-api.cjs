// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：scripts
// 文件名称：generate-vue-tree-from-api.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：树表 Vue 视图生成（ParentId + getXxxTree，左树右表）；单表见 generate-vue-crud-from-api.cjs；主子表见 generate-vue-master-detail-from-api.cjs
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const { findDomainEntityFile } = require('./generate-script-common.cjs');
const { buildTreeIndexStateRefs, buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  FORM_TAB_FIELDS_PER_TAB,
  resolveFormFieldColSpan,
  buildFormRowMarkup,
  buildFormFieldColItems,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderQueryFormItem,
  renderFormControl,
  renderFormItemOpening,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  computeFormTabCount,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  hasScopeContextFormFields,
  pascalToCamel,
  buildMenuIndex,
  buildMasterDetailChildRegistry,
  loadVueModuleContext,
  writeVueModuleOutputs,
  resolveFieldTranslationKey,
  fieldsUseDictSelect,
  buildDictDataStoreImportLine,
  buildDictDataStoreIndexSetup,
  buildGeneratedFormVueScriptFragments,
  buildResetPeriodListMapperScriptBlock,
  buildListDictTagValueExpr,
  resolveListSwitchAndDictColsForIndex,
  buildListSwitchBodyCellLine,
  buildListSwitchHandlersBlock,
  buildEntityNumericCoerceHelper,
  INDEX_FORM_RESET_NEXT_TICK,
  buildFormResetScopeDefaultsBlock,
  buildVueImportResultUtilImportLine,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  buildAdvancedQueryFactoryBlock,
  entityRowRecordTypeName,
} = require('./generate-vue-common.cjs');

/**
 * Domain 实体是否含 ParentId（树形）
 * @param {string} entityPascal
 * @param {string} backendRoot
 * @returns {boolean}
 */
function entityHasParentId(entityPascal, backendRoot) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (!entityFile) {
    return false;
  }
  const content = fs.readFileSync(entityFile, 'utf-8');
  return /public\s+long(?:\?)?\s+ParentId\s*\{/.test(content);
}

/**
 * 扩展 API 能力：树接口
 * @param {string} entityPascal
 * @param {Record<string, string>} methods
 */
function extendTreeApiCapabilities(entityPascal, methods) {
  const names = Object.keys(methods);
  const pick = (...candidates) => candidates.find((c) => names.includes(c)) || '';
  return {
    hasGetTree: Boolean(pick(`get${entityPascal}Tree`)),
    apiGetTree: pick(`get${entityPascal}Tree`),
    hasGetTreeOptions: Boolean(pick(`get${entityPascal}TreeOptions`)),
    apiGetTreeOptions: pick(`get${entityPascal}TreeOptions`),
    entityTreeType: `${entityPascal}Tree`,
  };
}

/**
 * 解析树节点标题字段与右侧关键字搜索字段
 * @param {Map<string, { properties: Array<{ name: string, type: string }> }>} interfaces
 * @param {string} entityPascal
 * @param {string} entityCamel
 * @param {object[]} listFields
 */
function resolveTreeFieldMeta(interfaces, entityPascal, entityCamel, listFields) {
  const props = interfaces.get(entityPascal)?.properties || [];
  const propNames = new Set(props.map((p) => p.name));
  const nameField = `${entityCamel}Name`;
  const codeField = `${entityCamel}Code`;
  let titleField = nameField;
  if (!propNames.has(nameField)) {
    titleField = propNames.has(codeField) ? codeField : (listFields.find(
      (f) => f.type === 'string' && !['parentId', 'sortOrder', 'level'].includes(f.name),
    )?.name || `${entityCamel}Id`);
  }
  const searchFields = [nameField, codeField].filter((f) => propNames.has(f));
  return {
    titleField,
    searchFields: searchFields.length > 0 ? searchFields : [titleField],
  };
}

/**
 * 生成树拖拽更新用的 buildXxxUpdateDto（详情 DTO → Update DTO，对齐 Create 字段）
 * @param {string} entityPascal
 * @param {string} entityCamel
 * @param {string} idField
 * @param {Array<{ name: string }>} createProperties CreateDto 属性（不含审计字段）
 */
function buildTreeUpdateDtoFunction(entityPascal, entityCamel, idField, createProperties) {
  if (!createProperties.length) {
    return '';
  }
  const hasSortOrderInUpdateDto = createProperties.some((p) => p.name === 'sortOrder');
  const overridesType = hasSortOrderInUpdateDto
    ? `Pick<${entityPascal}Update, 'parentId' | 'sortOrder'>`
    : `Pick<${entityPascal}Update, 'parentId'> & { sortOrder: number }`;
  const fieldLines = createProperties.map((p) => {
    if (p.name === 'companyDefaultCulture') {
      return `    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',`;
    }
    if (p.name === 'parentId' || p.name === 'sortOrder') {
      return `    ${p.name}: overrides.${p.name},`;
    }
    return `    ${p.name}: ${entityCamel}.${p.name},`;
  });
  return `/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param ${entityCamel} 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {${entityPascal}Update} 更新载荷
 */
function build${entityPascal}UpdateDto(
  ${entityCamel}: ${entityPascal},
  overrides: ${overridesType},
): ${entityPascal}Update {
  return {
    ${idField}: String(${entityCamel}.${idField}),
${fieldLines.join('\n')}
  }
}`;
}

/**
 * 生成树表 index.vue（左树右表，参照 views/human-resource/organization/dept/index.vue）
 * @param {object} ctx
 * @param {object} helpers fieldLabelTExpr 等由主脚本注入
 */
function generateTreeIndexVue(ctx, helpers) {
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    permissionPrefix,
    cssRootClass,
    apiBase,
    caps,
    fields,
    comment,
    treeMeta,
    updateDtoFields = [],
  } = ctx;
  const { fieldLabelTExpr, fieldPlaceholderTExpr } = helpers;
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const { titleField, searchFields } = treeMeta;
  const entityScope = fields.entityScope || 'company';
  const importApiNames = [
    caps.apiGetTree,
    caps.apiGetById,
    caps.apiCreate,
    caps.apiUpdate,
    caps.apiDelete,
    caps.apiDeleteBatch,
    caps.apiGetTemplate,
    caps.apiImport,
    caps.apiExport,
    caps.apiUpdateStatus,
    caps.apiUpdateBuiltIn,
    caps.apiUpdateSort,
  ].filter(Boolean);
  const listCols = fields.listFields.filter((f) => f.name !== caps.entityIdName && f.name !== 'children');
  const { switchListCols, dictTagListCols } = resolveListSwitchAndDictColsForIndex(
    listCols.filter((f) => f.dictType || f.isListSwitch),
    caps,
  );
  const needsDictInIndex = fieldsUseDictSelect(fields.queryFields)
    || fieldsUseDictSelect(fields.formFields)
    || dictTagListCols.length > 0;
  const indexDictImport = needsDictInIndex ? buildDictDataStoreImportLine() : '';
  const indexDictSetup = needsDictInIndex ? buildDictDataStoreIndexSetup() : '';
  const indexDictOnMounted = needsDictInIndex ? '  void dictDataStore.loadAllDictDataAsync()\n' : '';
  const searchFieldLabelExprs = (treeMeta.searchFields || []).length
    ? (treeMeta.searchFields || []).map((f) => `pi.label('${f}')`).join(', ')
    : 'pi.self()';
  const queryItems = fields.queryFields.map((f) => helpers.renderQueryFormItem(f)).join('\n');
  const queryFactoryBlock = buildAdvancedQueryFactoryBlock(entityPascal, fields.queryFields);
  const advancedQueryResetExpr = fields.queryFields.length
    ? 'createEmptyAdvancedQueryForm()'
    : `{\n${queryInit}\n  }`;
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}`;
  const queryInit = fields.queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const columnBlocks = listCols.map((f) => {
    if (f.name === titleField) {
      return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 160,
    resizable: true,
    ellipsis: true,
  },`;
    }
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
    customRender: ({ record }: { record: Record<string, unknown> }) => get${entityPascal}Field(record, '${f.name}') ?? ''
  },`;
  }).join('\n');
  const resetPeriodListMapperBlock = buildResetPeriodListMapperScriptBlock(dictTagListCols);
  const switchBodyCellExtra = switchListCols
    .filter((f) => f.name !== titleField)
    .map((f, i) => {
      const branch = titleField ? 'v-else-if' : (i === 0 ? 'v-if' : 'v-else-if');
      return buildListSwitchBodyCellLine(f, entityPascal, branch);
    })
    .join('\n');
  const dictBodyCellExtra = dictTagListCols.filter((f) => f.name !== titleField).map((f, i) => {
    const priorCount = (titleField ? 1 : 0) + switchListCols.filter((sf) => sf.name !== titleField).length;
    const branch = priorCount === 0 && i === 0 ? 'v-if' : 'v-else-if';
    const valueExpr = f.name === 'resetPeriod'
      ? `mapResetPeriodDictValue(get${entityPascal}DictValue(record, '${f.name}') as string | number | undefined)`
      : `get${entityPascal}DictValue(record, '${f.name}')`;
    return `          <template ${branch}="column.key === '${f.name}'">
            <TaktDictTag
              :value="${valueExpr}"
              dict-type="${f.dictType}"
            />
          </template>`;
  }).join('\n');
  const listSwitchHandlersBlock = buildListSwitchHandlersBlock(
    switchListCols,
    entityPascal,
    caps,
    { reloadAfterSuccess: true, recordType: `${entityPascal}RowRecord` },
  );
  const titleBodyCell = titleField ? `          <template v-if="column.key === '${titleField}'">
            <span
              class="inline-block"
              :style="{ paddingLeft: \`\${(record._treeDepth ?? 0) * 16}px\` }"
            >
              {{ get${entityPascal}Field(record, '${titleField}') }}
            </span>
          </template>` : '';
  const bodyCellBlock = (titleField || switchListCols.length || dictTagListCols.length)
    ? `        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
${titleBodyCell}
${switchBodyCellExtra}
${dictBodyCellExtra}
        </template>`
    : '';
  const idField = caps.entityIdName;
  const actionItems = [];
  if (caps.hasUpdate) {
    actionItems.push(`      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${permissionPrefix}:update',
        onClick: (record: ${rowRecordType}) => handleEdit(record)
      },`);
  }
  if (caps.hasDelete) {
    actionItems.push(`      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${permissionPrefix}:delete',
        onClick: (record: ${rowRecordType}) => handleDeleteOne(record)
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
        :key="formData?.${idField} ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>` : '';
  const importBlock = (caps.hasImport && caps.hasGetTemplate)
    ? buildImportModalVueBlock(entityPascal)
    : '';
  const entityI18nIndexImport = buildEntityI18nIndexImportBlock(entityPascal, viewEntityKebab, './composables', {
    includeListFields: false,
  });
  const needsUserStoreForTreeDrop = caps.hasUpdate && updateDtoFields.some((p) => p.name === 'companyDefaultCulture');
  const hasSortOrderInUpdateDto = updateDtoFields.some((p) => p.name === 'sortOrder');
  const treeUpdateDtoBlock = caps.hasUpdate
    ? buildTreeUpdateDtoFunction(entityPascal, entityCamel, idField, updateDtoFields)
    : '';
  const treeStateBlock = buildTreeIndexStateRefs(entityPascal, {
    hasForm: caps.hasCreate || caps.hasUpdate,
    hasImport: caps.hasImport && caps.hasGetTemplate,
    idField,
    titleField,
    queryInit,
    queryFactoryBlock,
    searchFieldLabelExprs,
    needsUserStore: needsUserStoreForTreeDrop,
    needsExcelNames: caps.hasImport || caps.hasExport,
    entityClassName: caps.entityClassName,
    entityPascal,
  });
  const remixIconImport = buildRemixIconImportLine({
    includeActionIcons: caps.hasUpdate || caps.hasDelete,
    queryFields: fields.queryFields,
  });
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：${comment}树表管理页（左树右表），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="${cssRootClass}">
    <!-- 查询栏 -->
    <div class="${cssRootClass}-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        :placeholder="tableSearchPlaceholder"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <!-- 工具栏 -->
    <div class="${cssRootClass}-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFull${entityPascal}Tree"
      />
      <TaktTreeRightToolsBar
${caps.hasCreate ? `        create-permission="${permissionPrefix}:create"` : ''}
${caps.hasUpdate ? `        update-permission="${permissionPrefix}:update"` : ''}
${caps.hasDelete ? `        delete-permission="${permissionPrefix}:delete"` : ''}
${caps.hasImport ? `        import-permission="${permissionPrefix}:import"` : ''}
${caps.hasExport ? `        export-permission="${permissionPrefix}:export"` : ''}
        :show-create="${caps.hasCreate}"
        :show-update="${caps.hasUpdate}"
        :show-delete="${caps.hasDelete || caps.hasDeleteBatch}"
        :show-import="${caps.hasImport && caps.hasGetTemplate}"
        :show-export="${caps.hasExport}"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :show-expand="true"
        :update-disabled="!selectedRow"
        :delete-disabled="!selectedRow && selectedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        :expanded="tableExpanded"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
${caps.hasImport && caps.hasGetTemplate ? '        @import="handleImport"' : ''}
${caps.hasExport ? '        @export="handleExport"' : ''}
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 左树右表 -->
    <div class="${cssRootClass}-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="false"
        :draggable="${caps.hasUpdate}"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="${entityScope}"
        v-model:current="tableCurrentPage"
        v-model:page-size="tablePageSize"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'${idField}'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="paginatedFlatTableRows"
        :loading="loading"
        :row-key="get${entityPascal}Id"
        :stripe="true"
        :row-selection="rowSelection"
        :show-pagination="true"
        :total="tableFlatTotal"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
${bodyCellBlock}
      </TaktTreeRightTable>
    </div>
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
      entity-scope="${entityScope}"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'${idField}'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}树表管理页 · ParentId 左树右表（参照 dept/index.vue）
 * @module views/${viewModulePath}
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
${(caps.hasCreate || caps.hasUpdate) ? `import ${entityPascal}Form from './components/${viewEntityKebab}-form.vue'\n` : ''}import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${entityKebab}'
import type { ${entityPascal}, ${caps.entityTreeType}, ${entityPascal}Update } from '@/types/${modulePath}/${entityKebab}'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
${indexDictImport}${(caps.hasImport || caps.hasExport) ? "import { taktExcelEntityNames } from '@/utils/naming'\n" : ''}${caps.hasExport ? "import { resolveExportDownloadFileName } from '@/utils/export-download-name'\n" : ''}${(caps.hasImport && caps.hasGetTemplate) ? buildVueImportResultUtilImportLine() : ''}${remixIconImport}${needsUserStoreForTreeDrop ? "import { useUserStore } from '@/stores/identity/user'\n" : ''}
${entityI18nIndexImport}
${treeStateBlock}
${indexDictSetup}
/** 解析树节点 key（与列表 ${idField}、左侧树 key 一致） */
function resolve${entityPascal}NodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.${idField} ?? node.id
  return raw == null ? '' : String(raw)
}

/**
 * 将接口树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 实体树 DTO 列表
 */
function ${entityCamel}TreeToTableNodes(nodes: ${caps.entityTreeType}[]): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? ${entityCamel}TreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.${idField} ?? ''),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

/** 将 fullTableTree 转为左侧 a-tree（与右侧表共用 key，保证点选联动） */
function mapFullTableTreeToTreeData(nodes: Array<Record<string, unknown>>): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes.map((n) => {
    const title = String(n[treeTitleField] ?? n.title ?? '')
    const key = resolve${entityPascal}NodeKey(n)
    const children = n.children as Array<Record<string, unknown>> | undefined
    if (!children?.length) return { title, key }
    const mapped = mapFullTableTreeToTreeData(children)
    return mapped.length > 0 ? { title, key, children: mapped } : { title, key }
  })
}

/**
 * 按 key 查找树节点（左侧树与右侧表共用 fullTableTree）
 * @param nodes 树节点列表
 * @param key 节点 key
 */
function findTreeNodeByKey(
  nodes: Array<Record<string, unknown>>,
  key: string | number,
): Record<string, unknown> | null {
  const k = String(key)
  for (const node of nodes) {
    if (resolve${entityPascal}NodeKey(node) === k) return node
    const children = node.children as Array<Record<string, unknown>> | undefined
    if (children?.length) {
      const found = findTreeNodeByKey(children, key)
      if (found) return found
    }
  }
  return null
}

/** 从树中取以某 key 为根的子树（返回单元素数组，便于作为表格根） */
function getSubtree(nodes: Array<Record<string, unknown>>, key: string | number): Array<Record<string, unknown>> {
  const node = findTreeNodeByKey(nodes, key)
  return node ? [node] : []
}

/**
 * 按关键字过滤左侧树：保留 title 匹配的节点及其祖先、子孙
 * @param nodes 树节点
 * @param keyword 关键字
 */
function filterTreeByKeyword(nodes: TreeDataItem[], keyword: string): TreeDataItem[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  /** 递归过滤子树 */
  function filter(nodes: TreeDataItem[]): TreeDataItem[] {
    if (!nodes?.length) return []
    return nodes
      .map((node) => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren } as TreeDataItem
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TreeDataItem
        }
        return null
      })
      .filter(Boolean) as TreeDataItem[]
  }
  return filter(nodes)
}

/** 左侧树绑定数据（按 treeQueryKeyword 客户端过滤） */
const filteredTreeData = computed(() =>
  filterTreeByKeyword(entityTreeData.value, treeQueryKeyword.value)
)

/** 从树数据中收集所有有子节点的 key（用于左侧树展开全部） */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  if (!nodes?.length) return []
  const keys: (string | number)[] = []
  for (const node of nodes) {
    const rawKey = node.key ?? node.${idField} ?? node.id
    if (rawKey == null) continue
    const key: string | number =
      typeof rawKey === 'string' || typeof rawKey === 'number' ? rawKey : String(rawKey)
    const children = (node.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length > 0) {
      keys.push(key)
      keys.push(...collectTreeExpandableKeys(children))
    }
  }
  return keys
}

/**
 * 深度优先拍平树表行（附带 _treeDepth 供缩进列渲染）
 * @param nodes 树表节点
 * @param depth 当前层级
 */
function flatten${entityPascal}TableRows(nodes: Array<Record<string, unknown>>, depth = 0): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  const rows: Array<Record<string, unknown>> = []
  for (const node of nodes) {
    const childList = node.children as Array<Record<string, unknown>> | undefined
    const { children: _children, ...rest } = node
    rows.push({ ...rest, _treeDepth: depth })
    if (childList?.length) {
      rows.push(...flatten${entityPascal}TableRows(childList, depth + 1))
    }
  }
  return rows
}

/** 右侧树表数据：选中左侧节点时显示该节点（含子级）；未选中时显示整棵树 */
const tableTreeData = computed(() => {
  const tree = fullTableTree.value
  if (!tree?.length) return []
  const keys = selectedTreeKeys.value
  if (keys.length > 0) {
    const activeKey = keys[keys.length - 1]
    if (activeKey === undefined) return tree
    const sub = getSubtree(tree, activeKey)
    if (sub.length > 0) return sub
  }
  return tree
})

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matches${entityPascal}RightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
${searchFields.map((f) => `    if (!String(record.${f} ?? '').toLowerCase().includes(k)) return false`).join('\n')}
  }
${fields.queryFields.map((f) => {
    if (f.type === 'number') {
      return `  if (advancedQueryForm.value.${f.name} !== undefined && record.${f.name} !== advancedQueryForm.value.${f.name}) return false`;
    }
    return `  if (advancedQueryForm.value.${f.name} && !String(record.${f.name} ?? '').includes(String(advancedQueryForm.value.${f.name}))) return false`;
  }).join('\n')}
  return true
}

/** 右侧拍平后的全部行（先左侧子树，再右侧查询过滤） */
const tableFlatRows = computed(() =>
  flatten${entityPascal}TableRows(tableTreeData.value).filter(matches${entityPascal}RightQuery)
)
/** 右侧拍平总行数（分页 total） */
const tableFlatTotal = computed(() => tableFlatRows.value.length)
/** 当前页行数据 */
const paginatedFlatTableRows = computed(() => {
  const start = (tableCurrentPage.value - 1) * tablePageSize.value
  return tableFlatRows.value.slice(start, start + tablePageSize.value)
})

/** 左侧选中节点或查询变化时，右侧拍平列表重置到第一页 */
watch(tableTreeData, () => {
  tableCurrentPage.value = getTaktDefaultPageIndex()
})

/** 左侧树选中：重置右侧分页到第一页 */
const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

${treeUpdateDtoBlock ? `${treeUpdateDtoBlock}\n\n` : ''}/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.${idField} ?? '')
    if (k === keyStr) {
      return { parentId: parentKey, sortOrder: i }
    }
    const children = (node?.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length) {
      const found = findParentAndOrderNum(children, targetKey, k)
      if (found) return found
    }
  }
  return null
}

${caps.hasUpdate ? `/**
 * 左侧树拖拽完成后更新 parentId 与 sortOrder
 * @param payload 新树数据与被拖拽节点 key
 */
const handleTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    entityTreeData.value = newTreeData
    const full = await ${caps.apiGetById}(String(dragKey))
    await ${caps.apiUpdate}(String(dragKey), build${entityPascal}UpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
${!hasSortOrderInUpdateDto && caps.apiUpdateSort ? `    await ${caps.apiUpdateSort}({ ${idField}: String(dragKey), sortOrder: pos.sortOrder })` : ''}
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await loadFull${entityPascal}Tree().catch(() => undefined)
  } finally {
    loading.value = false
  }
}` : 'const handleTreeDrop = async () => {}'}

/** 左侧树关键字搜索（客户端过滤，不重复请求接口） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 左侧展开/收缩：工具栏展开状态与树展开 key 联动 */
watch(treeExpanded, (expanded) => {
  if (expanded) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  } else {
    treeExpandedKeys.value = []
  }
})

/** 过滤后的左侧树变化且处于展开态时，同步 expandable keys */
watch(filteredTreeData, () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
})

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type ${entityPascal}RowRecord = ${entityPascal} | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const get${entityPascal}Id = (record: ${entityPascal}RowRecord): string => {
  if (record != null && '${idField}' in record && (record as Record<string, unknown>).${idField} != null) {
    return String((record as Record<string, unknown>).${idField})
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const get${entityPascal}Field = (record: ${entityPascal}RowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const get${entityPascal}DictValue = (
  record: ${entityPascal}RowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
${switchListCols.length > 0 ? buildEntityNumericCoerceHelper(entityPascal) : ''}
${resetPeriodListMapperBlock}

/** 从异常对象提取用户可见消息 */
const getErrorMessage = (error: unknown, fallback: string): string => {
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const messageText = (error as { message?: unknown }).message
    if (typeof messageText === 'string' && messageText.trim()) return messageText
  }
  return fallback
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = ref<TableColumnsType>([])
watchEffect(() => {
  columns.value = [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${idField}',
    key: '${idField}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      get${entityPascal}Field(record, '${idField}') ?? get${entityPascal}Field(record, 'id') ?? '',
  },
${columnBlocks}
  CreateActionColumn<${entityPascal}>({
    actions: [
${actionItems.join('\n')}
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${entityPascal}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: ${rowRecordType}, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && get${entityPascal}Id(selectedRow.value) === get${entityPascal}Id(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${entityPascal}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  },
}))

/** 加载全量树（左侧树 + 右侧树表共用数据源） */
async function loadFull${entityPascal}Tree() {
  const res = await ${caps.apiGetTree}('0', true)
  const resAny = res as { data?: ${caps.entityTreeType}[]; Data?: ${caps.entityTreeType}[] }
  const trees: ${caps.entityTreeType}[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = ${entityCamel}TreeToTableNodes(trees)
  fullTableTree.value = tableNodes
  entityTreeData.value = mapFullTableTreeToTreeData(tableNodes)
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 初始化或增删改后刷新全量树 */
async function loadData() {
  loading.value = true
  try {
    await loadFull${entityPascal}Tree()
  } catch (error: unknown) {
    logger.error('[${entityPascal}] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    fullTableTree.value = []
    entityTreeData.value = []
  } finally {
    loading.value = false
  }
}

/** 右侧查询（客户端过滤，不请求接口） */
const handleSearch = () => {
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/** 右侧重置（不影响左侧树与 fullTableTree） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = ${advancedQueryResetExpr}
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

${listSwitchHandlersBlock}

${caps.hasCreate ? `/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  const keys = selectedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true${INDEX_FORM_RESET_NEXT_TICK}
}` : ''}

${caps.hasUpdate ? `/** 打开编辑弹窗 */
function handleEdit(record: ${rowRecordType}) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}` : ''}

${(caps.hasCreate || caps.hasUpdate) ? `/** 提交新增/编辑表单 */
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
${caps.hasUpdate ? `      await ${caps.apiUpdate}(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))` : ''}
    } else {
${caps.hasCreate ? `      await ${caps.apiCreate}(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))` : ''}
    }
    formVisible.value = false
    formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
    await loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
}` : ''}

${caps.hasDelete ? `/** 删除单行 */
async function handleDeleteOne(record: ${rowRecordType}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${caps.apiDelete}((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    }
  })
}` : ''}

${caps.hasDeleteBatch ? `/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await ${caps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    }
  })
}` : ''}

${(caps.hasImport && caps.hasGetTemplate) ? buildImportHandlersScriptBlock({
  apiGetTemplate: caps.apiGetTemplate,
  apiImport: caps.apiImport,
  successBody: 'void loadData()',
}) : ''}

${caps.hasExport ? `/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await ${caps.apiExport}({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    logger.error('[${entityPascal}] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: pi.self() })))
  } finally {
    loading.value = false
  }
}` : ''}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置右侧分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/** 重置高级查询表单（不自动查询） */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = ${advancedQueryResetExpr}
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

/** 刷新树数据 */
function handleRefresh() {
  void loadData()
}

/** 表格 change / 列宽拖拽占位（树表分页在 TaktTreeRightTable 内） */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}

/** 页面挂载：租户上下文就绪后加载分页配置，再拉树数据 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
${indexDictOnMounted}  void loadData()
})
</script>

<style scoped lang="css">
.${cssRootClass} {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.${cssRootClass}-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.${cssRootClass}-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.${cssRootClass}-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
`;
}

function generateTreeFormBase(ctx) {
  const { entityPascal, entityCamel, entityKebab, viewEntityKebab, modulePath, viewModulePath, fields, comment } = ctx;
  const generatorScript = 'generate-vue-tree-from-api.cjs';
  const mdFormParts = { tabs: '', script: '', needsTaktSelect: false };
  const hasMasterDetail = false;
  const formFields = fields.formFields;
  const treeFormFields = formFields.filter((f) => f.name !== 'parentId');
  const entityIdField = `${entityCamel}Id`;
  const formCodeControlOptions = { entityIdField };
  const tabCount = computeFormTabCount(treeFormFields.length);
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const tabs = [];
  for (let tabIndex = 1; tabIndex <= tabCount; tabIndex += 1) {
    const start = (tabIndex - 1) * FORM_TAB_FIELDS_PER_TAB;
    const end = tabIndex * FORM_TAB_FIELDS_PER_TAB;
    const tabFields = treeFormFields.slice(start, end);
    const rowBlock = buildFormRowMarkup(buildFormFieldColItems(tabFields, formCodeControlOptions), '        ');
    const tabLabel = buildFormTabLabelAttr(tabIndex, tabCount);
    const tabComment = tabIndex === 1 && hasMasterDetail ? '      <!-- 主表 -->\n' : '';
    tabs.push(`${tabComment}      <a-tab-pane
        key="tab-${tabIndex - 1}"
        ${tabLabel}
        force-render
      >
        <div :class="formContentClass">
${rowBlock}
        </div>
      </a-tab-pane>`);
  }
  const needsTaktSelect = treeFormFields.some((f) => f.htmlType === 'select' && f.dictType) || mdFormParts.needsTaktSelect;
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasScopeContextFields = hasScopeContextFormFields(treeFormFields, masterDetailChildren);
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
  const taktSelectImport = needsTaktSelect
    ? "import TaktSelect from '@/components/business/takt-select/index.vue'\n"
    : '';
  const extFieldIconImport = buildExtFieldIconImportLine(treeFormFields);
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields: treeFormFields,
    entityIdField,
    childFieldStrip,
    hasScopeContextFields,
    watchSyncChild,
    useBuildSubmitPayload: hasMasterDetail,
  });
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(entityIdField, hasScopeContextFields);
  const getValuesBody = formScriptFragments.getValuesBody;
  const formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(treeFormFields.map((f) => f.name)),
    mdScript: mdFormParts.script,
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal,
    entityIdField,
  });
  return `<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${viewEntityKebab}-form.vue -->
<!-- 功能描述：${comment}维护弹窗内嵌表单。由 ${generatorScript} 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="${viewEntityKebab}-form-tabs"
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
${formScriptFragments.vueImportLine}
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
${buildEntityI18nFormImportBlock(entityPascal, viewEntityKebab)}
import type { ${typeImportLine} } from '@/types/${modulePath}/${entityKebab}'
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
${getValuesBody}
}

/** 重置表单与子表行 */
/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
${resetScopeDefaultsLine}${resetChildRows}
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
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
 * 树表表单 parentId 默认根节点 0（无树数据 / tree-options 为空时，参照 dept-form.vue）
 * @param {string} content
 * @returns {string}
 */
function patchTreeFormParentIdHandling(content) {
  if (content.includes('function normalizeTreeParentId')) {
    let next = content;
    if (/const FORM_FIELD_DEFAULTS:/.test(next) && !/parentId:\s*'0'/.test(next)) {
      next = next.replace(
        /const FORM_FIELD_DEFAULTS: Record<string, string \| number> = \{\n/,
        "const FORM_FIELD_DEFAULTS: Record<string, string | number> = {\n  parentId: '0',\n",
      );
    }
    if (!/normalizeTreeParentId\(target\)/.test(next)) {
      next = next.replace(
        'function applyFormDefaults(target: Record<string, unknown>) {\n  Object.assign(target, FORM_FIELD_DEFAULTS)\n}',
        `function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
      );
    }
    return next;
  }
  const normalizeHelper = `
/** 树表 parentId：空值归一为根节点 0（string，与后端 ParentId=0 一致） */
function normalizeTreeParentId(target: Record<string, unknown>) {
  const raw = target.parentId
  target.parentId = raw === '' || raw === undefined || raw === null ? '0' : String(raw)
}
`;
  let next = content.replace(
    'const formState = reactive<Record<string, any>>({})',
    "const formState = reactive<Record<string, any>>({ parentId: '0' })",
  );
  if (/const FORM_FIELD_DEFAULTS:/.test(next) && !/parentId:\s*'0'/.test(next)) {
    next = next.replace(
      /const FORM_FIELD_DEFAULTS: Record<string, string \| number> = \{\n/,
      "const FORM_FIELD_DEFAULTS: Record<string, string | number> = {\n  parentId: '0',\n",
    );
  }
  if (/\/\*\* 表单字段默认值（无字典默认项） \*\//.test(next)) {
    next = next.replace(
      /\/\*\* 表单字段默认值（无字典默认项） \*\/\nfunction applyFormDefaults\(target: Record<string, unknown>\) \{\n  void target\n\}/,
      `/** 表单字段默认值（树表 parentId 根节点） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  parentId: '0',
}
${normalizeHelper}
/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
    );
  } else {
    if (!next.includes('function normalizeTreeParentId')) {
      next = next.replace(
        /\/\*\* 写入表单默认值/,
        `${normalizeHelper}/** 写入表单默认值`,
      );
    }
    next = next.replace(
      'function applyFormDefaults(target: Record<string, unknown>) {\n  Object.assign(target, FORM_FIELD_DEFAULTS)\n}',
      `function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
    );
  }
  if (!next.includes('payload.parentId = parentId')) {
    next = next.replace(
      /  if \('sortOrder' in payload\) delete payload\.sortOrder\n  return payload/,
      `  const parentRaw = payload.parentId
  const parentId = parentRaw === '' || parentRaw === undefined || parentRaw === null ? '0' : String(parentRaw)
  payload.parentId = parentId
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload`,
    );
  }
  next = next.replace(
    '      Object.assign(formState, next)\n      formRef.value?.clearValidate()',
    `      Object.assign(formState, next)
      normalizeTreeParentId(formState)
      formRef.value?.clearValidate()`,
  );
  return next;
}

/**
 * 生成树表 *-form.vue（含 TaktTreeSelect parentId，参照 dept-form.vue）
 * @param {object} ctx
 * @param {object} helpers
 */
function generateTreeFormVue(ctx, helpers) {
  const base = generateTreeFormBase(ctx);
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    apiBase,
    caps,
    fields,
    comment,
  } = ctx;
  const { fieldLabelTExpr, fieldPlaceholderTExpr, renderFormControl, renderFormItemOpening, buildFormFieldColItems } = helpers;
  const formFields = fields.formFields.filter((f) => f.name !== 'parentId');
  const parentIdField = fields.formFields.find((f) => f.name === 'parentId')
    || { name: 'parentId', i18nKey: resolveFieldTranslationKey('parentId', entityI18nSlug), optional: false };
  const entityIdField = `${entityCamel}Id`;
  const treeOptionsUrl = `${apiBase}/tree-options`;
  const parentIdRow = `            <a-col :span="24">
              <a-form-item
                :label="${fieldLabelTExpr(parentIdField)}"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="${treeOptionsUrl}"
                  :placeholder="${fieldPlaceholderTExpr(parentIdField, 'common.page.form.placeholder.select')}"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>`;
  let content = base.replace(
    "import { useI18n } from 'vue-i18n'",
    "import { useI18n } from 'vue-i18n'\nimport TaktTreeSelect from '@/components/business/takt-tree-select/index.vue'",
  );
  content = content.replace(
    /功能描述：.*由 generate-vue-(?:crud|tree|master-detail)-from-api/,
    `功能描述：${comment}树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成`,
  );
  content = content.replace(
    /\*\s+\$\{comment\}维护表单/,
    `* ${comment}树表维护表单`,
  );
  const tabCount = helpers.computeFormTabCount(formFields.length);
  const tabs = [];
  for (let tabIndex = 1; tabIndex <= tabCount; tabIndex += 1) {
    const start = (tabIndex - 1) * helpers.FORM_TAB_FIELDS_PER_TAB;
    const end = tabIndex * helpers.FORM_TAB_FIELDS_PER_TAB;
    const tabFields = formFields.slice(start, end);
    const formCodeControlOptions = { entityIdField };
    const items = buildFormFieldColItems(tabFields, formCodeControlOptions);
    const tabLabel = helpers.buildFormTabLabelAttr(tabIndex, tabCount);
    const parentInsert = tabIndex === 1 ? `\n          <a-row :gutter="24">\n${parentIdRow}\n          </a-row>\n` : '';
    tabs.push(`      <a-tab-pane
        key="tab-${tabIndex - 1}"
        ${tabLabel}
        force-render
      >
        <div :class="formContentClass">
${parentInsert}          <a-row :gutter="24">
${items}
          </a-row>
        </div>
      </a-tab-pane>`);
  }
  content = content.replace(
    /<a-tabs[\s\S]*?<\/a-tabs>/,
    `<a-tabs
      v-model:active-key="activeTab"
      class="${viewEntityKebab}-form-tabs"
    >
${tabs.join('\n')}
    </a-tabs>`,
  );
  if (!formFields.some((f) => f.name === 'parentId')) {
    const rulesInsert = `  parentId: [
    {
      required: true,
      message: ${fieldPlaceholderTExpr(parentIdField, 'common.page.form.placeholder.select')},
      trigger: 'change'
    }
  ],`;
    content = content.replace(
      'const rules = computed<Record<string, Rule[]>>(() => ({',
      `const rules = computed<Record<string, Rule[]>>(() => ({\n${rulesInsert}`,
    );
  }
  return patchTreeFormParentIdHandling(content);
}

/**
 * 树表 API 模块处理
 * @param {string} apiFilePath
 * @param {object} options
 * @param {Map} registry
 */
function processTreeApiModule(apiFilePath, options, registry) {
  const bundle = loadVueModuleContext(apiFilePath, options, registry);
  if (bundle.skipped) {
    return bundle;
  }
  if (!bundle.isTreeEntity) {
    console.log(`⏭️  跳过（非树表：须 ParentId + getXxxTree）: ${bundle.rel}`);
    return { skipped: true };
  }
  if (!bundle.capsMerged.hasGetTree && !bundle.capsMerged.hasCreate && !bundle.capsMerged.hasUpdate) {
    console.warn(`⚠️  缺少树表 API，跳过: ${bundle.rel}`);
    return { skipped: true };
  }
  console.log(`  树表: ${bundle.fullCtx.caps.apiGetTree}（左树右表，参照 dept/index.vue）`);
  console.log(`  entityScope: ${bundle.fullCtx.fields.entityScope} ← Takt${bundle.entityShort}`);
  const treeFieldMetaRaw = resolveTreeFieldMeta(
    bundle.ifaceMap,
    bundle.entityShort,
    bundle.fullCtx.entityCamel,
    bundle.fullCtx.fields.listFields,
  );
  const treeMeta = {
    ...treeFieldMetaRaw,
  };
  const vueHelpers = {
    fieldLabelTExpr,
    fieldPlaceholderTExpr,
    renderQueryFormItem,
    renderFormControl,
    renderFormItemOpening,
    buildFormFieldColItems,
    computeFormTabCount,
    buildFormTabLabelAttr,
    FORM_TAB_FIELDS_PER_TAB,
  };
  const fullCtx = { ...bundle.fullCtx, treeMeta };
  const indexContent = generateTreeIndexVue(fullCtx, vueHelpers);
  const formContent = bundle.needsForm ? generateTreeFormVue(fullCtx, vueHelpers) : '';
  const listCols = fullCtx.fields.listFields.filter(
    (f) => f.name !== fullCtx.caps.entityIdName && f.name !== 'children',
  );
  const i18nComposableContent = buildEntityI18nComposableFile({
    entityPascal: fullCtx.entityPascal,
    entityI18nSlug: fullCtx.entityI18nSlug,
    entityKebab: fullCtx.entityKebab,
    viewModulePath: fullCtx.viewModulePath,
    viewEntityKebab: fullCtx.viewEntityKebab,
    modulePath: fullCtx.modulePath,
    listFields: listCols,
    formFields: fullCtx.fields.formFields,
    queryFields: fullCtx.fields.queryFields,
    comment: fullCtx.comment,
  });
  return writeVueModuleOutputs(bundle, indexContent, formContent, options, i18nComposableContent);
}

module.exports = {
  entityHasParentId,
  extendTreeApiCapabilities,
  resolveTreeFieldMeta,
  generateTreeIndexVue,
  generateTreeFormVue,
  patchTreeFormParentIdHandling,
  processTreeApiModule,
};

const { runVueGeneratorCli, VUE_TEMPLATE } = require('./generate-vue-common.cjs');


function printTreeUsage() {
  console.log(`
用法: node scripts/generate-vue-tree-from-api.cjs [参数]

模板: **树表 TREE**（ParentId + getXxxTree，左树右表 + TaktTreeSelect）

参数:
  --<实体名>            如 --CostCenter、--Dept（不带 Takt 前缀；Dept 为手工页，仍会跳过排除列表）
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 单表 CRUD → generate-vue-crud-from-api.cjs
  - 主子表 → generate-vue-master-detail-from-api.cjs
  - 一键 → generate-vue-all-from-api.cjs

示例:
  node scripts/generate-vue-tree-from-api.cjs --CostCenter
`);
}

if (require.main === module) {
  runVueGeneratorCli({
    banner: '🚀 开始生成树表 Vue 视图（generate-vue-tree-from-api.cjs）...\n',
    printUsage: printTreeUsage,
    templateType: VUE_TEMPLATE.TREE,
    buildRegistry: buildMasterDetailChildRegistry,
    onInit: buildMenuIndex,
    processModule: processTreeApiModule,
  });
}
