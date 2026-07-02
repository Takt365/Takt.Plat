<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/staffing-requirement -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：用人需求管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="human:resource:talent:staffing:requirement:create"
      update-permission="human:resource:talent:staffing:requirement:update"
      delete-permission="human:resource:talent:staffing:requirement:delete"
      import-permission="human:resource:talent:staffing:requirement:import"
      export-permission="human:resource:talent:staffing:requirement:export"
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

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="approval"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'talentStaffingRequirementId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTalentStaffingRequirementId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
      <TalentStaffingRequirementForm
        :key="formData?.talentStaffingRequirementId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-talent-staffing-requirement'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('reqNo')">
      <a-form-item :label="t('entity.talentstaffingrequirement.reqno')">
        <a-input
          v-model:value="advancedQueryForm.reqNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reqno') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.talentstaffingrequirement.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.deptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postId')">
      <a-form-item :label="t('entity.talentstaffingrequirement.postid')">
        <a-input
          v-model:value="advancedQueryForm.postId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.postid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGrade')">
      <a-form-item :label="t('entity.talentstaffingrequirement.jobgrade')">
        <a-input
          v-model:value="advancedQueryForm.jobGrade"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.jobgrade') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestQty')">
      <a-form-item :label="t('entity.talentstaffingrequirement.requestqty')">
        <a-input-number
          v-model:value="advancedQueryForm.requestQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.requestqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headcountType')">
      <a-form-item :label="t('entity.talentstaffingrequirement.headcounttype')">
        <a-input
          v-model:value="advancedQueryForm.headcountType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.headcounttype') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reasonCode')">
      <a-form-item :label="t('entity.talentstaffingrequirement.reasoncode')">
        <a-input
          v-model:value="advancedQueryForm.reasonCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.reasoncode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('replaceEmployeeId')">
      <a-form-item :label="t('entity.talentstaffingrequirement.replaceemployeeid')">
        <a-input
          v-model:value="advancedQueryForm.replaceEmployeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.replaceemployeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedOnboardDateStart')">
      <a-form-item :label="t('entity.talentstaffingrequirement.expectedonboarddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedOnboardDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.expectedonboarddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedOnboardDateEnd')">
      <a-form-item :label="t('entity.talentstaffingrequirement.expectedonboarddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedOnboardDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.expectedonboarddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractType')">
      <a-form-item :label="t('entity.talentstaffingrequirement.contracttype')">
        <a-input
          v-model:value="advancedQueryForm.contractType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.contracttype') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workLocation')">
      <a-form-item :label="t('entity.talentstaffingrequirement.worklocation')">
        <a-input
          v-model:value="advancedQueryForm.workLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.worklocation') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobDesc')">
      <a-form-item :label="t('entity.talentstaffingrequirement.jobdesc')">
        <a-input
          v-model:value="advancedQueryForm.jobDesc"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.jobdesc') })"
          show-count
          :maxlength="4000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualification')">
      <a-form-item :label="t('entity.talentstaffingrequirement.qualification')">
        <a-input
          v-model:value="advancedQueryForm.qualification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.qualification') })"
          show-count
          :maxlength="4000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetYear')">
      <a-form-item :label="t('entity.talentstaffingrequirement.budgetyear')">
        <a-input
          v-model:value="advancedQueryForm.budgetYear"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.budgetyear') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.talentstaffingrequirement.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.talentstaffingrequirement.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.talentstaffingrequirement.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.talentstaffingrequirement.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.talentstaffingrequirement.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.talentstaffingrequirement.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.talentstaffingrequirement.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentstaffingrequirement.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.talentstaffingrequirement.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentstaffingrequirement.flowinstanceid') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.talentstaffingrequirement._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.talentstaffingrequirement._self"
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
      :id-column-key="'talentStaffingRequirementId'"
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
 * 用人需求管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/talent/staffing-requirement
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import TalentStaffingRequirementForm from './components/staffing-requirement-form.vue'
import { getTalentStaffingRequirementList, getTalentStaffingRequirementById, createTalentStaffingRequirement, updateTalentStaffingRequirement, deleteTalentStaffingRequirementById, deleteTalentStaffingRequirementBatch, getTalentStaffingRequirementTemplate, importTalentStaffingRequirement, exportTalentStaffingRequirement } from '@/api/human-resource/talent/staffing-requirement'
import type { TalentStaffingRequirement, TalentStaffingRequirementQuery } from '@/types/human-resource/talent/staffing-requirement'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTalentStaffingRequirement')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.talentstaffingrequirement._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<TalentStaffingRequirement[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<TalentStaffingRequirement | null>(null)
/** 表格多选行 */
const selectedRows = ref<TalentStaffingRequirement[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<TalentStaffingRequirement> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  reqNo: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'reqNo', label: t('entity.talentstaffingrequirement.reqno') },
  { key: 'deptId', label: t('entity.talentstaffingrequirement.deptid') },
  { key: 'postId', label: t('entity.talentstaffingrequirement.postid') },
  { key: 'jobGrade', label: t('entity.talentstaffingrequirement.jobgrade') },
  { key: 'requestQty', label: t('entity.talentstaffingrequirement.requestqty') },
  { key: 'headcountType', label: t('entity.talentstaffingrequirement.headcounttype') },
  { key: 'reasonCode', label: t('entity.talentstaffingrequirement.reasoncode') },
  { key: 'replaceEmployeeId', label: t('entity.talentstaffingrequirement.replaceemployeeid') },
  { key: 'expectedOnboardDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.talentstaffingrequirement.expectedonboarddate')) },
  { key: 'expectedOnboardDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.talentstaffingrequirement.expectedonboarddate')) },
  { key: 'contractType', label: t('entity.talentstaffingrequirement.contracttype') },
  { key: 'workLocation', label: t('entity.talentstaffingrequirement.worklocation') },
  { key: 'jobDesc', label: t('entity.talentstaffingrequirement.jobdesc') },
  { key: 'qualification', label: t('entity.talentstaffingrequirement.qualification') },
  { key: 'budgetYear', label: t('entity.talentstaffingrequirement.budgetyear') },
  { key: 'approvalStatus', label: t('entity.talentstaffingrequirement.approvalstatus') },
  { key: 'initiatorId', label: t('entity.talentstaffingrequirement.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.talentstaffingrequirement.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.talentstaffingrequirement.initiatedatend') },
  { key: 'approvedBy', label: t('entity.talentstaffingrequirement.approvedby') },
  { key: 'approvedAtStart', label: t('entity.talentstaffingrequirement.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.talentstaffingrequirement.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.talentstaffingrequirement.flowinstanceid') },
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
const entityIdName = 'talentStaffingRequirementId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TalentStaffingRequirementQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<TalentStaffingRequirementQuery>): TalentStaffingRequirementQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TalentStaffingRequirementQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof TalentStaffingRequirementQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('reqNo', form.reqNo)
  assignTrimmed('deptId', form.deptId)
  assignTrimmed('postId', form.postId)
  assignTrimmed('jobGrade', form.jobGrade)
  if (form.requestQty !== undefined && form.requestQty !== null) {
    query.requestQty = form.requestQty
  }
  assignTrimmed('headcountType', form.headcountType)
  assignTrimmed('reasonCode', form.reasonCode)
  assignTrimmed('replaceEmployeeId', form.replaceEmployeeId)
  assignTrimmed('expectedOnboardDateStart', form.expectedOnboardDateStart)
  assignTrimmed('expectedOnboardDateEnd', form.expectedOnboardDateEnd)
  assignTrimmed('contractType', form.contractType)
  assignTrimmed('workLocation', form.workLocation)
  assignTrimmed('jobDesc', form.jobDesc)
  assignTrimmed('qualification', form.qualification)
  assignTrimmed('budgetYear', form.budgetYear)
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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







/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'talentStaffingRequirementId',
    key: 'talentStaffingRequirementId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'talentStaffingRequirementId') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.reqno'),
    dataIndex: 'reqNo',
    key: 'reqNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'reqNo') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.postid'),
    dataIndex: 'postId',
    key: 'postId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'postId') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.jobgrade'),
    dataIndex: 'jobGrade',
    key: 'jobGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'jobGrade') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.requestqty'),
    dataIndex: 'requestQty',
    key: 'requestQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'requestQty') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.headcounttype'),
    dataIndex: 'headcountType',
    key: 'headcountType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'headcountType') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.reasoncode'),
    dataIndex: 'reasonCode',
    key: 'reasonCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'reasonCode') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.replaceemployeeid'),
    dataIndex: 'replaceEmployeeId',
    key: 'replaceEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'replaceEmployeeId') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.expectedonboarddate'),
    dataIndex: 'expectedOnboardDate',
    key: 'expectedOnboardDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'expectedOnboardDate') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.contracttype'),
    dataIndex: 'contractType',
    key: 'contractType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'contractType') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.worklocation'),
    dataIndex: 'workLocation',
    key: 'workLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'workLocation') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.jobdesc'),
    dataIndex: 'jobDesc',
    key: 'jobDesc',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'jobDesc') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.qualification'),
    dataIndex: 'qualification',
    key: 'qualification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'qualification') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.budgetyear'),
    dataIndex: 'budgetYear',
    key: 'budgetYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'budgetYear') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.dept'),
    dataIndex: 'dept',
    key: 'dept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'dept') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.post'),
    dataIndex: 'post',
    key: 'post',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'post') ?? ''
  },
  {
    title: t('entity.talentstaffingrequirement.replaceemployee'),
    dataIndex: 'replaceEmployee',
    key: 'replaceEmployee',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'replaceEmployee') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:talent:staffing:requirement:update',
        onClick: (record: TalentStaffingRequirement) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:talent:staffing:requirement:delete',
        onClick: (record: TalentStaffingRequirement) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTalentStaffingRequirementId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTalentStaffingRequirementField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TalentStaffingRequirement[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TalentStaffingRequirement, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getTalentStaffingRequirementId(selectedRow.value) === getTalentStaffingRequirementId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TalentStaffingRequirement[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: TalentStaffingRequirement) => ({
  onClick: () => {
    const key = getTalentStaffingRequirementId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTalentStaffingRequirementId(item)))
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
    const res = await getTalentStaffingRequirementList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TalentStaffingRequirement] 加载数据失败', { error })
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
  reqNo: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.talentstaffingrequirement._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: TalentStaffingRequirement) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.talentstaffingrequirement._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.talentstaffingrequirement._self') }))
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
      await updateTalentStaffingRequirement(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.talentstaffingrequirement._self') }))
    } else {
      await createTalentStaffingRequirement(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.talentstaffingrequirement._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getTalentStaffingRequirementTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTalentStaffingRequirement(file, sheetName)
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
    const exportMeta = await exportTalentStaffingRequirement(
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
    message.success(t('common.feedback.export.success', { target: t('entity.talentstaffingrequirement._self') }))
  } catch (error: any) {
    logger.error('[TalentStaffingRequirement] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.talentstaffingrequirement._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: TalentStaffingRequirement) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.talentstaffingrequirement._self'), name: t('common.tip.this.target', { target: t('entity.talentstaffingrequirement._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTalentStaffingRequirementById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.talentstaffingrequirement._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.talentstaffingrequirement._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.talentstaffingrequirement._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTalentStaffingRequirementBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.talentstaffingrequirement._self') }))
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
  reqNo: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
