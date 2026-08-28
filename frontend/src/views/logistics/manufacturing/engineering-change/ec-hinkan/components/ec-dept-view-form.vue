<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-hinkan/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变品管部门表单；停产状态只读，执行内容可清空以消除 EOL -->
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
      <a-col :span="12"><a-form-item :label="pi.label('productionTeam')"><a-input v-model:value="formState.productionTeam" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('inspectionDate')"><a-date-picker v-model:value="formState.inspectionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('inspectionBatch')"><a-input v-model:value="formState.inspectionBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('samplingCode')"><a-input v-model:value="formState.samplingCode" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcHinkan, EcHinkanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-hinkan'
import { useEcDeptViewI18n } from '../../composables/use-ec-dept-view-i18n'

const props = defineProps<{ formData?: EcHinkan | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('echinkan')
const formRef = ref();
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
  productionTeam?: string;
  inspectionDate?: string;
  inspectionBatch?: string;
  samplingCode?: string;
}>({ isImplemented: 0, execContent: '', discontinuedStatus: 'Z0' });

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
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
    productionTeam: val.productionTeam,
    inspectionDate: val.inspectionDate,
    inspectionBatch: val.inspectionBatch,
    samplingCode: val.samplingCode,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcHinkanUpdate {
  return {
    isImplemented: formState.isImplemented,
    execContent: formState.execContent ?? '',
    productionTeam: formState.productionTeam,
    inspectionDate: formState.inspectionDate,
    inspectionBatch: formState.inspectionBatch,
    samplingCode: formState.samplingCode,
  } as EcHinkanUpdate;
}
function resetFields() {
  Object.assign(formState, {
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', ecModelCode: '', discontinuedStatus: 'Z0', isImplemented: 0, execContent: '',
    productionTeam: undefined, inspectionDate: undefined, inspectionBatch: undefined, samplingCode: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
