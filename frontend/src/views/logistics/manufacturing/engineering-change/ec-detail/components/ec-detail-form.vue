<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-detail/components -->
<!-- 文件名称：ec-detail-form.vue -->
<!-- 功能描述：设变维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ec-detail-form-tabs"
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
                :label="t('entity.ecDetail.ecid')"
                name="ecId"
              >
                <a-input
                  v-model:value="formState.ecId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecmodel')"
                name="ecModel"
              >
                <a-input
                  v-model:value="formState.ecModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecmodel') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecbomitem')"
                name="ecBomItem"
              >
                <a-input
                  v-model:value="formState.ecBomItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomitem') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecbomsubitem')"
                name="ecBomSubItem"
              >
                <a-input
                  v-model:value="formState.ecBomSubItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomsubitem') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecbomno')"
                name="ecBomNo"
              >
                <a-input
                  v-model:value="formState.ecBomNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecbomno') })"
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
                :label="t('entity.ecDetail.ecchange')"
                name="ecChange"
              >
                <a-input
                  v-model:value="formState.ecChange"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecchange') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.eclocal')"
                name="ecLocal"
              >
                <a-input
                  v-model:value="formState.ecLocal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.eclocal') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ecDetail.ecnote')"
                name="ecNote"
              >
                <a-textarea
                  v-model:value="formState.ecNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecDetail.ecnote') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecprocess')"
                name="ecProcess"
              >
                <a-input
                  v-model:value="formState.ecProcess"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecprocess') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecbomdate')"
                name="ecBomDate"
              >
                <a-date-picker
                  v-model:value="formState.ecBomDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDetail.ecbomdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecentrydate')"
                name="ecEntryDate"
              >
                <a-date-picker
                  v-model:value="formState.ecEntryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecDetail.ecentrydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecolditem')"
                name="ecOldItem"
              >
                <a-input
                  v-model:value="formState.ecOldItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecolditem') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecoldtext')"
                name="ecOldText"
              >
                <a-input
                  v-model:value="formState.ecOldText"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecoldtext') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecoldqty')"
                name="ecOldQty"
              >
                <a-input-number
                  v-model:value="formState.ecOldQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecoldqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecoldset')"
                name="ecOldSet"
              >
                <a-input
                  v-model:value="formState.ecOldSet"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecoldset') })"
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
                :label="t('entity.ecDetail.ecnewitem')"
                name="ecNewItem"
              >
                <a-input
                  v-model:value="formState.ecNewItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecnewitem') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecnewtext')"
                name="ecNewText"
              >
                <a-input
                  v-model:value="formState.ecNewText"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecnewtext') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecnewqty')"
                name="ecNewQty"
              >
                <a-input-number
                  v-model:value="formState.ecNewQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecnewqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecnewset')"
                name="ecNewSet"
              >
                <a-input
                  v-model:value="formState.ecNewSet"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecnewset') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.isprocurement')"
                name="isProcurement"
              >
                <a-input-number
                  v-model:value="formState.isProcurement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.isprocurement') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ischeck')"
                name="isCheck"
              >
                <a-input-number
                  v-model:value="formState.isCheck"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ischeck') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.ecwarehouse')"
                name="ecWarehouse"
              >
                <a-input
                  v-model:value="formState.ecWarehouse"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecwarehouse') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.isendofline')"
                name="isEndOfLine"
              >
                <a-input-number
                  v-model:value="formState.isEndOfLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.isendofline') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecDetail.deptrecords')"
                name="deptRecords"
              >
                <a-input
                  v-model:value="formState.deptRecords"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecDetail.deptrecords') })"
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
 * 设变维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-detail/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcDetailCreate } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ecId","ecNo","lineNumber","ecModel","ecBomItem","ecBomSubItem","ecBomNo","ecChange","ecLocal","ecNote","ecProcess","ecBomDate","ecEntryDate","ecOldItem","ecOldText","ecOldQty","ecOldSet","ecNewItem","ecNewText","ecNewQty","ecNewSet","isProcurement","isCheck","ecWarehouse","isEndOfLine","deptRecords","extFieldJson","remark"]


interface Props {
  formData?: Partial<EcDetailCreate & { ecDetailId?: string }> | null
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
    const isCreate = !props.formData?.ecDetailId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  ecId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecid') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecno') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.linenumber') }),
      trigger: 'change'
    }
  ],
  ecModel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecDetail.ecmodel') }),
      trigger: 'blur'
    }
  ],
  ecBomDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.ecbomdate') }),
      trigger: 'change'
    }
  ],
  ecEntryDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.ecentrydate') }),
      trigger: 'change'
    }
  ],
  isProcurement: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.isprocurement') }),
      trigger: 'change'
    }
  ],
  isCheck: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.ischeck') }),
      trigger: 'change'
    }
  ],
  isEndOfLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecDetail.isendofline') }),
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
