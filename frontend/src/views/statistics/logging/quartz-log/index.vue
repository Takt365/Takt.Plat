<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/logging/quartz-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 任务执行日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-logging-quartz-log">
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
      delete-permission="statistics:logging:quartzlog:delete"
      export-permission="statistics:logging:quartzlog:export"
      :show-create="false"
      :show-update="false"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'quartzLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQuartzLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 详情对话框 -->
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.quartzlog._self') })"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <QuartzLogDetail :detail="detailData" />
      </a-spin>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-statistics-logging-quartz-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('quartzTaskId')">
      <a-form-item :label="t('entity.quartzlog.quartztaskid')">
        <a-input
          v-model:value="advancedQueryForm.quartzTaskId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.quartztaskid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskName')">
      <a-form-item :label="t('entity.quartzlog.taskname')">
        <a-input
          v-model:value="advancedQueryForm.taskName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.taskname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGroup')">
      <a-form-item :label="t('entity.quartzlog.jobgroup')">
        <a-input-number
          v-model:value="advancedQueryForm.jobGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.jobgroup') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskType')">
      <a-form-item :label="t('entity.quartzlog.tasktype')">
        <a-input-number
          v-model:value="advancedQueryForm.taskType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.tasktype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeTimeStart')">
      <a-form-item :label="t('entity.quartzlog.executetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.executeTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeTimeEnd')">
      <a-form-item :label="t('entity.quartzlog.executetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.executeTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeDuration')">
      <a-form-item :label="t('entity.quartzlog.executeduration')">
        <a-input
          v-model:value="advancedQueryForm.executeDuration"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeduration') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeParams')">
      <a-form-item :label="t('entity.quartzlog.executeparams')">
        <a-input
          v-model:value="advancedQueryForm.executeParams"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeparams') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeMessage')">
      <a-form-item :label="t('entity.quartzlog.executemessage')">
        <a-input
          v-model:value="advancedQueryForm.executeMessage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executemessage') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('errorInfo')">
      <a-form-item :label="t('entity.quartzlog.errorinfo')">
        <a-input
          v-model:value="advancedQueryForm.errorInfo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.errorinfo') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeIp')">
      <a-form-item :label="t('entity.quartzlog.executeip')">
        <a-input
          v-model:value="advancedQueryForm.executeIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeip') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeHost')">
      <a-form-item :label="t('entity.quartzlog.executehost')">
        <a-input
          v-model:value="advancedQueryForm.executeHost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executehost') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeStatus')">
      <a-form-item :label="t('entity.quartzlog.executestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.executeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executestatus') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.ExtField')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'quartzLogId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * Quartz 任务执行日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/quartz-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import QuartzLogDetail from './components/quartz-log-detail.vue'
import { getQuartzLogList, getQuartzLogById, deleteQuartzLogById, deleteQuartzLogBatch, exportQuartzLog } from '@/api/statistics/logging/quartz-log'
import type { QuartzLog, QuartzLogQuery } from '@/types/statistics/logging/quartz-log'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEyeLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQuartzLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.quartzlog._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QuartzLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QuartzLog | null>(null)
/** 表格多选行 */
const selectedRows = ref<QuartzLog[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  quartzTaskId: '',
  taskName: '',
  jobGroup: undefined as number | undefined,
  taskType: undefined as number | undefined,
  executeTimeStart: '',
  executeTimeEnd: '',
  executeDuration: '',
  executeParams: '',
  executeMessage: '',
  errorInfo: '',
  executeIp: '',
  executeHost: '',
  executeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'quartzTaskId', label: t('entity.quartzlog.quartztaskid') },
  { key: 'taskName', label: t('entity.quartzlog.taskname') },
  { key: 'jobGroup', label: t('entity.quartzlog.jobgroup') },
  { key: 'taskType', label: t('entity.quartzlog.tasktype') },
  { key: 'executeTimeStart', label: t('entity.quartzlog.executetimestart') },
  { key: 'executeTimeEnd', label: t('entity.quartzlog.executetimeend') },
  { key: 'executeDuration', label: t('entity.quartzlog.executeduration') },
  { key: 'executeParams', label: t('entity.quartzlog.executeparams') },
  { key: 'executeMessage', label: t('entity.quartzlog.executemessage') },
  { key: 'errorInfo', label: t('entity.quartzlog.errorinfo') },
  { key: 'executeIp', label: t('entity.quartzlog.executeip') },
  { key: 'executeHost', label: t('entity.quartzlog.executehost') },
  { key: 'executeStatus', label: t('entity.quartzlog.executestatus') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'quartzLogId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)
/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据 */
const detailData = ref<QuartzLog | null>(null)


/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'quartzLogId',
    key: 'quartzLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzLogId') ?? ''
  },
  {
    title: t('entity.quartzlog.quartztaskid'),
    dataIndex: 'quartzTaskId',
    key: 'quartzTaskId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzTaskId') ?? ''
  },
  {
    title: t('entity.quartzlog.taskname'),
    dataIndex: 'taskName',
    key: 'taskName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'taskName') ?? ''
  },
  {
    title: t('entity.quartzlog.jobgroup'),
    dataIndex: 'jobGroup',
    key: 'jobGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'jobGroup') ?? ''
  },
  {
    title: t('entity.quartzlog.tasktype'),
    dataIndex: 'taskType',
    key: 'taskType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'taskType') ?? ''
  },
  {
    title: t('entity.quartzlog.executetime'),
    dataIndex: 'executeTime',
    key: 'executeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeTime') ?? ''
  },
  {
    title: t('entity.quartzlog.executeduration'),
    dataIndex: 'executeDuration',
    key: 'executeDuration',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeDuration') ?? ''
  },
  {
    title: t('entity.quartzlog.executeparams'),
    dataIndex: 'executeParams',
    key: 'executeParams',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeParams') ?? ''
  },
  {
    title: t('entity.quartzlog.executemessage'),
    dataIndex: 'executeMessage',
    key: 'executeMessage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeMessage') ?? ''
  },
  {
    title: t('entity.quartzlog.errorinfo'),
    dataIndex: 'errorInfo',
    key: 'errorInfo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'errorInfo') ?? ''
  },
  {
    title: t('entity.quartzlog.executeip'),
    dataIndex: 'executeIp',
    key: 'executeIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeIp') ?? ''
  },
  {
    title: t('entity.quartzlog.executehost'),
    dataIndex: 'executeHost',
    key: 'executeHost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeHost') ?? ''
  },
  {
    title: t('entity.quartzlog.executestatus'),
    dataIndex: 'executeStatus',
    key: 'executeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'executeStatus') ?? ''
  },
  {
    title: t('entity.quartzlog.quartztask'),
    dataIndex: 'quartzTask',
    key: 'quartzTask',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzLogField(record, 'quartzTask') ?? ''
  },
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'statistics:logging:quartzlog:query',
        onClick: (record: QuartzLog) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:quartzlog:delete',
        onClick: (record: QuartzLog) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQuartzLogId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getQuartzLogField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QuartzLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QuartzLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQuartzLogId(selectedRow.value) === getQuartzLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QuartzLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QuartzLog) => ({
  onClick: () => {
    const key = getQuartzLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQuartzLogId(item)))
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
    const kw = (queryKeyword.value ?? '').trim()
    const params: QuartzLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQuartzLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QuartzLog] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 打开详情弹窗 */
async function handleShowDetail(record: QuartzLog) {
  const id = getQuartzLogId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getQuartzLogById(id)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = null
}

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  quartzTaskId: '',
  taskName: '',
  jobGroup: undefined as number | undefined,
  taskType: undefined as number | undefined,
  executeTimeStart: '',
  executeTimeEnd: '',
  executeDuration: '',
  executeParams: '',
  executeMessage: '',
  errorInfo: '',
  executeIp: '',
  executeHost: '',
  executeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: QuartzLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQuartzLog(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.quartzlog._self') }))
  } catch (error: any) {
    logger.error('[QuartzLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.quartzlog._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QuartzLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.quartzlog._self'), name: t('common.tip.this.target', { target: t('entity.quartzlog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQuartzLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.quartzlog._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.quartzlog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.quartzlog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQuartzLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.quartzlog._self') }))
      loadData()
    }
  })
}
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
  quartzTaskId: '',
  taskName: '',
  jobGroup: undefined as number | undefined,
  taskType: undefined as number | undefined,
  executeTimeStart: '',
  executeTimeEnd: '',
  executeDuration: '',
  executeParams: '',
  executeMessage: '',
  errorInfo: '',
  executeIp: '',
  executeHost: '',
  executeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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
.statistics-logging-quartz-log {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
