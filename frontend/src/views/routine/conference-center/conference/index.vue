<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会议实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-conference-center-conference">
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
      create-permission="routine:conferencecenter:conference:create"
      update-permission="routine:conferencecenter:conference:update"
      delete-permission="routine:conferencecenter:conference:delete"
      import-permission="routine:conferencecenter:conference:import"
      export-permission="routine:conferencecenter:conference:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
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
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'conferenceId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getConferenceId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.conferenceParticipant._self') }}</div>
          <a-table
            v-if="hasConferenceParticipantRows(record)"
            :columns="conferenceParticipantExpandColumns"
            :data-source="getConferenceParticipantRows(record)"
            :row-key="(row: ConferenceParticipant, index?: number) => row?.conferenceParticipantId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.conferenceAgenda._self') }}</div>
          <a-table
            v-if="hasConferenceAgendaRows(record)"
            :columns="conferenceAgendaExpandColumns"
            :data-source="getConferenceAgendaRows(record)"
            :row-key="(row: ConferenceAgenda, index?: number) => row?.conferenceAgendaId || String(index ?? 0)"
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
      <ConferenceForm
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
      :storage-key="'takt-query-fields-routine-conference-center-conference'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('conferenceCode')">
      <a-form-item :label="t('entity.conference.code')">
        <a-input
          v-model:value="advancedQueryForm.conferenceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.conference.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conferenceType')">
      <a-form-item :label="t('entity.conference.type')">
        <a-input-number
          v-model:value="advancedQueryForm.conferenceType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conferenceStatus')">
      <a-form-item :label="t('entity.conference.status')">
        <a-input-number
          v-model:value="advancedQueryForm.conferenceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startTimeStart')">
      <a-form-item :label="t('entity.conference.starttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.starttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startTimeEnd')">
      <a-form-item :label="t('entity.conference.starttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.starttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endTimeStart')">
      <a-form-item :label="t('entity.conference.endtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.endtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endTimeEnd')">
      <a-form-item :label="t('entity.conference.endtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.endtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conferenceRoomId')">
      <a-form-item :label="t('entity.conference.roomid')">
        <a-input
          v-model:value="advancedQueryForm.conferenceRoomId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.roomid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conferenceRoomName')">
      <a-form-item :label="t('entity.conference.roomname')">
        <a-input
          v-model:value="advancedQueryForm.conferenceRoomName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.roomname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('location')">
      <a-form-item :label="t('entity.conference.location')">
        <a-input
          v-model:value="advancedQueryForm.location"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.location') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('meetingLink')">
      <a-form-item :label="t('entity.conference.meetinglink')">
        <a-input
          v-model:value="advancedQueryForm.meetingLink"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.meetinglink') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tags')">
      <a-form-item :label="t('entity.conference.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('organizerId')">
      <a-form-item :label="t('entity.conference.organizerid')">
        <a-input
          v-model:value="advancedQueryForm.organizerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.organizerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('organizerName')">
      <a-form-item :label="t('entity.conference.organizername')">
        <a-input
          v-model:value="advancedQueryForm.organizerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.organizername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.conference.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.conference.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.deptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxParticipants')">
      <a-form-item :label="t('entity.conference.maxparticipants')">
        <a-input-number
          v-model:value="advancedQueryForm.maxParticipants"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.maxparticipants') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reminderMinutes')">
      <a-form-item :label="t('entity.conference.reminderminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.reminderMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.reminderminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.conference.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.conference.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.conference.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.conference.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.conference.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.conference.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.conference.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conference.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.conference.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conference.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.conference._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.conference._self"
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
      :id-column-key="'conferenceId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/conference-center/conference
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ConferenceForm from './components/conference-form.vue'
import { getConferenceList, getConferenceById, createConference, updateConference, deleteConferenceById, deleteConferenceBatch, getConferenceTemplate, importConference, exportConference } from '@/api/routine/conference-center/conference'
import * as conferenceParticipantApi from '@/api/routine/conference-center/conference-participant'
import type { ConferenceParticipant, ConferenceParticipantQuery } from '@/types/routine/conference-center/conference-participant'
import type { ConferenceAgenda } from '@/types/routine/conference-center/conference-agenda'
import type { Conference, ConferenceQuery, ConferenceCreate, ConferenceUpdate } from '@/types/routine/conference-center/conference'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktConference')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.conference._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Conference[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Conference | null>(null)
/** 表格多选行 */
const selectedRows = ref<Conference[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Conference>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  startTimeStart: '',
  startTimeEnd: '',
  endTimeStart: '',
  endTimeEnd: '',
  conferenceRoomId: '',
  conferenceRoomName: '',
  location: '',
  meetingLink: '',
  tags: '',
  organizerId: '',
  organizerName: '',
  deptId: '',
  deptName: '',
  maxParticipants: undefined as number | undefined,
  reminderMinutes: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'conferenceCode', label: t('entity.conference.code') },
  { key: 'title', label: t('entity.conference.title') },
  { key: 'conferenceType', label: t('entity.conference.type') },
  { key: 'conferenceStatus', label: t('entity.conference.status') },
  { key: 'startTimeStart', label: t('entity.conference.starttimestart') },
  { key: 'startTimeEnd', label: t('entity.conference.starttimeend') },
  { key: 'endTimeStart', label: t('entity.conference.endtimestart') },
  { key: 'endTimeEnd', label: t('entity.conference.endtimeend') },
  { key: 'conferenceRoomId', label: t('entity.conference.roomid') },
  { key: 'conferenceRoomName', label: t('entity.conference.roomname') },
  { key: 'location', label: t('entity.conference.location') },
  { key: 'meetingLink', label: t('entity.conference.meetinglink') },
  { key: 'tags', label: t('entity.conference.tags') },
  { key: 'organizerId', label: t('entity.conference.organizerid') },
  { key: 'organizerName', label: t('entity.conference.organizername') },
  { key: 'deptId', label: t('entity.conference.deptid') },
  { key: 'deptName', label: t('entity.conference.deptname') },
  { key: 'maxParticipants', label: t('entity.conference.maxparticipants') },
  { key: 'reminderMinutes', label: t('entity.conference.reminderminutes') },
  { key: 'flowInstanceId', label: t('entity.conference.flowinstanceid') },
  { key: 'approvalStatus', label: t('entity.conference.approvalstatus') },
  { key: 'initiatorId', label: t('entity.conference.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.conference.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.conference.initiatedatend') },
  { key: 'approvedBy', label: t('entity.conference.approvedby') },
  { key: 'approvedAtStart', label: t('entity.conference.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.conference.approvedatend') },
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
const entityIdName = 'conferenceId'
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

/** 展开行预览：conferenceParticipant 列 */
const conferenceParticipantExpandColumns = computed(() => [
  {
    title: t('entity.conferenceParticipant.conferencename'),
    dataIndex: 'conferenceName',
    key: 'conferenceName',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.participantrole'),
    dataIndex: 'participantRole',
    key: 'participantRole',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.attendancestatus'),
    dataIndex: 'attendanceStatus',
    key: 'attendanceStatus',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.checkintime'),
    dataIndex: 'checkInTime',
    key: 'checkInTime',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.checkouttime'),
    dataIndex: 'checkOutTime',
    key: 'checkOutTime',
    ellipsis: true,
  },
  {
    title: t('entity.conferenceParticipant.conference'),
    dataIndex: 'conference',
    key: 'conference',
    ellipsis: true,
  },
])

/** 展开行预览：conferenceAgenda 列 */
const conferenceAgendaExpandColumns = computed(() => [

])

/** 读取主表行上的 conferenceParticipant 子表缓存 */
function getConferenceParticipantRows(record: Conference): ConferenceParticipant[] {
  return (record as any)?.participants ?? []
}

/** 主表行是否已加载 conferenceParticipant 子表 */
function hasConferenceParticipantRows(record: Conference): boolean {
  return getConferenceParticipantRows(record).length > 0
}

/** 读取主表行上的 conferenceAgenda 子表缓存 */
function getConferenceAgendaRows(record: Conference): ConferenceAgenda[] {
  return (record as any)?.agendaRecords ?? []
}

/** 主表行是否已加载 conferenceAgenda 子表 */
function hasConferenceAgendaRows(record: Conference): boolean {
  return getConferenceAgendaRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadConferenceDetail(record: Conference): Promise<Conference | null> {
  const id = getConferenceId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getConferenceById(id)
    const index = dataSource.value.findIndex((row) => getConferenceId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Conference
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 conferenceParticipant 子表（ConferenceParticipantQuery + conferenceParticipantApi，与主表 ConferenceQuery 分离） */
async function loadConferenceParticipantForConference(record: Conference): Promise<ConferenceParticipant[]> {
  const masterId = getConferenceId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConferenceParticipantQuery = {
      pageIndex: 1,
      pageSize: 500,
      conferenceId: masterId,
    }
    const result = await conferenceParticipantApi.getConferenceParticipantList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConferenceId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, participants: rows } as Conference
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 通过主表详情接口加载 conferenceAgenda 子表 */
async function loadConferenceAgendaForConference(record: Conference): Promise<ConferenceAgenda[]> {
  const detail = await loadConferenceDetail(record)
  return detail?.agendaRecords ?? []
}

/** 展开前确保各子表已懒加载 */
async function ensureConferenceChildrenLoaded(record: Conference) {
  if (!hasConferenceParticipantRows(record)) {
    await loadConferenceParticipantForConference(record)
  }
  if (!hasConferenceAgendaRows(record)) {
    await loadConferenceAgendaForConference(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Conference) {
  const key = getConferenceId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureConferenceChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'conferenceId',
    key: 'conferenceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceId') ?? ''
  },
  {
    title: t('entity.conference.code'),
    dataIndex: 'conferenceCode',
    key: 'conferenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceCode') ?? ''
  },
  {
    title: t('entity.conference.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'title') ?? ''
  },
  {
    title: t('entity.conference.type'),
    dataIndex: 'conferenceType',
    key: 'conferenceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceType') ?? ''
  },
  {
    title: t('entity.conference.status'),
    dataIndex: 'conferenceStatus',
    key: 'conferenceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceStatus') ?? ''
  },
  {
    title: t('entity.conference.starttime'),
    dataIndex: 'startTime',
    key: 'startTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'startTime') ?? ''
  },
  {
    title: t('entity.conference.endtime'),
    dataIndex: 'endTime',
    key: 'endTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'endTime') ?? ''
  },
  {
    title: t('entity.conference.roomid'),
    dataIndex: 'conferenceRoomId',
    key: 'conferenceRoomId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceRoomId') ?? ''
  },
  {
    title: t('entity.conference.roomname'),
    dataIndex: 'conferenceRoomName',
    key: 'conferenceRoomName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceRoomName') ?? ''
  },
  {
    title: t('entity.conference.location'),
    dataIndex: 'location',
    key: 'location',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'location') ?? ''
  },
  {
    title: t('entity.conference.meetinglink'),
    dataIndex: 'meetingLink',
    key: 'meetingLink',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'meetingLink') ?? ''
  },
  {
    title: t('entity.conference.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'tags') ?? ''
  },
  {
    title: t('entity.conference.organizerid'),
    dataIndex: 'organizerId',
    key: 'organizerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'organizerId') ?? ''
  },
  {
    title: t('entity.conference.organizername'),
    dataIndex: 'organizerName',
    key: 'organizerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'organizerName') ?? ''
  },
  {
    title: t('entity.conference.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.conference.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.conference.maxparticipants'),
    dataIndex: 'maxParticipants',
    key: 'maxParticipants',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'maxParticipants') ?? ''
  },
  {
    title: t('entity.conference.reminderminutes'),
    dataIndex: 'reminderMinutes',
    key: 'reminderMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'reminderMinutes') ?? ''
  },
  {
    title: t('entity.conference.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.conference.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.conference.room'),
    dataIndex: 'conferenceRoom',
    key: 'conferenceRoom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConferenceField(record, 'conferenceRoom') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:conferencecenter:conference:update',
        onClick: (record: Conference) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:conferencecenter:conference:delete',
        onClick: (record: Conference) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getConferenceId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getConferenceField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Conference[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Conference, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getConferenceId(selectedRow.value) === getConferenceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Conference[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Conference) => ({
  onClick: () => {
    const key = getConferenceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getConferenceId(item)))
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
    const params: ConferenceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getConferenceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Conference] 加载数据失败', { error })
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
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  startTimeStart: '',
  startTimeEnd: '',
  endTimeStart: '',
  endTimeEnd: '',
  conferenceRoomId: '',
  conferenceRoomName: '',
  location: '',
  meetingLink: '',
  tags: '',
  organizerId: '',
  organizerName: '',
  deptId: '',
  deptName: '',
  maxParticipants: undefined as number | undefined,
  reminderMinutes: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.conference._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Conference) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.conference._self') })
  formLoading.value = true
  try {
    const detail = await loadConferenceDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.conference._self') }))
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
      await updateConference(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.conference._self') }))
    } else {
      await createConference(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.conference._self') }))
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
  const res = await getConferenceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importConference(file, sheetName)
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
    const exportQuery: ConferenceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportConference(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.conference._self') }))
  } catch (error: any) {
    logger.error('[Conference] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.conference._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Conference) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.conference._self'), name: t('common.tip.this.target', { target: t('entity.conference._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteConferenceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.conference._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.conference._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.conference._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteConferenceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.conference._self') }))
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
  conferenceCode: '',
  title: '',
  conferenceType: undefined as number | undefined,
  conferenceStatus: undefined as number | undefined,
  startTimeStart: '',
  startTimeEnd: '',
  endTimeStart: '',
  endTimeEnd: '',
  conferenceRoomId: '',
  conferenceRoomName: '',
  location: '',
  meetingLink: '',
  tags: '',
  organizerId: '',
  organizerName: '',
  deptId: '',
  deptName: '',
  maxParticipants: undefined as number | undefined,
  reminderMinutes: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
.routine-conference-center-conference {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
