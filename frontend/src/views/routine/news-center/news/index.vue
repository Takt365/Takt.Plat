<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-news-center-news">
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
      create-permission="routine:newscenter:news:create"
      update-permission="routine:newscenter:news:update"
      delete-permission="routine:newscenter:news:delete"
      import-permission="routine:newscenter:news:import"
      export-permission="routine:newscenter:news:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="true"
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
      :columns="columns"
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'newsId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getNewsId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsAttachment._self') }}</div>
          <a-table
            v-if="hasNewsAttachmentRows(record)"
            :columns="newsAttachmentExpandColumns"
            :data-source="getNewsAttachmentRows(record)"
            :row-key="(row: NewsAttachment, index?: number) => row?.newsAttachmentId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsComment._self') }}</div>
          <a-table
            v-if="hasNewsCommentRows(record)"
            :columns="newsCommentExpandColumns"
            :data-source="getNewsCommentRows(record)"
            :row-key="(row: NewsComment, index?: number) => row?.newsCommentId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsLike._self') }}</div>
          <a-table
            v-if="hasNewsLikeRows(record)"
            :columns="newsLikeExpandColumns"
            :data-source="getNewsLikeRows(record)"
            :row-key="(row: NewsLike, index?: number) => row?.newsLikeId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsRead._self') }}</div>
          <a-table
            v-if="hasNewsReadRows(record)"
            :columns="newsReadExpandColumns"
            :data-source="getNewsReadRows(record)"
            :row-key="(row: NewsRead, index?: number) => row?.newsReadId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsFavorite._self') }}</div>
          <a-table
            v-if="hasNewsFavoriteRows(record)"
            :columns="newsFavoriteExpandColumns"
            :data-source="getNewsFavoriteRows(record)"
            :row-key="(row: NewsFavorite, index?: number) => row?.newsFavoriteId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.newsShare._self') }}</div>
          <a-table
            v-if="hasNewsShareRows(record)"
            :columns="newsShareExpandColumns"
            :data-source="getNewsShareRows(record)"
            :row-key="(row: NewsShare, index?: number) => row?.newsShareId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'newsStatus'">
          <TaktDictTag
            :value="getNewsField(record, 'newsStatus')"
            dict-type="sys_publish_status"
          />
        </template>
      </template>
    </TaktSingleTable>

    <!-- 分页组件 -->
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
      <NewsForm
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
      :storage-key="'takt-query-fields-routine-news-center-news'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('newsCode')">
      <a-form-item :label="t('entity.news.code')">
        <a-input
          v-model:value="advancedQueryForm.newsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCategory')">
      <a-form-item :label="t('entity.news.category')">
        <a-input-number
          v-model:value="advancedQueryForm.newsCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.category') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsTitle')">
      <a-form-item :label="t('entity.news.title')">
        <a-input
          v-model:value="advancedQueryForm.newsTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsSummary')">
      <a-form-item :label="t('entity.news.summary')">
        <a-input
          v-model:value="advancedQueryForm.newsSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.summary') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tags')">
      <a-form-item :label="t('entity.news.tags')">
        <a-input
          v-model:value="advancedQueryForm.tags"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.tags') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsContent')">
      <a-form-item :label="t('entity.news.content')">
        <a-textarea
          v-model:value="advancedQueryForm.newsContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.news.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCoverImage')">
      <a-form-item :label="t('entity.news.coverimage')">
        <a-input
          v-model:value="advancedQueryForm.newsCoverImage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.coverimage') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isTop')">
      <a-form-item :label="t('entity.news.istop')">
        <a-input-number
          v-model:value="advancedQueryForm.isTop"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.istop') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isRecommended')">
      <a-form-item :label="t('entity.news.isrecommended')">
        <a-input-number
          v-model:value="advancedQueryForm.isRecommended"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.isrecommended') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveTimeStart')">
      <a-form-item :label="t('entity.news.effectivetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.effectivetimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveTimeEnd')">
      <a-form-item :label="t('entity.news.effectivetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.effectivetimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeStart')">
      <a-form-item :label="t('entity.news.expiretimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.expiretimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expireTimeEnd')">
      <a-form-item :label="t('entity.news.expiretimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expireTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.expiretimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('readCount')">
      <a-form-item :label="t('entity.news.readcount')">
        <a-input-number
          v-model:value="advancedQueryForm.readCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.readcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('likeCount')">
      <a-form-item :label="t('entity.news.likecount')">
        <a-input-number
          v-model:value="advancedQueryForm.likeCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.likecount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('commentCount')">
      <a-form-item :label="t('entity.news.commentcount')">
        <a-input-number
          v-model:value="advancedQueryForm.commentCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.commentcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('favoriteCount')">
      <a-form-item :label="t('entity.news.favoritecount')">
        <a-input-number
          v-model:value="advancedQueryForm.favoriteCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.favoritecount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shareCount')">
      <a-form-item :label="t('entity.news.sharecount')">
        <a-input-number
          v-model:value="advancedQueryForm.shareCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sharecount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachmentCount')">
      <a-form-item :label="t('entity.news.attachmentcount')">
        <a-input-number
          v-model:value="advancedQueryForm.attachmentCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.attachmentcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.news.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.news.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.news.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherId')">
      <a-form-item :label="t('entity.news.publisherid')">
        <a-input
          v-model:value="advancedQueryForm.publisherId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherName')">
      <a-form-item :label="t('entity.news.publishername')">
        <a-input
          v-model:value="advancedQueryForm.publisherName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeStart')">
      <a-form-item :label="t('entity.news.publishtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.publishtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeEnd')">
      <a-form-item :label="t('entity.news.publishtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.publishtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.news.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsStatus')">
      <a-form-item :label="t('entity.news.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.newsStatus"
          dict-type="sys_publish_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.news.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.news.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.news.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.news.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.news.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.news.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.news.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.news.approvedatend') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.ExtField')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.news._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.news._self"
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
      :id-column-key="'newsId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/news-center/news
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import NewsForm from './components/news-form.vue'
import { getNewsList, getNewsById, createNews, updateNews, deleteNewsById, deleteNewsBatch, getNewsTemplate, importNews, exportNews } from '@/api/routine/news-center/news'
import * as newsAttachmentApi from '@/api/routine/news-center/news-attachment'
import * as newsCommentApi from '@/api/routine/news-center/news-comment'
import * as newsLikeApi from '@/api/routine/news-center/news-like'
import * as newsReadApi from '@/api/routine/news-center/news-read'
import * as newsFavoriteApi from '@/api/routine/news-center/news-favorite'
import * as newsShareApi from '@/api/routine/news-center/news-share'
import type { NewsAttachment, NewsAttachmentQuery } from '@/types/routine/news-center/news-attachment'
import type { NewsComment, NewsCommentQuery } from '@/types/routine/news-center/news-comment'
import type { NewsLike, NewsLikeQuery } from '@/types/routine/news-center/news-like'
import type { NewsRead, NewsReadQuery } from '@/types/routine/news-center/news-read'
import type { NewsFavorite, NewsFavoriteQuery } from '@/types/routine/news-center/news-favorite'
import type { NewsShare, NewsShareQuery } from '@/types/routine/news-center/news-share'
import type { News, NewsQuery, NewsCreate, NewsUpdate } from '@/types/routine/news-center/news'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktNews')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.news._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<News[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<News | null>(null)
/** 表格多选行 */
const selectedRows = ref<News[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<News>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  newsCode: '',
  newsCategory: undefined as number | undefined,
  newsTitle: '',
  newsSummary: '',
  tags: '',
  newsContent: '',
  newsCoverImage: '',
  isTop: undefined as number | undefined,
  isRecommended: undefined as number | undefined,
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  readCount: undefined as number | undefined,
  likeCount: undefined as number | undefined,
  commentCount: undefined as number | undefined,
  favoriteCount: undefined as number | undefined,
  shareCount: undefined as number | undefined,
  attachmentCount: undefined as number | undefined,
  flowInstanceId: '',
  deptId: '',
  deptName: '',
  publisherId: '',
  publisherName: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  sortOrder: undefined as number | undefined,
  newsStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'newsCode', label: t('entity.news.code') },
  { key: 'newsCategory', label: t('entity.news.category') },
  { key: 'newsTitle', label: t('entity.news.title') },
  { key: 'newsSummary', label: t('entity.news.summary') },
  { key: 'tags', label: t('entity.news.tags') },
  { key: 'newsContent', label: t('entity.news.content') },
  { key: 'newsCoverImage', label: t('entity.news.coverimage') },
  { key: 'isTop', label: t('entity.news.istop') },
  { key: 'isRecommended', label: t('entity.news.isrecommended') },
  { key: 'effectiveTimeStart', label: t('entity.news.effectivetimestart') },
  { key: 'effectiveTimeEnd', label: t('entity.news.effectivetimeend') },
  { key: 'expireTimeStart', label: t('entity.news.expiretimestart') },
  { key: 'expireTimeEnd', label: t('entity.news.expiretimeend') },
  { key: 'readCount', label: t('entity.news.readcount') },
  { key: 'likeCount', label: t('entity.news.likecount') },
  { key: 'commentCount', label: t('entity.news.commentcount') },
  { key: 'favoriteCount', label: t('entity.news.favoritecount') },
  { key: 'shareCount', label: t('entity.news.sharecount') },
  { key: 'attachmentCount', label: t('entity.news.attachmentcount') },
  { key: 'flowInstanceId', label: t('entity.news.flowinstanceid') },
  { key: 'deptId', label: t('entity.news.deptid') },
  { key: 'deptName', label: t('entity.news.deptname') },
  { key: 'publisherId', label: t('entity.news.publisherid') },
  { key: 'publisherName', label: t('entity.news.publishername') },
  { key: 'publishTimeStart', label: t('entity.news.publishtimestart') },
  { key: 'publishTimeEnd', label: t('entity.news.publishtimeend') },
  { key: 'sortOrder', label: t('entity.news.sortorder') },
  { key: 'newsStatus', label: t('entity.news.status') },
  { key: 'approvalStatus', label: t('entity.news.approvalstatus') },
  { key: 'initiatorId', label: t('entity.news.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.news.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.news.initiatedatend') },
  { key: 'approvedBy', label: t('entity.news.approvedby') },
  { key: 'approvedAtStart', label: t('entity.news.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.news.approvedatend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
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
const entityIdName = 'newsId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：newsAttachment 列 */
const newsAttachmentExpandColumns = computed(() => [
  {
    title: t('entity.newsAttachment.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.fileextension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    ellipsis: true,
  },
  {
    title: t('entity.newsAttachment.news'),
    dataIndex: 'news',
    key: 'news',
    ellipsis: true,
  },
])

/** 展开行预览：newsComment 列 */
const newsCommentExpandColumns = computed(() => [
  {
    title: t('entity.newsComment.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.useravatar'),
    dataIndex: 'userAvatar',
    key: 'userAvatar',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.replytouserid'),
    dataIndex: 'replyToUserId',
    key: 'replyToUserId',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.replytousername'),
    dataIndex: 'replyToUserName',
    key: 'replyToUserName',
    ellipsis: true,
  },
  {
    title: t('entity.newsComment.commentcontent'),
    dataIndex: 'commentContent',
    key: 'commentContent',
    ellipsis: true,
  },
])

/** 展开行预览：newsLike 列 */
const newsLikeExpandColumns = computed(() => [
  {
    title: t('entity.newsLike.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsLike.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.newsLike.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.newsLike.liketime'),
    dataIndex: 'likeTime',
    key: 'likeTime',
    ellipsis: true,
  },
  {
    title: t('entity.newsLike.news'),
    dataIndex: 'news',
    key: 'news',
    ellipsis: true,
  },
])

/** 展开行预览：newsRead 列 */
const newsReadExpandColumns = computed(() => [
  {
    title: t('entity.newsRead.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsRead.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.newsRead.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.newsRead.readtime'),
    dataIndex: 'readTime',
    key: 'readTime',
    ellipsis: true,
  },
  {
    title: t('entity.newsRead.news'),
    dataIndex: 'news',
    key: 'news',
    ellipsis: true,
  },
])

/** 展开行预览：newsFavorite 列 */
const newsFavoriteExpandColumns = computed(() => [
  {
    title: t('entity.newsFavorite.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsFavorite.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.newsFavorite.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.newsFavorite.favoritetime'),
    dataIndex: 'favoriteTime',
    key: 'favoriteTime',
    ellipsis: true,
  },
  {
    title: t('entity.newsFavorite.news'),
    dataIndex: 'news',
    key: 'news',
    ellipsis: true,
  },
])

/** 展开行预览：newsShare 列 */
const newsShareExpandColumns = computed(() => [
  {
    title: t('entity.newsShare.newsname'),
    dataIndex: 'newsName',
    key: 'newsName',
    ellipsis: true,
  },
  {
    title: t('entity.newsShare.userid'),
    dataIndex: 'userId',
    key: 'userId',
    ellipsis: true,
  },
  {
    title: t('entity.newsShare.username'),
    dataIndex: 'userName',
    key: 'userName',
    ellipsis: true,
  },
  {
    title: t('entity.newsShare.sharechannel'),
    dataIndex: 'shareChannel',
    key: 'shareChannel',
    ellipsis: true,
  },
  {
    title: t('entity.newsShare.sharetime'),
    dataIndex: 'shareTime',
    key: 'shareTime',
    ellipsis: true,
  },
  {
    title: t('entity.newsShare.news'),
    dataIndex: 'news',
    key: 'news',
    ellipsis: true,
  },
])

/** 读取主表行上的 newsAttachment 子表缓存 */
function getNewsAttachmentRows(record: News): NewsAttachment[] {
  return (record as any)?.attachments ?? []
}

/** 主表行是否已加载 newsAttachment 子表 */
function hasNewsAttachmentRows(record: News): boolean {
  return getNewsAttachmentRows(record).length > 0
}

/** 读取主表行上的 newsComment 子表缓存 */
function getNewsCommentRows(record: News): NewsComment[] {
  return (record as any)?.comments ?? []
}

/** 主表行是否已加载 newsComment 子表 */
function hasNewsCommentRows(record: News): boolean {
  return getNewsCommentRows(record).length > 0
}

/** 读取主表行上的 newsLike 子表缓存 */
function getNewsLikeRows(record: News): NewsLike[] {
  return (record as any)?.likes ?? []
}

/** 主表行是否已加载 newsLike 子表 */
function hasNewsLikeRows(record: News): boolean {
  return getNewsLikeRows(record).length > 0
}

/** 读取主表行上的 newsRead 子表缓存 */
function getNewsReadRows(record: News): NewsRead[] {
  return (record as any)?.reads ?? []
}

/** 主表行是否已加载 newsRead 子表 */
function hasNewsReadRows(record: News): boolean {
  return getNewsReadRows(record).length > 0
}

/** 读取主表行上的 newsFavorite 子表缓存 */
function getNewsFavoriteRows(record: News): NewsFavorite[] {
  return (record as any)?.favorites ?? []
}

/** 主表行是否已加载 newsFavorite 子表 */
function hasNewsFavoriteRows(record: News): boolean {
  return getNewsFavoriteRows(record).length > 0
}

/** 读取主表行上的 newsShare 子表缓存 */
function getNewsShareRows(record: News): NewsShare[] {
  return (record as any)?.shares ?? []
}

/** 主表行是否已加载 newsShare 子表 */
function hasNewsShareRows(record: News): boolean {
  return getNewsShareRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadNewsDetail(record: News): Promise<News | null> {
  const id = getNewsId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getNewsById(id)
    const index = dataSource.value.findIndex((row) => getNewsId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as News
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 newsAttachment 子表（NewsAttachmentQuery + newsAttachmentApi，与主表 NewsQuery 分离） */
async function loadNewsAttachmentForNews(record: News): Promise<NewsAttachment[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsAttachmentQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsAttachmentApi.getNewsAttachmentList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, attachments: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 newsComment 子表（NewsCommentQuery + newsCommentApi，与主表 NewsQuery 分离） */
async function loadNewsCommentForNews(record: News): Promise<NewsComment[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsCommentQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsCommentApi.getNewsCommentList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, comments: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 newsLike 子表（NewsLikeQuery + newsLikeApi，与主表 NewsQuery 分离） */
async function loadNewsLikeForNews(record: News): Promise<NewsLike[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsLikeQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsLikeApi.getNewsLikeList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, likes: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 newsRead 子表（NewsReadQuery + newsReadApi，与主表 NewsQuery 分离） */
async function loadNewsReadForNews(record: News): Promise<NewsRead[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsReadQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsReadApi.getNewsReadList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, reads: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 newsFavorite 子表（NewsFavoriteQuery + newsFavoriteApi，与主表 NewsQuery 分离） */
async function loadNewsFavoriteForNews(record: News): Promise<NewsFavorite[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsFavoriteQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsFavoriteApi.getNewsFavoriteList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, favorites: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 newsShare 子表（NewsShareQuery + newsShareApi，与主表 NewsQuery 分离） */
async function loadNewsShareForNews(record: News): Promise<NewsShare[]> {
  const masterId = getNewsId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: NewsShareQuery = {
      pageIndex: 1,
      pageSize: 500,
      newsId: masterId,
    }
    const result = await newsShareApi.getNewsShareList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getNewsId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, shares: rows } as News
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureNewsChildrenLoaded(record: News) {
  if (!hasNewsAttachmentRows(record)) {
    await loadNewsAttachmentForNews(record)
  }
  if (!hasNewsCommentRows(record)) {
    await loadNewsCommentForNews(record)
  }
  if (!hasNewsLikeRows(record)) {
    await loadNewsLikeForNews(record)
  }
  if (!hasNewsReadRows(record)) {
    await loadNewsReadForNews(record)
  }
  if (!hasNewsFavoriteRows(record)) {
    await loadNewsFavoriteForNews(record)
  }
  if (!hasNewsShareRows(record)) {
    await loadNewsShareForNews(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: News) {
  const key = getNewsId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureNewsChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'newsId',
    key: 'newsId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsId') ?? ''
  },
  {
    title: t('entity.news.code'),
    dataIndex: 'newsCode',
    key: 'newsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCode') ?? ''
  },
  {
    title: t('entity.news.category'),
    dataIndex: 'newsCategory',
    key: 'newsCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCategory') ?? ''
  },
  {
    title: t('entity.news.title'),
    dataIndex: 'newsTitle',
    key: 'newsTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsTitle') ?? ''
  },
  {
    title: t('entity.news.summary'),
    dataIndex: 'newsSummary',
    key: 'newsSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsSummary') ?? ''
  },
  {
    title: t('entity.news.tags'),
    dataIndex: 'tags',
    key: 'tags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'tags') ?? ''
  },
  {
    title: t('entity.news.content'),
    dataIndex: 'newsContent',
    key: 'newsContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsContent') ?? ''
  },
  {
    title: t('entity.news.coverimage'),
    dataIndex: 'newsCoverImage',
    key: 'newsCoverImage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCoverImage') ?? ''
  },
  {
    title: t('entity.news.istop'),
    dataIndex: 'isTop',
    key: 'isTop',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'isTop') ?? ''
  },
  {
    title: t('entity.news.isrecommended'),
    dataIndex: 'isRecommended',
    key: 'isRecommended',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'isRecommended') ?? ''
  },
  {
    title: t('entity.news.effectivetime'),
    dataIndex: 'effectiveTime',
    key: 'effectiveTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'effectiveTime') ?? ''
  },
  {
    title: t('entity.news.expiretime'),
    dataIndex: 'expireTime',
    key: 'expireTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'expireTime') ?? ''
  },
  {
    title: t('entity.news.readcount'),
    dataIndex: 'readCount',
    key: 'readCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'readCount') ?? ''
  },
  {
    title: t('entity.news.likecount'),
    dataIndex: 'likeCount',
    key: 'likeCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'likeCount') ?? ''
  },
  {
    title: t('entity.news.commentcount'),
    dataIndex: 'commentCount',
    key: 'commentCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'commentCount') ?? ''
  },
  {
    title: t('entity.news.favoritecount'),
    dataIndex: 'favoriteCount',
    key: 'favoriteCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'favoriteCount') ?? ''
  },
  {
    title: t('entity.news.sharecount'),
    dataIndex: 'shareCount',
    key: 'shareCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'shareCount') ?? ''
  },
  {
    title: t('entity.news.attachmentcount'),
    dataIndex: 'attachmentCount',
    key: 'attachmentCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'attachmentCount') ?? ''
  },
  {
    title: t('entity.news.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.news.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.news.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.news.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.news.publisherid'),
    dataIndex: 'publisherId',
    key: 'publisherId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'publisherId') ?? ''
  },
  {
    title: t('entity.news.publishername'),
    dataIndex: 'publisherName',
    key: 'publisherName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'publisherName') ?? ''
  },
  {
    title: t('entity.news.publishtime'),
    dataIndex: 'publishTime',
    key: 'publishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'publishTime') ?? ''
  },
  {
    title: t('entity.news.status'),
    dataIndex: 'newsStatus',
    key: 'newsStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:newscenter:news:update',
        onClick: (record: News) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:newscenter:news:delete',
        onClick: (record: News) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getNewsId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getNewsField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: News[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: News, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getNewsId(selectedRow.value) === getNewsId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: News[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: News) => ({
  onClick: () => {
    const key = getNewsId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getNewsId(item)))
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
    const kw = (queryKeyword.value ?? '').trim()
    const params: NewsQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getNewsList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[News] 加载数据失败', { error })
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
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  newsCode: '',
  newsCategory: undefined as number | undefined,
  newsTitle: '',
  newsSummary: '',
  tags: '',
  newsContent: '',
  newsCoverImage: '',
  isTop: undefined as number | undefined,
  isRecommended: undefined as number | undefined,
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  readCount: undefined as number | undefined,
  likeCount: undefined as number | undefined,
  commentCount: undefined as number | undefined,
  favoriteCount: undefined as number | undefined,
  shareCount: undefined as number | undefined,
  attachmentCount: undefined as number | undefined,
  flowInstanceId: '',
  deptId: '',
  deptName: '',
  publisherId: '',
  publisherName: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  sortOrder: undefined as number | undefined,
  newsStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.news._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: News) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.news._self') })
  formLoading.value = true
  try {
    const detail = await loadNewsDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.news._self') }))
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
      await updateNews(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.news._self') }))
    } else {
      await createNews(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.news._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getNewsTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importNews(file, sheetName)
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: NewsQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportNews(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.news._self') }))
  } catch (error: any) {
    logger.error('[News] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.news._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: News) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.news._self'), name: t('common.tip.this.target', { target: t('entity.news._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteNewsById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.news._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.news._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.news._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteNewsBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.news._self') }))
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  newsCode: '',
  newsCategory: undefined as number | undefined,
  newsTitle: '',
  newsSummary: '',
  tags: '',
  newsContent: '',
  newsCoverImage: '',
  isTop: undefined as number | undefined,
  isRecommended: undefined as number | undefined,
  effectiveTimeStart: '',
  effectiveTimeEnd: '',
  expireTimeStart: '',
  expireTimeEnd: '',
  readCount: undefined as number | undefined,
  likeCount: undefined as number | undefined,
  commentCount: undefined as number | undefined,
  favoriteCount: undefined as number | undefined,
  shareCount: undefined as number | undefined,
  attachmentCount: undefined as number | undefined,
  flowInstanceId: '',
  deptId: '',
  deptName: '',
  publisherId: '',
  publisherName: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  sortOrder: undefined as number | undefined,
  newsStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.routine-news-center-news {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
