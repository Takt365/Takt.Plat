<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract/components -->
<!-- 文件名称：service-request-form.vue -->
<!-- 功能描述：服务合同实体子表 serviceRequest 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
      <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.code')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.serviceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.requestdate')"
                name="requestDate"
              >
                <a-date-picker
                  v-model:value="formState.requestDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.expectedservicedate')"
                name="expectedServiceDate"
              >
                <a-date-picker
                  v-model:value="formState.expectedServiceDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.servicerequest.expectedservicedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.servicerequest.requesttype')"
                name="requestType"
              >
                <a-input-number
                  v-model:value="formState.requestType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requesttype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
      </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 服务合同实体子表 serviceRequest 维护表单
 * @module views/logistics/service/service-contract/components
 */
import { reactive, ref, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type {
  ServiceRequest,
  ServiceRequestCreate,
  ServiceRequestUpdate,
} from '@/types/logistics/customer-service/service-request'

/** i18n */
const { t } = useI18n()

interface Props {
  formData?: Partial<ServiceRequest> | null
  masterId?: string
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  masterId: '',
  loading: false,
})

const formRef = ref()
/** 表单双向绑定（与主表 *-form 一致，避免 Partial 在模板 v-model 推断为 unknown） */
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    Object.keys(formState).forEach((k) => delete formState[k])
    Object.assign(formState, val ? { ...val } : {})
  },
  { immediate: true, deep: true },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.plantcode') }),
      trigger: 'blur',
    },
  ],
  serviceRequestCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.code') }),
      trigger: 'blur',
    },
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientid') }),
      trigger: 'blur',
    },
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientcode') }),
      trigger: 'blur',
    },
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.clientname') }),
      trigger: 'blur',
    },
  ],
  requestDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.servicerequest.requestdate') }),
      trigger: 'change',
    },
  ],
  requestType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.servicerequest.requesttype') }),
      trigger: 'change',
    },
  ],
}))

async function validate() {
  await formRef.value?.validate()
}

function getValues(): ServiceRequestCreate | ServiceRequestUpdate {
  return {
    ...(formState as ServiceRequestCreate),
    serviceContractId: props.masterId,
  } as ServiceRequestCreate
}

function resetFields() {
  formRef.value?.resetFields()
}

defineExpose({ validate, getValues, resetFields })
</script>
