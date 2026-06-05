<template>
  <a-tabs v-model:active-key="activeTab">
    <a-tab-pane
      key="basic"
      :tab="t('common.page.form.tabs.basicinfo')"
    >
      <div :class="formContentClass">
        <a-form
          ref="formRef"
          :model="formState"
          :rules="rules"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 18 }"
          layout="horizontal"
          label-align="right"
        >
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.name')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.name') })"
                  show-count
                  :maxlength="50"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.code')"
                name="deptCode"
              >
                <a-input
                  v-model:value="formState.deptCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.code') })"
                  show-count
                  :maxlength="50"
                  :disabled="!!formData?.deptId"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.dept.parentid')"
                name="parentId"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="/api/TaktDepts/tree-options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.parentid') })"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.headuserid')"
                name="headUserId"
              >
                <TaktSelect
                  v-model:value="formState.headUserId"
                  api-url="/api/TaktUsers/options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.headuserid') })"
                  show-search
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.costcentercode')"
                name="costCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.costCenterCode"
                  api-url="/api/TaktCostCenters/options"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.costcentercode') })"
                  show-search
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.costcategory')"
                name="costCategory"
              >
                <a-select
                  v-model:value="formState.costCategory"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.costcategory') })"
                  :options="costCategoryOptions"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.ordernumhint')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.phone')"
                name="phone"
              >
                <a-input
                  v-model:value="formState.phone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.phone') })"
                  show-count
                  :maxlength="50"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.dept.email')"
                name="email"
              >
                <a-input
                  v-model:value="formState.email"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.email') })"
                  show-count
                  :maxlength="100"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.dept.location')"
                name="location"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <a-input
                  v-model:value="formState.location"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.location') })"
                  show-count
                  :maxlength="200"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.dept.description')"
                name="description"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.description') })"
                  :rows="2"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.input', { field: t('common.page.entity.remark') })"
                  :rows="2"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.dept.status')"
                name="deptStatus"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktSelect
                  v-model:value="formState.deptStatus"
                  api-url="/api/TaktDictDatas/options?dictTypeCode=sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.status') })"
                  :field-names="{ label: 'dictLabel', value: 'extLabel' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </div>
    </a-tab-pane>
  </a-tabs>
</template>

<script setup lang="ts">
import { reactive, watch, ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { Dept, DeptCreate } from '@/types/human-resource/organization/dept'

const { t } = useI18n()

/** 费用类别（与后端 TaktCostCategory 一致：1=直接，2=间接） */
const costCategoryOptions = computed(() => [
  { label: t('entity.dept.costcategory') + ' (1)', value: 1 },
  { label: t('entity.dept.costcategory') + ' (2)', value: 2 },
])

interface Props {
  formData?: Partial<Dept>
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

const formRef = ref()
const activeTab = ref('basic')
const TOTAL_FIELDS = 12
const formContentClass = computed(() => (TOTAL_FIELDS >= 30 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))

interface FormState {
  deptName: string
  deptCode: string
  parentId: string | number
  headUserId?: string | number
  costCenterCode: string
  costCategory: number
  phone: string
  email: string
  location: string
  sortOrder: number
  description: string
  deptStatus: number
  remark: string
}

/**
 * 创建空表单状态
 * @returns {FormState} 默认值
 */
function createEmptyFormState(): FormState {
  return {
    deptName: '',
    deptCode: '',
    parentId: '0',
    headUserId: undefined,
    costCenterCode: '',
    costCategory: 1,
    phone: '',
    email: '',
    location: '',
    sortOrder: 0,
    description: '',
    deptStatus: 1,
    remark: '',
  }
}

const formState = reactive<FormState>(createEmptyFormState())

const rules = computed<Record<string, Rule[]>>(() => ({
  deptName: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.name') }), trigger: 'blur' }],
  deptCode: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.code') }), trigger: 'blur' }],
  costCenterCode: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.dept.costcentercode') }), trigger: 'change' }],
  headUserId: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.dept.headuserid') }), trigger: 'change' }],
  costCategory: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.dept.costcategory') }), trigger: 'change' }],
  phone: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.phone') }), trigger: 'blur' }],
  email: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.email') }), trigger: 'blur' }],
  location: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.location') }), trigger: 'blur' }],
  description: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dept.description') }), trigger: 'blur' }],
  deptStatus: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.dept.status') }), trigger: 'change' }],
}))

watch(() => props.formData, (newData) => {
  if (newData && Object.keys(newData).length > 0) {
    Object.assign(formState, {
      deptName: newData.deptName ?? '',
      deptCode: newData.deptCode ?? '',
      parentId: newData.parentId != null ? String(newData.parentId) : '0',
      headUserId: newData.headUserId != null && newData.headUserId !== '' ? newData.headUserId : undefined,
      costCenterCode: newData.costCenterCode ?? '',
      costCategory: newData.costCategory ?? 1,
      phone: newData.phone ?? '',
      email: newData.email ?? '',
      location: newData.location ?? '',
      sortOrder: newData.sortOrder ?? 0,
      description: newData.description ?? '',
      deptStatus: newData.deptStatus ?? 1,
      remark: newData.remark ?? '',
    })
  } else {
    Object.assign(formState, createEmptyFormState())
  }
  activeTab.value = 'basic'
}, { immediate: true, deep: true })

const validate = async () => {
  await formRef.value?.validate()
}

/**
 * 获取部门提交载荷（字段与 TaktDeptCreateDto / DeptCreate 一致）
 * @returns {DeptCreate} 创建/更新 DTO
 */
const getValues = (): DeptCreate => {
  const p = formState.parentId
  const parentId = p === '' || p === undefined || p === null ? '0' : String(p)
  return {
    deptName: formState.deptName,
    deptCode: formState.deptCode,
    parentId,
    costCenterCode: String(formState.costCenterCode ?? '').trim(),
    costCategory: formState.costCategory,
    headUserId: formState.headUserId != null && formState.headUserId !== '' ? String(formState.headUserId) : '',
    phone: formState.phone,
    email: formState.email,
    location: formState.location,
    deptStatus: formState.deptStatus,
    isBuiltIn: props.formData?.isBuiltIn ?? 0,
    sortOrder: formState.sortOrder,
    description: formState.description,
    remark: formState.remark.trim() || undefined,
  }
}

const resetFields = () => {
  formRef.value?.resetFields()
  Object.assign(formState, createEmptyFormState())
  activeTab.value = 'basic'
}

defineExpose({ validate, getValues, resetFields })
</script>
