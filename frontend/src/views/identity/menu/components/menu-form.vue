<template>
  <a-tabs v-model:active-key="activeTab">
    <a-tab-pane
      key="basic"
      :tab="t('common.page.form.tabs.basicinfo')"
    >
      <div :class="formContentClass">
        <a-form
          ref="formRef"
          :model="formState"
          :rules="rules"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 18 }"
          layout="horizontal"
          label-align="right"
        >
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.name')"
                name="menuName"
              >
                <a-input
                  v-model:value="formState.menuName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.name') })"
                  show-count
                  :maxlength="50"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.code')"
                name="menuCode"
              >
                <a-input
                  v-model:value="formState.menuCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.code') })"
                  show-count
                  :maxlength="200"
                  :disabled="!!formData?.menuId"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.parentid')"
                name="parentId"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="/api/TaktMenus/tree-options"
                  :placeholder="t('identity.menu.page.placeholder.parentmenuhint')"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.path')"
                name="menuPath"
              >
                <a-input
                  v-model:value="formState.menuPath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.path') })"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.routepath')"
                name="routePath"
              >
                <a-input
                  v-model:value="formState.routePath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.routepath') })"
                  show-count
                  :maxlength="200"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.component')"
                name="componentPath"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <a-input
                  v-model:value="formState.componentPath"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.component') })"
                  show-count
                  :maxlength="200"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.icon')"
                name="icon"
              >
                <a-input
                  v-model:value="formState.icon"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.icon') })"
                  show-count
                  :maxlength="100"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.ordernumhint')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.type')"
                name="menuType"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktSelect
                  v-model:value="formState.menuType"
                  dict-type="sys_menu_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.type') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.status')"
                name="menuStatus"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktSelect
                  v-model:value="formState.menuStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.status') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.menu.permission')"
                name="permission"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <a-input
                  v-model:value="formState.permission"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.permission') })"
                  show-count
                  :maxlength="100"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.isvisible')"
                name="isVisible"
              >
                <TaktSelect
                  v-model:value="formState.isVisible"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.isvisible') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.iscached')"
                name="isCached"
              >
                <TaktSelect
                  v-model:value="formState.isCached"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.iscached') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.isexternal')"
                name="isExternal"
              >
                <TaktSelect
                  v-model:value="formState.isExternal"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.menu.isexternal') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.linkurl')"
                name="externalUrl"
              >
                <a-input
                  v-model:value="formState.externalUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.linkurl') })"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.l10nkey')"
                name="i18nKey"
              >
                <a-input
                  v-model:value="formState.i18nKey"
                  :placeholder="t('identity.menu.page.placeholder.l10nhint')"
                  show-count
                  :maxlength="100"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.menu.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.menu.description') })"
                  :rows="2"
                  show-count
                  :maxlength="500"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </div>
    </a-tab-pane>
  </a-tabs>
</template>

<script setup lang="ts">
import { reactive, watch, ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { Menu, MenuCreate } from '@/types/identity/menu'
import TaktSelect from '@/components/business/takt-select/index.vue'

const { t } = useI18n()

interface Props {
  formData?: Partial<Menu>
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const activeTab = ref('basic')
/** 表单总字段数（用于内容区高度：>=30 为 10 行，<30 为 5 行） */
const TOTAL_FIELDS = 17
const formContentClass = computed(() => (TOTAL_FIELDS >= 30 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))

/** 表单状态（字段名与 Menu / MenuCreate 一致；parentId 兼容树选择组件 string | number） */
interface FormState extends Omit<MenuCreate, 'parentId' | 'remark'> {
  parentId: string | number
}

/**
 * 创建空表单状态
 * @returns {FormState} 默认值
 */
function createEmptyFormState(): FormState {
  return {
    menuName: '',
    menuCode: '',
    i18nKey: '',
    parentId: '0',
    menuPath: '',
    routePath: '',
    componentPath: '',
    icon: '',
    sortOrder: 0,
    menuType: 0,
    menuStatus: 1,
    permission: '',
    isVisible: 1,
    isCached: 1,
    isExternal: 0,
    externalUrl: '',
    isBuiltIn: 0,
    description: ''
  }
}

const formState = reactive<FormState>(createEmptyFormState())

const rules = computed<Record<string, Rule[]>>(() => ({
  menuName: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.menu.name') }), trigger: 'blur' }],
  menuCode: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.menu.code') }), trigger: 'blur' }],
  menuType: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.menu.type') }), trigger: 'change' }]
}))

watch(() => props.formData, (newData) => {
  if (newData && Object.keys(newData).length > 0) {
    Object.assign(formState, {
      menuName: newData.menuName ?? '',
      menuCode: newData.menuCode ?? '',
      i18nKey: newData.i18nKey ?? '',
      parentId: newData.parentId ?? '0',
      menuPath: newData.menuPath ?? '',
      routePath: newData.routePath ?? '',
      componentPath: newData.componentPath ?? '',
      icon: newData.icon ?? '',
      sortOrder: newData.sortOrder ?? 0,
      menuType: newData.menuType ?? 0,
      menuStatus: newData.menuStatus ?? 1,
      permission: newData.permission ?? '',
      isVisible: newData.isVisible ?? 1,
      isCached: newData.isCached ?? 1,
      isExternal: newData.isExternal ?? 0,
      externalUrl: newData.externalUrl ?? '',
      isBuiltIn: newData.isBuiltIn ?? 0,
      description: newData.description ?? ''
    })
  } else {
    Object.assign(formState, createEmptyFormState())
  }
  activeTab.value = 'basic'
}, { immediate: true, deep: true })

const validate = async () => {
  await formRef.value?.validate()
}

/**
 * 获取提交载荷
 * @returns {MenuCreate} 创建/更新 DTO
 */
const getValues = (): MenuCreate => {
  const p = formState.parentId
  const parentId = p === '' || p === undefined || p === null ? '0' : String(p)
  return {
    menuName: formState.menuName,
    menuCode: formState.menuCode,
    i18nKey: formState.i18nKey,
    parentId,
    menuPath: formState.menuPath,
    menuType: formState.menuType,
    permission: formState.permission,
    routePath: formState.routePath,
    componentPath: formState.componentPath,
    icon: formState.icon,
    sortOrder: formState.sortOrder,
    isExternal: formState.isExternal,
    externalUrl: formState.externalUrl,
    isCached: formState.isCached,
    isVisible: formState.isVisible,
    menuStatus: formState.menuStatus,
    isBuiltIn: formState.isBuiltIn,
    description: formState.description
  }
}

const resetFields = () => {
  formRef.value?.resetFields()
  Object.assign(formState, createEmptyFormState())
  activeTab.value = 'basic'
}

defineExpose({ validate, getValues, resetFields })
</script>
