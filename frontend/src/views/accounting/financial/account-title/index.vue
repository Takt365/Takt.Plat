<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会计科目实体树表管理页（左树右表），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-account-title">
    <!-- 查询栏 -->
    <div class="accounting-financial-account-title-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        :placeholder="tableSearchPlaceholder"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <!-- 工具栏 -->
    <div class="accounting-financial-account-title-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadFullAccountTitleTree"
      />
      <TaktTreeRightToolsBar
        create-permission="accounting:financial:account:title:create"
        update-permission="accounting:financial:account:title:update"
        delete-permission="accounting:financial:account:title:delete"
        import-permission="accounting:financial:account:title:import"
        export-permission="accounting:financial:account:title:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-import="true"
        :show-export="true"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :show-expand="true"
        :update-disabled="!selectedRow"
        :delete-disabled="!selectedRow && selectedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        :expanded="tableExpanded"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @import="handleImport"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 左树右表 -->
    <div class="accounting-financial-account-title-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selected-keys="selectedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="false"
        :draggable="true"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="company"
        v-model:current="tableCurrentPage"
        v-model:page-size="tablePageSize"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'accountTitleId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="paginatedFlatTableRows"
        :loading="loading"
        :row-key="getAccountTitleId"
        :stripe="true"
        :row-selection="rowSelection"
        :show-pagination="true"
        :total="tableFlatTotal"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'accountTitleName'">
            <span
              class="inline-block"
              :style="{ paddingLeft: `${(record._treeDepth ?? 0) * 16}px` }"
            >
              {{ getAccountTitleField(record, 'accountTitleName') }}
            </span>
          </template>
        <template v-else-if="column.key === 'accountTitleType'">
          <TaktDictTag
            :value="getAccountTitleField(record, 'accountTitleType')"
            dict-type="accounting_account_title_type"
          />
        </template>
        <template v-else-if="column.key === 'auxiliaryType'">
          <TaktDictTag
            :value="getAccountTitleField(record, 'auxiliaryType')"
            dict-type="accounting_auxiliary_type"
          />
        </template>
        <template v-else-if="column.key === 'accountTitleStatus'">
          <a-switch
            :checked="getAccountTitleField(record, 'accountTitleStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleTitleStatusChange(record, Boolean(checked))"
          />
        </template>

        </template>
      </TaktTreeRightTable>
    </div>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <AccountTitleForm
        :key="formData?.accountTitleId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-accounting-financial-account-title'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('accountTitleCode')">
      <a-form-item :label="t('entity.accounttitle.code')">
        <a-input
          v-model:value="advancedQueryForm.accountTitleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleName')">
      <a-form-item :label="t('entity.accounttitle.name')">
        <a-input
          v-model:value="advancedQueryForm.accountTitleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.name') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="t('entity.accounttitle.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.parentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleType')">
      <a-form-item :label="t('entity.accounttitle.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitleType"
          dict-type="accounting_account_title_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('balanceDirection')">
      <a-form-item :label="t('entity.accounttitle.balancedirection')">
        <a-input-number
          v-model:value="advancedQueryForm.balanceDirection"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.balancedirection') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleLevel')">
      <a-form-item :label="t('entity.accounttitle.level')">
        <a-input-number
          v-model:value="advancedQueryForm.accountTitleLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLeaf')">
      <a-form-item :label="t('entity.accounttitle.isleaf')">
        <a-input-number
          v-model:value="advancedQueryForm.isLeaf"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isleaf') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isAuxiliary')">
      <a-form-item :label="t('entity.accounttitle.isauxiliary')">
        <a-input-number
          v-model:value="advancedQueryForm.isAuxiliary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isauxiliary') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('auxiliaryType')">
      <a-form-item :label="t('entity.accounttitle.auxiliarytype')">
        <TaktSelect
          v-model:value="advancedQueryForm.auxiliaryType"
          dict-type="accounting_auxiliary_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.auxiliarytype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQuantity')">
      <a-form-item :label="t('entity.accounttitle.isquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.isQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCurrency')">
      <a-form-item :label="t('entity.accounttitle.iscurrency')">
        <a-input-number
          v-model:value="advancedQueryForm.isCurrency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscurrency') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCash')">
      <a-form-item :label="t('entity.accounttitle.iscash')">
        <a-input-number
          v-model:value="advancedQueryForm.isCash"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscash') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBank')">
      <a-form-item :label="t('entity.accounttitle.isbank')">
        <a-input-number
          v-model:value="advancedQueryForm.isBank"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isbank') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.accounttitle.relatedplant')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleStatus')">
      <a-form-item :label="t('entity.accounttitle.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitleStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="t('entity.accounttitle.validfromstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfromstart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="t('entity.accounttitle.validfromend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfromend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="t('entity.accounttitle.validtostart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validtostart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="t('entity.accounttitle.validtoend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validtoend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ t('common.page.entity.extfield') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.accounttitle._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.accounttitle._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'accountTitleId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会计科目实体树表管理页 · ParentId 左树右表（参照 dept/index.vue）
 * @module views/accounting/financial/account-title
 */
import { ref, computed, watch, watchEffect, onMounted } from 'vue'
import type { TreeDataItem } from 'ant-design-vue/es/tree'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import AccountTitleForm from './components/account-title-form.vue'
import { getAccountTitleTree, getAccountTitleById, createAccountTitle, updateAccountTitle, deleteAccountTitleById, deleteAccountTitleBatch, getAccountTitleTemplate, importAccountTitle, exportAccountTitle, updateAccountTitleStatus } from '@/api/accounting/financial/account-title'
import type { AccountTitle, AccountTitleTree, AccountTitleUpdate } from '@/types/accounting/financial/account-title'
import type { TreeDropPayload } from '@/components/business/takt-tree-left-table/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAccountTitle')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [t('entity.accounttitle.id')].join(' / '),
  })
)

/** 左侧树关键字（客户端过滤，不重复请求 API） */
const treeQueryKeyword = ref('')
/** 右侧树表快捷查询关键字 */
const queryKeyword = ref('')
/** 左侧树工具栏「展开/收缩」状态 */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表 */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧表格展开状态（预留） */
const tableExpanded = ref(false)
/** 右侧拍平列表当前页码 */
const tableCurrentPage = ref(getTaktDefaultPageIndex())
/** 右侧拍平列表每页条数 */
const tablePageSize = ref(getTaktDefaultPageSize())
/** 页面 loading（树加载、提交、导出等） */
const loading = ref(false)
/** 全量树表节点（左侧树与右侧表共用，不受右侧查询过滤） */
const fullTableTree = ref<Record<string, unknown>[]>([])
/** 左侧 a-tree 绑定数据（由 fullTableTree 映射 title/key） */
const entityTreeData = ref<TreeDataItem[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selectedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selectedRow = ref<AccountTitle | null>(null)
/** 表格多选行 */
const selectedRows = ref<AccountTitle[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AccountTitle> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  accountTitleCode: '',
  accountTitleName: '',
  parentId: '',
  accountTitleType: '' as string | undefined,
  balanceDirection: undefined as number | undefined,
  accountTitleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: '' as string | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  plantCode: '',
  accountTitleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'accountTitleCode', label: t('entity.accounttitle.code') },
  { key: 'accountTitleName', label: t('entity.accounttitle.name') },
  { key: 'parentId', label: t('entity.accounttitle.parentid') },
  { key: 'accountTitleType', label: t('entity.accounttitle.type') },
  { key: 'balanceDirection', label: t('entity.accounttitle.balancedirection') },
  { key: 'accountTitleLevel', label: t('entity.accounttitle.level') },
  { key: 'isLeaf', label: t('entity.accounttitle.isleaf') },
  { key: 'isAuxiliary', label: t('entity.accounttitle.isauxiliary') },
  { key: 'auxiliaryType', label: t('entity.accounttitle.auxiliarytype') },
  { key: 'isQuantity', label: t('entity.accounttitle.isquantity') },
  { key: 'isCurrency', label: t('entity.accounttitle.iscurrency') },
  { key: 'isCash', label: t('entity.accounttitle.iscash') },
  { key: 'isBank', label: t('entity.accounttitle.isbank') },
  { key: 'plantCode', label: t('entity.accounttitle.relatedplant') },
  { key: 'accountTitleStatus', label: t('entity.accounttitle.status') },
  { key: 'validFromStart', label: t('entity.accounttitle.validfromstart') },
  { key: 'validFromEnd', label: t('entity.accounttitle.validfromend') },
  { key: 'validToStart', label: t('entity.accounttitle.validtostart') },
  { key: 'validToEnd', label: t('entity.accounttitle.validtoend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'accountTitleId'
/** 树节点标题字段名（左侧树 title：AccountTitleName 按 ParentId 递归） */
const treeTitleField = 'accountTitleName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 解析树节点 key（与列表 accountTitleId、左侧树 key 一致） */
function resolveAccountTitleNodeKey(node: Record<string, unknown>): string {
  const raw = node.key ?? node.accountTitleId ?? node.id
  return raw == null ? '' : String(raw)
}

/**
 * 将接口树转为树表节点（保留 children，供 getSubtree 与左侧树共用 key）
 * @param nodes 实体树 DTO 列表
 */
function accountTitleTreeToTableNodes(nodes: AccountTitleTree[]): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  return nodes.map((node) => {
    const childNodes = node.children?.length ? accountTitleTreeToTableNodes(node.children) : []
    return {
      ...node,
      key: String(node.accountTitleId ?? ''),
      children: childNodes.length > 0 ? childNodes : undefined,
    }
  })
}

/** 将 fullTableTree 转为左侧 a-tree（与右侧表共用 key，保证点选联动） */
function mapFullTableTreeToTreeData(nodes: Array<Record<string, unknown>>): TreeDataItem[] {
  if (!nodes?.length) return []
  return nodes.map((n) => {
    const title = String(n[treeTitleField] ?? n.title ?? '')
    const key = resolveAccountTitleNodeKey(n)
    const children = n.children as Array<Record<string, unknown>> | undefined
    if (!children?.length) return { title, key }
    const mapped = mapFullTableTreeToTreeData(children)
    return mapped.length > 0 ? { title, key, children: mapped } : { title, key }
  })
}

/**
 * 按 key 查找树节点（左侧树与右侧表共用 fullTableTree）
 * @param nodes 树节点列表
 * @param key 节点 key
 */
function findTreeNodeByKey(
  nodes: Array<Record<string, unknown>>,
  key: string | number,
): Record<string, unknown> | null {
  const k = String(key)
  for (const node of nodes) {
    if (resolveAccountTitleNodeKey(node) === k) return node
    const children = node.children as Array<Record<string, unknown>> | undefined
    if (children?.length) {
      const found = findTreeNodeByKey(children, key)
      if (found) return found
    }
  }
  return null
}

/** 从树中取以某 key 为根的子树（返回单元素数组，便于作为表格根） */
function getSubtree(nodes: Array<Record<string, unknown>>, key: string | number): Array<Record<string, unknown>> {
  const node = findTreeNodeByKey(nodes, key)
  return node ? [node] : []
}

/**
 * 按关键字过滤左侧树：保留 title 匹配的节点及其祖先、子孙
 * @param nodes 树节点
 * @param keyword 关键字
 */
function filterTreeByKeyword(nodes: TreeDataItem[], keyword: string): TreeDataItem[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  /** 递归过滤子树 */
  function filter(nodes: TreeDataItem[]): TreeDataItem[] {
    if (!nodes?.length) return []
    return nodes
      .map((node) => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren } as TreeDataItem
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TreeDataItem
        }
        return null
      })
      .filter(Boolean) as TreeDataItem[]
  }
  return filter(nodes)
}

/** 左侧树绑定数据（按 treeQueryKeyword 客户端过滤） */
const filteredTreeData = computed(() =>
  filterTreeByKeyword(entityTreeData.value, treeQueryKeyword.value)
)

/** 从树数据中收集所有有子节点的 key（用于左侧树展开全部） */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  if (!nodes?.length) return []
  const keys: (string | number)[] = []
  for (const node of nodes) {
    const rawKey = node.key ?? node.accountTitleId ?? node.id
    if (rawKey == null) continue
    const key: string | number =
      typeof rawKey === 'string' || typeof rawKey === 'number' ? rawKey : String(rawKey)
    const children = (node.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length > 0) {
      keys.push(key)
      keys.push(...collectTreeExpandableKeys(children))
    }
  }
  return keys
}

/**
 * 深度优先拍平树表行（附带 _treeDepth 供缩进列渲染）
 * @param nodes 树表节点
 * @param depth 当前层级
 */
function flattenAccountTitleTableRows(nodes: Array<Record<string, unknown>>, depth = 0): Array<Record<string, unknown>> {
  if (!nodes?.length) return []
  const rows: Array<Record<string, unknown>> = []
  for (const node of nodes) {
    const childList = node.children as Array<Record<string, unknown>> | undefined
    const { children: _children, ...rest } = node
    rows.push({ ...rest, _treeDepth: depth })
    if (childList?.length) {
      rows.push(...flattenAccountTitleTableRows(childList, depth + 1))
    }
  }
  return rows
}

/** 右侧树表数据：选中左侧节点时显示该节点（含子级）；未选中时显示整棵树 */
const tableTreeData = computed(() => {
  const tree = fullTableTree.value
  if (!tree?.length) return []
  const keys = selectedTreeKeys.value
  if (keys.length > 0) {
    const activeKey = keys[keys.length - 1]
    if (activeKey === undefined) return tree
    const sub = getSubtree(tree, activeKey)
    if (sub.length > 0) return sub
  }
  return tree
})

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matchesAccountTitleRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.accountTitleId ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.accountTitleCode && !String(record.accountTitleCode ?? '').includes(String(advancedQueryForm.value.accountTitleCode))) return false
  if (advancedQueryForm.value.accountTitleName && !String(record.accountTitleName ?? '').includes(String(advancedQueryForm.value.accountTitleName))) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.accountTitleType && record.accountTitleType !== advancedQueryForm.value.accountTitleType) return false
  if (advancedQueryForm.value.balanceDirection !== undefined && record.balanceDirection !== advancedQueryForm.value.balanceDirection) return false
  if (advancedQueryForm.value.accountTitleLevel !== undefined && record.accountTitleLevel !== advancedQueryForm.value.accountTitleLevel) return false
  if (advancedQueryForm.value.isLeaf !== undefined && record.isLeaf !== advancedQueryForm.value.isLeaf) return false
  if (advancedQueryForm.value.isAuxiliary !== undefined && record.isAuxiliary !== advancedQueryForm.value.isAuxiliary) return false
  if (advancedQueryForm.value.auxiliaryType && record.auxiliaryType !== advancedQueryForm.value.auxiliaryType) return false
  if (advancedQueryForm.value.isQuantity !== undefined && record.isQuantity !== advancedQueryForm.value.isQuantity) return false
  if (advancedQueryForm.value.isCurrency !== undefined && record.isCurrency !== advancedQueryForm.value.isCurrency) return false
  if (advancedQueryForm.value.isCash !== undefined && record.isCash !== advancedQueryForm.value.isCash) return false
  if (advancedQueryForm.value.isBank !== undefined && record.isBank !== advancedQueryForm.value.isBank) return false
  if (advancedQueryForm.value.plantCode && String(record.plantCode ?? '') !== String(advancedQueryForm.value.plantCode)) return false
  if (advancedQueryForm.value.accountTitleStatus !== undefined && record.accountTitleStatus !== advancedQueryForm.value.accountTitleStatus) return false
  if (advancedQueryForm.value.validFromStart && !String(record.validFromStart ?? '').includes(String(advancedQueryForm.value.validFromStart))) return false
  if (advancedQueryForm.value.validFromEnd && !String(record.validFromEnd ?? '').includes(String(advancedQueryForm.value.validFromEnd))) return false
  if (advancedQueryForm.value.validToStart && !String(record.validToStart ?? '').includes(String(advancedQueryForm.value.validToStart))) return false
  if (advancedQueryForm.value.validToEnd && !String(record.validToEnd ?? '').includes(String(advancedQueryForm.value.validToEnd))) return false
  if (advancedQueryForm.value.createdAtStart && !String(record.createdAtStart ?? '').includes(String(advancedQueryForm.value.createdAtStart))) return false
  if (advancedQueryForm.value.createdAtEnd && !String(record.createdAtEnd ?? '').includes(String(advancedQueryForm.value.createdAtEnd))) return false
  if (advancedQueryForm.value.extField && !String(record.extField ?? '').includes(String(advancedQueryForm.value.extField))) return false
  if (advancedQueryForm.value.remark && !String(record.remark ?? '').includes(String(advancedQueryForm.value.remark))) return false
  return true
}

/** 右侧拍平后的全部行（先左侧子树，再右侧查询过滤） */
const tableFlatRows = computed(() =>
  flattenAccountTitleTableRows(tableTreeData.value).filter(matchesAccountTitleRightQuery)
)
/** 右侧拍平总行数（分页 total） */
const tableFlatTotal = computed(() => tableFlatRows.value.length)
/** 当前页行数据 */
const paginatedFlatTableRows = computed(() => {
  const start = (tableCurrentPage.value - 1) * tablePageSize.value
  return tableFlatRows.value.slice(start, start + tablePageSize.value)
})

/** 左侧选中节点或查询变化时，右侧拍平列表重置到第一页 */
watch(tableTreeData, () => {
  tableCurrentPage.value = getTaktDefaultPageIndex()
})

/** 左侧树选中：重置右侧分页到第一页 */
const handleTreeSelect = (selectedKeys: (string | number)[]) => {
  selectedTreeKeys.value = selectedKeys
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param accountTitle 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {AccountTitleUpdate} 更新载荷
 */
function buildAccountTitleUpdateDto(
  accountTitle: AccountTitle,
  overrides: Pick<AccountTitleUpdate, 'parentId' | 'sortOrder'>,
): AccountTitleUpdate {
  return {
    accountTitleId: String(accountTitle.accountTitleId),
    tenantCode: accountTitle.tenantCode,
    companyCode: accountTitle.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    accountTitleCode: accountTitle.accountTitleCode,
    accountTitleName: accountTitle.accountTitleName,
    parentId: overrides.parentId,
    accountTitleType: accountTitle.accountTitleType,
    balanceDirection: accountTitle.balanceDirection,
    accountTitleLevel: accountTitle.accountTitleLevel,
    isAuxiliary: accountTitle.isAuxiliary,
    auxiliaryType: accountTitle.auxiliaryType,
    isQuantity: accountTitle.isQuantity,
    isCurrency: accountTitle.isCurrency,
    isCash: accountTitle.isCash,
    isBank: accountTitle.isBank,
    plantCode: accountTitle.plantCode,
    accountTitleStatus: accountTitle.accountTitleStatus,
    validFrom: accountTitle.validFrom,
    validTo: accountTitle.validTo,
    extField: accountTitle.extField,
    remark: accountTitle.remark,
  }
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.accountTitleId ?? '')
    if (k === keyStr) {
      return { parentId: parentKey, sortOrder: i }
    }
    const children = (node?.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length) {
      const found = findParentAndOrderNum(children, targetKey, k)
      if (found) return found
    }
  }
  return null
}

/**
 * 左侧树拖拽完成后更新 parentId 与 sortOrder
 * @param payload 新树数据与被拖拽节点 key
 */
const handleTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    entityTreeData.value = newTreeData
    const full = await getAccountTitleById(String(dragKey))
    await updateAccountTitle(String(dragKey), buildAccountTitleUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    message.success(t('common.feedback.updated', { target: t('entity.accounttitle._self') }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: t('entity.accounttitle._self') })))
    await loadFullAccountTitleTree().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

/** 左侧树关键字搜索（客户端过滤，不重复请求接口） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 左侧展开/收缩：工具栏展开状态与树展开 key 联动 */
watch(treeExpanded, (expanded) => {
  if (expanded) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  } else {
    treeExpandedKeys.value = []
  }
})

/** 过滤后的左侧树变化且处于展开态时，同步 expandable keys */
watch(filteredTreeData, () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
})

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type AccountTitleRowRecord = AccountTitle | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getAccountTitleId = (record: AccountTitleRowRecord): string => {
  if (record != null && 'accountTitleId' in record && (record as Record<string, unknown>).accountTitleId != null) {
    return String((record as Record<string, unknown>).accountTitleId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getAccountTitleField = (record: AccountTitleRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getAccountTitleDictValue = (
  record: AccountTitleRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 从异常对象提取用户可见消息 */
const getErrorMessage = (error: unknown, fallback: string): string => {
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const messageText = (error as { message?: unknown }).message
    if (typeof messageText === 'string' && messageText.trim()) return messageText
  }
  return fallback
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = ref<TableColumnsType>([])
watchEffect(() => {
  columns.value = [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'accountTitleId',
    key: 'accountTitleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getAccountTitleField(record, 'accountTitleId') ?? getAccountTitleField(record, 'id') ?? '',
  },
  {
    title: t('entity.accounttitle.code'),
    dataIndex: 'accountTitleCode',
    key: 'accountTitleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'accountTitleCode') ?? ''
  },
  {
    title: t('entity.accounttitle.name'),
    dataIndex: 'accountTitleName',
    key: 'accountTitleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'accountTitleName') ?? ''
  },
  {
    title: t('entity.accounttitle.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.accounttitle.type'),
    dataIndex: 'accountTitleType',
    key: 'accountTitleType',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.accounttitle.balancedirection'),
    dataIndex: 'balanceDirection',
    key: 'balanceDirection',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'balanceDirection') ?? ''
  },
  {
    title: t('entity.accounttitle.level'),
    dataIndex: 'accountTitleLevel',
    key: 'accountTitleLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'accountTitleLevel') ?? ''
  },
  {
    title: t('entity.accounttitle.isleaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isLeaf') ?? ''
  },
  {
    title: t('entity.accounttitle.isauxiliary'),
    dataIndex: 'isAuxiliary',
    key: 'isAuxiliary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isAuxiliary') ?? ''
  },
  {
    title: t('entity.accounttitle.auxiliarytype'),
    dataIndex: 'auxiliaryType',
    key: 'auxiliaryType',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.accounttitle.isquantity'),
    dataIndex: 'isQuantity',
    key: 'isQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isQuantity') ?? ''
  },
  {
    title: t('entity.accounttitle.iscurrency'),
    dataIndex: 'isCurrency',
    key: 'isCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isCurrency') ?? ''
  },
  {
    title: t('entity.accounttitle.iscash'),
    dataIndex: 'isCash',
    key: 'isCash',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isCash') ?? ''
  },
  {
    title: t('entity.accounttitle.isbank'),
    dataIndex: 'isBank',
    key: 'isBank',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'isBank') ?? ''
  },
  {
    title: t('entity.accounttitle.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.accounttitle.status'),
    dataIndex: 'accountTitleStatus',
    key: 'accountTitleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.accounttitle.validfrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'validFrom') ?? ''
  },
  {
    title: t('entity.accounttitle.validto'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'validTo') ?? ''
  },
  CreateActionColumn<AccountTitle>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:account:title:update',
        onClick: (record: AccountTitle) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:account:title:delete',
        onClick: (record: AccountTitle) => handleDeleteOne(record)
      }
    ],
  })]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AccountTitle[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: AccountTitle, selected: boolean) => {
    if (selected) selectedRow.value = record
    else if (selectedRow.value && getAccountTitleId(selectedRow.value) === getAccountTitleId(record)) selectedRow.value = null
  },
  onSelectAll: (selected: boolean, selectedRowsData: AccountTitle[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? selectedRowsData[0] : null
  },
}))

/** 加载全量树（左侧树 + 右侧树表共用数据源） */
async function loadFullAccountTitleTree() {
  const res = await getAccountTitleTree('0', true)
  const resAny = res as { data?: AccountTitleTree[]; Data?: AccountTitleTree[] }
  const trees: AccountTitleTree[] = Array.isArray(res) ? res : (resAny?.data ?? resAny?.Data ?? [])
  const tableNodes = accountTitleTreeToTableNodes(trees)
  fullTableTree.value = tableNodes
  entityTreeData.value = mapFullTableTreeToTreeData(tableNodes)
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 初始化或增删改后刷新全量树 */
async function loadData() {
  loading.value = true
  try {
    await loadFullAccountTitleTree()
  } catch (error: unknown) {
    logger.error('[AccountTitle] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    fullTableTree.value = []
    entityTreeData.value = []
  } finally {
    loading.value = false
  }
}

/** 右侧查询（客户端过滤，不请求接口） */
const handleSearch = () => {
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/** 右侧重置（不影响左侧树与 fullTableTree） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  accountTitleCode: '',
  accountTitleName: '',
  parentId: '',
  accountTitleType: '' as string | undefined,
  balanceDirection: undefined as number | undefined,
  accountTitleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: '' as string | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  plantCode: '',
  accountTitleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleTitleStatusChange(record: AccountTitleRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getAccountTitleField(record, 'accountTitleStatus')
  const id = getAccountTitleId(record)
  const row = null
  if (row) {
    row.accountTitleStatus = newVal
  }
  try {
    await updateAccountTitleStatus({ accountTitleId: id, accountTitleStatus: newVal })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: unknown) {
    if (row) {
      row.accountTitleStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
}

/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.accounttitle._self') })
  const keys = selectedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/** 打开编辑弹窗 */
function handleEdit(record: AccountTitle) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.accounttitle._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.accounttitle._self') }))
  }
}

/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateAccountTitle(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.accounttitle._self') }))
    } else {
      await createAccountTitle(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.accounttitle._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    await loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

/** 删除单行 */
async function handleDeleteOne(record: AccountTitle) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.accounttitle._self'), name: t('common.tip.this.target', { target: t('entity.accounttitle._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAccountTitleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.accounttitle._self') }))
      await loadData()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.accounttitle._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.accounttitle._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAccountTitleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.accounttitle._self') }))
      await loadData()
    }
  })
}

/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getAccountTitleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAccountTitle(file, sheetName)
}

/** 导入完成回调：刷新树并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportAccountTitle({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.accounttitle._self') }))
  } catch (error: unknown) {
    logger.error('[AccountTitle] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: t('entity.accounttitle._self') })))
  } finally {
    loading.value = false
  }
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置右侧分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  tableCurrentPage.value = getTaktDefaultPageIndex()
}

/** 重置高级查询表单（不自动查询） */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  accountTitleCode: '',
  accountTitleName: '',
  parentId: '',
  accountTitleType: '' as string | undefined,
  balanceDirection: undefined as number | undefined,
  accountTitleLevel: undefined as number | undefined,
  isLeaf: undefined as number | undefined,
  isAuxiliary: undefined as number | undefined,
  auxiliaryType: '' as string | undefined,
  isQuantity: undefined as number | undefined,
  isCurrency: undefined as number | undefined,
  isCash: undefined as number | undefined,
  isBank: undefined as number | undefined,
  plantCode: '',
  accountTitleStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新树数据 */
function handleRefresh() {
  void loadData()
}

/** 表格 change / 列宽拖拽占位（树表分页在 TaktTreeRightTable 内） */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}

/** 页面挂载：租户上下文就绪后加载分页配置，再拉树数据 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
</script>

<style scoped lang="css">
.accounting-financial-account-title {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.accounting-financial-account-title-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-financial-account-title-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-financial-account-title-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
