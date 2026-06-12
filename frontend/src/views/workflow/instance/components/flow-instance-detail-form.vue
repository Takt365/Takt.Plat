<!-- ======================================== -->
<!-- 项目名称：节节拍工厂·Takt Plat  -->
<!-- 命名空间：@/views/workflow/my/components -->
<!-- 文件名称：flow-instance-detail-form.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：我的流程中流程实例详情展示组件，展示实例编码、状态、当前节点、流转历史等（只读，由父级 TaktModal 包裹） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detail">
    <a-descriptions
      bordered
      size="small"
      :column="1"
    >
      <a-descriptions-item :label="t('entity.flowInstance.instancecode')">
        {{ detail.instanceCode }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.processname')">
        {{ detail.processName }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.processtitle')">
        {{ detail.processTitle }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.instancestatus')">
        {{ statusText(detail.instanceStatus) }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.currentactivityname')">
        {{ detail.currentActivityName ?? '-' }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.startusername')">
        {{ detail.startUserName }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.starttime')">
        {{ detail.startTime }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('workflow.instance.page.taskFormContent')">
        <TaskFormContent :detail="detail" />
      </a-descriptions-item>
      <a-descriptions-item :label="t('entity.flowInstance.historicactivities')">
        <div
          v-for="(h, i) in historyItems"
          :key="i"
          class="history-item"
        >
          {{ h.fromNodeName }} → {{ h.toNodeName }}（{{ h.transitionUserName }}，{{ h.transitionTime }}）
          <span v-if="h.transitionComment">：{{ h.transitionComment }}</span>
        </div>
        <span v-if="!historyItems.length">{{ t('workflow.instance.page.noHistory') }}</span>
      </a-descriptions-item>
    </a-descriptions>
    <takt-flow-pending-add-approvers-panel
      :detail="detail"
      :allow-reduce="!!detail?.canVerify"
      @refresh="$emit('refresh')"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 我的流程·实例详情展示（只读）：实例编码、流程名、标题、状态、当前节点、发起人、发起时间、流转历史；未处理加签与减签。
 */
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import type { FlowHistoryItem, FlowInstanceDetail } from '@/types/workflow/flow-engine'
import TaskFormContent from '@/views/workflow/todo/components/flow-task-form-content.vue'

/** 父组件传入的实例详情 */
interface Props {
  detail: FlowInstanceDetail | null
}

const props = defineProps<Props>()
defineEmits<{ refresh: [] }>()

const { t } = useI18n()

/** 校验流转历史项结构（兼容 API 偶发脏数据） */
function isFlowHistoryItem(item: unknown): item is FlowHistoryItem {
  if (item == null || typeof item !== 'object') return false
  const row = item as Partial<FlowHistoryItem>
  return typeof row.fromNodeName === 'string'
    && typeof row.toNodeName === 'string'
    && typeof row.transitionUserName === 'string'
    && typeof row.transitionTime === 'string'
}

const historyItems = computed<FlowHistoryItem[]>(() => {
  const list = props.detail?.history
  if (!Array.isArray(list)) return []
  return list.filter(isFlowHistoryItem)
})

/** 实例状态码转展示文案（走 i18n workflow.instance.page.status.*） */
function statusText(s: number) {
  return t(`workflow.instance.page.status.${s}`) || t('workflow.instance.page.status.unknown')
}
</script>

<style scoped lang="css">
.history-item {
  font-size: 12px;
  margin-bottom: 4px;
}
</style>
