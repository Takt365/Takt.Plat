<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/plant/components -->
<!-- 文件名称：plant-form.vue -->
<!-- 功能描述：Takt工厂实体 代表租户下的独立工厂维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="plant-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.plantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantName')"
                name="plantName"
              >
                <a-input
                  v-model:value="formState.plantName"
                  :placeholder="pi.ph('plantName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantShortName')"
                name="plantShortName"
              >
                <a-input
                  v-model:value="formState.plantShortName"
                  :placeholder="pi.ph('plantShortName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('codeAlias')"
                name="codeAlias"
              >
                <a-input
                  v-model:value="formState.codeAlias"
                  :placeholder="pi.ph('codeAlias')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defaultCulture')"
                name="defaultCulture"
              >
                <TaktSelect
                  v-model:value="formState.defaultCulture"
                  api-url="TaktCultures/options"
                  :placeholder="pi.ph('defaultCulture')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('enterpriseNature')"
                name="enterpriseNature"
              >
                <TaktSelect
                  v-model:value="formState.enterpriseNature"
                  dict-type="sys_enterprise_nature_type"
                  :placeholder="pi.ph('enterpriseNature')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industryAttribute')"
                name="industryAttribute"
              >
                <TaktSelect
                  v-model:value="formState.industryAttribute"
                  dict-type="sys_industry_attribute_type"
                  :placeholder="pi.ph('industryAttribute')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('enterpriseScale')"
                name="enterpriseScale"
              >
                <TaktSelect
                  v-model:value="formState.enterpriseScale"
                  dict-type="sys_enterprise_scale_type"
                  :placeholder="pi.ph('enterpriseScale')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessScope')"
                name="businessScope"
              >
                <a-textarea
                  v-model:value="formState.businessScope"
                  :placeholder="pi.ph('businessScope')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="pi.ph('registrationAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="pi.ph('registrationAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="pi.ph('registrationAddress3')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationRegion')"
                name="registrationRegion"
              >
                <a-input
                  v-model:value="formState.registrationRegion"
                  :placeholder="pi.ph('registrationRegion')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationProvince')"
                name="registrationProvince"
              >
                <a-input
                  v-model:value="formState.registrationProvince"
                  :placeholder="pi.ph('registrationProvince')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationCity')"
                name="registrationCity"
              >
                <a-input
                  v-model:value="formState.registrationCity"
                  :placeholder="pi.ph('registrationCity')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessRegion')"
                name="businessRegion"
              >
                <a-input
                  v-model:value="formState.businessRegion"
                  :placeholder="pi.ph('businessRegion')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessProvince')"
                name="businessProvince"
              >
                <a-input
                  v-model:value="formState.businessProvince"
                  :placeholder="pi.ph('businessProvince')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessCity')"
                name="businessCity"
              >
                <a-input
                  v-model:value="formState.businessCity"
                  :placeholder="pi.ph('businessCity')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessAddress1')"
                name="businessAddress1"
              >
                <a-textarea
                  v-model:value="formState.businessAddress1"
                  :placeholder="pi.ph('businessAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessAddress2')"
                name="businessAddress2"
              >
                <a-textarea
                  v-model:value="formState.businessAddress2"
                  :placeholder="pi.ph('businessAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessAddress3')"
                name="businessAddress3"
              >
                <a-textarea
                  v-model:value="formState.businessAddress3"
                  :placeholder="pi.ph('businessAddress3')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantAddress1')"
                name="plantAddress1"
              >
                <a-textarea
                  v-model:value="formState.plantAddress1"
                  :placeholder="pi.ph('plantAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantAddress2')"
                name="plantAddress2"
              >
                <a-textarea
                  v-model:value="formState.plantAddress2"
                  :placeholder="pi.ph('plantAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantAddress3')"
                name="plantAddress3"
              >
                <a-textarea
                  v-model:value="formState.plantAddress3"
                  :placeholder="pi.ph('plantAddress3')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantPhone')"
                name="plantPhone"
              >
                <a-input
                  v-model:value="formState.plantPhone"
                  :placeholder="pi.ph('plantPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantEmail')"
                name="plantEmail"
              >
                <a-input
                  v-model:value="formState.plantEmail"
                  :placeholder="pi.ph('plantEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantFax')"
                name="plantFax"
              >
                <a-input
                  v-model:value="formState.plantFax"
                  :placeholder="pi.ph('plantFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantWebsite')"
                name="plantWebsite"
              >
                <a-input
                  v-model:value="formState.plantWebsite"
                  :placeholder="pi.ph('plantWebsite')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unifiedSocialCreditCode')"
                name="unifiedSocialCreditCode"
              >
                <a-input
                  v-model:value="formState.unifiedSocialCreditCode"
                  :placeholder="pi.ph('unifiedSocialCreditCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.plantId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRegistrationNumber')"
                name="taxRegistrationNumber"
              >
                <a-input
                  v-model:value="formState.taxRegistrationNumber"
                  :placeholder="pi.ph('taxRegistrationNumber')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('legalRepresentative')"
                name="legalRepresentative"
              >
                <a-input
                  v-model:value="formState.legalRepresentative"
                  :placeholder="pi.ph('legalRepresentative')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantManager')"
                name="plantManager"
              >
                <a-input
                  v-model:value="formState.plantManager"
                  :placeholder="pi.ph('plantManager')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registeredCapital')"
                name="registeredCapital"
              >
                <a-input-number
                  v-model:value="formState.registeredCapital"
                  :placeholder="pi.ph('registeredCapital')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('establishmentDate')"
                name="establishmentDate"
              >
                <a-date-picker
                  v-model:value="formState.establishmentDate"
                  :placeholder="pi.ph('establishmentDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingDate')"
                name="closingDate"
              >
                <a-date-picker
                  v-model:value="formState.closingDate"
                  :placeholder="pi.ph('closingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantExistence')"
                name="plantExistence"
              >
                <TaktSelect
                  v-model:value="formState.plantExistence"
                  dict-type="sys_entity_existence_status"
                  :placeholder="pi.ph('plantExistence')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('relatedCompany')"
                name="relatedCompany"
              >
                <TaktSelect
                  v-model:value="formState.relatedCompany"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('relatedCompany')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantStatus')"
                name="plantStatus"
              >
                <TaktSelect
                  v-model:value="formState.plantStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('plantStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
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
 * Takt工厂实体 代表租户下的独立工厂维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/plant/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePlantI18n } from '../composables/use-plant-i18n'

/** 实体字段 i18n */
const pi = usePlantI18n()
import type { PlantCreate } from '@/types/logistics/materials/plant'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：租户级实体仅注入 tenantCode，表单只读
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PlantCreate & { plantId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  defaultCulture: "zh-CN",
  enterpriseNature: "150",
  industryAttribute: "C",
  enterpriseScale: "M",
  plantExistence: 1,
  plantStatus: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 plantId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.plantId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.plantId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'blur'
    }
  ],
  plantName: [
    {
      required: true,
      message: pi.ph('plantName'),
      trigger: 'blur'
    }
  ],
  plantShortName: [
    {
      required: true,
      message: pi.ph('plantShortName'),
      trigger: 'blur'
    }
  ],
  codeAlias: [
    {
      required: true,
      message: pi.ph('codeAlias'),
      trigger: 'blur'
    }
  ],
  defaultCulture: [
    {
      required: true,
      message: pi.ph('defaultCulture'),
      trigger: 'change'
    }
  ],
  enterpriseNature: [
    {
      required: true,
      message: pi.ph('enterpriseNature'),
      trigger: 'change'
    }
  ],
  industryAttribute: [
    {
      required: true,
      message: pi.ph('industryAttribute'),
      trigger: 'change'
    }
  ],
  enterpriseScale: [
    {
      required: true,
      message: pi.ph('enterpriseScale'),
      trigger: 'change'
    }
  ],
  businessScope: [
    {
      required: true,
      message: pi.ph('businessScope'),
      trigger: 'blur'
    }
  ],
  registrationAddress1: [
    {
      required: true,
      message: pi.ph('registrationAddress1'),
      trigger: 'blur'
    }
  ],
  registrationRegion: [
    {
      required: true,
      message: pi.ph('registrationRegion'),
      trigger: 'blur'
    }
  ],
  registrationProvince: [
    {
      required: true,
      message: pi.ph('registrationProvince'),
      trigger: 'blur'
    }
  ],
  registrationCity: [
    {
      required: true,
      message: pi.ph('registrationCity'),
      trigger: 'blur'
    }
  ],
  businessRegion: [
    {
      required: true,
      message: pi.ph('businessRegion'),
      trigger: 'blur'
    }
  ],
  businessProvince: [
    {
      required: true,
      message: pi.ph('businessProvince'),
      trigger: 'blur'
    }
  ],
  businessCity: [
    {
      required: true,
      message: pi.ph('businessCity'),
      trigger: 'blur'
    }
  ],
  businessAddress1: [
    {
      required: true,
      message: pi.ph('businessAddress1'),
      trigger: 'blur'
    }
  ],
  plantPhone: [
    {
      required: true,
      message: pi.ph('plantPhone'),
      trigger: 'blur'
    }
  ],
  plantEmail: [
    {
      required: true,
      message: pi.ph('plantEmail'),
      trigger: 'blur'
    }
  ],
  plantFax: [
    {
      required: true,
      message: pi.ph('plantFax'),
      trigger: 'blur'
    }
  ],
  plantWebsite: [
    {
      required: true,
      message: pi.ph('plantWebsite'),
      trigger: 'blur'
    }
  ],
  unifiedSocialCreditCode: [
    {
      required: true,
      message: pi.ph('unifiedSocialCreditCode'),
      trigger: 'blur'
    }
  ],
  taxRegistrationNumber: [
    {
      required: true,
      message: pi.ph('taxRegistrationNumber'),
      trigger: 'blur'
    }
  ],
  legalRepresentative: [
    {
      required: true,
      message: pi.ph('legalRepresentative'),
      trigger: 'blur'
    }
  ],
  plantManager: [
    {
      required: true,
      message: pi.ph('plantManager'),
      trigger: 'blur'
    }
  ],
  registeredCapital: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('registeredCapital'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('registeredCapital'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  establishmentDate: [
    {
      required: true,
      message: pi.ph('establishmentDate'),
      trigger: 'change'
    }
  ],
  plantExistence: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plantExistence'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plantExistence'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  relatedCompany: [
    {
      required: true,
      message: pi.ph('relatedCompany'),
      trigger: 'change'
    }
  ],
  plantStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plantStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plantStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('registeredCapital' in payload) {
    const rawregisteredCapital = payload.registeredCapital
    payload.registeredCapital = typeof rawregisteredCapital === 'number' ? rawregisteredCapital : Number(rawregisteredCapital)
  }
  if ('plantExistence' in payload) {
    const rawplantExistence = payload.plantExistence
    payload.plantExistence = typeof rawplantExistence === 'number' ? rawplantExistence : Number(rawplantExistence)
  }
  if ('plantStatus' in payload) {
    const rawplantStatus = payload.plantStatus
    payload.plantStatus = typeof rawplantStatus === 'number' ? rawplantStatus : Number(rawplantStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.plantId)

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
