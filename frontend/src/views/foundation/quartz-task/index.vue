<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/foundation/quartz-task -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Quartz 定时任务实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="foundation:quartz:task:create"
      update-permission="foundation:quartz:task:update"
      delete-permission="foundation:quartz:task:delete"
      import-permission="foundation:quartz:task:import"
      export-permission="foundation:quartz:task:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
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
      @import="handleImport"
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
      :id-column-key="'quartzTaskId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQuartzTaskId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'jobGroup'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'jobGroup')"
            dict-type="sys_quartz_job_group"
          />
        </template>
        <template v-else-if="column.key === 'taskType'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'taskType')"
            dict-type="sys_quartz_task_type"
          />
        </template>
        <template v-else-if="column.key === 'triggerType'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'triggerType')"
            dict-type="sys_quartz_trigger_type"
          />
        </template>
        <template v-else-if="column.key === 'concurrent'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'concurrent')"
            dict-type="sys_yes_no_type"
          />
        </template>
        <template v-else-if="column.key === 'misfirePolicy'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'misfirePolicy')"
            dict-type="sys_quartz_misfire_policy"
          />
        </template>
        <template v-else-if="column.key === 'taskStatus'">
          <TaktDictTag
            :value="getQuartzTaskField(record, 'taskStatus')"
            dict-type="sys_quartz_task_status"
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
      <QuartzTaskForm
        :key="formData?.quartzTaskId ?? 'create'"
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
      <a-form-item :label="t('entity.quartztask.taskcode')">
        <a-input
          v-model:value="advancedQueryForm.taskCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskName')">
      <a-form-item :label="t('entity.quartztask.taskname')">
        <a-input
          v-model:value="advancedQueryForm.taskName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.taskname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobName')">
      <a-form-item :label="t('entity.quartztask.jobname')">
        <a-input
          v-model:value="advancedQueryForm.jobName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.jobname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGroup')">
      <a-form-item :label="t('entity.quartztask.jobgroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.jobGroup"
          dict-type="sys_quartz_job_group"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.jobgroup') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskType')">
      <a-form-item :label="t('entity.quartztask.tasktype')">
        <TaktSelect
          v-model:value="advancedQueryForm.taskType"
          dict-type="sys_quartz_task_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.tasktype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assemblyName')">
      <a-form-item :label="t('entity.quartztask.assemblyname')">
        <a-input
          v-model:value="advancedQueryForm.assemblyName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.assemblyname') })"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('className')">
      <a-form-item :label="t('entity.quartztask.classname')">
        <a-input
          v-model:value="advancedQueryForm.className"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.classname') })"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('apiUrl')">
      <a-form-item :label="t('entity.quartztask.apiurl')">
        <a-input
          v-model:value="advancedQueryForm.apiUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.apiurl') })"
          show-count
          :maxlength="255"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestMethod')">
      <a-form-item :label="t('entity.quartztask.requestmethod')">
        <a-input
          v-model:value="advancedQueryForm.requestMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.requestmethod') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sqlScript')">
      <a-form-item :label="t('entity.quartztask.sqlscript')">
        <a-input
          v-model:value="advancedQueryForm.sqlScript"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.sqlscript') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('triggerType')">
      <a-form-item :label="t('entity.quartztask.triggertype')">
        <TaktSelect
          v-model:value="advancedQueryForm.triggerType"
          dict-type="sys_quartz_trigger_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.triggertype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cronExpression')">
      <a-form-item :label="t('entity.quartztask.cronexpression')">
        <a-input
          v-model:value="advancedQueryForm.cronExpression"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.cronexpression') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('intervalSeconds')">
      <a-form-item :label="t('entity.quartztask.intervalseconds')">
        <a-input-number
          v-model:value="advancedQueryForm.intervalSeconds"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.intervalseconds') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeParams')">
      <a-form-item :label="t('entity.quartztask.executeparams')">
        <a-input
          v-model:value="advancedQueryForm.executeParams"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.executeparams') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('concurrent')">
      <a-form-item :label="t('entity.quartztask.concurrent')">
        <TaktSelect
          v-model:value="advancedQueryForm.concurrent"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.concurrent') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('misfirePolicy')">
      <a-form-item :label="t('entity.quartztask.misfirepolicy')">
        <TaktSelect
          v-model:value="advancedQueryForm.misfirePolicy"
          dict-type="sys_quartz_misfire_policy"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.misfirepolicy') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtStart')">
      <a-form-item :label="t('entity.quartztask.firstrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.firstrunatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstRunAtEnd')">
      <a-form-item :label="t('entity.quartztask.firstrunatend')">
        <a-input
          v-model:value="advancedQueryForm.firstRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.firstrunatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeCount')">
      <a-form-item :label="t('entity.quartztask.executecount')">
        <a-input-number
          v-model:value="advancedQueryForm.executeCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.executecount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtStart')">
      <a-form-item :label="t('entity.quartztask.lastrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.lastrunatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastRunAtEnd')">
      <a-form-item :label="t('entity.quartztask.lastrunatend')">
        <a-input
          v-model:value="advancedQueryForm.lastRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.lastrunatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtStart')">
      <a-form-item :label="t('entity.quartztask.nextrunatstart')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.nextrunatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextRunAtEnd')">
      <a-form-item :label="t('entity.quartztask.nextrunatend')">
        <a-input
          v-model:value="advancedQueryForm.nextRunAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.quartztask.nextrunatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskDescription')">
      <a-form-item :label="t('entity.quartztask.taskdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.taskDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.quartztask.taskdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taskStatus')">
      <a-form-item :label="t('entity.quartztask.taskstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.taskStatus"
          dict-type="sys_quartz_task_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.quartztask.taskstatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.quartztask._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.quartztask._self"
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
 * Quartz 定时任务实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/quartz-task
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import QuartzTaskForm from './components/quartz-task-form.vue'
import { getQuartzTaskList, getQuartzTaskById, createQuartzTask, updateQuartzTask, deleteQuartzTaskById, deleteQuartzTaskBatch, getQuartzTaskTemplate, importQuartzTask, exportQuartzTask, updateQuartzTaskStatus } from '@/api/foundation/quartz-task'
import type { QuartzTask, QuartzTaskQuery } from '@/types/foundation/quartz-task'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQuartzTask')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.quartztask._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QuartzTask[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<QuartzTask> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: '',
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  taskDescription: '',
  taskStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'taskCode', label: t('entity.quartztask.taskcode') },
  { key: 'taskName', label: t('entity.quartztask.taskname') },
  { key: 'jobName', label: t('entity.quartztask.jobname') },
  { key: 'jobGroup', label: t('entity.quartztask.jobgroup') },
  { key: 'taskType', label: t('entity.quartztask.tasktype') },
  { key: 'assemblyName', label: t('entity.quartztask.assemblyname') },
  { key: 'className', label: t('entity.quartztask.classname') },
  { key: 'apiUrl', label: t('entity.quartztask.apiurl') },
  { key: 'requestMethod', label: t('entity.quartztask.requestmethod') },
  { key: 'sqlScript', label: t('entity.quartztask.sqlscript') },
  { key: 'triggerType', label: t('entity.quartztask.triggertype') },
  { key: 'cronExpression', label: t('entity.quartztask.cronexpression') },
  { key: 'intervalSeconds', label: t('entity.quartztask.intervalseconds') },
  { key: 'executeParams', label: t('entity.quartztask.executeparams') },
  { key: 'concurrent', label: t('entity.quartztask.concurrent') },
  { key: 'misfirePolicy', label: t('entity.quartztask.misfirepolicy') },
  { key: 'firstRunAtStart', label: t('entity.quartztask.firstrunatstart') },
  { key: 'firstRunAtEnd', label: t('entity.quartztask.firstrunatend') },
  { key: 'executeCount', label: t('entity.quartztask.executecount') },
  { key: 'lastRunAtStart', label: t('entity.quartztask.lastrunatstart') },
  { key: 'lastRunAtEnd', label: t('entity.quartztask.lastrunatend') },
  { key: 'nextRunAtStart', label: t('entity.quartztask.nextrunatstart') },
  { key: 'nextRunAtEnd', label: t('entity.quartztask.nextrunatend') },
  { key: 'taskDescription', label: t('entity.quartztask.taskdescription') },
  { key: 'taskStatus', label: t('entity.quartztask.taskstatus') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
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

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {QuartzTaskQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QuartzTaskQuery>): QuartzTaskQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QuartzTaskQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QuartzTaskQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('taskCode', form.taskCode)
  assignTrimmed('taskName', form.taskName)
  assignTrimmed('jobName', form.jobName)
  assignTrimmed('jobGroup', form.jobGroup)
  assignTrimmed('taskType', form.taskType)
  assignTrimmed('assemblyName', form.assemblyName)
  assignTrimmed('className', form.className)
  assignTrimmed('apiUrl', form.apiUrl)
  assignTrimmed('requestMethod', form.requestMethod)
  assignTrimmed('sqlScript', form.sqlScript)
  if (form.triggerType !== undefined && form.triggerType !== null) {
    query.triggerType = form.triggerType
  }
  assignTrimmed('cronExpression', form.cronExpression)
  if (form.intervalSeconds !== undefined && form.intervalSeconds !== null) {
    query.intervalSeconds = form.intervalSeconds
  }
  assignTrimmed('executeParams', form.executeParams)
  if (form.concurrent !== undefined && form.concurrent !== null) {
    query.concurrent = form.concurrent
  }
  if (form.misfirePolicy !== undefined && form.misfirePolicy !== null) {
    query.misfirePolicy = form.misfirePolicy
  }
  assignTrimmed('firstRunAtStart', form.firstRunAtStart)
  assignTrimmed('firstRunAtEnd', form.firstRunAtEnd)
  if (form.executeCount !== undefined && form.executeCount !== null) {
    query.executeCount = form.executeCount
  }
  assignTrimmed('lastRunAtStart', form.lastRunAtStart)
  assignTrimmed('lastRunAtEnd', form.lastRunAtEnd)
  assignTrimmed('nextRunAtStart', form.nextRunAtStart)
  assignTrimmed('nextRunAtEnd', form.nextRunAtEnd)
  assignTrimmed('taskDescription', form.taskDescription)
  if (form.taskStatus !== undefined && form.taskStatus !== null) {
    query.taskStatus = form.taskStatus
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
    dataIndex: 'quartzTaskId',
    key: 'quartzTaskId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'quartzTaskId') ?? ''
  },
  {
    title: t('entity.quartztask.taskcode'),
    dataIndex: 'taskCode',
    key: 'taskCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskCode') ?? ''
  },
  {
    title: t('entity.quartztask.taskname'),
    dataIndex: 'taskName',
    key: 'taskName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskName') ?? ''
  },
  {
    title: t('entity.quartztask.jobname'),
    dataIndex: 'jobName',
    key: 'jobName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'jobName') ?? ''
  },
  {
    title: t('entity.quartztask.jobgroup'),
    dataIndex: 'jobGroup',
    key: 'jobGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.quartztask.tasktype'),
    dataIndex: 'taskType',
    key: 'taskType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.quartztask.assemblyname'),
    dataIndex: 'assemblyName',
    key: 'assemblyName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'assemblyName') ?? ''
  },
  {
    title: t('entity.quartztask.classname'),
    dataIndex: 'className',
    key: 'className',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'className') ?? ''
  },
  {
    title: t('entity.quartztask.apiurl'),
    dataIndex: 'apiUrl',
    key: 'apiUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'apiUrl') ?? ''
  },
  {
    title: t('entity.quartztask.requestmethod'),
    dataIndex: 'requestMethod',
    key: 'requestMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'requestMethod') ?? ''
  },
  {
    title: t('entity.quartztask.sqlscript'),
    dataIndex: 'sqlScript',
    key: 'sqlScript',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'sqlScript') ?? ''
  },
  {
    title: t('entity.quartztask.triggertype'),
    dataIndex: 'triggerType',
    key: 'triggerType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.quartztask.cronexpression'),
    dataIndex: 'cronExpression',
    key: 'cronExpression',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'cronExpression') ?? ''
  },
  {
    title: t('entity.quartztask.intervalseconds'),
    dataIndex: 'intervalSeconds',
    key: 'intervalSeconds',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'intervalSeconds') ?? ''
  },
  {
    title: t('entity.quartztask.executeparams'),
    dataIndex: 'executeParams',
    key: 'executeParams',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'executeParams') ?? ''
  },
  {
    title: t('entity.quartztask.concurrent'),
    dataIndex: 'concurrent',
    key: 'concurrent',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.quartztask.misfirepolicy'),
    dataIndex: 'misfirePolicy',
    key: 'misfirePolicy',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.quartztask.firstrunat'),
    dataIndex: 'firstRunAt',
    key: 'firstRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'firstRunAt') ?? ''
  },
  {
    title: t('entity.quartztask.executecount'),
    dataIndex: 'executeCount',
    key: 'executeCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'executeCount') ?? ''
  },
  {
    title: t('entity.quartztask.lastrunat'),
    dataIndex: 'lastRunAt',
    key: 'lastRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'lastRunAt') ?? ''
  },
  {
    title: t('entity.quartztask.nextrunat'),
    dataIndex: 'nextRunAt',
    key: 'nextRunAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'nextRunAt') ?? ''
  },
  {
    title: t('entity.quartztask.taskdescription'),
    dataIndex: 'taskDescription',
    key: 'taskDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQuartzTaskField(record, 'taskDescription') ?? ''
  },
  {
    title: t('entity.quartztask.taskstatus'),
    dataIndex: 'taskStatus',
    key: 'taskStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:quartz:task:update',
        onClick: (record: QuartzTask) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:quartz:task:delete',
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
    } else if (selectedRow.value && getQuartzTaskId(selectedRow.value) === getQuartzTaskId(record)) {
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
    const res = await getQuartzTaskList(buildListQuery())
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
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: '',
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  taskDescription: '',
  taskStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.quartztask._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: QuartzTask) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.quartztask._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.quartztask._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.quartztask._self') }))
    } else {
      await createQuartzTask(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.quartztask._self') }))
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
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getQuartzTaskTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importQuartzTask(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
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
    const exportMeta = await exportQuartzTask(
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
    message.success(t('common.feedback.export.success', { target: t('entity.quartztask._self') }))
  } catch (error: any) {
    logger.error('[QuartzTask] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.quartztask._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QuartzTask) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.quartztask._self'), name: t('common.tip.this.target', { target: t('entity.quartztask._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQuartzTaskById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.quartztask._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.quartztask._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.quartztask._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQuartzTaskBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.quartztask._self') }))
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
  taskCode: '',
  taskName: '',
  jobName: '',
  jobGroup: '',
  taskType: '',
  assemblyName: '',
  className: '',
  apiUrl: '',
  requestMethod: '',
  sqlScript: '',
  triggerType: undefined as number | undefined,
  cronExpression: '',
  intervalSeconds: undefined as number | undefined,
  executeParams: '',
  concurrent: undefined as number | undefined,
  misfirePolicy: undefined as number | undefined,
  firstRunAtStart: '',
  firstRunAtEnd: '',
  executeCount: undefined as number | undefined,
  lastRunAtStart: '',
  lastRunAtEnd: '',
  nextRunAtStart: '',
  nextRunAtEnd: '',
  taskDescription: '',
  taskStatus: undefined as number | undefined,
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
