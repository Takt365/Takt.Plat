<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-detail -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:engineering:change:ec:detail:create"
      update-permission="logistics:manufacturing:engineering:change:ec:detail:update"
      delete-permission="logistics:manufacturing:engineering:change:ec:detail:delete"
      import-permission="logistics:manufacturing:engineering:change:ec:detail:import"
      export-permission="logistics:manufacturing:engineering:change:ec:detail:export"
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
      :master-row-key="getEcDetailId"
      :master-row-selection="rowSelection"
      master-id-column-key="ecDetailId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <EcDeptPanel
          ref="ecDeptPanelRef"
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
      <EcDetailForm
        :key="formData?.ecDetailId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-ec-detail'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecId')">
      <a-form-item :label="t('entity.ecdetail.ecid')">
        <a-input
          v-model:value="advancedQueryForm.ecId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ecdetail.ecno')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ecdetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecModel')">
      <a-form-item :label="t('entity.ecdetail.ecmodel')">
        <a-input
          v-model:value="advancedQueryForm.ecModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomItem')">
      <a-form-item :label="t('entity.ecdetail.ecbomitem')">
        <a-input
          v-model:value="advancedQueryForm.ecBomItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomitem') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomSubItem')">
      <a-form-item :label="t('entity.ecdetail.ecbomsubitem')">
        <a-input
          v-model:value="advancedQueryForm.ecBomSubItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomsubitem') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomNo')">
      <a-form-item :label="t('entity.ecdetail.ecbomno')">
        <a-input
          v-model:value="advancedQueryForm.ecBomNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomno') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecChange')">
      <a-form-item :label="t('entity.ecdetail.ecchange')">
        <a-input
          v-model:value="advancedQueryForm.ecChange"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecchange') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLocal')">
      <a-form-item :label="t('entity.ecdetail.eclocal')">
        <a-input
          v-model:value="advancedQueryForm.ecLocal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.eclocal') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNote')">
      <a-form-item :label="t('entity.ecdetail.ecnote')">
        <a-textarea
          v-model:value="advancedQueryForm.ecNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecProcess')">
      <a-form-item :label="t('entity.ecdetail.ecprocess')">
        <a-input
          v-model:value="advancedQueryForm.ecProcess"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecprocess') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateStart')">
      <a-form-item :label="t('entity.ecdetail.ecbomdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateEnd')">
      <a-form-item :label="t('entity.ecdetail.ecbomdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateStart')">
      <a-form-item :label="t('entity.ecdetail.ecentrydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateEnd')">
      <a-form-item :label="t('entity.ecdetail.ecentrydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldItem')">
      <a-form-item :label="t('entity.ecdetail.ecolditem')">
        <a-input
          v-model:value="advancedQueryForm.ecOldItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecolditem') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldText')">
      <a-form-item :label="t('entity.ecdetail.ecoldtext')">
        <a-input
          v-model:value="advancedQueryForm.ecOldText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldtext') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldQty')">
      <a-form-item :label="t('entity.ecdetail.ecoldqty')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldSet')">
      <a-form-item :label="t('entity.ecdetail.ecoldset')">
        <a-input
          v-model:value="advancedQueryForm.ecOldSet"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldset') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewItem')">
      <a-form-item :label="t('entity.ecdetail.ecnewitem')">
        <a-input
          v-model:value="advancedQueryForm.ecNewItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewitem') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewText')">
      <a-form-item :label="t('entity.ecdetail.ecnewtext')">
        <a-input
          v-model:value="advancedQueryForm.ecNewText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewtext') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewQty')">
      <a-form-item :label="t('entity.ecdetail.ecnewqty')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewSet')">
      <a-form-item :label="t('entity.ecdetail.ecnewset')">
        <a-input
          v-model:value="advancedQueryForm.ecNewSet"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewset') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isProcurement')">
      <a-form-item :label="t('entity.ecdetail.isprocurement')">
        <a-input-number
          v-model:value="advancedQueryForm.isProcurement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isprocurement') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCheck')">
      <a-form-item :label="t('entity.ecdetail.ischeck')">
        <a-input-number
          v-model:value="advancedQueryForm.isCheck"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ischeck') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecWarehouse')">
      <a-form-item :label="t('entity.ecdetail.ecwarehouse')">
        <a-input
          v-model:value="advancedQueryForm.ecWarehouse"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecwarehouse') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLine')">
      <a-form-item :label="t('entity.ecdetail.isendofline')">
        <a-input-number
          v-model:value="advancedQueryForm.isEndOfLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isendofline') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ecdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecdetail._self"
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
      :id-column-key="'ecDetailId'"
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
 * 设变管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-detail
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import EcDetailForm from './components/ec-detail-form.vue'
import EcDeptPanel from './components/ec-dept-panel.vue'
import { provideEcDetailMasterContext } from './composables/use-ec-detail-master-context'
import { getEcDetailList, getEcDetailById, createEcDetail, updateEcDetail, deleteEcDetailById, deleteEcDetailBatch, getEcDetailTemplate, importEcDetail, exportEcDetail } from '@/api/logistics/manufacturing/engineering-change/ec-detail'
import type { EcDetail, EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcDetail')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ecdetail._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EcDetail[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EcDetail | null>(null)
/** 表格多选行 */
const selectedRows = ref<EcDetail[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EcDetail> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  ecId: '',
  ecNo: '',
  lineNumber: undefined as number | undefined,
  ecModel: '',
  ecBomItem: '',
  ecBomSubItem: '',
  ecBomNo: '',
  ecChange: '',
  ecLocal: '',
  ecNote: '',
  ecProcess: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldQty: undefined as number | undefined,
  ecOldSet: '',
  ecNewItem: '',
  ecNewText: '',
  ecNewQty: undefined as number | undefined,
  ecNewSet: '',
  isProcurement: undefined as number | undefined,
  isCheck: undefined as number | undefined,
  ecWarehouse: '',
  isEndOfLine: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'ecId', label: t('entity.ecdetail.ecid') },
  { key: 'ecNo', label: t('entity.ecdetail.ecno') },
  { key: 'lineNumber', label: t('entity.ecdetail.linenumber') },
  { key: 'ecModel', label: t('entity.ecdetail.ecmodel') },
  { key: 'ecBomItem', label: t('entity.ecdetail.ecbomitem') },
  { key: 'ecBomSubItem', label: t('entity.ecdetail.ecbomsubitem') },
  { key: 'ecBomNo', label: t('entity.ecdetail.ecbomno') },
  { key: 'ecChange', label: t('entity.ecdetail.ecchange') },
  { key: 'ecLocal', label: t('entity.ecdetail.eclocal') },
  { key: 'ecNote', label: t('entity.ecdetail.ecnote') },
  { key: 'ecProcess', label: t('entity.ecdetail.ecprocess') },
  { key: 'ecBomDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
  { key: 'ecBomDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
  { key: 'ecEntryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecentrydate')) },
  { key: 'ecEntryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecentrydate')) },
  { key: 'ecOldItem', label: t('entity.ecdetail.ecolditem') },
  { key: 'ecOldText', label: t('entity.ecdetail.ecoldtext') },
  { key: 'ecOldQty', label: t('entity.ecdetail.ecoldqty') },
  { key: 'ecOldSet', label: t('entity.ecdetail.ecoldset') },
  { key: 'ecNewItem', label: t('entity.ecdetail.ecnewitem') },
  { key: 'ecNewText', label: t('entity.ecdetail.ecnewtext') },
  { key: 'ecNewQty', label: t('entity.ecdetail.ecnewqty') },
  { key: 'ecNewSet', label: t('entity.ecdetail.ecnewset') },
  { key: 'isProcurement', label: t('entity.ecdetail.isprocurement') },
  { key: 'isCheck', label: t('entity.ecdetail.ischeck') },
  { key: 'ecWarehouse', label: t('entity.ecdetail.ecwarehouse') },
  { key: 'isEndOfLine', label: t('entity.ecdetail.isendofline') },
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
const entityIdName = 'ecDetailId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideEcDetailMasterContext()
const ecDeptPanelRef = ref<InstanceType<typeof EcDeptPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcDetailQuery>): EcDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecId', form.ecId)
  assignTrimmed('ecNo', form.ecNo)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('ecModel', form.ecModel)
  assignTrimmed('ecBomItem', form.ecBomItem)
  assignTrimmed('ecBomSubItem', form.ecBomSubItem)
  assignTrimmed('ecBomNo', form.ecBomNo)
  assignTrimmed('ecChange', form.ecChange)
  assignTrimmed('ecLocal', form.ecLocal)
  assignTrimmed('ecNote', form.ecNote)
  assignTrimmed('ecProcess', form.ecProcess)
  assignTrimmed('ecBomDateStart', form.ecBomDateStart)
  assignTrimmed('ecBomDateEnd', form.ecBomDateEnd)
  assignTrimmed('ecEntryDateStart', form.ecEntryDateStart)
  assignTrimmed('ecEntryDateEnd', form.ecEntryDateEnd)
  assignTrimmed('ecOldItem', form.ecOldItem)
  assignTrimmed('ecOldText', form.ecOldText)
  if (form.ecOldQty !== undefined && form.ecOldQty !== null) {
    query.ecOldQty = form.ecOldQty
  }
  assignTrimmed('ecOldSet', form.ecOldSet)
  assignTrimmed('ecNewItem', form.ecNewItem)
  assignTrimmed('ecNewText', form.ecNewText)
  if (form.ecNewQty !== undefined && form.ecNewQty !== null) {
    query.ecNewQty = form.ecNewQty
  }
  assignTrimmed('ecNewSet', form.ecNewSet)
  if (form.isProcurement !== undefined && form.isProcurement !== null) {
    query.isProcurement = form.isProcurement
  }
  if (form.isCheck !== undefined && form.isCheck !== null) {
    query.isCheck = form.isCheck
  }
  assignTrimmed('ecWarehouse', form.ecWarehouse)
  if (form.isEndOfLine !== undefined && form.isEndOfLine !== null) {
    query.isEndOfLine = form.isEndOfLine
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: EcDetail | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getEcDetailId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as EcDetail
  const key = getEcDetailId(row)
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
async function loadEcDetailDetail(record: EcDetail): Promise<EcDetail | null> {
  const id = getEcDetailId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getEcDetailById(id)
    const index = dataSource.value.findIndex((row) => getEcDetailId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as EcDetail
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
    dataIndex: 'ecDetailId',
    key: 'ecDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecDetailId') ?? ''
  },
  {
    title: t('entity.ecdetail.ecid'),
    dataIndex: 'ecId',
    key: 'ecId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecId') ?? ''
  },
  {
    title: t('entity.ecdetail.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNo') ?? ''
  },
  {
    title: t('entity.ecdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.ecdetail.ecmodel'),
    dataIndex: 'ecModel',
    key: 'ecModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecModel') ?? ''
  },
  {
    title: t('entity.ecdetail.ecbomitem'),
    dataIndex: 'ecBomItem',
    key: 'ecBomItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecBomItem') ?? ''
  },
  {
    title: t('entity.ecdetail.ecbomsubitem'),
    dataIndex: 'ecBomSubItem',
    key: 'ecBomSubItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecBomSubItem') ?? ''
  },
  {
    title: t('entity.ecdetail.ecbomno'),
    dataIndex: 'ecBomNo',
    key: 'ecBomNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecBomNo') ?? ''
  },
  {
    title: t('entity.ecdetail.ecchange'),
    dataIndex: 'ecChange',
    key: 'ecChange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecChange') ?? ''
  },
  {
    title: t('entity.ecdetail.eclocal'),
    dataIndex: 'ecLocal',
    key: 'ecLocal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecLocal') ?? ''
  },
  {
    title: t('entity.ecdetail.ecnote'),
    dataIndex: 'ecNote',
    key: 'ecNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNote') ?? ''
  },
  {
    title: t('entity.ecdetail.ecprocess'),
    dataIndex: 'ecProcess',
    key: 'ecProcess',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecProcess') ?? ''
  },
  {
    title: t('entity.ecdetail.ecbomdate'),
    dataIndex: 'ecBomDate',
    key: 'ecBomDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecBomDate') ?? ''
  },
  {
    title: t('entity.ecdetail.ecentrydate'),
    dataIndex: 'ecEntryDate',
    key: 'ecEntryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecEntryDate') ?? ''
  },
  {
    title: t('entity.ecdetail.ecolditem'),
    dataIndex: 'ecOldItem',
    key: 'ecOldItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecOldItem') ?? ''
  },
  {
    title: t('entity.ecdetail.ecoldtext'),
    dataIndex: 'ecOldText',
    key: 'ecOldText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecOldText') ?? ''
  },
  {
    title: t('entity.ecdetail.ecoldqty'),
    dataIndex: 'ecOldQty',
    key: 'ecOldQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecOldQty') ?? ''
  },
  {
    title: t('entity.ecdetail.ecoldset'),
    dataIndex: 'ecOldSet',
    key: 'ecOldSet',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecOldSet') ?? ''
  },
  {
    title: t('entity.ecdetail.ecnewitem'),
    dataIndex: 'ecNewItem',
    key: 'ecNewItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNewItem') ?? ''
  },
  {
    title: t('entity.ecdetail.ecnewtext'),
    dataIndex: 'ecNewText',
    key: 'ecNewText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNewText') ?? ''
  },
  {
    title: t('entity.ecdetail.ecnewqty'),
    dataIndex: 'ecNewQty',
    key: 'ecNewQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNewQty') ?? ''
  },
  {
    title: t('entity.ecdetail.ecnewset'),
    dataIndex: 'ecNewSet',
    key: 'ecNewSet',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecNewSet') ?? ''
  },
  {
    title: t('entity.ecdetail.isprocurement'),
    dataIndex: 'isProcurement',
    key: 'isProcurement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'isProcurement') ?? ''
  },
  {
    title: t('entity.ecdetail.ischeck'),
    dataIndex: 'isCheck',
    key: 'isCheck',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'isCheck') ?? ''
  },
  {
    title: t('entity.ecdetail.ecwarehouse'),
    dataIndex: 'ecWarehouse',
    key: 'ecWarehouse',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ecWarehouse') ?? ''
  },
  {
    title: t('entity.ecdetail.isendofline'),
    dataIndex: 'isEndOfLine',
    key: 'isEndOfLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'isEndOfLine') ?? ''
  },
  {
    title: t('entity.ecdetail.ec'),
    dataIndex: 'ec',
    key: 'ec',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcDetailField(record, 'ec') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:ec:detail:update',
        onClick: (record: EcDetail) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:ec:detail:delete',
        onClick: (record: EcDetail) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEcDetailId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEcDetailField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: EcDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getEcDetailId(selectedRow.value) === getEcDetailId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getEcDetailList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EcDetail] 加载数据失败', { error })
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
  ecId: '',
  ecNo: '',
  lineNumber: undefined as number | undefined,
  ecModel: '',
  ecBomItem: '',
  ecBomSubItem: '',
  ecBomNo: '',
  ecChange: '',
  ecLocal: '',
  ecNote: '',
  ecProcess: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldQty: undefined as number | undefined,
  ecOldSet: '',
  ecNewItem: '',
  ecNewText: '',
  ecNewQty: undefined as number | undefined,
  ecNewSet: '',
  isProcurement: undefined as number | undefined,
  isCheck: undefined as number | undefined,
  ecWarehouse: '',
  isEndOfLine: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ecdetail._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: EcDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecdetail._self') })
  formLoading.value = true
  try {
    const detail = await loadEcDetailDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ecdetail._self') }))
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
      await updateEcDetail(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ecdetail._self') }))
    } else {
      await createEcDetail(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ecdetail._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  ecDeptPanelRef.value?.reload?.()
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
  const res = await getEcDetailTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcDetail(file, sheetName)
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
    const exportMeta = await exportEcDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ecdetail._self') }))
  } catch (error: any) {
    logger.error('[EcDetail] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ecdetail._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EcDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ecdetail._self'), name: t('common.tip.this.target', { target: t('entity.ecdetail._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcDetailById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ecdetail._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ecdetail._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ecdetail._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEcDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ecdetail._self') }))
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
  ecId: '',
  ecNo: '',
  lineNumber: undefined as number | undefined,
  ecModel: '',
  ecBomItem: '',
  ecBomSubItem: '',
  ecBomNo: '',
  ecChange: '',
  ecLocal: '',
  ecNote: '',
  ecProcess: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldQty: undefined as number | undefined,
  ecOldSet: '',
  ecNewItem: '',
  ecNewText: '',
  ecNewQty: undefined as number | undefined,
  ecNewSet: '',
  isProcurement: undefined as number | undefined,
  isCheck: undefined as number | undefined,
  ecWarehouse: '',
  isEndOfLine: undefined as number | undefined,
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
