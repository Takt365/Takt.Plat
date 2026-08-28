<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seikan/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变生管部门表单；defineExpose validate/getValues/resetFields；停产状态只读，执行内容可清空以消除 EOL 自动填充 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('tenantCode')"><a-input v-model:value="formState.tenantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('companyCode')"><a-input v-model:value="formState.companyCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('cultureCode')"><a-input v-model:value="formState.cultureCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('plantCode')"><a-input v-model:value="formState.plantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecModelCode')"><a-input v-model:value="formState.ecModelCode" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('discontinuedStatus')">
          <TaktSelect v-model:value="formState.discontinuedStatus" dict-type="logistics_materials_material_discontinued_status" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledProductionDate')"><a-date-picker v-model:value="formState.scheduledProductionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledBatch')"><a-input v-model:value="formState.scheduledBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('poRemainder')"><a-input v-model:value="formState.poRemainder" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('balance')"><a-input v-model:value="formState.balance" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('oldProductHandling')"><a-input v-model:value="formState.oldProductHandling" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" :placeholder="pi.t('common.page.form.placeholder.input')" />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcSeikan, EcSeikanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seikan'
import { useEcDeptViewI18n } from '../../composables/use-ec-dept-view-i18n'

const props = defineProps<{ formData?: EcSeikan | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecseikan')
/** 表单 ref */
const formRef = ref();
/** 表单状态 */
const formState = reactive<{
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  ecModelCode?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  execContent?: string;
  scheduledProductionDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
}>({
  isImplemented: 0,
  execContent: '',
  discontinuedStatus: 'Z0',
});

watch(() => props.formData, (val) => {
  if (!val) {
    resetFields();
    return;
  }
  Object.assign(formState, {
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    ecModelCode: val.ecModelCode,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: val.isImplemented ?? 0,
    execContent: val.execContent ?? '',
    scheduledProductionDate: val.scheduledProductionDate,
    scheduledBatch: val.scheduledBatch,
    poRemainder: val.poRemainder,
    balance: val.balance,
    oldProductHandling: val.oldProductHandling,
  });
}, { immediate: true });

/** 校验表单 */
async function validate() {
  await formRef.value?.validate();
}

/**
 * 获取提交 DTO（执行内容可清空以消除 EOL 自动填充）
 * @returns {EcSeikanUpdate} 更新 DTO
 */
function getValues(): EcSeikanUpdate {
  return {
    isImplemented: formState.isImplemented,
    execContent: formState.execContent ?? '',
    scheduledProductionDate: formState.scheduledProductionDate,
    scheduledBatch: formState.scheduledBatch,
    poRemainder: formState.poRemainder,
    balance: formState.balance,
    oldProductHandling: formState.oldProductHandling,
  } as EcSeikanUpdate;
}

/** 重置表单 */
function resetFields() {
  Object.assign(formState, {
    tenantCode: '',
    companyCode: '',
    cultureCode: '',
    plantCode: '',
    ecCode: '',
    ecModelCode: '',
    discontinuedStatus: 'Z0',
    isImplemented: 0,
    execContent: '',
    scheduledProductionDate: undefined,
    scheduledBatch: undefined,
    poRemainder: undefined,
    balance: undefined,
    oldProductHandling: undefined,
  });
}

defineExpose({ validate, getValues, resetFields });
</script>
