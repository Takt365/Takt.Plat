<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变来源主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      import-permission="logistics:manufacturing:engineering:change:source:ec:import"
      export-permission="logistics:manufacturing:engineering:change:source:ec:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
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
      :master-row-key="getSourceEcId"
      :master-row-selection="rowSelection"
      master-id-column-key="sourceEcId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <SourceEcDetailPanel
          ref="sourceEcDetailPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 详情对话框 -->
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.sourceec._self') })"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <SourceEcForm
          :key="detailData?.sourceEcId ?? 'detail'"
          :form-data="detailData"
          :loading="detailLoading"
          read-only
        />
      </a-spin>
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-source-ec'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('sourceEcNo')">
      <a-form-item :label="t('entity.sourceec.no')">
        <a-input
          v-model:value="advancedQueryForm.sourceEcNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.no') })"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceModel')">
      <a-form-item :label="t('entity.sourceec.sourcemodel')">
        <a-input
          v-model:value="advancedQueryForm.sourceModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemodel') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTitle')">
      <a-form-item :label="t('entity.sourceec.sourcetitle')">
        <a-input
          v-model:value="advancedQueryForm.sourceTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetitle') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceStatus')">
      <a-form-item :label="t('entity.sourceec.sourcestatus')">
        <a-input
          v-model:value="advancedQueryForm.sourceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcestatus') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceIssueDateStart')">
      <a-form-item :label="t('entity.sourceec.sourceissuedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceIssueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceissuedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceIssueDateEnd')">
      <a-form-item :label="t('entity.sourceec.sourceissuedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceIssueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceissuedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTcjOwner')">
      <a-form-item :label="t('entity.sourceec.sourcetcjowner')">
        <a-input
          v-model:value="advancedQueryForm.sourceTcjOwner"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetcjowner') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTcjDependency')">
      <a-form-item :label="t('entity.sourceec.sourcetcjdependency')">
        <a-input
          v-model:value="advancedQueryForm.sourceTcjDependency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetcjdependency') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcMeeting')">
      <a-form-item :label="t('entity.sourceec.meeting')">
        <a-input
          v-model:value="advancedQueryForm.sourceEcMeeting"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.meeting') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourcePpNo')">
      <a-form-item :label="t('entity.sourceec.sourceppno')">
        <a-input
          v-model:value="advancedQueryForm.sourcePpNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceppno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTechnicalNoticeNo')">
      <a-form-item :label="t('entity.sourceec.sourcetechnicalnoticeno')">
        <a-input
          v-model:value="advancedQueryForm.sourceTechnicalNoticeNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetechnicalnoticeno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceImplementation')">
      <a-form-item :label="t('entity.sourceec.sourceimplementation')">
        <a-input
          v-model:value="advancedQueryForm.sourceImplementation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceimplementation') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceMainChangeReason')">
      <a-form-item :label="t('entity.sourceec.sourcemainchangereason')">
        <a-input
          v-model:value="advancedQueryForm.sourceMainChangeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemainchangereason') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSecondaryChangeReason')">
      <a-form-item :label="t('entity.sourceec.sourcesecondarychangereason')">
        <a-input
          v-model:value="advancedQueryForm.sourceSecondaryChangeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcesecondarychangereason') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSafetyRegulation')">
      <a-form-item :label="t('entity.sourceec.sourcesafetyregulation')">
        <a-input
          v-model:value="advancedQueryForm.sourceSafetyRegulation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcesafetyregulation') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceProgressStatus')">
      <a-form-item :label="t('entity.sourceec.sourceprogressstatus')">
        <a-input
          v-model:value="advancedQueryForm.sourceProgressStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceprogressstatus') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSerialNumberControl')">
      <a-form-item :label="t('entity.sourceec.sourceserialnumbercontrol')">
        <a-input
          v-model:value="advancedQueryForm.sourceSerialNumberControl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceserialnumbercontrol') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCustomerApproval')">
      <a-form-item :label="t('entity.sourceec.sourcecustomerapproval')">
        <a-input
          v-model:value="advancedQueryForm.sourceCustomerApproval"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcecustomerapproval') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceServiceManualRevision')">
      <a-form-item :label="t('entity.sourceec.sourceservicemanualrevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceServiceManualRevision"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceservicemanualrevision') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceUserManualRevision')">
      <a-form-item :label="t('entity.sourceec.sourceusermanualrevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceUserManualRevision"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceusermanualrevision') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourcePromotionManualRevision')">
      <a-form-item :label="t('entity.sourceec.sourcepromotionmanualrevision')">
        <a-input
          v-model:value="advancedQueryForm.sourcePromotionManualRevision"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcepromotionmanualrevision') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceStandardDocumentRevision')">
      <a-form-item :label="t('entity.sourceec.sourcestandarddocumentrevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceStandardDocumentRevision"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcestandarddocumentrevision') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceInformationRelease')">
      <a-form-item :label="t('entity.sourceec.sourceinformationrelease')">
        <a-input
          v-model:value="advancedQueryForm.sourceInformationRelease"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceinformationrelease') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCostChange')">
      <a-form-item :label="t('entity.sourceec.sourcecostchange')">
        <a-input
          v-model:value="advancedQueryForm.sourceCostChange"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcecostchange') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceUnitCost')">
      <a-form-item :label="t('entity.sourceec.sourceunitcost')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceUnitCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceunitcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceMoldModificationCost')">
      <a-form-item :label="t('entity.sourceec.sourcemoldmodificationcost')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceMoldModificationCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemoldmodificationcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceRelatedDrawing')">
      <a-form-item :label="t('entity.sourceec.sourcerelateddrawing')">
        <a-input
          v-model:value="advancedQueryForm.sourceRelatedDrawing"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcerelateddrawing') })"
          show-count
          :maxlength="210"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcContent')">
      <a-form-item :label="t('entity.sourceec.content')">
        <a-textarea
          v-model:value="advancedQueryForm.sourceEcContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceec.content') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.sourceec._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.sourceec._self"
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
      :id-column-key="'sourceEcId'"
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
 * 设变来源主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/source-ec
 */
import { ref, computed, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import SourceEcForm from './components/source-ec-form.vue'
import SourceEcDetailPanel from './components/source-ec-detail-panel.vue'
import { provideSourceEcMasterContext } from './composables/use-source-ec-master-context'
import { getSourceEcList, getSourceEcById, getSourceEcTemplate, importSourceEc, exportSourceEc } from '@/api/logistics/manufacturing/engineering-change/source-ec'
import type { SourceEc, SourceEcQuery } from '@/types/logistics/manufacturing/engineering-change/source-ec'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEyeLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSourceEc')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sourceec._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SourceEc[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SourceEc | null>(null)
/** 表格多选行 */
const selectedRows = ref<SourceEc[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载 loading */
const detailLoading = ref(false)
/** 详情数据（只读展示） */
const detailData = ref<Partial<SourceEc> | null>(null)

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  sourceEcNo: '',
  sourceModel: '',
  sourceTitle: '',
  sourceStatus: '',
  sourceIssueDateStart: '',
  sourceIssueDateEnd: '',
  sourceTcjOwner: '',
  sourceTcjDependency: '',
  sourceEcMeeting: '',
  sourcePpNo: '',
  sourceTechnicalNoticeNo: '',
  sourceImplementation: '',
  sourceMainChangeReason: '',
  sourceSecondaryChangeReason: '',
  sourceSafetyRegulation: '',
  sourceProgressStatus: '',
  sourceSerialNumberControl: '',
  sourceCustomerApproval: '',
  sourceServiceManualRevision: '',
  sourceUserManualRevision: '',
  sourcePromotionManualRevision: '',
  sourceStandardDocumentRevision: '',
  sourceInformationRelease: '',
  sourceCostChange: '',
  sourceUnitCost: undefined as number | undefined,
  sourceMoldModificationCost: undefined as number | undefined,
  sourceRelatedDrawing: '',
  sourceEcContent: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'sourceEcNo', label: t('entity.sourceec.no') },
  { key: 'sourceModel', label: t('entity.sourceec.sourcemodel') },
  { key: 'sourceTitle', label: t('entity.sourceec.sourcetitle') },
  { key: 'sourceStatus', label: t('entity.sourceec.sourcestatus') },
  { key: 'sourceIssueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.sourceec.sourceissuedate')) },
  { key: 'sourceIssueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.sourceec.sourceissuedate')) },
  { key: 'sourceTcjOwner', label: t('entity.sourceec.sourcetcjowner') },
  { key: 'sourceTcjDependency', label: t('entity.sourceec.sourcetcjdependency') },
  { key: 'sourceEcMeeting', label: t('entity.sourceec.meeting') },
  { key: 'sourcePpNo', label: t('entity.sourceec.sourceppno') },
  { key: 'sourceTechnicalNoticeNo', label: t('entity.sourceec.sourcetechnicalnoticeno') },
  { key: 'sourceImplementation', label: t('entity.sourceec.sourceimplementation') },
  { key: 'sourceMainChangeReason', label: t('entity.sourceec.sourcemainchangereason') },
  { key: 'sourceSecondaryChangeReason', label: t('entity.sourceec.sourcesecondarychangereason') },
  { key: 'sourceSafetyRegulation', label: t('entity.sourceec.sourcesafetyregulation') },
  { key: 'sourceProgressStatus', label: t('entity.sourceec.sourceprogressstatus') },
  { key: 'sourceSerialNumberControl', label: t('entity.sourceec.sourceserialnumbercontrol') },
  { key: 'sourceCustomerApproval', label: t('entity.sourceec.sourcecustomerapproval') },
  { key: 'sourceServiceManualRevision', label: t('entity.sourceec.sourceservicemanualrevision') },
  { key: 'sourceUserManualRevision', label: t('entity.sourceec.sourceusermanualrevision') },
  { key: 'sourcePromotionManualRevision', label: t('entity.sourceec.sourcepromotionmanualrevision') },
  { key: 'sourceStandardDocumentRevision', label: t('entity.sourceec.sourcestandarddocumentrevision') },
  { key: 'sourceInformationRelease', label: t('entity.sourceec.sourceinformationrelease') },
  { key: 'sourceCostChange', label: t('entity.sourceec.sourcecostchange') },
  { key: 'sourceUnitCost', label: t('entity.sourceec.sourceunitcost') },
  { key: 'sourceMoldModificationCost', label: t('entity.sourceec.sourcemoldmodificationcost') },
  { key: 'sourceRelatedDrawing', label: t('entity.sourceec.sourcerelateddrawing') },
  { key: 'sourceEcContent', label: t('entity.sourceec.content') },
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
const entityIdName = 'sourceEcId'

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSourceEcMasterContext()
const sourceEcDetailPanelRef = ref<InstanceType<typeof SourceEcDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SourceEcQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SourceEcQuery>): SourceEcQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SourceEcQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('sourceEcNo', form.sourceEcNo)
  assignTrimmed('sourceModel', form.sourceModel)
  assignTrimmed('sourceTitle', form.sourceTitle)
  assignTrimmed('sourceStatus', form.sourceStatus)
  assignTrimmed('sourceIssueDateStart', form.sourceIssueDateStart)
  assignTrimmed('sourceIssueDateEnd', form.sourceIssueDateEnd)
  assignTrimmed('sourceTcjOwner', form.sourceTcjOwner)
  assignTrimmed('sourceTcjDependency', form.sourceTcjDependency)
  assignTrimmed('sourceEcMeeting', form.sourceEcMeeting)
  assignTrimmed('sourcePpNo', form.sourcePpNo)
  assignTrimmed('sourceTechnicalNoticeNo', form.sourceTechnicalNoticeNo)
  assignTrimmed('sourceImplementation', form.sourceImplementation)
  assignTrimmed('sourceMainChangeReason', form.sourceMainChangeReason)
  assignTrimmed('sourceSecondaryChangeReason', form.sourceSecondaryChangeReason)
  assignTrimmed('sourceSafetyRegulation', form.sourceSafetyRegulation)
  assignTrimmed('sourceProgressStatus', form.sourceProgressStatus)
  assignTrimmed('sourceSerialNumberControl', form.sourceSerialNumberControl)
  assignTrimmed('sourceCustomerApproval', form.sourceCustomerApproval)
  assignTrimmed('sourceServiceManualRevision', form.sourceServiceManualRevision)
  assignTrimmed('sourceUserManualRevision', form.sourceUserManualRevision)
  assignTrimmed('sourcePromotionManualRevision', form.sourcePromotionManualRevision)
  assignTrimmed('sourceStandardDocumentRevision', form.sourceStandardDocumentRevision)
  assignTrimmed('sourceInformationRelease', form.sourceInformationRelease)
  assignTrimmed('sourceCostChange', form.sourceCostChange)
  if (form.sourceUnitCost !== undefined && form.sourceUnitCost !== null) {
    query.sourceUnitCost = form.sourceUnitCost
  }
  if (form.sourceMoldModificationCost !== undefined && form.sourceMoldModificationCost !== null) {
    query.sourceMoldModificationCost = form.sourceMoldModificationCost
  }
  assignTrimmed('sourceRelatedDrawing', form.sourceRelatedDrawing)
  assignTrimmed('sourceEcContent', form.sourceEcContent)
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
function syncMasterSelection(record: SourceEc | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSourceEcId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SourceEc
  const key = getSourceEcId(row)
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
async function loadSourceEcDetail(record: SourceEc): Promise<SourceEc | null> {
  const id = getSourceEcId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSourceEcById(id)
    const index = dataSource.value.findIndex((row) => getSourceEcId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SourceEc
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
    dataIndex: 'sourceEcId',
    key: 'sourceEcId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcId') ?? ''
  },
  {
    title: t('entity.sourceec.no'),
    dataIndex: 'sourceEcNo',
    key: 'sourceEcNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcNo') ?? ''
  },
  {
    title: t('entity.sourceec.sourcemodel'),
    dataIndex: 'sourceModel',
    key: 'sourceModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceModel') ?? ''
  },
  {
    title: t('entity.sourceec.sourcetitle'),
    dataIndex: 'sourceTitle',
    key: 'sourceTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTitle') ?? ''
  },
  {
    title: t('entity.sourceec.sourcestatus'),
    dataIndex: 'sourceStatus',
    key: 'sourceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceStatus') ?? ''
  },
  {
    title: t('entity.sourceec.sourceissuedate'),
    dataIndex: 'sourceIssueDate',
    key: 'sourceIssueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceIssueDate') ?? ''
  },
  {
    title: t('entity.sourceec.sourcetcjowner'),
    dataIndex: 'sourceTcjOwner',
    key: 'sourceTcjOwner',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTcjOwner') ?? ''
  },
  {
    title: t('entity.sourceec.sourcetcjdependency'),
    dataIndex: 'sourceTcjDependency',
    key: 'sourceTcjDependency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTcjDependency') ?? ''
  },
  {
    title: t('entity.sourceec.meeting'),
    dataIndex: 'sourceEcMeeting',
    key: 'sourceEcMeeting',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcMeeting') ?? ''
  },
  {
    title: t('entity.sourceec.sourceppno'),
    dataIndex: 'sourcePpNo',
    key: 'sourcePpNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourcePpNo') ?? ''
  },
  {
    title: t('entity.sourceec.sourcetechnicalnoticeno'),
    dataIndex: 'sourceTechnicalNoticeNo',
    key: 'sourceTechnicalNoticeNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTechnicalNoticeNo') ?? ''
  },
  {
    title: t('entity.sourceec.sourceimplementation'),
    dataIndex: 'sourceImplementation',
    key: 'sourceImplementation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceImplementation') ?? ''
  },
  {
    title: t('entity.sourceec.sourcemainchangereason'),
    dataIndex: 'sourceMainChangeReason',
    key: 'sourceMainChangeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceMainChangeReason') ?? ''
  },
  {
    title: t('entity.sourceec.sourcesecondarychangereason'),
    dataIndex: 'sourceSecondaryChangeReason',
    key: 'sourceSecondaryChangeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSecondaryChangeReason') ?? ''
  },
  {
    title: t('entity.sourceec.sourcesafetyregulation'),
    dataIndex: 'sourceSafetyRegulation',
    key: 'sourceSafetyRegulation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSafetyRegulation') ?? ''
  },
  {
    title: t('entity.sourceec.sourceprogressstatus'),
    dataIndex: 'sourceProgressStatus',
    key: 'sourceProgressStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceProgressStatus') ?? ''
  },
  {
    title: t('entity.sourceec.sourceserialnumbercontrol'),
    dataIndex: 'sourceSerialNumberControl',
    key: 'sourceSerialNumberControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSerialNumberControl') ?? ''
  },
  {
    title: t('entity.sourceec.sourcecustomerapproval'),
    dataIndex: 'sourceCustomerApproval',
    key: 'sourceCustomerApproval',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceCustomerApproval') ?? ''
  },
  {
    title: t('entity.sourceec.sourceservicemanualrevision'),
    dataIndex: 'sourceServiceManualRevision',
    key: 'sourceServiceManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceServiceManualRevision') ?? ''
  },
  {
    title: t('entity.sourceec.sourceusermanualrevision'),
    dataIndex: 'sourceUserManualRevision',
    key: 'sourceUserManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceUserManualRevision') ?? ''
  },
  {
    title: t('entity.sourceec.sourcepromotionmanualrevision'),
    dataIndex: 'sourcePromotionManualRevision',
    key: 'sourcePromotionManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourcePromotionManualRevision') ?? ''
  },
  {
    title: t('entity.sourceec.sourcestandarddocumentrevision'),
    dataIndex: 'sourceStandardDocumentRevision',
    key: 'sourceStandardDocumentRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceStandardDocumentRevision') ?? ''
  },
  {
    title: t('entity.sourceec.sourceinformationrelease'),
    dataIndex: 'sourceInformationRelease',
    key: 'sourceInformationRelease',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceInformationRelease') ?? ''
  },
  {
    title: t('entity.sourceec.sourcecostchange'),
    dataIndex: 'sourceCostChange',
    key: 'sourceCostChange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceCostChange') ?? ''
  },
  {
    title: t('entity.sourceec.sourceunitcost'),
    dataIndex: 'sourceUnitCost',
    key: 'sourceUnitCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceUnitCost') ?? ''
  },
  {
    title: t('entity.sourceec.sourcemoldmodificationcost'),
    dataIndex: 'sourceMoldModificationCost',
    key: 'sourceMoldModificationCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceMoldModificationCost') ?? ''
  },
  {
    title: t('entity.sourceec.sourcerelateddrawing'),
    dataIndex: 'sourceRelatedDrawing',
    key: 'sourceRelatedDrawing',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceRelatedDrawing') ?? ''
  },
  {
    title: t('entity.sourceec.content'),
    dataIndex: 'sourceEcContent',
    key: 'sourceEcContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcContent') ?? ''
  },
  CreateActionColumn({
    width: 88,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'logistics:manufacturing:engineering:change:source:ec:query',
        onClick: (record: SourceEc) => void handleShowDetail(record),
      },
    ],
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSourceEcId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSourceEcField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SourceEc[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SourceEc, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSourceEcId(selectedRow.value) === getSourceEcId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SourceEc[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getSourceEcList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SourceEc] 加载数据失败', { error })
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
  sourceEcNo: '',
  sourceModel: '',
  sourceTitle: '',
  sourceStatus: '',
  sourceIssueDateStart: '',
  sourceIssueDateEnd: '',
  sourceTcjOwner: '',
  sourceTcjDependency: '',
  sourceEcMeeting: '',
  sourcePpNo: '',
  sourceTechnicalNoticeNo: '',
  sourceImplementation: '',
  sourceMainChangeReason: '',
  sourceSecondaryChangeReason: '',
  sourceSafetyRegulation: '',
  sourceProgressStatus: '',
  sourceSerialNumberControl: '',
  sourceCustomerApproval: '',
  sourceServiceManualRevision: '',
  sourceUserManualRevision: '',
  sourcePromotionManualRevision: '',
  sourceStandardDocumentRevision: '',
  sourceInformationRelease: '',
  sourceCostChange: '',
  sourceUnitCost: undefined as number | undefined,
  sourceMoldModificationCost: undefined as number | undefined,
  sourceRelatedDrawing: '',
  sourceEcContent: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开详情弹窗（含子表明细） */
async function handleShowDetail(record: SourceEc) {
  const id = getSourceEcId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    const detail = await loadSourceEcDetail(record)
    detailData.value = detail ? { ...detail } : { ...record }
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = null
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getSourceEcTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSourceEc(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
  if (selectedMasterKey.value) {
    sourceEcDetailPanelRef.value?.reload?.()
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
    const exportMeta = await exportSourceEc(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sourceec._self') }))
  } catch (error: any) {
    logger.error('[SourceEc] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.sourceec._self') }))
  } finally {
    loading.value = false
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
  sourceEcNo: '',
  sourceModel: '',
  sourceTitle: '',
  sourceStatus: '',
  sourceIssueDateStart: '',
  sourceIssueDateEnd: '',
  sourceTcjOwner: '',
  sourceTcjDependency: '',
  sourceEcMeeting: '',
  sourcePpNo: '',
  sourceTechnicalNoticeNo: '',
  sourceImplementation: '',
  sourceMainChangeReason: '',
  sourceSecondaryChangeReason: '',
  sourceSafetyRegulation: '',
  sourceProgressStatus: '',
  sourceSerialNumberControl: '',
  sourceCustomerApproval: '',
  sourceServiceManualRevision: '',
  sourceUserManualRevision: '',
  sourcePromotionManualRevision: '',
  sourceStandardDocumentRevision: '',
  sourceInformationRelease: '',
  sourceCostChange: '',
  sourceUnitCost: undefined as number | undefined,
  sourceMoldModificationCost: undefined as number | undefined,
  sourceRelatedDrawing: '',
  sourceEcContent: '',
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
