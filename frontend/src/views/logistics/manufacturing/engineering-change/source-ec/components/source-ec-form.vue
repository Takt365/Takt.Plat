<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/components -->
<!-- 文件名称：source-ec-form.vue -->
<!-- 功能描述：设变来源主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form source-ec-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    :disabled="loading || readOnly"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="source-ec-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.no')"
                name="sourceEcCode"
              >
                <a-input
                  v-model:value="formState.sourceEcCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.no') })"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcemodel')"
                name="sourceModel"
              >
                <a-input
                  v-model:value="formState.sourceModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemodel') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcetitle')"
                name="sourceTitle"
              >
                <a-input
                  v-model:value="formState.sourceTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetitle') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcestatus')"
                name="sourceStatus"
              >
                <a-input
                  v-model:value="formState.sourceStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcestatus') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceissuedate')"
                name="sourceIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.sourceIssueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceissuedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcetcjowner')"
                name="sourceTcjOwner"
              >
                <a-input
                  v-model:value="formState.sourceTcjOwner"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetcjowner') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcetcjdependency')"
                name="sourceTcjDependency"
              >
                <a-input
                  v-model:value="formState.sourceTcjDependency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetcjdependency') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.meeting')"
                name="sourceEcMeeting"
              >
                <a-input
                  v-model:value="formState.sourceEcMeeting"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.meeting') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceppCode')"
                name="sourcePpCode"
              >
                <a-input
                  v-model:value="formState.sourcePpCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceppCode') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcetechnicalnoticeCode')"
                name="sourceTechnicalNoticeCode"
              >
                <a-input
                  v-model:value="formState.sourceTechnicalNoticeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetechnicalnoticeCode') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceimplementation')"
                name="sourceImplementation"
              >
                <a-input
                  v-model:value="formState.sourceImplementation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceimplementation') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcemainchangereason')"
                name="sourceMainChangeReason"
              >
                <a-input
                  v-model:value="formState.sourceMainChangeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemainchangereason') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcesecondarychangereason')"
                name="sourceSecondaryChangeReason"
              >
                <a-input
                  v-model:value="formState.sourceSecondaryChangeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcesecondarychangereason') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcesafetyregulation')"
                name="sourceSafetyRegulation"
              >
                <a-input
                  v-model:value="formState.sourceSafetyRegulation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcesafetyregulation') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceprogressstatus')"
                name="sourceProgressStatus"
              >
                <a-input
                  v-model:value="formState.sourceProgressStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceprogressstatus') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceserialnumbercontrol')"
                name="sourceSerialNumberControl"
              >
                <a-input
                  v-model:value="formState.sourceSerialNumberControl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceserialnumbercontrol') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcecustomerapproval')"
                name="sourceCustomerApproval"
              >
                <a-input
                  v-model:value="formState.sourceCustomerApproval"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcecustomerapproval') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceservicemanualrevision')"
                name="sourceServiceManualRevision"
              >
                <a-input
                  v-model:value="formState.sourceServiceManualRevision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceservicemanualrevision') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceusermanualrevision')"
                name="sourceUserManualRevision"
              >
                <a-input
                  v-model:value="formState.sourceUserManualRevision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceusermanualrevision') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcepromotionmanualrevision')"
                name="sourcePromotionManualRevision"
              >
                <a-input
                  v-model:value="formState.sourcePromotionManualRevision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcepromotionmanualrevision') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcestandarddocumentrevision')"
                name="sourceStandardDocumentRevision"
              >
                <a-input
                  v-model:value="formState.sourceStandardDocumentRevision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcestandarddocumentrevision') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceinformationrelease')"
                name="sourceInformationRelease"
              >
                <a-input
                  v-model:value="formState.sourceInformationRelease"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceinformationrelease') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcecostchange')"
                name="sourceCostChange"
              >
                <a-input
                  v-model:value="formState.sourceCostChange"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcecostchange') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourceunitcost')"
                name="sourceUnitCost"
              >
                <a-input-number
                  v-model:value="formState.sourceUnitCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourceunitcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcemoldmodificationcost')"
                name="sourceMoldModificationCost"
              >
                <a-input-number
                  v-model:value="formState.sourceMoldModificationCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemoldmodificationcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceec.sourcerelateddrawing')"
                name="sourceRelatedDrawing"
              >
                <a-input
                  v-model:value="formState.sourceRelatedDrawing"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcerelateddrawing') })"
                  show-count
                  :maxlength="210"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.sourceec.content')"
                name="sourceEcContent"
              >
                <a-textarea
                  v-model:value="formState.sourceEcContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.sourceec.content') })"
                  :rows="10"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 sourceEcDetails -->
    <TaktEditableTable
      ref="sourceEcDetailTableRef"
      v-model="childSourceEcDetailRows"
      :columns="sourceEcDetailFormColumns"
      :title="t('entity.sourceecdetail._self')"
      :add-button-entity="t('entity.sourceecdetail._self')"
      id-field="sourceEcDetailId"
      :default-row="createEmptySourceEcDetailRow"
      :disabled="loading || readOnly"
      section-border
    >
      <template #cell-SourceCompatibility="{ record }">
        <TaktSelect
          v-model:value="record.SourceCompatibility"
          dict-type="logistics_ec_source_compatibility"
          allow-clear
          :disabled="loading || readOnly"
        />
      </template>
      <template #cell-sourceDistinction="{ record }">
        <TaktSelect
          v-model:value="record.sourceDistinction"
          dict-type="logistics_ec_source_distinction"
          allow-clear
          :disabled="loading || readOnly"
        />
      </template>
      <template #cell-sourceLegacyPartDisposition="{ record }">
        <TaktSelect
          v-model:value="record.sourceLegacyPartDisposition"
          dict-type="logistics_ec_legacy_part_disposition"
          allow-clear
          :disabled="loading || readOnly"
        />
      </template>
      <template #cell-SourceInstruction="{ record }">
        <TaktSelect
          v-model:value="record.SourceInstruction"
          dict-type="logistics_ec_source_instruction"
          allow-clear
          :disabled="loading || readOnly"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变来源主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/source-ec/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SourceEcCreate } from '@/types/logistics/manufacturing/engineering-change/source-ec'
import {
  buildSourceEcDetailEditableColumns,
  createEmptySourceEcDetailRow,
} from '../composables/use-source-ec-detail-fields'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SourceEcCreate & { sourceEcId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 详情只读模式（禁用全部字段） */
  readOnly?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  readOnly: false,
})

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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","sourceEcCode","sourceModel","sourceTitle","sourceStatus","sourceIssueDate","sourceTcjOwner","sourceTcjDependency","sourceEcMeeting","sourcePpCode","sourceTechnicalNoticeCode","sourceImplementation","sourceMainChangeReason","sourceSecondaryChangeReason","sourceSafetyRegulation","sourceProgressStatus","sourceSerialNumberControl","sourceCustomerApproval","sourceServiceManualRevision","sourceUserManualRevision","sourcePromotionManualRevision","sourceStandardDocumentRevision","sourceInformationRelease","sourceCostChange","sourceUnitCost","sourceMoldModificationCost","sourceRelatedDrawing","sourceEcContent"]

const childSourceEcDetailRows = ref<Record<string, unknown>[]>([])
const sourceEcDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 sourceEcDetail 可编辑列（与 source-ec-detail.d.ts 16 个业务字段对齐） */
const sourceEcDetailFormColumns = computed(() => buildSourceEcDetailEditableColumns(t, props.readOnly))

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SourceEcCreate & { sourceEcId?: string }> | null | undefined) {
  childSourceEcDetailRows.value = ((val as any)?.sourceEcDetails ?? []) as Record<string, unknown>[]
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sourceEcId ?? ''
  return {
    ...formState,
    sourceEcDetails: sourceEcDetailTableRef.value?.getRows?.() ?? childSourceEcDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      sourceEcId: masterId,
    })),
  }
}

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 sourceEcId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sourceEcId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).sourceEcDetails
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
    const isCreate = !props.formData?.sourceEcId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  sourceEcCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceec.no') }),
      trigger: 'blur'
    }
  ],
  sourceModel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcemodel') }),
      trigger: 'blur'
    }
  ],
  sourceTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcetitle') }),
      trigger: 'blur'
    }
  ],
  sourceStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceec.sourcestatus') }),
      trigger: 'blur'
    }
  ],
  sourceIssueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceissuedate') }),
      trigger: 'change'
    }
  ],
  sourceUnitCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceunitcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourceunitcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceMoldModificationCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourcemoldmodificationcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceec.sourcemoldmodificationcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceEcContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceec.content') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await sourceEcDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sourceUnitCost' in payload) {
    const rawsourceUnitCost = payload.sourceUnitCost
    payload.sourceUnitCost = typeof rawsourceUnitCost === 'number' ? rawsourceUnitCost : Number(rawsourceUnitCost)
  }
  if ('sourceMoldModificationCost' in payload) {
    const rawsourceMoldModificationCost = payload.sourceMoldModificationCost
    payload.sourceMoldModificationCost = typeof rawsourceMoldModificationCost === 'number' ? rawsourceMoldModificationCost : Number(rawsourceMoldModificationCost)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sourceEcId)
  childSourceEcDetailRows.value = []
  sourceEcDetailTableRef.value?.resetRows?.()
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
