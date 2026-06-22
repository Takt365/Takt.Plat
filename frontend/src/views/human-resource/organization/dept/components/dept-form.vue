<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/dept/components -->
<!-- 文件名称：dept-form.vue -->
<!-- 功能描述：部门实体树表弹窗内嵌表单。defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="24">
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
      <a-col :span="24">
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
      <a-col :span="24">
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
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.name')"
          name="deptName"
        >
          <a-input
            v-model:value="formState.deptName"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.name') })"
            show-count
            :maxlength="100"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.code')"
          name="deptCode"
        >
          <a-input
            v-model:value="formState.deptCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.code') })"
            show-count
            :maxlength="50"
            allow-clear
            :disabled="!!formData?.deptId"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.parentid')"
          name="parentId"
        >
          <TaktTreeSelect
            v-model:value="formState.parentId"
            api-url="TaktDepts/tree-options"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.parentid') })"
            allow-clear
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.headuserid')"
          name="headUserId"
        >
          <TaktSelect
            v-model:value="formState.headUserId"
            api-url="TaktUsers/options"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.headuserid') })"
            show-search
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.costcentercode')"
          name="costCenterCode"
        >
          <TaktSelect
            v-model:value="formState.costCenterCode"
            api-url="TaktCostCenters/options"
            allow-clear
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.costcentercode') })"
            show-search
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.costcategory')"
          name="costCategory"
        >
          <a-select
            v-model:value="formState.costCategory"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.costcategory') })"
            :options="costCategoryOptions"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
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
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.phone')"
          name="phone"
        >
          <a-input
            v-model:value="formState.phone"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.phone') })"
            show-count
            :maxlength="20"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.email')"
          name="email"
        >
          <a-input
            v-model:value="formState.email"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.email') })"
            show-count
            :maxlength="100"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.location')"
          name="location"
        >
          <a-input
            v-model:value="formState.location"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dept.location') })"
            show-count
            :maxlength="200"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.description')"
          name="description"
        >
          <a-textarea
            v-model:value="formState.description"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.dept.description') })"
            :rows="2"
            show-count
            :maxlength="500"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.dept.status')"
          name="deptStatus"
        >
          <TaktSelect
            v-model:value="formState.deptStatus"
            dict-type="sys_normal_disable_status"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dept.status') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 部门树表弹窗内嵌表单
 * @module views/human-resource/organization/dept/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DeptCreate } from '@/types/human-resource/organization/dept'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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

/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = [
  'tenantCode',
  'companyCode',
  'companyDefaultCulture',
  'deptName',
  'deptCode',
  'parentId',
  'headUserId',
  'costCenterCode',
  'costCategory',
  'sortOrder',
  'phone',
  'email',
  'location',
  'description',
  'deptStatus',
  'remark',
]

/** 费用类别（与实体 TaktDept.CostCategory 一致：1=直接，2=间接） */
const costCategoryOptions = computed(() => [
  { label: t('entity.dept.costcategory') + ' (1)', value: 1 },
  { label: t('entity.dept.costcategory') + ' (2)', value: 2 },
])

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DeptCreate & { deptId?: string; isBuiltIn?: number }> | null
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

/** 表单字段默认值（与 TaktDept 实体 DefaultValue 对齐） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  parentId: '0',
  costCategory: 2,
  sortOrder: 0,
  deptStatus: 1,
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 deptId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.deptId) {
      const next = { ...val } as Record<string, unknown>
      if (next.parentId != null && next.parentId !== '') {
        next.parentId = String(next.parentId)
      }
      if (next.headUserId != null && next.headUserId !== '') {
        next.headUserId = String(next.headUserId)
      }
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        if (next.parentId != null && next.parentId !== '') {
          next.parentId = String(next.parentId)
        }
        Object.assign(formState, next)
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
    const isCreate = !props.formData?.deptId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  deptName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.name') }),
      trigger: 'blur',
    },
  ],
  deptCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.code') }),
      trigger: 'blur',
    },
  ],
  costCenterCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.dept.costcentercode') }),
      trigger: 'change',
    },
  ],
  headUserId: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.dept.headuserid') }),
      trigger: 'change',
    },
  ],
  costCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.dept.costcategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.dept.costcategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  phone: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.phone') }),
      trigger: 'blur',
    },
  ],
  email: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.email') }),
      trigger: 'blur',
    },
  ],
  location: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.location') }),
      trigger: 'blur',
    },
  ],
  description: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.dept.description') }),
      trigger: 'blur',
    },
  ],
  deptStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.dept.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.dept.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): DeptCreate {
  const payload = { ...formState }
  const parentRaw = payload.parentId
  const parentId = parentRaw === '' || parentRaw === undefined || parentRaw === null ? '0' : String(parentRaw)
  if ('costCategory' in payload) {
    const raw = payload.costCategory
    payload.costCategory = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('deptStatus' in payload) {
    const raw = payload.deptStatus
    payload.deptStatus = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('sortOrder' in payload) {
    const raw = payload.sortOrder
    payload.sortOrder = typeof raw === 'number' ? raw : Number(raw)
  }
  if (payload.headUserId != null && payload.headUserId !== '') {
    payload.headUserId = String(payload.headUserId)
  } else {
    payload.headUserId = ''
  }
  payload.parentId = parentId
  payload.costCenterCode = String(payload.costCenterCode ?? '').trim()
  payload.isBuiltIn = props.formData?.isBuiltIn ?? 0
  if (typeof payload.remark === 'string') {
    const trimmed = payload.remark.trim()
    payload.remark = trimmed.length > 0 ? trimmed : undefined
  }
  return payload as DeptCreate
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.deptId)
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
