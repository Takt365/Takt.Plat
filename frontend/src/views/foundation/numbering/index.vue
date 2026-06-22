<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/views/foundation/numbering -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：编号规则列表页，含查询、增删改、导出与状态切换 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-numbering">
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
      create-permission="foundation:numbering:create"
      update-permission="foundation:numbering:update"
      delete-permission="foundation:numbering:delete"
      export-permission="foundation:numbering:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-refresh="true"
      :create-disabled="false"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :export-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <!-- 表格 -->
    <div class="foundation-numbering-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getNumberingRowKey"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      @change="() => {}"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'dateFormat'">
          <TaktDictTag
            :value="record.dateFormat || 'none'"
            dict-type="sys_numbering_date_format_config"
          />
        </template>
        <template v-else-if="column.key === 'resetPeriod'">
          <TaktDictTag
            :value="mapResetPeriodDictValue(record.resetPeriod)"
            dict-type="sys_reset_period_config"
          />
        </template>
        <template v-else-if="column.key === 'isBuiltIn'">
          <a-switch
            :checked="record.isBuiltIn === 1"
            :checked-children="t('dict.sys.yes.no.type.1')"
            :un-checked-children="t('dict.sys.yes.no.type.0')"
          />
        </template>
        <template v-else-if="column.key === 'status'">
          <a-switch
            :checked="record.status === 1"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      </TaktSingleTable>
    </div>
    <!-- 分页 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    <!-- 新增/编辑弹窗 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <NumberingForm
        :key="formData?.numberingId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.numbering.rulecode')">
        <a-input
          v-model:value="advancedQueryForm.ruleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.rulename')">
        <a-input
          v-model:value="advancedQueryForm.ruleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulename') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('common.page.entity.companycode')">
        <a-input
          v-model:value="advancedQueryForm.companyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.departmentcode')">
        <TaktSelect
          v-model:value="advancedQueryForm.departmentCode"
          api-url="TaktIsoCodes/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.departmentcode') })"
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.isbuiltin')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') })"
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.status"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.status') })"
        />
      </a-form-item>
    </TaktQueryDrawer>
    <!-- 列设置 -->
    <TaktColumnDrawer
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'numberingId'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 编号规则列表页
 * @module views/foundation/numbering
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import NumberingForm from './components/numbering-form.vue'
import {
  getNumberingList,
  createNumbering,
  updateNumbering,
  deleteNumberingById,
  deleteNumberingBatch,
  updateNumberingStatus,
  exportNumbering
} from '@/api/foundation/numbering'
import type { Numbering, NumberingQuery, NumberingCreate, NumberingUpdate } from '@/types/foundation/numbering'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import { CreateActionColumn } from '@/components/business/takt-action-column'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeNumberingResetPeriod } from '@/utils/takt-numbering-reset-period'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktNumbering')
/** 表格 scroll.y（服务端分页固定视口高度；scroll.x 由 TaktSingleTable 按列宽累计） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const

/**
 * 列表展示用重置周期字典值
 * @param value 后端 resetPeriod
 * @returns {string} 字典 dictValue
 */
function mapResetPeriodDictValue(value?: string | null): string {
  return normalizeNumberingResetPeriod(value)
}
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.numbering._self') })
)

/**
 * 从异常对象提取 message
 * @param err 未知异常
 * @param fallback 兜底文案
 * @returns {string} 错误信息
 */
function pickErrorMessage(err: unknown, fallback: string): string {
  if (err !== null && typeof err === 'object' && 'message' in err) {
    const m = (err as { message?: unknown }).message
    if (typeof m === 'string' && m.length > 0) {
      return m
    }
  }
  return fallback
}

/**
 * 表格 row-key
 * @param record 行数据
 * @returns {string} numberingId
 */
const getNumberingRowKey = (record: unknown): string => {
  if (record == null || typeof record !== 'object') return ''
  const r = record as Record<string, unknown>
  const id = r['numberingId']
  return id != null && String(id) !== '' ? String(id) : ''
}

type NumberingTableColumn = TableColumnsType[number]

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Numbering[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Numbering | null>(null)
/** 表格多选行 */
const selectedRows = ref<Numbering[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])
/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题 */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Numbering> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref */
const formRef = ref()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  ruleCode: '',
  ruleName: '',
  companyCode: '',
  departmentCode: '',
  isBuiltIn: undefined as number | undefined,
  status: undefined as number | undefined
})
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 工具栏「编辑」是否禁用 */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用 */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 表格列定义 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'numberingId',
    key: 'numberingId',
    width: 120,
    fixed: 'left'
  },
  {
    title: t('entity.numbering.rulecode'),
    dataIndex: 'ruleCode',
    key: 'ruleCode',
    width: 140,
    ellipsis: true
  },
  {
    title: t('entity.numbering.rulename'),
    dataIndex: 'ruleName',
    key: 'ruleName',
    width: 160,
    ellipsis: true
  },
  {
    title: t('entity.numbering.documenttype'),
    dataIndex: 'documentType',
    key: 'documentType',
    width: 120,
    ellipsis: true
  },
  {
    title: t('entity.numbering.departmentcode'),
    dataIndex: 'departmentCode',
    key: 'departmentCode',
    width: 120
  },
  {
    title: t('entity.numbering.prefixcode'),
    dataIndex: 'prefixCode',
    key: 'prefixCode',
    width: 80
  },
  {
    title: t('entity.numbering.dateformat'),
    dataIndex: 'dateFormat',
    key: 'dateFormat',
    width: 100
  },
  {
    title: t('entity.numbering.sequencelength'),
    dataIndex: 'sequenceLength',
    key: 'sequenceLength',
    width: 90
  },
  {
    title: t('entity.numbering.suffixcode'),
    dataIndex: 'suffixCode',
    key: 'suffixCode',
    width: 80
  },
  {
    title: t('entity.numbering.currentsequence'),
    dataIndex: 'currentSequence',
    key: 'currentSequence',
    width: 100
  },
  {
    title: t('entity.numbering.examplecode'),
    dataIndex: 'exampleCode',
    key: 'exampleCode',
    width: 200,
    ellipsis: true
  },
  {
    title: t('entity.numbering.sequencestep'),
    dataIndex: 'sequenceStep',
    key: 'sequenceStep',
    width: 70
  },
  {
    title: t('entity.numbering.resetperiod'),
    dataIndex: 'resetPeriod',
    key: 'resetPeriod',
    width: 100
  },
  {
    title: t('entity.numbering.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 100
  },
  {
    title: t('entity.numbering.status'),
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  CreateActionColumn<Numbering>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:numbering:update',
        onClick: (r: Numbering) => handleEdit(r)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:numbering:delete',
        onClick: (r: Numbering) => handleDeleteOne(r)
      }
    ]
  })
])

/** 表格行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Numbering[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Numbering, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value?.numberingId === record.numberingId) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Numbering[]) => {
    if (selected) {
      selectedRow.value = selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    } else {
      selectedRow.value = null
    }
  }
}))

/**
 * 行点击切换选中
 * @param record 当前行
 */
const onClickRow = (record: Numbering) => ({
  onClick: () => {
    const key = record.numberingId || ''
    const idx = selectedRowKeys.value.indexOf(key)
    if (idx > -1) {
      selectedRowKeys.value.splice(idx, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item: Numbering) => selectedRowKeys.value.includes(item.numberingId || ''))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    rowSelection.value.onChange?.(selectedRowKeys.value, selectedRows.value)
  }
})

/**
 * 合并高级查询条件到分页请求
 * @param params 分页查询 DTO
 */
function applyAdvancedQuery(params: NumberingQuery) {
  const adv = advancedQueryForm.value
  if (adv.ruleCode) params.ruleCode = adv.ruleCode
  if (adv.ruleName) params.ruleName = adv.ruleName
  if (adv.companyCode) params.companyCode = adv.companyCode
  if (adv.departmentCode) params.departmentCode = adv.departmentCode
  if (adv.isBuiltIn !== undefined) params.isBuiltIn = adv.isBuiltIn
  if (adv.status !== undefined) params.status = adv.status
}

/** 加载分页列表 */
async function loadData() {
  try {
    loading.value = true
    const params: NumberingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    if (queryKeyword.value) {
      params.keyWords = queryKeyword.value
    }
    applyAdvancedQuery(params)
    const res = await getNumberingList(params)
    dataSource.value = res?.data ?? []
    total.value = res?.total ?? 0
  } catch (e: unknown) {
    logger.error('[Numbering] loadData error', undefined, e)
    message.error(pickErrorMessage(e, t('common.feedback.load.data.failed')))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时刷新列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询并刷新 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
    ruleCode: '',
    ruleName: '',
    companyCode: '',
    departmentCode: '',
    isBuiltIn: undefined,
    status: undefined
  }
  currentPage.value = 1
  loadData()
}

/**
 * 列宽拖拽
 * @param w 新宽度
 * @param col 列定义
 */
function handleResizeColumn(w: number, col: NumberingTableColumn) {
  const resolveColPart = (x: NumberingTableColumn) => {
    const c = x as { key?: unknown; dataIndex?: unknown; title?: unknown }
    return c.key ?? c.dataIndex ?? c.title
  }
  const colKey = resolveColPart(col)
  const column = columns.value.find((c: NumberingTableColumn) => {
    const cKey = resolveColPart(c)
    return colKey != null && cKey != null && String(colKey) === String(cKey)
  }) as { width?: number } | undefined
  if (column) {
    column.width = w
  }
}

/**
 * 分页页码变更
 * @param page 页码
 * @param size 每页条数
 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/**
 * 每页条数变更
 * @param _current 当前页
 * @param size 每页条数
 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.numbering._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/**
 * 打开编辑弹窗
 * @param record 当前行
 */
function handleEdit(record: Numbering) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.numbering._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.numbering._self') }))
  }
}

/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleStatusChange(record: Numbering, checked: boolean) {
  const newStatus = checked ? 1 : 0
  const oldStatus = record.status
  const idx = dataSource.value.findIndex((r: Numbering) => r.numberingId === record.numberingId)
  const row = idx !== -1 ? dataSource.value[idx] : undefined
  if (row) {
    row.status = newStatus
  }
  try {
    await updateNumberingStatus({ numberingId: record.numberingId, status: newStatus })
    message.success(t('common.feedback.updated', { target: t('entity.numbering._self') }))
  } catch (e: unknown) {
    if (row) {
      row.status = oldStatus
    }
    message.error(pickErrorMessage(e, t('common.feedback.update.failed', { target: t('entity.numbering._self') })))
  }
}

/**
 * 单条删除
 * @param record 当前行
 */
function handleDeleteOne(record: Numbering) {
  const name = record.ruleName || record.ruleCode || t('common.tip.this.target', { target: t('entity.numbering._self') })
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.numbering._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteNumberingById(String(record.numberingId))
        message.success(t('common.feedback.deleted', { target: t('entity.numbering._self') }))
        loadData()
      } catch (e: unknown) {
        message.error(pickErrorMessage(e, t('common.feedback.delete.failed', { target: t('entity.numbering._self') })))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 批量删除 */
function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.numbering._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.numbering._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteNumberingBatch(selectedRows.value.map((r: Numbering) => String(r.numberingId)))
        message.success(t('common.feedback.deleted', { target: t('entity.numbering._self') }))
        selectedRowKeys.value = []
        selectedRows.value = []
        selectedRow.value = null
        loadData()
      } catch (e: unknown) {
        message.error(pickErrorMessage(e, t('common.feedback.delete.failed', { target: t('entity.numbering._self') })))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 提交高级查询 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadData()
  advancedQueryVisible.value = false
}

/** 重置高级查询表单 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
    ruleCode: '',
    ruleName: '',
    companyCode: '',
    departmentCode: '',
    isBuiltIn: undefined,
    status: undefined
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/**
 * 列设置可见列变更
 * @param keys 可见列 key
 */
function handleColumnKeysChange(keys: (string | number)[]) {
  visibleColumnKeys.value = keys.map(k => String(k))
}

/** 列设置恢复默认 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 导出当前查询结果 */
async function handleExport() {
  try {
    loading.value = true
    const query: NumberingQuery = {
      pageIndex: 1,
      pageSize: 99999
    }
    if (queryKeyword.value) {
      query.keyWords = queryKeyword.value
    }
    applyAdvancedQuery(query)
    const blob = await exportNumbering(query, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: null,
      contentType: blob.type || null,
      fallbackBase
    })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.numbering._self') }))
  } catch (e: unknown) {
    message.error(pickErrorMessage(e, t('common.feedback.export.failed', { target: t('entity.numbering._self') })))
  } finally {
    loading.value = false
  }
}

/** 弹窗确定：校验并提交表单 */
async function handleFormSubmit() {
  if (!formRef.value) return
  try {
    await formRef.value.validate()
    const values = formRef.value.getValues()
    formLoading.value = true
    if ('numberingId' in values && values.numberingId) {
      await updateNumbering(values.numberingId, values as NumberingUpdate)
      message.success(t('common.feedback.updated', { target: t('entity.numbering._self') }))
    } else {
      await createNumbering(values as NumberingCreate)
      message.success(t('common.feedback.created', { target: t('entity.numbering._self') }))
    }
    formVisible.value = false
    formData.value = null
    nextTick(() => formRef.value?.resetFields())
    loadData()
  } catch (e: unknown) {
    if (e !== null && typeof e === 'object' && 'errorFields' in e) {
      return
    }
    message.error(pickErrorMessage(e, t('common.feedback.save.failed', { target: t('entity.numbering._self') })))
  } finally {
    formLoading.value = false
  }
}

/** 关闭弹窗 */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

onMounted(() => loadData())
</script>

<style scoped lang="css">
.foundation-numbering {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.foundation-numbering-table-wrap {
  flex: 1;
  min-height: 0;
  min-width: 0;
}
</style>
