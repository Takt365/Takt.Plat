<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：assy-defect-detail-form.vue -->
<!-- 功能描述：组立不良明细独立 CRUD 弹窗表单；defineExpose 提供 validate、getValues、resetFields -->
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
    <a-row :gutter="24">
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.linenumber')" name="lineNumber">
          <a-input-number
            v-model:value="formState.lineNumber"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.linenumber') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.prodordercode')" name="prodOrderCode">
          <a-input
            v-model:value="formState.prodOrderCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.prodordercode') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.defectcategory')" name="defectCategory">
          <a-input
            v-model:value="formState.defectCategory"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectcategory') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.defectqty')" name="defectQty">
          <a-input-number
            v-model:value="formState.defectQty"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectqty') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.cumulativedefectqty')" name="cumulativeDefectQty">
          <a-input-number
            v-model:value="formState.cumulativeDefectQty"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.cumulativedefectqty') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.randomcardno')" name="randomCardNo">
          <a-input
            v-model:value="formState.randomCardNo"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.randomcardno') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.occurrenceengineering')" name="occurrenceEngineering">
          <a-input
            v-model:value="formState.occurrenceEngineering"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.occurrenceengineering') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.teststep')" name="testStep">
          <a-input
            v-model:value="formState.testStep"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.teststep') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.defectsymptom')" name="defectSymptom">
          <a-input
            v-model:value="formState.defectSymptom"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assydefectdetail.defectsymptom') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.defectlocation')" name="defectLocation">
          <a-input
            v-model:value="formState.defectLocation"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assydefectdetail.defectlocation') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.defectreason')" name="defectReason">
          <a-input
            v-model:value="formState.defectReason"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assydefectdetail.defectreason') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.assydefectdetail.repairoperator')" name="repairOperator">
          <a-input
            v-model:value="formState.repairOperator"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assydefectdetail.repairoperator') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="t('common.page.entity.remark')" name="remark">
          <a-textarea
            v-model:value="formState.remark"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="2"
            size="small"
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立不良明细独立维护表单（右侧分栏 / 弹窗）
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { reactive, ref, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type {
  AssyDefectDetail,
  AssyDefectDetailCreate,
  AssyDefectDetailUpdate,
} from '@/types/logistics/manufacturing/defect/assy-defect-detail'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 父级传入的编辑 DTO；新增时为 undefined */
interface Props {
  formData?: Partial<AssyDefectDetail> | null
  /** 主表组立不良 ID（新建明细必填） */
  masterAssyDefectId?: string
  /** 主表生产订单号（冗余字段） */
  masterProdOrderCode?: string
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  masterAssyDefectId: '',
  masterProdOrderCode: '',
  loading: false,
})

/** 明细表单模型 */
interface AssyDefectDetailFormState {
  tenantCode?: string
  companyCode?: string
  companyDefaultCulture?: string
  assyDefectId?: string
  prodOrderCode?: string
  lineNumber?: number
  defectCategory?: string
  defectQty?: number
  cumulativeDefectQty?: number
  randomCardNo?: string
  occurrenceEngineering?: string
  testStep?: string
  defectSymptom?: string
  defectLocation?: string
  defectReason?: string
  repairOperator?: string
  remark?: string
  assyDefectDetailId?: string
}

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<AssyDefectDetailFormState>({})

/**
 * 写入租户/公司隔离字段
 * @param target 表单对象
 * @param force 新增态强制覆盖
 */
function applyScopeDefaults(target: AssyDefectDetailFormState, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.companyDefaultCulture) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.linenumber') }),
      trigger: 'change',
    },
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.prodordercode') }),
      trigger: 'blur',
    },
  ],
  defectQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.defectqty') }),
      trigger: 'change',
    },
  ],
  cumulativeDefectQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assydefectdetail.cumulativedefectqty') }),
      trigger: 'change',
    },
  ],
}))

watch(
  () => [props.formData, props.masterAssyDefectId, props.masterProdOrderCode] as const,
  ([val]) => {
    Object.keys(formState).forEach((k) => delete (formState as Record<string, unknown>)[k])
    const next: AssyDefectDetailFormState = val ? { ...val } : {
      lineNumber: 10,
      prodOrderCode: props.masterProdOrderCode || '',
      defectCategory: '',
      defectQty: 0,
      cumulativeDefectQty: 0,
      randomCardNo: '',
      occurrenceEngineering: '',
      testStep: '',
      defectSymptom: '',
      defectLocation: '',
      defectReason: '',
      repairOperator: '',
      remark: '',
    }
    if (!val?.assyDefectDetailId) {
      next.assyDefectId = props.masterAssyDefectId
      if (!next.prodOrderCode) {
        next.prodOrderCode = props.masterProdOrderCode || ''
      }
    }
    applyScopeDefaults(next, !val?.assyDefectDetailId)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true },
)

/** 校验表单（失败 throw） */
async function validate() {
  await formRef.value?.validate()
}

/** 映射为 Create/Update DTO */
function getValues(): AssyDefectDetailCreate | AssyDefectDetailUpdate {
  const payload: AssyDefectDetailCreate = {
    tenantCode: formState.tenantCode ?? tenantStore.tenantCode,
    companyCode: formState.companyCode ?? tenantStore.companyCode,
    companyDefaultCulture: formState.companyDefaultCulture ?? userStore.userInfo?.companyDefaultCulture ?? '',
    assyDefectId: String(formState.assyDefectId || props.masterAssyDefectId || ''),
    prodOrderCode: String(formState.prodOrderCode || props.masterProdOrderCode || ''),
    lineNumber: formState.lineNumber ?? 0,
    defectCategory: formState.defectCategory,
    defectQty: formState.defectQty ?? 0,
    cumulativeDefectQty: formState.cumulativeDefectQty ?? 0,
    randomCardNo: formState.randomCardNo,
    occurrenceEngineering: formState.occurrenceEngineering,
    testStep: formState.testStep,
    defectSymptom: formState.defectSymptom,
    defectLocation: formState.defectLocation,
    defectReason: formState.defectReason,
    repairOperator: formState.repairOperator,
    remark: formState.remark,
  }
  if (props.formData?.assyDefectDetailId) {
    return {
      ...payload,
      assyDefectDetailId: props.formData.assyDefectDetailId,
    }
  }
  return payload
}

/** 重置表单 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete (formState as Record<string, unknown>)[k])
}

defineExpose({ validate, getValues, resetFields })
</script>
