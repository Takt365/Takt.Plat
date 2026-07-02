<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
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
      create-permission="routine:help:desk:ticket:create"
      update-permission="routine:help:desk:ticket:update"
      delete-permission="routine:help:desk:ticket:delete"
      import-permission="routine:help:desk:ticket:import"
      export-permission="routine:help:desk:ticket:export"
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

    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getTicketId"
      :master-row-selection="rowSelection"
      master-id-column-key="ticketId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ticketStatus'">
          <TaktDictTag
            :value="getTicketField(record, 'ticketStatus')"
            dict-type="sys_ticket_status"
          />
        </template>
        <template v-else-if="column.key === 'priority'">
          <TaktDictTag
            :value="getTicketField(record, 'priority')"
            dict-type="sys_priority_level_category"
          />
        </template>
        <template v-else-if="column.key === 'urgency'">
          <TaktDictTag
            :value="getTicketField(record, 'urgency')"
            dict-type="sys_urgency_level_category"
          />
        </template>
        <template v-else-if="column.key === 'impact'">
          <TaktDictTag
            :value="getTicketField(record, 'impact')"
            dict-type="sys_impact_level_category"
          />
        </template>
      </template>
      <template #detail>
        <TicketChangeLogPanel
          ref="ticketChangeLogPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <TicketForm
        :key="formData?.ticketId ?? 'create'"
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
      :storage-key="'takt-query-fields-routine-help-desk-ticket-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ticketNo')">
      <a-form-item :label="t('entity.ticket.no')">
        <a-input
          v-model:value="advancedQueryForm.ticketNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.no') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.ticket.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.title') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.ticket.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ticket.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.ticket.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.attachments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketStatus')">
      <a-form-item :label="t('entity.ticket.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.ticketStatus"
          dict-type="sys_ticket_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.ticket.priority')">
        <TaktSelect
          v-model:value="advancedQueryForm.priority"
          dict-type="sys_priority_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.priority') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('urgency')">
      <a-form-item :label="t('entity.ticket.urgency')">
        <TaktSelect
          v-model:value="advancedQueryForm.urgency"
          dict-type="sys_urgency_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.urgency') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('impact')">
      <a-form-item :label="t('entity.ticket.impact')">
        <TaktSelect
          v-model:value="advancedQueryForm.impact"
          dict-type="sys_impact_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.impact') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('categoryCode')">
      <a-form-item :label="t('entity.ticket.categorycode')">
        <a-input
          v-model:value="advancedQueryForm.categoryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.categorycode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketSource')">
      <a-form-item :label="t('entity.ticket.source')">
        <a-input-number
          v-model:value="advancedQueryForm.ticketSource"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.source') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('submitterId')">
      <a-form-item :label="t('entity.ticket.submitterid')">
        <a-input
          v-model:value="advancedQueryForm.submitterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submitterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('submitterName')">
      <a-form-item :label="t('entity.ticket.submittername')">
        <a-input
          v-model:value="advancedQueryForm.submitterName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submittername') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assigneeId')">
      <a-form-item :label="t('entity.ticket.assigneeid')">
        <a-input
          v-model:value="advancedQueryForm.assigneeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assigneeName')">
      <a-form-item :label="t('entity.ticket.assigneename')">
        <a-input
          v-model:value="advancedQueryForm.assigneeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneename') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('knowledgeId')">
      <a-form-item :label="t('entity.ticket.knowledgeid')">
        <a-input
          v-model:value="advancedQueryForm.knowledgeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.knowledgeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentTicketId')">
      <a-form-item :label="t('entity.ticket.parentticketid')">
        <a-input
          v-model:value="advancedQueryForm.parentTicketId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.parentticketid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseAtStart')">
      <a-form-item :label="t('entity.ticket.firstresponseatstart')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseAtEnd')">
      <a-form-item :label="t('entity.ticket.firstresponseatend')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseDueByStart')">
      <a-form-item :label="t('entity.ticket.firstresponseduebystart')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseDueByStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseduebystart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseDueByEnd')">
      <a-form-item :label="t('entity.ticket.firstresponseduebyend')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseDueByEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseduebyend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolvedAtStart')">
      <a-form-item :label="t('entity.ticket.resolvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.resolvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolvedAtEnd')">
      <a-form-item :label="t('entity.ticket.resolvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.resolvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.resolvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolutionDueByStart')">
      <a-form-item :label="t('entity.ticket.resolutionduebystart')">
        <a-input
          v-model:value="advancedQueryForm.resolutionDueByStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolutionduebystart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolutionDueByEnd')">
      <a-form-item :label="t('entity.ticket.resolutionduebyend')">
        <a-input
          v-model:value="advancedQueryForm.resolutionDueByEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolutionduebyend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedAtStart')">
      <a-form-item :label="t('entity.ticket.closedatstart')">
        <a-input
          v-model:value="advancedQueryForm.closedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.closedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedAtEnd')">
      <a-form-item :label="t('entity.ticket.closedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.closedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.closedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itAssetId')">
      <a-form-item :label="t('entity.ticket.itassetid')">
        <a-input
          v-model:value="advancedQueryForm.itAssetId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.itassetid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="t('entity.ticket.assetcode')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assetcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantDeptId')">
      <a-form-item :label="t('entity.ticket.applicantdeptid')">
        <a-input
          v-model:value="advancedQueryForm.applicantDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantDeptName')">
      <a-form-item :label="t('entity.ticket.applicantdeptname')">
        <a-input
          v-model:value="advancedQueryForm.applicantDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantBy')">
      <a-form-item :label="t('entity.ticket.applicantby')">
        <a-input
          v-model:value="advancedQueryForm.applicantBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantby') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ticket._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ticket._self"
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
      :id-column-key="'ticketId'"
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
 * Takt工单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/ticket-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import TicketForm from './components/ticket-form.vue'
import TicketChangeLogPanel from './components/ticket-change-log-panel.vue'
import { provideTicketMasterContext } from './composables/use-ticket-master-context'
import { getTicketList, getTicketById, createTicket, updateTicket, deleteTicketById, deleteTicketBatch, getTicketTemplate, importTicket, exportTicket, updateTicketStatus } from '@/api/routine/help-desk/ticket'
import type { Ticket, TicketQuery } from '@/types/routine/help-desk/ticket'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTicket')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ticket._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Ticket[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Ticket | null>(null)
/** 表格多选行 */
const selectedRows = ref<Ticket[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Ticket> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  ticketNo: '',
  title: '',
  content: '',
  attachments: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  urgency: undefined as number | undefined,
  impact: undefined as number | undefined,
  categoryCode: '',
  ticketSource: undefined as number | undefined,
  submitterId: '',
  submitterName: '',
  assigneeId: '',
  assigneeName: '',
  knowledgeId: '',
  parentTicketId: '',
  firstResponseAtStart: '',
  firstResponseAtEnd: '',
  firstResponseDueByStart: '',
  firstResponseDueByEnd: '',
  resolvedAtStart: '',
  resolvedAtEnd: '',
  resolutionDueByStart: '',
  resolutionDueByEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
  itAssetId: '',
  assetCode: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'ticketNo', label: t('entity.ticket.no') },
  { key: 'title', label: t('entity.ticket.title') },
  { key: 'content', label: t('entity.ticket.content') },
  { key: 'attachments', label: t('entity.ticket.attachments') },
  { key: 'ticketStatus', label: t('entity.ticket.status') },
  { key: 'priority', label: t('entity.ticket.priority') },
  { key: 'urgency', label: t('entity.ticket.urgency') },
  { key: 'impact', label: t('entity.ticket.impact') },
  { key: 'categoryCode', label: t('entity.ticket.categorycode') },
  { key: 'ticketSource', label: t('entity.ticket.source') },
  { key: 'submitterId', label: t('entity.ticket.submitterid') },
  { key: 'submitterName', label: t('entity.ticket.submittername') },
  { key: 'assigneeId', label: t('entity.ticket.assigneeid') },
  { key: 'assigneeName', label: t('entity.ticket.assigneename') },
  { key: 'knowledgeId', label: t('entity.ticket.knowledgeid') },
  { key: 'parentTicketId', label: t('entity.ticket.parentticketid') },
  { key: 'firstResponseAtStart', label: t('entity.ticket.firstresponseatstart') },
  { key: 'firstResponseAtEnd', label: t('entity.ticket.firstresponseatend') },
  { key: 'firstResponseDueByStart', label: t('entity.ticket.firstresponseduebystart') },
  { key: 'firstResponseDueByEnd', label: t('entity.ticket.firstresponseduebyend') },
  { key: 'resolvedAtStart', label: t('entity.ticket.resolvedatstart') },
  { key: 'resolvedAtEnd', label: t('entity.ticket.resolvedatend') },
  { key: 'resolutionDueByStart', label: t('entity.ticket.resolutionduebystart') },
  { key: 'resolutionDueByEnd', label: t('entity.ticket.resolutionduebyend') },
  { key: 'closedAtStart', label: t('entity.ticket.closedatstart') },
  { key: 'closedAtEnd', label: t('entity.ticket.closedatend') },
  { key: 'itAssetId', label: t('entity.ticket.itassetid') },
  { key: 'assetCode', label: t('entity.ticket.assetcode') },
  { key: 'applicantDeptId', label: t('entity.ticket.applicantdeptid') },
  { key: 'applicantDeptName', label: t('entity.ticket.applicantdeptname') },
  { key: 'applicantBy', label: t('entity.ticket.applicantby') },
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
const entityIdName = 'ticketId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideTicketMasterContext()
const ticketChangeLogPanelRef = ref<InstanceType<typeof TicketChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TicketQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<TicketQuery>): TicketQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TicketQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof TicketQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ticketNo', form.ticketNo)
  assignTrimmed('title', form.title)
  assignTrimmed('content', form.content)
  assignTrimmed('attachments', form.attachments)
  if (form.ticketStatus !== undefined && form.ticketStatus !== null) {
    query.ticketStatus = form.ticketStatus
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  if (form.urgency !== undefined && form.urgency !== null) {
    query.urgency = form.urgency
  }
  if (form.impact !== undefined && form.impact !== null) {
    query.impact = form.impact
  }
  assignTrimmed('categoryCode', form.categoryCode)
  if (form.ticketSource !== undefined && form.ticketSource !== null) {
    query.ticketSource = form.ticketSource
  }
  assignTrimmed('submitterId', form.submitterId)
  assignTrimmed('submitterName', form.submitterName)
  assignTrimmed('assigneeId', form.assigneeId)
  assignTrimmed('assigneeName', form.assigneeName)
  assignTrimmed('knowledgeId', form.knowledgeId)
  assignTrimmed('parentTicketId', form.parentTicketId)
  assignTrimmed('firstResponseAtStart', form.firstResponseAtStart)
  assignTrimmed('firstResponseAtEnd', form.firstResponseAtEnd)
  assignTrimmed('firstResponseDueByStart', form.firstResponseDueByStart)
  assignTrimmed('firstResponseDueByEnd', form.firstResponseDueByEnd)
  assignTrimmed('resolvedAtStart', form.resolvedAtStart)
  assignTrimmed('resolvedAtEnd', form.resolvedAtEnd)
  assignTrimmed('resolutionDueByStart', form.resolutionDueByStart)
  assignTrimmed('resolutionDueByEnd', form.resolutionDueByEnd)
  assignTrimmed('closedAtStart', form.closedAtStart)
  assignTrimmed('closedAtEnd', form.closedAtEnd)
  assignTrimmed('itAssetId', form.itAssetId)
  assignTrimmed('assetCode', form.assetCode)
  assignTrimmed('applicantDeptId', form.applicantDeptId)
  assignTrimmed('applicantDeptName', form.applicantDeptName)
  assignTrimmed('applicantBy', form.applicantBy)
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


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: Ticket | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getTicketId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as Ticket
  const key = getTicketId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadTicketDetail(record: Ticket): Promise<Ticket | null> {
  const id = getTicketId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getTicketById(id)
    const index = dataSource.value.findIndex((row) => getTicketId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Ticket
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ticketId',
    key: 'ticketId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTicketField(record, 'ticketId') ?? ''
  },
  {
    title: t('entity.ticket.no'),
    dataIndex: 'ticketNo',
    key: 'ticketNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'ticketNo') ?? ''
  },
  {
    title: t('entity.ticket.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'title') ?? ''
  },
  {
    title: t('entity.ticket.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'content') ?? ''
  },
  {
    title: t('entity.ticket.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'attachments') ?? ''
  },
  {
    title: t('entity.ticket.status'),
    dataIndex: 'ticketStatus',
    key: 'ticketStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.ticket.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.ticket.urgency'),
    dataIndex: 'urgency',
    key: 'urgency',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.ticket.impact'),
    dataIndex: 'impact',
    key: 'impact',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.ticket.categorycode'),
    dataIndex: 'categoryCode',
    key: 'categoryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'categoryCode') ?? ''
  },
  {
    title: t('entity.ticket.source'),
    dataIndex: 'ticketSource',
    key: 'ticketSource',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'ticketSource') ?? ''
  },
  {
    title: t('entity.ticket.submitterid'),
    dataIndex: 'submitterId',
    key: 'submitterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'submitterId') ?? ''
  },
  {
    title: t('entity.ticket.submittername'),
    dataIndex: 'submitterName',
    key: 'submitterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'submitterName') ?? ''
  },
  {
    title: t('entity.ticket.assigneeid'),
    dataIndex: 'assigneeId',
    key: 'assigneeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'assigneeId') ?? ''
  },
  {
    title: t('entity.ticket.assigneename'),
    dataIndex: 'assigneeName',
    key: 'assigneeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'assigneeName') ?? ''
  },
  {
    title: t('entity.ticket.knowledgeid'),
    dataIndex: 'knowledgeId',
    key: 'knowledgeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'knowledgeId') ?? ''
  },
  {
    title: t('entity.ticket.parentticketid'),
    dataIndex: 'parentTicketId',
    key: 'parentTicketId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'parentTicketId') ?? ''
  },
  {
    title: t('entity.ticket.firstresponseat'),
    dataIndex: 'firstResponseAt',
    key: 'firstResponseAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'firstResponseAt') ?? ''
  },
  {
    title: t('entity.ticket.firstresponsedueby'),
    dataIndex: 'firstResponseDueBy',
    key: 'firstResponseDueBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'firstResponseDueBy') ?? ''
  },
  {
    title: t('entity.ticket.resolvedat'),
    dataIndex: 'resolvedAt',
    key: 'resolvedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'resolvedAt') ?? ''
  },
  {
    title: t('entity.ticket.resolutiondueby'),
    dataIndex: 'resolutionDueBy',
    key: 'resolutionDueBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'resolutionDueBy') ?? ''
  },
  {
    title: t('entity.ticket.closedat'),
    dataIndex: 'closedAt',
    key: 'closedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'closedAt') ?? ''
  },
  {
    title: t('entity.ticket.itassetid'),
    dataIndex: 'itAssetId',
    key: 'itAssetId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'itAssetId') ?? ''
  },
  {
    title: t('entity.ticket.assetcode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'assetCode') ?? ''
  },
  {
    title: t('entity.ticket.applicantdeptid'),
    dataIndex: 'applicantDeptId',
    key: 'applicantDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'applicantDeptId') ?? ''
  },
  {
    title: t('entity.ticket.applicantdeptname'),
    dataIndex: 'applicantDeptName',
    key: 'applicantDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'applicantDeptName') ?? ''
  },
  {
    title: t('entity.ticket.applicantby'),
    dataIndex: 'applicantBy',
    key: 'applicantBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'applicantBy') ?? ''
  },
  {
    title: t('entity.ticket.childtickets'),
    dataIndex: 'childTickets',
    key: 'childTickets',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'childTickets') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:help:desk:ticket:update',
        onClick: (record: Ticket) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:help:desk:ticket:delete',
        onClick: (record: Ticket) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTicketId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTicketField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Ticket[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: Ticket, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getTicketId(selectedRow.value) === getTicketId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Ticket[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getTicketList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Ticket] 加载数据失败', { error })
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
  ticketNo: '',
  title: '',
  content: '',
  attachments: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  urgency: undefined as number | undefined,
  impact: undefined as number | undefined,
  categoryCode: '',
  ticketSource: undefined as number | undefined,
  submitterId: '',
  submitterName: '',
  assigneeId: '',
  assigneeName: '',
  knowledgeId: '',
  parentTicketId: '',
  firstResponseAtStart: '',
  firstResponseAtEnd: '',
  firstResponseDueByStart: '',
  firstResponseDueByEnd: '',
  resolvedAtStart: '',
  resolvedAtEnd: '',
  resolutionDueByStart: '',
  resolutionDueByEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
  itAssetId: '',
  assetCode: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ticket._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Ticket) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ticket._self') })
  formLoading.value = true
  try {
    const detail = await loadTicketDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ticket._self') }))
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
      await updateTicket(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ticket._self') }))
    } else {
      await createTicket(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ticket._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  ticketChangeLogPanelRef.value?.reload?.()
    }
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
  const res = await getTicketTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTicket(file, sheetName)
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
    const exportMeta = await exportTicket(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ticket._self') }))
  } catch (error: any) {
    logger.error('[Ticket] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ticket._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Ticket) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ticket._self'), name: t('common.tip.this.target', { target: t('entity.ticket._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTicketById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ticket._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ticket._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ticket._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTicketBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ticket._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
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
  ticketNo: '',
  title: '',
  content: '',
  attachments: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  urgency: undefined as number | undefined,
  impact: undefined as number | undefined,
  categoryCode: '',
  ticketSource: undefined as number | undefined,
  submitterId: '',
  submitterName: '',
  assigneeId: '',
  assigneeName: '',
  knowledgeId: '',
  parentTicketId: '',
  firstResponseAtStart: '',
  firstResponseAtEnd: '',
  firstResponseDueByStart: '',
  firstResponseDueByEnd: '',
  resolvedAtStart: '',
  resolvedAtEnd: '',
  resolutionDueByStart: '',
  resolutionDueByEnd: '',
  closedAtStart: '',
  closedAtEnd: '',
  itAssetId: '',
  assetCode: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
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
</script>
