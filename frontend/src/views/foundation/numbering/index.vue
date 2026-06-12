<template>
  <div class="routine-numbering-rule">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('routine.tasks.numbering-rule.page.listSearchPlaceholder')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
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
      :update-disabled="!selectedRow"
      :delete-disabled="selectedRows.length === 0"
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
    <TaktSingleTable
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
        <template v-if="column.key === 'status'">
          <a-switch
            :checked="record.status === 1"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="720"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <NumberingForm
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
      <a-form-item :label="t('routine.tasks.numbering-rule.advanced.ruleCode')">
        <a-input
          v-model:value="advancedQueryForm.ruleCode"
          :placeholder="t('routine.tasks.numbering-rule.advanced.placeholderRuleCode')"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('routine.tasks.numbering-rule.advanced.ruleName')">
        <a-input
          v-model:value="advancedQueryForm.ruleName"
          :placeholder="t('routine.tasks.numbering-rule.advanced.placeholderRuleName')"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('routine.tasks.numbering-rule.advanced.companyCode')">
        <a-input
          v-model:value="advancedQueryForm.companyCode"
          :placeholder="t('routine.tasks.numbering-rule.advanced.placeholderCompanyCode')"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.departmentcode')">
        <a-input
          v-model:value="advancedQueryForm.departmentCode"
          :placeholder="t('routine.tasks.numbering-rule.advanced.placeholderDeptCode')"
          allow-clear
        />
      </a-form-item>
      <a-form-item :label="t('entity.numbering.status')">
        <a-select
          v-model:value="advancedQueryForm.status"
          :placeholder="t('common.page.form.placeholder.selectonly')"
          allow-clear
          :options="statusSelectOptions"
        />
      </a-form-item>
    </TaktQueryDrawer>
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
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
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

const { t } = useI18n()

function pickErrorMessage(err: unknown, fallback: string): string {
  if (err !== null && typeof err === 'object' && 'message' in err) {
    const m = (err as { message?: unknown }).message
    if (typeof m === 'string' && m.length > 0) {
      return m
    }
  }
  return fallback
}

/** TaktSingleTable 的 rowKey 入参为 TableRecord，按 unknown 解析 numberingId。 */
const getNumberingRowKey = (record: unknown): string => {
  if (record == null || typeof record !== 'object') return ''
  const r = record as Record<string, unknown>
  const id = r['numberingId']
  return id != null && String(id) !== '' ? String(id) : ''
}

type NumberingTableColumn = TableColumnsType[number]

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Numbering[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Numbering | null>(null)
const selectedRows = ref<Numbering[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Numbering>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ruleCode: '',
  ruleName: '',
  companyCode: '',
  departmentCode: '',
  status: undefined as number | undefined
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

const statusSelectOptions = computed(() => [
  { label: t('common.page.button.enable'), value: 1 },
  { label: t('common.page.button.disable'), value: 0 }
])

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
    width: 100
  },
  {
    title: t('entity.numbering.departmentcode'),
    dataIndex: 'departmentCode',
    key: 'departmentCode',
    width: 120
  },
  {
    title: t('entity.numbering.prefix'),
    dataIndex: 'prefix',
    key: 'prefix',
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
    title: t('entity.numbering.suffix'),
    dataIndex: 'suffix',
    key: 'suffix',
    width: 80
  },
  {
    title: t('entity.numbering.currentsequence'),
    dataIndex: 'currentSequence',
    key: 'currentSequence',
    width: 100
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
    title: t('entity.numbering.status'),
    dataIndex: 'status',
    key: 'status',
    width: 100
  },
  {
    title: t('common.page.entity.createtime'),
    dataIndex: 'createdAt',
    key: 'createdAt',
    width: 160
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
    const adv = advancedQueryForm.value
    if (adv.ruleCode) params.ruleCode = adv.ruleCode
    if (adv.ruleName) params.ruleName = adv.ruleName
    if (adv.companyCode) params.companyCode = adv.companyCode
    if (adv.departmentCode) params.departmentCode = adv.departmentCode
    if (adv.status !== undefined) {
      params.status = adv.status
    }
    const res = await getNumberingList(params)
    dataSource.value = res?.data ?? []
    total.value = res?.total ?? 0
  } catch (e: unknown) {
    logger.error('[Numbering] loadData error', undefined, e)
    message.error(pickErrorMessage(e, t('routine.tasks.numbering-rule.messages.loadFail')))
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

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
    ruleCode: '',
    ruleName: '',
    companyCode: '',
    departmentCode: '',
    status: undefined
  }
  currentPage.value = 1
  loadData()
}

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

function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  loadData()
}

function handleCreate() {
  formTitle.value = t('routine.tasks.numbering-rule.page.formCreate')
  formData.value = {}
  formVisible.value = true
}

function handleEdit(record: Numbering) {
  formTitle.value = t('routine.tasks.numbering-rule.page.formEdit')
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('routine.tasks.numbering-rule.messages.selectOne'))
  }
}

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
    message.success(checked ? t('routine.tasks.numbering-rule.messages.statusEnabled') : t('routine.tasks.numbering-rule.messages.statusDisabled'))
  } catch (e: unknown) {
    if (row) {
      row.status = oldStatus
    }
    message.error(pickErrorMessage(e, t('common.page.msg.operatefail')))
  }
}

function handleDeleteOne(record: Numbering) {
  const name = record.ruleName || record.ruleCode || ''
  Modal.confirm({
    title: t('common.page.action.confirmdelete'),
    content: t('common.page.confirm.deleteentity', {
      entity: t('routine.tasks.numbering-rule.page.entityName'),
      name
    }),
    onOk: async () => {
      try {
        loading.value = true
        await deleteNumberingById(String(record.numberingId))
        message.success(t('common.page.msg.deletesuccess'))
        loadData()
      } catch (e: unknown) {
        message.error(pickErrorMessage(e, t('common.page.msg.deletefail')))
      } finally {
        loading.value = false
      }
    }
  })
}

function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('routine.tasks.numbering-rule.messages.selectDelete'))
    return
  }
  Modal.confirm({
    title: t('common.page.action.confirmdelete'),
    content: t('common.page.confirm.deletecountentity', {
      count: selectedRows.value.length,
      entity: t('routine.tasks.numbering-rule.page.entityName')
    }),
    onOk: async () => {
      try {
        loading.value = true
        await deleteNumberingBatch(selectedRows.value.map((r: Numbering) => String(r.numberingId)))
        message.success(t('common.page.msg.deletesuccess'))
        selectedRowKeys.value = []
        selectedRows.value = []
        selectedRow.value = null
        loadData()
      } catch (e: unknown) {
        message.error(pickErrorMessage(e, t('common.page.msg.deletefail')))
      } finally {
        loading.value = false
      }
    }
  })
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadData()
  advancedQueryVisible.value = false
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
    ruleCode: '',
    ruleName: '',
    companyCode: '',
    departmentCode: '',
    status: undefined
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: (string | number)[]) {
  visibleColumnKeys.value = keys.map(k => String(k))
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

function handleRefresh() {
  loadData()
}

function padNum(n: number): string {
  return n < 10 ? `0${n}` : String(n)
}

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
    const adv = advancedQueryForm.value
    if (adv.ruleCode) query.ruleCode = adv.ruleCode
    if (adv.ruleName) query.ruleName = adv.ruleName
    if (adv.companyCode) query.companyCode = adv.companyCode
    if (adv.departmentCode) query.departmentCode = adv.departmentCode
    if (adv.status !== undefined) {
      query.status = adv.status
    }
    const exportLabel = t('routine.tasks.numbering-rule.page.exportDataLabel')
    const blob = await exportNumbering(query, undefined, exportLabel)
    const ts = new Date()
    const fileName = `${exportLabel}_${ts.getFullYear()}${padNum(ts.getMonth() + 1)}${padNum(ts.getDate())}${padNum(ts.getHours())}${padNum(ts.getMinutes())}${padNum(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.click()
    window.URL.revokeObjectURL(url)
    message.success(t('common.page.msg.exportsuccess'))
  } catch (e: unknown) {
    message.error(pickErrorMessage(e, t('common.page.msg.exportfail')))
  } finally {
    loading.value = false
  }
}

async function handleFormSubmit() {
  if (!formRef.value) return
  try {
    await formRef.value.validate()
    const values = formRef.value.getValues()
    formLoading.value = true
    if ('numberingId' in values && values.numberingId) {
      await updateNumbering(values.numberingId, values as NumberingUpdate)
      message.success(t('common.page.msg.updatesuccess'))
    } else {
      await createNumbering(values as NumberingCreate)
      message.success(t('common.page.msg.createsuccess'))
    }
    formVisible.value = false
    formData.value = {}
    loadData()
  } catch (e: unknown) {
    if (e !== null && typeof e === 'object' && 'errorFields' in e) {
      return
    }
    message.error(pickErrorMessage(e, t('common.page.msg.operatefail')))
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
  formData.value = {}
}

onMounted(() => loadData())
</script>

<style scoped lang="css">
.routine-numbering-rule {
  padding: 16px;
}
</style>
