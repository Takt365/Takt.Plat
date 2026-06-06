<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-dept/components -->
<!-- 文件名称：ec-dept-form.vue -->
<!-- 功能描述：设变-部门通用实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ec-dept-form-tabs"
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
                :label="t('entity.ecDept.ecndetailid')"
                name="ecnDetailId"
              >
                <a-input
                  v-model:value="formState.ecnDetailId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.ecndetailid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.ecno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.deptcode')"
                name="deptCode"
              >
                <a-input
                  v-model:value="formState.deptCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.deptcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.isimplemented')"
                name="isImplemented"
              >
                <a-input-number
                  v-model:value="formState.isImplemented"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.isimplemented') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ecDept.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecDept.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.scheduledproductiondate')"
                name="scheduledProductionDate"
              >
                <a-date-picker
                  v-model:value="formState.scheduledProductionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.scheduledproductiondate') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.scheduledbatch')"
                name="scheduledBatch"
              >
                <a-input
                  v-model:value="formState.scheduledBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.scheduledbatch') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.poremainder')"
                name="poRemainder"
              >
                <a-input
                  v-model:value="formState.poRemainder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.poremainder') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.balance')"
                name="balance"
              >
                <a-input
                  v-model:value="formState.balance"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.balance') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.oldproducthandling')"
                name="oldProductHandling"
              >
                <a-input
                  v-model:value="formState.oldProductHandling"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.oldproducthandling') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.purchaseorderissuedate')"
                name="purchaseOrderIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseOrderIssueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.purchaseorderissuedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.supplier')"
                name="supplier"
              >
                <a-input
                  v-model:value="formState.supplier"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.supplier') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.purchaseorderno')"
                name="purchaseOrderNo"
              >
                <a-input
                  v-model:value="formState.purchaseOrderNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.purchaseorderno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.iqcorderno')"
                name="iqcOrderNo"
              >
                <a-input
                  v-model:value="formState.iqcOrderNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.iqcorderno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.outboundbatch')"
                name="outboundBatch"
              >
                <a-input
                  v-model:value="formState.outboundBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.outboundbatch') })"
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
                :label="t('entity.ecDept.outbounddate')"
                name="outboundDate"
              >
                <a-date-picker
                  v-model:value="formState.outboundDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.outbounddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.productiondate')"
                name="productionDate"
              >
                <a-date-picker
                  v-model:value="formState.productionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.productiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.productionbatch')"
                name="productionBatch"
              >
                <a-input
                  v-model:value="formState.productionBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.productionbatch') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.outboundorderno')"
                name="outboundOrderNo"
              >
                <a-input
                  v-model:value="formState.outboundOrderNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.outboundorderno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.productionteam')"
                name="productionTeam"
              >
                <a-input
                  v-model:value="formState.productionTeam"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.productionteam') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.implementationdate')"
                name="implementationDate"
              >
                <a-date-picker
                  v-model:value="formState.implementationDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDept.implementationdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.inspectionbatch')"
                name="inspectionBatch"
              >
                <a-input
                  v-model:value="formState.inspectionBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.inspectionbatch') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.samplingno')"
                name="samplingNo"
              >
                <a-input
                  v-model:value="formState.samplingNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.samplingno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDept.issopupdated')"
                name="isSopUpdated"
              >
                <a-input-number
                  v-model:value="formState.isSopUpdated"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDept.issopupdated') })"
                  size="small"
                  style="width: 100%"
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
 * 设变-部门通用实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-dept/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcDeptCreate } from '@/types/logistics/manufacturing/engineering-change/ec-dept'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ecnDetailId","ecNo","lineNumber","deptCode","isImplemented","content","scheduledProductionDate","scheduledBatch","poRemainder","balance","oldProductHandling","purchaseOrderIssueDate","supplier","purchaseOrderNo","iqcOrderNo","inspectionDate","outboundBatch","outboundDate","productionDate","productionBatch","outboundOrderNo","productionTeam","implementationDate","inspectionBatch","samplingNo","isSopUpdated","extFieldJson","remark"]


interface Props {
  formData?: Partial<EcDeptCreate & { ecDeptId?: string }> | null
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
    const isCreate = !props.formData?.ecDeptId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  ecnDetailId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDept.ecndetailid') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDept.ecno') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDept.linenumber') }),
      trigger: 'change'
    }
  ],
  deptCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDept.deptcode') }),
      trigger: 'blur'
    }
  ],
  isImplemented: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDept.isimplemented') }),
      trigger: 'change'
    }
  ],
  isSopUpdated: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDept.issopupdated') }),
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
