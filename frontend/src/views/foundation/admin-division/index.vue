<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/admin-division -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：行政区划实体树表管理页（左树懒加载+virtual，右表 list 分页），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-admin-division">
    <!-- 查询栏 -->
    <div class="foundation-admin-division-query-row">
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
    <div class="foundation-admin-division-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="reloadLeftTreeRoots"
      />
      <TaktTreeRightToolsBar
        create-permission="foundation:admin:division:create"
        update-permission="foundation:admin:division:update"
        delete-permission="foundation:admin:division:delete"
        import-permission="foundation:admin:division:import"
        export-permission="foundation:admin:division:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-import="true"
        :show-export="true"
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
        @import="handleImport"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 左树右表（大数据：左侧懒加载+virtual，右侧服务端分页） -->
    <div class="foundation-admin-division-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        :load-data="handleLeftTreeLoadData"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="tenant"
        v-model:current="tableCurrentPage"
        v-model:page-size="tablePageSize"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'adminDivisionId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableDataSource"
        :loading="listLoading"
        :row-key="getAdminDivisionId"
        :stripe="true"
        :row-selection="rowSelection"
        :show-pagination="true"
        :total="tableTotal"
        :virtual="true"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'adminDivisionId'">
            <span>{{ getAdminDivisionField(record, 'adminDivisionId') }}</span>
          </template>
        <template v-else-if="column.key === 'divisionStatus'">
          <a-switch
            :checked="getAdminDivisionDictValue(record, 'divisionStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleDivisionStatusChange(record, Boolean(checked))"
          />
        </template>
          <template v-else-if="column.key === 'countryCode'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'countryCode')"
              dict-type="sys_country_code"
            />
          </template>
          <template v-else-if="column.key === 'level'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'level')"
              dict-type="sys_admin_division_level_type"
            />
          </template>
          <template v-else-if="column.key === 'isLeaf'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'isLeaf')"
              dict-type="sys_yes_no_type"
            />
          </template>
          <template v-else-if="column.key === 'cultureCode'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'cultureCode')"
              dict-type="sys_culture_code"
            />
          </template>
          <template v-else-if="column.key === 'currencyCode'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'currencyCode')"
              dict-type="accounting_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'isBuiltIn'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'isBuiltIn')"
              dict-type="sys_yes_no_type"
            />
          </template>
        </template>
      </TaktTreeRightTable>
    </div>

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
      <AdminDivisionForm
        :key="formData?.adminDivisionId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-foundation-admin-division'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('countryCode')">
      <a-form-item :label="pi.queryLabel('countryCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.countryCode"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('countryCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionCode')">
      <a-form-item :label="pi.queryLabel('divisionCode')">
        <a-input
          v-model:value="advancedQueryForm.divisionCode"
          :placeholder="pi.queryPh('divisionCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionName')">
      <a-form-item :label="pi.queryLabel('divisionName')">
        <a-input
          v-model:value="advancedQueryForm.divisionName"
          :placeholder="pi.queryPh('divisionName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="pi.queryLabel('parentId')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="pi.queryPh('parentId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('level')">
      <a-form-item :label="pi.queryLabel('level')">
        <TaktSelect
          v-model:value="advancedQueryForm.level"
          dict-type="sys_admin_division_level_type"
          :placeholder="pi.queryPh('level', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionPath')">
      <a-form-item :label="pi.queryLabel('divisionPath')">
        <a-input
          v-model:value="advancedQueryForm.divisionPath"
          :placeholder="pi.queryPh('divisionPath', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLeaf')">
      <a-form-item :label="pi.queryLabel('isLeaf')">
        <TaktSelect
          v-model:value="advancedQueryForm.isLeaf"
          dict-type="sys_yes_no_type"
          :placeholder="pi.queryPh('isLeaf', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postalCode')">
      <a-form-item :label="pi.queryLabel('postalCode')">
        <a-input
          v-model:value="advancedQueryForm.postalCode"
          :placeholder="pi.queryPh('postalCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('phoneCode')">
      <a-form-item :label="pi.queryLabel('phoneCode')">
        <a-input
          v-model:value="advancedQueryForm.phoneCode"
          :placeholder="pi.queryPh('phoneCode', 'required')"
          show-count
          :maxlength="16"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="pi.queryLabel('isBuiltIn')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no_type"
          :placeholder="pi.queryPh('isBuiltIn', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionStatus')">
      <a-form-item :label="pi.queryLabel('divisionStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.divisionStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="pi.queryPh('divisionStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="ADMINDIVISION_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'adminDivisionId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 行政区划实体树表管理页 · 大数据：左侧懒加载+virtual，右侧 getXxxList 服务端分页（参照 admin-division/index.vue）
 * @module views/foundation/admin-division
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { useTableRefresh } from '@/composables/use-table-refresh'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
  type TaktLazyTreeNode,
} from '@/composables/use-lazy-tree'
import AdminDivisionForm from './components/admin-division-form.vue'
import { getAdminDivisionList, getAdminDivisionTree, getAdminDivisionById, createAdminDivision, updateAdminDivision, deleteAdminDivisionById, deleteAdminDivisionBatch, getAdminDivisionTemplate, importAdminDivision, exportAdminDivision, updateAdminDivisionStatus, updateAdminDivisionSort } from '@/api/foundation/admin-division'
import type { AdminDivision, AdminDivisionTree, AdminDivisionUpdate } from '@/types/foundation/admin-division'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useAdminDivisionI18n,
  ADMINDIVISION_QUERY_STRING_FIELDS,
  ADMINDIVISION_QUERY_FIELDS,
  ADMINDIVISION_SELF_I18N_KEY,
} from './composables/use-admin-division-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useAdminDivisionI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAdminDivision')
/** 右侧列表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [pi.label('adminDivisionId')].join(' / '),
  })
)

/** 左侧树关键字（仅过滤已加载节点，不重复请求 API） */
const treeQueryKeyword = ref('')
/** 右侧列表快捷查询关键字 */
const queryKeyword = ref('')
/** 左侧树工具栏「展开/收缩」状态（仅已加载层） */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表 */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧表格展开状态（预留） */
const tableExpanded = ref(false)
/** 右侧列表当前页码（服务端分页） */
const tableCurrentPage = ref(getTaktDefaultPageIndex())
/** 右侧列表每页条数 */
const tablePageSize = ref(getTaktDefaultPageSize())
/** 左侧树 loading */
const loading = ref(false)
/** 右侧列表 loading */
const listLoading = ref(false)
/** 左侧 a-tree 懒加载数据（仅已展开路径） */
const entityTreeData = ref<TaktLazyTreeNode[]>([])
/** 右侧列表数据源（服务端分页，当前父级直接子节点） */
const tableDataSource = ref<AdminDivision[]>([])
/** 右侧列表总行数 */
const tableTotal = ref(0)
/** 左侧树当前选中的节点 key 列表 */
const selectedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selectedRow = ref<AdminDivisionRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<AdminDivisionRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AdminDivision> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ADMINDIVISION_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ADMINDIVISION_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    level: undefined as number | undefined,
    isLeaf: undefined as number | undefined,
    isBuiltIn: undefined as number | undefined,
    divisionStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ADMINDIVISION_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'adminDivisionId'
/** 树节点标题字段名（左侧树 title） */
const treeTitleField = 'adminDivisionId'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 将树 API 一层 DTO 映射为左侧懒加载节点
 * @param rows 一层子节点
 */
function mapAdminDivisionLazyNodes(rows: AdminDivisionTree[]): TaktLazyTreeNode[] {
  return mapLazyTreeNodes(rows, {
    getKey: (n) => String(n.adminDivisionId ?? ''),
    getTitle: (n) => String(n.adminDivisionId || n.adminDivisionCode || n.adminDivisionId || ''),
    isLeaf: (n) => taktIsLeafFlag((n as { isLeaf?: unknown }).isLeaf),
  })
}

/**
 * 按关键字过滤左侧树：仅过滤已加载节点（大规模树不做全量搜索）
 * @param nodes 树节点
 * @param keyword 关键字
 */
function filterTreeByKeyword(nodes: TaktLazyTreeNode[], keyword: string): TaktLazyTreeNode[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  /** 递归过滤子树 */
  function filter(list: TaktLazyTreeNode[]): TaktLazyTreeNode[] {
    if (!list?.length) return []
    return list
      .map((node) => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children as TaktLazyTreeNode[]) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren }
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TaktLazyTreeNode
        }
        return null
      })
      .filter(Boolean) as TaktLazyTreeNode[]
  }
  return filter(nodes)
}

/** 左侧树绑定数据（按 treeQueryKeyword 客户端过滤已加载节点） */
const filteredTreeData = computed(() =>
  filterTreeByKeyword(entityTreeData.value, treeQueryKeyword.value)
)

/**
 * 收集已加载且有子节点的 key（展开全部仅作用于已加载层）
 * @param nodes 树节点
 */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  if (!nodes?.length) return []
  const keys: (string | number)[] = []
  for (const node of nodes) {
    const rawKey = node.key ?? node.adminDivisionId ?? node.id
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
 * 当前右侧列表的父级 ID（左侧选中；未选中则为根 0）
 * @returns {string} parentId
 */
function getRightListParentId(): string {
  const keys = selectedTreeKeys.value
  if (keys.length > 0 && keys[keys.length - 1] != null) {
    return String(keys[keys.length - 1])
  }
  return '0'
}

/**
 * 组装右侧列表查询参数（服务端分页）
 * @returns 查询 DTO
 */
function buildRightListQuery() {
  const aq = advancedQueryForm.value
  return {
    pageIndex: tableCurrentPage.value,
    pageSize: tablePageSize.value,
    parentId: aq.parentId ? aq.parentId : getRightListParentId(),
    keyWords: queryKeyword.value.trim() || undefined,
    countryCode: aq.countryCode || undefined,
    divisionCode: aq.divisionCode || undefined,
    divisionName: aq.divisionName || undefined,
    level: aq.level,
    divisionPath: aq.divisionPath || undefined,
    isLeaf: aq.isLeaf,
    postalCode: aq.postalCode || undefined,
    cultureCode: aq.cultureCode || undefined,
    currencyCode: aq.currencyCode || undefined,
    phoneCode: aq.phoneCode || undefined,
    isBuiltIn: aq.isBuiltIn,
    divisionStatus: aq.divisionStatus,
    createdAtStart: aq.createdAtStart || undefined,
    createdAtEnd: aq.createdAtEnd || undefined,
    extField: aq.extField || undefined,
    remark: aq.remark || undefined,
  }
}

/**
 * 加载右侧列表（服务端分页：当前父级直接子节点）
 */
async function loadRightList() {
  listLoading.value = true
  try {
    const res = await getAdminDivisionList(buildRightListQuery())
    const resAny = res as { data?: AdminDivision[]; Data?: AdminDivision[]; items?: AdminDivision[]; total?: number; Total?: number }
    const items = Array.isArray(res?.data)
      ? res.data
      : Array.isArray(resAny?.Data)
        ? resAny.Data
        : Array.isArray(resAny?.items)
          ? resAny.items
          : []
    tableDataSource.value = items
    tableTotal.value = Number(res?.total ?? resAny?.Total ?? items.length) || 0
  } catch (error: unknown) {
    logger.error('[AdminDivision] 加载列表失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    tableDataSource.value = []
    tableTotal.value = 0
  } finally {
    listLoading.value = false
  }
}

/**
 * 加载左侧树根节点（parentId=0，一层）
 */
async function reloadLeftTreeRoots() {
  const res = await getAdminDivisionTree('0', true)
  const resAny = res as { data?: AdminDivisionTree[]; Data?: AdminDivisionTree[] }
  const trees: AdminDivisionTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  entityTreeData.value = mapAdminDivisionLazyNodes(trees)
  treeExpandedKeys.value = []
  treeExpanded.value = false
}

/**
 * 重新加载指定父节点下已展开的子节点（CRUD 后局部刷新）
 * @param parentKey 父节点 key
 */
async function reloadLeftTreeChildren(parentKey: string) {
  if (!parentKey || parentKey === '0') {
    await reloadLeftTreeRoots()
    return
  }
  const res = await getAdminDivisionTree(parentKey, true)
  const resAny = res as { data?: AdminDivisionTree[]; Data?: AdminDivisionTree[] }
  const trees: AdminDivisionTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const children = mapAdminDivisionLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentKey,
    children,
  ) as TaktLazyTreeNode[]
}

/**
 * 左侧树懒加载子节点
 * @param treeNode Ant Design Tree 节点
 */
async function handleLeftTreeLoadData(treeNode: Record<string, unknown>) {
  const dataRef = (treeNode.dataRef ?? treeNode) as Record<string, unknown>
  const key = dataRef.key ?? treeNode.key
  if (key == null) return
  if (Array.isArray(dataRef.children) && dataRef.children.length > 0) return
  const res = await getAdminDivisionTree(String(key), true)
  const resAny = res as { data?: AdminDivisionTree[]; Data?: AdminDivisionTree[] }
  const trees: AdminDivisionTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const children = mapAdminDivisionLazyNodes(trees)
  dataRef.children = children
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    String(key),
    children,
  ) as TaktLazyTreeNode[]
}

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param adminDivision 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {AdminDivisionUpdate} 更新载荷
 */
function buildAdminDivisionUpdateDto(
  adminDivision: AdminDivision,
  overrides: Pick<AdminDivisionUpdate, 'parentId'> & { sortOrder: number },
): AdminDivisionUpdate {
  return {
    adminDivisionId: String(adminDivision.adminDivisionId),
    tenantCode: adminDivision.tenantCode,
    countryCode: adminDivision.countryCode,
    divisionCode: adminDivision.divisionCode,
    divisionName: adminDivision.divisionName,
    parentId: overrides.parentId,
    divisionPath: adminDivision.divisionPath,
    postalCode: adminDivision.postalCode,
    cultureCode: adminDivision.cultureCode,
    currencyCode: adminDivision.currencyCode,
    phoneCode: adminDivision.phoneCode,
    isBuiltIn: adminDivision.isBuiltIn,
    divisionStatus: adminDivision.divisionStatus,
    extField: adminDivision.extField,
    remark: adminDivision.remark,
  }
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.adminDivisionId ?? '')
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

/**
 * 左侧树拖拽完成后更新 parentId 与 sortOrder
 * @param payload 新树数据与被拖拽节点 key
 */
const handleTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    entityTreeData.value = newTreeData as TaktLazyTreeNode[]
    const full = await getAdminDivisionById(String(dragKey))
    await updateAdminDivision(String(dragKey), buildAdminDivisionUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    await updateAdminDivisionSort({ adminDivisionId: String(dragKey), sortOrder: pos.sortOrder })
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await reloadLeftTreeRoots().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

/** 左侧树关键字搜索（仅过滤已加载节点） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 左侧展开/收缩：仅展开已加载节点 */
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

/** 左侧树选中：刷新右侧列表（服务端 parentId） */
const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
  const firstPage = getTaktDefaultPageIndex()
  if (tableCurrentPage.value === firstPage) {
    void loadRightList()
  } else {
    tableCurrentPage.value = firstPage
  }
}

/** 服务端分页：页码/每页条数变化时重新拉列表 */
watch([tableCurrentPage, tablePageSize], () => {
  void loadRightList()
})

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type AdminDivisionRowRecord = AdminDivision | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getAdminDivisionId = (record: AdminDivisionRowRecord): string => {
  if (record != null && 'adminDivisionId' in record && (record as Record<string, unknown>).adminDivisionId != null) {
    return String((record as Record<string, unknown>).adminDivisionId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getAdminDivisionField = (record: AdminDivisionRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getAdminDivisionDictValue = (
  record: AdminDivisionRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
/** 将行字段/字典值转为有限 number */
const toAdminDivisionNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}



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
    dataIndex: 'adminDivisionId',
    key: 'adminDivisionId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getAdminDivisionField(record, 'adminDivisionId') ?? getAdminDivisionField(record, 'id') ?? '',
  },
  {
    title: pi.label('countryCode'),
    dataIndex: 'countryCode',
    key: 'countryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionCode'),
    dataIndex: 'divisionCode',
    key: 'divisionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'divisionCode') ?? ''
  },
  {
    title: pi.label('divisionName'),
    dataIndex: 'divisionName',
    key: 'divisionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'divisionName') ?? ''
  },
  {
    title: pi.label('parentId'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('level'),
    dataIndex: 'level',
    key: 'level',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionPath'),
    dataIndex: 'divisionPath',
    key: 'divisionPath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'divisionPath') ?? ''
  },
  {
    title: pi.label('isLeaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('postalCode'),
    dataIndex: 'postalCode',
    key: 'postalCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'postalCode') ?? ''
  },
  {
    title: pi.label('cultureCode'),
    dataIndex: 'cultureCode',
    key: 'cultureCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('phoneCode'),
    dataIndex: 'phoneCode',
    key: 'phoneCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'phoneCode') ?? ''
  },
  {
    title: pi.label('isBuiltIn'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionStatus'),
    dataIndex: 'divisionStatus',
    key: 'divisionStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn<AdminDivision>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:admin:division:update',
        onClick: (record: AdminDivisionRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:admin:division:delete',
        onClick: (record: AdminDivisionRowRecord) => handleDeleteOne(record)
      }
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AdminDivision[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: AdminDivisionRowRecord, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getAdminDivisionId(selectedRow.value) === getAdminDivisionId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: AdminDivision[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  },
}))

/** 加载根树 + 右侧列表（初始化 / 租户切换） */
async function loadData() {
  loading.value = true
  try {
    await reloadLeftTreeRoots()
    await loadRightList()
  } catch (error: unknown) {
    logger.error('[AdminDivision] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    entityTreeData.value = []
    tableDataSource.value = []
    tableTotal.value = 0
  } finally {
    loading.value = false
  }
}

/**
 * CRUD / 状态变更后局部刷新：当前父级子节点 + 右侧列表
 */
async function refreshAfterMutation() {
  loading.value = true
  try {
    const parentKey = getRightListParentId()
    await reloadLeftTreeChildren(parentKey)
    await loadRightList()
  } catch (error: unknown) {
    logger.error('[AdminDivision] 刷新失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
  } finally {
    loading.value = false
  }
}

/** 右侧查询（服务端） */
const handleSearch = () => {
  const firstPage = getTaktDefaultPageIndex()
  if (tableCurrentPage.value === firstPage) {
    void loadRightList()
  } else {
    tableCurrentPage.value = firstPage
  }
}

/** 右侧重置并重新查询 */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  const firstPage = getTaktDefaultPageIndex()
  if (tableCurrentPage.value === firstPage) {
    void loadRightList()
  } else {
    tableCurrentPage.value = firstPage
  }
}


/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleDivisionStatusChange(record: AdminDivisionRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toAdminDivisionNumber(getAdminDivisionDictValue(record, 'divisionStatus'))
  const id = getAdminDivisionId(record)
  try {
    await updateAdminDivisionStatus({ adminDivisionId: id, divisionStatus: newVal })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: unknown) {
    message.error(t('common.feedback.failed'))
  }
}

/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  const keys = selectedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/** 打开编辑弹窗 */
function handleEdit(record: AdminDivisionRowRecord) {
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
}

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
      await updateAdminDivision(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createAdminDivision(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    await refreshAfterMutation()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

/** 删除单行 */
async function handleDeleteOne(record: AdminDivisionRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAdminDivisionById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}

/** 批量删除选中行 */
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
      await deleteAdminDivisionBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}


/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getAdminDivisionTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAdminDivision(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void refreshAfterMutation()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportAdminDivision({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
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
    logger.error('[AdminDivision] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: pi.self() })))
  } finally {
    loading.value = false
  }
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重新拉右侧列表 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  const firstPage = getTaktDefaultPageIndex()
  if (tableCurrentPage.value === firstPage) {
    void loadRightList()
  } else {
    tableCurrentPage.value = firstPage
  }
}

/** 重置高级查询表单（不自动查询） */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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

/** 页面挂载：租户上下文就绪后加载分页配置，再拉树+列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
useTableRefresh(loadData)
</script>

<style scoped lang="css">
.foundation-admin-division {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.foundation-admin-division-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.foundation-admin-division-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.foundation-admin-division-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
