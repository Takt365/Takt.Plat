<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-help-desk-ticket">
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
      create-permission="routine:helpdesk:ticket:create"
      update-permission="routine:helpdesk:ticket:update"
      delete-permission="routine:helpdesk:ticket:delete"
      import-permission="routine:helpdesk:ticket:import"
      export-permission="routine:helpdesk:ticket:export"
      :left-actions="toolbarLeftActions"
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
      :id-column-key="'ticketId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTicketId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
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
        <template v-else-if="column.key === 'ticketSource'">
          <TaktDictTag
            :value="getTicketField(record, 'ticketSource')"
            dict-type="routine_ticket_source_type"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.ticketChangeLog._self') }}</div>
          <a-table
            v-if="hasTicketChangeLogRows(record)"
            :columns="ticketChangeLogExpandColumns"
            :data-source="getTicketChangeLogRows(record)"
            :row-key="(row: TicketChangeLog, index?: number) => row?.ticketChangeLogId || String(index ?? 0)"
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
      <TicketForm
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
      :storage-key="'takt-query-fields-routine-help-desk-ticket'"
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
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.ticket.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.title') })"
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
      <div v-show="isFieldVisible('attachmentsJson')">
      <a-form-item :label="t('entity.ticket.attachmentsjson')">
        <a-input
          v-model:value="advancedQueryForm.attachmentsJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.attachmentsjson') })"
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
      <div v-show="isFieldVisible('categoryCode')">
      <a-form-item :label="t('entity.ticket.categorycode')">
        <a-input
          v-model:value="advancedQueryForm.categoryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.categorycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ticketSource')">
      <a-form-item :label="t('entity.ticket.source')">
        <TaktSelect
          v-model:value="advancedQueryForm.ticketSource"
          dict-type="routine_ticket_source_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.source') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('submitterId')">
      <a-form-item :label="t('entity.ticket.submitterid')">
        <a-input
          v-model:value="advancedQueryForm.submitterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submitterid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('submitterName')">
      <a-form-item :label="t('entity.ticket.submittername')">
        <a-input
          v-model:value="advancedQueryForm.submitterName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.submittername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assigneeId')">
      <a-form-item :label="t('entity.ticket.assigneeid')">
        <a-input
          v-model:value="advancedQueryForm.assigneeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assigneeName')">
      <a-form-item :label="t('entity.ticket.assigneename')">
        <a-input
          v-model:value="advancedQueryForm.assigneeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.assigneename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('knowledgeId')">
      <a-form-item :label="t('entity.ticket.knowledgeid')">
        <a-input
          v-model:value="advancedQueryForm.knowledgeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.knowledgeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentTicketId')">
      <a-form-item :label="t('entity.ticket.parentticketid')">
        <a-input
          v-model:value="advancedQueryForm.parentTicketId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.parentticketid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseAtStart')">
      <a-form-item :label="t('entity.ticket.firstresponseatstart')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseAtEnd')">
      <a-form-item :label="t('entity.ticket.firstresponseatend')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseDueByStart')">
      <a-form-item :label="t('entity.ticket.firstresponseduebystart')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseDueByStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseduebystart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('firstResponseDueByEnd')">
      <a-form-item :label="t('entity.ticket.firstresponseduebyend')">
        <a-input
          v-model:value="advancedQueryForm.firstResponseDueByEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.firstresponseduebyend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolvedAtStart')">
      <a-form-item :label="t('entity.ticket.resolvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.resolvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolvedatstart') })"
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
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resolutionDueByEnd')">
      <a-form-item :label="t('entity.ticket.resolutionduebyend')">
        <a-input
          v-model:value="advancedQueryForm.resolutionDueByEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.resolutionduebyend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedAtStart')">
      <a-form-item :label="t('entity.ticket.closedatstart')">
        <a-input
          v-model:value="advancedQueryForm.closedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.closedatstart') })"
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
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.ticket.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantDeptId')">
      <a-form-item :label="t('entity.ticket.applicantdeptid')">
        <a-input
          v-model:value="advancedQueryForm.applicantDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantDeptName')">
      <a-form-item :label="t('entity.ticket.applicantdeptname')">
        <a-input
          v-model:value="advancedQueryForm.applicantDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantdeptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantBy')">
      <a-form-item :label="t('entity.ticket.applicantby')">
        <a-input
          v-model:value="advancedQueryForm.applicantBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ticket.applicantby') })"
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
    <!-- 工单 ITSM 工作流抽屉 -->
    <TicketWorkflowDrawer
      v-model:open="workflowVisible"
      :ticket-id="workflowTicketId"
      @changed="loadData"
    />
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
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * Takt工单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/ticket
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import TicketForm from './components/ticket-form.vue'
import TicketWorkflowDrawer from './components/ticket-workflow-drawer.vue'
import { getTicketList, getTicketById, submitTicket, updateTicket, deleteTicketById, deleteTicketBatch, getTicketTemplate, importTicket, exportTicket } from '@/api/routine/help-desk/ticket'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index'
import * as ticketChangeLogApi from '@/api/routine/help-desk/ticket-change-log'
import type { TicketChangeLog, TicketChangeLogQuery } from '@/types/routine/help-desk/ticket-change-log'
import type { Ticket, TicketQuery, TicketSubmit, TicketUpdate } from '@/types/routine/help-desk/ticket'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiCustomerService2Line } from '@remixicon/vue'

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
const formData = ref<Partial<Ticket>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 工作流抽屉 */
const workflowVisible = ref(false)
/** 工作流当前工单 ID */
const workflowTicketId = ref<string | null>(null)
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  ticketNo: '',
  title: '',
  content: '',
  attachmentsJson: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
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
  flowInstanceId: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'ticketNo', label: t('entity.ticket.no') },
  { key: 'title', label: t('entity.ticket.title') },
  { key: 'content', label: t('entity.ticket.content') },
  { key: 'attachmentsJson', label: t('entity.ticket.attachmentsjson') },
  { key: 'ticketStatus', label: t('entity.ticket.status') },
  { key: 'urgency', label: t('entity.ticket.urgency') },
  { key: 'impact', label: t('entity.ticket.impact') },
  { key: 'priority', label: t('entity.ticket.priority') },
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
  { key: 'flowInstanceId', label: t('entity.ticket.flowinstanceid') },
  { key: 'applicantDeptId', label: t('entity.ticket.applicantdeptid') },
  { key: 'applicantDeptName', label: t('entity.ticket.applicantdeptname') },
  { key: 'applicantBy', label: t('entity.ticket.applicantby') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
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

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：ticketChangeLog 列 */
const ticketChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.ticketChangeLog.ticketname'),
    dataIndex: 'ticketName',
    key: 'ticketName',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.ticketno'),
    dataIndex: 'ticketNo',
    key: 'ticketNo',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.ticketChangeLog.ticket'),
    dataIndex: 'ticket',
    key: 'ticket',
    ellipsis: true,
  },
])

/** 读取主表行上的 ticketChangeLog 子表缓存 */
function getTicketChangeLogRows(record: Ticket): TicketChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 ticketChangeLog 子表 */
function hasTicketChangeLogRows(record: Ticket): boolean {
  return getTicketChangeLogRows(record).length > 0
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
/** 懒加载 ticketChangeLog 子表（TicketChangeLogQuery + ticketChangeLogApi，与主表 TicketQuery 分离） */
async function loadTicketChangeLogForTicket(record: Ticket): Promise<TicketChangeLog[]> {
  const masterId = getTicketId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: TicketChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      ticketId: masterId,
    }
    const result = await ticketChangeLogApi.getTicketChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getTicketId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as Ticket
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureTicketChildrenLoaded(record: Ticket) {
  if (!hasTicketChangeLogRows(record)) {
    await loadTicketChangeLogForTicket(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Ticket) {
  const key = getTicketId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureTicketChildrenLoaded(record)
  expandedRowKeys.value = [key]
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
    title: t('entity.ticket.attachmentsjson'),
    dataIndex: 'attachmentsJson',
    key: 'attachmentsJson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'attachmentsJson') ?? ''
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
    title: t('entity.ticket.urgency'),
    dataIndex: 'urgency',
    key: 'urgency',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.ticket.impact'),
    dataIndex: 'impact',
    key: 'impact',
    width: 100,
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
    title: t('entity.ticket.knowledgename'),
    dataIndex: 'knowledgeName',
    key: 'knowledgeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'knowledgeName') ?? ''
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
    title: t('entity.ticket.parentticketname'),
    dataIndex: 'parentTicketName',
    key: 'parentTicketName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'parentTicketName') ?? ''
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
    title: t('entity.ticket.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.ticket.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTicketField(record, 'flowInstanceName') ?? ''
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
        key: 'workflow',
        label: t('routine.help-desk.ticket.page.workflow.title'),
        shape: 'plain',
        icon: RiCustomerService2Line,
        permission: 'routine:helpdesk:ticket:query',
        onClick: (record: Ticket) => handleOpenWorkflow(record)
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:helpdesk:ticket:update',
        onClick: (record: Ticket) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:helpdesk:ticket:delete',
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
  },
  onSelect: (record: Ticket, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getTicketId(selectedRow.value) === getTicketId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Ticket[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Ticket) => ({
  onClick: () => {
    const key = getTicketId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTicketId(item)))
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
    const params: TicketQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getTicketList(params)
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
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  ticketNo: '',
  title: '',
  content: '',
  attachmentsJson: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
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
  flowInstanceId: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ticket._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开工单工作流抽屉 */
function handleOpenWorkflow(record: Ticket): void {
  workflowTicketId.value = getTicketId(record)
  workflowVisible.value = true
}

/** 工具栏扩展：工单处理（选中单行） */
const toolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'workflow',
    label: t('routine.help-desk.ticket.page.workflow.title'),
    shape: 'plain',
    icon: RiCustomerService2Line,
    permission: 'routine:helpdesk:ticket:query',
    disabled: !selectedRow.value,
    onClick: () => {
      if (selectedRow.value) {
        handleOpenWorkflow(selectedRow.value)
      }
    },
  },
])

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
      await updateTicket(id, payload as TicketUpdate)
      message.success(t('common.feedback.updated', { target: t('entity.ticket._self') }))
    } else {
      const submitDto: TicketSubmit = {
        title: payload.title,
        content: payload.content,
        attachmentsJson: payload.attachmentsJson,
        priority: payload.priority,
        categoryCode: payload.categoryCode,
        remark: payload.remark,
      }
      await submitTicket(submitDto)
      message.success(t('common.feedback.created', { target: t('entity.ticket._self') }))
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: TicketQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportTicket(exportQuery, excelNames.sheet, excelNames.fileBase)
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
  ticketNo: '',
  title: '',
  content: '',
  attachmentsJson: '',
  ticketStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
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
  flowInstanceId: '',
  applicantDeptId: '',
  applicantDeptName: '',
  applicantBy: '',
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
.routine-help-desk-ticket {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
