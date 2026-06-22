<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：document-version-form.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制子表 documentVersion 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form document-version-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="document-version-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.versionno')"
                name="versionNo"
              >
                <a-input-number
                  v-model:value="formState.versionNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.versionno') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.documentversion.versionnote')"
                name="versionNote"
              >
                <a-textarea
                  v-model:value="formState.versionNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.documentversion.versionnote') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.fileid')"
                name="fileId"
              >
                <a-input
                  v-model:value="formState.fileId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.fileid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.filename')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filename') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.filepath')"
                name="filePath"
              >
                <a-input
                  v-model:value="formState.filePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filepath') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.filesize')"
                name="fileSize"
              >
                <a-input
                  v-model:value="formState.fileSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filesize') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.filetype')"
                name="fileType"
              >
                <a-input
                  v-model:value="formState.fileType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.filetype') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.documentversion.fileextension')"
                name="fileExtension"
              >
                <a-input
                  v-model:value="formState.fileExtension"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.documentversion.fileextension') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制子表 documentVersion 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/routine/document-center/document/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DocumentVersionCreate } from '@/types/routine/document-center/document-version'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["versionNo","versionNote","fileId","fileName","filePath","fileSize","fileType","fileExtension"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DocumentVersionCreate & { documentVersionId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 documentVersionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.documentVersionId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  versionNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.documentversion.versionno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.documentversion.versionno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  fileId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentversion.fileid') }),
      trigger: 'blur'
    }
  ],
  fileName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentversion.filename') }),
      trigger: 'blur'
    }
  ],
  filePath: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentversion.filepath') }),
      trigger: 'blur'
    }
  ],
  fileSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.documentversion.filesize') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 documentId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('versionNo' in payload) {
    const rawversionNo = payload.versionNo
    payload.versionNo = typeof rawversionNo === 'number' ? rawversionNo : Number(rawversionNo)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.documentId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
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
