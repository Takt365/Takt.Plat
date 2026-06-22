<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会计科目实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:financial:account:title:create"
      update-permission="accounting:financial:account:title:update"
      delete-permission="accounting:financial:account:title:delete"
      import-permission="accounting:financial:account:title:import"
      export-permission="accounting:financial:account:title:export"
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
      :master-row-key="getAccountTitleId"
      :master-row-selection="rowSelection"
      master-id-column-key="accountTitleId"
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
        <template v-if="column.key === 'titleStatus'">
          <a-switch
            :checked="getAccountTitleField(record, 'titleStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleTitleStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <template #detail>
        <AccountTitleChangeLogPanel
          ref="accountTitleChangeLogPanelRef"
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
      <AccountTitleForm
        :key="formData?.accountTitleId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-account-title-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('titleCode')">
      <a-form-item :label="t('entity.accounttitle.titlecode')">
        <a-input
          v-model:value="advancedQueryForm.titleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('titleName')">
      <a-form-item :label="t('entity.accounttitle.titlename')">
        <a-input
          v-model:value="advancedQueryForm.titleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlename') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="t('entity.accounttitle.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.parentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('titleType')">
      <a-form-item :label="t('entity.accounttitle.titletype')">
        <a-input-number
          v-model:value="advancedQueryForm.titleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titletype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('balanceDirection')">
      <a-form-item :label="t('entity.accounttitle.balancedirection')">
        <a-input-number
          v-model:value="advancedQueryForm.balanceDirection"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.balancedirection') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('titleLevel')">
      <a-form-item :label="t('entity.accounttitle.titlelevel')">
        <a-input-number
          v-model:value="advancedQueryForm.titleLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlelevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLeaf')">
      <a-form-item :label="t('entity.accounttitle.isleaf')">
        <a-input-number
          v-model:value="advancedQueryForm.isLeaf"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isleaf') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isAuxiliary')">
      <a-form-item :label="t('entity.accounttitle.isauxiliary')">
        <a-input-number
          v-model:value="advancedQueryForm.isAuxiliary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isauxiliary') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('auxiliaryType')">
      <a-form-item :label="t('entity.accounttitle.auxiliarytype')">
        <a-input-number
          v-model:value="advancedQueryForm.auxiliaryType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.auxiliarytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQuantity')">
      <a-form-item :label="t('entity.accounttitle.isquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.isQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCurrency')">
      <a-form-item :label="t('entity.accounttitle.iscurrency')">
        <a-input-number
          v-model:value="advancedQueryForm.isCurrency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscurrency') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCash')">
      <a-form-item :label="t('entity.accounttitle.iscash')">
        <a-input-number
          v-model:value="advancedQueryForm.isCash"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscash') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBank')">
      <a-form-item :label="t('entity.accounttitle.isbank')">
        <a-input-number
          v-model:value="advancedQueryForm.isBank"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isbank') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.accounttitle.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.relatedplant') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('titleStatus')">
      <a-form-item :label="t('entity.accounttitle.titlestatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.titleStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlestatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="t('entity.accounttitle.validfromstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfromstart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="t('entity.accounttitle.validfromend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfromend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="t('entity.accounttitle.validtostart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validtostart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="t('entity.accounttitle.validtoend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validtoend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.accounttitle._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.accounttitle._self"
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
      :id-column-key="'accountTitleId'"
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
 * 会计科目实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/account-title-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import AccountTitleForm from './components/account-title-form.vue'
import AccountTitleChangeLogPanel from './components/account-title-change-log-panel.vue'
import { provideAccountTitleMasterContext } from './composables/use-account-title-master-context'
import { getAccountTitleList, getAccountTitleById, createAccountTitle, updateAccountTitle, deleteAccountTitleById, deleteAccountTitleBatch, getAccountTitleTemplate, importAccountTitle, exportAccountTitle, updateAccountTitleStatus } from '@/api/accounting/financial/account-title'
import type { AccountTitle, AccountTitleQuery } from '@/types/accounting/financial/account-title'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAccountTitle')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.accounttitle._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<AccountTitle[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<AccountTitle | null>(null)
/** 表格多选行 */
const selectedRows = ref<AccountTitle[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AccountTitle> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: undefined as number | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  relatedPlant: '',
  titleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'titleCode', label: t('entity.accounttitle.titlecode') },
  { key: 'titleName', label: t('entity.accounttitle.titlename') },
  { key: 'parentId', label: t('entity.accounttitle.parentid') },
  { key: 'titleType', label: t('entity.accounttitle.titletype') },
  { key: 'balanceDirection', label: t('entity.accounttitle.balancedirection') },
  { key: 'titleLevel', label: t('entity.accounttitle.titlelevel') },
  { key: 'isLeaf', label: t('entity.accounttitle.isleaf') },
  { key: 'isAuxiliary', label: t('entity.accounttitle.isauxiliary') },
  { key: 'auxiliaryType', label: t('entity.accounttitle.auxiliarytype') },
  { key: 'isQuantity', label: t('entity.accounttitle.isquantity') },
  { key: 'isCurrency', label: t('entity.accounttitle.iscurrency') },
  { key: 'isCash', label: t('entity.accounttitle.iscash') },
  { key: 'isBank', label: t('entity.accounttitle.isbank') },
  { key: 'relatedPlant', label: t('entity.accounttitle.relatedplant') },
  { key: 'titleStatus', label: t('entity.accounttitle.titlestatus') },
  { key: 'validFromStart', label: t('entity.accounttitle.validfromstart') },
  { key: 'validFromEnd', label: t('entity.accounttitle.validfromend') },
  { key: 'validToStart', label: t('entity.accounttitle.validtostart') },
  { key: 'validToEnd', label: t('entity.accounttitle.validtoend') },
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
const entityIdName = 'accountTitleId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideAccountTitleMasterContext()
const accountTitleChangeLogPanelRef = ref<InstanceType<typeof AccountTitleChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {AccountTitleQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AccountTitleQuery>): AccountTitleQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AccountTitleQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AccountTitleQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('titleCode', form.titleCode)
  assignTrimmed('titleName', form.titleName)
  assignTrimmed('parentId', form.parentId)
  if (form.titleType !== undefined && form.titleType !== null) {
    query.titleType = form.titleType
  }
  if (form.balanceDirection !== undefined && form.balanceDirection !== null) {
    query.balanceDirection = form.balanceDirection
  }
  if (form.titleLevel !== undefined && form.titleLevel !== null) {
    query.titleLevel = form.titleLevel
  }
  if (form.isLeaf !== undefined && form.isLeaf !== null) {
    query.isLeaf = form.isLeaf
  }
  if (form.isAuxiliary !== undefined && form.isAuxiliary !== null) {
    query.isAuxiliary = form.isAuxiliary
  }
  if (form.auxiliaryType !== undefined && form.auxiliaryType !== null) {
    query.auxiliaryType = form.auxiliaryType
  }
  if (form.isQuantity !== undefined && form.isQuantity !== null) {
    query.isQuantity = form.isQuantity
  }
  if (form.isCurrency !== undefined && form.isCurrency !== null) {
    query.isCurrency = form.isCurrency
  }
  if (form.isCash !== undefined && form.isCash !== null) {
    query.isCash = form.isCash
  }
  if (form.isBank !== undefined && form.isBank !== null) {
    query.isBank = form.isBank
  }
  assignTrimmed('relatedPlant', form.relatedPlant)
  if (form.titleStatus !== undefined && form.titleStatus !== null) {
    query.titleStatus = form.titleStatus
  }
  assignTrimmed('validFromStart', form.validFromStart)
  assignTrimmed('validFromEnd', form.validFromEnd)
  assignTrimmed('validToStart', form.validToStart)
  assignTrimmed('validToEnd', form.validToEnd)
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
function syncMasterSelection(record: AccountTitle | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getAccountTitleId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as AccountTitle
  const key = getAccountTitleId(row)
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
async function loadAccountTitleDetail(record: AccountTitle): Promise<AccountTitle | null> {
  const id = getAccountTitleId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getAccountTitleById(id)
    const index = dataSource.value.findIndex((row) => getAccountTitleId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as AccountTitle
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
    dataIndex: 'accountTitleId',
    key: 'accountTitleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'accountTitleId') ?? ''
  },
  {
    title: t('entity.accounttitle.titlecode'),
    dataIndex: 'titleCode',
    key: 'titleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleCode') ?? ''
  },
  {
    title: t('entity.accounttitle.titlename'),
    dataIndex: 'titleName',
    key: 'titleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleName') ?? ''
  },
  {
    title: t('entity.accounttitle.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.accounttitle.titletype'),
    dataIndex: 'titleType',
    key: 'titleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleType') ?? ''
  },
  {
    title: t('entity.accounttitle.balancedirection'),
    dataIndex: 'balanceDirection',
    key: 'balanceDirection',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'balanceDirection') ?? ''
  },
  {
    title: t('entity.accounttitle.titlelevel'),
    dataIndex: 'titleLevel',
    key: 'titleLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'titleLevel') ?? ''
  },
  {
    title: t('entity.accounttitle.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isLeaf') ?? ''
  },
  {
    title: t('entity.accounttitle.isauxiliary'),
    dataIndex: 'isAuxiliary',
    key: 'isAuxiliary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isAuxiliary') ?? ''
  },
  {
    title: t('entity.accounttitle.auxiliarytype'),
    dataIndex: 'auxiliaryType',
    key: 'auxiliaryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'auxiliaryType') ?? ''
  },
  {
    title: t('entity.accounttitle.isquantity'),
    dataIndex: 'isQuantity',
    key: 'isQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isQuantity') ?? ''
  },
  {
    title: t('entity.accounttitle.iscurrency'),
    dataIndex: 'isCurrency',
    key: 'isCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isCurrency') ?? ''
  },
  {
    title: t('entity.accounttitle.iscash'),
    dataIndex: 'isCash',
    key: 'isCash',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isCash') ?? ''
  },
  {
    title: t('entity.accounttitle.isbank'),
    dataIndex: 'isBank',
    key: 'isBank',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'isBank') ?? ''
  },
  {
    title: t('entity.accounttitle.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'relatedPlant') ?? ''
  },
  {
    title: t('entity.accounttitle.titlestatus'),
    dataIndex: 'titleStatus',
    key: 'titleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.accounttitle.validfrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'validFrom') ?? ''
  },
  {
    title: t('entity.accounttitle.validto'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAccountTitleField(record, 'validTo') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:account:title:update',
        onClick: (record: AccountTitle) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:account:title:delete',
        onClick: (record: AccountTitle) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAccountTitleId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAccountTitleField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AccountTitle[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: AccountTitle, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getAccountTitleId(selectedRow.value) === getAccountTitleId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AccountTitle[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getAccountTitleList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AccountTitle] 加载数据失败', { error })
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
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: undefined as number | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  relatedPlant: '',
  titleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.accounttitle._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: AccountTitle) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.accounttitle._self') })
  formLoading.value = true
  try {
    const detail = await loadAccountTitleDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.accounttitle._self') }))
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
      await updateAccountTitle(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.accounttitle._self') }))
    } else {
      await createAccountTitle(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.accounttitle._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  accountTitleChangeLogPanelRef.value?.reload?.()
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
  const res = await getAccountTitleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAccountTitle(file, sheetName)
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
    const exportMeta = await exportAccountTitle(
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
    message.success(t('common.feedback.export.success', { target: t('entity.accounttitle._self') }))
  } catch (error: any) {
    logger.error('[AccountTitle] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.accounttitle._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: AccountTitle) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.accounttitle._self'), name: t('common.tip.this.target', { target: t('entity.accounttitle._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAccountTitleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.accounttitle._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.accounttitle._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.accounttitle._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAccountTitleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.accounttitle._self') }))
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
async function handleTitleStatusChange(record: AccountTitle, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getAccountTitleField(record, 'titleStatus')
  const id = getAccountTitleId(record)
  const row = dataSource.value.find((item) => getAccountTitleId(item) === id)
  if (row) {
    row.titleStatus = newVal
  }
  try {
    await updateAccountTitleStatus({ accountTitleId: id, titleStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.titleStatus = oldVal
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
  titleCode: '',
  titleName: '',
  parentId: '',
  titleType: undefined as number | undefined,
  balanceDirection: undefined as number | undefined,
  titleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: undefined as number | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  relatedPlant: '',
  titleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
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
