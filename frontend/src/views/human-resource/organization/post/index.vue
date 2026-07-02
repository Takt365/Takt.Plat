<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/organization/post -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：岗位实体 代表组织架构中的岗位/职位 参照 SAP Position管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="human:resource:organization:post:create"
      update-permission="human:resource:organization:post:update"
      delete-permission="human:resource:organization:post:delete"
      import-permission="human:resource:organization:post:import"
      export-permission="human:resource:organization:post:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'postId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPostId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'postStatus'">
          <a-switch
            :checked="getPostField(record, 'postStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handlePostStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'postCategory'">
          <TaktDictTag
            :value="getPostField(record, 'postCategory')"
            dict-type="sys_post_category"
          />
        </template>
        <template v-else-if="column.key === 'postLevel'">
          <TaktDictTag
            :value="getPostField(record, 'postLevel')"
            dict-type="sys_post_level_category"
          />
        </template>
        <template v-else-if="column.key === 'educationRequired'">
          <TaktDictTag
            :value="getPostField(record, 'educationRequired')"
            dict-type="hr_education_level_category"
          />
        </template>
        <template v-else-if="column.key === 'isBuiltIn'">
          <TaktDictTag
            :value="getPostField(record, 'isBuiltIn')"
            dict-type="sys_yes_no_type"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

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
      <PostForm
        :key="formData?.postId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-organization-post'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('postCode')">
      <a-form-item :label="t('entity.post.code')">
        <a-input
          v-model:value="advancedQueryForm.postCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postName')">
      <a-form-item :label="t('entity.post.name')">
        <a-input
          v-model:value="advancedQueryForm.postName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.name') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.post.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.deptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postCategory')">
      <a-form-item :label="t('entity.post.category')">
        <TaktSelect
          v-model:value="advancedQueryForm.postCategory"
          dict-type="sys_post_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.category') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postLevel')">
      <a-form-item :label="t('entity.post.level')">
        <TaktSelect
          v-model:value="advancedQueryForm.postLevel"
          dict-type="sys_post_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.level') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headcount')">
      <a-form-item :label="t('entity.post.headcount')">
        <a-input-number
          v-model:value="advancedQueryForm.headcount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.headcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentCount')">
      <a-form-item :label="t('entity.post.currentcount')">
        <a-input-number
          v-model:value="advancedQueryForm.currentCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.currentcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibilities')">
      <a-form-item :label="t('entity.post.responsibilities')">
        <a-input
          v-model:value="advancedQueryForm.responsibilities"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.responsibilities') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requirements')">
      <a-form-item :label="t('entity.post.requirements')">
        <a-input
          v-model:value="advancedQueryForm.requirements"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.requirements') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('educationRequired')">
      <a-form-item :label="t('entity.post.educationrequired')">
        <TaktSelect
          v-model:value="advancedQueryForm.educationRequired"
          dict-type="hr_education_level_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.educationrequired') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('experienceYears')">
      <a-form-item :label="t('entity.post.experienceyears')">
        <a-input-number
          v-model:value="advancedQueryForm.experienceYears"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.experienceyears') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salaryMin')">
      <a-form-item :label="t('entity.post.salarymin')">
        <a-input-number
          v-model:value="advancedQueryForm.salaryMin"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.salarymin') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salaryMax')">
      <a-form-item :label="t('entity.post.salarymax')">
        <a-input-number
          v-model:value="advancedQueryForm.salaryMax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.post.salarymax') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postStatus')">
      <a-form-item :label="t('entity.post.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.postStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.post.isbuiltin')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.post.isbuiltin') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.post.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.post.description') })"
          :rows="2"
          allow-clear
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
      :title="t('common.dialog.title.import', { entity: t('entity.post._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.post._self"
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
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'postId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 岗位实体 代表组织架构中的岗位/职位 参照 SAP Position管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/organization/post
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import PostForm from './components/post-form.vue'
import { getPostList, getPostById, createPost, updatePost, deletePostById, deletePostBatch, getPostTemplate, importPost, exportPost, updatePostStatus } from '@/api/human-resource/organization/post'
import type { Post, PostQuery } from '@/types/human-resource/organization/post'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPost')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.post._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Post[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Post | null>(null)
/** 表格多选行 */
const selectedRows = ref<Post[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Post> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  postCode: '',
  postName: '',
  deptId: '',
  postCategory: '',
  postLevel: '',
  headcount: undefined as number | undefined,
  currentCount: undefined as number | undefined,
  responsibilities: '',
  requirements: '',
  educationRequired: undefined as number | undefined,
  experienceYears: undefined as number | undefined,
  salaryMin: undefined as number | undefined,
  salaryMax: undefined as number | undefined,
  postStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'postCode', label: t('entity.post.code') },
  { key: 'postName', label: t('entity.post.name') },
  { key: 'deptId', label: t('entity.post.deptid') },
  { key: 'postCategory', label: t('entity.post.category') },
  { key: 'postLevel', label: t('entity.post.level') },
  { key: 'headcount', label: t('entity.post.headcount') },
  { key: 'currentCount', label: t('entity.post.currentcount') },
  { key: 'responsibilities', label: t('entity.post.responsibilities') },
  { key: 'requirements', label: t('entity.post.requirements') },
  { key: 'educationRequired', label: t('entity.post.educationrequired') },
  { key: 'experienceYears', label: t('entity.post.experienceyears') },
  { key: 'salaryMin', label: t('entity.post.salarymin') },
  { key: 'salaryMax', label: t('entity.post.salarymax') },
  { key: 'postStatus', label: t('entity.post.status') },
  { key: 'isBuiltIn', label: t('entity.post.isbuiltin') },
  { key: 'description', label: t('entity.post.description') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'postId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PostQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PostQuery>): PostQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PostQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PostQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('postCode', form.postCode)
  assignTrimmed('postName', form.postName)
  assignTrimmed('deptId', form.deptId)
  assignTrimmed('postCategory', form.postCategory)
  assignTrimmed('postLevel', form.postLevel)
  if (form.headcount !== undefined && form.headcount !== null) {
    query.headcount = form.headcount
  }
  if (form.currentCount !== undefined && form.currentCount !== null) {
    query.currentCount = form.currentCount
  }
  assignTrimmed('responsibilities', form.responsibilities)
  assignTrimmed('requirements', form.requirements)
  if (form.educationRequired !== undefined && form.educationRequired !== null) {
    query.educationRequired = form.educationRequired
  }
  if (form.experienceYears !== undefined && form.experienceYears !== null) {
    query.experienceYears = form.experienceYears
  }
  if (form.salaryMin !== undefined && form.salaryMin !== null) {
    query.salaryMin = form.salaryMin
  }
  if (form.salaryMax !== undefined && form.salaryMax !== null) {
    query.salaryMax = form.salaryMax
  }
  if (form.postStatus !== undefined && form.postStatus !== null) {
    query.postStatus = form.postStatus
  }
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    query.isBuiltIn = form.isBuiltIn
  }
  assignTrimmed('description', form.description)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})







/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'postId',
    key: 'postId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPostField(record, 'postId') ?? ''
  },
  {
    title: t('entity.post.code'),
    dataIndex: 'postCode',
    key: 'postCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'postCode') ?? ''
  },
  {
    title: t('entity.post.name'),
    dataIndex: 'postName',
    key: 'postName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'postName') ?? ''
  },
  {
    title: t('entity.post.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.post.category'),
    dataIndex: 'postCategory',
    key: 'postCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.post.level'),
    dataIndex: 'postLevel',
    key: 'postLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.post.headcount'),
    dataIndex: 'headcount',
    key: 'headcount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'headcount') ?? ''
  },
  {
    title: t('entity.post.currentcount'),
    dataIndex: 'currentCount',
    key: 'currentCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'currentCount') ?? ''
  },
  {
    title: t('entity.post.responsibilities'),
    dataIndex: 'responsibilities',
    key: 'responsibilities',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'responsibilities') ?? ''
  },
  {
    title: t('entity.post.requirements'),
    dataIndex: 'requirements',
    key: 'requirements',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'requirements') ?? ''
  },
  {
    title: t('entity.post.educationrequired'),
    dataIndex: 'educationRequired',
    key: 'educationRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.post.experienceyears'),
    dataIndex: 'experienceYears',
    key: 'experienceYears',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'experienceYears') ?? ''
  },
  {
    title: t('entity.post.salarymin'),
    dataIndex: 'salaryMin',
    key: 'salaryMin',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'salaryMin') ?? ''
  },
  {
    title: t('entity.post.salarymax'),
    dataIndex: 'salaryMax',
    key: 'salaryMax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'salaryMax') ?? ''
  },
  {
    title: t('entity.post.status'),
    dataIndex: 'postStatus',
    key: 'postStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.post.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.post.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'description') ?? ''
  },
  {
    title: t('entity.post.employeeposts'),
    dataIndex: 'employeePosts',
    key: 'employeePosts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPostField(record, 'employeePosts') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:organization:post:update',
        onClick: (record: Post) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:organization:post:delete',
        onClick: (record: Post) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPostId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPostField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Post[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Post, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPostId(selectedRow.value) === getPostId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Post[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Post) => ({
  onClick: () => {
    const key = getPostId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPostId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getPostList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Post] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  postCode: '',
  postName: '',
  deptId: '',
  postCategory: '',
  postLevel: '',
  headcount: undefined as number | undefined,
  currentCount: undefined as number | undefined,
  responsibilities: '',
  requirements: '',
  educationRequired: undefined as number | undefined,
  experienceYears: undefined as number | undefined,
  salaryMin: undefined as number | undefined,
  salaryMax: undefined as number | undefined,
  postStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.post._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: Post) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.post._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.post._self') }))
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
      await updatePost(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.post._self') }))
    } else {
      await createPost(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.post._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    loadData()
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
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getPostTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPost(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
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
    const exportMeta = await exportPost(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
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
    message.success(t('common.feedback.export.success', { target: t('entity.post._self') }))
  } catch (error: any) {
    logger.error('[Post] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.post._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Post) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.post._self'), name: t('common.tip.this.target', { target: t('entity.post._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePostById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.post._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.post._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.post._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePostBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.post._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handlePostStatusChange(record: Post, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getPostField(record, 'postStatus')
  const id = getPostId(record)
  const row = dataSource.value.find((item) => getPostId(item) === id)
  if (row) {
    row.postStatus = newVal
  }
  try {
    await updatePostStatus({ postId: id, postStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.postStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  postCode: '',
  postName: '',
  deptId: '',
  postCategory: '',
  postLevel: '',
  headcount: undefined as number | undefined,
  currentCount: undefined as number | undefined,
  responsibilities: '',
  requirements: '',
  educationRequired: undefined as number | undefined,
  experienceYears: undefined as number | undefined,
  salaryMin: undefined as number | undefined,
  salaryMax: undefined as number | undefined,
  postStatus: undefined as number | undefined,
  isBuiltIn: undefined as number | undefined,
  description: '',
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

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
