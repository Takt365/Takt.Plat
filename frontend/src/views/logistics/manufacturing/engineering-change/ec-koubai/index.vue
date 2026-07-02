<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-koubai -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变采购部门页面 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar v-model="queryKeyword" :placeholder="t('common.page.form.placeholder.search')" :loading="loading" @search="handleSearch" @reset="handleReset" />
    <TaktToolsBar
      :show-create="false"
      :show-update="true"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      update-permission="logistics:manufacturing:engineering:change:koubai:update"
      export-permission="logistics:manufacturing:engineering:change:koubai:export"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @refresh="loadData"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecDetailId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcDetailId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    <TaktModal v-model:open="formVisible" :title="t('common.dialog.title.edit', { entity: t('menu.logistics.manufacturing.engineering.change.koubai') })" width="900px" :confirm-loading="formLoading" @ok="handleFormSubmit">
      <EcDeptViewForm ref="formRef" :form-data="formData" :loading="formLoading" />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变采购部门列表页
 */
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { getEcKoubaiList, updateEcKoubai, exportEcKoubaiData } from '@/api/logistics/manufacturing/engineering-change/ec-koubai';
import type { EcKoubai, EcKoubaiUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-koubai';
import EcDeptViewForm from './components/ec-dept-view-form.vue';
import { TaktEcExecCodes } from '@/constants/logistics/ec-exec-codes';
import { useEcExecSignalRGroup } from '@/composables/use-ec-dept-signalr-group';

const { t } = useI18n();
/** 列表 loading */
const loading = ref(false);
/** 数据源 */
const dataSource = ref<EcKoubai[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词 */
const queryKeyword = ref('');
/** 选中行 keys */
const selectedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selectedRows = ref<EcKoubai[]>([]);
/** 表单可见 */
const formVisible = ref(false);
/** 表单 loading */
const formLoading = ref(false);
/** 编辑数据 */
const formData = ref<EcKoubai | null>(null);
/** 表单 ref */
const formRef = ref<InstanceType<typeof EcDeptViewForm> | null>(null);
/** 列定义 */
const columns = ref([
  { title: t('entity.ec.no'), dataIndex: 'ecNo', key: 'ecNo', width: 120 }, { title: t('entity.ecdetail.ecmodel'), dataIndex: 'ecModel', key: 'ecModel', width: 140 }, { title: t('entity.ecdetail.ecolditem'), dataIndex: 'ecOldItem', key: 'ecOldItem', width: 140 }, { title: t('entity.ecdetail.ecnewitem'), dataIndex: 'ecNewItem', key: 'ecNewItem', width: 140 }
]);
/** 可见列 keys */
const visibleColumnKeys = ref(columns.value.map(c => String(c.key)));
/** 行选择 */
const rowSelection = computed(() => ({ selectedRowKeys: selectedRowKeys.value, onChange: (keys: (string | number)[], rows: EcKoubai[]) => { selectedRowKeys.value = keys; selectedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}
/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKoubaiList({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    dataSource.value = res.data ?? [];
    total.value = res.total ?? 0;
  } finally {
    loading.value = false;
  }
}
/** 搜索 */
function handleSearch() { currentPage.value = 1; loadData(); }
/** 重置 */
function handleReset() { queryKeyword.value = ''; currentPage.value = 1; loadData(); }
/** 分页变化 */
function handlePaginationChange() { loadData(); }
/** 表格变化 */
function handleTableChange() {}
/** 列宽变化 */
function handleResizeColumn() {}
/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return { onClick: () => { const id = getEcDetailId(record); selectedRowKeys.value = [id]; selectedRows.value = [record as unknown as EcKoubai]; } };
}
/** 编辑 */
async function handleUpdate() {
  const row = selectedRows.value[0];
  if (!row) return;
  formData.value = { ...row };
  formVisible.value = true;
}
/** 提交表单 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) return;
  await formRef.value.validate();
  const dto: EcKoubaiUpdate = formRef.value.getValues();
  formLoading.value = true;
  try {
    await updateEcKoubai(String(formData.value.ecDetailId), dto);
    message.success(t('common.feedback.updated', { target: t('menu.logistics.manufacturing.engineering.change.koubai') }));
    formVisible.value = false;
    await loadData();
  } finally {
    formLoading.value = false;
  }
}
/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcKoubaiData({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.koubai')}.xlsx`;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  } finally {
    loading.value = false;
  }
}
useTableRefresh(loadData);
useEcExecSignalRGroup(TaktEcExecCodes.Mp);
onMounted(loadData);
</script>
