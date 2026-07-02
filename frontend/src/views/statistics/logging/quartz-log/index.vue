<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/logging/quartz-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 任务执行日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="statistics:logging:quartz:log:create"
      update-permission="statistics:logging:quartz:log:update"
      delete-permission="statistics:logging:quartz:log:delete"

      export-permission="statistics:logging:quartz:log:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
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
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
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
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'jobGroup'">
          <TaktDictTag
            :value="getQuartzLogField(record, 'jobGroup')"
            dict-type="sys_quartz_job_group"
          />
        </template>
        <template v-else-if="column.key === 'taskType'">
          <TaktDictTag
            :value="getQuartzLogField(record, 'taskType')"
            dict-type="sys_quartz_task_type"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

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
      <QuartzLogForm
        :key="formData?.quartzLogId ?? 'create'"
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
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskName')">
      <a-form-item :label="t('entity.quartzlog.taskname')">
        <a-input
          v-model:value="advancedQueryForm.taskName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.taskname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGroup')">
      <a-form-item :label="t('entity.quartzlog.jobgroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.jobGroup"
          dict-type="sys_quartz_job_group"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.jobgroup') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskType')">
      <a-form-item :label="t('entity.quartzlog.tasktype')">
        <TaktSelect
          v-model:value="advancedQueryForm.taskType"
          dict-type="sys_quartz_task_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.tasktype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeTimeStart')">
      <a-form-item :label="t('entity.quartzlog.executetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.executeTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeTimeEnd')">
      <a-form-item :label="t('entity.quartzlog.executetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.executeTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartzlog.executetimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeDuration')">
      <a-form-item :label="t('entity.quartzlog.executeduration')">
        <a-input
          v-model:value="advancedQueryForm.executeDuration"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeduration') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeParams')">
      <a-form-item :label="t('entity.quartzlog.executeparams')">
        <a-input
          v-model:value="advancedQueryForm.executeParams"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeparams') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeMessage')">
      <a-form-item :label="t('entity.quartzlog.executemessage')">
        <a-input
          v-model:value="advancedQueryForm.executeMessage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executemessage') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('errorInfo')">
      <a-form-item :label="t('entity.quartzlog.errorinfo')">
        <a-input
          v-model:value="advancedQueryForm.errorInfo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.errorinfo') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeIp')">
      <a-form-item :label="t('entity.quartzlog.executeip')">
        <a-input
          v-model:value="advancedQueryForm.executeIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executeip') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeHost')">
      <a-form-item :label="t('entity.quartzlog.executehost')">
        <a-input
          v-model:value="advancedQueryForm.executeHost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzlog.executehost') })"
          show-count
          :maxlength="100"
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
/**
 * Quartz 任务执行日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/quartz-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import QuartzLogForm from './components/quartz-log-form.vue'
import { getQuartzLogList, getQuartzLogById, createQuartzLog, updateQuartzLog, deleteQuartzLogById, deleteQuartzLogBatch, exportQuartzLog, updateQuartzLogStatus } from '@/api/statistics/logging/quartz-log'
import type { QuartzLog, QuartzLogQuery } from '@/types/statistics/logging/quartz-log'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

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

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QuartzLog> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  quartzTaskId: '',
  taskName: '',
  jobGroup: '',
  taskType: '',
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
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'quartzTaskId', label: t('entity.quartzlog.quartztaskid') },
  { key: 'taskName', label: t('entity.quartzlog.taskname') },
  { key: 'jobGroup', label: t('entity.quartzlog.jobgroup') },
  { key: 'taskType', label: t('entity.quartzlog.tasktype') },
  { key: 'executeTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.quartzlog.executetime')) },
  { key: 'executeTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.quartzlog.executetime')) },
  { key: 'executeDuration', label: t('entity.quartzlog.executeduration') },
  { key: 'executeParams', label: t('entity.quartzlog.executeparams') },
  { key: 'executeMessage', label: t('entity.quartzlog.executemessage') },
  { key: 'errorInfo', label: t('entity.quartzlog.errorinfo') },
  { key: 'executeIp', label: t('entity.quartzlog.executeip') },
  { key: 'executeHost', label: t('entity.quartzlog.executehost') },
  { key: 'executeStatus', label: t('entity.quartzlog.executestatus') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
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
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {QuartzLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QuartzLogQuery>): QuartzLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QuartzLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QuartzLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('quartzTaskId', form.quartzTaskId)
  assignTrimmed('taskName', form.taskName)
  assignTrimmed('jobGroup', form.jobGroup)
  assignTrimmed('taskType', form.taskType)
  assignTrimmed('executeTimeStart', form.executeTimeStart)
  assignTrimmed('executeTimeEnd', form.executeTimeEnd)
  assignTrimmed('executeDuration', form.executeDuration)
  assignTrimmed('executeParams', form.executeParams)
  assignTrimmed('executeMessage', form.executeMessage)
  assignTrimmed('errorInfo', form.errorInfo)
  assignTrimmed('executeIp', form.executeIp)
  assignTrimmed('executeHost', form.executeHost)
  if (form.executeStatus !== undefined && form.executeStatus !== null) {
    query.executeStatus = form.executeStatus
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
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
  },
  {
    title: t('entity.quartzlog.tasktype'),
    dataIndex: 'taskType',
    key: 'taskType',
    width: 120,
    resizable: true,
    ellipsis: true,
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
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:logging:quartz:log:update',
        onClick: (record: QuartzLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:quartz:log:delete',
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
    } else if (selectedRow.value && getQuartzLogId(selectedRow.value) === getQuartzLogId(record)) {
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
    const res = await getQuartzLogList(buildListQuery())
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

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  quartzTaskId: '',
  taskName: '',
  jobGroup: '',
  taskType: '',
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
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.quartzlog._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: QuartzLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.quartzlog._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.quartzlog._self') }))
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
      await updateQuartzLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.quartzlog._self') }))
    } else {
      await createQuartzLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.quartzlog._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    loadData()
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
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportQuartzLog(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  quartzTaskId: '',
  taskName: '',
  jobGroup: '',
  taskType: '',
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

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
