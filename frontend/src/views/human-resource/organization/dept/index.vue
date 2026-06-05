<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/humanresource/organization/dept -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：部门管理页面，左 1/4 部门树、右 3/4 部门树表，含查询、增删改、导入导出等 -->
<!-- ======================================== -->

<template>
  <div class="organization-dept">
    <div class="dept-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        placeholder="树关键字"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        placeholder="请输入部门名称或编码"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <div class="dept-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFullDeptTree"
      />
      <TaktTreeRightToolsBar
        create-permission="humanresource:organization:dept:create"
        update-permission="humanresource:organization:dept:update"
        delete-permission="humanresource:organization:dept:delete"
        import-permission="humanresource:organization:dept:import"
        export-permission="humanresource:organization:dept:export"
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

    <div class="dept-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredDeptTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="false"
        :draggable="true"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        v-model:current="tableCurrentPage"
        v-model:page-size="tablePageSize"
        :columns="columns"
        entity-scope="company"
        :visible-column-keys="visibleColumnKeys"
        :data-source="paginatedFlatTableRows"
        :loading="loading"
        :row-key="getDeptId"
        :stripe="true"
        :row-selection="rowSelection"
        :show-pagination="true"
        :total="tableFlatTotal"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'deptName'">
            <span
              class="inline-block"
              :style="{ paddingLeft: `${(record._treeDepth ?? 0) * 16}px` }"
            >
              {{ getDeptField(record, 'deptName') }}
            </span>
          </template>
          <template v-else-if="column.key === 'costCategory'">
            {{ getDeptField(record, 'costCategory') }}
          </template>
          <template v-else-if="column.key === 'deptStatus'">
            <TaktDictTag
              :value="getDeptDictValue(record, 'deptStatus')"
              dict-type="sys_normal_disable"
            />
          </template>
        </template>
      </TaktTreeRightTable>
    </div>

    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <DeptForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.dept.name')">
        <a-input v-model:value="advancedQueryForm.deptName" />
      </a-form-item>
      <a-form-item :label="t('entity.dept.code')">
        <a-input v-model:value="advancedQueryForm.deptCode" />
      </a-form-item>
      <a-form-item :label="t('entity.dept.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.deptStatus"
          api-url="/api/TaktDictDatas/options?dictTypeCode=sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.status') })"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'extLabel' }"
        />
      </a-form-item>
    </TaktQueryDrawer>

    <TaktModal
      v-model:open="importVisible"
      :title="t('common.page.button.import') + t('entity.dept._self')"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.dept._self"
        file-type="xlsx"
        :sheet-name="deptExcelNames.sheet"
        :template-file-name="deptExcelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>

    <AssignDeptEmployees
      v-model:open="assignDeptEmployeesVisible"
      :dept="currentAssignDept"
      @success="handleAssignSuccess"
    />

    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'id'"
      :action-column-key="'action'"
      entity-scope="company"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import DeptForm from './components/dept-form.vue'
import AssignDeptEmployees from './components/assign-dept-employees.vue'
import {
  getDeptTree,
  getDeptById,
  createDept,
  updateDept,
  deleteDeptById,
  getDeptTemplate,
  importDept,
  exportDept
} from '@/api/human-resource/organization/dept'
import type { Dept, DeptTree } from '@/types/human-resource/organization/dept'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { taktExcelEntityNames } from '@/utils/naming'
import { RiEditLine, RiDeleteBinLine, RiUserLine } from '@remixicon/vue'

const { t } = useI18n()
const deptExcelNames = taktExcelEntityNames('TaktDept')

const treeQueryKeyword = ref('')
const queryKeyword = ref('')
const treeExpanded = ref(false)
const treeExpandedKeys = ref<(string | number)[]>([])
const tableExpanded = ref(false)
/** 右侧扁平列表分页 */
const tableCurrentPage = ref(1)
const tablePageSize = ref(20)
const loading = ref(false)
const dataSource = ref<Dept[]>([])
/** 右侧树表数据源（受右侧查询条件影响） */
const fullTableTree = ref<Record<string, unknown>[]>([])
const deptTreeData = ref<TreeDataItem[]>([])
const selectedTreeKeys = ref<(string | number)[]>([])
const total = ref(0)
const selectedRow = ref<Dept | null>(null)
const selectedRows = ref<Dept[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('新增部门')
const formData = ref<Partial<Dept>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref<{ deptName: string; deptCode: string; deptStatus?: number }>({
  deptName: '',
  deptCode: '',
  deptStatus: undefined,
})
const importVisible = ref(false)
const assignDeptEmployeesVisible = ref(false)
const currentAssignDept = ref<Dept | null>(null)
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

/** 解析部门节点 key（与列表 deptId、左侧树 key 一致） */
function resolveDeptNodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.deptId ?? node.id
  return raw == null ? '' : String(raw)
}

/**
 * 将接口部门树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 部门树
 */
function deptTreeToTableNodes(nodes: DeptTree[]): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? deptTreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.deptId ?? ''),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

/** 将 fullTableTree 转为左侧 a-tree（与右侧表共用 key，保证点选联动） */
function mapFullTableTreeToTreeData(nodes: Array<Record<string, unknown>>): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes.map((n) => {
    const title = String(n.deptName ?? n.title ?? '')
    const key = resolveDeptNodeKey(n)
    const children = n.children as Array<Record<string, unknown>> | undefined
    if (!children?.length) return { title, key }
    const mapped = mapFullTableTreeToTreeData(children)
    return mapped.length > 0 ? { title, key, children: mapped } : { title, key }
  })
}

/**
 * 按 key 查找树节点（左侧树与右侧表共用 fullTableTree）
 * @param nodes 树节点列表
 * @param key 节点 key（deptId）
 */
function findTreeNodeByKey(
  nodes: Array<Record<string, unknown>>,
  key: string | number,
): Record<string, unknown> | null {
  const k = String(key)
  for (const node of nodes) {
    if (resolveDeptNodeKey(node) === k) return node
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

/** 按关键字过滤左侧树：保留 title 包含关键字的节点及其祖先、子孙 */
function filterTreeByKeyword(nodes: TreeDataItem[], keyword: string): TreeDataItem[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  function filter(nodes: TreeDataItem[]): TreeDataItem[] {
    if (!nodes?.length) return []
    return nodes
      .map(node => {
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

const filteredDeptTreeData = computed(() =>
  filterTreeByKeyword(deptTreeData.value, treeQueryKeyword.value)
)

/** 从树数据中收集所有有子节点的 key（用于左侧树展开全部） */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  if (!nodes?.length) return []
  const keys: (string | number)[] = []
  for (const node of nodes) {
    const rawKey = node.key ?? node.deptId ?? node.id
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
 * 将树表数据深度优先拍平（供右侧分页展示）
 * @param nodes 树节点
 * @param depth 层级缩进
 */
function flattenDeptTableRows(nodes: Array<Record<string, unknown>>, depth = 0): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  const rows: Array<Record<string, unknown>> = []
  for (const node of nodes) {
    const childList = node.children as Array<Record<string, unknown>> | undefined
    const { children: _children, ...rest } = node
    rows.push({ ...rest, _treeDepth: depth })
    if (childList?.length) {
      rows.push(...flattenDeptTableRows(childList, depth + 1))
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
    if (sub.length > 0) {
      return sub
    }
  }
  return tree
})

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树、不替换 fullTableTree） */
function matchesDeptRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    const code = String(record.deptCode ?? '').toLowerCase()
    const name = String(record.deptName ?? '').toLowerCase()
    if (!code.includes(k) && !name.includes(k)) return false
  }
  const adv = advancedQueryForm.value
  if (adv.deptName && !String(record.deptName ?? '').includes(adv.deptName)) return false
  if (adv.deptCode && !String(record.deptCode ?? '').includes(adv.deptCode)) return false
  if (adv.deptStatus !== undefined && record.deptStatus !== adv.deptStatus) return false
  return true
}

/** 右侧拍平后的全部行（先左侧子树，再右侧查询过滤） */
const tableFlatRows = computed(() =>
  flattenDeptTableRows(tableTreeData.value).filter(matchesDeptRightQuery)
)

/** 右侧拍平总行数（分页 total） */
const tableFlatTotal = computed(() => tableFlatRows.value.length)

/** 当前页行数据 */
const paginatedFlatTableRows = computed(() => {
  const start = (tableCurrentPage.value - 1) * tablePageSize.value
  return tableFlatRows.value.slice(start, start + tablePageSize.value)
})

watch(tableTreeData, () => {
  tableCurrentPage.value = 1
})

const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
  tableCurrentPage.value = 1
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0'
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.deptId ?? '')
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

const handleTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    deptTreeData.value = newTreeData
    const full = await getDeptById(String(dragKey))
    await updateDept(String(dragKey), {
      ...full,
      deptId: String(full.deptId ?? dragKey),
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    })
    message.success('排序/父级已更新')
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, '更新失败'))
    await loadFullDeptTree().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

/** 左侧树关键字搜索（客户端过滤，不重复请求接口） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredDeptTreeData.value)
  }
}

/** 左侧展开/收缩：工具栏展开状态与树展开 key 联动 */
watch(
  treeExpanded,
  (expanded) => {
    if (expanded) {
      treeExpandedKeys.value = collectTreeExpandableKeys(filteredDeptTreeData.value)
    } else {
      treeExpandedKeys.value = []
    }
  },
  { immediate: false }
)

watch(filteredDeptTreeData, () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredDeptTreeData.value)
  }
})

onMounted(() => {
  loadData()
})

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type DeptRowRecord = Dept | Record<string, unknown>

const getDeptId = (record: DeptRowRecord): string => {
  if (record != null && 'deptId' in record && record.deptId != null) return String(record.deptId)
  if (record != null && 'id' in record && record.id != null) return String(record.id)
  return ''
}
const getDeptField = (record: DeptRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getDeptDictValue = (
  record: DeptRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) {
    return undefined
  }
  if (typeof value === 'string' || typeof value === 'number') {
    return value
  }
  return String(value)
}

/**
 * 从 unknown 异常中读取可读消息
 * @param error 捕获的异常
 * @param fallback 无 message 时的兜底文案
 * @returns {string} 展示用错误消息
 */
const getErrorMessage = (error: unknown, fallback: string): string => {
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const messageText = (error as { message?: unknown }).message
    if (typeof messageText === 'string' && messageText.trim()) return messageText
  }
  return fallback
}

/**
 * 是否为 ant-design-vue 表单校验失败对象
 * @param error 捕获的异常
 * @returns {boolean} 是否校验错误
 */
const isFormValidationError = (error: unknown): boolean =>
  typeof error === 'object' && error !== null && 'errorFields' in error

const columns = ref<TableColumnsType>([])
watchEffect(() => {
  columns.value = [
  {
    title: 'ID',
    dataIndex: 'deptId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getDeptField(record, 'deptId') ?? getDeptField(record, 'id') ?? '',
  },
  {
    title: t('entity.dept.name'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 140,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.dept.code'),
    dataIndex: 'deptCode',
    key: 'deptCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.dept.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 90,
    ellipsis: true,
  },
  {
    title: t('entity.dept.headuserid'),
    dataIndex: 'headUserName',
    key: 'headUserName',
    width: 100,
    ellipsis: true,
  },
  {
    title: t('entity.dept.phone'),
    dataIndex: 'phone',
    key: 'phone',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.dept.costcentercode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.dept.costcategory'),
    dataIndex: 'costCategory',
    key: 'costCategory',
    width: 90,
  },
  {
    title: t('entity.dept.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 80,
  },
  {
    title: t('entity.dept.status'),
    dataIndex: 'deptStatus',
    key: 'deptStatus',
    width: 80,
  },
  CreateActionColumn<Dept>({
    actions: [
      { key: 'update', label: t('common.page.button.edit'), shape: 'plain', icon: RiEditLine, permission: 'humanresource:organization:dept:update', onClick: (record: Dept) => handleEdit(record) },
      { key: 'allocate-dept-user', label: t('common.page.button.allocate') + t('entity.employee._self'), shape: 'plain', icon: RiUserLine, permission: 'humanresource:organization:dept:update', onClick: (record: Dept) => handleAssignDeptEmployees(record) },
      { key: 'delete', label: t('common.page.button.delete'), shape: 'plain', icon: RiDeleteBinLine, permission: 'humanresource:organization:dept:delete', onClick: (record: Dept) => handleDeleteOne(record) },
    ],
  }),
  ]
})

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Dept[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: Dept, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getDeptId(selectedRow.value) === getDeptId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: Dept[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  }
}))

/**
 * 加载全量部门树（左右共用 fullTableTree / deptTreeData，点选联动；不受右侧查询影响）
 * @returns {Promise<void>}
 */
const loadFullDeptTree = async () => {
  const res = await getDeptTree('0', true)
  const resAny = res as { data?: DeptTree[]; Data?: DeptTree[] }
  const trees: DeptTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = deptTreeToTableNodes(trees)
  fullTableTree.value = tableNodes
  deptTreeData.value = mapFullTableTreeToTreeData(tableNodes)
  dataSource.value = flattenDeptTableRows(tableNodes) as unknown as Dept[]
  total.value = dataSource.value.length
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredDeptTreeData.value)
  }
}

/** 右侧查询/重置：仅客户端过滤 tableFlatRows，不请求接口、不替换 fullTableTree */
const applyRightTableQuery = () => {
  tableCurrentPage.value = 1
}

/** 初始化或增删改后刷新全量树 */
const loadData = async () => {
  loading.value = true
  try {
    await loadFullDeptTree()
  } catch (error: unknown) {
    logger.error('[Dept] 加载数据失败', undefined, error)
    message.error(getErrorMessage(error, '加载数据失败'))
    dataSource.value = []
    fullTableTree.value = []
    deptTreeData.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 右侧查询（不影响左侧树与 fullTableTree） */
const handleSearch = () => {
  applyRightTableQuery()
}

/** 右侧重置（不影响左侧树） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = { deptName: '', deptCode: '', deptStatus: undefined }
  applyRightTableQuery()
}

const handleTableChange = (_pagination: unknown, _filters: unknown, sorter: { field?: unknown; order?: unknown }) => {
  if (sorter?.order) logger.debug('[Dept] 排序', { field: sorter.field, order: sorter.order })
}

const handleResizeColumn = (w: number, col: { width?: number | string }) => {
  const colMeta = col as { key?: unknown; dataIndex?: unknown; title?: unknown }
  const column = columns.value.find((c: { key?: unknown; dataIndex?: unknown; title?: unknown }) => {
    const colKey = colMeta.key || colMeta.dataIndex || colMeta.title
    const cKey = c.key || c.dataIndex || c.title
    return colKey && cKey && String(colKey) === String(cKey)
  })
  if (column) (column as { width?: number }).width = w
}

const handleCreate = () => {
  formTitle.value = '新增部门'
  formData.value = {}
  formVisible.value = true
}

const handleEdit = (record: Dept) => {
  formTitle.value = '编辑部门'
  formData.value = { ...record }
  formVisible.value = true
}

const handleAssignDeptEmployees = (record: Dept) => {
  currentAssignDept.value = record
  assignDeptEmployeesVisible.value = true
}

const handleAssignSuccess = () => {
  loadData()
}

const handleUpdate = () => {
  if (selectedRow.value) handleEdit(selectedRow.value)
  else message.warning('请选择要编辑的部门')
}

const handleDeleteOne = (record: Dept) => {
  const name = getDeptField(record, 'deptName') || getDeptId(record)
  Modal.confirm({
    title: '确认删除',
    content: `确定要删除部门 "${name}" 吗？`,
    okText: '删除',
    cancelText: '取消',
    onOk: async () => {
      try {
        loading.value = true
        await deleteDeptById(getDeptId(record))
        message.success('删除成功')
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, '删除失败'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleDelete = () => {
  if (selectedRows.value.length === 0) {
    message.warning('请选择要删除的部门')
    return
  }
  Modal.confirm({
    title: '确认删除',
    content: `确定要删除选中的 ${selectedRows.value.length} 个部门吗？`,
    okText: '删除',
    cancelText: '取消',
    onOk: async () => {
      try {
        loading.value = true
        await Promise.all(selectedRows.value.map(record => deleteDeptById(getDeptId(record))))
        message.success('删除成功')
        selectedRows.value = []
        selectedRowKeys.value = []
        selectedRow.value = null
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, '删除失败'))
      } finally {
        loading.value = false
      }
    }
  })
}

const handleFormSubmit = async () => {
  try {
    if (!formRef.value) return
    await formRef.value.validate()
    const formValues = formRef.value.getValues()
    formLoading.value = true
    if (formData.value?.deptId) {
      await updateDept(formData.value.deptId, { ...formValues, deptId: formData.value.deptId })
      message.success('更新成功')
    } else {
      await createDept(formValues)
      message.success('创建成功')
    }
    formRef.value?.resetFields()
    formData.value = {}
    formVisible.value = false
    loadData()
  } catch (error: unknown) {
    if (isFormValidationError(error)) return
    message.error(getErrorMessage(error, '操作失败'))
  } finally {
    formLoading.value = false
  }
}

const handleFormCancel = () => {
  formVisible.value = false
  formData.value = {}
  formRef.value?.resetFields()
}

const handleImport = () => { importVisible.value = true }
const handleDownloadTemplate = async (sheetName?: string, fileName?: string) => {
  return await getDeptTemplate(sheetName, fileName)
}
const handleImportFile = async (file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> => {
  return await importDept(file, sheetName)
}
const handleImportSuccess = (result: { success: number; fail: number; errors: string[] }) => {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}
const handleImportCancel = () => { importVisible.value = false }

const handleExport = async () => {
  try {
    loading.value = true
    const queryParams: Record<string, unknown> = {}
    if (queryKeyword.value) queryParams.KeyWords = queryKeyword.value
    if (advancedQueryForm.value.deptName) queryParams.DeptName = advancedQueryForm.value.deptName
    if (advancedQueryForm.value.deptCode) queryParams.DeptCode = advancedQueryForm.value.deptCode
    if (advancedQueryForm.value.deptStatus !== undefined) queryParams.DeptStatus = advancedQueryForm.value.deptStatus
    const blob = await exportDept(queryParams, undefined, '部门数据')
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `部门数据_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success('导出成功')
  } catch (error: unknown) {
    message.error(getErrorMessage(error, '导出失败'))
  } finally {
    loading.value = false
  }
}

const handleAdvancedQuery = () => { advancedQueryVisible.value = true }
const handleAdvancedQuerySubmit = () => {
  applyRightTableQuery()
  advancedQueryVisible.value = false
}
const handleAdvancedQueryReset = () => {
  advancedQueryForm.value = { deptName: '', deptCode: '', deptStatus: undefined }
}

const handleColumnSetting = () => { columnSettingVisible.value = true }
const handleColumnKeysChange = (keys: (string | number)[]) => {
  visibleColumnKeys.value = keys.map(k => String(k))
}
const handleColumnSettingReset = () => { visibleColumnKeys.value = [] }

const handleRefresh = () => handleSearch()
</script>

<style scoped lang="css">
/* 边距由子组件（takt-tree-left-* / takt-tree-right-*）统一设置，本视图不重复设置 */
.organization-dept {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.dept-query-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
}

.dept-toolbar-row {
  display: flex;
  flex-direction: row;
  align-items: center;
  width: 100%;
  flex-wrap: nowrap;
  min-width: 0;
}

.dept-tree-table-wrap {
  flex: 1;
  min-height: 400px;
  display: flex;
  flex-direction: row;
  min-width: 0;
}
</style>
