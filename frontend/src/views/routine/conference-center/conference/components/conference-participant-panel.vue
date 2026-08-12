<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/conference-center/conference/components -->
<!-- 文件名称：conference-participant-panel.vue -->
<!-- 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理主表实体右侧明细 conferenceParticipant 独立 CRUD（按主表选中 conferenceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="conference-participant-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.conferenceparticipant._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="routine:conference:center:create"
      update-permission="routine:conference:center:update"
      delete-permission="routine:conference:center:delete"
      import-permission="routine:conference:center:import"
      export-permission="routine:conference:center:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div class="conference-participant-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getConferenceParticipantId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="conferenceParticipantId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <ConferenceParticipantForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterConferenceId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-routine-conference-center-conference-conference-participant"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('userId')">
      <a-form-item :label="t('entity.conferenceparticipant.userid')">
        <a-input
          v-model:value="advancedQueryForm.userId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceparticipant.userid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.conferenceparticipant.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceparticipant.username') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('participantRole')">
      <a-form-item :label="t('entity.conferenceparticipant.participantrole')">
        <a-input-number
          v-model:value="advancedQueryForm.participantRole"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceparticipant.participantrole') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attendanceStatus')">
      <a-form-item :label="t('entity.conferenceparticipant.attendancestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.attendanceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceparticipant.attendancestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkInTimeStart')">
      <a-form-item :label="t('entity.conferenceparticipant.checkintimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.checkInTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conferenceparticipant.checkintimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkInTimeEnd')">
      <a-form-item :label="t('entity.conferenceparticipant.checkintimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.checkInTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conferenceparticipant.checkintimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkOutTimeStart')">
      <a-form-item :label="t('entity.conferenceparticipant.checkouttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.checkOutTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conferenceparticipant.checkouttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkOutTimeEnd')">
      <a-form-item :label="t('entity.conferenceparticipant.checkouttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.checkOutTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.conferenceparticipant.checkouttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkInMethod')">
      <a-form-item :label="t('entity.conferenceparticipant.checkinmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.checkInMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.conferenceparticipant.checkinmethod') })"
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
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.conferenceparticipant._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.conferenceparticipant._self"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="conferenceParticipantId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理子表 conferenceParticipant 右栏面板
 * @module views/routine/conference-center/conference/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import ConferenceParticipantForm from './conference-participant-form.vue'
import { useConferenceMasterContext } from '../composables/use-conference-master-context'
import {
  getConferenceParticipantList,
  getConferenceParticipantById,
  createConferenceParticipant,
  updateConferenceParticipant,
  deleteConferenceParticipantById,
  deleteConferenceParticipantBatch,
  getConferenceParticipantTemplate,
  importConferenceParticipant,
  exportConferenceParticipant,
} from '@/api/routine/conference-center/conference-participant'
import type { ConferenceParticipant, ConferenceParticipantQuery } from '@/types/routine/conference-center/conference-participant'

const { t } = useI18n()
const { selectedMasterRow } = useConferenceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktConferenceParticipant')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.conferenceparticipant._self') }),
)

const loading = ref(false)
const dataSource = ref<ConferenceParticipant[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ConferenceParticipant | null>(null)
const selectedRows = ref<ConferenceParticipant[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ConferenceParticipant>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  userId: '',
  userName: '',
  participantRole: undefined as number | undefined,
  attendanceStatus: undefined as number | undefined,
  checkInTimeStart: '',
  checkInTimeEnd: '',
  checkOutTimeStart: '',
  checkOutTimeEnd: '',
  checkInMethod: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'userId', label: t('entity.conferenceparticipant.userid') },
  { key: 'userName', label: t('entity.conferenceparticipant.username') },
  { key: 'participantRole', label: t('entity.conferenceparticipant.participantrole') },
  { key: 'attendanceStatus', label: t('entity.conferenceparticipant.attendancestatus') },
  { key: 'checkInTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.conferenceparticipant.checkintime')) },
  { key: 'checkInTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.conferenceparticipant.checkintime')) },
  { key: 'checkOutTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.conferenceparticipant.checkouttime')) },
  { key: 'checkOutTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.conferenceparticipant.checkouttime')) },
  { key: 'checkInMethod', label: t('entity.conferenceparticipant.checkinmethod') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  userId: '',
  userName: '',
  participantRole: undefined as number | undefined,
  attendanceStatus: undefined as number | undefined,
  checkInTimeStart: '',
  checkInTimeEnd: '',
  checkOutTimeStart: '',
  checkOutTimeEnd: '',
  checkInMethod: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'conferenceParticipantId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.conferenceId)
const masterConferenceId = computed(() => selectedMasterRow.value?.conferenceId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getConferenceParticipantId(record: ConferenceParticipant | Record<string, unknown>): string {
  return String((record as ConferenceParticipant)?.[entityIdName] ?? '')
}

function getConferenceParticipantField(record: ConferenceParticipant | Record<string, unknown>, field: string): unknown {
  return (record as ConferenceParticipant)?.[field as keyof ConferenceParticipant]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'conferenceParticipantId',
    key: 'conferenceParticipantId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'conferenceParticipantId') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'userId') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'userName') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.participantrole'),
    dataIndex: 'participantRole',
    key: 'participantRole',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'participantRole') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.attendancestatus'),
    dataIndex: 'attendanceStatus',
    key: 'attendanceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'attendanceStatus') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.checkintime'),
    dataIndex: 'checkInTime',
    key: 'checkInTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'checkInTime') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.checkouttime'),
    dataIndex: 'checkOutTime',
    key: 'checkOutTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'checkOutTime') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.checkinmethod'),
    dataIndex: 'checkInMethod',
    key: 'checkInMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'checkInMethod') ?? ''),
  },
  {
    title: t('entity.conferenceparticipant.conference'),
    dataIndex: 'conference',
    key: 'conference',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ConferenceParticipant }) =>
      String(getConferenceParticipantField(record, 'conference') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:conference:center:update',
        onClick: (record: ConferenceParticipant) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:conference:center:delete',
        onClick: (record: ConferenceParticipant) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ConferenceParticipant[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ConferenceParticipant, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getConferenceParticipantId(selectedRow.value) === getConferenceParticipantId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ConferenceParticipant[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ConferenceParticipant) {
  const key = getConferenceParticipantId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ConferenceParticipantQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ConferenceParticipantQuery>): ConferenceParticipantQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ConferenceParticipantQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    conferenceId: masterConferenceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ConferenceParticipantQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('userId', form.userId)
  assignTrimmed('userName', form.userName)
  if (form.participantRole !== undefined && form.participantRole !== null) {
    query.participantRole = form.participantRole
  }
  if (form.attendanceStatus !== undefined && form.attendanceStatus !== null) {
    query.attendanceStatus = form.attendanceStatus
  }
  assignTrimmed('checkInTimeStart', form.checkInTimeStart)
  assignTrimmed('checkInTimeEnd', form.checkInTimeEnd)
  assignTrimmed('checkOutTimeStart', form.checkOutTimeStart)
  assignTrimmed('checkOutTimeEnd', form.checkOutTimeEnd)
  if (form.checkInMethod !== undefined && form.checkInMethod !== null) {
    query.checkInMethod = form.checkInMethod
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getConferenceParticipantList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterConferenceId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.conferenceparticipant._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ConferenceParticipant) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.conferenceparticipant._self') })
  formLoading.value = true
  try {
    const detail = await getConferenceParticipantById(getConferenceParticipantId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.conferenceparticipant._self'),
    }))
  }
}

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
    const payload = refInst.getValues?.()
    const id = formData.value?.conferenceParticipantId
    if (id) {
      await updateConferenceParticipant(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.conferenceparticipant._self') }))
    } else {
      await createConferenceParticipant(payload)
      message.success(t('common.feedback.created', { target: t('entity.conferenceparticipant._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: ConferenceParticipant) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.conferenceparticipant._self'),
      name: t('common.tip.this.target', { target: t('entity.conferenceparticipant._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteConferenceParticipantById(getConferenceParticipantId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.conferenceparticipant._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.conferenceparticipant._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.conferenceparticipant._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getConferenceParticipantId(r)).filter(Boolean)
      await deleteConferenceParticipantBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.conferenceparticipant._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getConferenceParticipantTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importConferenceParticipant(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportConferenceParticipant(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.conferenceparticipant._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.conferenceparticipant._self') }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
