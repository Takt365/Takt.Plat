<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizougijutsu/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变制造技术课部门表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form ref="formRef" :model="formState" layout="horizontal" label-align="right" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
    <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="t('entity.ec.no')"><a-input v-model:value="formState.ecNo" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdetail.ecmodel')"><a-input v-model:value="formState.ecModel" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.isimplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.confirmationdate')"><a-date-picker v-model:value="formState.confirmationDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="t('entity.ecdept.issopupdated')"><TaktSelect v-model:value="formState.isSopUpdated" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="t('entity.ecdept.content')" :label-col="{ span: 3 }" :wrapper-col="{ span: 20 }"><a-textarea v-model:value="formState.content" :rows="3" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { EcDeptView, EcDeptViewUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-dept-view';

const props = defineProps<{ formData?: EcDeptView | null; loading?: boolean }>();
const { t } = useI18n();
const formRef = ref();
const formState = reactive<EcDeptViewUpdate & { ecNo?: string; ecModel?: string }>({
  ecDetailId: '',
  isImplemented: 0,
  isSopUpdated: 0,
  content: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecNo: val.ecNo,
    ecModel: val.ecModel,
    isImplemented: val.isImplemented ?? 0,
    content: val.content ?? '',
    isSopUpdated: val.isSopUpdated ?? 0,
    confirmationDate: val.confirmationDate,
  });
}, { immediate: true });

/** 校验表单 */
async function validate() { await formRef.value?.validate(); }
/** 获取提交 DTO */
function getValues(): EcDeptViewUpdate {
  const { ecNo, ecModel, ...rest } = formState;
  return rest;
}
/** 重置表单 */
function resetFields() {
  Object.assign(formState, { ecDetailId: '', isImplemented: 0, isSopUpdated: 0, content: '', ecNo: '', ecModel: '', confirmationDate: undefined });
}
defineExpose({ validate, getValues, resetFields });
</script>
