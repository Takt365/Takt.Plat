<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output/components -->
<!-- 文件名称：assy-output-form.vue -->
<!-- 功能描述：组立日报维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-output-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    :disabled="loading || isMasterProdDateLocked"
  >
    <a-alert
      v-if="isMasterProdDateLocked"
      type="warning"
      show-icon
      class="mb-3 shrink-0"
      :message="prodDateLockedAlertMessage"
    />
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-output-form-tabs shrink-0"
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
                :label="pi.label('plantCode')"
                name="plantCode"
                :required="formItemRequired('plantCode')"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodCategory')"
                name="prodCategory"
                :required="formItemRequired('prodCategory')"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_prod_category"
                  :placeholder="pi.ph('prodCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
                :required="formItemRequired('prodDate')"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  :disabled-date="prodDatePickerDisabledDate"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodTeam')"
                name="prodTeam"
                :required="formItemRequired('prodTeam')"
              >
                <TaktSelect
                  v-model:value="formState.prodTeam"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('prodTeam')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('directLabor')"
                name="directLabor"
                :required="formItemRequired('directLabor')"
              >
                <a-input-number
                  v-model:value="formState.directLabor"
                  :placeholder="pi.ph('directLabor')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectLabor')"
                name="indirectLabor"
                :required="formItemRequired('indirectLabor')"
              >
                <a-input-number
                  v-model:value="formState.indirectLabor"
                  :placeholder="pi.ph('indirectLabor')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
                :required="formItemRequired('shiftNo')"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderType')"
                name="prodOrderType"
                :required="formItemRequired('prodOrderType')"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="pi.ph('prodOrderType')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
                :required="formItemRequired('prodOrderCode')"
              >
                <TaktSelect
                  v-model:value="formState.prodOrderCode"
                  api-url="TaktProductionOrders/options"
                  :api-params="prodOrderSelectApiParams"
                  remote-search
                  virtual
                  :placeholder="pi.ph('prodOrderCode')"
                  :disabled="!!formData?.assyOutputId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelCode')"
                name="modelCode"
                :required="formItemRequired('modelCode')"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="pi.ph('modelCode')"
                  show-count
                  :maxlength="20"
                  disabled
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
                :label="pi.label('materialCode')"
                name="materialCode"
                :required="formItemRequired('materialCode')"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchNo')"
                name="batchNo"
                :required="formItemRequired('batchNo')"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="pi.ph('batchNo')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderQty')"
                name="prodOrderQty"
                :required="formItemRequired('prodOrderQty')"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="pi.ph('prodOrderQty')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNo')"
                name="serialNo"
                :required="formItemRequired('serialNo')"
              >
                <a-input
                  v-model:value="formState.serialNo"
                  :placeholder="pi.ph('serialNo')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdMinutes')"
                name="stdMinutes"
                :required="formItemRequired('stdMinutes')"
              >
                <a-input-number
                  v-model:value="formState.stdMinutes"
                  :placeholder="pi.ph('stdMinutes')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                name="stdCapacity"
                :required="formItemRequired('stdCapacity')"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="pi.stdCapacityHint()"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('stdCapacity') }}</span>
                  </span>
                </template>
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="pi.ph('stdCapacity')"
                  :precision="2"
                  style="width: 100%"
                  disabled
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
                :required="formItemRequired('tenantCode')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
                :required="formItemRequired('companyCode')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
                :required="formItemRequired('companyDefaultCulture')"
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
                :required="formItemRequired('extField')"
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
                :required="formItemRequired('remark')"
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
    <!-- 下：子表 assyOutputDetails -->
    <TaktEditableTable
      ref="assyOutputDetailTableRef"
      v-model="childAssyOutputDetailRows"
      :columns="assyOutputDetailFormColumns"
      :title="assyOutputDetailPi.self()"
      id-field="assyOutputDetailId"
      :default-row="createDefaultAssyOutputDetailRow"
      :disabled="loading || isMasterProdDateLocked"
      :show-add="false"
      :show-delete="false"
      :enable-vertical-scroll="false"
      :min-rows="1"
      section-border
      @cell-value-change="handleDetailCellValueChange"
    >
      <template #cell-downtimeReason="{ record }">
        <TaktSelect
          :model-value="getDetailDictMultiSelectModelValue(record, 'downtimeReason')"
          dict-type="logistics_stop_reason_category"
          multiple
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyOutputDetailPi.ph('downtimeReason')"
          :disabled="loading || isMasterProdDateLocked"
          @update:model-value="(v) => applyDetailDictMultiChange(record, 'downtimeReason', v)"
        />
      </template>
      <template #cell-unachievedReason="{ record }">
        <TaktSelect
          :model-value="getDetailDictMultiSelectModelValue(record, 'unachievedReason')"
          dict-type="logistics_nonachievement_reason_category"
          multiple
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyOutputDetailPi.ph('unachievedReason')"
          :disabled="loading || isMasterProdDateLocked"
          @update:model-value="(v) => applyDetailDictMultiChange(record, 'unachievedReason', v)"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立日报维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output/components
 */
import { reactive, watch, computed, ref, onMounted, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyOutputI18n } from '../composables/use-assy-output-i18n'
import {
  assyOutputProdDatePickerDisabledDate,
  isAssyOutputProdDateLocked,
  isAssyOutputProdDateSelectable,
} from '../composables/takt-assy-output-prod-date-edit-lock'
import {
  calculateAssyOutputDetailDerived,
  resolvePersonnelOperationRatePercent,
  resolveStdMinutesByMaterial,
} from '../composables/use-assy-output-derived-calc'
import { useAssyOutputDetailDictMultiFormat } from '../composables/use-assy-output-detail-dict-multi-format'
import { useAssyOutputDetailEditableDict } from '../composables/use-assy-output-detail-editable-dict'
import { useAssyOutputDetailFormColumns } from '../composables/use-assy-output-detail-form-columns'
import { getAssyOutputDefaultTimePeriods } from '@/api/logistics/manufacturing/output/assy-output'
import { getProductionOrderByCode } from '@/api/logistics/manufacturing/aps/production-order'
import { getModelDestinationByMaterial } from '@/api/logistics/materials/model-destination'
import {
  applyAssyCleaningPeriodDefaults,
  ASSY_CLEANING_STOP_REASON_DICT_VALUE,
  ASSY_CLEANING_STOP_REASON_LABEL,
  calculateAssyStdCapacity,
  isAssyCleaningTimePeriod,
  normalizeAssyTimePeriod,
} from '@/utils/takt-production-stat'

/** 实体字段 i18n */
const pi = useAssyOutputI18n()

import type { AssyOutputCreate } from '@/types/logistics/manufacturing/output/assy-output'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()
const {
  parseDowntimeReasonForSelect,
  sortDowntimeReasonValues,
} = useAssyOutputDetailDictMultiFormat()
const {
  ensureDetailDictMultiFields,
  getDetailDictMultiSelectModelValue,
  applyDetailDictMultiChange,
  normalizeAssyOutputDetailRowForSubmit,
} = useAssyOutputDetailEditableDict()

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodTeam","directLabor","indirectLabor","shiftNo","prodOrderType","prodOrderCode","modelCode","materialCode","batchNo","prodOrderQty","serialNo","stdMinutes","stdCapacity","extField","remark"]

/** 非必填主表字段（不显示红 *） */
const OPTIONAL_FORM_FIELDS = new Set(['extField', 'remark'])

/** 是否显示表单项红 *（除 extField、remark 外全部必填） */
function formItemRequired(name: string): boolean {
  return !OPTIONAL_FORM_FIELDS.has(name)
}



import { useAssyOutputDetailI18n } from '../composables/use-assy-output-detail-i18n'

const assyOutputDetailPi = useAssyOutputDetailI18n()
const assyOutputDetailFormColumns = useAssyOutputDetailFormColumns()

const childAssyOutputDetailRows = ref<Record<string, unknown>[]>([])
const assyOutputDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 明细标准产能折算用标准生产稼动率(%) */
const masterOperationRatePercent = ref(0)
/** 工单回填进行中：跳过依赖 watch 的中间态刷新 */
const isBackfillingFromOrder = ref(false)
/** 主表派生刷新序号：丢弃过期异步结果 */
let masterDerivedRefreshSeq = 0

/** 主表生产日期是否已锁定（次月 cutoff 日之后不可改） */
const isMasterProdDateLocked = computed(() =>
  isAssyOutputProdDateLocked(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 锁定提示文案 */
const prodDateLockedAlertMessage = computed(() =>
  pi.prodDateLockedMessage(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 生产日期不可选已锁定日期 */
function prodDatePickerDisabledDate(current: Parameters<typeof assyOutputProdDatePickerDisabledDate>[0]) {
  return assyOutputProdDatePickerDisabledDate(current)
}

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

/** 清洁时段停线默认值 + 停线原因多选字典值（TaktSelect 绑定 DictValue 数组） */
function applyAssyCleaningPeriodDefaultsForEditableRow(
  row: Record<string, unknown>,
  directLabor: number,
) {
  applyAssyCleaningPeriodDefaults(row, directLabor)
  if (!isAssyCleaningTimePeriod(String(row.timePeriod ?? ''))) {
    return
  }
  const prodQty = Number(row.prodActualQty) || 0
  if (prodQty > 0) {
    const parsed = parseDowntimeReasonForSelect(ASSY_CLEANING_STOP_REASON_LABEL)
    row.downtimeReason = sortDowntimeReasonValues(
      parsed.length > 0 ? parsed : [ASSY_CLEANING_STOP_REASON_DICT_VALUE],
    )
  } else {
    delete row.downtimeReason
  }
  ensureDetailDictMultiFields(row)
}

/** 灌入子表行时补齐可编辑文本字段，多选字典按 sortOrder 排序展示 */
function normalizeChildDetailRow(row: Record<string, unknown>): Record<string, unknown> {
  const normalized: Record<string, unknown> = {
    ...row,
    timePeriod: normalizeAssyTimePeriod(String(row.timePeriod ?? '')),
    prodOrderCode: row.prodOrderCode ?? String(formState.prodOrderCode ?? ''),
    downtimeDescription: row.downtimeDescription ?? '',
    unachievedDescription: row.unachievedDescription ?? '',
  }
  ensureDetailDictMultiFields(normalized)
  return normalized
}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<AssyOutputCreate & { assyOutputId?: string }> | null | undefined) {
  const rows = ((val as any)?.assyOutputDetails ?? []) as Record<string, unknown>[]
  const directLabor = Number(formState.directLabor) || 0
  childAssyOutputDetailRows.value = rows.map((row) => {
    const normalized = normalizeChildDetailRow(row)
    applyAssyCleaningPeriodDefaultsForEditableRow(normalized, directLabor)
    return normalized
  })
}

function createDefaultAssyOutputDetailRow(timePeriod = ''): Record<string, unknown> {
  const row: Record<string, unknown> = {
    prodOrderCode: String(formState.prodOrderCode ?? ''),
    lineNumber: (childAssyOutputDetailRows.value.length + 1) * 10,
    timePeriod: normalizeAssyTimePeriod(timePeriod),
    stdCapacity: Number(formState.stdCapacity) || 0,
    prodActualQty: 0,
    downtimeMinutes: 0,
    downtimeDescription: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    inputMinutes: 0,
    actualMinutes: 0,
    indirectMinutes: 0,
    mixedProd: 0,
    achievementRate: 0,
  }
  ensureDetailDictMultiFields(row)
  return row
}

/** 按工厂与生产日期解析标准生产稼动率(%)，并同步重算标准产能 */
async function refreshMasterOperationRatePercent() {
  const plantCode = String(formState.plantCode ?? '').trim()
  const prodDate = String(formState.prodDate ?? '').trim()
  if (!plantCode || !prodDate) {
    masterOperationRatePercent.value = 0
    refreshMasterStdCapacityOnly()
    return
  }
  try {
    masterOperationRatePercent.value = await resolvePersonnelOperationRatePercent(plantCode, prodDate)
  } catch {
    masterOperationRatePercent.value = 0
  }
  refreshMasterStdCapacityOnly()
}

/** 主表标准产能：本地计算（直接人员 × 标准工时 × 已缓存稼动率） */
function refreshMasterStdCapacityOnly() {
  const directLabor = Number(formState.directLabor) || 0
  const stdMinutes = Number(formState.stdMinutes) || 0
  formState.stdCapacity = calculateAssyStdCapacity(directLabor, stdMinutes, masterOperationRatePercent.value)
  refreshChildDetailCalculatedFields()
  void initDefaultDetailRowsIfNeeded()
}

/** 主表标准工时：GET TaktStandardOperationTimes/by-material（与稼动率无关） */
async function refreshMasterStdMinutes() {
  const seq = ++masterDerivedRefreshSeq
  const materialCode = String(formState.materialCode ?? '').trim()
  const plantCode = String(formState.plantCode ?? '').trim()
  const prodDate = String(formState.prodDate ?? '').trim()
  if (!materialCode || !prodDate) {
    if (seq !== masterDerivedRefreshSeq) {
      return
    }
    formState.stdMinutes = 0
    refreshMasterStdCapacityOnly()
    return
  }
  try {
    const stdMinutes = await resolveStdMinutesByMaterial(materialCode, plantCode, prodDate)
    if (seq !== masterDerivedRefreshSeq) {
      return
    }
    formState.stdMinutes = stdMinutes
  } catch {
    if (seq !== masterDerivedRefreshSeq) {
      return
    }
    formState.stdMinutes = 0
  }
  refreshMasterStdCapacityOnly()
}

/** 主表派生：标准工时（API）+ 标准产能（本地，含稼动率） */
async function refreshMasterDerivedFields() {
  await refreshMasterStdMinutes()
  await initDefaultDetailRowsIfNeeded()
}

/** 刷新子表派生字段（仅改 TaktEditableTable 内部行，编辑态不回写 v-model，避免整表重建丢输入） */
function refreshChildDetailCalculatedFields() {
  const table = assyOutputDetailTableRef.value as {
    forEachRow?: (fn: (row: Record<string, unknown>) => void) => void
  } | null
  if (table?.forEachRow) {
    table.forEachRow((row) => refreshChildDetailDerivedRow(row))
    return
  }
  childAssyOutputDetailRows.value.forEach((row) => refreshChildDetailDerivedRow(row))
}

/** 刷新单条子表派生字段（清洁时段停线 + 投入/实际工时/标准产能/达成率） */
function refreshChildDetailDerivedRow(row: Record<string, unknown>) {
  ensureDetailDictMultiFields(row)
  const master = {
    directLabor: Number(formState.directLabor) || 0,
    indirectLabor: Number(formState.indirectLabor) || 0,
    stdCapacity: Number(formState.stdCapacity) || 0,
    stdMinutes: Number(formState.stdMinutes) || 0,
    operationRatePercent: masterOperationRatePercent.value,
  }
  const isCreate = !props.formData?.assyOutputId
  const directLabor = Number(formState.directLabor) || 0
  applyAssyCleaningPeriodDefaultsForEditableRow(row, directLabor)
  const mixedProd = isCreate ? 0 : (Number(row.mixedProd) || 0)
  const derived = calculateAssyOutputDetailDerived(master, {
    prodActualQty: Number(row.prodActualQty) || 0,
    downtimeMinutes: Number(row.downtimeMinutes) || 0,
    confirmMinutes: Number(row.confirmMinutes) || 0,
    mixedProd,
  })
  row.inputMinutes = derived.inputMinutes
  row.actualMinutes = derived.actualMinutes
  row.indirectMinutes = derived.indirectMinutes
  row.stdCapacity = derived.stdCapacity
  row.achievementRate = derived.achievementRate
  if (isCreate) {
    row.mixedProd = 0
  }
  if (formState.prodOrderCode) {
    row.prodOrderCode = formState.prodOrderCode
  }
  ensureDetailDictMultiFields(row)
}

/** 子表派生刷新 debounce 定时器（避免 stepper 点击时同步重算导致整行重渲染） */
let detailDerivedRefreshTimer: ReturnType<typeof setTimeout> | undefined
/** 待刷新派生字段的子表行 */
let detailDerivedRefreshRecord: Record<string, unknown> | null = null

/** 子表需触发派生重算的数值列 */
const DETAIL_DERIVED_VALUE_COLUMNS = new Set(['prodActualQty', 'confirmMinutes', 'downtimeMinutes'])

/** 子表内置编辑器值变更：debounce 后刷新派生字段（不 sync 父级 v-model） */
function handleDetailCellValueChange(payload: {
  record: Record<string, unknown>
  columnKey: string
  value: unknown
}) {
  if (!DETAIL_DERIVED_VALUE_COLUMNS.has(payload.columnKey)) {
    return
  }
  detailDerivedRefreshRecord = payload.record
  if (detailDerivedRefreshTimer) {
    clearTimeout(detailDerivedRefreshTimer)
  }
  detailDerivedRefreshTimer = setTimeout(() => {
    detailDerivedRefreshTimer = undefined
    if (detailDerivedRefreshRecord) {
      const record = detailDerivedRefreshRecord
      detailDerivedRefreshRecord = null
      nextTick(() => refreshChildDetailDerivedRow(record))
    }
  }, 80)
}

/** 新增态：主表标准产能 > 0 时初始化 13 条固定生产时段子表行 */
async function initDefaultDetailRowsIfNeeded() {
  if (props.formData?.assyOutputId) {
    return
  }
  const stdCapacity = Number(formState.stdCapacity) || 0
  if (stdCapacity <= 0) {
    return
  }
  if (childAssyOutputDetailRows.value.length > 0) {
    return
  }
  try {
    const periods = await getAssyOutputDefaultTimePeriods()
    childAssyOutputDetailRows.value = (periods ?? []).map((timePeriod, index) => ({
      ...createDefaultAssyOutputDetailRow(timePeriod),
      lineNumber: (index + 1) * 10,
      timePeriod: normalizeAssyTimePeriod(timePeriod),
    }))
    await nextTick()
    refreshChildDetailCalculatedFields()
  } catch {
    childAssyOutputDetailRows.value = []
  }
}

/** 按工单号回填主表字段（仅新增态） */
async function backfillFromProductionOrder() {
  if (props.formData?.assyOutputId) {
    return
  }
  const prodOrderCode = String(formState.prodOrderCode ?? '').trim()
  if (!prodOrderCode) {
    return
  }
  isBackfillingFromOrder.value = true
  try {
    const plantCode = String(formState.plantCode ?? '').trim()
    const order = await getProductionOrderByCode(prodOrderCode, plantCode || undefined)
    if (order.plantCode) {
      formState.plantCode = order.plantCode
    }
    formState.prodOrderType = order.prodOrderType ?? ''
    formState.materialCode = order.materialCode ?? ''
    formState.prodOrderQty = order.prodOrderQty ?? 0
    formState.batchNo = order.prodBatch ?? ''
    formState.serialNo = order.serialNo ?? ''
    if (order.materialCode) {
      const model = await getModelDestinationByMaterial(order.materialCode)
      if (model?.modelCode) {
        formState.modelCode = model.modelCode
      }
    }
    await refreshMasterOperationRatePercent()
    await refreshMasterDerivedFields()
  } catch {
    // 工单不存在时保留用户已填内容
  } finally {
    isBackfillingFromOrder.value = false
  }
}

/** 组装 Create/Update 载荷（新增含子表；更新含子表就地更新，工单号/时段/行号保持库内值） */
function buildSubmitPayload() {
  const masterId = props.formData?.assyOutputId ?? ''
  const isUpdate = Boolean(masterId)
  const payload: Record<string, unknown> = {
    ...formState,
    tenantCode: tenantStore.tenantCode,
    companyCode: tenantStore.companyCode,
    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
  }
  const rawRows = assyOutputDetailTableRef.value?.getRows?.() ?? childAssyOutputDetailRows.value
  const detailScope = {
    tenantCode: tenantStore.tenantCode,
    companyCode: tenantStore.companyCode,
    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
    assyOutputId: isUpdate ? masterId : 0,
  }
  if (isUpdate) {
    payload.assyOutputDetails = rawRows.map((row) => {
      const normalized = normalizeAssyOutputDetailRowForSubmit(row)
      return {
        ...detailScope,
        prodOrderCode: row.prodOrderCode ?? formState.prodOrderCode,
        lineNumber: row.lineNumber,
        timePeriod: row.timePeriod,
        stdCapacity: normalized.stdCapacity ?? 0,
        prodActualQty: normalized.prodActualQty ?? 0,
        downtimeMinutes: normalized.downtimeMinutes ?? 0,
        downtimeReason: normalized.downtimeReason ?? '',
        downtimeDescription: normalized.downtimeDescription ?? '',
        unachievedReason: normalized.unachievedReason ?? '',
        unachievedDescription: normalized.unachievedDescription ?? '',
        confirmMinutes: normalized.confirmMinutes ?? 0,
        inputMinutes: normalized.inputMinutes ?? 0,
        actualMinutes: normalized.actualMinutes ?? 0,
        indirectMinutes: normalized.indirectMinutes ?? 0,
        mixedProd: normalized.mixedProd ?? 0,
        achievementRate: normalized.achievementRate ?? 0,
        extField: normalized.extField,
      }
    })
    return payload
  }
  payload.assyOutputDetails = rawRows.map((row) => normalizeAssyOutputDetailRowForSubmit({
    ...row,
    ...detailScope,
    prodOrderCode: row.prodOrderCode ?? formState.prodOrderCode,
  }))
  return payload
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyOutputCreate & { assyOutputId?: string }> | null
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
/** 生产工单下拉：按工厂过滤；输入关键字远程模糊搜索（后端返回全部匹配，不截断） */
const prodOrderSelectApiParams = computed(() => {
  const plantCode = String(formState.plantCode ?? '').trim()
  return plantCode ? { plantCode } : {}
})
/** 表单字段默认值（生产类别、班次） */
function formatLocalDateYmd(date: Date): string {
  const y = date.getFullYear()
  const m = String(date.getMonth() + 1).padStart(2, '0')
  const d = String(date.getDate()).padStart(2, '0')
  return `${y}-${m}-${d}`
}

/** 新增默认生产日期：昨天；若不可选则回退到今天或范围内最近一天 */
function resolveDefaultAssyOutputProdDateYmd(): string {
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  const yesterday = new Date(today)
  yesterday.setDate(yesterday.getDate() - 1)
  const yesterdayYmd = formatLocalDateYmd(yesterday)
  if (isAssyOutputProdDateSelectable(yesterdayYmd)) {
    return yesterdayYmd
  }
  const todayYmd = formatLocalDateYmd(today)
  if (isAssyOutputProdDateSelectable(todayYmd)) {
    return todayYmd
  }
  return todayYmd
}

/** 字典 logistics_prod_category：常规生产 */
const DEFAULT_ASSY_OUTPUT_PROD_CATEGORY = 'FPP'
/** 字典 logistics_shift_category：白班 */
const DEFAULT_ASSY_OUTPUT_SHIFT_NO = 4

function applyFormDefaults(target: Record<string, unknown>) {
  if (!target.prodDate) {
    target.prodDate = resolveDefaultAssyOutputProdDateYmd()
  }
  if (target.prodCategory == null || target.prodCategory === '') {
    target.prodCategory = DEFAULT_ASSY_OUTPUT_PROD_CATEGORY
  }
  if (target.shiftNo == null || target.shiftNo === '') {
    target.shiftNo = DEFAULT_ASSY_OUTPUT_SHIFT_NO
  }
  if (target.directLabor == null || target.directLabor === '') {
    target.directLabor = 0
  }
  if (target.indirectLabor == null || target.indirectLabor === '') {
    target.indirectLabor = 0
  }
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 字典缓存就绪后重洗子表多选字段（避免 options 未加载时写入无法命中的 DictValue） */
watch(
  () => dictDataStore.loaded,
  (loaded) => {
    if (!loaded) {
      return
    }
    refreshChildDetailCalculatedFields()
  },
)

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyOutputId 才视为编辑） */
watch(
  () => props.formData,
  async (val) => {
    if (val?.assyOutputId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).assyOutputDetails
      applyScopeDefaults(next)
      Object.assign(formState, next)
      applyFormDefaults(formState)
    syncChildRowsFromFormData(val)
      await refreshMasterOperationRatePercent()
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      void refreshMasterOperationRatePercent()
      void refreshMasterDerivedFields()
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

watch(
  () => [formState.plantCode, formState.materialCode, formState.prodDate] as const,
  () => {
    if (isBackfillingFromOrder.value) {
      return
    }
    void refreshMasterDerivedFields()
  }
)

watch(
  () => [formState.plantCode, formState.prodDate] as const,
  () => {
    if (isBackfillingFromOrder.value) {
      return
    }
    void refreshMasterOperationRatePercent().then(() => {
      void initDefaultDetailRowsIfNeeded()
    })
  }
)

watch(
  () => formState.directLabor,
  () => {
    refreshMasterStdCapacityOnly()
    void initDefaultDetailRowsIfNeeded()
  }
)

/** 间接人员变更时重算子表间接工时 */
watch(
  () => formState.indirectLabor,
  () => {
    refreshChildDetailCalculatedFields()
  }
)

/** 表头标准产能/标准工时/稼动率变更：同步子表标准产能；标准产能 > 0 时创建子表行 */
watch(
  () => [formState.stdCapacity, formState.stdMinutes, masterOperationRatePercent.value] as const,
  () => {
    refreshChildDetailCalculatedFields()
    void initDefaultDetailRowsIfNeeded()
  }
)

watch(
  () => formState.prodOrderCode,
  () => {
    void backfillFromProductionOrder()
  }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.assyOutputId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 主表字符串必填（extField、remark 除外） */
function requiredStringRule(field: Parameters<typeof pi.ph>[0], trigger: 'blur' | 'change' = 'blur'): Rule {
  return {
    required: true,
    message: pi.ph(field),
    trigger,
  }
}

/** 主表数值必填（0 合法；required: true 用于显示标签红 *） */
function requiredNumberRule(field: Parameters<typeof pi.ph>[0]): Rule {
  return {
    required: true,
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph(field))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph(field))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 主表数值必填且须大于 0（直接/间接人员、工单数量、标准工时、标准产能） */
function requiredPositiveNumberRule(field: Parameters<typeof pi.ph>[0]): Rule {
  return {
    required: true,
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph(field))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph(field))
      }
      if (num <= 0) {
        return Promise.reject(t('common.validation.out.of.range', { field: pi.label(field) }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 生产日期未锁定且在可选范围内 */
function prodDateEditableRule(): Rule {
  return {
    validator: async (_rule, value) => {
      const ymd = String(value ?? '').trim().slice(0, 10)
      if (!ymd) {
        return Promise.resolve()
      }
      if (isAssyOutputProdDateLocked(ymd)) {
        return Promise.reject(pi.prodDateLockedMessage(ymd))
      }
      if (!isAssyOutputProdDateSelectable(ymd)) {
        return Promise.reject(pi.prodDateOutOfRangeMessage())
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 表单校验规则（除 extField、remark 外全部必填） */
const rules = computed<Record<string, Rule[]>>(() => ({
  tenantCode: [requiredStringRule('tenantCode')],
  companyCode: [requiredStringRule('companyCode')],
  companyDefaultCulture: [requiredStringRule('companyDefaultCulture')],
  plantCode: [requiredStringRule('plantCode', 'change')],
  prodCategory: [requiredStringRule('prodCategory', 'change')],
  prodDate: [requiredStringRule('prodDate', 'change'), prodDateEditableRule()],
  prodTeam: [requiredStringRule('prodTeam', 'change')],
  directLabor: [requiredPositiveNumberRule('directLabor')],
  indirectLabor: [requiredPositiveNumberRule('indirectLabor')],
  shiftNo: [requiredNumberRule('shiftNo')],
  prodOrderType: [requiredStringRule('prodOrderType')],
  prodOrderCode: [requiredStringRule('prodOrderCode', 'change')],
  modelCode: [requiredStringRule('modelCode', 'change')],
  materialCode: [requiredStringRule('materialCode', 'change')],
  batchNo: [requiredStringRule('batchNo')],
  prodOrderQty: [requiredPositiveNumberRule('prodOrderQty')],
  serialNo: [requiredStringRule('serialNo')],
  stdMinutes: [requiredPositiveNumberRule('stdMinutes')],
  stdCapacity: [requiredPositiveNumberRule('stdCapacity')],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  if (isMasterProdDateLocked.value) {
    throw new Error(prodDateLockedAlertMessage.value)
  }
  await formRef.value?.validate()
  await assyOutputDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('directLabor' in payload) {
    const rawdirectLabor = payload.directLabor
    payload.directLabor = typeof rawdirectLabor === 'number' ? rawdirectLabor : Number(rawdirectLabor)
  }
  if ('indirectLabor' in payload) {
    const rawindirectLabor = payload.indirectLabor
    payload.indirectLabor = typeof rawindirectLabor === 'number' ? rawindirectLabor : Number(rawindirectLabor)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    payload.prodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
  }
  if ('stdMinutes' in payload) {
    const rawstdMinutes = payload.stdMinutes
    payload.stdMinutes = typeof rawstdMinutes === 'number' ? rawstdMinutes : Number(rawstdMinutes)
  }
  if ('stdCapacity' in payload) {
    const rawstdCapacity = payload.stdCapacity
    payload.stdCapacity = typeof rawstdCapacity === 'number' ? rawstdCapacity : Number(rawstdCapacity)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assyOutputId)
  childAssyOutputDetailRows.value = []
  assyOutputDetailTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields, isMasterProdDateLocked })
</script>

<style scoped lang="css">
:deep(.assy-output-form-tabs .ant-tabs-content-holder),
:deep(.assy-output-form-tabs .ant-tabs-tabpane) {
  min-height: unset;
  overflow: visible;
}
</style>
