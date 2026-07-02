<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-batch -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变投入批次转置列表（行=设变明细，列=各阶段日期+批次） -->
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
      export-permission="logistics:manufacturing:engineering:change:batch:export"
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
            <a-typography-link @click.stop="handleDetail(record as EcExecBatchTransposed)">
              {{ record.ecNo }}
            </a-typography-link>
          </template>
          <template v-else-if="String(column.key ?? '').startsWith('stageDate_')">
            <span>{{ formatStageDate(record, String(column.key)) }}</span>
          </template>
          <template v-else-if="String(column.key ?? '').startsWith('stageBatch_')">
            <span>{{ formatStageBatch(record, String(column.key)) }}</span>
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
      :title="t('common.dialog.title.detail', { entity: t('menu.logistics.manufacturing.engineering.change.batch') })"
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
 * 设变投入批次转置列表页
 */
import { RiEyeLine } from '@remixicon/vue';
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { getEcBatchTransposedList } from '@/api/logistics/manufacturing/engineering-change/ec-batch';
import { getEcDetailById } from '@/api/logistics/manufacturing/engineering-change/ec-detail';
import { exportEcBatchData } from '@/api/logistics/manufacturing/engineering-change/ec-batch';
import { CreateActionColumn } from '@/components/business/takt-action-column/index';
import { TaktEcBatchStageCodes } from '@/constants/logistics/ec-batch-stage-codes';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ec-detail';
import type {
  EcExecBatchTransposed,
  EcExecBatchTransposedResult,
} from '@/types/logistics/manufacturing/engineering-change/ec-exec-transposed';

const { t } = useI18n();
const localePrefix = 'logistics.manufacturing.engineering-change.ec-batch.page';
/** 列表 loading */
const loading = ref(false);
/** 转置结果 */
const transposedResult = ref<EcExecBatchTransposedResult | null>(null);
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
 * 阶段列标题映射
 * @param stageCode 阶段编码
 * @param kind 日期或批次
 * @returns 列标题
 */
function stageColumnTitle(stageCode: string, kind: 'date' | 'batch'): string {
  const map: Record<string, { date: string; batch: string }> = {
    [TaktEcBatchStageCodes.Scheduled]: {
      date: t(`${localePrefix}.stage.scheduledDate`),
      batch: t(`${localePrefix}.stage.scheduledBatch`),
    },
    [TaktEcBatchStageCodes.Outbound]: {
      date: t(`${localePrefix}.stage.outboundDate`),
      batch: t(`${localePrefix}.stage.outboundBatch`),
    },
    [TaktEcBatchStageCodes.PcbaProduction]: {
      date: t(`${localePrefix}.stage.pcbaProductionDate`),
      batch: t(`${localePrefix}.stage.pcbaProductionBatch`),
    },
    [TaktEcBatchStageCodes.AssyProduction]: {
      date: t(`${localePrefix}.stage.assyProductionDate`),
      batch: t(`${localePrefix}.stage.assyProductionBatch`),
    },
    [TaktEcBatchStageCodes.SampleInspection]: {
      date: t(`${localePrefix}.stage.sampleInspectionDate`),
      batch: '',
    },
  };
  const item = map[stageCode];
  if (!item) return stageCode;
  return kind === 'date' ? item.date : item.batch;
}

/** 动态列 */
const columns = computed(() => {
  const order = transposedResult.value?.stageCodeOrder ?? [];
  const base: TableColumnsType = [
    { title: t('entity.ec.no'), dataIndex: 'ecNo', key: 'ecNo', width: 110, fixed: 'left' as const },
    { title: t(`${localePrefix}.column.technicalLiaisonNo`), dataIndex: 'technicalLiaisonNo', key: 'technicalLiaisonNo', width: 110 },
    { title: t(`${localePrefix}.column.pNo`), dataIndex: 'pNo', key: 'pNo', width: 100 },
    { title: t(`${localePrefix}.column.tcjLiaisonNo`), dataIndex: 'tcjLiaisonNo', key: 'tcjLiaisonNo', width: 120 },
    { title: t('entity.ec.issuedate'), dataIndex: 'ecIssueDate', key: 'ecIssueDate', width: 100 },
    { title: t('entity.ecdetail.ecmodel'), dataIndex: 'ecModel', key: 'ecModel', width: 100 },
    { title: t('entity.ecdetail.ecnewitem'), dataIndex: 'ecNewItem', key: 'ecNewItem', width: 120 },
    { title: t(`${localePrefix}.column.ecEntryDate`), dataIndex: 'ecEntryDate', key: 'ecEntryDate', width: 100 },
  ];
  order.forEach((stageCode) => {
    base.push({
      title: stageColumnTitle(stageCode, 'date'),
      dataIndex: `stageDate_${stageCode}`,
      key: `stageDate_${stageCode}`,
      width: 100,
    });
    if (stageCode !== TaktEcBatchStageCodes.SampleInspection) {
      base.push({
        title: stageColumnTitle(stageCode, 'batch'),
        dataIndex: `stageBatch_${stageCode}`,
        key: `stageBatch_${stageCode}`,
        width: 120,
      });
    }
  });
  base.push(
    CreateActionColumn<EcExecBatchTransposed>({
      actions: [
        {
          key: 'detail',
          label: t('common.page.button.detail'),
          shape: 'plain',
          icon: RiEyeLine,
          permission: 'logistics:manufacturing:engineering:change:batch:detail',
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

/** 表格行（扁平化阶段列） */
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
 * 阶段日期展示
 * @param record 行数据
 * @param columnKey 列 key
 * @returns 展示文本
 */
function formatStageDate(record: Record<string, unknown>, columnKey: string): string {
  const stageCode = columnKey.replace('stageDate_', '');
  const row = record as unknown as EcExecBatchTransposed;
  const cell = row.stageCells?.[stageCode];
  if (cell?.dateDisplayText) return cell.dateDisplayText;
  return cell?.stageDate ? String(cell.stageDate).slice(0, 10).replace(/-/g, '') : '';
}

/**
 * 阶段批次展示
 * @param record 行数据
 * @param columnKey 列 key
 * @returns 展示文本
 */
function formatStageBatch(record: Record<string, unknown>, columnKey: string): string {
  const stageCode = columnKey.replace('stageBatch_', '');
  const row = record as unknown as EcExecBatchTransposed;
  return row.stageCells?.[stageCode]?.batchNo ?? '';
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
    const result = await getEcBatchTransposedList(buildQueryParams());
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
async function handleDetail(record: EcExecBatchTransposed) {
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
    const blob = await exportEcBatchData(buildQueryParams());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.batch')}.xlsx`;
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
