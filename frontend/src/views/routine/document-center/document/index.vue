<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-document-center-document">
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
      create-permission="routine:documentcenter:document:create"
      update-permission="routine:documentcenter:document:update"
      delete-permission="routine:documentcenter:document:delete"
      import-permission="routine:documentcenter:document:import"
      export-permission="routine:documentcenter:document:export"
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
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'documentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getDocumentId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.documentVersion._self') }}</div>
          <a-table
            v-if="hasDocumentVersionRows(record)"
            :columns="documentVersionExpandColumns"
            :data-source="getDocumentVersionRows(record)"
            :row-key="(row: DocumentVersion, index?: number) => row?.documentVersionId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.documentChangeLog._self') }}</div>
          <a-table
            v-if="hasDocumentChangeLogRows(record)"
            :columns="documentChangeLogExpandColumns"
            :data-source="getDocumentChangeLogRows(record)"
            :row-key="(row: DocumentChangeLog, index?: number) => row?.documentChangeLogId || String(index ?? 0)"
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
      <DocumentForm
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
      :storage-key="'takt-query-fields-routine-document-center-document'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('documentCode')">
      <a-form-item :label="t('entity.document.code')">
        <a-input
          v-model:value="advancedQueryForm.documentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.document.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentCategory')">
      <a-form-item :label="t('entity.document.category')">
        <a-input-number
          v-model:value="advancedQueryForm.documentCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.category') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentStatus')">
      <a-form-item :label="t('entity.document.status')">
        <a-input-number
          v-model:value="advancedQueryForm.documentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('confidentialLevel')">
      <a-form-item :label="t('entity.document.confidentiallevel')">
        <a-input-number
          v-model:value="advancedQueryForm.confidentialLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.confidentiallevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('version')">
      <a-form-item :label="t('entity.document.version')">
        <a-input-number
          v-model:value="advancedQueryForm.version"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.version') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.document.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('summary')">
      <a-form-item :label="t('entity.document.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.summary') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tags')">
      <a-form-item :label="t('entity.document.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileId')">
      <a-form-item :label="t('entity.document.fileid')">
        <a-input
          v-model:value="advancedQueryForm.fileId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="t('entity.document.filename')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('filePath')">
      <a-form-item :label="t('entity.document.filepath')">
        <a-input
          v-model:value="advancedQueryForm.filePath"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filepath') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileSize')">
      <a-form-item :label="t('entity.document.filesize')">
        <a-input
          v-model:value="advancedQueryForm.fileSize"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filesize') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileType')">
      <a-form-item :label="t('entity.document.filetype')">
        <a-input
          v-model:value="advancedQueryForm.fileType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileExtension')">
      <a-form-item :label="t('entity.document.fileextension')">
        <a-input
          v-model:value="advancedQueryForm.fileExtension"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileextension') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveTimeStart')">
      <a-form-item :label="t('entity.document.effectivetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.effectivetimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveTimeEnd')">
      <a-form-item :label="t('entity.document.effectivetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.effectivetimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeStart')">
      <a-form-item :label="t('entity.document.expiretimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.expiretimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeEnd')">
      <a-form-item :label="t('entity.document.expiretimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.expiretimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeStart')">
      <a-form-item :label="t('entity.document.publishtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.publishtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeEnd')">
      <a-form-item :label="t('entity.document.publishtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.publishtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherId')">
      <a-form-item :label="t('entity.document.publisherid')">
        <a-input
          v-model:value="advancedQueryForm.publisherId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publisherid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherName')">
      <a-form-item :label="t('entity.document.publishername')">
        <a-input
          v-model:value="advancedQueryForm.publisherName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publishername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.document.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.document.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isTop')">
      <a-form-item :label="t('entity.document.istop')">
        <a-input-number
          v-model:value="advancedQueryForm.isTop"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.istop') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.document.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('viewCount')">
      <a-form-item :label="t('entity.document.viewcount')">
        <a-input-number
          v-model:value="advancedQueryForm.viewCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.viewcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downloadCount')">
      <a-form-item :label="t('entity.document.downloadcount')">
        <a-input-number
          v-model:value="advancedQueryForm.downloadCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.downloadcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetScope')">
      <a-form-item :label="t('entity.document.targetscope')">
        <a-textarea
          v-model:value="advancedQueryForm.targetScope"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.targetscope') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetDepartments')">
      <a-form-item :label="t('entity.document.targetdepartments')">
        <a-input
          v-model:value="advancedQueryForm.targetDepartments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetdepartments') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetUsers')">
      <a-form-item :label="t('entity.document.targetusers')">
        <a-input
          v-model:value="advancedQueryForm.targetUsers"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetusers') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.document.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.document.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.document.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.document.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.document.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.document.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.document.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.document.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.document._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.document._self"
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
      :id-column-key="'documentId'"
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
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/document-center/document
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import DocumentForm from './components/document-form.vue'
import { getDocumentList, getDocumentById, createDocument, updateDocument, deleteDocumentById, deleteDocumentBatch, getDocumentTemplate, importDocument, exportDocument } from '@/api/routine/document-center/document'
import * as documentVersionApi from '@/api/routine/document-center/document-version'
import * as documentChangeLogApi from '@/api/routine/document-center/document-change-log'
import type { DocumentVersion, DocumentVersionQuery } from '@/types/routine/document-center/document-version'
import type { DocumentChangeLog, DocumentChangeLogQuery } from '@/types/routine/document-center/document-change-log'
import type { Document, DocumentQuery, DocumentCreate, DocumentUpdate } from '@/types/routine/document-center/document'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktDocument')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.document._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Document[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Document | null>(null)
/** 表格多选行 */
const selectedRows = ref<Document[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Document>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  fileId: '',
  fileName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  publisherId: '',
  publisherName: '',
  deptId: '',
  deptName: '',
  isTop: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  downloadCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
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
  { key: 'documentCode', label: t('entity.document.code') },
  { key: 'title', label: t('entity.document.title') },
  { key: 'documentCategory', label: t('entity.document.category') },
  { key: 'documentStatus', label: t('entity.document.status') },
  { key: 'confidentialLevel', label: t('entity.document.confidentiallevel') },
  { key: 'version', label: t('entity.document.version') },
  { key: 'content', label: t('entity.document.content') },
  { key: 'summary', label: t('entity.document.summary') },
  { key: 'tags', label: t('entity.document.tags') },
  { key: 'fileId', label: t('entity.document.fileid') },
  { key: 'fileName', label: t('entity.document.filename') },
  { key: 'filePath', label: t('entity.document.filepath') },
  { key: 'fileSize', label: t('entity.document.filesize') },
  { key: 'fileType', label: t('entity.document.filetype') },
  { key: 'fileExtension', label: t('entity.document.fileextension') },
  { key: 'effectiveTimeStart', label: t('entity.document.effectivetimestart') },
  { key: 'effectiveTimeEnd', label: t('entity.document.effectivetimeend') },
  { key: 'expireTimeStart', label: t('entity.document.expiretimestart') },
  { key: 'expireTimeEnd', label: t('entity.document.expiretimeend') },
  { key: 'publishTimeStart', label: t('entity.document.publishtimestart') },
  { key: 'publishTimeEnd', label: t('entity.document.publishtimeend') },
  { key: 'publisherId', label: t('entity.document.publisherid') },
  { key: 'publisherName', label: t('entity.document.publishername') },
  { key: 'deptId', label: t('entity.document.deptid') },
  { key: 'deptName', label: t('entity.document.deptname') },
  { key: 'isTop', label: t('entity.document.istop') },
  { key: 'sortOrder', label: t('entity.document.sortorder') },
  { key: 'viewCount', label: t('entity.document.viewcount') },
  { key: 'downloadCount', label: t('entity.document.downloadcount') },
  { key: 'targetScope', label: t('entity.document.targetscope') },
  { key: 'targetDepartments', label: t('entity.document.targetdepartments') },
  { key: 'targetUsers', label: t('entity.document.targetusers') },
  { key: 'approvalStatus', label: t('entity.document.approvalstatus') },
  { key: 'initiatorId', label: t('entity.document.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.document.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.document.initiatedatend') },
  { key: 'approvedBy', label: t('entity.document.approvedby') },
  { key: 'approvedAtStart', label: t('entity.document.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.document.approvedatend') },
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
const entityIdName = 'documentId'
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

/** 展开行预览：documentVersion 列 */
const documentVersionExpandColumns = computed(() => [
  {
    title: t('entity.documentVersion.documentname'),
    dataIndex: 'documentName',
    key: 'documentName',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.versionno'),
    dataIndex: 'versionNo',
    key: 'versionNo',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.versionnote'),
    dataIndex: 'versionNote',
    key: 'versionNote',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    ellipsis: true,
  },
  {
    title: t('entity.documentVersion.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    ellipsis: true,
  },
])

/** 展开行预览：documentChangeLog 列 */
const documentChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.documentChangeLog.documentname'),
    dataIndex: 'documentName',
    key: 'documentName',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.documentcode'),
    dataIndex: 'documentCode',
    key: 'documentCode',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.documenttitle'),
    dataIndex: 'documentTitle',
    key: 'documentTitle',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.documentChangeLog.versionatchange'),
    dataIndex: 'versionAtChange',
    key: 'versionAtChange',
    ellipsis: true,
  },
])

/** 读取主表行上的 documentVersion 子表缓存 */
function getDocumentVersionRows(record: Document): DocumentVersion[] {
  return (record as any)?.versions ?? []
}

/** 主表行是否已加载 documentVersion 子表 */
function hasDocumentVersionRows(record: Document): boolean {
  return getDocumentVersionRows(record).length > 0
}

/** 读取主表行上的 documentChangeLog 子表缓存 */
function getDocumentChangeLogRows(record: Document): DocumentChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 documentChangeLog 子表 */
function hasDocumentChangeLogRows(record: Document): boolean {
  return getDocumentChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadDocumentDetail(record: Document): Promise<Document | null> {
  const id = getDocumentId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getDocumentById(id)
    const index = dataSource.value.findIndex((row) => getDocumentId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Document
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 documentVersion 子表（DocumentVersionQuery + documentVersionApi，与主表 DocumentQuery 分离） */
async function loadDocumentVersionForDocument(record: Document): Promise<DocumentVersion[]> {
  const masterId = getDocumentId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: DocumentVersionQuery = {
      pageIndex: 1,
      pageSize: 500,
      documentId: masterId,
    }
    const result = await documentVersionApi.getDocumentVersionList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getDocumentId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, versions: rows } as Document
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 documentChangeLog 子表（DocumentChangeLogQuery + documentChangeLogApi，与主表 DocumentQuery 分离） */
async function loadDocumentChangeLogForDocument(record: Document): Promise<DocumentChangeLog[]> {
  const masterId = getDocumentId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: DocumentChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      documentId: masterId,
    }
    const result = await documentChangeLogApi.getDocumentChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getDocumentId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as Document
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureDocumentChildrenLoaded(record: Document) {
  if (!hasDocumentVersionRows(record)) {
    await loadDocumentVersionForDocument(record)
  }
  if (!hasDocumentChangeLogRows(record)) {
    await loadDocumentChangeLogForDocument(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Document) {
  const key = getDocumentId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureDocumentChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'documentId',
    key: 'documentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentId') ?? ''
  },
  {
    title: t('entity.document.code'),
    dataIndex: 'documentCode',
    key: 'documentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentCode') ?? ''
  },
  {
    title: t('entity.document.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'title') ?? ''
  },
  {
    title: t('entity.document.category'),
    dataIndex: 'documentCategory',
    key: 'documentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentCategory') ?? ''
  },
  {
    title: t('entity.document.status'),
    dataIndex: 'documentStatus',
    key: 'documentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentStatus') ?? ''
  },
  {
    title: t('entity.document.confidentiallevel'),
    dataIndex: 'confidentialLevel',
    key: 'confidentialLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'confidentialLevel') ?? ''
  },
  {
    title: t('entity.document.version'),
    dataIndex: 'version',
    key: 'version',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'version') ?? ''
  },
  {
    title: t('entity.document.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'content') ?? ''
  },
  {
    title: t('entity.document.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'summary') ?? ''
  },
  {
    title: t('entity.document.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'tags') ?? ''
  },
  {
    title: t('entity.document.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileId') ?? ''
  },
  {
    title: t('entity.document.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileName') ?? ''
  },
  {
    title: t('entity.document.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'filePath') ?? ''
  },
  {
    title: t('entity.document.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileSize') ?? ''
  },
  {
    title: t('entity.document.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileType') ?? ''
  },
  {
    title: t('entity.document.fileextension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileExtension') ?? ''
  },
  {
    title: t('entity.document.effectivetime'),
    dataIndex: 'effectiveTime',
    key: 'effectiveTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'effectiveTime') ?? ''
  },
  {
    title: t('entity.document.expiretime'),
    dataIndex: 'expireTime',
    key: 'expireTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'expireTime') ?? ''
  },
  {
    title: t('entity.document.publishtime'),
    dataIndex: 'publishTime',
    key: 'publishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'publishTime') ?? ''
  },
  {
    title: t('entity.document.publisherid'),
    dataIndex: 'publisherId',
    key: 'publisherId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'publisherId') ?? ''
  },
  {
    title: t('entity.document.publishername'),
    dataIndex: 'publisherName',
    key: 'publisherName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'publisherName') ?? ''
  },
  {
    title: t('entity.document.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.document.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.document.istop'),
    dataIndex: 'isTop',
    key: 'isTop',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'isTop') ?? ''
  },
  {
    title: t('entity.document.viewcount'),
    dataIndex: 'viewCount',
    key: 'viewCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'viewCount') ?? ''
  },
  {
    title: t('entity.document.downloadcount'),
    dataIndex: 'downloadCount',
    key: 'downloadCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'downloadCount') ?? ''
  },
  {
    title: t('entity.document.targetscope'),
    dataIndex: 'targetScope',
    key: 'targetScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'targetScope') ?? ''
  },
  {
    title: t('entity.document.targetdepartments'),
    dataIndex: 'targetDepartments',
    key: 'targetDepartments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'targetDepartments') ?? ''
  },
  {
    title: t('entity.document.targetusers'),
    dataIndex: 'targetUsers',
    key: 'targetUsers',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'targetUsers') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:documentcenter:document:update',
        onClick: (record: Document) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:documentcenter:document:delete',
        onClick: (record: Document) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getDocumentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getDocumentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Document[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Document, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getDocumentId(selectedRow.value) === getDocumentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Document[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Document) => ({
  onClick: () => {
    const key = getDocumentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getDocumentId(item)))
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
    const params: DocumentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getDocumentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Document] 加载数据失败', { error })
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
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  fileId: '',
  fileName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  publisherId: '',
  publisherName: '',
  deptId: '',
  deptName: '',
  isTop: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  downloadCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.document._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Document) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.document._self') })
  formLoading.value = true
  try {
    const detail = await loadDocumentDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.document._self') }))
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
      await updateDocument(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.document._self') }))
    } else {
      await createDocument(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.document._self') }))
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
  const res = await getDocumentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importDocument(file, sheetName)
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
    const exportQuery: DocumentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportDocument(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.document._self') }))
  } catch (error: any) {
    logger.error('[Document] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.document._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Document) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.document._self'), name: t('common.tip.this.target', { target: t('entity.document._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDocumentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.document._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.document._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.document._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteDocumentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.document._self') }))
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
  documentCode: '',
  title: '',
  documentCategory: undefined as number | undefined,
  documentStatus: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  content: '',
  summary: '',
  tags: '',
  fileId: '',
  fileName: '',
  filePath: '',
  fileSize: '',
  fileType: '',
  fileExtension: '',
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  publisherId: '',
  publisherName: '',
  deptId: '',
  deptName: '',
  isTop: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  downloadCount: undefined as number | undefined,
  targetScope: '',
  targetDepartments: '',
  targetUsers: '',
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
.routine-document-center-document {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
