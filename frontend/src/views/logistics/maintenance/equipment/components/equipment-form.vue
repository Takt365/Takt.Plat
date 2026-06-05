<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/equipment/components -->
<!-- 文件名称：equipment-form.vue -->
<!-- 功能描述：Takt工厂设备实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="equipment-form-tabs"
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
                :label="t('entity.equipment.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.code')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.name')"
                name="equipmentName"
              >
                <a-input
                  v-model:value="formState.equipmentName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.type')"
                name="equipmentType"
              >
                <a-input-number
                  v-model:value="formState.equipmentType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.model')"
                name="equipmentModel"
              >
                <a-input
                  v-model:value="formState.equipmentModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.model') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.specification')"
                name="equipmentSpecification"
              >
                <a-input
                  v-model:value="formState.equipmentSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.specification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.brand')"
                name="equipmentBrand"
              >
                <a-input
                  v-model:value="formState.equipmentBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.brand') })"
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
                :label="t('entity.equipment.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.manufacturer') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.dealerby')"
                name="dealerBy"
              >
                <a-input
                  v-model:value="formState.dealerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.dealerby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.serialnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workshopby')"
                name="workshopBy"
              >
                <a-input
                  v-model:value="formState.workshopBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workshopby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.productionlineby')"
                name="productionLineBy"
              >
                <a-input
                  v-model:value="formState.productionLineBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.productionlineby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.workstationby')"
                name="workstationBy"
              >
                <a-input
                  v-model:value="formState.workstationBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workstationby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.deptby')"
                name="deptBy"
              >
                <a-input
                  v-model:value="formState.deptBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.deptby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.location')"
                name="equipmentLocation"
              >
                <a-input
                  v-model:value="formState.equipmentLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.location') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.responsibleuserby')"
                name="responsibleUserBy"
              >
                <a-input
                  v-model:value="formState.responsibleUserBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.responsibleuserby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.operatorby')"
                name="operatorBy"
              >
                <a-input
                  v-model:value="formState.operatorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.operatorby') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.equipment.purchasedate')"
                name="purchaseDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.purchasedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.installationdate')"
                name="installationDate"
              >
                <a-date-picker
                  v-model:value="formState.installationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.installationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantystartdate')"
                name="warrantyStartDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystartdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.warrantyenddate')"
                name="warrantyEndDate"
              >
                <a-date-picker
                  v-model:value="formState.warrantyEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantyenddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.originalvalue')"
                name="equipmentOriginalValue"
              >
                <a-input-number
                  v-model:value="formState.equipmentOriginalValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.originalvalue') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.technicalparameters')"
                name="technicalParameters"
              >
                <a-input
                  v-model:value="formState.technicalParameters"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.technicalparameters') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.images')"
                name="equipmentImages"
              >
                <a-input
                  v-model:value="formState.equipmentImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.images') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.documents')"
                name="equipmentDocuments"
              >
                <a-input
                  v-model:value="formState.equipmentDocuments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.documents') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.iscritical')"
                name="isCritical"
              >
                <a-input-number
                  v-model:value="formState.isCritical"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.iscritical') })"
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
                :label="t('entity.equipment.warrantystatus')"
                name="warrantyStatus"
              >
                <a-input-number
                  v-model:value="formState.warrantyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.warrantystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.status')"
                name="equipmentStatus"
              >
                <a-input-number
                  v-model:value="formState.equipmentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.equipment.maintenancerecords')"
                name="maintenanceRecords"
              >
                <a-input
                  v-model:value="formState.maintenanceRecords"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.maintenancerecords') })"
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
 * Takt工厂设备实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/maintenance/equipment/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EquipmentCreate } from '@/types/logistics/maintenance/equipment'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","equipmentCode","equipmentName","equipmentType","equipmentModel","equipmentSpecification","equipmentBrand","manufacturer","dealerBy","serialNumber","workshopBy","productionLineBy","workstationBy","deptBy","equipmentLocation","responsibleUserBy","operatorBy","purchaseDate","installationDate","startDate","warrantyStartDate","warrantyEndDate","equipmentOriginalValue","technicalParameters","equipmentImages","equipmentDocuments","isCritical","warrantyStatus","equipmentStatus","maintenanceRecords","extFieldJson","remark"]


interface Props {
  formData?: Partial<EquipmentCreate & { equipmentId?: string }> | null
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
    const isCreate = !props.formData?.equipmentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.plantcode') }),
      trigger: 'blur'
    }
  ],
  equipmentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.code') }),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.equipment.name') }),
      trigger: 'blur'
    }
  ],
  equipmentType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.type') }),
      trigger: 'change'
    }
  ],
  equipmentOriginalValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.originalvalue') }),
      trigger: 'change'
    }
  ],
  isCritical: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.iscritical') }),
      trigger: 'change'
    }
  ],
  warrantyStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystatus') }),
      trigger: 'change'
    }
  ],
  equipmentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.equipment.status') }),
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
