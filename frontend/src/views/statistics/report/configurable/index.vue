<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/report/configurable -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：自定义报表主实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-report-configurable">
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
      create-permission="statistics:report:configurable:create"
      update-permission="statistics:report:configurable:update"
      delete-permission="statistics:report:configurable:delete"
      import-permission="statistics:report:configurable:import"
      export-permission="statistics:report:configurable:export"
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
      :id-column-key="'configurableId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getConfigurableId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'reportStatus'">
          <TaktDictTag
            :value="getConfigurableField(record, 'reportStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableSource._self') }}</div>
          <a-table
            v-if="hasConfigurableSourceRows(record)"
            :columns="configurableSourceExpandColumns"
            :data-source="getConfigurableSourceRows(record)"
            :row-key="(row: ConfigurableSource, index?: number) => row?.configurableSourceId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableJoin._self') }}</div>
          <a-table
            v-if="hasConfigurableJoinRows(record)"
            :columns="configurableJoinExpandColumns"
            :data-source="getConfigurableJoinRows(record)"
            :row-key="(row: ConfigurableJoin, index?: number) => row?.configurableJoinId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableField._self') }}</div>
          <a-table
            v-if="hasConfigurableFieldRows(record)"
            :columns="configurableFieldExpandColumns"
            :data-source="getConfigurableFieldRows(record)"
            :row-key="(row: ConfigurableField, index?: number) => row?.configurableFieldId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableSelection._self') }}</div>
          <a-table
            v-if="hasConfigurableSelectionRows(record)"
            :columns="configurableSelectionExpandColumns"
            :data-source="getConfigurableSelectionRows(record)"
            :row-key="(row: ConfigurableSelection, index?: number) => row?.configurableSelectionId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableGroupBy._self') }}</div>
          <a-table
            v-if="hasConfigurableGroupByRows(record)"
            :columns="configurableGroupByExpandColumns"
            :data-source="getConfigurableGroupByRows(record)"
            :row-key="(row: ConfigurableGroupBy, index?: number) => row?.configurableGroupById || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.configurableOrderBy._self') }}</div>
          <a-table
            v-if="hasConfigurableOrderByRows(record)"
            :columns="configurableOrderByExpandColumns"
            :data-source="getConfigurableOrderByRows(record)"
            :row-key="(row: ConfigurableOrderBy, index?: number) => row?.configurableOrderById || String(index ?? 0)"
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
      <ConfigurableForm
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
      :storage-key="'takt-query-fields-statistics-report-configurable'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('reportCode')">
      <a-form-item :label="t('entity.configurable.reportcode')">
        <a-input
          v-model:value="advancedQueryForm.reportCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reportName')">
      <a-form-item :label="t('entity.configurable.reportname')">
        <a-input
          v-model:value="advancedQueryForm.reportName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reportDomain')">
      <a-form-item :label="t('entity.configurable.reportdomain')">
        <a-input-number
          v-model:value="advancedQueryForm.reportDomain"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportdomain') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reportSubCategory')">
      <a-form-item :label="t('entity.configurable.reportsubcategory')">
        <a-input
          v-model:value="advancedQueryForm.reportSubCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.reportsubcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('distinctRows')">
      <a-form-item :label="t('entity.configurable.distinctrows')">
        <a-input-number
          v-model:value="advancedQueryForm.distinctRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.distinctrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxExportRows')">
      <a-form-item :label="t('entity.configurable.maxexportrows')">
        <a-input-number
          v-model:value="advancedQueryForm.maxExportRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxexportrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxQueryRows')">
      <a-form-item :label="t('entity.configurable.maxqueryrows')">
        <a-input-number
          v-model:value="advancedQueryForm.maxQueryRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxqueryrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ownerUserId')">
      <a-form-item :label="t('entity.configurable.owneruserid')">
        <a-input
          v-model:value="advancedQueryForm.ownerUserId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.owneruserid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.configurable.isbuiltin')">
        <a-input-number
          v-model:value="advancedQueryForm.isBuiltIn"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.isbuiltin') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.configurable.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reportStatus')">
      <a-form-item :label="t('entity.configurable.reportstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.reportStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.reportstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.configurable.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.configurable.description') })"
          :rows="2"
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
      :title="t('common.dialog.title.import', { entity: t('entity.configurable._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.configurable._self"
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
      :id-column-key="'configurableId'"
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
 * 自定义报表主实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/statistics/report/configurable
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ConfigurableForm from './components/configurable-form.vue'
import { getConfigurableList, getConfigurableById, createConfigurable, updateConfigurable, deleteConfigurableById, deleteConfigurableBatch, getConfigurableTemplate, importConfigurable, exportConfigurable } from '@/api/statistics/report/configurable'
import * as configurableSourceApi from '@/api/statistics/report/configurable-source'
import * as configurableJoinApi from '@/api/statistics/report/configurable-join'
import * as configurableFieldApi from '@/api/statistics/report/configurable-field'
import * as configurableSelectionApi from '@/api/statistics/report/configurable-selection'
import * as configurableGroupByApi from '@/api/statistics/report/configurable-group-by'
import * as configurableOrderByApi from '@/api/statistics/report/configurable-order-by'
import type { ConfigurableSource, ConfigurableSourceQuery } from '@/types/statistics/report/configurable-source'
import type { ConfigurableJoin, ConfigurableJoinQuery } from '@/types/statistics/report/configurable-join'
import type { ConfigurableField, ConfigurableFieldQuery } from '@/types/statistics/report/configurable-field'
import type { ConfigurableSelection, ConfigurableSelectionQuery } from '@/types/statistics/report/configurable-selection'
import type { ConfigurableGroupBy, ConfigurableGroupByQuery } from '@/types/statistics/report/configurable-group-by'
import type { ConfigurableOrderBy, ConfigurableOrderByQuery } from '@/types/statistics/report/configurable-order-by'
import type { Configurable, ConfigurableQuery, ConfigurableCreate, ConfigurableUpdate } from '@/types/statistics/report/configurable'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktConfigurable')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.configurable._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Configurable[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Configurable | null>(null)
/** 表格多选行 */
const selectedRows = ref<Configurable[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Configurable>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  reportCode: '',
  reportName: '',
  reportDomain: undefined as number | undefined,
  reportSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  ownerUserId: '',
  isBuiltIn: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  reportStatus: undefined as number | undefined,
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'reportCode', label: t('entity.configurable.reportcode') },
  { key: 'reportName', label: t('entity.configurable.reportname') },
  { key: 'reportDomain', label: t('entity.configurable.reportdomain') },
  { key: 'reportSubCategory', label: t('entity.configurable.reportsubcategory') },
  { key: 'distinctRows', label: t('entity.configurable.distinctrows') },
  { key: 'maxExportRows', label: t('entity.configurable.maxexportrows') },
  { key: 'maxQueryRows', label: t('entity.configurable.maxqueryrows') },
  { key: 'ownerUserId', label: t('entity.configurable.owneruserid') },
  { key: 'isBuiltIn', label: t('entity.configurable.isbuiltin') },
  { key: 'sortOrder', label: t('entity.configurable.sortorder') },
  { key: 'reportStatus', label: t('entity.configurable.reportstatus') },
  { key: 'description', label: t('entity.configurable.description') },
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
const entityIdName = 'configurableId'
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

/** 展开行预览：configurableSource 列 */
const configurableSourceExpandColumns = computed(() => [
  {
    title: t('entity.configurableSource.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSource.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSource.tablename'),
    dataIndex: 'tableName',
    key: 'tableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSource.isprimary'),
    dataIndex: 'isPrimary',
    key: 'isPrimary',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSource.configurable'),
    dataIndex: 'configurable',
    key: 'configurable',
    ellipsis: true,
  },
])

/** 展开行预览：configurableJoin 列 */
const configurableJoinExpandColumns = computed(() => [
  {
    title: t('entity.configurableJoin.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.jointype'),
    dataIndex: 'joinType',
    key: 'joinType',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.leftsourcealias'),
    dataIndex: 'leftSourceAlias',
    key: 'leftSourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.leftcolumnname'),
    dataIndex: 'leftColumnName',
    key: 'leftColumnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.rightsourcealias'),
    dataIndex: 'rightSourceAlias',
    key: 'rightSourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.rightcolumnname'),
    dataIndex: 'rightColumnName',
    key: 'rightColumnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableJoin.configurable'),
    dataIndex: 'configurable',
    key: 'configurable',
    ellipsis: true,
  },
])

/** 展开行预览：configurableField 列 */
const configurableFieldExpandColumns = computed(() => [
  {
    title: t('entity.configurableField.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.displayname'),
    dataIndex: 'displayName',
    key: 'displayName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.outputalias'),
    dataIndex: 'outputAlias',
    key: 'outputAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.aggregatefunc'),
    dataIndex: 'aggregateFunc',
    key: 'aggregateFunc',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.isvisible'),
    dataIndex: 'isVisible',
    key: 'isVisible',
    ellipsis: true,
  },
  {
    title: t('entity.configurableField.configurable'),
    dataIndex: 'configurable',
    key: 'configurable',
    ellipsis: true,
  },
])

/** 展开行预览：configurableSelection 列 */
const configurableSelectionExpandColumns = computed(() => [
  {
    title: t('entity.configurableSelection.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.displayname'),
    dataIndex: 'displayName',
    key: 'displayName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.filteroperator'),
    dataIndex: 'filterOperator',
    key: 'filterOperator',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.defaultvalue'),
    dataIndex: 'defaultValue',
    key: 'defaultValue',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.defaultvalueto'),
    dataIndex: 'defaultValueTo',
    key: 'defaultValueTo',
    ellipsis: true,
  },
  {
    title: t('entity.configurableSelection.isrequired'),
    dataIndex: 'isRequired',
    key: 'isRequired',
    ellipsis: true,
  },
])

/** 展开行预览：configurableGroupBy 列 */
const configurableGroupByExpandColumns = computed(() => [
  {
    title: t('entity.configurableGroupBy.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableGroupBy.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableGroupBy.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableGroupBy.configurable'),
    dataIndex: 'configurable',
    key: 'configurable',
    ellipsis: true,
  },
])

/** 展开行预览：configurableOrderBy 列 */
const configurableOrderByExpandColumns = computed(() => [
  {
    title: t('entity.configurableOrderBy.configurablename'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableOrderBy.sourcealias'),
    dataIndex: 'sourceAlias',
    key: 'sourceAlias',
    ellipsis: true,
  },
  {
    title: t('entity.configurableOrderBy.columnname'),
    dataIndex: 'columnName',
    key: 'columnName',
    ellipsis: true,
  },
  {
    title: t('entity.configurableOrderBy.sortdirection'),
    dataIndex: 'sortDirection',
    key: 'sortDirection',
    ellipsis: true,
  },
  {
    title: t('entity.configurableOrderBy.configurable'),
    dataIndex: 'configurable',
    key: 'configurable',
    ellipsis: true,
  },
])

/** 读取主表行上的 configurableSource 子表缓存 */
function getConfigurableSourceRows(record: Configurable): ConfigurableSource[] {
  return (record as any)?.sources ?? []
}

/** 主表行是否已加载 configurableSource 子表 */
function hasConfigurableSourceRows(record: Configurable): boolean {
  return getConfigurableSourceRows(record).length > 0
}

/** 读取主表行上的 configurableJoin 子表缓存 */
function getConfigurableJoinRows(record: Configurable): ConfigurableJoin[] {
  return (record as any)?.joins ?? []
}

/** 主表行是否已加载 configurableJoin 子表 */
function hasConfigurableJoinRows(record: Configurable): boolean {
  return getConfigurableJoinRows(record).length > 0
}

/** 读取主表行上的 configurableField 子表缓存 */
function getConfigurableFieldRows(record: Configurable): ConfigurableField[] {
  return (record as any)?.fields ?? []
}

/** 主表行是否已加载 configurableField 子表 */
function hasConfigurableFieldRows(record: Configurable): boolean {
  return getConfigurableFieldRows(record).length > 0
}

/** 读取主表行上的 configurableSelection 子表缓存 */
function getConfigurableSelectionRows(record: Configurable): ConfigurableSelection[] {
  return (record as any)?.selections ?? []
}

/** 主表行是否已加载 configurableSelection 子表 */
function hasConfigurableSelectionRows(record: Configurable): boolean {
  return getConfigurableSelectionRows(record).length > 0
}

/** 读取主表行上的 configurableGroupBy 子表缓存 */
function getConfigurableGroupByRows(record: Configurable): ConfigurableGroupBy[] {
  return (record as any)?.groupBys ?? []
}

/** 主表行是否已加载 configurableGroupBy 子表 */
function hasConfigurableGroupByRows(record: Configurable): boolean {
  return getConfigurableGroupByRows(record).length > 0
}

/** 读取主表行上的 configurableOrderBy 子表缓存 */
function getConfigurableOrderByRows(record: Configurable): ConfigurableOrderBy[] {
  return (record as any)?.orderBys ?? []
}

/** 主表行是否已加载 configurableOrderBy 子表 */
function hasConfigurableOrderByRows(record: Configurable): boolean {
  return getConfigurableOrderByRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadConfigurableDetail(record: Configurable): Promise<Configurable | null> {
  const id = getConfigurableId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getConfigurableById(id)
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Configurable
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 configurableSource 子表（ConfigurableSourceQuery + configurableSourceApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableSourceForConfigurable(record: Configurable): Promise<ConfigurableSource[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableSourceQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableSourceApi.getConfigurableSourceList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, sources: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 configurableJoin 子表（ConfigurableJoinQuery + configurableJoinApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableJoinForConfigurable(record: Configurable): Promise<ConfigurableJoin[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableJoinQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableJoinApi.getConfigurableJoinList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, joins: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 configurableField 子表（ConfigurableFieldQuery + configurableFieldApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableFieldForConfigurable(record: Configurable): Promise<ConfigurableField[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableFieldQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableFieldApi.getConfigurableFieldList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, fields: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 configurableSelection 子表（ConfigurableSelectionQuery + configurableSelectionApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableSelectionForConfigurable(record: Configurable): Promise<ConfigurableSelection[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableSelectionQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableSelectionApi.getConfigurableSelectionList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, selections: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 configurableGroupBy 子表（ConfigurableGroupByQuery + configurableGroupByApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableGroupByForConfigurable(record: Configurable): Promise<ConfigurableGroupBy[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableGroupByQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableGroupByApi.getConfigurableGroupByList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, groupBys: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 configurableOrderBy 子表（ConfigurableOrderByQuery + configurableOrderByApi，与主表 ConfigurableQuery 分离） */
async function loadConfigurableOrderByForConfigurable(record: Configurable): Promise<ConfigurableOrderBy[]> {
  const masterId = getConfigurableId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ConfigurableOrderByQuery = {
      pageIndex: 1,
      pageSize: 500,
      configurableId: masterId,
    }
    const result = await configurableOrderByApi.getConfigurableOrderByList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, orderBys: rows } as Configurable
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureConfigurableChildrenLoaded(record: Configurable) {
  if (!hasConfigurableSourceRows(record)) {
    await loadConfigurableSourceForConfigurable(record)
  }
  if (!hasConfigurableJoinRows(record)) {
    await loadConfigurableJoinForConfigurable(record)
  }
  if (!hasConfigurableFieldRows(record)) {
    await loadConfigurableFieldForConfigurable(record)
  }
  if (!hasConfigurableSelectionRows(record)) {
    await loadConfigurableSelectionForConfigurable(record)
  }
  if (!hasConfigurableGroupByRows(record)) {
    await loadConfigurableGroupByForConfigurable(record)
  }
  if (!hasConfigurableOrderByRows(record)) {
    await loadConfigurableOrderByForConfigurable(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Configurable) {
  const key = getConfigurableId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureConfigurableChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'configurableId',
    key: 'configurableId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'configurableId') ?? ''
  },
  {
    title: t('entity.configurable.reportcode'),
    dataIndex: 'reportCode',
    key: 'reportCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'reportCode') ?? ''
  },
  {
    title: t('entity.configurable.reportname'),
    dataIndex: 'reportName',
    key: 'reportName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'reportName') ?? ''
  },
  {
    title: t('entity.configurable.reportdomain'),
    dataIndex: 'reportDomain',
    key: 'reportDomain',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'reportDomain') ?? ''
  },
  {
    title: t('entity.configurable.reportsubcategory'),
    dataIndex: 'reportSubCategory',
    key: 'reportSubCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'reportSubCategory') ?? ''
  },
  {
    title: t('entity.configurable.distinctrows'),
    dataIndex: 'distinctRows',
    key: 'distinctRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'distinctRows') ?? ''
  },
  {
    title: t('entity.configurable.maxexportrows'),
    dataIndex: 'maxExportRows',
    key: 'maxExportRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'maxExportRows') ?? ''
  },
  {
    title: t('entity.configurable.maxqueryrows'),
    dataIndex: 'maxQueryRows',
    key: 'maxQueryRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'maxQueryRows') ?? ''
  },
  {
    title: t('entity.configurable.owneruserid'),
    dataIndex: 'ownerUserId',
    key: 'ownerUserId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'ownerUserId') ?? ''
  },
  {
    title: t('entity.configurable.ownerusername'),
    dataIndex: 'ownerUserName',
    key: 'ownerUserName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'ownerUserName') ?? ''
  },
  {
    title: t('entity.configurable.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'isBuiltIn') ?? ''
  },
  {
    title: t('entity.configurable.reportstatus'),
    dataIndex: 'reportStatus',
    key: 'reportStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.configurable.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'description') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:report:configurable:update',
        onClick: (record: Configurable) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:report:configurable:delete',
        onClick: (record: Configurable) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getConfigurableId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getConfigurableField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Configurable[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Configurable, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getConfigurableId(selectedRow.value) === getConfigurableId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Configurable[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Configurable) => ({
  onClick: () => {
    const key = getConfigurableId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getConfigurableId(item)))
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
    const params: ConfigurableQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getConfigurableList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Configurable] 加载数据失败', { error })
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
  reportCode: '',
  reportName: '',
  reportDomain: undefined as number | undefined,
  reportSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  ownerUserId: '',
  isBuiltIn: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  reportStatus: undefined as number | undefined,
  description: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.configurable._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Configurable) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.configurable._self') })
  formLoading.value = true
  try {
    const detail = await loadConfigurableDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.configurable._self') }))
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
      await updateConfigurable(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.configurable._self') }))
    } else {
      await createConfigurable(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.configurable._self') }))
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
  const res = await getConfigurableTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importConfigurable(file, sheetName)
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
    const exportQuery: ConfigurableQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportConfigurable(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.configurable._self') }))
  } catch (error: any) {
    logger.error('[Configurable] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.configurable._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Configurable) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.configurable._self'), name: t('common.tip.this.target', { target: t('entity.configurable._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteConfigurableById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.configurable._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.configurable._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.configurable._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteConfigurableBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.configurable._self') }))
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
  reportCode: '',
  reportName: '',
  reportDomain: undefined as number | undefined,
  reportSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  ownerUserId: '',
  isBuiltIn: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  reportStatus: undefined as number | undefined,
  description: '',
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
.statistics-report-configurable {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
