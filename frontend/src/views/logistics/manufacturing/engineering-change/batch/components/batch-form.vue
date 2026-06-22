<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/batch/components -->
<!-- 文件名称：batch-form.vue -->
<!-- 功能描述：投入批次编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.scheduledbatch')"><a-input v-model:value="formState.scheduledBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.productionbatch')"><a-input v-model:value="formState.productionBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.scheduledproductiondate')"><a-date-picker v-model:value="formState.scheduledProductionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.productiondate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcBatch, EcBatchUpdate } from '@/types/logistics/manufacturing/engineering-change/batch';

const props = defineProps<{ formData?: EcBatch | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcBatchUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  scheduledBatch: '',
  productionBatch: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    scheduledBatch: val.scheduledBatch ?? '',
    productionBatch: val.productionBatch ?? '',
    scheduledProductionDate: val.scheduledProductionDate,
    productionDate: val.productionDate,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcBatchUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', scheduledBatch: '', productionBatch: '', ecNo: '', ecModel: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
