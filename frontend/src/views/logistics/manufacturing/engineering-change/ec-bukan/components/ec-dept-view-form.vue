<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-bukan/components -->
<!-- 文件名称：ec-dept-view-form.vue -->
<!-- 功能描述：设变部管部门表单；机种/完成品/新物料只读，执行内容可清空以消除 EOL -->
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
      <a-col :span="12"><a-form-item :label="pi.label('lineNumber')"><a-input-number v-model:value="formState.lineNumber" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('discontinuedStatus')">
          <TaktSelect v-model:value="formState.discontinuedStatus" dict-type="logistics_materials_material_discontinued_status" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecModelCode')"><a-input v-model:value="formState.ecModelCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecFinishedGoods')"><a-input v-model:value="formState.ecFinishedGoods" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialCode')"><a-input v-model:value="formState.ecNewMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialDescription')"><a-input v-model:value="formState.ecNewMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewPurchaseType')">
          <TaktSelect v-model:value="formState.ecNewPurchaseType" dict-type="logistics_procurement_type" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewWarehouse')">
          <TaktSelect v-model:value="formState.ecNewWarehouse" api-url="TaktWarehouses/options" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('outboundBatch')"><a-input v-model:value="formState.outboundBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('outboundDate')"><a-date-picker v-model:value="formState.outboundDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecFinishedGoodsDescription')"><a-input v-model:value="formState.ecFinishedGoodsDescription" disabled /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcBukan, EcBukanUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-bukan'
import { useEcDeptViewI18n } from '../../composables/use-ec-dept-view-i18n'

const props = defineProps<{ formData?: EcBukan | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecbukan')
const formRef = ref();
const formState = reactive<{
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCode?: string;
  ecFinishedGoods?: string;
  ecFinishedGoodsDescription?: string;
  discontinuedStatus?: string;
  ecNewMaterialCode?: string;
  ecNewMaterialDescription?: string;
  ecNewPurchaseType?: string;
  ecNewWarehouse?: string;
  isImplemented: number;
  execContent?: string;
  outboundDate?: string;
  outboundBatch?: string;
}>({ isImplemented: 0, execContent: '', discontinuedStatus: 'Z0' });

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCode: val.ecModelCode,
    ecFinishedGoods: val.ecFinishedGoods,
    ecFinishedGoodsDescription: val.ecFinishedGoodsDescription,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    ecNewMaterialCode: val.ecNewMaterialCode,
    ecNewMaterialDescription: val.ecNewMaterialDescription,
    ecNewPurchaseType: val.ecNewPurchaseType,
    ecNewWarehouse: val.ecNewWarehouse,
    isImplemented: val.isImplemented ?? 0,
    execContent: val.execContent ?? '',
    outboundDate: val.outboundDate,
    outboundBatch: val.outboundBatch,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcBukanUpdate {
  return {
    isImplemented: formState.isImplemented,
    execContent: formState.execContent ?? '',
    outboundDate: formState.outboundDate,
    outboundBatch: formState.outboundBatch,
  } as EcBukanUpdate;
}
function resetFields() {
  Object.assign(formState, {
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCode: '', ecFinishedGoods: '',
    ecFinishedGoodsDescription: '', discontinuedStatus: 'Z0',
    ecNewMaterialCode: '', ecNewMaterialDescription: '',
    ecNewPurchaseType: undefined, ecNewWarehouse: undefined,
    isImplemented: 0, execContent: '',
    outboundDate: undefined, outboundBatch: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
