<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：document-form.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="document-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.document.code')"
                name="documentCode"
              >
                <a-input
                  v-model:value="formState.documentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.category')"
                name="documentCategory"
              >
                <a-input-number
                  v-model:value="formState.documentCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.category') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.status')"
                name="documentStatus"
              >
                <a-input-number
                  v-model:value="formState.documentStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.confidentiallevel')"
                name="confidentialLevel"
              >
                <a-input-number
                  v-model:value="formState.confidentialLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.confidentiallevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.version')"
                name="version"
              >
                <a-input-number
                  v-model:value="formState.version"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.version') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.document.content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.content') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.summary')"
                name="summary"
              >
                <a-input
                  v-model:value="formState.summary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.summary') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.tags')"
                name="tags"
              >
                <a-input
                  v-model:value="formState.tags"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.tags') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.fileid')"
                name="fileId"
              >
                <a-input
                  v-model:value="formState.fileId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filepath')"
                name="filePath"
              >
                <a-input
                  v-model:value="formState.filePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filepath') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filesize')"
                name="fileSize"
              >
                <a-input
                  v-model:value="formState.fileSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filesize') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.filetype')"
                name="fileType"
              >
                <a-input
                  v-model:value="formState.fileType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.filetype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.fileextension')"
                name="fileExtension"
              >
                <a-input
                  v-model:value="formState.fileExtension"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.fileextension') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.effectivetime')"
                name="effectiveTime"
              >
                <a-input
                  v-model:value="formState.effectiveTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.effectivetime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.expiretime')"
                name="expireTime"
              >
                <a-input
                  v-model:value="formState.expireTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.expiretime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publisherid')"
                name="publisherId"
              >
                <a-input
                  v-model:value="formState.publisherId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publisherid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.publishername')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.publishername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.istop')"
                name="isTop"
              >
                <a-input-number
                  v-model:value="formState.isTop"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.istop') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.viewcount')"
                name="viewCount"
              >
                <a-input-number
                  v-model:value="formState.viewCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.viewcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.downloadcount')"
                name="downloadCount"
              >
                <a-input-number
                  v-model:value="formState.downloadCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.downloadcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.document.targetscope')"
                name="targetScope"
              >
                <a-textarea
                  v-model:value="formState.targetScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.document.targetscope') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.targetdepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetdepartments') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.document.targetusers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.document.targetusers') })"
                  size="small"
                  allow-clear
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
      <!-- 子表：documentVersion -->
      <a-tab-pane
        key="child-versions"
        :tab="t('entity.documentVersion._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddDocumentVersionRow">
            {{ t('common.page.button.create') }}{{ t('entity.documentVersion._self') }}
          </a-button>
        </div>
        <a-table
          :columns="documentVersionFormColumns"
          :data-source="childDocumentVersionRows"
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
            <template v-else-if="column.key === 'versionNo'">
              <a-input-number
                v-model:value="record.versionNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.versionno') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'versionNote'">
              <a-textarea
                v-model:value="record.versionNote"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.documentVersion.versionnote') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'fileId'">
              <a-input
                v-model:value="record.fileId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.fileid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileName'">
              <a-input
                v-model:value="record.fileName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filename') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'filePath'">
              <a-input
                v-model:value="record.filePath"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filepath') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileSize'">
              <a-input
                v-model:value="record.fileSize"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filesize') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileType'">
              <a-input
                v-model:value="record.fileType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.filetype') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'fileExtension'">
              <a-input
                v-model:value="record.fileExtension"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentVersion.fileextension') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveDocumentVersionRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：documentChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.documentChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddDocumentChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.documentChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="documentChangeLogFormColumns"
          :data-source="childDocumentChangeLogRows"
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
            <template v-else-if="column.key === 'documentTitle'">
              <a-input
                v-model:value="record.documentTitle"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.documenttitle') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeSummary'">
              <a-input
                v-model:value="record.changeSummary"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.changesummary') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'versionAtChange'">
              <a-input-number
                v-model:value="record.versionAtChange"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentChangeLog.versionatchange') })"
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
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveDocumentChangeLogRow(index)">
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
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/document-center/document/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DocumentCreate, DocumentVersionCreate, DocumentVersion, DocumentChangeLogCreate, DocumentChangeLog } from '@/types/routine/document-center/document'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","documentCode","title","documentCategory","documentStatus","confidentialLevel","version","content","summary","tags","fileId","fileName","filePath","fileSize","fileType","fileExtension","effectiveTime","expireTime","publishTime","publisherId","publisherName","deptId","deptName","isTop","sortOrder","viewCount","downloadCount","targetScope","targetDepartments","targetUsers","extFieldJson","remark"]

/** documentVersion 子表行（表单 Tab 内嵌） */
const childDocumentVersionRows = ref<Record<string, unknown>[]>([])
/** documentChangeLog 子表行（表单 Tab 内嵌） */
const childDocumentChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 documentVersion 表单列定义 */
const documentVersionFormColumns = computed(() => [
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
    title: t('entity.documentVersion.versionno'),
    dataIndex: 'versionNo',
    key: 'versionNo',
    width: 140,
  },
  {
    title: t('entity.documentVersion.versionnote'),
    dataIndex: 'versionNote',
    key: 'versionNote',
    width: 140,
  },
  {
    title: t('entity.documentVersion.fileid'),
    dataIndex: 'fileId',
    key: 'fileId',
    width: 140,
  },
  {
    title: t('entity.documentVersion.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 140,
  },
  {
    title: t('entity.documentVersion.filepath'),
    dataIndex: 'filePath',
    key: 'filePath',
    width: 140,
  },
  {
    title: t('entity.documentVersion.filesize'),
    dataIndex: 'fileSize',
    key: 'fileSize',
    width: 140,
  },
  {
    title: t('entity.documentVersion.filetype'),
    dataIndex: 'fileType',
    key: 'fileType',
    width: 140,
  },
  {
    title: t('entity.documentVersion.fileextension'),
    dataIndex: 'fileExtension',
    key: 'fileExtension',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 documentChangeLog 表单列定义 */
const documentChangeLogFormColumns = computed(() => [
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
    title: t('entity.documentChangeLog.documenttitle'),
    dataIndex: 'documentTitle',
    key: 'documentTitle',
    width: 140,
  },
  {
    title: t('entity.documentChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.documentChangeLog.changesummary'),
    dataIndex: 'changeSummary',
    key: 'changeSummary',
    width: 140,
  },
  {
    title: t('entity.documentChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.documentChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('entity.documentChangeLog.versionatchange'),
    dataIndex: 'versionAtChange',
    key: 'versionAtChange',
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
function syncChildRowsFromFormData(val: Partial<DocumentCreate & { documentId?: string }> | null | undefined) {
  childDocumentVersionRows.value = ((val as any)?.versions ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.documentVersionId ?? `new-${index}`,
  }))
  childDocumentChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.documentChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 documentVersion 行 */
function handleAddDocumentVersionRow() {
  childDocumentVersionRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      versionNo: 0,
      versionNote: '',
      fileId: '',
      fileName: '',
      filePath: '',
      fileSize: '',
      fileType: '',
      fileExtension: '',
  })
}

/** 表单 Tab 内删除 documentVersion 行 */
function handleRemoveDocumentVersionRow(index: number) {
  childDocumentVersionRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 documentChangeLog 行 */
function handleAddDocumentChangeLogRow() {
  childDocumentChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      documentTitle: '',
      changeType: 0,
      changeSummary: '',
      changeFields: '',
      changeReason: '',
      versionAtChange: 0,
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 documentChangeLog 行 */
function handleRemoveDocumentChangeLogRow(index: number) {
  childDocumentChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    versions: childDocumentVersionRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childDocumentChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DocumentCreate & { documentId?: string }> | null
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
    delete (next as any).versions
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.documentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  documentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.code') }),
      trigger: 'blur'
    }
  ],
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.title') }),
      trigger: 'blur'
    }
  ],
  documentCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.category') }),
      trigger: 'change'
    }
  ],
  documentStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.status') }),
      trigger: 'change'
    }
  ],
  confidentialLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.confidentiallevel') }),
      trigger: 'change'
    }
  ],
  version: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.version') }),
      trigger: 'change'
    }
  ],
  fileSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.filesize') }),
      trigger: 'blur'
    }
  ],
  publisherId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.publisherid') }),
      trigger: 'blur'
    }
  ],
  publisherName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.publishername') }),
      trigger: 'blur'
    }
  ],
  isTop: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.istop') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.sortorder') }),
      trigger: 'change'
    }
  ],
  viewCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.viewcount') }),
      trigger: 'change'
    }
  ],
  downloadCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.document.downloadcount') }),
      trigger: 'change'
    }
  ],
  targetScope: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.document.targetscope') }),
      trigger: 'blur'
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
  childDocumentVersionRows.value = []
  childDocumentChangeLogRows.value = []
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
