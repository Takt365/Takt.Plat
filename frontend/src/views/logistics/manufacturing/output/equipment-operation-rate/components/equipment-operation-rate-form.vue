<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/equipment-operation-rate/components -->
<!-- 文件名称：equipment-operation-rate-form.vue -->
<!-- 功能描述：机器稼动率实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.timecategory')"
                name="timeCategory"
              >
                <a-input-number
                  v-model:value="formState.timeCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.timecategory') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.enddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.weeknumber')"
                name="weekNumber"
              >
                <a-input-number
                  v-model:value="formState.weekNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.weeknumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.monthnumber')"
                name="monthNumber"
              >
                <a-input-number
                  v-model:value="formState.monthNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.monthnumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmentcode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentcode') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.equipmentOperationRate.equipmentname')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmenttype')"
                name="equipmentType"
              >
                <a-input-number
                  v-model:value="formState.equipmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmenttype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.productionline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.plannedruntime')"
                name="plannedRuntime"
              >
                <a-input-number
                  v-model:value="formState.plannedRuntime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plannedruntime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.actualruntime')"
                name="actualRuntime"
              >
                <a-input-number
                  v-model:value="formState.actualRuntime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.actualruntime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.downtime')"
                name="downtime"
              >
                <a-input-number
                  v-model:value="formState.downtime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmentoperationrate')"
                name="equipmentOperationRate"
              >
                <a-input-number
                  v-model:value="formState.equipmentOperationRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentoperationrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.plannedoutput')"
                name="plannedOutput"
              >
                <a-input-number
                  v-model:value="formState.plannedOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plannedoutput') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.actualoutput')"
                name="actualOutput"
              >
                <a-input-number
                  v-model:value="formState.actualOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.actualoutput') })"
                  size="small"
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
                :label="t('entity.equipmentOperationRate.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.qualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.defectivequantity')"
                name="defectiveQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectiveQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.defectivequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.yieldrate')"
                name="yieldRate"
              >
                <a-input-number
                  v-model:value="formState.yieldRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.yieldrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.downtimereasontype')"
                name="downtimeReasonType"
              >
                <a-input-number
                  v-model:value="formState.downtimeReasonType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtimereasontype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.downtimereason')"
                name="downtimeReason"
              >
                <a-input
                  v-model:value="formState.downtimeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtimereason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmentstatus')"
                name="equipmentStatus"
              >
                <a-input-number
                  v-model:value="formState.equipmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmentoperator')"
                name="equipmentOperator"
              >
                <a-input
                  v-model:value="formState.equipmentOperator"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentoperator') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.equipmentmaintainer')"
                name="equipmentMaintainer"
              >
                <a-input
                  v-model:value="formState.equipmentMaintainer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentmaintainer') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.teamleader')"
                name="teamLeader"
              >
                <a-input
                  v-model:value="formState.teamLeader"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.teamleader') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipmentOperationRate.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.status') })"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 机器稼动率实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/equipment-operation-rate/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EquipmentOperationRateCreate } from '@/types/logistics/manufacturing/output/equipment-operation-rate'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","timeCategory","startDate","endDate","weekNumber","monthNumber","equipmentCode","equipmentName","equipmentType","productionLine","shiftNo","plannedRuntime","actualRuntime","downtime","equipmentOperationRate","plannedOutput","actualOutput","qualifiedQuantity","defectiveQuantity","yieldRate","downtimeReasonType","downtimeReason","equipmentStatus","equipmentOperator","equipmentMaintainer","teamLeader","status","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EquipmentOperationRateCreate & { equipmentOperationRateId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
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
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plantcode') }),
      trigger: 'blur'
    }
  ],
  timeCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.timecategory') }),
      trigger: 'change'
    }
  ],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.enddate') }),
      trigger: 'change'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentcode') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentname') }),
      trigger: 'blur'
    }
  ],
  equipmentType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.equipmenttype') }),
      trigger: 'change'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.shiftno') }),
      trigger: 'change'
    }
  ],
  plannedRuntime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.plannedruntime') }),
      trigger: 'change'
    }
  ],
  actualRuntime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.actualruntime') }),
      trigger: 'change'
    }
  ],
  downtime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.downtime') }),
      trigger: 'change'
    }
  ],
  equipmentOperationRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.equipmentoperationrate') }),
      trigger: 'change'
    }
  ],
  plannedOutput: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.plannedoutput') }),
      trigger: 'change'
    }
  ],
  actualOutput: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.actualoutput') }),
      trigger: 'change'
    }
  ],
  qualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.qualifiedquantity') }),
      trigger: 'change'
    }
  ],
  defectiveQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.defectivequantity') }),
      trigger: 'change'
    }
  ],
  yieldRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.yieldrate') }),
      trigger: 'change'
    }
  ],
  equipmentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.equipmentstatus') }),
      trigger: 'change'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.status') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return { ...formState }
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
