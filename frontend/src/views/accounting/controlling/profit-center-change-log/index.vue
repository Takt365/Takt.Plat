<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/profit-center-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：利润中心实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:controlling:profit:center:create"
      update-permission="accounting:controlling:profit:center:update"
      delete-permission="accounting:controlling:profit:center:delete"
      import-permission="accounting:controlling:profit:center:import"
      export-permission="accounting:controlling:profit:center:export"
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
      :master-row-key="getProfitCenterId"
      :master-row-selection="rowSelection"
      master-id-column-key="profitCenterId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'profitCenterStatus'">
          <a-switch
            :checked="getProfitCenterDictValue(record, 'profitCenterStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleProfitCenterStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <ProfitCenterChangeLogPanel
          ref="profitCenterChangeLogPanelRef"
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
      <ProfitCenterForm
        :key="formData?.profitCenterId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-profit-center-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('profitCenterCode')">
      <a-form-item :label="pi.queryLabel('profitCenterCode')">
        <a-input
          v-model:value="advancedQueryForm.profitCenterCode"
          :placeholder="pi.queryPh('profitCenterCode', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterName')">
      <a-form-item :label="pi.queryLabel('profitCenterName')">
        <a-input
          v-model:value="advancedQueryForm.profitCenterName"
          :placeholder="pi.queryPh('profitCenterName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="pi.queryLabel('parentId')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="pi.queryPh('parentId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('managerId')">
      <a-form-item :label="pi.queryLabel('managerId')">
        <a-input
          v-model:value="advancedQueryForm.managerId"
          :placeholder="pi.queryPh('managerId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('managerName')">
      <a-form-item :label="pi.queryLabel('managerName')">
        <a-input
          v-model:value="advancedQueryForm.managerName"
          :placeholder="pi.queryPh('managerName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="pi.queryLabel('deptId')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="pi.queryPh('deptId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="pi.queryLabel('deptName')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="pi.queryPh('deptName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterLevel')">
      <a-form-item :label="pi.queryLabel('profitCenterLevel')">
        <a-input-number
          v-model:value="advancedQueryForm.profitCenterLevel"
          :placeholder="pi.queryPh('profitCenterLevel', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="pi.queryLabel('validFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="pi.queryPh('validFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="pi.queryLabel('validFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="pi.queryPh('validFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="pi.queryLabel('validToStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="pi.queryPh('validToStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="pi.queryLabel('validToEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="pi.queryPh('validToEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="pi.queryLabel('relatedPlant')">
        <TaktSelect
          v-model:value="advancedQueryForm.relatedPlant"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('relatedPlant', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterStatus')">
      <a-form-item :label="pi.queryLabel('profitCenterStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.profitCenterStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="pi.queryPh('profitCenterStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="PROFITCENTER_SELF_I18N_KEY"
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
      :id-column-key="'profitCenterId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 利润中心实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/profit-center-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ProfitCenterForm from './components/profit-center-form.vue'
import ProfitCenterChangeLogPanel from './components/profit-center-change-log-panel.vue'
import { provideProfitCenterMasterContext, type ProfitCenterRowRecord } from './composables/use-profit-center-master-context'
import { getProfitCenterList, getProfitCenterById, createProfitCenter, updateProfitCenter, deleteProfitCenterById, deleteProfitCenterBatch, getProfitCenterTemplate, importProfitCenter, exportProfitCenter, updateProfitCenterStatus } from '@/api/accounting/controlling/profit-center'
import type { ProfitCenter, ProfitCenterQuery } from '@/types/accounting/controlling/profit-center'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useProfitCenterI18n,
  PROFITCENTER_LIST_FIELDS,
  PROFITCENTER_QUERY_STRING_FIELDS,
  PROFITCENTER_QUERY_FIELDS,
  PROFITCENTER_SELF_I18N_KEY,
} from './composables/use-profit-center-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useProfitCenterI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktProfitCenter')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ProfitCenter[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ProfitCenterRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<ProfitCenterRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ProfitCenter> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(PROFITCENTER_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PROFITCENTER_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    profitCenterLevel: undefined as number | undefined,
    profitCenterStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  PROFITCENTER_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'profitCenterId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideProfitCenterMasterContext()
const profitCenterChangeLogPanelRef = ref<InstanceType<typeof ProfitCenterChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ProfitCenterQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ProfitCenterQuery>): ProfitCenterQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ProfitCenterQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ProfitCenterQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PROFITCENTER_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.profitCenterLevel !== undefined && form.profitCenterLevel !== null) {
    query.profitCenterLevel = form.profitCenterLevel
  }
  if (form.profitCenterStatus !== undefined && form.profitCenterStatus !== null) {
    query.profitCenterStatus = form.profitCenterStatus
  }
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
function syncMasterSelection(record: ProfitCenterRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getProfitCenterId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as ProfitCenterRowRecord
  const key = getProfitCenterId(row)
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
async function loadProfitCenterDetail(record: ProfitCenterRowRecord): Promise<ProfitCenter | null> {
  const id = getProfitCenterId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getProfitCenterById(id)
    const index = dataSource.value.findIndex((row) => getProfitCenterId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ProfitCenter
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
    dataIndex: 'profitCenterId',
    key: 'profitCenterId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'profitCenterId') ?? ''
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'profitCenterCode') ?? ''
  },
  {
    title: pi.label('profitCenterName'),
    dataIndex: 'profitCenterName',
    key: 'profitCenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'profitCenterName') ?? ''
  },
  {
    title: pi.label('parentId'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('managerId'),
    dataIndex: 'managerId',
    key: 'managerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'managerId') ?? ''
  },
  {
    title: pi.label('managerName'),
    dataIndex: 'managerName',
    key: 'managerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'managerName') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('profitCenterLevel'),
    dataIndex: 'profitCenterLevel',
    key: 'profitCenterLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'profitCenterLevel') ?? ''
  },
  {
    title: pi.label('validFrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'validFrom') ?? ''
  },
  {
    title: pi.label('validTo'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'validTo') ?? ''
  },
  {
    title: pi.label('relatedPlant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getProfitCenterField(record, 'relatedPlant') ?? ''
  },
  {
    title: pi.label('profitCenterStatus'),
    dataIndex: 'profitCenterStatus',
    key: 'profitCenterStatus',
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
        permission: 'accounting:controlling:profit:center:update',
        onClick: (record: ProfitCenterRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:profit:center:delete',
        onClick: (record: ProfitCenterRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getProfitCenterId = (record: ProfitCenterRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getProfitCenterField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getProfitCenterDictValue = (
  record: ProfitCenterRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toProfitCenterNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ProfitCenterRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: ProfitCenterRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getProfitCenterId(selectedRow.value) === getProfitCenterId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ProfitCenterRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getProfitCenterList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ProfitCenter] 加载数据失败', { error })
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
  profitCenterCode: '',
  profitCenterName: '',
  parentId: '',
  managerId: '',
  managerName: '',
  deptId: '',
  deptName: '',
  profitCenterLevel: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  relatedPlant: '',
  profitCenterStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ProfitCenterRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadProfitCenterDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      await updateProfitCenter(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createProfitCenter(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  profitCenterChangeLogPanelRef.value?.reload?.()
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
  const res = await getProfitCenterTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importProfitCenter(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    profitCenterChangeLogPanelRef.value?.reload?.()
      }
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
    const exportMeta = await exportProfitCenter(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[ProfitCenter] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ProfitCenterRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteProfitCenterById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteProfitCenterBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleProfitCenterStatusChange(record: ProfitCenterRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toProfitCenterNumber(getProfitCenterDictValue(record, 'profitCenterStatus'))
  const id = getProfitCenterId(record)
  const row = dataSource.value.find((item) => getProfitCenterId(item) === id)
  if (row) {
    row.profitCenterStatus = newVal
  }
  try {
    await updateProfitCenterStatus({ profitCenterId: id, profitCenterStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.profitCenterStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  profitCenterCode: '',
  profitCenterName: '',
  parentId: '',
  managerId: '',
  managerName: '',
  deptId: '',
  deptName: '',
  profitCenterLevel: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  relatedPlant: '',
  profitCenterStatus: undefined as number | undefined,
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
