<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kakunin/components -->
<!-- 文件名称：kakunin-form.vue -->
<!-- 功能描述：物料确认编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.no')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isoldcheck')"><TaktSelect v-model:value="formState.isOldCheck" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isoldprocurement')"><TaktSelect v-model:value="formState.isOldProcurement" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isnewcheck')"><TaktSelect v-model:value="formState.isNewCheck" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isnewprocurement')"><TaktSelect v-model:value="formState.isNewProcurement" dict-type="sys_yes_no" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcKakunin, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-kakunin';

const props = defineProps<{ formData?: EcKakunin | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcKakuninUpdate & { ecCode?: string; ecModel?: string }>({
  ecDetailId: '',
  isOldProcurement: 0,
  isOldCheck: 0,
  isNewProcurement: 0,
  isNewCheck: 0,
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecCode: val.ecCode,
    ecModel: val.ecModel,
    isOldProcurement: val.isOldProcurement ?? 0,
    isOldCheck: val.isOldCheck ?? 0,
    isNewProcurement: val.isNewProcurement ?? 0,
    isNewCheck: val.isNewCheck ?? 0,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcKakuninUpdate {
  const { ecCode, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, {
    ecDetailId: '',
    isOldProcurement: 0,
    isOldCheck: 0,
    isNewProcurement: 0,
    isNewCheck: 0,
    ecCode: '',
    ecModel: '',
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
