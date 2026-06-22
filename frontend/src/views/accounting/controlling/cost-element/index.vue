<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/cost-element -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本要素实体树表管理页（左树右表），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-controlling-cost-element">
    <!-- 查询栏 -->
    <div class="accounting-controlling-cost-element-query-row">
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
    <div class="accounting-controlling-cost-element-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFullCostElementTree"
      />
      <TaktTreeRightToolsBar
        create-permission="accounting:controlling:cost:element:create"
        update-permission="accounting:controlling:cost:element:update"
        delete-permission="accounting:controlling:cost:element:delete"
        import-permission="accounting:controlling:cost:element:import"
        export-permission="accounting:controlling:cost:element:export"
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

    <!-- 左树右表 -->
    <div class="accounting-controlling-cost-element-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="false"
        :draggable="true"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="company"
        v-model:current="tableCurrentPage"
        v-model:page-size="tablePageSize"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'costElementId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="paginatedFlatTableRows"
        :loading="loading"
        :row-key="getCostElementId"
        :stripe="true"
        :row-selection="rowSelection"
        :show-pagination="true"
        :total="tableFlatTotal"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'costElementName'">
            <span
              class="inline-block"
              :style="{ paddingLeft: `${(record._treeDepth ?? 0) * 16}px` }"
            >
              {{ getCostElementField(record, 'costElementName') }}
            </span>
          </template>
        <template v-else-if="column.key === 'costElementStatus'">
          <a-switch
            :checked="getCostElementField(record, 'costElementStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleCostElementStatusChange(record, Boolean(checked))"
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
      <CostElementForm
        :key="formData?.costElementId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-cost-element'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('costElementCode')">
      <a-form-item :label="t('entity.costelement.code')">
        <a-input
          v-model:value="advancedQueryForm.costElementCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementName')">
      <a-form-item :label="t('entity.costelement.name')">
        <a-input
          v-model:value="advancedQueryForm.costElementName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.name') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementType')">
      <a-form-item :label="t('entity.costelement.type')">
        <a-input-number
          v-model:value="advancedQueryForm.costElementType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementCategory')">
      <a-form-item :label="t('entity.costelement.category')">
        <a-input-number
          v-model:value="advancedQueryForm.costElementCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.category') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="t('entity.costelement.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.parentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementLevel')">
      <a-form-item :label="t('entity.costelement.level')">
        <a-input-number
          v-model:value="advancedQueryForm.costElementLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementStatus')">
      <a-form-item :label="t('entity.costelement.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.costElementStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="t('entity.costelement.validfromstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validfromstart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="t('entity.costelement.validfromend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validfromend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="t('entity.costelement.validtostart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validtostart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="t('entity.costelement.validtoend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validtoend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.costelement._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.costelement._self"
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
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'costElementId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 成本要素实体树表管理页 · ParentId 左树右表（参照 dept/index.vue）
 * @module views/accounting/controlling/cost-element
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CostElementForm from './components/cost-element-form.vue'
import { getCostElementTree, getCostElementById, createCostElement, updateCostElement, deleteCostElementById, deleteCostElementBatch, getCostElementTemplate, importCostElement, exportCostElement, updateCostElementStatus } from '@/api/accounting/controlling/cost-element'
import type { CostElement, CostElementTree, CostElementUpdate } from '@/types/accounting/controlling/cost-element'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCostElement')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [t('entity.costelement.name'), t('entity.costelement.code')].join(' / '),
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
const tableCurrentPage = ref(getTaktDefaultPageIndex())
/** 右侧拍平列表每页条数 */
const tablePageSize = ref(getTaktDefaultPageSize())
/** 页面 loading（树加载、提交、导出等） */
const loading = ref(false)
/** 全量树表节点（左侧树与右侧表共用，不受右侧查询过滤） */
const fullTableTree = ref<Record<string, unknown>[]>([])
/** 左侧 a-tree 绑定数据（由 fullTableTree 映射 title/key） */
const entityTreeData = ref<TreeDataItem[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selectedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selectedRow = ref<CostElement | null>(null)
/** 表格多选行 */
const selectedRows = ref<CostElement[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CostElement> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'costElementCode', label: t('entity.costelement.code') },
  { key: 'costElementName', label: t('entity.costelement.name') },
  { key: 'costElementType', label: t('entity.costelement.type') },
  { key: 'costElementCategory', label: t('entity.costelement.category') },
  { key: 'parentId', label: t('entity.costelement.parentid') },
  { key: 'costElementLevel', label: t('entity.costelement.level') },
  { key: 'costElementStatus', label: t('entity.costelement.status') },
  { key: 'validFromStart', label: t('entity.costelement.validfromstart') },
  { key: 'validFromEnd', label: t('entity.costelement.validfromend') },
  { key: 'validToStart', label: t('entity.costelement.validtostart') },
  { key: 'validToEnd', label: t('entity.costelement.validtoend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'costElementId'
/** 树节点标题字段名（左侧树 title 与缩进列） */
const treeTitleField = 'costElementName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 解析树节点 key（与列表 costElementId、左侧树 key 一致） */
function resolveCostElementNodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.costElementId ?? node.id
  return raw == null ? '' : String(raw)
}

/**
 * 将接口树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 实体树 DTO 列表
 */
function costElementTreeToTableNodes(nodes: CostElementTree[]): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? costElementTreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.costElementId ?? ''),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

/** 将 fullTableTree 转为左侧 a-tree（与右侧表共用 key，保证点选联动） */
function mapFullTableTreeToTreeData(nodes: Array<Record<string, unknown>>): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes.map((n) => {
    const title = String(n[treeTitleField] ?? n.title ?? '')
    const key = resolveCostElementNodeKey(n)
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
    if (resolveCostElementNodeKey(node) === k) return node
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
    const rawKey = node.key ?? node.costElementId ?? node.id
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
function flattenCostElementTableRows(nodes: Array<Record<string, unknown>>, depth = 0): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  const rows: Array<Record<string, unknown>> = []
  for (const node of nodes) {
    const childList = node.children as Array<Record<string, unknown>> | undefined
    const { children: _children, ...rest } = node
    rows.push({ ...rest, _treeDepth: depth })
    if (childList?.length) {
      rows.push(...flattenCostElementTableRows(childList, depth + 1))
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
function matchesCostElementRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.costElementName ?? '').toLowerCase().includes(k)) return false
    if (!String(record.costElementCode ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.costElementCode && !String(record.costElementCode ?? '').includes(String(advancedQueryForm.value.costElementCode))) return false
  if (advancedQueryForm.value.costElementName && !String(record.costElementName ?? '').includes(String(advancedQueryForm.value.costElementName))) return false
  if (advancedQueryForm.value.costElementType !== undefined && record.costElementType !== advancedQueryForm.value.costElementType) return false
  if (advancedQueryForm.value.costElementCategory !== undefined && record.costElementCategory !== advancedQueryForm.value.costElementCategory) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.costElementLevel !== undefined && record.costElementLevel !== advancedQueryForm.value.costElementLevel) return false
  if (advancedQueryForm.value.costElementStatus !== undefined && record.costElementStatus !== advancedQueryForm.value.costElementStatus) return false
  if (advancedQueryForm.value.validFromStart && !String(record.validFromStart ?? '').includes(String(advancedQueryForm.value.validFromStart))) return false
  if (advancedQueryForm.value.validFromEnd && !String(record.validFromEnd ?? '').includes(String(advancedQueryForm.value.validFromEnd))) return false
  if (advancedQueryForm.value.validToStart && !String(record.validToStart ?? '').includes(String(advancedQueryForm.value.validToStart))) return false
  if (advancedQueryForm.value.validToEnd && !String(record.validToEnd ?? '').includes(String(advancedQueryForm.value.validToEnd))) return false
  if (advancedQueryForm.value.createdAtStart && !String(record.createdAtStart ?? '').includes(String(advancedQueryForm.value.createdAtStart))) return false
  if (advancedQueryForm.value.createdAtEnd && !String(record.createdAtEnd ?? '').includes(String(advancedQueryForm.value.createdAtEnd))) return false
  if (advancedQueryForm.value.extField && !String(record.extField ?? '').includes(String(advancedQueryForm.value.extField))) return false
  if (advancedQueryForm.value.remark && !String(record.remark ?? '').includes(String(advancedQueryForm.value.remark))) return false
  return true
}

/** 右侧拍平后的全部行（先左侧子树，再右侧查询过滤） */
const tableFlatRows = computed(() =>
  flattenCostElementTableRows(tableTreeData.value).filter(matchesCostElementRightQuery)
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

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param costElement 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {CostElementUpdate} 更新载荷
 */
function buildCostElementUpdateDto(
  costElement: CostElement,
  overrides: Pick<CostElementUpdate, 'parentId' | 'sortOrder'>,
): CostElementUpdate {
  return {
    costElementId: String(costElement.costElementId),
    tenantCode: costElement.tenantCode,
    companyCode: costElement.companyCode,
    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
    costElementCode: costElement.costElementCode,
    costElementName: costElement.costElementName,
    costElementType: costElement.costElementType,
    costElementCategory: costElement.costElementCategory,
    parentId: overrides.parentId,
    costElementLevel: costElement.costElementLevel,
    costElementStatus: costElement.costElementStatus,
    validFrom: costElement.validFrom,
    validTo: costElement.validTo,
    changeLogs: costElement.changeLogs,
    extField: costElement.extField,
    remark: costElement.remark,
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
    const k = String(node?.key ?? node?.costElementId ?? '')
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
    entityTreeData.value = newTreeData
    const full = await getCostElementById(String(dragKey))
    await updateCostElement(String(dragKey), buildCostElementUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    message.success(t('common.feedback.updated', { target: t('entity.costelement._self') }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: t('entity.costelement._self') })))
    await loadFullCostElementTree().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

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
type CostElementRowRecord = CostElement | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getCostElementId = (record: CostElementRowRecord): string => {
  if (record != null && 'costElementId' in record && (record as Record<string, unknown>).costElementId != null) {
    return String((record as Record<string, unknown>).costElementId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getCostElementField = (record: CostElementRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getCostElementDictValue = (
  record: CostElementRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
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
    dataIndex: 'costElementId',
    key: 'costElementId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getCostElementField(record, 'costElementId') ?? getCostElementField(record, 'id') ?? '',
  },
  {
    title: t('entity.costelement.code'),
    dataIndex: 'costElementCode',
    key: 'costElementCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'costElementCode') ?? ''
  },
  {
    title: t('entity.costelement.name'),
    dataIndex: 'costElementName',
    key: 'costElementName',
    width: 160,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.costelement.type'),
    dataIndex: 'costElementType',
    key: 'costElementType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'costElementType') ?? ''
  },
  {
    title: t('entity.costelement.category'),
    dataIndex: 'costElementCategory',
    key: 'costElementCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'costElementCategory') ?? ''
  },
  {
    title: t('entity.costelement.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.costelement.level'),
    dataIndex: 'costElementLevel',
    key: 'costElementLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'costElementLevel') ?? ''
  },
  {
    title: t('entity.costelement.status'),
    dataIndex: 'costElementStatus',
    key: 'costElementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.costelement.validfrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'validFrom') ?? ''
  },
  {
    title: t('entity.costelement.validto'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostElementField(record, 'validTo') ?? ''
  },
  CreateActionColumn<CostElement>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:controlling:cost:element:update',
        onClick: (record: CostElement) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:cost:element:delete',
        onClick: (record: CostElement) => handleDeleteOne(record)
      }
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CostElement[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: CostElement, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getCostElementId(selectedRow.value) === getCostElementId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: CostElement[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  },
}))

/** 加载全量树（左侧树 + 右侧树表共用数据源） */
async function loadFullCostElementTree() {
  const res = await getCostElementTree('0', true)
  const resAny = res as { data?: CostElementTree[]; Data?: CostElementTree[] }
  const trees: CostElementTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = costElementTreeToTableNodes(trees)
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
    await loadFullCostElementTree()
  } catch (error: unknown) {
    logger.error('[CostElement] 加载树数据失败', undefined, error)
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
  advancedQueryForm.value = {
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  tableCurrentPage.value = getTaktDefaultPageIndex()
}


/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleCostElementStatusChange(record: CostElementRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getCostElementField(record, 'costElementStatus')
  const id = getCostElementId(record)
  const row = null
  if (row) {
    row.costElementStatus = newVal
  }
  try {
    await updateCostElementStatus({ costElementId: id, costElementStatus: newVal })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: unknown) {
    if (row) {
      row.costElementStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
}

/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.costelement._self') })
  const keys = selectedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/** 打开编辑弹窗 */
function handleEdit(record: CostElement) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.costelement._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.costelement._self') }))
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
      await updateCostElement(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.costelement._self') }))
    } else {
      await createCostElement(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.costelement._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    await loadData()
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
async function handleDeleteOne(record: CostElement) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.costelement._self'), name: t('common.tip.this.target', { target: t('entity.costelement._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCostElementById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.costelement._self') }))
      await loadData()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.costelement._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.costelement._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCostElementBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.costelement._self') }))
      await loadData()
    }
  })
}

/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getCostElementTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCostElement(file, sheetName)
}

/** 导入完成回调：刷新树并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportCostElement({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.costelement._self') }))
  } catch (error: unknown) {
    logger.error('[CostElement] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: t('entity.costelement._self') })))
  } finally {
    loading.value = false
  }
}

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
  advancedQueryForm.value = {
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
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
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
</script>

<style scoped lang="css">
.accounting-controlling-cost-element {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.accounting-controlling-cost-element-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-controlling-cost-element-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-controlling-cost-element-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
