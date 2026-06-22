<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：overtime-item-form.vue -->
<!-- 功能描述：加班申请子表 overtimeItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields -->
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
                :label="t('entity.overtimeitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeename') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.plannedhours')"
                name="plannedHours"
              >
                <a-input-number
                  v-model:value="formState.plannedHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.plannedhours') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.actualstarttime')"
                name="actualStartTime"
              >
                <a-input
                  v-model:value="formState.actualStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.actualstarttime') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.actualendtime')"
                name="actualEndTime"
              >
                <a-input
                  v-model:value="formState.actualEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.actualendtime') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.actualhours')"
                name="actualHours"
              >
                <a-input-number
                  v-model:value="formState.actualHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.actualhours') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.overtimeitem.extfield')"
                name="ExtField"
              >
                <a-textarea
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.overtimeitem.extfield') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
      </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 加班申请子表 overtimeItem 维护表单
 * @module views/human-resource/attendance/overtime/components
 */
import { reactive, ref, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type {
  OvertimeItem,
  OvertimeItemCreate,
  OvertimeItemUpdate,
} from '@/types/human-resource/attendance/overtime-item'

/** i18n */
const { t } = useI18n()

interface Props {
  formData?: Partial<OvertimeItem> | null
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
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.linenumber') }),
      trigger: 'change',
    },
  ],
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeeid') }),
      trigger: 'blur',
    },
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.employeename') }),
      trigger: 'blur',
    },
  ],
  plannedHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtimeitem.plannedhours') }),
      trigger: 'change',
    },
  ],
}))

async function validate() {
  await formRef.value?.validate()
}

function getValues(): OvertimeItemCreate | OvertimeItemUpdate {
  return {
    ...(formState as OvertimeItemCreate),
    overtimeId: props.masterId,
  } as OvertimeItemCreate
}

function resetFields() {
  formRef.value?.resetFields()
}

defineExpose({ validate, getValues, resetFields })
</script>
