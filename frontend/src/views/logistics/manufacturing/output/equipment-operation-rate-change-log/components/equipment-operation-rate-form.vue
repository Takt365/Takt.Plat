<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/equipment-operation-rate-change-log/components -->
<!-- 文件名称：equipment-operation-rate-form.vue -->
<!-- 功能描述：机器稼动率实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form equipment-operation-rate-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="equipment-operation-rate-form-tabs"
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
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.equipmentOperationRateId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.timecategory')"
                name="timeCategory"
              >
                <a-input-number
                  v-model:value="formState.timeCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.timecategory') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.startdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.enddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.weeknumber')"
                name="weekNumber"
              >
                <a-input-number
                  v-model:value="formState.weekNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.weeknumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.monthnumber')"
                name="monthNumber"
              >
                <a-input-number
                  v-model:value="formState.monthNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.monthnumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmentcode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.equipmentOperationRateId"
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
                :label="t('entity.equipmentoperationrate.equipmentname')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmenttype')"
                name="equipmentType"
              >
                <a-input-number
                  v-model:value="formState.equipmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmenttype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.productionline') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.shiftno') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.plannedruntime')"
                name="plannedRuntime"
              >
                <a-input-number
                  v-model:value="formState.plannedRuntime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.plannedruntime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.actualruntime')"
                name="actualRuntime"
              >
                <a-input-number
                  v-model:value="formState.actualRuntime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.actualruntime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.downtime')"
                name="downtime"
              >
                <a-input-number
                  v-model:value="formState.downtime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.downtime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmentoperationrate')"
                name="equipmentOperationRate"
              >
                <a-input-number
                  v-model:value="formState.equipmentOperationRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentoperationrate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.plannedoutput')"
                name="plannedOutput"
              >
                <a-input-number
                  v-model:value="formState.plannedOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.plannedoutput') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.actualoutput')"
                name="actualOutput"
              >
                <a-input-number
                  v-model:value="formState.actualOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.actualoutput') })"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.qualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.defectivequantity')"
                name="defectiveQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectiveQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.defectivequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.yieldrate')"
                name="yieldRate"
              >
                <a-input-number
                  v-model:value="formState.yieldRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.yieldrate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.downtimereasontype')"
                name="downtimeReasonType"
              >
                <a-input-number
                  v-model:value="formState.downtimeReasonType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.downtimereasontype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.downtimereason')"
                name="downtimeReason"
              >
                <a-input
                  v-model:value="formState.downtimeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.downtimereason') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmentstatus')"
                name="equipmentStatus"
              >
                <a-input-number
                  v-model:value="formState.equipmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmentoperator')"
                name="equipmentOperator"
              >
                <a-input
                  v-model:value="formState.equipmentOperator"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentoperator') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.equipmentmaintainer')"
                name="equipmentMaintainer"
              >
                <a-input
                  v-model:value="formState.equipmentMaintainer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentmaintainer') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.teamleader')"
                name="teamLeader"
              >
                <a-input
                  v-model:value="formState.teamLeader"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.teamleader') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentoperationrate.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.status') })"
                  style="width: 100%"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="equipmentOperationRateChangeLogTableRef"
      v-model="childEquipmentOperationRateChangeLogRows"
      :columns="equipmentOperationRateChangeLogFormColumns"
      :title="t('entity.equipmentoperationratechangelog._self')"
      :add-button-entity="t('entity.equipmentoperationratechangelog._self')"
      id-field="equipmentOperationRateChangeLogId"
      :default-row="createDefaultEquipmentOperationRateChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 机器稼动率实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/equipment-operation-rate-change-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EquipmentOperationRateCreate } from '@/types/logistics/manufacturing/output/equipment-operation-rate'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","timeCategory","startDate","endDate","weekNumber","monthNumber","equipmentCode","equipmentName","equipmentType","productionLine","shiftNo","plannedRuntime","actualRuntime","downtime","equipmentOperationRate","plannedOutput","actualOutput","qualifiedQuantity","defectiveQuantity","yieldRate","downtimeReasonType","downtimeReason","equipmentStatus","equipmentOperator","equipmentMaintainer","teamLeader","status","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childEquipmentOperationRateChangeLogRows = ref<Record<string, unknown>[]>([])
const equipmentOperationRateChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 equipmentOperationRateChangeLog 可编辑列 */
const equipmentOperationRateChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'equipmentCode',
    title: t('entity.equipmentoperationratechangelog.equipmentcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'changeFields',
    title: t('entity.equipmentoperationratechangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.equipmentoperationratechangelog.changefields') }),
  },
  {
    key: 'changeTime',
    title: t('entity.equipmentoperationratechangelog.changetime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'changeBy',
    title: t('entity.equipmentoperationratechangelog.changeby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.equipmentoperationratechangelog.changeby') }),
  },
  {
    key: 'changeReason',
    title: t('entity.equipmentoperationratechangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.equipmentoperationratechangelog.changereason') }),
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<EquipmentOperationRateCreate & { equipmentOperationRateId?: string }> | null | undefined) {
  childEquipmentOperationRateChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultEquipmentOperationRateChangeLogRow(): Record<string, unknown> {
  return {
    equipmentCode: '',
    changeFields: '',
    changeTime: '',
    changeBy: '',
    changeReason: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.equipmentOperationRateId ?? ''
  return {
    ...formState,
    changeLogs: equipmentOperationRateChangeLogTableRef.value?.getRows?.() ?? childEquipmentOperationRateChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      equipmentOperationRateId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EquipmentOperationRateCreate & { equipmentOperationRateId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 equipmentOperationRateId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.equipmentOperationRateId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.equipmentOperationRateId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.plantcode') }),
      trigger: 'blur'
    }
  ],
  timeCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.timecategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.timecategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.enddate') }),
      trigger: 'change'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentcode') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentoperationrate.equipmentname') }),
      trigger: 'blur'
    }
  ],
  equipmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmenttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmenttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.shiftno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.shiftno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedRuntime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.plannedruntime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.plannedruntime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualRuntime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.actualruntime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.actualruntime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downtime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.downtime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.downtime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentOperationRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmentoperationrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmentoperationrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.plannedoutput') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.plannedoutput') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.actualoutput') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.actualoutput') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.qualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.qualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectiveQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.defectivequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.defectivequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  yieldRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.yieldrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.yieldrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmentstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.equipmentstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  status: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.equipmentoperationrate.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await equipmentOperationRateChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('timeCategory' in payload) {
    const rawtimeCategory = payload.timeCategory
    payload.timeCategory = typeof rawtimeCategory === 'number' ? rawtimeCategory : Number(rawtimeCategory)
  }
  if ('weekNumber' in payload) {
    const rawweekNumber = payload.weekNumber
    payload.weekNumber = typeof rawweekNumber === 'number' ? rawweekNumber : Number(rawweekNumber)
  }
  if ('monthNumber' in payload) {
    const rawmonthNumber = payload.monthNumber
    payload.monthNumber = typeof rawmonthNumber === 'number' ? rawmonthNumber : Number(rawmonthNumber)
  }
  if ('equipmentType' in payload) {
    const rawequipmentType = payload.equipmentType
    payload.equipmentType = typeof rawequipmentType === 'number' ? rawequipmentType : Number(rawequipmentType)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('plannedRuntime' in payload) {
    const rawplannedRuntime = payload.plannedRuntime
    payload.plannedRuntime = typeof rawplannedRuntime === 'number' ? rawplannedRuntime : Number(rawplannedRuntime)
  }
  if ('actualRuntime' in payload) {
    const rawactualRuntime = payload.actualRuntime
    payload.actualRuntime = typeof rawactualRuntime === 'number' ? rawactualRuntime : Number(rawactualRuntime)
  }
  if ('downtime' in payload) {
    const rawdowntime = payload.downtime
    payload.downtime = typeof rawdowntime === 'number' ? rawdowntime : Number(rawdowntime)
  }
  if ('equipmentOperationRate' in payload) {
    const rawequipmentOperationRate = payload.equipmentOperationRate
    payload.equipmentOperationRate = typeof rawequipmentOperationRate === 'number' ? rawequipmentOperationRate : Number(rawequipmentOperationRate)
  }
  if ('plannedOutput' in payload) {
    const rawplannedOutput = payload.plannedOutput
    payload.plannedOutput = typeof rawplannedOutput === 'number' ? rawplannedOutput : Number(rawplannedOutput)
  }
  if ('actualOutput' in payload) {
    const rawactualOutput = payload.actualOutput
    payload.actualOutput = typeof rawactualOutput === 'number' ? rawactualOutput : Number(rawactualOutput)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('defectiveQuantity' in payload) {
    const rawdefectiveQuantity = payload.defectiveQuantity
    payload.defectiveQuantity = typeof rawdefectiveQuantity === 'number' ? rawdefectiveQuantity : Number(rawdefectiveQuantity)
  }
  if ('yieldRate' in payload) {
    const rawyieldRate = payload.yieldRate
    payload.yieldRate = typeof rawyieldRate === 'number' ? rawyieldRate : Number(rawyieldRate)
  }
  if ('downtimeReasonType' in payload) {
    const rawdowntimeReasonType = payload.downtimeReasonType
    payload.downtimeReasonType = typeof rawdowntimeReasonType === 'number' ? rawdowntimeReasonType : Number(rawdowntimeReasonType)
  }
  if ('equipmentStatus' in payload) {
    const rawequipmentStatus = payload.equipmentStatus
    payload.equipmentStatus = typeof rawequipmentStatus === 'number' ? rawequipmentStatus : Number(rawequipmentStatus)
  }
  if ('status' in payload) {
    const rawstatus = payload.status
    payload.status = typeof rawstatus === 'number' ? rawstatus : Number(rawstatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.equipmentOperationRateId)
  childEquipmentOperationRateChangeLogRows.value = []
  equipmentOperationRateChangeLogTableRef.value?.resetRows?.()
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
