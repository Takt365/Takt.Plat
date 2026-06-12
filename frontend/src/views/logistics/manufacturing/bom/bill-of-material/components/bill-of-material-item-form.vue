<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：bill-of-material-item-form.vue -->
<!-- 功能描述：BOM 明细独立 CRUD 弹窗表单；defineExpose 提供 validate、getValues、resetFields -->
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
        <a-form-item :label="t('entity.billOfMaterialItem.linenumber')" name="lineNumber">
          <a-input-number
            v-model:value="formState.lineNumber"
            :min="1"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.linenumber') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.materialcode')" name="materialCode">
          <a-input
            v-model:value="formState.materialCode"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialcode') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.materialid')" name="materialId">
          <a-input
            v-model:value="formState.materialId"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialid') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.usagequantity')" name="usageQuantity">
          <a-input-number
            v-model:value="formState.usageQuantity"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.usagequantity') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.materialunit')" name="materialUnit">
          <a-input
            v-model:value="formState.materialUnit"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialunit') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.scraprate')" name="scrapRate">
          <a-input-number
            v-model:value="formState.scrapRate"
            :min="0"
            :max="100"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.scraprate') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.actualusagequantity')" name="actualUsageQuantity">
          <a-input-number
            v-model:value="formState.actualUsageQuantity"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.actualusagequantity') })"
            size="small"
            style="width: 100%"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.billOfMaterialItem.operationseq')" name="operationSeq">
          <a-input-number
            v-model:value="formState.operationSeq"
            :min="0"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.operationseq') })"
            size="small"
            style="width: 100%"
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
 * BOM 明细独立维护表单（底部面板 / 可选复用）
 * @module views/logistics/manufacturing/bom/bill-of-material/components
 */
import { reactive, ref, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type {
  BillOfMaterialItem,
  BillOfMaterialItemCreate,
  BillOfMaterialItemUpdate,
} from '@/types/logistics/manufacturing/bom/bill-of-material-item'
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
  formData?: Partial<BillOfMaterialItem> | null
  /** 主表 BOM 头 ID（新建明细必填） */
  masterBillOfMaterialId?: string
  /** 主表 BOM 编码（新建明细冗余字段） */
  masterBomCode?: string
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  masterBillOfMaterialId: '',
  masterBomCode: '',
  loading: false,
})

/** 明细表单模型（独立 CRUD 常用字段） */
interface BillOfMaterialItemFormState {
  tenantCode?: string
  companyCode?: string
  companyDefaultCulture?: string
  billOfMaterialId?: string
  bomCode?: string
  lineNumber?: number
  materialId?: string
  materialCode?: string
  usageQuantity?: number
  materialUnit?: string
  scrapRate?: number
  actualUsageQuantity?: number
  operationSeq?: number
  substitutePriority?: number
  isOptional?: number
  isPhantom?: number
  remark?: string
  billOfMaterialItemId?: string
}

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<BillOfMaterialItemFormState>({})

/**
 * 写入租户/公司隔离字段
 * @param target 表单对象
 * @param force 新增态强制覆盖
 */
function applyScopeDefaults(target: BillOfMaterialItemFormState, force = false) {
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
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.linenumber') }),
      trigger: 'change',
    },
  ],
  materialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialid') }),
      trigger: 'blur',
    },
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialcode') }),
      trigger: 'blur',
    },
  ],
  usageQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.usagequantity') }),
      trigger: 'change',
    },
  ],
  materialUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialunit') }),
      trigger: 'blur',
    },
  ],
  scrapRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.scraprate') }),
      trigger: 'change',
    },
  ],
  actualUsageQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.actualusagequantity') }),
      trigger: 'change',
    },
  ],
  operationSeq: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.operationseq') }),
      trigger: 'change',
    },
  ],
}))

watch(
  () => [props.formData, props.masterBillOfMaterialId, props.masterBomCode] as const,
  ([val]) => {
    Object.keys(formState).forEach((k) => delete (formState as Record<string, unknown>)[k])
    const next: BillOfMaterialItemFormState = val ? { ...val } : {
      lineNumber: 10,
      materialId: '',
      materialCode: '',
      usageQuantity: 0,
      materialUnit: '',
      scrapRate: 0,
      actualUsageQuantity: 0,
      operationSeq: 0,
      substitutePriority: 0,
      isOptional: 0,
      isPhantom: 0,
      remark: '',
    }
    if (!val?.billOfMaterialItemId) {
      next.billOfMaterialId = props.masterBillOfMaterialId
      next.bomCode = props.masterBomCode
    }
    applyScopeDefaults(next, !val?.billOfMaterialItemId)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

/** 校验表单（失败 throw） */
async function validate() {
  await formRef.value?.validate()
}

/** 映射为 Create/Update DTO */
function getValues(): BillOfMaterialItemCreate | BillOfMaterialItemUpdate {
  const payload: BillOfMaterialItemCreate = {
    tenantCode: formState.tenantCode ?? tenantStore.tenantCode,
    companyCode: formState.companyCode ?? tenantStore.companyCode,
    companyDefaultCulture: formState.companyDefaultCulture ?? userStore.userInfo?.companyDefaultCulture ?? '',
    billOfMaterialId: String(formState.billOfMaterialId || props.masterBillOfMaterialId || ''),
    bomCode: String(formState.bomCode || props.masterBomCode || ''),
    lineNumber: formState.lineNumber ?? 0,
    materialId: formState.materialId ?? '',
    materialCode: formState.materialCode ?? '',
    usageQuantity: formState.usageQuantity ?? 0,
    materialUnit: formState.materialUnit ?? '',
    scrapRate: formState.scrapRate ?? 0,
    actualUsageQuantity: formState.actualUsageQuantity ?? 0,
    operationSeq: formState.operationSeq ?? 0,
    substitutePriority: formState.substitutePriority ?? 0,
    isOptional: formState.isOptional ?? 0,
    isPhantom: formState.isPhantom ?? 0,
    remark: formState.remark,
  }
  if (props.formData?.billOfMaterialItemId) {
    return {
      ...payload,
      billOfMaterialItemId: props.formData.billOfMaterialItemId,
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
