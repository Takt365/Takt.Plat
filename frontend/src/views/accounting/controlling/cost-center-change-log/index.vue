<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/cost-center-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本中心实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:controlling:cost:center:create"
      update-permission="accounting:controlling:cost:center:update"
      delete-permission="accounting:controlling:cost:center:delete"
      import-permission="accounting:controlling:cost:center:import"
      export-permission="accounting:controlling:cost:center:export"
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
      :master-row-key="getCostCenterId"
      :master-row-selection="rowSelection"
      master-id-column-key="costCenterId"
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
        <template v-if="column.key === 'costCenterStatus'">
          <a-switch
            :checked="getCostCenterDictValue(record, 'costCenterStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleCostCenterStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <CostCenterChangeLogPanel
          ref="costCenterChangeLogPanelRef"
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
      <CostCenterForm
        :key="formData?.costCenterId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-cost-center-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="pi.queryLabel('costCenterCode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="pi.queryPh('costCenterCode', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterName')">
      <a-form-item :label="pi.queryLabel('costCenterName')">
        <a-input
          v-model:value="advancedQueryForm.costCenterName"
          :placeholder="pi.queryPh('costCenterName', 'required')"
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
      <div v-show="isFieldVisible('costCenterType')">
      <a-form-item :label="pi.queryLabel('costCenterType')">
        <a-input-number
          v-model:value="advancedQueryForm.costCenterType"
          :placeholder="pi.queryPh('costCenterType', 'required')"
          style="width: 100%"
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
      <div v-show="isFieldVisible('costCenterLevel')">
      <a-form-item :label="pi.queryLabel('costCenterLevel')">
        <a-input-number
          v-model:value="advancedQueryForm.costCenterLevel"
          :placeholder="pi.queryPh('costCenterLevel', 'required')"
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
      <div v-show="isFieldVisible('costCenterStatus')">
      <a-form-item :label="pi.queryLabel('costCenterStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.costCenterStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="pi.queryPh('costCenterStatus', 'select')"
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
        :entity-i18n-key="COSTCENTER_SELF_I18N_KEY"
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
      :id-column-key="'costCenterId'"
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
 * 成本中心实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/cost-center-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CostCenterForm from './components/cost-center-form.vue'
import CostCenterChangeLogPanel from './components/cost-center-change-log-panel.vue'
import { provideCostCenterMasterContext, type CostCenterRowRecord } from './composables/use-cost-center-master-context'
import { getCostCenterList, getCostCenterById, createCostCenter, updateCostCenter, deleteCostCenterById, deleteCostCenterBatch, getCostCenterTemplate, importCostCenter, exportCostCenter, updateCostCenterStatus } from '@/api/accounting/controlling/cost-center'
import type { CostCenter, CostCenterQuery } from '@/types/accounting/controlling/cost-center'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useCostCenterI18n,
  COSTCENTER_LIST_FIELDS,
  COSTCENTER_QUERY_STRING_FIELDS,
  COSTCENTER_QUERY_FIELDS,
  COSTCENTER_SELF_I18N_KEY,
} from './composables/use-cost-center-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCostCenterI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCostCenter')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CostCenter[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CostCenterRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<CostCenterRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CostCenter> | null>(null)
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
  const form = Object.fromEntries(COSTCENTER_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof COSTCENTER_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    costCenterType: undefined as number | undefined,
    costCenterLevel: undefined as number | undefined,
    costCenterStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  COSTCENTER_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'costCenterId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCostCenterMasterContext()
const costCenterChangeLogPanelRef = ref<InstanceType<typeof CostCenterChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CostCenterQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CostCenterQuery>): CostCenterQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CostCenterQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CostCenterQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of COSTCENTER_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.costCenterType !== undefined && form.costCenterType !== null) {
    query.costCenterType = form.costCenterType
  }
  if (form.costCenterLevel !== undefined && form.costCenterLevel !== null) {
    query.costCenterLevel = form.costCenterLevel
  }
  if (form.costCenterStatus !== undefined && form.costCenterStatus !== null) {
    query.costCenterStatus = form.costCenterStatus
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
function syncMasterSelection(record: CostCenterRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCostCenterId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CostCenterRowRecord
  const key = getCostCenterId(row)
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
async function loadCostCenterDetail(record: CostCenterRowRecord): Promise<CostCenter | null> {
  const id = getCostCenterId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCostCenterById(id)
    const index = dataSource.value.findIndex((row) => getCostCenterId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as CostCenter
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
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'costCenterId') ?? ''
  },
  {
    title: pi.label('costCenterCode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'costCenterCode') ?? ''
  },
  {
    title: pi.label('costCenterName'),
    dataIndex: 'costCenterName',
    key: 'costCenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'costCenterName') ?? ''
  },
  {
    title: pi.label('parentId'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('costCenterType'),
    dataIndex: 'costCenterType',
    key: 'costCenterType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'costCenterType') ?? ''
  },
  {
    title: pi.label('managerId'),
    dataIndex: 'managerId',
    key: 'managerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'managerId') ?? ''
  },
  {
    title: pi.label('managerName'),
    dataIndex: 'managerName',
    key: 'managerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'managerName') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('costCenterLevel'),
    dataIndex: 'costCenterLevel',
    key: 'costCenterLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'costCenterLevel') ?? ''
  },
  {
    title: pi.label('validFrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'validFrom') ?? ''
  },
  {
    title: pi.label('validTo'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'validTo') ?? ''
  },
  {
    title: pi.label('relatedPlant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostCenterField(record, 'relatedPlant') ?? ''
  },
  {
    title: pi.label('costCenterStatus'),
    dataIndex: 'costCenterStatus',
    key: 'costCenterStatus',
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
        permission: 'accounting:controlling:cost:center:update',
        onClick: (record: CostCenterRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:cost:center:delete',
        onClick: (record: CostCenterRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCostCenterId = (record: CostCenterRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCostCenterField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getCostCenterDictValue = (
  record: CostCenterRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toCostCenterNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CostCenterRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CostCenterRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCostCenterId(selectedRow.value) === getCostCenterId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CostCenterRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getCostCenterList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CostCenter] 加载数据失败', { error })
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
  costCenterCode: '',
  costCenterName: '',
  parentId: '',
  costCenterType: undefined as number | undefined,
  managerId: '',
  managerName: '',
  deptId: '',
  deptName: '',
  costCenterLevel: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  relatedPlant: '',
  costCenterStatus: undefined as number | undefined,
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
async function handleEdit(record: CostCenterRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadCostCenterDetail(record)
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
      await updateCostCenter(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCostCenter(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  costCenterChangeLogPanelRef.value?.reload?.()
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
  const res = await getCostCenterTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCostCenter(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    costCenterChangeLogPanelRef.value?.reload?.()
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
    const exportMeta = await exportCostCenter(
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
    logger.error('[CostCenter] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CostCenterRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCostCenterById((record as any)[entityIdName])
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
      await deleteCostCenterBatch(ids)
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
async function handleCostCenterStatusChange(record: CostCenterRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toCostCenterNumber(getCostCenterDictValue(record, 'costCenterStatus'))
  const id = getCostCenterId(record)
  const row = dataSource.value.find((item) => getCostCenterId(item) === id)
  if (row) {
    row.costCenterStatus = newVal
  }
  try {
    await updateCostCenterStatus({ costCenterId: id, costCenterStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.costCenterStatus = oldVal
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
  costCenterCode: '',
  costCenterName: '',
  parentId: '',
  costCenterType: undefined as number | undefined,
  managerId: '',
  managerName: '',
  deptId: '',
  deptName: '',
  costCenterLevel: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  relatedPlant: '',
  costCenterStatus: undefined as number | undefined,
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
