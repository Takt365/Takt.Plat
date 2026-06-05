<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/maintenance/components -->
<!-- 文件名称：maintenance-form.vue -->
<!-- 功能描述：Takt设备维护记录实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="maintenance-form-tabs"
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
                :label="t('entity.maintenance.equipmentid')"
                name="equipmentId"
              >
                <a-input
                  v-model:value="formState.equipmentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.equipmentcode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.type')"
                name="maintenanceType"
              >
                <a-input-number
                  v-model:value="formState.maintenanceType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.company')"
                name="maintenanceCompany"
              >
                <a-input
                  v-model:value="formState.maintenanceCompany"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.company') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.technician')"
                name="maintenanceTechnician"
              >
                <a-input
                  v-model:value="formState.maintenanceTechnician"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.technician') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.date')"
                name="maintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.maintenanceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.date') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.starttime')"
                name="maintenanceStartTime"
              >
                <a-input
                  v-model:value="formState.maintenanceStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.starttime') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.endtime')"
                name="maintenanceEndTime"
              >
                <a-input
                  v-model:value="formState.maintenanceEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.endtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenance.content')"
                name="maintenanceContent"
              >
                <a-textarea
                  v-model:value="formState.maintenanceContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenance.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.maintenance.faultdescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenance.faultdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.solution')"
                name="solution"
              >
                <a-input
                  v-model:value="formState.solution"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.solution') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.usedparts')"
                name="usedParts"
              >
                <a-input
                  v-model:value="formState.usedParts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.usedparts') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.cost')"
                name="maintenanceCost"
              >
                <a-input-number
                  v-model:value="formState.maintenanceCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.cost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.result')"
                name="maintenanceResult"
              >
                <a-input-number
                  v-model:value="formState.maintenanceResult"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.result') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.status')"
                name="maintenanceStatus"
              >
                <a-input-number
                  v-model:value="formState.maintenanceStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.nextmaintenancedate')"
                name="nextMaintenanceDate"
              >
                <a-date-picker
                  v-model:value="formState.nextMaintenanceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.nextmaintenancedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.cycledays')"
                name="maintenanceCycleDays"
              >
                <a-input-number
                  v-model:value="formState.maintenanceCycleDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.cycledays') })"
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
                :label="t('entity.maintenance.documents')"
                name="maintenanceDocuments"
              >
                <a-input
                  v-model:value="formState.maintenanceDocuments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.documents') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.images')"
                name="maintenanceImages"
              >
                <a-input
                  v-model:value="formState.maintenanceImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.images') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.acceptedsummary')"
                name="acceptedSummary"
              >
                <a-input
                  v-model:value="formState.acceptedSummary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedsummary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.acceptedby')"
                name="acceptedBy"
              >
                <a-input
                  v-model:value="formState.acceptedBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenance.acceptedat')"
                name="acceptedAt"
              >
                <a-input
                  v-model:value="formState.acceptedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedat') })"
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
 * Takt设备维护记录实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/maintenance/maintenance/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaintenanceCreate } from '@/types/logistics/maintenance/maintenance'
import { useTenantStore } from '@/stores/identity/tenant'

const { t } = useI18n()

const tenantStore = useTenantStore()

/** 只读隔离字段：新增/编辑时展示当前租户与公司（不可改） */
function applyScopeDefaults(target: Record<string, unknown>) {
  if (formFields.includes('tenantCode') && !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
}
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","equipmentId","equipmentCode","lineNumber","maintenanceType","maintenanceCompany","maintenanceTechnician","maintenanceDate","maintenanceStartTime","maintenanceEndTime","maintenanceContent","faultDescription","solution","usedParts","maintenanceCost","maintenanceResult","maintenanceStatus","nextMaintenanceDate","maintenanceCycleDays","maintenanceDocuments","maintenanceImages","acceptedSummary","acceptedBy","acceptedAt","extFieldJson","remark"]


interface Props {
  formData?: Partial<MaintenanceCreate & { maintenanceId?: string }> | null
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

const rules = computed<Record<string, Rule[]>>(() => ({
  equipmentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentid') }),
      trigger: 'blur'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.linenumber') }),
      trigger: 'change'
    }
  ],
  maintenanceType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.type') }),
      trigger: 'change'
    }
  ],
  maintenanceDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.date') }),
      trigger: 'change'
    }
  ],
  maintenanceCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.cost') }),
      trigger: 'change'
    }
  ],
  maintenanceResult: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.result') }),
      trigger: 'change'
    }
  ],
  maintenanceStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.status') }),
      trigger: 'change'
    }
  ],
  maintenanceCycleDays: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.maintenance.cycledays') }),
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
