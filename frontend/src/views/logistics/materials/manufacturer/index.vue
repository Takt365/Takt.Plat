<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/manufacturer -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt制造商实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-manufacturer">
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
      create-permission="logistics:materials:manufacturer:create"
      update-permission="logistics:materials:manufacturer:update"
      delete-permission="logistics:materials:manufacturer:delete"
      import-permission="logistics:materials:manufacturer:import"
      export-permission="logistics:materials:manufacturer:export"
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
      :id-column-key="'manufacturerId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getManufacturerId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'manufacturerStatus'">
          <TaktDictTag
            :value="getManufacturerField(record, 'manufacturerStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.manufacturerMaterial._self') }}</div>
          <a-table
            v-if="hasManufacturerMaterialRows(record)"
            :columns="manufacturerMaterialExpandColumns"
            :data-source="getManufacturerMaterialRows(record)"
            :row-key="(row: ManufacturerMaterial, index?: number) => row?.manufacturerMaterialId || String(index ?? 0)"
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
      <ManufacturerForm
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
      :storage-key="'takt-query-fields-logistics-materials-manufacturer'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('manufacturerCode')">
      <a-form-item :label="t('entity.manufacturer.code')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerName')">
      <a-form-item :label="t('entity.manufacturer.name')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerShortName')">
      <a-form-item :label="t('entity.manufacturer.shortname')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerShortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.shortname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerType')">
      <a-form-item :label="t('entity.manufacturer.type')">
        <a-input-number
          v-model:value="advancedQueryForm.manufacturerType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.manufacturer.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.industrysector') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerTaxNumber')">
      <a-form-item :label="t('entity.manufacturer.taxnumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerTaxNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.taxnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCountry')">
      <a-form-item :label="t('entity.manufacturer.registrationcountry')">
        <a-input
          v-model:value="advancedQueryForm.registrationCountry"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.registrationcountry') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress1')">
      <a-form-item :label="t('entity.manufacturer.registrationaddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress1') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress2')">
      <a-form-item :label="t('entity.manufacturer.registrationaddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress2"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress2') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress3')">
      <a-form-item :label="t('entity.manufacturer.registrationaddress3')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress3"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.manufacturer.registrationaddress3') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPhone')">
      <a-form-item :label="t('entity.manufacturer.phone')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.phone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerFax')">
      <a-form-item :label="t('entity.manufacturer.fax')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerFax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.fax') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerEmail')">
      <a-form-item :label="t('entity.manufacturer.email')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.email') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerWebsite')">
      <a-form-item :label="t('entity.manufacturer.website')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerWebsite"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.website') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPerson')">
      <a-form-item :label="t('entity.manufacturer.contactperson')">
        <a-input
          v-model:value="advancedQueryForm.contactPerson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactperson') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="t('entity.manufacturer.contactphone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactphone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="t('entity.manufacturer.contactemail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.contactemail') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerLevel')">
      <a-form-item :label="t('entity.manufacturer.level')">
        <a-input-number
          v-model:value="advancedQueryForm.manufacturerLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityCertification')">
      <a-form-item :label="t('entity.manufacturer.qualitycertification')">
        <a-input-number
          v-model:value="advancedQueryForm.qualityCertification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.qualitycertification') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationScore')">
      <a-form-item :label="t('entity.manufacturer.evaluationscore')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.evaluationscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQualified')">
      <a-form-item :label="t('entity.manufacturer.isqualified')">
        <a-input-number
          v-model:value="advancedQueryForm.isQualified"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.isqualified') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerStatus')">
      <a-form-item :label="t('entity.manufacturer.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturerStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.manufacturer.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.manufacturer.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.manufacturer.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.manufacturer._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.manufacturer._self"
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
      :id-column-key="'manufacturerId'"
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
 * Takt制造商实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/manufacturer
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ManufacturerForm from './components/manufacturer-form.vue'
import { getManufacturerList, getManufacturerById, createManufacturer, updateManufacturer, deleteManufacturerById, deleteManufacturerBatch, getManufacturerTemplate, importManufacturer, exportManufacturer } from '@/api/logistics/materials/manufacturer'
import * as manufacturerMaterialApi from '@/api/logistics/materials/manufacturer-material'
import type { ManufacturerMaterial, ManufacturerMaterialQuery } from '@/types/logistics/materials/manufacturer-material'
import type { Manufacturer, ManufacturerQuery, ManufacturerCreate, ManufacturerUpdate } from '@/types/logistics/materials/manufacturer'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktManufacturer')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.manufacturer._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Manufacturer[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Manufacturer | null>(null)
/** 表格多选行 */
const selectedRows = ref<Manufacturer[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Manufacturer>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  manufacturerCode: '',
  manufacturerName: '',
  manufacturerShortName: '',
  manufacturerType: undefined as number | undefined,
  industrySector: '',
  manufacturerTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  manufacturerPhone: '',
  manufacturerFax: '',
  manufacturerEmail: '',
  manufacturerWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  manufacturerLevel: undefined as number | undefined,
  qualityCertification: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  manufacturerStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'manufacturerCode', label: t('entity.manufacturer.code') },
  { key: 'manufacturerName', label: t('entity.manufacturer.name') },
  { key: 'manufacturerShortName', label: t('entity.manufacturer.shortname') },
  { key: 'manufacturerType', label: t('entity.manufacturer.type') },
  { key: 'industrySector', label: t('entity.manufacturer.industrysector') },
  { key: 'manufacturerTaxNumber', label: t('entity.manufacturer.taxnumber') },
  { key: 'registrationCountry', label: t('entity.manufacturer.registrationcountry') },
  { key: 'registrationAddress1', label: t('entity.manufacturer.registrationaddress1') },
  { key: 'registrationAddress2', label: t('entity.manufacturer.registrationaddress2') },
  { key: 'registrationAddress3', label: t('entity.manufacturer.registrationaddress3') },
  { key: 'manufacturerPhone', label: t('entity.manufacturer.phone') },
  { key: 'manufacturerFax', label: t('entity.manufacturer.fax') },
  { key: 'manufacturerEmail', label: t('entity.manufacturer.email') },
  { key: 'manufacturerWebsite', label: t('entity.manufacturer.website') },
  { key: 'contactPerson', label: t('entity.manufacturer.contactperson') },
  { key: 'contactPhone', label: t('entity.manufacturer.contactphone') },
  { key: 'contactEmail', label: t('entity.manufacturer.contactemail') },
  { key: 'manufacturerLevel', label: t('entity.manufacturer.level') },
  { key: 'qualityCertification', label: t('entity.manufacturer.qualitycertification') },
  { key: 'evaluationScore', label: t('entity.manufacturer.evaluationscore') },
  { key: 'isQualified', label: t('entity.manufacturer.isqualified') },
  { key: 'manufacturerStatus', label: t('entity.manufacturer.status') },
  { key: 'sortOrder', label: t('entity.manufacturer.sortorder') },
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
const entityIdName = 'manufacturerId'
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

/** 展开行预览：manufacturerMaterial 列 */
const manufacturerMaterialExpandColumns = computed(() => [
  {
    title: t('entity.manufacturerMaterial.manufacturername'),
    dataIndex: 'manufacturerName',
    key: 'manufacturerName',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.manufacturercode'),
    dataIndex: 'manufacturerCode',
    key: 'manufacturerCode',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.materialtype'),
    dataIndex: 'materialType',
    key: 'materialType',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.code'),
    dataIndex: 'manufacturerMaterialCode',
    key: 'manufacturerMaterialCode',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.name'),
    dataIndex: 'manufacturerMaterialName',
    key: 'manufacturerMaterialName',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.specification'),
    dataIndex: 'manufacturerMaterialSpecification',
    key: 'manufacturerMaterialSpecification',
    ellipsis: true,
  },
  {
    title: t('entity.manufacturerMaterial.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
])

/** 读取主表行上的 manufacturerMaterial 子表缓存 */
function getManufacturerMaterialRows(record: Manufacturer): ManufacturerMaterial[] {
  return (record as any)?.manufacturerMaterials ?? []
}

/** 主表行是否已加载 manufacturerMaterial 子表 */
function hasManufacturerMaterialRows(record: Manufacturer): boolean {
  return getManufacturerMaterialRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadManufacturerDetail(record: Manufacturer): Promise<Manufacturer | null> {
  const id = getManufacturerId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getManufacturerById(id)
    const index = dataSource.value.findIndex((row) => getManufacturerId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Manufacturer
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 manufacturerMaterial 子表（ManufacturerMaterialQuery + manufacturerMaterialApi，与主表 ManufacturerQuery 分离） */
async function loadManufacturerMaterialForManufacturer(record: Manufacturer): Promise<ManufacturerMaterial[]> {
  const masterId = getManufacturerId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ManufacturerMaterialQuery = {
      pageIndex: 1,
      pageSize: 500,
      manufacturerId: masterId,
    }
    const result = await manufacturerMaterialApi.getManufacturerMaterialList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getManufacturerId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, manufacturerMaterials: rows } as Manufacturer
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureManufacturerChildrenLoaded(record: Manufacturer) {
  if (!hasManufacturerMaterialRows(record)) {
    await loadManufacturerMaterialForManufacturer(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Manufacturer) {
  const key = getManufacturerId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureManufacturerChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'manufacturerId',
    key: 'manufacturerId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerId') ?? ''
  },
  {
    title: t('entity.manufacturer.code'),
    dataIndex: 'manufacturerCode',
    key: 'manufacturerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerCode') ?? ''
  },
  {
    title: t('entity.manufacturer.name'),
    dataIndex: 'manufacturerName',
    key: 'manufacturerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerName') ?? ''
  },
  {
    title: t('entity.manufacturer.shortname'),
    dataIndex: 'manufacturerShortName',
    key: 'manufacturerShortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerShortName') ?? ''
  },
  {
    title: t('entity.manufacturer.type'),
    dataIndex: 'manufacturerType',
    key: 'manufacturerType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerType') ?? ''
  },
  {
    title: t('entity.manufacturer.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.manufacturer.taxnumber'),
    dataIndex: 'manufacturerTaxNumber',
    key: 'manufacturerTaxNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerTaxNumber') ?? ''
  },
  {
    title: t('entity.manufacturer.registrationcountry'),
    dataIndex: 'registrationCountry',
    key: 'registrationCountry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'registrationCountry') ?? ''
  },
  {
    title: t('entity.manufacturer.registrationaddress1'),
    dataIndex: 'registrationAddress1',
    key: 'registrationAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'registrationAddress1') ?? ''
  },
  {
    title: t('entity.manufacturer.registrationaddress2'),
    dataIndex: 'registrationAddress2',
    key: 'registrationAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'registrationAddress2') ?? ''
  },
  {
    title: t('entity.manufacturer.registrationaddress3'),
    dataIndex: 'registrationAddress3',
    key: 'registrationAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'registrationAddress3') ?? ''
  },
  {
    title: t('entity.manufacturer.phone'),
    dataIndex: 'manufacturerPhone',
    key: 'manufacturerPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerPhone') ?? ''
  },
  {
    title: t('entity.manufacturer.fax'),
    dataIndex: 'manufacturerFax',
    key: 'manufacturerFax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerFax') ?? ''
  },
  {
    title: t('entity.manufacturer.email'),
    dataIndex: 'manufacturerEmail',
    key: 'manufacturerEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerEmail') ?? ''
  },
  {
    title: t('entity.manufacturer.website'),
    dataIndex: 'manufacturerWebsite',
    key: 'manufacturerWebsite',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerWebsite') ?? ''
  },
  {
    title: t('entity.manufacturer.contactperson'),
    dataIndex: 'contactPerson',
    key: 'contactPerson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'contactPerson') ?? ''
  },
  {
    title: t('entity.manufacturer.contactphone'),
    dataIndex: 'contactPhone',
    key: 'contactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'contactPhone') ?? ''
  },
  {
    title: t('entity.manufacturer.contactemail'),
    dataIndex: 'contactEmail',
    key: 'contactEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'contactEmail') ?? ''
  },
  {
    title: t('entity.manufacturer.level'),
    dataIndex: 'manufacturerLevel',
    key: 'manufacturerLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'manufacturerLevel') ?? ''
  },
  {
    title: t('entity.manufacturer.qualitycertification'),
    dataIndex: 'qualityCertification',
    key: 'qualityCertification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'qualityCertification') ?? ''
  },
  {
    title: t('entity.manufacturer.evaluationscore'),
    dataIndex: 'evaluationScore',
    key: 'evaluationScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'evaluationScore') ?? ''
  },
  {
    title: t('entity.manufacturer.isqualified'),
    dataIndex: 'isQualified',
    key: 'isQualified',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getManufacturerField(record, 'isQualified') ?? ''
  },
  {
    title: t('entity.manufacturer.status'),
    dataIndex: 'manufacturerStatus',
    key: 'manufacturerStatus',
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
        permission: 'logistics:materials:manufacturer:update',
        onClick: (record: Manufacturer) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:manufacturer:delete',
        onClick: (record: Manufacturer) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getManufacturerId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getManufacturerField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Manufacturer[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Manufacturer, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getManufacturerId(selectedRow.value) === getManufacturerId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Manufacturer[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Manufacturer) => ({
  onClick: () => {
    const key = getManufacturerId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getManufacturerId(item)))
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
    const params: ManufacturerQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getManufacturerList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Manufacturer] 加载数据失败', { error })
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
  manufacturerCode: '',
  manufacturerName: '',
  manufacturerShortName: '',
  manufacturerType: undefined as number | undefined,
  industrySector: '',
  manufacturerTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  manufacturerPhone: '',
  manufacturerFax: '',
  manufacturerEmail: '',
  manufacturerWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  manufacturerLevel: undefined as number | undefined,
  qualityCertification: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  manufacturerStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.manufacturer._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Manufacturer) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.manufacturer._self') })
  formLoading.value = true
  try {
    const detail = await loadManufacturerDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.manufacturer._self') }))
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
      await updateManufacturer(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.manufacturer._self') }))
    } else {
      await createManufacturer(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.manufacturer._self') }))
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
  const res = await getManufacturerTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importManufacturer(file, sheetName)
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
    const exportQuery: ManufacturerQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportManufacturer(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.manufacturer._self') }))
  } catch (error: any) {
    logger.error('[Manufacturer] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.manufacturer._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Manufacturer) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.manufacturer._self'), name: t('common.tip.this.target', { target: t('entity.manufacturer._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteManufacturerById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.manufacturer._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.manufacturer._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.manufacturer._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteManufacturerBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.manufacturer._self') }))
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
  manufacturerCode: '',
  manufacturerName: '',
  manufacturerShortName: '',
  manufacturerType: undefined as number | undefined,
  industrySector: '',
  manufacturerTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  manufacturerPhone: '',
  manufacturerFax: '',
  manufacturerEmail: '',
  manufacturerWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  manufacturerLevel: undefined as number | undefined,
  qualityCertification: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  manufacturerStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
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
.logistics-materials-manufacturer {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
