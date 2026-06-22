<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt全局物料实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:materials:material:change:log:create"
      update-permission="logistics:materials:material:change:log:update"
      delete-permission="logistics:materials:material:change:log:delete"
      import-permission="logistics:materials:material:change:log:import"
      export-permission="logistics:materials:material:change:log:export"
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
      :master-row-key="getMaterialId"
      :master-row-selection="rowSelection"
      master-id-column-key="materialId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="tenant"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'materialStatus'">
          <a-switch
            :checked="getMaterialField(record, 'materialStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleMaterialStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <MaterialChangeLogPanel
          ref="materialChangeLogPanelRef"
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
      <MaterialForm
        :key="formData?.materialId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-materials-material-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.material.code')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.code') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.material.name')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.name') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.material.specification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specification') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.material.description')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.material.description') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.material.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.industrysector') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialHierarchy')">
      <a-form-item :label="t('entity.material.hierarchy')">
        <a-input
          v-model:value="advancedQueryForm.materialHierarchy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.hierarchy') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroupCode')">
      <a-form-item :label="t('entity.material.groupcode')">
        <a-input
          v-model:value="advancedQueryForm.materialGroupCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.groupcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="t('entity.material.type')">
        <a-input-number
          v-model:value="advancedQueryForm.materialType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialModel')">
      <a-form-item :label="t('entity.material.model')">
        <a-input
          v-model:value="advancedQueryForm.materialModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.model') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialBrand')">
      <a-form-item :label="t('entity.material.brand')">
        <a-input
          v-model:value="advancedQueryForm.materialBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.brand') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="t('entity.material.baseunit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.baseunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="t('entity.material.manufacturer')">
        <a-input
          v-model:value="advancedQueryForm.manufacturer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturer') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartNumber')">
      <a-form-item :label="t('entity.material.manufacturerpartnumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturerpartnumber') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialStatus')">
      <a-form-item :label="t('entity.material.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialAttributes')">
      <a-form-item :label="t('entity.material.attributes')">
        <a-input
          v-model:value="advancedQueryForm.materialAttributes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.attributes') })"
          show-count
          :maxlength="4000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLife')">
      <a-form-item :label="t('entity.material.isendoflife')">
        <a-input
          v-model:value="advancedQueryForm.isEndOfLife"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isendoflife') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateStart')">
      <a-form-item :label="t('entity.material.endoflifedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.endoflifedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateEnd')">
      <a-form-item :label="t('entity.material.endoflifedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.endoflifedateend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.material._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.material._self"
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
      :id-column-key="'materialId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MaterialForm from './components/material-form.vue'
import MaterialChangeLogPanel from './components/material-change-log-panel.vue'
import { provideMaterialMasterContext } from './composables/use-material-master-context'
import { getMaterialList, getMaterialById, createMaterial, updateMaterial, deleteMaterialById, deleteMaterialBatch, getMaterialTemplate, importMaterial, exportMaterial, updateMaterialStatus } from '@/api/logistics/materials/material'
import type { Material, MaterialQuery } from '@/types/logistics/materials/material'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.material._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Material[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Material | null>(null)
/** 表格多选行 */
const selectedRows = ref<Material[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Material> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  manufacturer: '',
  manufacturerPartNumber: '',
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'materialCode', label: t('entity.material.code') },
  { key: 'materialName', label: t('entity.material.name') },
  { key: 'materialSpecification', label: t('entity.material.specification') },
  { key: 'materialDescription', label: t('entity.material.description') },
  { key: 'industrySector', label: t('entity.material.industrysector') },
  { key: 'materialHierarchy', label: t('entity.material.hierarchy') },
  { key: 'materialGroupCode', label: t('entity.material.groupcode') },
  { key: 'materialType', label: t('entity.material.type') },
  { key: 'materialModel', label: t('entity.material.model') },
  { key: 'materialBrand', label: t('entity.material.brand') },
  { key: 'baseUnit', label: t('entity.material.baseunit') },
  { key: 'manufacturer', label: t('entity.material.manufacturer') },
  { key: 'manufacturerPartNumber', label: t('entity.material.manufacturerpartnumber') },
  { key: 'materialStatus', label: t('entity.material.status') },
  { key: 'materialAttributes', label: t('entity.material.attributes') },
  { key: 'isEndOfLife', label: t('entity.material.isendoflife') },
  { key: 'endOfLifeDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.material.endoflifedate')) },
  { key: 'endOfLifeDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.material.endoflifedate')) },
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
const entityIdName = 'materialId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMaterialMasterContext()
const materialChangeLogPanelRef = ref<InstanceType<typeof MaterialChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialQuery>): MaterialQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialName', form.materialName)
  assignTrimmed('materialSpecification', form.materialSpecification)
  assignTrimmed('materialDescription', form.materialDescription)
  assignTrimmed('industrySector', form.industrySector)
  assignTrimmed('materialHierarchy', form.materialHierarchy)
  assignTrimmed('materialGroupCode', form.materialGroupCode)
  if (form.materialType !== undefined && form.materialType !== null) {
    query.materialType = form.materialType
  }
  assignTrimmed('materialModel', form.materialModel)
  assignTrimmed('materialBrand', form.materialBrand)
  assignTrimmed('baseUnit', form.baseUnit)
  assignTrimmed('manufacturer', form.manufacturer)
  assignTrimmed('manufacturerPartNumber', form.manufacturerPartNumber)
  if (form.materialStatus !== undefined && form.materialStatus !== null) {
    query.materialStatus = form.materialStatus
  }
  assignTrimmed('materialAttributes', form.materialAttributes)
  assignTrimmed('isEndOfLife', form.isEndOfLife)
  assignTrimmed('endOfLifeDateStart', form.endOfLifeDateStart)
  assignTrimmed('endOfLifeDateEnd', form.endOfLifeDateEnd)
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
function syncMasterSelection(record: Material | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaterialId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as Material
  const key = getMaterialId(row)
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
async function loadMaterialDetail(record: Material): Promise<Material | null> {
  const id = getMaterialId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMaterialById(id)
    const index = dataSource.value.findIndex((row) => getMaterialId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Material
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
    dataIndex: 'materialId',
    key: 'materialId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialId') ?? ''
  },
  {
    title: t('entity.material.code'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.material.name'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialName') ?? ''
  },
  {
    title: t('entity.material.specification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialSpecification') ?? ''
  },
  {
    title: t('entity.material.description'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialDescription') ?? ''
  },
  {
    title: t('entity.material.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.material.hierarchy'),
    dataIndex: 'materialHierarchy',
    key: 'materialHierarchy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialHierarchy') ?? ''
  },
  {
    title: t('entity.material.groupcode'),
    dataIndex: 'materialGroupCode',
    key: 'materialGroupCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialGroupCode') ?? ''
  },
  {
    title: t('entity.material.type'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialType') ?? ''
  },
  {
    title: t('entity.material.model'),
    dataIndex: 'materialModel',
    key: 'materialModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialModel') ?? ''
  },
  {
    title: t('entity.material.brand'),
    dataIndex: 'materialBrand',
    key: 'materialBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialBrand') ?? ''
  },
  {
    title: t('entity.material.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'baseUnit') ?? ''
  },
  {
    title: t('entity.material.manufacturer'),
    dataIndex: 'manufacturer',
    key: 'manufacturer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturer') ?? ''
  },
  {
    title: t('entity.material.manufacturerpartnumber'),
    dataIndex: 'manufacturerPartNumber',
    key: 'manufacturerPartNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturerPartNumber') ?? ''
  },
  {
    title: t('entity.material.status'),
    dataIndex: 'materialStatus',
    key: 'materialStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.material.attributes'),
    dataIndex: 'materialAttributes',
    key: 'materialAttributes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialAttributes') ?? ''
  },
  {
    title: t('entity.material.isendoflife'),
    dataIndex: 'isEndOfLife',
    key: 'isEndOfLife',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'isEndOfLife') ?? ''
  },
  {
    title: t('entity.material.endoflifedate'),
    dataIndex: 'endOfLifeDate',
    key: 'endOfLifeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'endOfLifeDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:change:log:update',
        onClick: (record: Material) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:change:log:delete',
        onClick: (record: Material) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Material[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: Material, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getMaterialId(selectedRow.value) === getMaterialId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Material[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaterialList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Material] 加载数据失败', { error })
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  manufacturer: '',
  manufacturerPartNumber: '',
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.material._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Material) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.material._self') })
  formLoading.value = true
  try {
    const detail = await loadMaterialDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.material._self') }))
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
      await updateMaterial(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.material._self') }))
    } else {
      await createMaterial(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.material._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  materialChangeLogPanelRef.value?.reload?.()
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
  const res = await getMaterialTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaterial(file, sheetName)
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
    const exportMeta = await exportMaterial(
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
    message.success(t('common.feedback.export.success', { target: t('entity.material._self') }))
  } catch (error: any) {
    logger.error('[Material] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.material._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Material) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.material._self'), name: t('common.tip.this.target', { target: t('entity.material._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.material._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.material._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.material._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.material._self') }))
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
async function handleMaterialStatusChange(record: Material, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getMaterialField(record, 'materialStatus')
  const id = getMaterialId(record)
  const row = dataSource.value.find((item) => getMaterialId(item) === id)
  if (row) {
    row.materialStatus = newVal
  }
  try {
    await updateMaterialStatus({ materialId: id, materialStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.materialStatus = oldVal
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  manufacturer: '',
  manufacturerPartNumber: '',
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
