<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection-detail/components -->
<!-- 文件名称：pcba-inspection-detail-form.vue -->
<!-- 功能描述：PCBA检查明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="pcba-inspection-detail-form-tabs"
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
                :label="t('entity.pcbaInspectionDetail.pcbainspectionid')"
                name="pcbaInspectionId"
              >
                <a-input
                  v-model:value="formState.pcbaInspectionId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.pcbainspectionid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.pcbaboardtype')"
                name="pcbaBoardType"
              >
                <a-input
                  v-model:value="formState.pcbaBoardType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.pcbaboardtype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.visualinspectionline')"
                name="visualInspectionLine"
              >
                <a-input
                  v-model:value="formState.visualInspectionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.visualinspectionline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.aoiline')"
                name="aoiLine"
              >
                <a-input
                  v-model:value="formState.aoiLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.aoiline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.bsideassemblydate')"
                name="bSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.bSideAssemblyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.bsideassemblydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.pcbaInspectionDetail.tsideassemblydate')"
                name="tSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.tSideAssemblyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.tsideassemblydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.inspectorname')"
                name="inspectorName"
              >
                <a-input
                  v-model:value="formState.inspectorName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.inspectorname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.dailycompletedqty')"
                name="dailyCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.dailyCompletedQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.dailycompletedqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.inspectionqty')"
                name="inspectionQty"
              >
                <a-input-number
                  v-model:value="formState.inspectionQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.inspectionqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.inspectionstatus')"
                name="inspectionStatus"
              >
                <a-input-number
                  v-model:value="formState.inspectionStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.inspectionstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.prodline')"
                name="prodLine"
              >
                <a-input
                  v-model:value="formState.prodLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.prodline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.inspectionworkhours')"
                name="inspectionWorkHours"
              >
                <a-input-number
                  v-model:value="formState.inspectionWorkHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.inspectionworkhours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.aoiworkhours')"
                name="aoiWorkHours"
              >
                <a-input-number
                  v-model:value="formState.aoiWorkHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.aoiworkhours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.defectqty')"
                name="defectQty"
              >
                <a-input-number
                  v-model:value="formState.defectQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.defectqty') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.handplacement')"
                name="handPlacement"
              >
                <a-input
                  v-model:value="formState.handPlacement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.handplacement') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.serialnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.pcbaInspectionDetail.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspectionDetail.defectlocation')"
                name="defectLocation"
              >
                <a-input
                  v-model:value="formState.defectLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.defectlocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
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
 * PCBA检查明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection-detail/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaInspectionDetailCreate } from '@/types/logistics/manufacturing/defect/pcba-inspection-detail'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","pcbaInspectionId","prodOrderCode","lineNumber","pcbaBoardType","visualInspectionLine","aoiLine","bSideAssemblyDate","tSideAssemblyDate","shiftNo","inspectorName","dailyCompletedQty","inspectionQty","inspectionStatus","prodLine","inspectionWorkHours","aoiWorkHours","defectQty","handPlacement","serialNumber","content","defectLocation","extFieldJson","remark"]


interface Props {
  formData?: Partial<PcbaInspectionDetailCreate & { pcbaInspectionDetailId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

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

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.pcbaInspectionDetailId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  pcbaInspectionId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.pcbainspectionid') }),
      trigger: 'blur'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspectionDetail.prodordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.linenumber') }),
      trigger: 'change'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.shiftno') }),
      trigger: 'change'
    }
  ],
  dailyCompletedQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.dailycompletedqty') }),
      trigger: 'change'
    }
  ],
  inspectionQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.inspectionqty') }),
      trigger: 'change'
    }
  ],
  inspectionStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.inspectionstatus') }),
      trigger: 'change'
    }
  ],
  inspectionWorkHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.inspectionworkhours') }),
      trigger: 'change'
    }
  ],
  aoiWorkHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.aoiworkhours') }),
      trigger: 'change'
    }
  ],
  defectQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspectionDetail.defectqty') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

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
