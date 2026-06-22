<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/knowledge -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台知识库实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-help-desk-knowledge">
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
      create-permission="routine:helpdesk:knowledge:create"
      update-permission="routine:helpdesk:knowledge:update"
      delete-permission="routine:helpdesk:knowledge:delete"
      import-permission="routine:helpdesk:knowledge:import"
      export-permission="routine:helpdesk:knowledge:export"
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
      :id-column-key="'knowledgeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getKnowledgeId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.knowledgeChangeLog._self') }}</div>
          <a-table
            v-if="hasKnowledgeChangeLogRows(record)"
            :columns="knowledgeChangeLogExpandColumns"
            :data-source="getKnowledgeChangeLogRows(record)"
            :row-key="(row: KnowledgeChangeLog, index?: number) => row?.knowledgeChangeLogId || String(index ?? 0)"
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
      <KnowledgeForm
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
      :storage-key="'takt-query-fields-routine-help-desk-knowledge'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.knowledge.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.knowledge.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.knowledge.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('summary')">
      <a-form-item :label="t('entity.knowledge.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.summary') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('categoryCode')">
      <a-form-item :label="t('entity.knowledge.categorycode')">
        <a-input
          v-model:value="advancedQueryForm.categoryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.categorycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tags')">
      <a-form-item :label="t('entity.knowledge.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('knowledgeStatus')">
      <a-form-item :label="t('entity.knowledge.status')">
        <a-input-number
          v-model:value="advancedQueryForm.knowledgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.knowledge.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('viewCount')">
      <a-form-item :label="t('entity.knowledge.viewcount')">
        <a-input-number
          v-model:value="advancedQueryForm.viewCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.viewcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('helpfulCount')">
      <a-form-item :label="t('entity.knowledge.helpfulcount')">
        <a-input-number
          v-model:value="advancedQueryForm.helpfulCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.helpfulcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unhelpfulCount')">
      <a-form-item :label="t('entity.knowledge.unhelpfulcount')">
        <a-input-number
          v-model:value="advancedQueryForm.unhelpfulCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.unhelpfulcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPublished')">
      <a-form-item :label="t('entity.knowledge.ispublished')">
        <a-input-number
          v-model:value="advancedQueryForm.isPublished"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.ispublished') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('version')">
      <a-form-item :label="t('entity.knowledge.version')">
        <a-input-number
          v-model:value="advancedQueryForm.version"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.version') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishedAtStart')">
      <a-form-item :label="t('entity.knowledge.publishedatstart')">
        <a-input
          v-model:value="advancedQueryForm.publishedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.publishedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishedAtEnd')">
      <a-form-item :label="t('entity.knowledge.publishedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.knowledge.publishedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedAtStart')">
      <a-form-item :label="t('entity.knowledge.revisedatstart')">
        <a-input
          v-model:value="advancedQueryForm.revisedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.knowledge.revisedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisedAtEnd')">
      <a-form-item :label="t('entity.knowledge.revisedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.revisedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.knowledge.revisedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.knowledge._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.knowledge._self"
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
      :id-column-key="'knowledgeId'"
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
 * 服务台知识库实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/knowledge
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import KnowledgeForm from './components/knowledge-form.vue'
import { getKnowledgeList, getKnowledgeById, createKnowledge, updateKnowledge, deleteKnowledgeById, deleteKnowledgeBatch, getKnowledgeTemplate, importKnowledge, exportKnowledge } from '@/api/routine/help-desk/knowledge'
import * as knowledgeChangeLogApi from '@/api/routine/help-desk/knowledge-change-log'
import type { KnowledgeChangeLog, KnowledgeChangeLogQuery } from '@/types/routine/help-desk/knowledge-change-log'
import type { Knowledge, KnowledgeQuery, KnowledgeCreate, KnowledgeUpdate } from '@/types/routine/help-desk/knowledge'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktKnowledge')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.knowledge._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Knowledge[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Knowledge | null>(null)
/** 表格多选行 */
const selectedRows = ref<Knowledge[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Knowledge>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  title: '',
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  helpfulCount: undefined as number | undefined,
  unhelpfulCount: undefined as number | undefined,
  isPublished: undefined as number | undefined,
  version: undefined as number | undefined,
  publishedAtStart: '',
  publishedAtEnd: '',
  revisedAtStart: '',
  revisedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'title', label: t('entity.knowledge.title') },
  { key: 'content', label: t('entity.knowledge.content') },
  { key: 'summary', label: t('entity.knowledge.summary') },
  { key: 'categoryCode', label: t('entity.knowledge.categorycode') },
  { key: 'tags', label: t('entity.knowledge.tags') },
  { key: 'knowledgeStatus', label: t('entity.knowledge.status') },
  { key: 'sortOrder', label: t('entity.knowledge.sortorder') },
  { key: 'viewCount', label: t('entity.knowledge.viewcount') },
  { key: 'helpfulCount', label: t('entity.knowledge.helpfulcount') },
  { key: 'unhelpfulCount', label: t('entity.knowledge.unhelpfulcount') },
  { key: 'isPublished', label: t('entity.knowledge.ispublished') },
  { key: 'version', label: t('entity.knowledge.version') },
  { key: 'publishedAtStart', label: t('entity.knowledge.publishedatstart') },
  { key: 'publishedAtEnd', label: t('entity.knowledge.publishedatend') },
  { key: 'revisedAtStart', label: t('entity.knowledge.revisedatstart') },
  { key: 'revisedAtEnd', label: t('entity.knowledge.revisedatend') },
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
const entityIdName = 'knowledgeId'
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

/** 展开行预览：knowledgeChangeLog 列 */
const knowledgeChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.knowledgeChangeLog.knowledgename'),
    dataIndex: 'knowledgeName',
    key: 'knowledgeName',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.knowledgetitle'),
    dataIndex: 'knowledgeTitle',
    key: 'knowledgeTitle',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.versionatchange'),
    dataIndex: 'versionAtChange',
    key: 'versionAtChange',
    ellipsis: true,
  },
  {
    title: t('entity.knowledgeChangeLog.knowledge'),
    dataIndex: 'knowledge',
    key: 'knowledge',
    ellipsis: true,
  },
])

/** 读取主表行上的 knowledgeChangeLog 子表缓存 */
function getKnowledgeChangeLogRows(record: Knowledge): KnowledgeChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 knowledgeChangeLog 子表 */
function hasKnowledgeChangeLogRows(record: Knowledge): boolean {
  return getKnowledgeChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadKnowledgeDetail(record: Knowledge): Promise<Knowledge | null> {
  const id = getKnowledgeId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getKnowledgeById(id)
    const index = dataSource.value.findIndex((row) => getKnowledgeId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Knowledge
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 knowledgeChangeLog 子表（KnowledgeChangeLogQuery + knowledgeChangeLogApi，与主表 KnowledgeQuery 分离） */
async function loadKnowledgeChangeLogForKnowledge(record: Knowledge): Promise<KnowledgeChangeLog[]> {
  const masterId = getKnowledgeId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: KnowledgeChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      knowledgeId: masterId,
    }
    const result = await knowledgeChangeLogApi.getKnowledgeChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getKnowledgeId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as Knowledge
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureKnowledgeChildrenLoaded(record: Knowledge) {
  if (!hasKnowledgeChangeLogRows(record)) {
    await loadKnowledgeChangeLogForKnowledge(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Knowledge) {
  const key = getKnowledgeId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureKnowledgeChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'knowledgeId',
    key: 'knowledgeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'knowledgeId') ?? ''
  },
  {
    title: t('entity.knowledge.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'title') ?? ''
  },
  {
    title: t('entity.knowledge.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'content') ?? ''
  },
  {
    title: t('entity.knowledge.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'summary') ?? ''
  },
  {
    title: t('entity.knowledge.categorycode'),
    dataIndex: 'categoryCode',
    key: 'categoryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'categoryCode') ?? ''
  },
  {
    title: t('entity.knowledge.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'tags') ?? ''
  },
  {
    title: t('entity.knowledge.status'),
    dataIndex: 'knowledgeStatus',
    key: 'knowledgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'knowledgeStatus') ?? ''
  },
  {
    title: t('entity.knowledge.viewcount'),
    dataIndex: 'viewCount',
    key: 'viewCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'viewCount') ?? ''
  },
  {
    title: t('entity.knowledge.helpfulcount'),
    dataIndex: 'helpfulCount',
    key: 'helpfulCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'helpfulCount') ?? ''
  },
  {
    title: t('entity.knowledge.unhelpfulcount'),
    dataIndex: 'unhelpfulCount',
    key: 'unhelpfulCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'unhelpfulCount') ?? ''
  },
  {
    title: t('entity.knowledge.ispublished'),
    dataIndex: 'isPublished',
    key: 'isPublished',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'isPublished') ?? ''
  },
  {
    title: t('entity.knowledge.version'),
    dataIndex: 'version',
    key: 'version',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'version') ?? ''
  },
  {
    title: t('entity.knowledge.publishedat'),
    dataIndex: 'publishedAt',
    key: 'publishedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'publishedAt') ?? ''
  },
  {
    title: t('entity.knowledge.revisedat'),
    dataIndex: 'revisedAt',
    key: 'revisedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getKnowledgeField(record, 'revisedAt') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:helpdesk:knowledge:update',
        onClick: (record: Knowledge) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:helpdesk:knowledge:delete',
        onClick: (record: Knowledge) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getKnowledgeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getKnowledgeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Knowledge[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Knowledge, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getKnowledgeId(selectedRow.value) === getKnowledgeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Knowledge[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Knowledge) => ({
  onClick: () => {
    const key = getKnowledgeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getKnowledgeId(item)))
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
    const params: KnowledgeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getKnowledgeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Knowledge] 加载数据失败', { error })
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
  title: '',
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  helpfulCount: undefined as number | undefined,
  unhelpfulCount: undefined as number | undefined,
  isPublished: undefined as number | undefined,
  version: undefined as number | undefined,
  publishedAtStart: '',
  publishedAtEnd: '',
  revisedAtStart: '',
  revisedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.knowledge._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Knowledge) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.knowledge._self') })
  formLoading.value = true
  try {
    const detail = await loadKnowledgeDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.knowledge._self') }))
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
      await updateKnowledge(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.knowledge._self') }))
    } else {
      await createKnowledge(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.knowledge._self') }))
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
  const res = await getKnowledgeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importKnowledge(file, sheetName)
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
    const exportQuery: KnowledgeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportKnowledge(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.knowledge._self') }))
  } catch (error: any) {
    logger.error('[Knowledge] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.knowledge._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Knowledge) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.knowledge._self'), name: t('common.tip.this.target', { target: t('entity.knowledge._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteKnowledgeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.knowledge._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.knowledge._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.knowledge._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteKnowledgeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.knowledge._self') }))
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
  content: '',
  summary: '',
  categoryCode: '',
  tags: '',
  knowledgeStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  viewCount: undefined as number | undefined,
  helpfulCount: undefined as number | undefined,
  unhelpfulCount: undefined as number | undefined,
  isPublished: undefined as number | undefined,
  version: undefined as number | undefined,
  publishedAtStart: '',
  publishedAtEnd: '',
  revisedAtStart: '',
  revisedAtEnd: '',
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
.routine-help-desk-knowledge {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
