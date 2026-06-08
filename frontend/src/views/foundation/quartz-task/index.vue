<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 定时任务实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-quartz-task">
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
      create-permission="foundation:quartztask:create"
      update-permission="foundation:quartztask:update"
      delete-permission="foundation:quartztask:delete"
      import-permission="foundation:quartztask:import"
      export-permission="foundation:quartztask:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="true"
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
      @import="handleImport"
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
      :id-column-key="'quartzTaskId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQuartzTaskId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.quartzLog._self') }}</div>
          <a-table
            v-if="hasQuartzLogRows(record)"
            :columns="quartzLogExpandColumns"
            :data-source="getQuartzLogRows(record)"
            :row-key="(row: QuartzLog, index?: number) => row?.quartzLogId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
      </template>
    </TaktSingleTable>

    <!-- 分页组件 -->
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
      <QuartzTaskForm
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
      :storage-key="'takt-query-fields-foundation-quartz-task'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('taskCode')">
      <a-form-item :label="t('entity.quartzTask.taskcode')">
        <a-input
          v-model:value="advancedQueryForm.taskCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskName')">
      <a-form-item :label="t('entity.quartzTask.taskname')">
        <a-input
          v-model:value="advancedQueryForm.taskName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobName')">
      <a-form-item :label="t('entity.quartzTask.jobname')">
        <a-input
          v-model:value="advancedQueryForm.jobName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGroup')">
      <a-form-item :label="t('entity.quartzTask.jobgroup')">
        <a-input
          v-model:value="advancedQueryForm.jobGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.jobgroup') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskType')">
      <a-form-item :label="t('entity.quartzTask.tasktype')">
        <a-input-number
          v-model:value="advancedQueryForm.taskType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.tasktype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assemblyName')">
      <a-form-item :label="t('entity.quartzTask.assemblyname')">
        <a-input
          v-model:value="advancedQueryForm.assemblyName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.assemblyname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('className')">
      <a-form-item :label="t('entity.quartzTask.classname')">
        <a-input
          v-model:value="advancedQueryForm.className"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.classname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('apiUrl')">
      <a-form-item :label="t('entity.quartzTask.apiurl')">
        <a-input
          v-model:value="advancedQueryForm.apiUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.apiurl') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestMethod')">
      <a-form-item :label="t('entity.quartzTask.requestmethod')">
        <a-input
          v-model:value="advancedQueryForm.requestMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.requestmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sqlScript')">
      <a-form-item :label="t('entity.quartzTask.sqlscript')">
        <a-input
          v-model:value="advancedQueryForm.sqlScript"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.sqlscript') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('triggerType')">
      <a-form-item :label="t('entity.quartzTask.triggertype')">
        <a-input-number
          v-model:value="advancedQueryForm.triggerType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.triggertype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cronExpression')">
      <a-form-item :label="t('entity.quartzTask.cronexpression')">
        <a-input
          v-model:value="advancedQueryForm.cronExpression"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.cronexpression') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('intervalSeconds')">
      <a-form-item :label="t('entity.quartzTask.intervalseconds')">
        <a-input-number
          v-model:value="advancedQueryForm.intervalSeconds"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.intervalseconds') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeParams')">
      <a-form-item :label="t('entity.quartzTask.executeparams')">
        <a-input
          v-model:value="advancedQueryForm.executeParams"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.executeparams') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskStatus')">
      <a-form-item :label="t('entity.quartzTask.taskstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.taskStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.taskstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('concurrent')">
      <a-form-item :label="t('entity.quartzTask.concurrent')">
        <a-input-number
          v-model:value="advancedQueryForm.concurrent"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.concurrent') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('misfirePolicy')">
      <a-form-item :label="t('entity.quartzTask.misfirepolicy')">
        <a-input-number
          v-model:value="advancedQueryForm.misfirePolicy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.misfirepolicy') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtStart')">
      <a-form-item :label="t('entity.quartzTask.firstrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.firstrunatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtEnd')">
      <a-form-item :label="t('entity.quartzTask.firstrunatend')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.firstrunatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeCount')">
      <a-form-item :label="t('entity.quartzTask.executecount')">
        <a-input-number
          v-model:value="advancedQueryForm.executeCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.executecount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtStart')">
      <a-form-item :label="t('entity.quartzTask.lastrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.lastrunatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtEnd')">
      <a-form-item :label="t('entity.quartzTask.lastrunatend')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.lastrunatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtStart')">
      <a-form-item :label="t('entity.quartzTask.nextrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.nextrunatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtEnd')">
      <a-form-item :label="t('entity.quartzTask.nextrunatend')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartzTask.nextrunatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.quartzTask.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.quartzTask.description') })"
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
      <div v-show="isFieldVisible('extFieldJson')">
      <a-form-item :label="t('common.page.entity.extfieldjson')">
        <a-input
          v-model:value="advancedQueryForm.extFieldJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
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

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.quartzTask._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.quartzTask._self"
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
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'quartzTaskId'"
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
 * Quartz 定时任务实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import QuartzTaskForm from './components/quartz-task-form.vue'
import { getQuartzTaskList, getQuartzTaskById, createQuartzTask, updateQuartzTask, deleteQuartzTaskById, deleteQuartzTaskBatch, getQuartzTaskTemplate, importQuartzTask, exportQuartzTask } from '@/api/foundation/quartz-task'
import type { QuartzLog } from '@/types/foundation/quartz-log'
import type { QuartzTask, QuartzTaskQuery, QuartzTaskCreate, QuartzTaskUpdate } from '@/types/foundation/quartz-task'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQuartzTask')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.quartzTask._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QuartzTask[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QuartzTask | null>(null)
/** 表格多选行 */
const selectedRows = ref<QuartzTask[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QuartzTask>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: undefined as number | undefined,
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  taskStatus: undefined as number | undefined,
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'taskCode', label: t('entity.quartzTask.taskcode') },
  { key: 'taskName', label: t('entity.quartzTask.taskname') },
  { key: 'jobName', label: t('entity.quartzTask.jobname') },
  { key: 'jobGroup', label: t('entity.quartzTask.jobgroup') },
  { key: 'taskType', label: t('entity.quartzTask.tasktype') },
  { key: 'assemblyName', label: t('entity.quartzTask.assemblyname') },
  { key: 'className', label: t('entity.quartzTask.classname') },
  { key: 'apiUrl', label: t('entity.quartzTask.apiurl') },
  { key: 'requestMethod', label: t('entity.quartzTask.requestmethod') },
  { key: 'sqlScript', label: t('entity.quartzTask.sqlscript') },
  { key: 'triggerType', label: t('entity.quartzTask.triggertype') },
  { key: 'cronExpression', label: t('entity.quartzTask.cronexpression') },
  { key: 'intervalSeconds', label: t('entity.quartzTask.intervalseconds') },
  { key: 'executeParams', label: t('entity.quartzTask.executeparams') },
  { key: 'taskStatus', label: t('entity.quartzTask.taskstatus') },
  { key: 'concurrent', label: t('entity.quartzTask.concurrent') },
  { key: 'misfirePolicy', label: t('entity.quartzTask.misfirepolicy') },
  { key: 'firstRunAtStart', label: t('entity.quartzTask.firstrunatstart') },
  { key: 'firstRunAtEnd', label: t('entity.quartzTask.firstrunatend') },
  { key: 'executeCount', label: t('entity.quartzTask.executecount') },
  { key: 'lastRunAtStart', label: t('entity.quartzTask.lastrunatstart') },
  { key: 'lastRunAtEnd', label: t('entity.quartzTask.lastrunatend') },
  { key: 'nextRunAtStart', label: t('entity.quartzTask.nextrunatstart') },
  { key: 'nextRunAtEnd', label: t('entity.quartzTask.nextrunatend') },
  { key: 'description', label: t('entity.quartzTask.description') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'quartzTaskId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：quartzLog 列 */
const quartzLogExpandColumns = computed(() => [

])

/** 读取主表行上的 quartzLog 子表缓存 */
function getQuartzLogRows(record: QuartzTask): QuartzLog[] {
  return (record as any)?.quartzLogs ?? []
}

/** 主表行是否已加载 quartzLog 子表 */
function hasQuartzLogRows(record: QuartzTask): boolean {
  return getQuartzLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadQuartzTaskDetail(record: QuartzTask): Promise<QuartzTask | null> {
  const id = getQuartzTaskId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getQuartzTaskById(id)
    const index = dataSource.value.findIndex((row) => getQuartzTaskId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as QuartzTask
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 通过主表详情接口加载 quartzLog 子表 */
async function loadQuartzLogForQuartzTask(record: QuartzTask): Promise<QuartzLog[]> {
  const detail = await loadQuartzTaskDetail(record)
  return detail?.quartzLogs ?? []
}

/** 展开前确保各子表已懒加载 */
async function ensureQuartzTaskChildrenLoaded(record: QuartzTask) {
  if (!hasQuartzLogRows(record)) {
    await loadQuartzLogForQuartzTask(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: QuartzTask) {
  const key = getQuartzTaskId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureQuartzTaskChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'quartzTaskId',
    key: 'quartzTaskId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'quartzTaskId') ?? ''
  },
  {
    title: t('entity.quartzTask.taskcode'),
    dataIndex: 'taskCode',
    key: 'taskCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskCode') ?? ''
  },
  {
    title: t('entity.quartzTask.taskname'),
    dataIndex: 'taskName',
    key: 'taskName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskName') ?? ''
  },
  {
    title: t('entity.quartzTask.jobname'),
    dataIndex: 'jobName',
    key: 'jobName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'jobName') ?? ''
  },
  {
    title: t('entity.quartzTask.jobgroup'),
    dataIndex: 'jobGroup',
    key: 'jobGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'jobGroup') ?? ''
  },
  {
    title: t('entity.quartzTask.tasktype'),
    dataIndex: 'taskType',
    key: 'taskType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskType') ?? ''
  },
  {
    title: t('entity.quartzTask.assemblyname'),
    dataIndex: 'assemblyName',
    key: 'assemblyName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'assemblyName') ?? ''
  },
  {
    title: t('entity.quartzTask.classname'),
    dataIndex: 'className',
    key: 'className',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'className') ?? ''
  },
  {
    title: t('entity.quartzTask.apiurl'),
    dataIndex: 'apiUrl',
    key: 'apiUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'apiUrl') ?? ''
  },
  {
    title: t('entity.quartzTask.requestmethod'),
    dataIndex: 'requestMethod',
    key: 'requestMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'requestMethod') ?? ''
  },
  {
    title: t('entity.quartzTask.sqlscript'),
    dataIndex: 'sqlScript',
    key: 'sqlScript',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'sqlScript') ?? ''
  },
  {
    title: t('entity.quartzTask.triggertype'),
    dataIndex: 'triggerType',
    key: 'triggerType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'triggerType') ?? ''
  },
  {
    title: t('entity.quartzTask.cronexpression'),
    dataIndex: 'cronExpression',
    key: 'cronExpression',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'cronExpression') ?? ''
  },
  {
    title: t('entity.quartzTask.intervalseconds'),
    dataIndex: 'intervalSeconds',
    key: 'intervalSeconds',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'intervalSeconds') ?? ''
  },
  {
    title: t('entity.quartzTask.executeparams'),
    dataIndex: 'executeParams',
    key: 'executeParams',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'executeParams') ?? ''
  },
  {
    title: t('entity.quartzTask.taskstatus'),
    dataIndex: 'taskStatus',
    key: 'taskStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskStatus') ?? ''
  },
  {
    title: t('entity.quartzTask.concurrent'),
    dataIndex: 'concurrent',
    key: 'concurrent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'concurrent') ?? ''
  },
  {
    title: t('entity.quartzTask.misfirepolicy'),
    dataIndex: 'misfirePolicy',
    key: 'misfirePolicy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'misfirePolicy') ?? ''
  },
  {
    title: t('entity.quartzTask.firstrunat'),
    dataIndex: 'firstRunAt',
    key: 'firstRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'firstRunAt') ?? ''
  },
  {
    title: t('entity.quartzTask.executecount'),
    dataIndex: 'executeCount',
    key: 'executeCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'executeCount') ?? ''
  },
  {
    title: t('entity.quartzTask.lastrunat'),
    dataIndex: 'lastRunAt',
    key: 'lastRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'lastRunAt') ?? ''
  },
  {
    title: t('entity.quartzTask.nextrunat'),
    dataIndex: 'nextRunAt',
    key: 'nextRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'nextRunAt') ?? ''
  },
  {
    title: t('entity.quartzTask.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'description') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:quartztask:update',
        onClick: (record: QuartzTask) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:quartztask:delete',
        onClick: (record: QuartzTask) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQuartzTaskId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getQuartzTaskField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QuartzTask[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QuartzTask, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQuartzTaskId(selectedRow.value) === getQuartzTaskId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QuartzTask[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QuartzTask) => ({
  onClick: () => {
    const key = getQuartzTaskId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQuartzTaskId(item)))
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
    const params: QuartzTaskQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQuartzTaskList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QuartzTask] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
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
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: undefined as number | undefined,
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  taskStatus: undefined as number | undefined,
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.quartzTask._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: QuartzTask) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.quartzTask._self') })
  formLoading.value = true
  try {
    const detail = await loadQuartzTaskDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.quartzTask._self') }))
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
      await updateQuartzTask(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.quartzTask._self') }))
    } else {
      await createQuartzTask(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.quartzTask._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getQuartzTaskTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQuartzTask(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: QuartzTaskQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQuartzTask(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.quartzTask._self') }))
  } catch (error: any) {
    logger.error('[QuartzTask] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.quartzTask._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QuartzTask) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.quartzTask._self'), name: t('common.tip.this.target', { target: t('entity.quartzTask._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQuartzTaskById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.quartzTask._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.quartzTask._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.quartzTask._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQuartzTaskBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.quartzTask._self') }))
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
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: undefined as number | undefined,
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  taskStatus: undefined as number | undefined,
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
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
.foundation-quartz-task {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
