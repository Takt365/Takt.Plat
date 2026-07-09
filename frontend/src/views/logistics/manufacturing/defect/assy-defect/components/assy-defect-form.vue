<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：assy-defect-form.vue -->
<!-- 功能描述：组立不良日报实体 不良率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-defect-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-defect-form-tabs"
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
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <TaktSelect
                  v-if="!formData?.assyDefectId"
                  v-model:value="selectedAssyOutputId"
                  api-url="TaktAssyOutputs/prod-order-options"
                  :api-params="prodOrderOptionsApiParams"
                  :placeholder="pi.ph('prodOrderCode')"
                  :disabled="loading"
                  allow-clear
                  @change="handleProdOrderSelectChange"
                />
                <a-input
                  v-else
                  v-model:value="formState.prodOrderCode"
                  :placeholder="pi.ph('prodOrderCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodActualQty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="pi.ph('prodActualQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('goodQuantity')"
                name="goodQuantity"
              >
                <a-input-number
                  v-model:value="formState.goodQuantity"
                  :placeholder="pi.ph('goodQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodCategory')"
                name="prodCategory"
              >
                <a-input
                  v-model:value="formState.prodCategory"
                  :placeholder="pi.ph('prodCategory')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                :label="pi.label('prodTeam')"
                name="prodTeam"
              >
                <a-input
                  v-model:value="formState.prodTeam"
                  :placeholder="pi.ph('prodTeam')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="pi.ph('shiftNo')"
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderType')"
                name="prodOrderType"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="pi.ph('prodOrderType')"
                  show-count
                  :maxlength="10"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderQty')"
                name="prodOrderQty"
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
                :label="pi.label('modelCode')"
                name="modelCode"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchNo')"
                name="batchNo"
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
                :label="pi.label('materialCode')"
                name="materialCode"
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
    <!-- 下：子表 assyDefectDetails -->
    <TaktEditableTable
      ref="assyDefectDetailTableRef"
      v-model="childAssyDefectDetailRows"
      :columns="assyDefectDetailFormColumns"
      :title="assyDefectDetailPi.self()"
      :add-button-entity="assyDefectDetailPi.self()"
      id-field="assyDefectDetailId"
      obsolete-field="isObsolete"
      :default-row="createDefaultAssyDefectDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
      @cell-value-change="handleAssyDefectDetailCellValueChange"
      @obsolete="handleAssyDefectDetailObsoleteChange"
      @revoke="handleAssyDefectDetailObsoleteChange"
    >
      <template #cell-defectCategory="{ record }">
        <TaktSelect
          :model-value="getDetailDictSelectModelValue(record, 'defectCategory')"
          dict-type="logistics_defect_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyDefectDetailPi.ph('defectCategory')"
          :disabled="loading || isAssyDefectDetailRowObsolete(record)"
          allow-clear
          @update:model-value="(v) => handleDetailDictSelectChange(record, 'defectCategory', v)"
        />
      </template>
      <template #cell-defectLocation="{ record }">
        <TaktSelect
          :model-value="getDetailDictSelectModelValue(record, 'defectLocation')"
          dict-type="logistics_assy_location_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyDefectDetailPi.ph('defectLocation')"
          :disabled="loading || isAssyDefectDetailRowObsolete(record)"
          allow-clear
          @update:model-value="(v) => handleDetailDictSelectChange(record, 'defectLocation', v)"
        />
      </template>
      <template #cell-repairOperator="{ record }">
        <TaktSelect
          :model-value="getDetailDictSelectModelValue(record, 'repairOperator')"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyDefectDetailPi.queryPh('repairOperator', 'select')"
          :disabled="loading || isAssyDefectDetailRowObsolete(record)"
          allow-clear
          @update:model-value="(v) => handleDetailDictSelectChange(record, 'repairOperator', v)"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立不良日报实体 不良率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyDefectI18n } from '../composables/use-assy-defect-i18n'
import {
  useAssyDefectDetailEditableDict,
  type AssyDefectDetailDictField,
} from '../composables/use-assy-defect-detail-editable-dict'

/** 实体字段 i18n */
const pi = useAssyDefectI18n()
const {
  loadEmployeeOptionsAsync,
  ensureDetailDictFields,
  getDetailDictSelectModelValue,
  applyDetailDictChange,
  normalizeAssyDefectDetailRowForSubmit,
} = useAssyDefectDetailEditableDict()

import type { AssyDefectCreate } from '@/types/logistics/manufacturing/defect/assy-defect'
import type { AssyOutput } from '@/types/logistics/manufacturing/output/assy-output'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getAssyOutputById } from '@/api/logistics/manufacturing/output/assy-output'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { isEditableRowObsolete } from '@/components/business/takt-editable-table/types'

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodOrderCode","prodActualQty","goodQuantity","prodCategory","prodDate","prodTeam","shiftNo","prodOrderType","prodOrderQty","modelCode","batchNo","materialCode","extField","remark"]

/** 新增态选中的组立日报 Id（TaktAssyOutputs/prod-order-options DictValue） */
const selectedAssyOutputId = ref<string | undefined>(undefined)

/** 工单下拉请求参数（编辑态保留当前不良日报对应选项） */
const prodOrderOptionsApiParams = computed(() => ({
  excludeAssyDefectId: props.formData?.assyDefectId || undefined,
}))

/** 随工单回填的主表字段（不含生实实绩、无不良数量） */
const MASTER_BACKFILL_FIELD_KEYS = [
  'plantCode',
  'prodCategory',
  'prodDate',
  'prodTeam',
  'shiftNo',
  'prodOrderType',
  'prodOrderCode',
  'prodOrderQty',
  'modelCode',
  'batchNo',
  'materialCode',
] as const

/**
 * 将 API 日期规范为 YYYY-MM-DD
 * @param value 生产日期
 * @returns {string} 日期字符串
 */
function normalizeProdDateYmd(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  const raw = String(value)
  return raw.length >= 10 ? raw.slice(0, 10) : raw
}

/**
 * 汇总组立日报明细生实实绩
 * @param output 组立日报
 * @returns {number} 合计数量
 */
function sumAssyOutputProdActualQty(output: AssyOutput): number {
  return (output.assyOutputDetails ?? []).reduce((sum, detail) => {
    const qty = typeof detail.prodActualQty === 'number' ? detail.prodActualQty : Number(detail.prodActualQty)
    return sum + (Number.isFinite(qty) ? qty : 0)
  }, 0)
}

/** 清空工单回填主表字段 */
function clearMasterBackfillFields() {
  for (const key of MASTER_BACKFILL_FIELD_KEYS) {
    if (key === 'shiftNo' || key === 'prodOrderQty') {
      formState[key] = undefined
    } else {
      formState[key] = ''
    }
  }
}

/**
 * 根据组立日报回填主表生实实绩（无不良数量须手工录入，不与不良数量联动）
 * @param output 组立日报详情
 */
function applyMasterFromAssyOutput(output: AssyOutput) {
  formState.plantCode = output.plantCode ?? ''
  formState.prodCategory = output.prodCategory ?? ''
  formState.prodDate = normalizeProdDateYmd(output.prodDate)
  formState.prodTeam = output.prodTeam ?? ''
  formState.shiftNo = output.shiftNo
  formState.prodOrderType = output.prodOrderType ?? ''
  formState.prodOrderCode = output.prodOrderCode ?? ''
  formState.prodOrderQty = output.prodOrderQty ?? 0
  formState.modelCode = output.modelCode ?? ''
  formState.batchNo = output.batchNo ?? ''
  formState.materialCode = output.materialCode ?? ''
  const prodActualQty = sumAssyOutputProdActualQty(output)
  formState.prodActualQty = prodActualQty
  syncChildRowsRedundantFromMaster()
}

/**
 * 从工单下拉选项解析工单号（ExtValue 优先，否则从 Label 截取）
 * @param option 下拉选项
 * @returns {string} 工单号
 */
function resolveProdOrderCodeFromSelectOption(
  option: { extValue?: string | number; dictLabel?: string; label?: string } | null | undefined,
): string {
  if (option?.extValue != null && String(option.extValue).trim() !== '') {
    return String(option.extValue).trim()
  }
  const label = String(option?.dictLabel ?? option?.label ?? '').trim()
  if (!label) {
    return ''
  }
  const match = label.match(/^(\S+)/)
  return match?.[1] ?? label
}

/**
 * 新增态工单选择变更：同步 formState.prodOrderCode 并拉取组立日报回填
 * @param value 选中的组立日报 Id
 * @param option 下拉选项（含 extValue=工单号）
 */
async function handleProdOrderSelectChange(
  value: string | number | (string | number)[] | undefined,
  option?: { extValue?: string | number; dictLabel?: string; label?: string } | null,
) {
  const assyOutputId = value != null && value !== '' && !Array.isArray(value) ? String(value) : undefined
  selectedAssyOutputId.value = assyOutputId
  if (!assyOutputId) {
    formState.prodOrderCode = ''
    clearMasterBackfillFields()
    formState.prodActualQty = undefined
    formState.goodQuantity = undefined
    syncChildRowsRedundantFromMaster()
    return
  }
  const prodOrderCodeFromOption = resolveProdOrderCodeFromSelectOption(option)
  if (prodOrderCodeFromOption) {
    formState.prodOrderCode = prodOrderCodeFromOption
  }
  formRef.value?.clearValidate(['prodOrderCode'])
  const output = await getAssyOutputById(assyOutputId)
  applyMasterFromAssyOutput(output)
  formRef.value?.clearValidate(['prodOrderCode'])
}


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useAssyDefectDetailI18n } from '../composables/use-assy-defect-detail-i18n'

const assyDefectDetailPi = useAssyDefectDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childAssyDefectDetailRows = ref<Record<string, unknown>[]>([])
const assyDefectDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 assyDefectDetail 可编辑列 */
const assyDefectDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: assyDefectDetailPi.label('lineNumber'),
    readonly: true,
    width: 140,
  },
  {
    key: 'defectCategory',
    title: assyDefectDetailPi.label('defectCategory'),
    width: 140,
  },
  {
    key: 'defectQty',
    title: assyDefectDetailPi.label('defectQty'),
    editor: 'inputNumber',
    width: 140,
    summary: 'sum',
    summaryPrecision: 3,
  },
  {
    key: 'randomCardNo',
    title: assyDefectDetailPi.label('randomCardNo'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('randomCardNo'),
  },
  {
    key: 'occurrenceEngineering',
    title: assyDefectDetailPi.label('occurrenceEngineering'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('occurrenceEngineering'),
  },
  {
    key: 'testStep',
    title: assyDefectDetailPi.label('testStep'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('testStep'),
  },
  {
    key: 'defectSymptom',
    title: assyDefectDetailPi.label('defectSymptom'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('defectSymptom'),
  },
  {
    key: 'defectLocation',
    title: assyDefectDetailPi.label('defectLocation'),
    width: 140,
  },
  {
    key: 'defectReason',
    title: assyDefectDetailPi.label('defectReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('defectReason'),
  },
  {
    key: 'repairOperator',
    title: assyDefectDetailPi.label('repairOperator'),
    width: 140,
  },
  {
    key: 'cumulativeDefectQty',
    title: assyDefectDetailPi.label('cumulativeDefectQty'),
    readonly: true,
    width: 140,
    summary: 'max',
    summaryPrecision: 3,
  },
  {
    key: 'goodQuantity',
    title: assyDefectDetailPi.label('goodQuantity'),
    readonly: true,
    width: 140,
  },
  {
    key: 'prodOrderCode',
    title: assyDefectDetailPi.label('prodOrderCode'),
    readonly: true,
    width: 140,
  },
  {
    key: 'prodActualQty',
    title: assyDefectDetailPi.label('prodActualQty'),
    readonly: true,
    width: 140,
  },
])

/** 主表冗余字段（同步至子表各行，只读展示） */
function resolveMasterRedundantFields() {
  return {
    prodOrderCode: String(formState.prodOrderCode ?? ''),
    prodActualQty: Number(formState.prodActualQty) || 0,
    goodQuantity: Number(formState.goodQuantity) || 0,
  }
}

/**
 * 解析子表不良数量
 * @param value 单元格值
 * @returns {number} 非负数值
 */
function parseAssyDefectDetailQty(value: unknown): number {
  const num = typeof value === 'number' ? value : Number(value)
  return Number.isFinite(num) ? num : 0
}

/**
 * 读取子表当前行（优先 TaktEditableTable 内部态，含插槽列未同步到 v-model 的编辑）
 * @returns {Record<string, unknown>[]} 子表行
 */
function getEditableDetailRows(): Record<string, unknown>[] {
  return assyDefectDetailTableRef.value?.getRows?.() ?? childAssyDefectDetailRows.value
}

/**
 * 写回子表行并驱动 TaktEditableTable 同步
 * @param rows 子表行
 */
function setEditableDetailRows(rows: Record<string, unknown>[]) {
  childAssyDefectDetailRows.value = rows
}

/** 是否已持久化的子表行（有 assyDefectDetailId） */
function isPersistedAssyDefectDetailRow(row: Record<string, unknown>): boolean {
  const id = row.assyDefectDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（基于当前表格行，含作废行） */
function allocateNextAssyDefectDetailLineNumber(): number {
  return resolveNextDetailLineNumber(0, getEditableDetailRows())
}

/** 将主表冗余字段同步到全部子表行 */
function syncChildRowsRedundantFromMaster() {
  const redundant = resolveMasterRedundantFields()
  setEditableDetailRows(getEditableDetailRows().map((row) => ({
    ...row,
    ...redundant,
  })))
}

/** 子表行是否已作废 */
function isAssyDefectDetailRowObsolete(row: Record<string, unknown>): boolean {
  return isEditableRowObsolete(row, 'isObsolete', 1)
}

/**
 * 按行号顺序累加 defectQty，回写各行 cumulativeDefectQty（跳过作废行；不改写主表无不良数量）
 */
function recalculateAssyDefectDetailCumulativeQtys() {
  const rows = getEditableDetailRows()
  if (!rows.length) {
    return
  }
  const order = rows
    .map((row, index) => ({ index, lineNumber: Number(row.lineNumber) || 0, obsolete: isAssyDefectDetailRowObsolete(row) }))
    .filter((item) => !item.obsolete)
    .sort((a, b) => a.lineNumber - b.lineNumber || a.index - b.index)
  let running = 0
  const cumulativeByIndex = new Map<number, number>()
  for (const { index } of order) {
    running += parseAssyDefectDetailQty(rows[index]?.defectQty)
    cumulativeByIndex.set(index, running)
  }
  const redundant = resolveMasterRedundantFields()
  setEditableDetailRows(rows.map((row, index) => ({
    ...row,
    ...redundant,
    cumulativeDefectQty: cumulativeByIndex.get(index) ?? 0,
  })))
}

/** 子表派生字段：累计不良、主表冗余（不重排行号、不改写无不良数量） */
function refreshAssyDefectDetailDerivedFields() {
  recalculateAssyDefectDetailCumulativeQtys()
}

/**
 * 子表字典/修理员 Select 变更：写回行内并同步 v-model
 * @param record 子表行
 * @param field 字段名
 * @param value Select 绑定值
 */
function handleDetailDictSelectChange(
  record: Record<string, unknown>,
  field: AssyDefectDetailDictField,
  value: string | number | readonly (string | number)[] | null | undefined,
) {
  applyDetailDictChange(record, field, value)
  assyDefectDetailTableRef.value?.syncModelValue?.()
}

/**
 * 子表单元格变更：defectQty 变更时重算累计不良
 * @param payload 变更上下文
 */
function handleAssyDefectDetailCellValueChange(payload: {
  record: Record<string, unknown>
  columnKey: string
  value: unknown
}) {
  if (isAssyDefectDetailRowObsolete(payload.record)) {
    return
  }
  if (payload.columnKey === 'defectQty') {
    recalculateAssyDefectDetailCumulativeQtys()
  }
}

/** 子表作废/撤销后重算累计不良 */
function handleAssyDefectDetailObsoleteChange() {
  recalculateAssyDefectDetailCumulativeQtys()
}

/** 字典/员工选项就绪后，将子表库内 Label 转为 Select 绑定值 */
async function hydrateChildDetailDictFields() {
  await dictDataStore.loadAllDictDataAsync()
  await loadEmployeeOptionsAsync()
  const rows = getEditableDetailRows()
  if (!rows.length) {
    return
  }
  setEditableDetailRows(rows.map((row) => normalizeChildDetailRow(row)))
}

/** 灌入子表行时：字典/修理员库内 Label → TaktSelect 绑定 Value */
function normalizeChildDetailRow(row: Record<string, unknown>): Record<string, unknown> {
  const normalized: Record<string, unknown> = {
    ...row,
    isObsolete: Number(row.isObsolete) === 1 ? 1 : 0,
  }
  ensureDetailDictFields(normalized)
  return normalized
}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<AssyDefectCreate & { assyDefectId?: string }> | null | undefined) {
  const rows = ((val as any)?.assyDefectDetails ?? []) as Record<string, unknown>[]
  childAssyDefectDetailRows.value = rows.map((row) => normalizeChildDetailRow(row))
  refreshAssyDefectDetailDerivedFields()
  void hydrateChildDetailDictFields()
}

function createDefaultAssyDefectDetailRow(): Record<string, unknown> {
  const row: Record<string, unknown> = {
    ...resolveMasterRedundantFields(),
    isObsolete: 0,
    lineNumber: allocateNextAssyDefectDetailLineNumber(),
    defectQty: 0,
    cumulativeDefectQty: 0,
    randomCardNo: '',
    occurrenceEngineering: '',
    testStep: '',
    defectSymptom: '',
    defectReason: '',
  }
  ensureDetailDictFields(row)
  return row
}

/** 组装 Create/Update 载荷（主表 + 子表数组；更新时携带 assyDefectDetailId 就地更新） */
function buildSubmitPayload() {
  const masterId = props.formData?.assyDefectId ?? ''
  const isUpdate = Boolean(masterId)
  const rawRows = assyDefectDetailTableRef.value?.getRows?.() ?? childAssyDefectDetailRows.value
  const detailScope = {
    tenantCode: tenantStore.tenantCode,
    companyCode: tenantStore.companyCode,
    companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
    assyDefectId: masterId,
  }
  const assyDefectDetails = rawRows
    .filter((row) => !isAssyDefectDetailRowObsolete(row))
    .map((row) => {
    const normalized = normalizeAssyDefectDetailRowForSubmit({
      ...row,
      ...detailScope,
    })
    if (isUpdate && isPersistedAssyDefectDetailRow(row)) {
      normalized.assyDefectDetailId = row.assyDefectDetailId
    } else {
      delete normalized.assyDefectDetailId
    }
    return normalized
  })
  return {
    ...formState,
    assyDefectDetails,
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyDefectCreate & { assyDefectId?: string }> | null
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
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载字典与员工选项 */
onMounted(() => {
  void hydrateChildDetailDictFields()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyDefectId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyDefectId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as { assyDefectDetails?: unknown }).assyDefectDetails
      applyScopeDefaults(next)
      Object.assign(formState, next)
      selectedAssyOutputId.value = undefined
      syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      selectedAssyOutputId.value = undefined
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
    const isCreate = !props.formData?.assyDefectId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 主表生实实绩变更时，同步子表冗余列（不改写无不良数量） */
watch(
  () => formState.prodActualQty,
  () => {
    syncChildRowsRedundantFromMaster()
  },
)

/** 主表工单号 / 无不良数量变更时同步子表只读冗余列 */
watch(
  () => [formState.prodOrderCode, formState.goodQuantity] as const,
  () => {
    syncChildRowsRedundantFromMaster()
  },
)

/** 增删子表行后重算行号与累计不良 */
watch(
  () => childAssyDefectDetailRows.value.length,
  () => {
    refreshAssyDefectDetailDerivedFields()
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'blur',
    },
  ],
  prodOrderCode: [
    {
      validator: async (_rule, value) => {
        const isCreate = !props.formData?.assyDefectId
        if (isCreate) {
          if (selectedAssyOutputId.value) {
            return Promise.resolve()
          }
        }
        const text = value == null ? '' : String(value).trim()
        if (text) {
          return Promise.resolve()
        }
        return Promise.reject(pi.ph('prodOrderCode'))
      },
      trigger: 'change',
    },
  ],
  prodActualQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  goodQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('goodQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('goodQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  prodCategory: [
    {
      required: true,
      message: pi.ph('prodCategory'),
      trigger: 'change',
    },
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change',
    },
  ],
  prodTeam: [
    {
      required: true,
      message: pi.ph('prodTeam'),
      trigger: 'change',
    },
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  prodOrderType: [
    {
      required: true,
      message: pi.ph('prodOrderType'),
      trigger: 'blur',
    },
  ],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  modelCode: [
    {
      required: true,
      message: pi.ph('modelCode'),
      trigger: 'blur',
    },
  ],
  batchNo: [
    {
      required: true,
      message: pi.ph('batchNo'),
      trigger: 'blur',
    },
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur',
    },
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await assyDefectDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    payload.prodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
  }
  if ('prodActualQty' in payload) {
    const rawprodActualQty = payload.prodActualQty
    payload.prodActualQty = typeof rawprodActualQty === 'number' ? rawprodActualQty : Number(rawprodActualQty)
  }
  if ('goodQuantity' in payload) {
    const rawgoodQuantity = payload.goodQuantity
    payload.goodQuantity = typeof rawgoodQuantity === 'number' ? rawgoodQuantity : Number(rawgoodQuantity)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  selectedAssyOutputId.value = undefined
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assyDefectId)
  childAssyDefectDetailRows.value = []
  assyDefectDetailTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

