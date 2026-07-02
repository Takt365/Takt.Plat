<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-bukan -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变部门转置列表（行=设变明细，列=各部门实施状态） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-refresh="false"
      export-permission="logistics:manufacturing:engineering:change:bukan:export"
      :export-loading="loading"
      @export="handleExport"
    />
    <div class="overflow-x-auto">
      <TaktSingleTable
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'ecDetailId'"
        table-mode="single"
        :data-source="tableRows"
        :loading="loading"
        :stripe="true"
        :row-key="getEcDetailId"
        :scroll="tableScroll"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'ecNo'">
            <a-typography-link @click.stop="handleDetail(record as EcExecTransposed)">
              {{ record.ecNo }}
            </a-typography-link>
          </template>
          <template v-else-if="String(column.key ?? '').startsWith('dept_')">
            <span :class="deptCellClass(record, String(column.key))">
              {{ formatDeptCell(record, String(column.key)) }}
            </span>
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
    />
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('menu.logistics.manufacturing.engineering.change.bukan') })"
      width="960px"
      :hide-footer="true"
      @cancel="detailVisible = false"
    >
      <a-spin :spinning="detailLoading">
        <a-descriptions v-if="detailData" bordered :column="2" size="small">
          <a-descriptions-item :label="t('entity.ec.no')">{{ detailData.ecNo }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ecdetail.linenumber')">{{ detailData.lineNumber }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ecdetail.ecmodel')">{{ detailData.ecModel }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ecdetail.ecnewitem')">{{ detailData.ecNewItem ?? '—' }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ecdetail.ecolditem')">{{ detailData.ecOldItem ?? '—' }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ecdetail.ecentrydate')">{{ detailData.ecEntryDate ?? '—' }}</a-descriptions-item>
        </a-descriptions>
      </a-spin>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变部门转置列表页
 */
import { RiEyeLine } from '@remixicon/vue';
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { getEcBukanTransposedList } from '@/api/logistics/manufacturing/engineering-change/ec-bukan';
import { getEcDetailById } from '@/api/logistics/manufacturing/engineering-change/ec-detail';
import { exportEcBukanData } from '@/api/logistics/manufacturing/engineering-change/ec-bukan';
import { CreateActionColumn } from '@/components/business/takt-action-column/index';
import { TaktEcExecTransposedOrder } from '@/constants/logistics/ec-exec-codes';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ec-detail';
import type {
  EcExecTransposed,
  EcExecTransposedResult,
} from '@/types/logistics/manufacturing/engineering-change/ec-exec-transposed';

const { t } = useI18n();
const localePrefix = 'logistics.manufacturing.engineering-change.ec-bukan.page';
/** 列表 loading */
const loading = ref(false);
/** 转置结果 */
const transposedResult = ref<EcExecTransposedResult | null>(null);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词 */
const queryKeyword = ref('');
/** 详情弹窗 */
const detailVisible = ref(false);
/** 详情 loading */
const detailLoading = ref(false);
/** 详情数据 */
const detailData = ref<EcDetail | null>(null);
/** 表格横向滚动 */
const tableScroll = { x: 'max-content' } as const;

/**
 * 部门列标题
 * @param deptCode 部门编码
 * @returns 列标题
 */
function deptColumnTitle(deptCode: string): string {
  return t(`${localePrefix}.dept.${deptCode.toLowerCase()}`);
}

/** 动态列 */
const columns = computed(() => {
  const order = transposedResult.value?.deptCodeOrder?.length
    ? transposedResult.value.deptCodeOrder
    : [...TaktEcExecTransposedOrder];
  const base: TableColumnsType = [
    { title: t('entity.ec.issuedate'), dataIndex: 'ecIssueDate', key: 'ecIssueDate', width: 100, fixed: 'left' as const },
    { title: t('entity.ec.leader'), dataIndex: 'ecLeader', key: 'ecLeader', width: 90, fixed: 'left' as const },
    { title: t('entity.ec.no'), dataIndex: 'ecNo', key: 'ecNo', width: 110, fixed: 'left' as const },
    { title: t('entity.ecdetail.ecmodel'), dataIndex: 'ecModel', key: 'ecModel', width: 100 },
    { title: t('entity.ecdetail.ecnewitem'), dataIndex: 'ecNewItem', key: 'ecNewItem', width: 120 },
  ];
  order.forEach((deptCode) => {
    base.push({
      title: deptColumnTitle(deptCode),
      dataIndex: `dept_${deptCode}`,
      key: `dept_${deptCode}`,
      width: 100,
    });
  });
  base.push(
    CreateActionColumn<EcExecTransposed>({
      actions: [
        {
          key: 'detail',
          label: t('common.page.button.detail'),
          shape: 'plain',
          icon: RiEyeLine,
          permission: 'logistics:manufacturing:engineering:change:bukan:detail',
          buttonClass: 'takt-button-detail',
          onClick: (record) => handleDetail(record),
        },
      ],
    }),
  );
  return base;
});

/** 可见列 keys */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)));

/** 表格行 */
const tableRows = computed(() => transposedResult.value?.paged?.data ?? []);

/**
 * 行主键
 * @param record 行数据
 * @returns ecDetailId
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}

/**
 * 部门单元格展示
 * @param record 行数据
 * @param columnKey 列 key
 * @returns 展示文本
 */
function formatDeptCell(record: Record<string, unknown>, columnKey: string): string {
  const deptCode = columnKey.replace('dept_', '');
  const row = record as unknown as EcExecTransposed;
  const cell = row.deptCells?.[deptCode];
  if (cell?.displayText) return cell.displayText;
  if (cell?.isImplemented === 1 && cell.completedDate) {
    return String(cell.completedDate).slice(0, 10).replace(/-/g, '');
  }
  return t(`${localePrefix}.notProcessed`);
}

/**
 * 部门单元格样式
 * @param record 行数据
 * @param columnKey 列 key
 * @returns CSS 类名
 */
function deptCellClass(record: Record<string, unknown>, columnKey: string): string {
  const deptCode = columnKey.replace('dept_', '');
  const row = record as unknown as EcExecTransposed;
  const cell = row.deptCells?.[deptCode];
  if (cell?.isImplemented === 1 && (cell.displayText || cell.completedDate)) {
    return '';
  }
  return 'text-text-secondary';
}

/**
 * 构建查询参数
 * @returns 查询 DTO
 */
function buildQueryParams() {
  return {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    keyWords: queryKeyword.value || undefined,
  };
}

/** 加载转置列表 */
async function loadData() {
  loading.value = true;
  try {
    const result = await getEcBukanTransposedList(buildQueryParams());
    transposedResult.value = result;
    total.value = result?.paged?.total ?? 0;
  } finally {
    loading.value = false;
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = 1;
  loadData();
}

/** 重置 */
function handleReset() {
  queryKeyword.value = '';
  currentPage.value = 1;
  loadData();
}

/** 分页变化 */
function handlePaginationChange() {
  loadData();
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽变化 */
function handleResizeColumn() {}

/**
 * 打开设变明细详情
 * @param record 转置行
 */
async function handleDetail(record: EcExecTransposed) {
  detailVisible.value = true;
  detailLoading.value = true;
  detailData.value = null;
  try {
    detailData.value = await getEcDetailById(record.ecDetailId);
  } finally {
    detailLoading.value = false;
  }
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcBukanData(buildQueryParams());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.bukan')}.xlsx`;
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
