<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：source-ec-input.vue -->
<!-- 功能描述：来源设变录入：查询尚未导入设变主的来源设变，加载草稿至 ec-form（不落库）；列表表高为当前窗体视口 × 5/4；defineExpose 提供 resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="rootEl"
    class="flex flex-col gap-3 min-h-0"
  >
    <!-- 工厂与查询 -->
    <a-form
      layout="inline"
      label-align="right"
      class="flex flex-wrap gap-y-2"
    >
      <a-form-item
        :label="pi.label('plantCode')"
      >
        <a-input
          :value="mappedPlantCode"
          disabled
          class="w-28"
        />
        <div class="text-xs text-text-secondary mt-1">
          {{ t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.plantFromCompany', { company: mappedCompanyCode || '—', plant: mappedPlantCode || '—' }) }}
        </div>
      </a-form-item>
      <a-form-item :label="t('common.page.button.query')">
        <a-input-search
          v-model:value="queryKeyword"
          :placeholder="t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.searchPlaceholder')"
          allow-clear
          class="w-64"
          :loading="listLoading"
          @search="handleSearch"
        />
      </a-form-item>
    </a-form>
    <!-- 未导入来源设变列表 -->
    <div
      class="source-ec-input-table-wrap min-h-0"
      :style="{ minHeight: `${sourceEcTableScrollYPx}px` }"
    >
      <TaktSingleTable
        class="min-h-0"
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'sourceEcId'"
        table-mode="single"
        :data-source="dataSource"
        :loading="listLoading"
        :stripe="true"
        :row-key="getSourceEcId"
        :row-selection="rowSelection"
        :include-audit-fields="false"
        scroll-layout="editable"
        :scroll="sourceEcTableScroll"
        :show-pagination="false"
        @change="handleTableChange"
      />
    </div>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
    />
    <div class="flex justify-end gap-2 pt-1">
      <a-button @click="handleReset">
        {{ t('common.page.button.reset') }}
      </a-button>
      <a-button
        v-permission="'logistics:manufacturing:engineering:change:gijutsu:create'"
        type="primary"
        :loading="draftLoading"
        :disabled="!mappedPlantCode || !selectedSourceEcId"
        @click="handleLoadToForm"
      >
        {{ t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.loadToForm') }}
      </a-button>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 来源设变录入：展示尚未导入设变主的来源设变，加载草稿至 ec-form（不落库）
 */
import { computed, ref, watch, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import {
  computeFormHostRatioScrollYPx,
  TAKT_TABLE_SCROLL_Y_MIN,
} from '@/utils/table-scroll'
import { getEcGijutsuDraftFromSourceEc, getEcGijutsuSourcePlantCode, getUnimportedSourceEcGijutsuList } from '@/api/logistics/manufacturing/engineering-change/ec-gijutsu'
import type { EcGijutsuSourceEcInputItem } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu-source-input'
import type { EcGijutsuFormData } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'
import { useUserStore } from '@/stores/identity/user'
import { useTenantStore } from '@/stores/identity/tenant'
import {
  getSourceEcInputField,
  SOURCE_EC_INPUT_EXTRA_FIELDS,
  SOURCE_EC_INPUT_SOURCE_FIELDS,
  useSourceEcInputI18n,
  type SourceEcInputListField,
  type SourceEcInputSourceField,
} from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/use-source-ec-input-fields'

const emit = defineEmits<{
  /** 草稿已就绪，父级打开 ec-form */
  'draft-ready': [draft: EcGijutsuFormData]
}>()

const { t } = useI18n()
const pi = useSourceEcInputI18n()
const userStore = useUserStore()
const tenantStore = useTenantStore()

/** 映射后的目标工厂代码（Database:CompanyCodes/PlantCodes 同序） */
const mappedPlantCode = ref('')
/** 映射来源公司代码 */
const mappedCompanyCode = ref('')
/** 关键词 */
const queryKeyword = ref('')
/** 列表 loading */
const listLoading = ref(false)
/** 加载草稿 loading */
const draftLoading = ref(false)
/** 列表数据 */
const dataSource = ref<EcGijutsuSourceEcInputItem[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 选中来源设变 ID（单选） */
const selectedSourceEcId = ref('')
/** 默认可见列（弹窗内嵌列表，不含主键列） */
const visibleColumnKeys = ref<string[]>([
  ...SOURCE_EC_INPUT_SOURCE_FIELDS,
  ...SOURCE_EC_INPUT_EXTRA_FIELDS,
])

/** 根节点（用于定位最近弹窗窗体） */
const rootEl = ref<HTMLElement | null>(null)
/** 窗体 ResizeObserver */
let formHostResizeObserver: ResizeObserver | null = null

/** 来源设变列表 scroll.y = 当前窗体视口高度 × 5/4 */
function computeSourceEcTableScrollYPx(): number {
  return Math.max(
    TAKT_TABLE_SCROLL_Y_MIN,
    computeFormHostRatioScrollYPx(rootEl.value, 5, 4),
  )
}

/** 来源设变列表纵向滚动高度（px） */
const sourceEcTableScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)

/** 来源设变列表 scroll 配置 */
const sourceEcTableScroll = computed(() => ({ y: sourceEcTableScrollYPx.value }))

/** 按当前窗体视口重算列表高度 */
function recalcSourceEcTableScrollY(): void {
  sourceEcTableScrollYPx.value = computeSourceEcTableScrollYPx()
}

/**
 * 绑定窗体 ResizeObserver（优先 .ant-modal-content）
 */
function bindFormHostResizeObserver(): void {
  formHostResizeObserver?.disconnect()
  formHostResizeObserver = null
  const host = rootEl.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const target =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
    ?? host
  formHostResizeObserver = new ResizeObserver(() => {
    recalcSourceEcTableScrollY()
  })
  formHostResizeObserver.observe(target)
}

onMounted(() => {
  void nextTick(() => {
    recalcSourceEcTableScrollY()
    bindFormHostResizeObserver()
  })
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', recalcSourceEcTableScrollY)
  }
})

onBeforeUnmount(() => {
  formHostResizeObserver?.disconnect()
  formHostResizeObserver = null
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', recalcSourceEcTableScrollY)
  }
})

/** 来源设变列表列宽 */
const SOURCE_EC_INPUT_COLUMN_WIDTH: Partial<Record<SourceEcInputListField, number>> = {
  sourceEcCode: 120,
  sourceModel: 120,
  sourceTitle: 180,
  sourceStatus: 120,
  sourceIssueDate: 120,
  sourceTcjOwner: 120,
  detailCount: 90,
}

/**
 * 构建来源设变实体列
 * @param field 字段名
 */
function buildSourceEcInputSourceColumn(field: SourceEcInputSourceField) {
  const width = SOURCE_EC_INPUT_COLUMN_WIDTH[field] ?? 120
  const base = {
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcGijutsuSourceEcInputItem }) =>
      getSourceEcInputField(record, field) ?? '',
  }
  if (field === 'sourceEcCode') {
    return {
      ...base,
      sorter: (a: EcGijutsuSourceEcInputItem, b: EcGijutsuSourceEcInputItem) =>
        String(getSourceEcInputField(a, field) ?? '').localeCompare(String(getSourceEcInputField(b, field) ?? '')),
    }
  }
  if (field === 'sourceIssueDate') {
    return {
      ...base,
      sorter: (a: EcGijutsuSourceEcInputItem, b: EcGijutsuSourceEcInputItem) =>
        new Date(String(getSourceEcInputField(a, field) ?? 0)).getTime()
        - new Date(String(getSourceEcInputField(b, field) ?? 0)).getTime(),
    }
  }
  return base
}

/** 表格列（与 TaktSourceEc / TaktEcGijutsuSourceEcInputItemDto 对齐） */
const columns = computed<TableColumnsType>(() => [
  ...SOURCE_EC_INPUT_SOURCE_FIELDS.map((field) => buildSourceEcInputSourceColumn(field)),
  {
    title: t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.detailCount'),
    dataIndex: 'detailCount',
    key: 'detailCount',
    width: SOURCE_EC_INPUT_COLUMN_WIDTH.detailCount ?? 90,
    resizable: true,
    align: 'right',
    customRender: ({ record }: { record: EcGijutsuSourceEcInputItem }) =>
      getSourceEcInputField(record, 'detailCount') ?? '',
  },
])

/** 行选择配置（单选） */
const rowSelection = computed(() => ({
  type: 'radio' as const,
  selectedRowKeys: selectedSourceEcId.value ? [selectedSourceEcId.value] : [],
  onChange: (keys: (string | number)[]) => {
    selectedSourceEcId.value = keys.length > 0 ? String(keys[0]) : ''
  },
}))

/**
 * 获取来源设变行主键
 * @param record 行数据
 * @returns 主键
 */
function getSourceEcId(record: any): string {
  return String(record?.sourceEcId ?? '')
}

/**
 * 加载当前公司对应的工厂代码映射
 * @returns {Promise<boolean>} 是否成功解析映射
 */
async function loadPlantMapping(): Promise<boolean> {
  if (!tenantStore.companyCode?.trim()) {
    mappedPlantCode.value = ''
    mappedCompanyCode.value = ''
    return false
  }
  try {
    const result = await getEcGijutsuSourcePlantCode()
    mappedCompanyCode.value = result.companyCode ?? ''
    mappedPlantCode.value = result.plantCode ?? ''
    return !!mappedPlantCode.value
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    mappedPlantCode.value = ''
    mappedCompanyCode.value = ''
    return false
  }
}

/**
 * 加载未导入来源设变列表
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  if (!mappedPlantCode.value) {
    dataSource.value = []
    total.value = 0
    return
  }
  listLoading.value = true
  try {
    const result = await getUnimportedSourceEcGijutsuList({
      plantCode: mappedPlantCode.value,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value?.trim() || undefined,
    })
    dataSource.value = result.data ?? []
    total.value = result.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
  } finally {
    listLoading.value = false
  }
}

/** 查询（重置到第一页） */
function handleSearch(): void {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/**
 * 分页变更
 * @param {number} page 页码
 * @param {number} size 每页条数
 */
function handlePaginationChange(page: number, size: number): void {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

/** 表格 change（仅处理排序，分页由 TaktPagination 处理） */
function handleTableChange(): void {}

/** 重置表单与选择 */
function resetFields(): void {
  queryKeyword.value = ''
  dataSource.value = []
  total.value = 0
  currentPage.value = getTaktDefaultPageIndex()
  selectedSourceEcId.value = ''
  mappedPlantCode.value = ''
  mappedCompanyCode.value = ''
}

/** 重置并清空列表 */
function handleReset(): void {
  resetFields()
}

/** 加载选中来源设变草稿至 ec-form（不落库） */
async function handleLoadToForm(): Promise<void> {
  if (!mappedPlantCode.value) {
    message.warning(t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.companyRequired'))
    return
  }
  if (!selectedSourceEcId.value) {
    message.warning(t('logistics.manufacturing.engineering-change.ec-gijutsu.page.sourceEcInput.selectRequired'))
    return
  }
  draftLoading.value = true
  try {
    const draft = await getEcGijutsuDraftFromSourceEc({
      plantCode: mappedPlantCode.value,
      sourceEcId: selectedSourceEcId.value,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    })
    const formDraft: EcGijutsuFormData = {
      ...draft,
      plantCode: draft.plantCode ?? mappedPlantCode.value,
      ecLeader: draft.ecLeader ?? '',
      ecDistinction: draft.ecDistinction === 0 ? undefined : draft.ecDistinction,
      ecDetails: draft.ecDetails ?? [],
      attachments: [],
    }
    delete (formDraft as Record<string, unknown>).notifications
    emit('draft-ready', formDraft)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.operation.failed'))
  } finally {
    draftLoading.value = false
  }
}

watch(
  () => tenantStore.companyCode,
  async () => {
    const mapped = await loadPlantMapping()
    if (mapped) {
      currentPage.value = getTaktDefaultPageIndex()
      selectedSourceEcId.value = ''
      void loadData()
      return
    }
    dataSource.value = []
    total.value = 0
  },
  { immediate: true },
)

defineExpose({
  resetFields,
  loadData,
})
</script>

<style scoped lang="css">
/* 来源设变录入列表：min-height 由 JS 按窗体视口 × 5/4 绑定 */
.source-ec-input-table-wrap {
  min-height: 0;
}
</style>
