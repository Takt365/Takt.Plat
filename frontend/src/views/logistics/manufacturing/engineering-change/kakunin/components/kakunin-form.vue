<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/kakunin/components -->
<!-- 文件名称：kakunin-form.vue -->
<!-- 功能描述：物料确认编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.ecno')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ischeck')"><TaktSelect v-model:value="formState.isCheck" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.isprocurement')"><TaktSelect v-model:value="formState.isProcurement" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ecdetail.ecnote')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.ecNote" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcKakunin, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/kakunin';

const props = defineProps<{ formData?: EcKakunin | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcKakuninUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  isCheck: 0,
  isProcurement: 0,
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    isCheck: val.isCheck ?? 0,
    isProcurement: val.isProcurement ?? 0,
    ecNote: val.ecNote ?? '',
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcKakuninUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isCheck: 0, isProcurement: 0, ecNote: '', ecNo: '', ecModel: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
