<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/legacy-product/components -->
<!-- 文件名称：legacy-product-form.vue -->
<!-- 功能描述：旧品管制编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.no')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecolditem')"><a-input v-model:value="formState.ecOldItem" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.oldproducthandling')"><a-input v-model:value="formState.oldProductHandling" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isendofline')"><TaktSelect v-model:value="formState.isEndOfLine" dict-type="logistics_material_eol_status" allow-clear /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ec.remark')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.remark" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcLegacyProduct, EcLegacyProductUpdate } from '@/types/logistics/manufacturing/engineering-change/legacy-product';

const props = defineProps<{ formData?: EcLegacyProduct | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcLegacyProductUpdate & { ecCode?: string; ecOldItem?: string }>({
  ecDetailId: '',
  isEndOfLine: '',
  oldProductHandling: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecCode: val.ecCode,
    ecOldItem: val.ecOldItem ?? '',
    oldProductHandling: val.oldProductHandling ?? '',
    isEndOfLine: val.isEndOfLine ?? '',
    remark: val.remark ?? '',
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcLegacyProductUpdate {
  const { ecCode, ecOldItem, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isEndOfLine: 0, oldProductHandling: '', remark: '', ecCode: '', ecOldItem: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
