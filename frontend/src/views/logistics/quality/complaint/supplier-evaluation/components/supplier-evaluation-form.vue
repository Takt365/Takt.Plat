<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-form.vue -->
<!-- 功能描述：供应商评价考核主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form supplier-evaluation-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="supplier-evaluation-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierEvaluationCode')"
                name="supplierEvaluationCode"
              >
                <a-input
                  v-model:value="formState.supplierEvaluationCode"
                  :placeholder="pi.ph('supplierEvaluationCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.supplierEvaluationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierId')"
                name="supplierId"
              >
                <TaktSelect
                  v-model:value="formState.supplierId"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierName1')"
                name="supplierName1"
              >
                <a-input
                  v-model:value="formState.supplierName1"
                  :placeholder="pi.ph('supplierName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.supplierEvaluationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationDate')"
                name="evaluationDate"
              >
                <a-date-picker
                  v-model:value="formState.evaluationDate"
                  :placeholder="pi.ph('evaluationDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationPeriod')"
                name="evaluationPeriod"
              >
                <TaktSelect
                  v-model:value="formState.evaluationPeriod"
                  dict-type="logistics_quality_period"
                  :placeholder="pi.ph('evaluationPeriod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationType')"
                name="evaluationType"
              >
                <a-input-number
                  v-model:value="formState.evaluationType"
                  :placeholder="pi.ph('evaluationType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluatorBy')"
                name="evaluatorBy"
              >
                <TaktSelect
                  v-model:value="formState.evaluatorBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('evaluatorBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationDept')"
                name="evaluationDept"
              >
                <TaktSelect
                  v-model:value="formState.evaluationDept"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('evaluationDept')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overallRating')"
                name="overallRating"
              >
                <TaktSelect
                  v-model:value="formState.overallRating"
                  dict-type="logistics_quality_supplier_rating"
                  :placeholder="pi.ph('overallRating')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalScore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="pi.ph('totalScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qualityScore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="pi.ph('qualityScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deliveryScore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="pi.ph('deliveryScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceScore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="pi.ph('priceScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceScore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="pi.ph('serviceScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('technicalScore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="pi.ph('technicalScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainStrengths')"
                name="mainStrengths"
              >
                <a-input
                  v-model:value="formState.mainStrengths"
                  :placeholder="pi.ph('mainStrengths')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mainIssues')"
                name="mainIssues"
              >
                <a-input
                  v-model:value="formState.mainIssues"
                  :placeholder="pi.ph('mainIssues')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('improvementRequirements')"
                name="improvementRequirements"
              >
                <a-input
                  v-model:value="formState.improvementRequirements"
                  :placeholder="pi.ph('improvementRequirements')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('evaluationConclusion')"
                name="evaluationConclusion"
              >
                <TaktSelect
                  v-model:value="formState.evaluationConclusion"
                  dict-type="logistics_quality_evaluation_conclusion"
                  :placeholder="pi.ph('evaluationConclusion')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('rectificationDeadline')"
                name="rectificationDeadline"
              >
                <a-date-picker
                  v-model:value="formState.rectificationDeadline"
                  :placeholder="pi.ph('rectificationDeadline')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="pi.ph('attachments')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('evaluationStatus')"
                name="evaluationStatus"
              >
                <TaktSelect
                  v-model:value="formState.evaluationStatus"
                  dict-type="logistics_quality_evaluation_status"
                  :placeholder="pi.ph('evaluationStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('relatedPlant')"
                name="relatedPlant"
              >
                <TaktSelect
                  v-model:value="formState.relatedPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('relatedPlant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('rectificationStatus')"
                name="rectificationStatus"
              >
                <TaktSelect
                  v-model:value="formState.rectificationStatus"
                  dict-type="logistics_quality_rectification_status"
                  :placeholder="pi.ph('rectificationStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="supplierEvaluationItemTableRef"
      v-model="childSupplierEvaluationItemRows"
      :columns="supplierEvaluationItemFormColumns"
      :title="supplierEvaluationItemPi.self()"
      :add-button-entity="supplierEvaluationItemPi.self()"
      id-field="supplierEvaluationItemId"
      :default-row="createDefaultSupplierEvaluationItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-evaluationId="{ record }">
        <TaktSelect
          v-model:value="record.evaluationId"
          api-url="TaktSupplierEvaluations/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="supplierEvaluationItemPi.queryPh('evaluationId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-categoryType="{ record }">
        <TaktSelect
          v-model:value="record.categoryType"
          dict-type="logistics_quality_evaluation_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="supplierEvaluationItemPi.ph('categoryType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-ratingLevel="{ record }">
        <TaktSelect
          v-model:value="record.ratingLevel"
          dict-type="logistics_quality_supplier_rating"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="supplierEvaluationItemPi.ph('ratingLevel')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-rectificationStatus="{ record }">
        <TaktSelect
          v-model:value="record.rectificationStatus"
          dict-type="logistics_quality_rectification_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="supplierEvaluationItemPi.ph('rectificationStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="supplierEvaluationItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 供应商评价考核主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSupplierEvaluationI18n } from '../composables/use-supplier-evaluation-i18n'

/** 实体字段 i18n */
const pi = useSupplierEvaluationI18n()

import type { SupplierEvaluationCreate } from '@/types/logistics/quality/complaint/supplier-evaluation'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","supplierEvaluationCode","supplierId","supplierName1","supplierCode","evaluationDate","evaluationPeriod","evaluationType","evaluatorBy","evaluationDept","overallRating","totalScore","qualityScore","deliveryScore","priceScore","serviceScore","technicalScore","mainStrengths","mainIssues","improvementRequirements","evaluationConclusion","rectificationDeadline","attachments","evaluationStatus","relatedPlant","rectificationStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useSupplierEvaluationItemI18n } from '../composables/use-supplier-evaluation-item-i18n'

const supplierEvaluationItemPi = useSupplierEvaluationItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSupplierEvaluationItemRows = ref<Record<string, unknown>[]>([])
const supplierEvaluationItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSupplierEvaluationItemRow(row: Record<string, unknown>): boolean {
  const id = row.supplierEvaluationItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSupplierEvaluationItemLineNumber(): number {
  const rows = supplierEvaluationItemTableRef.value?.getRows?.() ?? childSupplierEvaluationItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 supplierEvaluationItem 可编辑列 */
const supplierEvaluationItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'evaluationId',
    title: supplierEvaluationItemPi.label('evaluationId'),
    width: 140,
  },
  {
    key: 'lineNumber',
    title: supplierEvaluationItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'categoryType',
    title: supplierEvaluationItemPi.label('categoryType'),
    width: 140,
  },
  {
    key: 'itemName',
    title: supplierEvaluationItemPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemDescription',
    title: supplierEvaluationItemPi.label('itemDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: supplierEvaluationItemPi.ph('itemDescription'),
    width: 180,
  },
  {
    key: 'weight',
    title: supplierEvaluationItemPi.label('weight'),
    width: 140,
  },
  {
    key: 'scoringStandard',
    title: supplierEvaluationItemPi.label('scoringStandard'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: supplierEvaluationItemPi.ph('scoringStandard'),
  },
  {
    key: 'score',
    title: supplierEvaluationItemPi.label('score'),
    width: 140,
  },
  {
    key: 'ratingLevel',
    title: supplierEvaluationItemPi.label('ratingLevel'),
    width: 140,
  },
  {
    key: 'evaluationComment',
    title: supplierEvaluationItemPi.label('evaluationComment'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: supplierEvaluationItemPi.ph('evaluationComment'),
  },
  {
    key: 'existingIssues',
    title: supplierEvaluationItemPi.label('existingIssues'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: supplierEvaluationItemPi.ph('existingIssues'),
  },
  {
    key: 'improvementRequirement',
    title: supplierEvaluationItemPi.label('improvementRequirement'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: supplierEvaluationItemPi.ph('improvementRequirement'),
  },
  {
    key: 'rectificationRequired',
    title: supplierEvaluationItemPi.label('rectificationRequired'),
    width: 140,
  },
  {
    key: 'rectificationDeadline',
    title: supplierEvaluationItemPi.label('rectificationDeadline'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'rectificationStatus',
    title: supplierEvaluationItemPi.label('rectificationStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: supplierEvaluationItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null | undefined) {
  const rows_supplierEvaluationItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSupplierEvaluationItemRows.value = rows_supplierEvaluationItem
}

function createDefaultSupplierEvaluationItemRow(): Record<string, unknown> {
  return {
    evaluationId: '',
    lineNumber: allocateNextSupplierEvaluationItemLineNumber(),
    categoryType: 0,
    itemName: '',
    itemDescription: '',
    weight: 0,
    scoringStandard: '',
    score: 0,
    ratingLevel: 0,
    evaluationComment: '',
    existingIssues: '',
    improvementRequirement: '',
    rectificationRequired: 0,
    rectificationDeadline: '',
    rectificationStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.supplierEvaluationId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: supplierEvaluationItemTableRef.value?.getRows?.() ?? childSupplierEvaluationItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        supplierEvaluationCode: masterId,
      }
      if (isUpdate && isPersistedSupplierEvaluationItemRow(row)) {
        normalized.supplierEvaluationItemId = row.supplierEvaluationItemId
      } else {
        delete normalized.supplierEvaluationItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SupplierEvaluationCreate & { supplierEvaluationId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  evaluationPeriod: 1,
  evaluationConclusion: 0,
  evaluationStatus: 0,
  rectificationStatus: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 supplierEvaluationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.supplierEvaluationId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.supplierEvaluationId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  supplierEvaluationCode: [
    {
      required: true,
      message: pi.ph('supplierEvaluationCode'),
      trigger: 'blur'
    }
  ],
  supplierId: [
    {
      required: true,
      message: pi.ph('supplierId'),
      trigger: 'change'
    }
  ],
  supplierName1: [
    {
      required: true,
      message: pi.ph('supplierName1'),
      trigger: 'blur'
    }
  ],
  evaluationDate: [
    {
      required: true,
      message: pi.ph('evaluationDate'),
      trigger: 'change'
    }
  ],
  evaluationPeriod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationPeriod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationPeriod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overallRating: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overallRating'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overallRating'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationConclusion: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationConclusion'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationConclusion'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  relatedPlant: [
    {
      required: true,
      message: pi.ph('relatedPlant'),
      trigger: 'change'
    }
  ],
  rectificationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('rectificationStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('rectificationStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await supplierEvaluationItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('evaluationPeriod' in payload) {
    const rawevaluationPeriod = payload.evaluationPeriod
    payload.evaluationPeriod = typeof rawevaluationPeriod === 'number' ? rawevaluationPeriod : Number(rawevaluationPeriod)
  }
  if ('evaluationType' in payload) {
    const rawevaluationType = payload.evaluationType
    payload.evaluationType = typeof rawevaluationType === 'number' ? rawevaluationType : Number(rawevaluationType)
  }
  if ('overallRating' in payload) {
    const rawoverallRating = payload.overallRating
    payload.overallRating = typeof rawoverallRating === 'number' ? rawoverallRating : Number(rawoverallRating)
  }
  if ('totalScore' in payload) {
    const rawtotalScore = payload.totalScore
    payload.totalScore = typeof rawtotalScore === 'number' ? rawtotalScore : Number(rawtotalScore)
  }
  if ('qualityScore' in payload) {
    const rawqualityScore = payload.qualityScore
    payload.qualityScore = typeof rawqualityScore === 'number' ? rawqualityScore : Number(rawqualityScore)
  }
  if ('deliveryScore' in payload) {
    const rawdeliveryScore = payload.deliveryScore
    payload.deliveryScore = typeof rawdeliveryScore === 'number' ? rawdeliveryScore : Number(rawdeliveryScore)
  }
  if ('priceScore' in payload) {
    const rawpriceScore = payload.priceScore
    payload.priceScore = typeof rawpriceScore === 'number' ? rawpriceScore : Number(rawpriceScore)
  }
  if ('serviceScore' in payload) {
    const rawserviceScore = payload.serviceScore
    payload.serviceScore = typeof rawserviceScore === 'number' ? rawserviceScore : Number(rawserviceScore)
  }
  if ('technicalScore' in payload) {
    const rawtechnicalScore = payload.technicalScore
    payload.technicalScore = typeof rawtechnicalScore === 'number' ? rawtechnicalScore : Number(rawtechnicalScore)
  }
  if ('evaluationConclusion' in payload) {
    const rawevaluationConclusion = payload.evaluationConclusion
    payload.evaluationConclusion = typeof rawevaluationConclusion === 'number' ? rawevaluationConclusion : Number(rawevaluationConclusion)
  }
  if ('evaluationStatus' in payload) {
    const rawevaluationStatus = payload.evaluationStatus
    payload.evaluationStatus = typeof rawevaluationStatus === 'number' ? rawevaluationStatus : Number(rawevaluationStatus)
  }
  if ('rectificationStatus' in payload) {
    const rawrectificationStatus = payload.rectificationStatus
    payload.rectificationStatus = typeof rawrectificationStatus === 'number' ? rawrectificationStatus : Number(rawrectificationStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.supplierEvaluationId)
  childSupplierEvaluationItemRows.value = []
  supplierEvaluationItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
