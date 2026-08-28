<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizounika/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变制造二课表单；停产状态只读，执行内容可清空以消除 EOL -->
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
      <a-col :span="12"><a-form-item :label="pi.label('ecParentMaterialCode')"><a-input v-model:value="formState.ecParentMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('discontinuedStatus')">
          <TaktSelect v-model:value="formState.discontinuedStatus" dict-type="logistics_materials_material_discontinued_status" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('productionDate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('productionBatch')"><a-input v-model:value="formState.productionBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('productionTeam')"><a-input v-model:value="formState.productionTeam" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('outboundOrderCode')"><a-input v-model:value="formState.outboundOrderCode" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcSeizounika, EcSeizounikaUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-seizounika'
import { useEcDeptViewI18n } from '../../composables/use-ec-dept-view-i18n'

const props = defineProps<{ formData?: EcSeizounika | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecseizounika')
const formRef = ref();
const formState = reactive<{
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  ecModelCode?: string;
  ecParentMaterialCode?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  execContent?: string;
  productionDate?: string;
  productionBatch?: string;
  productionTeam?: string;
  outboundOrderCode?: string;
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
    ecParentMaterialCode: val.ecParentMaterialCode,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: val.isImplemented ?? 0,
    execContent: val.execContent ?? '',
    productionDate: val.productionDate,
    productionBatch: val.productionBatch,
    productionTeam: val.productionTeam,
    outboundOrderCode: val.outboundOrderCode,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcSeizounikaUpdate {
  return {
    isImplemented: formState.isImplemented,
    execContent: formState.execContent ?? '',
    productionDate: formState.productionDate,
    productionBatch: formState.productionBatch,
    productionTeam: formState.productionTeam,
    outboundOrderCode: formState.outboundOrderCode,
  } as EcSeizounikaUpdate;
}
function resetFields() {
  Object.assign(formState, {
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', ecModelCode: '', ecParentMaterialCode: '', discontinuedStatus: 'Z0', isImplemented: 0, execContent: '',
    productionDate: undefined, productionBatch: undefined, productionTeam: undefined, outboundOrderCode: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
