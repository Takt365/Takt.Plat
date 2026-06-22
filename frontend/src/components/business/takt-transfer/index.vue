<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：@/components/business/takt-transfer -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：分配弹窗 Transfer 封装：视口居中、checkbox、@scroll、单击选中、双击穿梭；#children 可覆写树形列表。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-transfer
    v-model:target-keys="targetKeys"
    class="takt-transfer"
    :data-source="dataSource"
    show-search
    :filter-option="taktTransferFilterOption"
    :list-style="TAKT_ASSIGN_TRANSFER_LIST_STYLE"
    :operation-style="TAKT_TRANSFER_OPERATION_STYLE"
    :titles="titles"
    :loading="loading"
    :disabled="disabled"
    :render="render"
    @scroll="taktAssignTransferHandleScroll"
  >
    <template #children="slotProps">
      <slot
        name="children"
        v-bind="slotProps"
      >
        <ul class="ant-transfer-list-content">
          <li
            v-for="item in slotProps.filteredItems"
            :key="String(item.key)"
            class="ant-transfer-list-content-item"
            :class="{
              'ant-transfer-list-content-item-checked': isItemSelected(item.key, slotProps.selectedKeys),
              'ant-transfer-list-content-item-disabled': slotProps.disabled || item.disabled,
            }"
            @click="handleItemClick(item, slotProps.selectedKeys, slotProps.onItemSelect, slotProps.disabled)"
            @dblclick="handleItemDblClick(item, slotProps.direction, slotProps.onItemSelect, slotProps.disabled)"
          >
            <a-checkbox
              :checked="isItemSelected(item.key, slotProps.selectedKeys)"
              :disabled="Boolean(slotProps.disabled || item.disabled)"
              @click.stop
              @dblclick.stop
              @change="(e) => handleCheckboxChange(item, e.target.checked, slotProps.onItemSelect, slotProps.disabled)"
            />
            <span class="ant-transfer-list-content-item-text">{{ item.title }}</span>
          </li>
        </ul>
      </slot>
    </template>
  </a-transfer>
</template>

<script setup lang="ts">
/**
 * 业务 Transfer：unplugin-vue-components 全局注册为 takt-transfer；扁平列表含 checkbox，单击选中/双击穿梭。
 */
import type { TransferProps } from 'ant-design-vue'
import {
  taktTransferFilterOption,
  TAKT_ASSIGN_TRANSFER_LIST_STYLE,
  TAKT_TRANSFER_OPERATION_STYLE,
  taktAssignTransferHandleScroll,
  taktAssignTransferHandleItemClick,
  taktAssignTransferShuttleByDblClick,
  taktAssignTransferIsItemSelected,
  type TaktAssignTransferDirection,
  type TaktTransferRecord,
} from '@/utils/takt-transfer'

interface Props {
  /** 右侧已选 key 列表 */
  targetKeys?: string[]
  /** Transfer 数据源 */
  dataSource?: TransferProps['dataSource']
  /** 左右列标题 */
  titles: [string, string]
  /** 选项 loading */
  loading?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 行渲染（树形 #children 时可省略） */
  render?: TransferProps['render']
}

const props = withDefaults(defineProps<Props>(), {
  targetKeys: () => [],
  dataSource: () => [],
  loading: false,
  disabled: false,
  render: (item: TaktTransferRecord) => item.title ?? '',
})

const emit = defineEmits<{
  'update:targetKeys': [value: string[]]
}>()

/** 右侧已选 key（v-model:target-keys） */
const targetKeys = computed({
  get: () => props.targetKeys,
  set: (value: string[]) => emit('update:targetKeys', value),
})

/**
 * 行是否选中
 * @param key 行 key
 * @param selectedKeys 当前列已选
 * @returns 是否选中
 */
function isItemSelected(
  key: string | number | undefined,
  selectedKeys: readonly (string | number)[],
): boolean {
  if (key == null) return false
  return taktAssignTransferIsItemSelected(key, selectedKeys)
}

/**
 * 单击切换选中
 * @param item 行数据
 * @param selectedKeys 当前列已选
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleItemClick(
  item: TaktTransferRecord,
  selectedKeys: readonly (string | number)[],
  onItemSelect: (itemKey: string, selected: boolean) => void,
  listDisabled?: boolean,
): void {
  taktAssignTransferHandleItemClick(item.key!, selectedKeys, onItemSelect, listDisabled, item.disabled)
}

/**
 * Checkbox 切换选中（与行单击、表头全选一致）
 * @param item 行数据
 * @param checked 是否选中
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleCheckboxChange(
  item: TaktTransferRecord,
  checked: boolean,
  onItemSelect: (itemKey: string, selected: boolean) => void,
  listDisabled?: boolean,
): void {
  if (listDisabled || item.disabled) return
  onItemSelect(String(item.key), checked)
}

/**
 * 双击穿梭
 * @param item 行数据
 * @param direction 列方向
 * @param onItemSelect Transfer 回调
 * @param listDisabled 整列禁用
 */
function handleItemDblClick(
  item: TaktTransferRecord,
  direction: TaktAssignTransferDirection,
  onItemSelect: (itemKey: string, selected: boolean) => void,
  listDisabled?: boolean,
): void {
  taktAssignTransferShuttleByDblClick(
    item.key!,
    direction,
    () => targetKeys.value,
    (keys) => { targetKeys.value = keys },
    onItemSelect,
    listDisabled,
    item.disabled,
  )
}
</script>
