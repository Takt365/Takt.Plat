<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：ec-dept-exec-lr-page.vue -->
<!-- 功能描述：执行部门左右主子表壳：左栏设变明细主表（TaktEcDetail），右栏本部门执行行 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getEcDetailId"
      :master-row-selection="rowSelection"
      master-id-column-key="ecDetailId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
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
          :show-export="false"
          :show-expand="false"
          :show-advanced-query="false"
          :show-column-setting="true"
          :show-fullscreen="true"
          :show-refresh="true"
          :refresh-loading="loading"
          @column-setting="columnSettingVisible = true"
          @refresh="handleRefresh"
        />
      </template>
      <template #detail>
        <EcDeptExecLinePanel
          class="h-full min-h-0 flex-1"
          :update-permission="updatePermission"
          :export-permission="exportPermission"
          :menu-i18n-key="menuI18nKey"
          :id-field="idField"
          :dept-slug="deptSlug"
          :extra-query="extraQuery"
          :get-line-list="getLineList"
          :update-line="updateLine"
          :export-lines="exportLines"
          :form-component="formComponent"
        />
      </template>
    </TaktMasterDetailTableLr>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecDetailId"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门左右主子表页面壳（左栏 TaktEcDetail）
 */
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import type { TaktPagedResult } from '@/types/common'
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
import type { TaktEcExecCode } from '@/constants/logistics/ec-exec-codes'
import { useEcExecSignalRGroup } from '@/composables/use-ec-dept-signalr-group'
import {
  ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS,
  buildEcDetailTableColumns,
  useEcDetailI18n,
} from '../ec-gijutsu/composables/use-ec-detail-i18n'
import { provideEcDeptExecMasterContext } from '../composables/use-ec-dept-exec-master-context'
import EcDeptExecLinePanel from './ec-dept-exec-line-panel.vue'

const props = defineProps<{
  /** 页面列表权限（与菜单种子 Permission 一致，末段 list） */
  listPermission: string
  /** 更新权限（与控制器 [TaktPermission] 一致，末段 update） */
  updatePermission: string
  /** 导出权限（与控制器 [TaktPermission] 一致，末段 export） */
  exportPermission: string
  /** 菜单 i18n 键 */
  menuI18nKey: string
  /** 执行行主键字段 */
  idField: string
  /** 部门实体 slug（eckoubai / echinkan 等，用于子表按实体出列） */
  deptSlug: string
  /** SignalR 部门编码 */
  execCode: TaktEcExecCode
  /** 左栏设变明细主表列表 */
  getMasterList: (query: any) => Promise<TaktPagedResult<EcDetail>>
  /** 右栏执行行列表 */
  getLineList: (query: any) => Promise<TaktPagedResult<any>>
  /** 更新执行行 */
  updateLine: (id: string, dto: any) => Promise<any>
  /** 导出执行行 */
  exportLines: (query?: any) => Promise<Blob>
  /** 编辑表单组件 */
  formComponent: Component
  /** 附加查询参数（如制二页签 pcbaTab） */
  extraQuery?: Record<string, unknown>
}>()

const pi = useEcDetailI18n()
const { t } = useI18n()
const { selectedMasterRow } = provideEcDeptExecMasterContext()

/** 查询关键词 */
const queryKeyword = ref('')
/** 主表 loading */
const loading = ref(false)
/** 主表数据 */
const dataSource = ref<EcDetail[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 选中行 */
const selectedRows = ref<EcDetail[]>([])
/** 选中 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 主表选中 key */
const selectedMasterKey = ref('')
/** 列设置 */
const columnSettingVisible = ref(false)

/** 行选择 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
  },
}))

/**
 * 主表主键（TaktEcDetail）
 * @param record 主表行
 * @returns {string} 主键
 */
function getEcDetailId(record: EcDetail | Record<string, unknown>) {
  return String((record as EcDetail).ecDetailId ?? '')
}

/** 主表列（完整 TaktEcDetail 业务字段；默认可见列见 ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS） */
const columns = computed<TableColumnsType>(() =>
  buildEcDetailTableColumns((field) => pi.columnLabel(field)),
)
/** 可见列（默认子集；列设置可打开全部） */
const visibleColumnKeys = ref<string[]>([...ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS])

/**
 * 同步主表选中到右栏
 * @param record 主表行
 */
function syncMasterSelection(record: EcDetail | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getEcDetailId(record) : ''
}

/**
 * 主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as EcDetail
  const key = getEcDetailId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  syncMasterSelection(row)
}

/** 主表分页 */
function handleMasterPaginationChange() {
  loadData()
}

/** 加载主表（GET 列表，不是提交） */
async function loadData() {
  loading.value = true
  try {
    const res = await props.getMasterList({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
    const currentKey = selectedMasterKey.value
    if (currentKey) {
      const found = dataSource.value.find((row) => getEcDetailId(row) === currentKey)
      syncMasterSelection(found ?? null)
      if (!found) {
        selectedRowKeys.value = []
        selectedRows.value = []
      }
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
    syncMasterSelection(null)
    selectedRowKeys.value = []
    selectedRows.value = []
  } finally {
    loading.value = false
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 刷新 */
function handleRefresh() {
  loadData()
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽 */
function handleResizeColumn() {}

/**
 * 列显隐
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [...ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS]
}

useTableRefresh(loadData)
useEcExecSignalRGroup(props.execCode)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>
