<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news/components -->
<!-- 文件名称：news-form.vue -->
<!-- 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="news-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.code')"
                name="newsCode"
              >
                <a-input
                  v-model:value="formState.newsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.category')"
                name="newsCategory"
              >
                <a-input-number
                  v-model:value="formState.newsCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.category') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.title')"
                name="newsTitle"
              >
                <a-input
                  v-model:value="formState.newsTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.summary')"
                name="newsSummary"
              >
                <a-input
                  v-model:value="formState.newsSummary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.summary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.tags') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.news.content')"
                name="newsContent"
              >
                <a-textarea
                  v-model:value="formState.newsContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.news.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.coverimage')"
                name="newsCoverImage"
              >
                <a-input
                  v-model:value="formState.newsCoverImage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.coverimage') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.istop')"
                name="isTop"
              >
                <a-input-number
                  v-model:value="formState.isTop"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.istop') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.isrecommended')"
                name="isRecommended"
              >
                <a-input-number
                  v-model:value="formState.isRecommended"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.isrecommended') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.effectivetime')"
                name="effectiveTime"
              >
                <a-input
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.effectivetime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.expiretime')"
                name="expireTime"
              >
                <a-input
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.expiretime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.readcount')"
                name="readCount"
              >
                <a-input-number
                  v-model:value="formState.readCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.readcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.likecount')"
                name="likeCount"
              >
                <a-input-number
                  v-model:value="formState.likeCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.likecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.commentcount')"
                name="commentCount"
              >
                <a-input-number
                  v-model:value="formState.commentCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.commentcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.favoritecount')"
                name="favoriteCount"
              >
                <a-input-number
                  v-model:value="formState.favoriteCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.favoritecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.sharecount')"
                name="shareCount"
              >
                <a-input-number
                  v-model:value="formState.shareCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sharecount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.attachmentcount')"
                name="attachmentCount"
              >
                <a-input-number
                  v-model:value="formState.attachmentCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.attachmentcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publisherid')"
                name="publisherId"
              >
                <a-input
                  v-model:value="formState.publisherId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publishername')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.news.status')"
                name="newsStatus"
              >
                <a-input-number
                  v-model:value="formState.newsStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.news.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <!-- 子表：newsAttachment -->
      <a-tab-pane
        key="child-attachments"
        :tab="t('entity.newsAttachment._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsAttachmentRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsAttachment._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsAttachmentFormColumns"
          :data-source="childNewsAttachmentRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'fileId'">
              <a-input
                v-model:value="record.fileId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.fileid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileName'">
              <a-input
                v-model:value="record.fileName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.filename') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'filePath'">
              <a-input
                v-model:value="record.filePath"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.filepath') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileSize'">
              <a-input
                v-model:value="record.fileSize"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.filesize') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileType'">
              <a-input
                v-model:value="record.fileType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.filetype') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileExtension'">
              <a-input
                v-model:value="record.fileExtension"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.fileextension') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'sortOrder'">
              <a-input-number
                v-model:value="record.sortOrder"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsAttachment.sortorder') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsAttachmentRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：newsComment -->
      <a-tab-pane
        key="child-comments"
        :tab="t('entity.newsComment._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsCommentRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsComment._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsCommentFormColumns"
          :data-source="childNewsCommentRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'parentId'">
              <a-input
                v-model:value="record.parentId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.parentid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userAvatar'">
              <a-input
                v-model:value="record.userAvatar"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.useravatar') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'replyToUserId'">
              <a-input
                v-model:value="record.replyToUserId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.replytouserid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'replyToUserName'">
              <a-input
                v-model:value="record.replyToUserName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.replytousername') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'commentContent'">
              <a-textarea
                v-model:value="record.commentContent"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.newsComment.commentcontent') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'commentTime'">
              <a-input
                v-model:value="record.commentTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsComment.commenttime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsCommentRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：newsLike -->
      <a-tab-pane
        key="child-likes"
        :tab="t('entity.newsLike._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsLikeRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsLike._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsLikeFormColumns"
          :data-source="childNewsLikeRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsLike.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsLike.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'likeTime'">
              <a-input
                v-model:value="record.likeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsLike.liketime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsLikeRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：newsRead -->
      <a-tab-pane
        key="child-reads"
        :tab="t('entity.newsRead._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsReadRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsRead._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsReadFormColumns"
          :data-source="childNewsReadRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsRead.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsRead.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'readTime'">
              <a-input
                v-model:value="record.readTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsRead.readtime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsReadRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：newsFavorite -->
      <a-tab-pane
        key="child-favorites"
        :tab="t('entity.newsFavorite._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsFavoriteRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsFavorite._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsFavoriteFormColumns"
          :data-source="childNewsFavoriteRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsFavorite.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsFavorite.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'favoriteTime'">
              <a-input
                v-model:value="record.favoriteTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsFavorite.favoritetime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsFavoriteRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：newsShare -->
      <a-tab-pane
        key="child-shares"
        :tab="t('entity.newsShare._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddNewsShareRow">
            {{ t('common.page.button.create') }}{{ t('entity.newsShare._self') }}
          </a-button>
        </div>
        <a-table
          :columns="newsShareFormColumns"
          :data-source="childNewsShareRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'userId'">
              <a-input
                v-model:value="record.userId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsShare.userid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'userName'">
              <a-input
                v-model:value="record.userName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsShare.username') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'shareChannel'">
              <a-input
                v-model:value="record.shareChannel"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsShare.sharechannel') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'shareTime'">
              <a-input
                v-model:value="record.shareTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.newsShare.sharetime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveNewsShareRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/news-center/news/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NewsCreate, NewsAttachmentCreate, NewsAttachment, NewsCommentCreate, NewsComment, NewsLikeCreate, NewsLike, NewsReadCreate, NewsRead, NewsFavoriteCreate, NewsFavorite, NewsShareCreate, NewsShare } from '@/types/routine/news-center/news'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","newsCode","newsCategory","newsTitle","newsSummary","tags","newsContent","newsCoverImage","isTop","isRecommended","effectiveTime","expireTime","readCount","likeCount","commentCount","favoriteCount","shareCount","attachmentCount","flowInstanceId","deptId","deptName","publisherId","publisherName","publishTime","sortOrder","newsStatus","extFieldJson","remark"]

/** newsAttachment 子表行（表单 Tab 内嵌） */
const childNewsAttachmentRows = ref<Record<string, unknown>[]>([])
/** newsComment 子表行（表单 Tab 内嵌） */
const childNewsCommentRows = ref<Record<string, unknown>[]>([])
/** newsLike 子表行（表单 Tab 内嵌） */
const childNewsLikeRows = ref<Record<string, unknown>[]>([])
/** newsRead 子表行（表单 Tab 内嵌） */
const childNewsReadRows = ref<Record<string, unknown>[]>([])
/** newsFavorite 子表行（表单 Tab 内嵌） */
const childNewsFavoriteRows = ref<Record<string, unknown>[]>([])
/** newsShare 子表行（表单 Tab 内嵌） */
const childNewsShareRows = ref<Record<string, unknown>[]>([])

/** 子表 newsAttachment 表单列定义 */
const newsAttachmentFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.fileextension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    width: 140,
  },
  {
    title: t('entity.newsAttachment.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 newsComment 表单列定义 */
const newsCommentFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsComment.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 140,
  },
  {
    title: t('entity.newsComment.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.newsComment.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.newsComment.useravatar'),
    dataIndex: 'userAvatar',
    key: 'userAvatar',
    width: 140,
  },
  {
    title: t('entity.newsComment.replytouserid'),
    dataIndex: 'replyToUserId',
    key: 'replyToUserId',
    width: 140,
  },
  {
    title: t('entity.newsComment.replytousername'),
    dataIndex: 'replyToUserName',
    key: 'replyToUserName',
    width: 140,
  },
  {
    title: t('entity.newsComment.commentcontent'),
    dataIndex: 'commentContent',
    key: 'commentContent',
    width: 140,
  },
  {
    title: t('entity.newsComment.commenttime'),
    dataIndex: 'commentTime',
    key: 'commentTime',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 newsLike 表单列定义 */
const newsLikeFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsLike.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.newsLike.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.newsLike.liketime'),
    dataIndex: 'likeTime',
    key: 'likeTime',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 newsRead 表单列定义 */
const newsReadFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsRead.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.newsRead.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.newsRead.readtime'),
    dataIndex: 'readTime',
    key: 'readTime',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 newsFavorite 表单列定义 */
const newsFavoriteFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsFavorite.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.newsFavorite.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.newsFavorite.favoritetime'),
    dataIndex: 'favoriteTime',
    key: 'favoriteTime',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 newsShare 表单列定义 */
const newsShareFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.newsShare.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 140,
  },
  {
    title: t('entity.newsShare.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 140,
  },
  {
    title: t('entity.newsShare.sharechannel'),
    dataIndex: 'shareChannel',
    key: 'shareChannel',
    width: 140,
  },
  {
    title: t('entity.newsShare.sharetime'),
    dataIndex: 'shareTime',
    key: 'shareTime',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<NewsCreate & { newsId?: string }> | null | undefined) {
  childNewsAttachmentRows.value = ((val as any)?.attachments ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsAttachmentId ?? `new-${index}`,
  }))
  childNewsCommentRows.value = ((val as any)?.comments ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsCommentId ?? `new-${index}`,
  }))
  childNewsLikeRows.value = ((val as any)?.likes ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsLikeId ?? `new-${index}`,
  }))
  childNewsReadRows.value = ((val as any)?.reads ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsReadId ?? `new-${index}`,
  }))
  childNewsFavoriteRows.value = ((val as any)?.favorites ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsFavoriteId ?? `new-${index}`,
  }))
  childNewsShareRows.value = ((val as any)?.shares ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.newsShareId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 newsAttachment 行 */
function handleAddNewsAttachmentRow() {
  childNewsAttachmentRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      fileId: '',
      fileName: '',
      filePath: '',
      fileSize: '',
      fileType: '',
      fileExtension: '',
      sortOrder: 0,
      extFieldJson: '',
  })
}

/** 表单 Tab 内删除 newsAttachment 行 */
function handleRemoveNewsAttachmentRow(index: number) {
  childNewsAttachmentRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 newsComment 行 */
function handleAddNewsCommentRow() {
  childNewsCommentRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      parentId: '',
      userId: '',
      userName: '',
      userAvatar: '',
      replyToUserId: '',
      replyToUserName: '',
      commentContent: '',
      commentTime: '',
  })
}

/** 表单 Tab 内删除 newsComment 行 */
function handleRemoveNewsCommentRow(index: number) {
  childNewsCommentRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 newsLike 行 */
function handleAddNewsLikeRow() {
  childNewsLikeRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      userId: '',
      userName: '',
      likeTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 newsLike 行 */
function handleRemoveNewsLikeRow(index: number) {
  childNewsLikeRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 newsRead 行 */
function handleAddNewsReadRow() {
  childNewsReadRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      userId: '',
      userName: '',
      readTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 newsRead 行 */
function handleRemoveNewsReadRow(index: number) {
  childNewsReadRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 newsFavorite 行 */
function handleAddNewsFavoriteRow() {
  childNewsFavoriteRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      userId: '',
      userName: '',
      favoriteTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 newsFavorite 行 */
function handleRemoveNewsFavoriteRow(index: number) {
  childNewsFavoriteRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 newsShare 行 */
function handleAddNewsShareRow() {
  childNewsShareRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      userId: '',
      userName: '',
      shareChannel: '',
      shareTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 newsShare 行 */
function handleRemoveNewsShareRow(index: number) {
  childNewsShareRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    attachments: childNewsAttachmentRows.value.map(({ __rowKey, ...rest }) => rest),
    comments: childNewsCommentRows.value.map(({ __rowKey, ...rest }) => rest),
    likes: childNewsLikeRows.value.map(({ __rowKey, ...rest }) => rest),
    reads: childNewsReadRows.value.map(({ __rowKey, ...rest }) => rest),
    favorites: childNewsFavoriteRows.value.map(({ __rowKey, ...rest }) => rest),
    shares: childNewsShareRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<NewsCreate & { newsId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).attachments
    delete (next as any).comments
    delete (next as any).likes
    delete (next as any).reads
    delete (next as any).favorites
    delete (next as any).shares
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.newsId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  newsCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.code') }),
      trigger: 'blur'
    }
  ],
  newsCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.category') }),
      trigger: 'change'
    }
  ],
  newsTitle: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.title') }),
      trigger: 'blur'
    }
  ],
  newsContent: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.content') }),
      trigger: 'blur'
    }
  ],
  isTop: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.istop') }),
      trigger: 'change'
    }
  ],
  isRecommended: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.isrecommended') }),
      trigger: 'change'
    }
  ],
  readCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.readcount') }),
      trigger: 'change'
    }
  ],
  likeCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.likecount') }),
      trigger: 'change'
    }
  ],
  commentCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.commentcount') }),
      trigger: 'change'
    }
  ],
  favoriteCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.favoritecount') }),
      trigger: 'change'
    }
  ],
  shareCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.sharecount') }),
      trigger: 'change'
    }
  ],
  attachmentCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.attachmentcount') }),
      trigger: 'change'
    }
  ],
  publisherId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.publisherid') }),
      trigger: 'blur'
    }
  ],
  publisherName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.news.publishername') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.sortorder') }),
      trigger: 'change'
    }
  ],
  newsStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.news.status') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childNewsAttachmentRows.value = []
  childNewsCommentRows.value = []
  childNewsLikeRows.value = []
  childNewsReadRows.value = []
  childNewsFavoriteRows.value = []
  childNewsShareRows.value = []
  activeTab.value = 'tab-0'
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
