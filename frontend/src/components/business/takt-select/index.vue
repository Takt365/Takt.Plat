<!-- ======================================== -->
<!-- 项目名称：Takt.Plat -->
<!-- 命名空间：@/components/business/takt-select -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 下拉选择框组件，支持 API 动态加载、字典数据、业务选项等 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <!-- 字典数量 3 个及以下且非多选模式下，使用 Radio 单选 -->
  <a-radio-group
    v-if="shouldUseRadio && !multiple"
    :value="modelValue"
    :disabled="disabled"
    :size="radioSize"
    @change="handleRadioChange"
  >
    <a-radio
      v-for="option in radioOptions"
      :key="String(option.value)"
      :value="option.value"
    >
      {{ option.label }}
    </a-radio>
  </a-radio-group>
  
  <!-- 其他情况使用 Select 下拉选择框 -->
  <a-select
    v-else
    :value="effectiveModelValue"
    :options="options"
    :loading="loading"
    :placeholder="placeholder ?? t('common.page.form.placeholder.selectonly')"
    :allow-clear="allowClear"
    :disabled="disabled"
    :mode="multiple ? 'multiple' : undefined"
    :size="size"
    :show-search="showSearch"
    :filter-option="effectiveFilterOption"
    :option-filter-prop="remoteSearch ? undefined : 'label'"
    :virtual="shouldUseVirtual"
    :list-height="listHeight"
    v-bind="{
      ...$attrs,
      ...(multiple ? { maxTagCount: effectiveMaxTagCount } : {})
    }"
    @change="handleChange"
    @search="handleSearch"
  >
    <template
      v-if="$slots.default"
      #default
    >
      <slot />
    </template>
  </a-select>
</template>

<script setup lang="ts">
import type { SelectValue, DefaultOptionType } from 'ant-design-vue/es/select'
import type { TaktDictSelectFieldNames, TaktDictSelectOption, TaktSelectOption } from '@/types/common'
import request from '@/api/request'
import { createLogger } from '@/utils/logger'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { isEmptyFormFieldValue } from '@/utils/takt-dict-default'
import { useI18n } from 'vue-i18n'

const selectLogger = createLogger('takt-select')

/** API 选项最大条数（08-overflow-fullstack） */
const MAX_SELECT_OPTIONS = 500

const { t } = useI18n()
type SelectOptionLike = { label?: string; value?: string | number; dictLabel?: string; dictValue?: string | number; extLabel?: string; extValue?: string | number } & Record<string, unknown>

interface Props {
  /** 绑定值 */
  modelValue?: string | number | (string | number)[] | undefined
  /** 字典类型编码(用于加载字典数据,优先级高于 apiUrl) */
  dictType?: string | undefined
  /** API 端点(可选,如果提供了 dictType 或 options 则不需要) */
  apiUrl?: string | undefined
  /** 选项数据(可选,如果提供了则直接使用,不再通过 dictType 或 apiUrl 加载)。支持标准格式 { label, value } 或 TaktSelectOption 格式 { dictLabel, dictValue } */
  options?: TaktSelectOption[] | Array<{ label: string; value: string | number } & Record<string, unknown>> | undefined
  /** 占位符 */
  placeholder?: string | undefined
  /** 是否显示清除按钮 */
  allowClear?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 是否多选 */
  multiple?: boolean
  /** 尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 是否支持搜索 */
  showSearch?: boolean
  /** 自定义过滤函数 */
  filterOption?: boolean | ((input: string, option?: DefaultOptionType) => boolean)
  /** 多选时最多显示多少个标签,超出部分以 +N 形式展示。支持数字或 'responsive'(响应式模式,但不推荐在大表单场景下使用,因为对性能有所消耗) */
  maxTagCount?: number | 'responsive' | undefined
  /** 是否开启虚拟滚动(大数据量时建议开启,可提升渲染性能)。如果不指定,当选项数量超过 100 条时会自动开启 */
  virtual?: boolean
  /** API 请求附加查询参数（与 apiUrl 联用，如 plantCode、keyword） */
  apiParams?: Record<string, string | number | boolean | undefined | null> | undefined
  /** 是否远程搜索（apiUrl 模式下输入关键字重新请求后端，关闭客户端 filter） */
  remoteSearch?: boolean
  /** 远程搜索时写入请求的查询参数字段名，默认 keyword */
  searchParamKey?: string
  /** 远程搜索防抖毫秒，默认 300 */
  searchDebounceMs?: number
  /** 虚拟滚动时列表高度(单位:px),默认 256px */
  listHeight?: number
  /** 字段映射配置(用于自定义 label 和 value 字段名) */
  fieldNames?: {
    label?: string
    value?: string
  }
  /** dict-type 模式下值未绑定时是否自动选中 IsDefault=1 项（默认 true；用户手动清空后不再回填） */
  applyDictDefault?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: undefined,
  dictType: undefined,
  apiUrl: undefined,
  options: undefined,
  placeholder: undefined,
  allowClear: true,
  disabled: false,
  multiple: false,
  size: 'middle',
  showSearch: true,
  filterOption: true,
  maxTagCount: undefined,
  virtual: false,
  apiParams: undefined,
  remoteSearch: false,
  searchParamKey: 'keyword',
  searchDebounceMs: 300,
  listHeight: 256,
  fieldNames: () => ({
    label: 'label',
    value: 'value'
  }),
  applyDictDefault: true,
})

/**
 * 默认客户端过滤：按 label / value / dictLabel / dictValue 模糊匹配
 * @param input 搜索输入
 * @param option 选项
 * @returns 是否保留
 */
function defaultFilterOption(input: string, option?: DefaultOptionType): boolean {
  const needle = (input || '').trim().toLowerCase()
  if (!needle) {
    return true
  }
  const opt = option as SelectOptionLike | undefined
  const nested = (option as { option?: SelectOptionLike } | undefined)?.option
  const label = String(opt?.label ?? nested?.label ?? opt?.dictLabel ?? nested?.dictLabel ?? '')
  const value = String(opt?.value ?? nested?.value ?? opt?.dictValue ?? nested?.dictValue ?? '')
  const extLabel = String(opt?.extLabel ?? nested?.extLabel ?? '')
  return `${label} ${value} ${extLabel}`.toLowerCase().includes(needle)
}

const effectiveFilterOption = computed(() => {
  if (props.remoteSearch && props.apiUrl) {
    return false
  }
  if (typeof props.filterOption === 'function') {
    const customFilter = props.filterOption
    return (input: string, option?: DefaultOptionType) => customFilter(input, option)
  }
  if (props.filterOption === true) {
    return defaultFilterOption
  }
  return props.filterOption
})

const emit = defineEmits<{
  'update:modelValue': [value: string | number | (string | number)[] | undefined]
  'change': [value: string | number | (string | number)[] | undefined, option: SelectOptionLike | SelectOptionLike[] | null]
  'search': [value: string]
}>()

const loading = ref(false)
const rawData = ref<TaktSelectOption[]>([])
const remoteSearchKeyword = ref('')
let remoteSearchTimer: ReturnType<typeof setTimeout> | undefined
const dictDataStore = useDictDataStore()
/** 当前 dictType 是否已自动写入过 IsDefault（用户清空后不再自动回填） */
const dictDefaultApplied = ref(false)

/**
 * 解析 dict-type 模式下 valueField 映射
 * @returns getDictDefaultValue 使用的 valueField
 */
function resolveDictValueField(): TaktDictSelectFieldNames['valueField'] {
  const valueFieldKey = props.fieldNames?.value
  if (valueFieldKey === 'extLabel') {
    return 'extLabel'
  }
  if (valueFieldKey === 'extValue') {
    return 'extValue'
  }
  if (valueFieldKey === 'sortOrder') {
    return 'sortOrder'
  }
  return 'dictValue'
}

/**
 * dict-type 且绑定值为空时，按 IsDefault=1 自动选中（每 dictType 仅一次）
 */
function tryApplyDictDefault(): void {
  if (!props.dictType || props.applyDictDefault === false || props.multiple || dictDefaultApplied.value) {
    return
  }
  if (!isEmptyFormFieldValue(props.modelValue)) {
    return
  }

  const dictValueField = resolveDictValueField()
  const defaultValue = dictDataStore.getDictDefaultValue(props.dictType, dictValueField)
  if (defaultValue === undefined) {
    return
  }

  let expectedValueType = inferValueType(props.modelValue)
  if (expectedValueType === 'string' && props.modelValue == null) {
    const dictOptions = dictDataStore.getDictOptionsForSelect(props.dictType, {
      valueField: dictValueField,
      labelField: props.fieldNames?.label === 'extLabel' ? 'extLabel' : 'dictLabel',
    })
    if (dictOptions.every((option) => isNumericValue(option.value))) {
      expectedValueType = 'number'
    }
  }

  const rawValue = typeof defaultValue === 'number' ? defaultValue : String(defaultValue)
  const convertedValue = convertValueType(rawValue, expectedValueType, props.dictType)
  dictDefaultApplied.value = true
  emit('update:modelValue', convertedValue)
}

/**
 * 推断期望的值类型（根据 modelValue 的类型）
 * @param modelValue 绑定值
 * @returns 期望的值类型：'number' | 'string'
 */
function inferValueType(modelValue: string | number | (string | number)[] | undefined): 'number' | 'string' {
  if (modelValue === undefined || modelValue === null) {
    // 无法推断，默认返回 string
    return 'string'
  }
  
  // 如果是数组，检查第一个元素的类型
  if (Array.isArray(modelValue)) {
    if (modelValue.length === 0) {
      return 'string'
    }
    return typeof modelValue[0] === 'number' ? 'number' : 'string'
  }
  
  // 单个值，直接检查类型
  return typeof modelValue === 'number' ? 'number' : 'string'
}

/**
 * 判断值是否是数值类型（number 或可转换为 number 的 string）
 */
function isNumericValue(value: string | number | (string | number)[] | undefined): boolean {
  if (value == null) return false
  
  if (Array.isArray(value)) {
    if (value.length === 0) return false
    const first = value[0]
    return typeof first === 'number' || (typeof first === 'string' && first !== '' && !isNaN(Number(first)))
  }
  
  return typeof value === 'number' || (typeof value === 'string' && value !== '' && !isNaN(Number(value)))
}

/**
 * 判断字符串是否是纯数字字符串（用于字典值的类型转换）
 */
function isNumericString(str: string): boolean {
  if (!str || str.trim() === '') return false
  const trimmed = String(str).trim()
  if (!/^-?\d+(\.\d+)?$/.test(trimmed) || Number.isNaN(Number(trimmed))) return false
  return Number.isSafeInteger(Number(trimmed))
}

/**
 * 转换值类型（根据期望的类型转换字典数据的值）
 */
function convertValueType(value: string | number, expectedType: 'number' | 'string', dictType: string): string | number {
  if (typeof value === expectedType) {
    return value
  }
  
  if (expectedType === 'number' && typeof value === 'string') {
    if (!value || value.trim() === '') {
      selectLogger.warn('字典值为空，无法转换为 number，返回 0', {
        action: 'convertValueType',
        dictType,
        value,
      })
      return 0
    }
    
    if (!isNumericString(value)) {
      selectLogger.error('字典值不是数值字符串，无法转换为 number', {
        action: 'convertValueType',
        dictType,
        value,
      })
      throw new Error(`字典数据值类型转换失败：字典类型 ${dictType} 的值 "${value}" 不是数值字符串，无法转换为 number 类型`)
    }
    
    const numValue = Number(value)
    if (isNaN(numValue)) {
      selectLogger.error('字典值无法转换为 number', {
        action: 'convertValueType',
        dictType,
        value,
      })
      throw new Error(`字典数据值类型转换失败：字典类型 ${dictType} 的值 "${value}" 无法转换为 number 类型`)
    }
    
    return numValue
  }
  
  if (expectedType === 'string' && typeof value === 'number') {
    return String(value)
  }
  
  return value
}

function normalizeValue(value: unknown): string | number {
  if (typeof value === 'string' || typeof value === 'number') {
    return value
  }
  return ''
}

/**
 * 多选模式绑定值规范化：未选/空/无效 DictValue 视为 undefined，并与 options.value 对齐
 * @param modelValue 绑定值
 * @param optionList 当前下拉选项
 * @returns a-select 可识别的多选值；未选时为 undefined
 */
function normalizeMultipleSelectValue(
  modelValue: string | number | (string | number)[] | undefined | null,
  optionList: ReadonlyArray<{ value?: string | number }>,
): (string | number)[] | undefined {
  if (modelValue == null || modelValue === '') {
    return undefined
  }
  let candidates: (string | number)[]
  if (Array.isArray(modelValue)) {
    candidates = [...modelValue]
  } else if (typeof modelValue === 'number' && modelValue === 0) {
    return undefined
  } else if (modelValue === '0') {
    return undefined
  } else {
    candidates = [modelValue]
  }
  const filtered = candidates.filter((item) => {
    if (item == null) {
      return false
    }
    const text = String(item).trim()
    return text !== '' && text !== '0'
  })
  if (filtered.length === 0) {
    return undefined
  }
  if (optionList.length === 0) {
    return undefined
  }
  const aligned = filtered
    .map((item) => optionList.find((opt) => String(opt.value) === String(item))?.value)
    .filter((item): item is string | number => item != null && item !== '')
  return aligned.length > 0 ? aligned : undefined
}

/** a-select 实际绑定值（多选时剔除无效项，避免空白 tag + ×） */
const effectiveModelValue = computed(() => {
  if (!props.multiple) {
    return props.modelValue
  }
  return normalizeMultipleSelectValue(
    props.modelValue,
    options.value as ReadonlyArray<{ value?: string | number }>,
  )
})

// 将后端数据转换为 Select 组件需要的格式
const options = computed(() => {
  const expectedValueType = props.apiUrl ? 'string' as const : inferValueType(props.modelValue)
  
  // 如果直接提供了 options，需要转换字段名和值类型
  if (props.options?.length) {
    const labelField = props.fieldNames?.label ?? 'label'
    const valueField = props.fieldNames?.value ?? 'value'
    
    return props.options.map(item => {
      const itemObj = item as SelectOptionLike
      const rawValueSource = 'value' in item ? item.value : (itemObj[valueField] ?? itemObj.dictValue ?? itemObj.extLabel ?? itemObj.extValue ?? '')
      const label = 'label' in item ? item.label : (itemObj[labelField] ?? itemObj.dictLabel ?? itemObj.extLabel ?? '')
      const rawValue = normalizeValue(rawValueSource)
      const convertedValue = convertValueType(rawValue, expectedValueType, props.dictType || 'custom')
      
      return {
        ...itemObj,
        [labelField]: label,
        [valueField]: convertedValue,
        label,
        value: convertedValue
      }
    })
  }
  
  // 如果提供了 dictType，从字典 store 加载数据
  if (props.dictType) {
    const labelField = props.fieldNames?.label
    const valueField = props.fieldNames?.value
    
    // 转换 fieldNames 为 getDictOptions 需要的格式
    const dictLabelField: 'dictLabel' | 'extLabel' = (labelField === 'extLabel' ? 'extLabel' : 'dictLabel')
    const valueFieldKey = props.fieldNames?.value
    const dictValueField: TaktDictSelectFieldNames['valueField'] =
      valueFieldKey === 'extLabel'
        ? 'extLabel'
        : valueFieldKey === 'extValue'
          ? 'extValue'
          : valueFieldKey === 'sortOrder'
            ? 'sortOrder'
            : 'dictValue'

    const dictOptions = dictDataStore.getDictOptionsForSelect(props.dictType, {
      valueField: dictValueField,
      labelField: dictLabelField
    })
    
    // 根据 modelValue 的类型推断期望的值类型
    let expectedValueType = inferValueType(props.modelValue)
    
    // 如果 modelValue 是 undefined/null，但所有字典选项的值都是数值字符串，则推断为 number 类型
    if (expectedValueType === 'string' && props.modelValue == null) {
      if (dictOptions.every((option: { label: string; value: string | number }) => isNumericValue(option.value))) {
        expectedValueType = 'number'
      }
    }
    
    return dictOptions.map((option: TaktDictSelectOption) => {
      // sys_culture_code：DictLabel 即 NativeName（本族语+地区缩写），全球统一展示，不走 t(i18nKey)
      const resolvedLabel = props.dictType === 'sys_culture_code'
        ? String(option.dictLabel ?? option.label ?? '')
        : (option.i18nKey?.trim() ? t(option.i18nKey) : String(option.label ?? option.dictLabel ?? ''))
      return {
        ...option,
        label: resolvedLabel,
        value: convertValueType(option.value, expectedValueType, props.dictType || '')
      }
    })
  }
  
  // 否则使用从 API 加载的数据
  const labelField = props.fieldNames?.label ?? 'label'
  const valueField = props.fieldNames?.value ?? 'value'
  
  return rawData.value.map(item => {
    const itemAny = item as SelectOptionLike
    const label = labelField === 'extLabel' 
      ? String(itemAny.extLabel ?? itemAny.dictLabel ?? '')
      : (itemAny.dictLabel ?? '')
    
    const rawValue = valueField === 'extLabel'
      ? (itemAny.extLabel ?? itemAny.dictValue ?? '')
      : valueField === 'extValue'
      ? (itemAny.extValue ?? itemAny.dictValue ?? '')
      : (itemAny.dictValue ?? '')
    
    // apiUrl：后端 DTO 主键已为 string（ValueToStringConverter），统一转 string 选项值
    const convertedValue = props.apiUrl
      ? convertValueType(normalizeValue(rawValue), 'string', 'api')
      : convertValueType(rawValue, expectedValueType, props.dictType || 'api')
    
    return {
      ...item,
      [labelField]: label,
      [valueField]: convertedValue,
      label,
      value: convertedValue
    }
  })
})

// 根据数据量自动决定是否开启虚拟滚动（超过 100 条或显式 virtual 时开启）
const shouldUseVirtual = computed(() => {
  return props.virtual === true || options.value.length > 100
})

// 判断是否应该使用 Radio 单选（字典数量 3 个及以下且非多选模式，且值必须是数值类型）
const shouldUseRadio = computed(() => {
  if (!props.dictType || props.multiple || options.value.length === 0 || options.value.length > 3) {
    return false
  }
  
  const allOptionsAreNumeric = options.value.every((option: unknown) => isNumericValue((option as SelectOptionLike).value))
  if (!allOptionsAreNumeric) {
    return false
  }
  
  return props.modelValue == null || isNumericValue(props.modelValue)
})

// Radio 组件的尺寸（RadioGroup 不支持 'middle'，只支持 'small' | 'large' | 'default'）
const radioSize = computed(() => {
  return props.size === 'middle' ? 'default' : (props.size === 'small' ? 'small' : 'large')
})

// Radio 选项数据（options.value 已包含正确转换后的 label 和 value）
const radioOptions = computed(() => {
  return options.value.map((option: unknown) => {
    const item = option as SelectOptionLike
    return {
      label: item.label ?? item.dictLabel ?? '',
      value: item.value ?? ''
    }
  })
})

// 多选时，maxTagCount 只在 multiple 为 true 时生效
// 多选模式下，如果未设置 maxTagCount，默认显示 3 个标签
const effectiveMaxTagCount = computed<number | 'responsive'>(() => {
  // 仅在多选模式下才计算，确保返回值不会是 undefined
  const val = props.maxTagCount
  if (val !== undefined && val !== null) {
    return val
  }
  return 3
})

/**
 * apiParams 已传入时，是否满足发起请求（各级联参数字段均非空）
 * @returns 是否可请求
 */
function canLoadApiOptions(): boolean {
  if (props.disabled) {
    return false
  }
  if (!props.apiParams) {
    return true
  }
  const entries = Object.entries(props.apiParams)
  if (entries.length === 0) {
    return true
  }
  return entries.every(([, val]) => val !== undefined && val !== null && String(val).trim() !== '')
}

// 加载数据
const loadData = async () => {
  // 如果提供了 options，直接使用，不需要加载
  if (props.options?.length) {
    return
  }

  // dict-type：预热 Pinia 字典缓存，避免选项空白（登录 bootstrap 未完成时弹窗已打开）
  if (props.dictType) {
    try {
      loading.value = true
      await dictDataStore.loadAllDictDataAsync()
      tryApplyDictDefault()
    } catch (error) {
      selectLogger.warn('加载字典数据失败', { action: 'loadData', dictType: props.dictType }, error)
    } finally {
      loading.value = false
    }
    return
  }

  // 如果提供了 apiUrl，通过 API 加载数据
  if (props.apiUrl) {
    if (!canLoadApiOptions()) {
      rawData.value = []
      return
    }
    try {
      loading.value = true
      const params: Record<string, string | number | boolean> = {}
      if (props.apiParams) {
        for (const [key, val] of Object.entries(props.apiParams)) {
          if (val !== undefined && val !== null && val !== '') {
            params[key] = val
          }
        }
      }
      if (props.remoteSearch) {
        const keyword = remoteSearchKeyword.value.trim()
        if (keyword) {
          params[props.searchParamKey] = keyword
        }
      }
      const data = await request<TaktSelectOption[]>({
        url: props.apiUrl,
        method: 'get',
        params: Object.keys(params).length > 0 ? params : undefined,
      })
      rawData.value = Array.isArray(data) ? data : []
      if (!props.remoteSearch && rawData.value.length > MAX_SELECT_OPTIONS) {
        selectLogger.warn('选项数超过上限，已截断', {
          action: 'loadData',
          apiUrl: props.apiUrl,
          count: rawData.value.length,
          max: MAX_SELECT_OPTIONS,
        })
        rawData.value = rawData.value.slice(0, MAX_SELECT_OPTIONS)
      }
    } catch (error) {
      selectLogger.error('加载选项数据失败', { action: 'loadData', apiUrl: props.apiUrl }, error)
      rawData.value = []
    } finally {
      loading.value = false
    }
    return
  }

  // 如果 dictType、apiUrl 和 options 都未提供，才发出警告
  selectLogger.warn('dictType、apiUrl 和 options 都未提供，无法加载数据', { action: 'loadData' })
}

// 辅助函数：从 SelectValue 中提取原始值
const extractRawValue = (value: SelectValue): string | number | (string | number)[] | undefined => {
  if (value === undefined || value === null) {
    return undefined
  }
  
  // 如果是数组
  if (Array.isArray(value)) {
    return value.map(v => {
      if (typeof v === 'object' && v !== null && 'value' in v) {
        return (v).value
      }
      return v as string | number
    })
  }
  
  // 如果是对象（LabeledValue）
  if (typeof value === 'object' && 'value' in value) {
    return (value).value
  }
  
  // 原始值
  return value as string | number
}

// 处理 Radio 值变化
const handleRadioChange = (event: unknown) => {
  const eventValue = typeof event === 'object' && event !== null && 'target' in event
    ? (event as { target?: { value?: string | number } }).target?.value
    : undefined
  const value = eventValue ?? (event as string | number | null | undefined)
  if (value == null) return
  
  emit('update:modelValue', value)
  const option = radioOptions.value.find((opt: { label: string; value: string | number }) => opt.value === value)
  emit('change', value, option ?? null)
}

// 处理 Select 值变化
const handleChange = (value: SelectValue, option: DefaultOptionType | DefaultOptionType[]) => {
  const rawValue = extractRawValue(value)
  emit('update:modelValue', rawValue)
  const normalizedOption = option as SelectOptionLike | SelectOptionLike[] | null
  emit('change', rawValue, normalizedOption)
}

// 处理搜索
const handleSearch = (value: string) => {
  emit('search', value)
  if (!props.remoteSearch || !props.apiUrl) {
    return
  }
  if (remoteSearchTimer) {
    clearTimeout(remoteSearchTimer)
  }
  remoteSearchTimer = setTimeout(() => {
    remoteSearchKeyword.value = value
    void loadData()
  }, props.searchDebounceMs)
}

watch(() => props.dictType, () => {
  dictDefaultApplied.value = false
})

// 监听 dictType、API URL、options、apiParams 与 disabled 变化
watch(() => [props.dictType, props.apiUrl, props.options, props.apiParams, props.disabled], () => {
  if (props.options?.length) {
    return
  }
  if (props.dictType || props.apiUrl) {
    void loadData()
  }
}, { deep: true })

watch(() => props.modelValue, (val) => {
  if (!props.remoteSearch || !props.apiUrl || val == null || val === '') {
    return
  }
  const text = String(Array.isArray(val) ? val[0] : val).trim()
  if (!text || remoteSearchKeyword.value === text) {
    return
  }
  remoteSearchKeyword.value = text
  void loadData()
})

onMounted(() => {
  // 使用 nextTick 确保 props 已经完全初始化（特别是在条件渲染的场景下）
  nextTick(() => {
    if (props.remoteSearch && props.apiUrl && props.modelValue != null && props.modelValue !== '') {
      remoteSearchKeyword.value = String(
        Array.isArray(props.modelValue) ? props.modelValue[0] : props.modelValue,
      ).trim()
    }
    if (props.dictType || (props.apiUrl && !props.options?.length)) {
      void loadData()
    }
  })
})
</script>

<style scoped>
/* 组件样式 */
</style>
