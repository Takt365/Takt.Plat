<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/kanban -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变设变看板页面 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar v-model="queryKeyword" :placeholder="t('common.page.form.placeholder.search')" :loading="loading" @search="handleSearch" @reset="handleReset" />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      update-permission="logistics:manufacturing:engineeringchange:kanban:update"
      export-permission="logistics:manufacturing:engineeringchange:kanban:export"
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
      :id-column-key="'ecId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    
  </div>
</template>

<script setup lang="ts">
/**
 * 设变设变看板列表页
 */
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { getEcKanbanList, exportEcKanbanData } from '@/api/logistics/manufacturing/engineering-change/kanban';
import type { EcKanban } from '@/types/logistics/manufacturing/engineering-change/kanban';

const { t } = useI18n();
/** 列表 loading */
const loading = ref(false);
/** 数据源 */
const dataSource = ref<EcKanban[]>([]);
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
const selectedRows = ref<EcKanban[]>([]);
/** 表单可见 */
const formVisible = ref(false);
/** 表单 loading */
const formLoading = ref(false);
/** 编辑数据 */
const formData = ref<Record<string, unknown> | null>(null);
/** 表单 ref */
const formRef = ref<{ validate: () => Promise<void>; getValues: () => Record<string, unknown> } | null>(null);
/** 列定义 */
const columns = ref([
  { title: t('entity.ec.ecno'), dataIndex: 'ecNo', key: 'ecNo', width: 120 }, { title: t('entity.ec.ectitle'), dataIndex: 'ecTitle', key: 'ecTitle', width: 200 }, { title: t('entity.ec.changestatus'), dataIndex: 'changeStatus', key: 'changeStatus', width: 100 }, { title: t('entity.ec.ecstatus'), dataIndex: 'ecStatus', key: 'ecStatus', width: 100 }, { title: t('entity.ec.ecleader'), dataIndex: 'ecLeader', key: 'ecLeader', width: 120 }
]);
/** 可见列 keys */
const visibleColumnKeys = ref(columns.value.map(c => String(c.key)));
/** 行选择 */
const rowSelection = computed(() => ({ selectedRowKeys: selectedRowKeys.value, onChange: (keys: (string | number)[], rows: EcKanban[]) => { selectedRowKeys.value = keys; selectedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function getEcId(record: Record<string, unknown>) {
  return String(record.ecId ?? '');
}
/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKanbanList({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
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
  return { onClick: () => { const id = getEcId(record); selectedRowKeys.value = [id]; selectedRows.value = [record as unknown as EcKanban]; } };
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcKanbanData({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = '设变看板.xlsx';
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
onMounted(loadData);
</script>
