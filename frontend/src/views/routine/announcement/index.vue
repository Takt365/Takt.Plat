<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/announcement -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公告通知实体 用于发布系统公告、通知、新闻等信息 支持富文本内容、附件、置顶、定时发布等功能 需要审批流程：草稿→审批→发布管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-announcement">
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
      create-permission="routine:announcement:create"
      update-permission="routine:announcement:update"
      delete-permission="routine:announcement:delete"
      import-permission="routine:announcement:import"
      export-permission="routine:announcement:export"
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
      :columns="columns"
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'announcementId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getAnnouncementId"
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
      <AnnouncementForm
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
      :storage-key="'takt-query-fields-routine-announcement'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.announcement.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('announcementType')">
      <a-form-item :label="t('entity.announcement.type')">
        <a-input-number
          v-model:value="advancedQueryForm.announcementType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.announcement.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.announcement.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('summary')">
      <a-form-item :label="t('entity.announcement.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.summary') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tags')">
      <a-form-item :label="t('entity.announcement.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.announcement.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.attachments') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeStart')">
      <a-form-item :label="t('entity.announcement.publishtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.publishtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeEnd')">
      <a-form-item :label="t('entity.announcement.publishtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.publishtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isScheduled')">
      <a-form-item :label="t('entity.announcement.isscheduled')">
        <a-input-number
          v-model:value="advancedQueryForm.isScheduled"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.isscheduled') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isTop')">
      <a-form-item :label="t('entity.announcement.istop')">
        <a-input-number
          v-model:value="advancedQueryForm.isTop"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.istop') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('topPriority')">
      <a-form-item :label="t('entity.announcement.toppriority')">
        <a-input-number
          v-model:value="advancedQueryForm.topPriority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.toppriority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeStart')">
      <a-form-item :label="t('entity.announcement.expiretimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.expiretimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeEnd')">
      <a-form-item :label="t('entity.announcement.expiretimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.expiretimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('viewCount')">
      <a-form-item :label="t('entity.announcement.viewcount')">
        <a-input-number
          v-model:value="advancedQueryForm.viewCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.viewcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetScope')">
      <a-form-item :label="t('entity.announcement.targetscope')">
        <a-textarea
          v-model:value="advancedQueryForm.targetScope"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.announcement.targetscope') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetDepartments')">
      <a-form-item :label="t('entity.announcement.targetdepartments')">
        <a-input
          v-model:value="advancedQueryForm.targetDepartments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.targetdepartments') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetUsers')">
      <a-form-item :label="t('entity.announcement.targetusers')">
        <a-input
          v-model:value="advancedQueryForm.targetUsers"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.targetusers') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('announcementStatus')">
      <a-form-item :label="t('entity.announcement.status')">
        <a-input-number
          v-model:value="advancedQueryForm.announcementStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.announcement.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.announcement.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.announcement.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.announcement.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.announcement.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.announcement.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.announcement.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.announcement.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.announcement.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.announcement._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.announcement._self"
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
      :id-column-key="'announcementId'"
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
 * 公告通知实体 用于发布系统公告、通知、新闻等信息 支持富文本内容、附件、置顶、定时发布等功能 需要审批流程：草稿→审批→发布管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/announcement
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import AnnouncementForm from './components/announcement-form.vue'
import { getAnnouncementList, getAnnouncementById, createAnnouncement, updateAnnouncement, deleteAnnouncementById, deleteAnnouncementBatch, getAnnouncementTemplate, importAnnouncement, exportAnnouncement } from '@/api/routine/announcement/announcement'
import type { Announcement, AnnouncementQuery, AnnouncementCreate, AnnouncementUpdate } from '@/types/routine/announcement/announcement'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAnnouncement')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.announcement._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Announcement[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Announcement | null>(null)
/** 表格多选行 */
const selectedRows = ref<Announcement[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Announcement>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  title: '',
  announcementType: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  attachments: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  isScheduled: undefined as number | undefined,
  isTop: undefined as number | undefined,
  topPriority: undefined as number | undefined,
  expireTimeStart: '',
  expireTimeEnd: '',
  viewCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
  announcementStatus: undefined as number | undefined,
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
  { key: 'title', label: t('entity.announcement.title') },
  { key: 'announcementType', label: t('entity.announcement.type') },
  { key: 'content', label: t('entity.announcement.content') },
  { key: 'summary', label: t('entity.announcement.summary') },
  { key: 'tags', label: t('entity.announcement.tags') },
  { key: 'attachments', label: t('entity.announcement.attachments') },
  { key: 'publishTimeStart', label: t('entity.announcement.publishtimestart') },
  { key: 'publishTimeEnd', label: t('entity.announcement.publishtimeend') },
  { key: 'isScheduled', label: t('entity.announcement.isscheduled') },
  { key: 'isTop', label: t('entity.announcement.istop') },
  { key: 'topPriority', label: t('entity.announcement.toppriority') },
  { key: 'expireTimeStart', label: t('entity.announcement.expiretimestart') },
  { key: 'expireTimeEnd', label: t('entity.announcement.expiretimeend') },
  { key: 'viewCount', label: t('entity.announcement.viewcount') },
  { key: 'targetScope', label: t('entity.announcement.targetscope') },
  { key: 'targetDepartments', label: t('entity.announcement.targetdepartments') },
  { key: 'targetUsers', label: t('entity.announcement.targetusers') },
  { key: 'announcementStatus', label: t('entity.announcement.status') },
  { key: 'approvalStatus', label: t('entity.announcement.approvalstatus') },
  { key: 'initiatorId', label: t('entity.announcement.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.announcement.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.announcement.initiatedatend') },
  { key: 'approvedBy', label: t('entity.announcement.approvedby') },
  { key: 'approvedAtStart', label: t('entity.announcement.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.announcement.approvedatend') },
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
const entityIdName = 'announcementId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)


/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'announcementId',
    key: 'announcementId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'announcementId') ?? ''
  },
  {
    title: t('entity.announcement.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'title') ?? ''
  },
  {
    title: t('entity.announcement.type'),
    dataIndex: 'announcementType',
    key: 'announcementType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'announcementType') ?? ''
  },
  {
    title: t('entity.announcement.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'content') ?? ''
  },
  {
    title: t('entity.announcement.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'summary') ?? ''
  },
  {
    title: t('entity.announcement.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'tags') ?? ''
  },
  {
    title: t('entity.announcement.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'attachments') ?? ''
  },
  {
    title: t('entity.announcement.publishtime'),
    dataIndex: 'publishTime',
    key: 'publishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'publishTime') ?? ''
  },
  {
    title: t('entity.announcement.isscheduled'),
    dataIndex: 'isScheduled',
    key: 'isScheduled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'isScheduled') ?? ''
  },
  {
    title: t('entity.announcement.istop'),
    dataIndex: 'isTop',
    key: 'isTop',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'isTop') ?? ''
  },
  {
    title: t('entity.announcement.toppriority'),
    dataIndex: 'topPriority',
    key: 'topPriority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'topPriority') ?? ''
  },
  {
    title: t('entity.announcement.expiretime'),
    dataIndex: 'expireTime',
    key: 'expireTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'expireTime') ?? ''
  },
  {
    title: t('entity.announcement.viewcount'),
    dataIndex: 'viewCount',
    key: 'viewCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'viewCount') ?? ''
  },
  {
    title: t('entity.announcement.targetscope'),
    dataIndex: 'targetScope',
    key: 'targetScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'targetScope') ?? ''
  },
  {
    title: t('entity.announcement.targetdepartments'),
    dataIndex: 'targetDepartments',
    key: 'targetDepartments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'targetDepartments') ?? ''
  },
  {
    title: t('entity.announcement.targetusers'),
    dataIndex: 'targetUsers',
    key: 'targetUsers',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'targetUsers') ?? ''
  },
  {
    title: t('entity.announcement.status'),
    dataIndex: 'announcementStatus',
    key: 'announcementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAnnouncementField(record, 'announcementStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:announcement:update',
        onClick: (record: Announcement) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:announcement:delete',
        onClick: (record: Announcement) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAnnouncementId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAnnouncementField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Announcement[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Announcement, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAnnouncementId(selectedRow.value) === getAnnouncementId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Announcement[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Announcement) => ({
  onClick: () => {
    const key = getAnnouncementId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAnnouncementId(item)))
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
    const params: AnnouncementQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAnnouncementList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Announcement] 加载数据失败', { error })
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
  title: '',
  announcementType: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  attachments: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  isScheduled: undefined as number | undefined,
  isTop: undefined as number | undefined,
  topPriority: undefined as number | undefined,
  expireTimeStart: '',
  expireTimeEnd: '',
  viewCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
  announcementStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.announcement._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Announcement) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.announcement._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.announcement._self') }))
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
      await updateAnnouncement(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.announcement._self') }))
    } else {
      await createAnnouncement(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.announcement._self') }))
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
  const res = await getAnnouncementTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAnnouncement(file, sheetName)
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
    const exportQuery: AnnouncementQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAnnouncement(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.announcement._self') }))
  } catch (error: any) {
    logger.error('[Announcement] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.announcement._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Announcement) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.announcement._self'), name: t('common.tip.this.target', { target: t('entity.announcement._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAnnouncementById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.announcement._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.announcement._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.announcement._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAnnouncementBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.announcement._self') }))
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
  title: '',
  announcementType: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  attachments: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  isScheduled: undefined as number | undefined,
  isTop: undefined as number | undefined,
  topPriority: undefined as number | undefined,
  expireTimeStart: '',
  expireTimeEnd: '',
  viewCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
  announcementStatus: undefined as number | undefined,
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
.routine-announcement {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
