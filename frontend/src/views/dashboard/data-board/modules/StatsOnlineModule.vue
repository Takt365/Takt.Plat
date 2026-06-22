<template>
  <div class="stats-online-module">
    <a-spin :spinning="loading">
      <a-row :gutter="16">
        <a-col :span="8">
          <a-statistic
            :title="t('dashboard.data-board.page.online.users')"
            :value="onlineStats.users"
          />
        </a-col>
        <a-col :span="8">
          <a-statistic
            :title="t('dashboard.data-board.page.online.todayvisits')"
            :value="onlineStats.todayVisits"
          />
        </a-col>
        <a-col :span="8">
          <a-statistic
            :title="t('dashboard.data-board.page.online.sessions')"
            :value="onlineStats.sessions"
          />
        </a-col>
      </a-row>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { getOnlineStatistics } from '@/api/foundation/online'

const { t } = useI18n()

const loading = ref(false)
const onlineStats = ref({
  users: 0,
  todayVisits: 0,
  sessions: 0
})

async function fetchOnlineStats() {
  loading.value = true
  try {
    // 使用专门的统计API，性能更好
    const stats = await getOnlineStatistics()
    onlineStats.value = {
      users: stats?.onlineCount ?? 0,
      todayVisits: stats?.todayDurationSeconds ?? 0,
      sessions: stats?.currentDurationSeconds ?? 0,
    }
  } catch (error) {
    logger.error('[StatsOnlineModule] 获取在线用户统计失败', undefined, error)
    onlineStats.value = { users: 0, todayVisits: 0, sessions: 0 }
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchOnlineStats()
})
</script>

<style scoped lang="css">
.stats-online-module {
  min-height: 80px;
}
</style>
