<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/dept -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：部门实体 代表组织架构中的部门树表管理页（左树右表），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-organization-dept">
    <!-- 查询栏 -->
    <div class="human-resource-organization-dept-query-row">
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
    <div class="human-resource-organization-dept-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFullDeptTree"
      />
      <TaktTreeRightToolsBar
        create-permission="human:resource:organization:dept:create"
        update-permission="human:resource:organization:dept:update"
        delete-permission="human:resource:organization:dept:delete"
        import-permission="human:resource:organization:dept:import"
        export-permission="human:resource:organization:dept:export"
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
    <div class="human-resource-organization-dept-tree-table-wrap">
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
        :id-column-key="'deptId'"
        :action-column-key="'action'"
        table-mode="tree"
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
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'deptName'">
            <span
              class="inline-block"
              :style="{ paddingLeft: `${(record._treeDepth ?? 0) * 16}px` }"
            >
              {{ getDeptField(record, 'deptName') }}
            </span>
          </template>
        <template v-else-if="column.key === 'deptStatus'">
          <a-switch
            :checked="getDeptField(record, 'deptStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleDeptStatusChange(record, Boolean(checked))"
          />
        </template>
          <template v-else-if="column.key === 'isBuiltIn'">
            <TaktDictTag
              :value="getDeptDictValue(record, 'isBuiltIn')"
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
      <DeptForm
        :key="formData?.deptId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-organization-dept'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('deptCode')">
      <a-form-item :label="t('entity.dept.code')">
        <a-input
          v-model:value="advancedQueryForm.deptCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.dept.name')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.name') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="t('entity.dept.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.parentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('level')">
      <a-form-item :label="t('entity.dept.level')">
        <a-input-number
          v-model:value="advancedQueryForm.level"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptPath')">
      <a-form-item :label="t('entity.dept.path')">
        <a-input
          v-model:value="advancedQueryForm.deptPath"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.path') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLeaf')">
      <a-form-item :label="t('entity.dept.isleaf')">
        <a-input-number
          v-model:value="advancedQueryForm.isLeaf"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.isleaf') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="t('entity.dept.costcentercode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.costcentercode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCategory')">
      <a-form-item :label="t('entity.dept.costcategory')">
        <a-input-number
          v-model:value="advancedQueryForm.costCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.costcategory') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headUserId')">
      <a-form-item :label="t('entity.dept.headuserid')">
        <a-input
          v-model:value="advancedQueryForm.headUserId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.headuserid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('phone')">
      <a-form-item :label="t('entity.dept.phone')">
        <a-input
          v-model:value="advancedQueryForm.phone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.phone') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('email')">
      <a-form-item :label="t('entity.dept.email')">
        <a-input
          v-model:value="advancedQueryForm.email"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.email') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('location')">
      <a-form-item :label="t('entity.dept.location')">
        <a-input
          v-model:value="advancedQueryForm.location"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.location') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptStatus')">
      <a-form-item :label="t('entity.dept.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.deptStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.dept.isbuiltin')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.isbuiltin') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.dept.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.dept.description') })"
          :rows="2"
          allow-clear
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
      :title="t('common.dialog.title.import', { entity: t('entity.dept._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.dept._self"
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
      :id-column-key="'deptId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 部门实体 代表组织架构中的部门树表管理页 · ParentId 左树右表（参照 dept/index.vue）
 * @module views/human-resource/organization/dept
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import DeptForm from './components/dept-form.vue'
import { getDeptTree, getDeptById, createDept, updateDept, deleteDeptById, deleteDeptBatch, getDeptTemplate, importDept, exportDept, updateDeptStatus } from '@/api/human-resource/organization/dept'
import type { Dept, DeptTree, DeptUpdate } from '@/types/human-resource/organization/dept'
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
const excelNames = taktExcelEntityNames('TaktDept')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [t('entity.dept.name'), t('entity.dept.code')].join(' / '),
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
const selectedRow = ref<Dept | null>(null)
/** 表格多选行 */
const selectedRows = ref<Dept[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Dept> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  deptCode: '',
  deptName: '',
  parentId: '',
  level: undefined as number | undefined,
  deptPath: '',
  isLeaf: undefined as number | undefined,
  costCenterCode: '',
  costCategory: undefined as number | undefined,
  headUserId: '',
  phone: '',
  email: '',
  location: '',
  deptStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'deptCode', label: t('entity.dept.code') },
  { key: 'deptName', label: t('entity.dept.name') },
  { key: 'parentId', label: t('entity.dept.parentid') },
  { key: 'level', label: t('entity.dept.level') },
  { key: 'deptPath', label: t('entity.dept.path') },
  { key: 'isLeaf', label: t('entity.dept.isleaf') },
  { key: 'costCenterCode', label: t('entity.dept.costcentercode') },
  { key: 'costCategory', label: t('entity.dept.costcategory') },
  { key: 'headUserId', label: t('entity.dept.headuserid') },
  { key: 'phone', label: t('entity.dept.phone') },
  { key: 'email', label: t('entity.dept.email') },
  { key: 'location', label: t('entity.dept.location') },
  { key: 'deptStatus', label: t('entity.dept.status') },
  { key: 'isBuiltIn', label: t('entity.dept.isbuiltin') },
  { key: 'description', label: t('entity.dept.description') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'deptId'
/** 树节点标题字段名（左侧树 title 与缩进列） */
const treeTitleField = 'deptName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 解析树节点 key（与列表 deptId、左侧树 key 一致） */
function resolveDeptNodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.deptId ?? node.id
  return raw == null ? '' : String(raw)
}

/**
 * 将接口树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 实体树 DTO 列表
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
    const title = String(n[treeTitleField] ?? n.title ?? '')
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
 * @param key 节点 key
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
 * 深度优先拍平树表行（附带 _treeDepth 供缩进列渲染）
 * @param nodes 树表节点
 * @param depth 当前层级
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
    if (sub.length > 0) return sub
  }
  return tree
})

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matchesDeptRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.deptName ?? '').toLowerCase().includes(k)) return false
    if (!String(record.deptCode ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.deptCode && !String(record.deptCode ?? '').includes(String(advancedQueryForm.value.deptCode))) return false
  if (advancedQueryForm.value.deptName && !String(record.deptName ?? '').includes(String(advancedQueryForm.value.deptName))) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.level !== undefined && record.level !== advancedQueryForm.value.level) return false
  if (advancedQueryForm.value.deptPath && !String(record.deptPath ?? '').includes(String(advancedQueryForm.value.deptPath))) return false
  if (advancedQueryForm.value.isLeaf !== undefined && record.isLeaf !== advancedQueryForm.value.isLeaf) return false
  if (advancedQueryForm.value.costCenterCode && !String(record.costCenterCode ?? '').includes(String(advancedQueryForm.value.costCenterCode))) return false
  if (advancedQueryForm.value.costCategory !== undefined && record.costCategory !== advancedQueryForm.value.costCategory) return false
  if (advancedQueryForm.value.headUserId && !String(record.headUserId ?? '').includes(String(advancedQueryForm.value.headUserId))) return false
  if (advancedQueryForm.value.phone && !String(record.phone ?? '').includes(String(advancedQueryForm.value.phone))) return false
  if (advancedQueryForm.value.email && !String(record.email ?? '').includes(String(advancedQueryForm.value.email))) return false
  if (advancedQueryForm.value.location && !String(record.location ?? '').includes(String(advancedQueryForm.value.location))) return false
  if (advancedQueryForm.value.deptStatus !== undefined && record.deptStatus !== advancedQueryForm.value.deptStatus) return false
  if (advancedQueryForm.value.isBuiltIn !== undefined && record.isBuiltIn !== advancedQueryForm.value.isBuiltIn) return false
  if (advancedQueryForm.value.description && !String(record.description ?? '').includes(String(advancedQueryForm.value.description))) return false
  if (advancedQueryForm.value.createdAtStart && !String(record.createdAtStart ?? '').includes(String(advancedQueryForm.value.createdAtStart))) return false
  if (advancedQueryForm.value.createdAtEnd && !String(record.createdAtEnd ?? '').includes(String(advancedQueryForm.value.createdAtEnd))) return false
  if (advancedQueryForm.value.extField && !String(record.extField ?? '').includes(String(advancedQueryForm.value.extField))) return false
  if (advancedQueryForm.value.remark && !String(record.remark ?? '').includes(String(advancedQueryForm.value.remark))) return false
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
 * @param dept 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {DeptUpdate} 更新载荷
 */
function buildDeptUpdateDto(
  dept: Dept,
  overrides: Pick<DeptUpdate, 'parentId' | 'sortOrder'>,
): DeptUpdate {
  return {
    deptId: String(dept.deptId),
    tenantCode: dept.tenantCode,
    companyCode: dept.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    deptCode: dept.deptCode,
    deptName: dept.deptName,
    parentId: overrides.parentId,
    costCenterCode: dept.costCenterCode,
    costCategory: dept.costCategory,
    headUserId: dept.headUserId,
    phone: dept.phone,
    email: dept.email,
    location: dept.location,
    deptStatus: dept.deptStatus,
    isBuiltIn: dept.isBuiltIn,
    description: dept.description,
    roleIds: dept.roleIds,
    employeeIds: dept.employeeIds,
    extField: dept.extField,
    remark: dept.remark,
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
    const full = await getDeptById(String(dragKey))
    await updateDept(String(dragKey), buildDeptUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    message.success(t('common.feedback.updated', { target: t('entity.dept._self') }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: t('entity.dept._self') })))
    await loadFullDeptTree().catch(() => undefined)
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
type DeptRowRecord = Dept | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getDeptId = (record: DeptRowRecord): string => {
  if (record != null && 'deptId' in record && (record as Record<string, unknown>).deptId != null) {
    return String((record as Record<string, unknown>).deptId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getDeptField = (record: DeptRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getDeptDictValue = (
  record: DeptRowRecord,
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
    dataIndex: 'deptId',
    key: 'deptId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getDeptField(record, 'deptId') ?? getDeptField(record, 'id') ?? '',
  },
  {
    title: t('entity.dept.code'),
    dataIndex: 'deptCode',
    key: 'deptCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'deptCode') ?? ''
  },
  {
    title: t('entity.dept.name'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 160,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.dept.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.dept.level'),
    dataIndex: 'level',
    key: 'level',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'level') ?? ''
  },
  {
    title: t('entity.dept.path'),
    dataIndex: 'deptPath',
    key: 'deptPath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'deptPath') ?? ''
  },
  {
    title: t('entity.dept.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'isLeaf') ?? ''
  },
  {
    title: t('entity.dept.costcentercode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'costCenterCode') ?? ''
  },
  {
    title: t('entity.dept.costcategory'),
    dataIndex: 'costCategory',
    key: 'costCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'costCategory') ?? ''
  },
  {
    title: t('entity.dept.headuserid'),
    dataIndex: 'headUserId',
    key: 'headUserId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'headUserId') ?? ''
  },
  {
    title: t('entity.dept.phone'),
    dataIndex: 'phone',
    key: 'phone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'phone') ?? ''
  },
  {
    title: t('entity.dept.email'),
    dataIndex: 'email',
    key: 'email',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'email') ?? ''
  },
  {
    title: t('entity.dept.location'),
    dataIndex: 'location',
    key: 'location',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'location') ?? ''
  },
  {
    title: t('entity.dept.status'),
    dataIndex: 'deptStatus',
    key: 'deptStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.dept.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.dept.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'description') ?? ''
  },
  {
    title: t('entity.dept.roledepts'),
    dataIndex: 'roleDepts',
    key: 'roleDepts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'roleDepts') ?? ''
  },
  {
    title: t('entity.dept.employeedepts'),
    dataIndex: 'employeeDepts',
    key: 'employeeDepts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getDeptField(record, 'employeeDepts') ?? ''
  },
  CreateActionColumn<Dept>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:organization:dept:update',
        onClick: (record: Dept) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:organization:dept:delete',
        onClick: (record: Dept) => handleDeleteOne(record)
      }
    ],
  })]
})

/** 行选择配置 */
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
  },
}))

/** 加载全量树（左侧树 + 右侧树表共用数据源） */
async function loadFullDeptTree() {
  const res = await getDeptTree('0', true)
  const resAny = res as { data?: DeptTree[]; Data?: DeptTree[] }
  const trees: DeptTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = deptTreeToTableNodes(trees)
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
    await loadFullDeptTree()
  } catch (error: unknown) {
    logger.error('[Dept] 加载树数据失败', undefined, error)
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
  deptCode: '',
  deptName: '',
  parentId: '',
  level: undefined as number | undefined,
  deptPath: '',
  isLeaf: undefined as number | undefined,
  costCenterCode: '',
  costCategory: undefined as number | undefined,
  headUserId: '',
  phone: '',
  email: '',
  location: '',
  deptStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
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
async function handleDeptStatusChange(record: DeptRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getDeptField(record, 'deptStatus')
  const id = getDeptId(record)
  const row = null
  if (row) {
    row.deptStatus = newVal
  }
  try {
    await updateDeptStatus({ deptId: id, deptStatus: newVal })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: unknown) {
    if (row) {
      row.deptStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
}

/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.dept._self') })
  const keys = selectedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/** 打开编辑弹窗 */
function handleEdit(record: Dept) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.dept._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.dept._self') }))
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
      await updateDept(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.dept._self') }))
    } else {
      await createDept(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.dept._self') }))
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
async function handleDeleteOne(record: Dept) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.dept._self'), name: t('common.tip.this.target', { target: t('entity.dept._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDeptById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.dept._self') }))
      await loadData()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.dept._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.dept._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteDeptBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.dept._self') }))
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
  const res = await getDeptTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importDept(file, sheetName)
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
    const exportMeta = await exportDept({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.dept._self') }))
  } catch (error: unknown) {
    logger.error('[Dept] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: t('entity.dept._self') })))
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
  deptCode: '',
  deptName: '',
  parentId: '',
  level: undefined as number | undefined,
  deptPath: '',
  isLeaf: undefined as number | undefined,
  costCenterCode: '',
  costCategory: undefined as number | undefined,
  headUserId: '',
  phone: '',
  email: '',
  location: '',
  deptStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
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
.human-resource-organization-dept {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.human-resource-organization-dept-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.human-resource-organization-dept-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.human-resource-organization-dept-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
