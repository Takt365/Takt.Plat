<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/code/database/database-backup/components -->
<!-- 文件名称：backup-local-folder-dialog.vue -->
<!-- 功能描述：本地（服务器端）备份目录选择（TaktDirectoryExplore method=server） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <takt-modal
    v-model:open="openProxy"
    :title="t('code.database.database-backup.page.dialog.localtitle')"
    :confirm-loading="confirmLoading"
    :use-viewport-size="false"
    width="960px"
    @ok="handleOk"
  >
    <takt-directory-explore
      v-if="openProxy"
      ref="exploreRef"
      method="server"
      :initial-path="initialPath"
      :panel-height="480"
      :table-scroll-y="340"
    />
  </takt-modal>
</template>

<script setup lang="ts">
/**
 * 本地备份目录：通用目录浏览（服务器目录）
 */
import { computed, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'

/** i18n */
const { t } = useI18n()

const props = defineProps<{
  open: boolean
  initialPath?: string
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
  confirm: [path: string]
}>()

const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v),
})

const confirmLoading = ref(false)
const exploreRef = ref<{
  resolveConfirmPath: (ensure?: boolean) => Promise<string>
} | null>(null)

async function handleOk() {
  confirmLoading.value = true
  try {
    const path = await exploreRef.value?.resolveConfirmPath(true)
    if (!path || !/^[A-Za-z]:[\\/]/.test(path)) {
      message.warning(t('code.database.database-backup.page.dialog.localneedabsolute'))
      return
    }
    emit('confirm', path)
    openProxy.value = false
  } catch (error) {
    logger.error('[BackupLocalDialog] confirm failed', { error })
    message.error(
      (error instanceof Error && error.message)
        || t('code.database.database-backup.page.dialog.createdirectoryfailed'),
    )
  } finally {
    confirmLoading.value = false
  }
}

watch(
  () => props.open,
  () => {
    confirmLoading.value = false
  },
)
</script>
