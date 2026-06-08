<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/foundation/i18n/components -->
<!-- 文件名称：culture-form.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：区域文化表单组件，包含主表和子表（翻译） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="culture-form">
    <a-tabs v-model:active-key="activeTab">
      <!-- 主表：区域文化信息 -->
      <a-tab-pane
        key="main"
        :tab="t('routine.localization.language.tabs.main')"
      >
        <a-form
          ref="mainFormRef"
          :model="mainFormState"
          :rules="mainFormRules"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 20 }"
          layout="horizontal"
        >
          <a-form-item
            :label="t('entity.language.name')"
            name="languageName"
          >
            <a-input
              v-model:value="mainFormState.languageName"
              :placeholder="t('routine.localization.language.placeholders.languageName')"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.culturecode')"
            name="cultureCode"
          >
            <a-input
              v-model:value="mainFormState.cultureCode"
              :placeholder="t('routine.localization.language.placeholders.cultureCode')"
              :disabled="!!props.formData?.cultureId"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.nativename')"
            name="nativeName"
          >
            <a-input
              v-model:value="mainFormState.nativeName"
              :placeholder="t('routine.localization.language.placeholders.nativeName')"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.icon')"
            name="icon"
          >
            <a-input
              v-model:value="mainFormState.icon"
              :placeholder="t('routine.localization.language.placeholders.languageIcon')"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.sortOrder')"
            name="sortOrder"
          >
            <a-input-number
              v-model:value="mainFormState.sortOrder"
              :min="0"
              :placeholder="t('routine.localization.language.placeholders.sortOrder')"
              style="width: 100%"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.status')"
            name="languageStatus"
          >
            <TaktSelect
              v-model:value="mainFormState.languageStatus"
              dict-type="sys_status"
              allow-clear
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.language.status') })"
            />
          </a-form-item>
          <a-form-item
            :label="t('entity.language.isdefault')"
            name="isDefault"
          >
            <TaktSelect
              v-model:value="mainFormState.isDefault"
              dict-type="sys_yes_no"
              allow-clear
              :placeholder="t('common.page.form.placeholder.select', { field: t('entity.language.isdefault') })"
            />
          </a-form-item>
          <a-form-item
            :label="t('common.page.entity.remark')"
            name="remark"
          >
            <a-textarea
              v-model:value="mainFormState.remark"
              :placeholder="t('routine.localization.language.placeholders.remark')"
              :rows="3"
            />
          </a-form-item>
        </a-form>
      </a-tab-pane>
      <!-- 子表：翻译列表 -->
      <a-tab-pane
        key="translation"
        :tab="t('routine.localization.language.tabs.translation')"
      >
        <div class="translation-toolbar">
          <a-button
            type="primary"
            @click="handleAddTranslation"
          >
            <template #icon>
              <PlusOutlined />
            </template>
            {{ t('routine.localization.language.typeForm.addTranslation') }}
          </a-button>
        </div>
        <a-table
          :columns="translationColumns"
          :data-source="translationList"
          :pagination="false"
          row-key="translationId"
          size="small"
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'i18nKey'">
              <a-input
                v-model:value="record.i18nKey"
                :placeholder="t('routine.localization.language.placeholders.translationResourceKey')"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'cultureCode'">
              <a-input
                v-model:value="record.cultureCode"
                :placeholder="t('routine.localization.language.placeholders.translationCultureCode')"
                size="small"
                :disabled="true"
              />
            </template>
            <template v-else-if="column.key === 'translationText'">
              <a-input
                v-model:value="record.translationText"
                :placeholder="t('routine.localization.language.placeholders.translationValue')"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'resourceType'">
              <a-select
                v-model:value="record.resourceType"
                :placeholder="t('routine.localization.translation.placeholders.resourceTypeSelect')"
                size="small"
                style="width: 100%"
              >
                <a-select-option :value="0">
                  {{ t('routine.localization.translation.options.frontend') }}
                </a-select-option>
                <a-select-option :value="1">
                  {{ t('routine.localization.translation.options.backend') }}
                </a-select-option>
              </a-select>
            </template>
            <template v-else-if="column.key === 'resourceGroup'">
              <a-input-number
                v-model:value="record.resourceGroup"
                :min="0"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'action'">
              <a-button
                type="link"
                danger
                size="small"
                @click="handleRemoveTranslation(index)"
              >
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, watch, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { PlusOutlined } from '@ant-design/icons-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { TableColumnsType } from 'ant-design-vue'
import type { Culture, CultureCreate, CultureUpdate } from '@/types/foundation/culture'
import type { TranslationCreate } from '@/types/foundation/translation'

/** 子表行：仅编辑所需字段，不要求完整 Translation/TaktCompanyEntityBase */
export interface CultureTranslationInlineRow {
  translationId: string
  i18nKey: string
  cultureId: string
  cultureCode: string
  translationText: string
  resourceType: number
  resourceGroup: number
  remark?: string
}

type MainFormState = Omit<CultureCreate, 'icon' | 'remark'> & {
  cultureId?: string
  icon: string
  remark: string
}

interface Props {
  /** 编辑模式下的区域文化数据 */
  formData?: Culture | null
  /** 提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

const { t } = useI18n()

/** 当前 Tab（main / translation） */
const activeTab = ref('main')
/** 主表表单 ref */
const mainFormRef = ref()
/** 内联翻译子表行 */
const translationList = ref<CultureTranslationInlineRow[]>([])

/** 主表表单状态（与 Culture 接口字段顺序一致） */
const mainFormState = reactive<MainFormState>({
  languageName: '',
  cultureCode: '',
  nativeName: '',
  icon: '',
  sortOrder: 0,
  languageStatus: 0,
  isDefault: 1,
  remark: ''
})

/** 主表校验规则 */
const mainFormRules = computed<Record<string, Rule[]>>(() => ({
  cultureCode: [
    { required: true, message: t('routine.localization.language.rules.cultureCodeRequired'), trigger: 'blur' }
  ],
  languageName: [
    { required: true, message: t('routine.localization.language.rules.languageNameRequired'), trigger: 'blur' }
  ],
  nativeName: [
    { required: true, message: t('routine.localization.language.rules.nativeNameRequired'), trigger: 'blur' }
  ]
}))

/** 翻译子表列（与 Translation 字段对齐） */
const translationColumns = computed<TableColumnsType<CultureTranslationInlineRow>>(() => [
  {
    title: t('entity.translation.resourcekey'),
    dataIndex: 'i18nKey',
    key: 'i18nKey',
    width: 200
  },
  {
    title: t('entity.translation.culturecode'),
    dataIndex: 'cultureCode',
    key: 'cultureCode',
    width: 150
  },
  {
    title: t('entity.translation.translationvalue'),
    dataIndex: 'translationText',
    key: 'translationText',
    width: 300
  },
  {
    title: t('entity.translation.resourcetype'),
    dataIndex: 'resourceType',
    key: 'resourceType',
    width: 120
  },
  {
    title: t('entity.translation.resourcegroup'),
    dataIndex: 'resourceGroup',
    key: 'resourceGroup',
    width: 150,
    ellipsis: true
  },
  {
    title: t('common.page.action.operation'),
    key: 'action',
    width: 80,
    fixed: 'right'
  }
])

/**
 * 解析资源类别（兼容历史字符串 Frontend/Backend）
 * @param value 原始值
 * @returns {number} 0=前端，1=后端
 */
function parseInlineResourceType(value: unknown): number {
  if (value === 1 || value === '1' || value === 'Backend') return 1
  return 0
}

/**
 * 解析资源分组为数字
 * @param value 原始值
 * @returns {number} 分组编号
 */
function parseInlineResourceGroup(value: unknown): number {
  const n = Number(value)
  return Number.isFinite(n) ? n : 0
}

/**
 * 将 API/历史翻译行映射为内联编辑行
 * @param item 原始翻译对象
 * @returns {CultureTranslationInlineRow} 内联行
 */
function mapToInlineTranslationRow(item: unknown): CultureTranslationInlineRow {
  const o = item as Record<string, unknown>
  const row: CultureTranslationInlineRow = {
    translationId: String(o.translationId ?? ''),
    i18nKey: String(o.i18nKey ?? o.resourceKey ?? ''),
    cultureId: String(o.cultureId ?? o.languageId ?? ''),
    cultureCode: String(o.cultureCode ?? ''),
    translationText: String(o.translationText ?? o.translationValue ?? ''),
    resourceType: parseInlineResourceType(o.resourceType),
    resourceGroup: parseInlineResourceGroup(o.resourceGroup)
  }
  if (o.remark != null && o.remark !== '') {
    row.remark = String(o.remark)
  }
  return row
}

watch(
  () => props.formData,
  (newData) => {
    if (newData) {
      Object.assign(mainFormState, {
        cultureId: newData.cultureId,
        languageName: newData.languageName || '',
        cultureCode: newData.cultureCode || '',
        nativeName: newData.nativeName || '',
        icon: newData.icon || '',
        sortOrder: newData.sortOrder ?? 0,
        languageStatus: newData.languageStatus ?? 0,
        isDefault: newData.isDefault ?? 1,
        remark: newData.remark || ''
      })
      translationList.value = Array.isArray(newData.translationList)
        ? newData.translationList.map(mapToInlineTranslationRow)
        : []
    } else {
      Object.assign(mainFormState, {
        cultureId: undefined,
        languageName: '',
        cultureCode: '',
        nativeName: '',
        icon: '',
        sortOrder: 0,
        languageStatus: 0,
        isDefault: 1,
        remark: ''
      })
      translationList.value = []
    }
  },
  { immediate: true, deep: true }
)

/**
 * 新增内联翻译行
 * @returns {void}
 */
function handleAddTranslation() {
  translationList.value.push({
    translationId: `temp_${Date.now()}_${Math.random()}`,
    i18nKey: '',
    cultureId: mainFormState.cultureId || '',
    cultureCode: mainFormState.cultureCode || '',
    translationText: '',
    resourceType: 0,
    resourceGroup: 0
  })
}

/**
 * 删除内联翻译行
 * @param index 行索引
 * @returns {void}
 */
function handleRemoveTranslation(index: number) {
  translationList.value.splice(index, 1)
}

/**
 * 校验主表与子表
 * @returns {Promise<void>}
 */
async function validate() {
  await mainFormRef.value?.validate()
  for (let i = 0; i < translationList.value.length; i++) {
    const item = translationList.value[i]
    if (!item) continue
    if (!item.i18nKey || !item.translationText) {
      throw new Error(t('routine.localization.language.messages.translationRowInvalid', { row: i + 1 }))
    }
  }
}

/**
 * 获取提交 DTO（CultureCreate / CultureUpdate）
 * @returns {CultureCreate | CultureUpdate} 区域文化 DTO
 */
function getFormData(): CultureCreate | CultureUpdate {
  const translationRows: TranslationCreate[] = translationList.value
    .filter((item) => item.i18nKey && item.translationText)
    .map((item) => {
      const row: TranslationCreate = {
        i18nKey: item.i18nKey,
        cultureId: item.cultureId || mainFormState.cultureId || '',
        cultureCode: mainFormState.cultureCode,
        translationText: item.translationText,
        resourceType: item.resourceType,
        resourceGroup: item.resourceGroup
      }
      if (item.remark && item.remark.trim() !== '') {
        row.remark = item.remark
      }
      return row
    })
  const base: CultureCreate = {
    languageName: mainFormState.languageName,
    cultureCode: mainFormState.cultureCode,
    nativeName: mainFormState.nativeName,
    icon: mainFormState.icon || undefined,
    sortOrder: mainFormState.sortOrder,
    languageStatus: mainFormState.languageStatus,
    isDefault: mainFormState.isDefault,
    remark: mainFormState.remark || undefined,
    translationList: translationRows.length > 0 ? translationRows : undefined
  }
  if (mainFormState.cultureId) {
    return { ...base, cultureId: mainFormState.cultureId }
  }
  return base
}

defineExpose({
  validate,
  getFormData
})
</script>

<style scoped lang="css">
.culture-form {
  .translation-toolbar {
    margin-bottom: 16px;
  }
}
</style>
