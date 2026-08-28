<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/cost-center -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本中心实体树表管理页（左树右表），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-controlling-cost-center">
    <!-- 第一行：左树查询栏 | 右表查询栏 -->
    <div class="accounting-controlling-cost-center-query-row">
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

    <!-- 第二行：左树工具栏 | 右表工具栏 -->
    <div class="accounting-controlling-cost-center-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFullCostCenterTree"
      />
      <TaktTreeRightToolsBar
        create-permission="accounting:controlling:cost:center:create"
        update-permission="accounting:controlling:cost:center:update"
        delete-permission="accounting:controlling:cost:center:delete"
        import-permission="accounting:controlling:cost:center:import"
        export-permission="accounting:controlling:cost:center:export"
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

    <!-- 第三行：左树 | 右树表 -->
    <div class="accounting-controlling-cost-center-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'costCenterId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :loading="loading"
        :row-key="getCostCenterId"
        :stripe="true"
        :row-selection="rowSelection"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'costCenterName'">
            <span>{{ getCostCenterField(record, 'costCenterName') }}</span>
          </template>
        <template v-else-if="column.key === 'costCenterType'">
          <TaktDictTag
            :value="getCostCenterDictValue(record, 'costCenterType')"
            dict-type="accounting_controlling_cost_center_type"
          />
        </template>
        <template v-else-if="column.key === 'costCenterStatus'">
          <a-switch
            :checked="getCostCenterDictValue(record, 'costCenterStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleCostCenterStatusChange(record, Boolean(checked))"
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
      <CostCenterForm
        :key="formData?.costCenterId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-cost-center'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="pi.queryLabel('costCenterCode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="pi.queryPh('costCenterCode', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterName')">
      <a-form-item :label="pi.queryLabel('costCenterName')">
        <a-input
          v-model:value="advancedQueryForm.costCenterName"
          :placeholder="pi.queryPh('costCenterName', 'required')"
          show-count
          :maxlength="100"
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
      <div v-show="isFieldVisible('costCenterType')">
      <a-form-item :label="pi.queryLabel('costCenterType')">
        <TaktSelect
          v-model:value="advancedQueryForm.costCenterType"
          dict-type="accounting_controlling_cost_center_type"
          :placeholder="pi.queryPh('costCenterType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('managerId')">
      <a-form-item :label="pi.queryLabel('managerId')">
        <a-input
          v-model:value="advancedQueryForm.managerId"
          :placeholder="pi.queryPh('managerId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('managerName')">
      <a-form-item :label="pi.queryLabel('managerName')">
        <a-input
          v-model:value="advancedQueryForm.managerName"
          :placeholder="pi.queryPh('managerName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="pi.queryLabel('deptId')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="pi.queryPh('deptId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="pi.queryLabel('deptName')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="pi.queryPh('deptName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterLevel')">
      <a-form-item :label="pi.queryLabel('costCenterLevel')">
        <a-input-number
          v-model:value="advancedQueryForm.costCenterLevel"
          :placeholder="pi.queryPh('costCenterLevel', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="pi.queryLabel('validFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="pi.queryPh('validFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="pi.queryLabel('validFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="pi.queryPh('validFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="pi.queryLabel('validToStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="pi.queryPh('validToStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="pi.queryLabel('validToEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="pi.queryPh('validToEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterStatus')">
      <a-form-item :label="pi.queryLabel('costCenterStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.costCenterStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('costCenterStatus', 'select')"
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
        :entity-i18n-key="COSTCENTER_SELF_I18N_KEY"
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
      :id-column-key="'costCenterId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 成本中心实体树表管理页 · 全量树左树右表（参照 identity/menu/index.vue）
 * @module views/accounting/controlling/cost-center
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { useTableRefresh } from '@/composables/use-table-refresh'
import CostCenterForm from './components/cost-center-form.vue'
import { getCostCenterTree, getCostCenterById, createCostCenter, updateCostCenter, deleteCostCenterById, deleteCostCenterBatch, getCostCenterTemplate, importCostCenter, exportCostCenter, updateCostCenterStatus, updateCostCenterSort } from '@/api/accounting/controlling/cost-center'
import type { CostCenter, CostCenterTree, CostCenterUpdate } from '@/types/accounting/controlling/cost-center'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useUserStore } from '@/stores/identity/user'
import {
  collectTaktTreeTableExpandableKeys,
  filterTaktTreeTableNodes,
  taktTreeTableNodeKey,
} from '@/utils/takt-tree-table'
import {
  useCostCenterI18n,
  COSTCENTER_QUERY_STRING_FIELDS,
  COSTCENTER_QUERY_FIELDS,
  COSTCENTER_SELF_I18N_KEY,
} from './composables/use-cost-center-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCostCenterI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCostCenter')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [pi.label('costCenterName'), pi.label('costCenterCode')].join(' / '),
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
/** 右侧树表工具栏「全部展开/收缩」 */
const tableExpanded = ref(false)
/** 右侧 a-table 树表当前展开行 key */
const tableExpandedRowKeys = ref<(string | number)[]>([])
/** 页面 loading（树加载、提交、导出等） */
const loading = ref(false)
/** 全量树表节点（左侧树与右侧表共用，不受右侧查询过滤） */
const fullTableTree = ref<Record<string, unknown>[]>([])
/** 左侧 a-tree 绑定数据（由 fullTableTree 映射 title/key） */
const entityTreeData = ref<TreeDataItem[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selectedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selectedRow = ref<CostCenterRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<CostCenterRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CostCenter> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of COSTCENTER_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.costCenterType !== undefined && form.costCenterType !== null) {
    return true
  }
  if (form.costCenterLevel !== undefined && form.costCenterLevel !== null) {
    return true
  }
  if (form.costCenterStatus !== undefined && form.costCenterStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(COSTCENTER_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof COSTCENTER_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    costCenterType: undefined as number | undefined,
    costCenterLevel: undefined as number | undefined,
    costCenterStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  COSTCENTER_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'costCenterId'
/** 树节点标题字段名（左侧树 title） */
const treeTitleField = 'costCenterName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 将接口树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 实体树 DTO 列表
 */
function costCenterTreeToTableNodes(nodes: CostCenterTree[]): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? costCenterTreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.costCenterId ?? ''),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

/** 解析树节点 key（与列表 costCenterId、左侧树 key 一致） */
function resolveCostCenterNodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.costCenterId ?? node.id
  return raw == null ? '' : String(raw)
}

/** 将 fullTableTree 转为左侧 a-tree（与右侧表共用 key，保证点选联动） */
function mapFullTableTreeToTreeData(nodes: Array<Record<string, unknown>>): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes.map((n) => {
    const title = String(n[treeTitleField] ?? n.title ?? '')
    const key = resolveCostCenterNodeKey(n)
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
    if (resolveCostCenterNodeKey(node) === k) return node
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
  function filter(list: TreeDataItem[]): TreeDataItem[] {
    if (!list?.length) return []
    return list
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
  return collectTaktTreeTableExpandableKeys(nodes, (node) => taktTreeTableNodeKey(node, 'costCenterId'))
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
function matchesCostCenterRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.costCenterName ?? '').toLowerCase().includes(k) && !String(record.costCenterCode ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.costCenterCode && !String(record.costCenterCode ?? '').includes(String(advancedQueryForm.value.costCenterCode))) return false
  if (advancedQueryForm.value.costCenterName && !String(record.costCenterName ?? '').includes(String(advancedQueryForm.value.costCenterName))) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.costCenterType !== undefined && record.costCenterType !== advancedQueryForm.value.costCenterType) return false
  if (advancedQueryForm.value.managerId && !String(record.managerId ?? '').includes(String(advancedQueryForm.value.managerId))) return false
  if (advancedQueryForm.value.managerName && !String(record.managerName ?? '').includes(String(advancedQueryForm.value.managerName))) return false
  if (advancedQueryForm.value.deptId && !String(record.deptId ?? '').includes(String(advancedQueryForm.value.deptId))) return false
  if (advancedQueryForm.value.deptName && !String(record.deptName ?? '').includes(String(advancedQueryForm.value.deptName))) return false
  if (advancedQueryForm.value.costCenterLevel !== undefined && record.costCenterLevel !== advancedQueryForm.value.costCenterLevel) return false
  if (advancedQueryForm.value.validFromStart && !String(record.validFromStart ?? '').includes(String(advancedQueryForm.value.validFromStart))) return false
  if (advancedQueryForm.value.validFromEnd && !String(record.validFromEnd ?? '').includes(String(advancedQueryForm.value.validFromEnd))) return false
  if (advancedQueryForm.value.validToStart && !String(record.validToStart ?? '').includes(String(advancedQueryForm.value.validToStart))) return false
  if (advancedQueryForm.value.validToEnd && !String(record.validToEnd ?? '').includes(String(advancedQueryForm.value.validToEnd))) return false
  if (advancedQueryForm.value.plantCode && !String(record.plantCode ?? '').includes(String(advancedQueryForm.value.plantCode))) return false
  if (advancedQueryForm.value.costCenterStatus !== undefined && record.costCenterStatus !== advancedQueryForm.value.costCenterStatus) return false
  if (advancedQueryForm.value.createdAtStart && !String(record.createdAtStart ?? '').includes(String(advancedQueryForm.value.createdAtStart))) return false
  if (advancedQueryForm.value.createdAtEnd && !String(record.createdAtEnd ?? '').includes(String(advancedQueryForm.value.createdAtEnd))) return false
  if (advancedQueryForm.value.extField && !String(record.extField ?? '').includes(String(advancedQueryForm.value.extField))) return false
  if (advancedQueryForm.value.remark && !String(record.remark ?? '').includes(String(advancedQueryForm.value.remark))) return false
  return true
}

/** 右侧过滤后的树（保留 children，供组件按展开路径拍平） */
const tableFilteredTree = computed(() =>
  filterTaktTreeTableNodes(tableTreeData.value, matchesCostCenterRightQuery)
)

/**
 * 同步右侧树表全部展开/收缩
 * @returns {void}
 */
function applyCostCenterTableExpandState() {
  tableExpandedRowKeys.value = tableExpanded.value
    ? collectTaktTreeTableExpandableKeys(tableFilteredTree.value, (node) =>
        taktTreeTableNodeKey(node, 'costCenterId'),
      )
    : []
}

watch(tableExpanded, applyCostCenterTableExpandState)
watch(tableFilteredTree, () => {
  if (tableExpanded.value) applyCostCenterTableExpandState()
})

/** 左侧树选中：过滤右侧子树 */
const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
}

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param costCenter 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {CostCenterUpdate} 更新载荷
 */
function buildCostCenterUpdateDto(
  costCenter: CostCenter,
  overrides: Pick<CostCenterUpdate, 'parentId'> & { sortOrder: number },
): CostCenterUpdate {
  return {
    costCenterId: String(costCenter.costCenterId),
    tenantCode: costCenter.tenantCode,
    companyCode: costCenter.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    costCenterName: costCenter.costCenterName,
    parentId: overrides.parentId,
    costCenterType: costCenter.costCenterType,
    managerId: costCenter.managerId,
    managerName: costCenter.managerName,
    deptId: costCenter.deptId,
    deptName: costCenter.deptName,
    costCenterLevel: costCenter.costCenterLevel,
    validFrom: costCenter.validFrom,
    validTo: costCenter.validTo,
    plantCode: costCenter.plantCode,
    costCenterStatus: costCenter.costCenterStatus,
    extField: costCenter.extField,
    remark: costCenter.remark,
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
    const k = String(node?.key ?? node?.costCenterId ?? '')
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
    entityTreeData.value = newTreeData as TreeDataItem[]
    const full = await getCostCenterById(String(dragKey))
    await updateCostCenter(String(dragKey), buildCostCenterUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    await updateCostCenterSort({ costCenterId: String(dragKey), sortOrder: pos.sortOrder })
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await loadFullCostCenterTree().catch(() => undefined)
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
type CostCenterRowRecord = CostCenter | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getCostCenterId = (record: CostCenterRowRecord): string => {
  if (record != null && 'costCenterId' in record && (record as Record<string, unknown>).costCenterId != null) {
    return String((record as Record<string, unknown>).costCenterId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getCostCenterField = (record: CostCenterRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getCostCenterDictValue = (
  record: CostCenterRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
/** 将行字段/字典值转为有限 number */
const toCostCenterNumber = (value: string | number | undefined | null): number => {
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
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getCostCenterField(record, 'costCenterId') ?? getCostCenterField(record, 'id') ?? '',
  },
  {
    title: pi.label('costCenterCode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'costCenterCode') ?? ''
  },
  {
    title: pi.label('costCenterName'),
    dataIndex: 'costCenterName',
    key: 'costCenterName',
    width: 160,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('parentId'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('costCenterType'),
    dataIndex: 'costCenterType',
    key: 'costCenterType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('managerId'),
    dataIndex: 'managerId',
    key: 'managerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'managerId') ?? ''
  },
  {
    title: pi.label('managerName'),
    dataIndex: 'managerName',
    key: 'managerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'managerName') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('costCenterLevel'),
    dataIndex: 'costCenterLevel',
    key: 'costCenterLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'costCenterLevel') ?? ''
  },
  {
    title: pi.label('validFrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'validFrom') ?? ''
  },
  {
    title: pi.label('validTo'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getCostCenterField(record, 'validTo') ?? ''
  },
  {
    title: pi.label('costCenterStatus'),
    dataIndex: 'costCenterStatus',
    key: 'costCenterStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn<CostCenter>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:controlling:cost:center:update',
        onClick: (record: CostCenterRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:cost:center:delete',
        onClick: (record: CostCenterRowRecord) => handleDeleteOne(record)
      }
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CostCenter[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: CostCenterRowRecord, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getCostCenterId(selectedRow.value) === getCostCenterId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: CostCenter[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  },
}))

/** 加载全量树（左侧树 + 右侧树表共用数据源） */
async function loadFullCostCenterTree() {
  const res = await getCostCenterTree('0', true)
  const resAny = res as { data?: CostCenterTree[]; Data?: CostCenterTree[] }
  const trees: CostCenterTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = costCenterTreeToTableNodes(trees)
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
    await loadFullCostCenterTree()
  } catch (error: unknown) {
    logger.error('[CostCenter] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    fullTableTree.value = []
    entityTreeData.value = []
  } finally {
    loading.value = false
  }
}

/** 右侧查询（客户端过滤，不请求接口） */
const handleSearch = () => {}

/** 右侧重置（不影响左侧树与 fullTableTree） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}


/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleCostCenterStatusChange(record: CostCenterRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toCostCenterNumber(getCostCenterDictValue(record, 'costCenterStatus'))
  const id = getCostCenterId(record)
  try {
    await updateCostCenterStatus({ costCenterId: id, costCenterStatus: newVal })
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
function handleEdit(record: CostCenterRowRecord) {
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
      await updateCostCenter(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCostCenter(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
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
async function handleDeleteOne(record: CostCenterRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCostCenterById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
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
      await deleteCostCenterBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
  const res = await getCostCenterTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCostCenter(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
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
    const exportMeta = await exportCostCenter({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    logger.error('[CostCenter] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: pi.self() })))
  } finally {
    loading.value = false
  }
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉（过滤为 computed，无需重新请求） */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
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

/** 表格 change / 列宽拖拽占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}

/** 页面挂载：加载字典与全量树 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
useTableRefresh(loadData)
</script>

<style scoped lang="css">
.accounting-controlling-cost-center {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
  overflow: hidden;
  box-sizing: border-box;
}
.accounting-controlling-cost-center-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
  flex-shrink: 0;
}
.accounting-controlling-cost-center-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
  flex-shrink: 0;
}
.accounting-controlling-cost-center-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
  overflow: hidden;
  align-items: stretch;
}
</style>
