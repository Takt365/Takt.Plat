<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/navigation/takt-header-user
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:用户菜单组件,显示头像和个人信息下拉菜单;引用键 common.page.button.* 与 common.tip.confirm.*

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-dropdown
    :trigger="['click']"
    placement="bottomRight"
  >
    <a-button type="text">
      <template #icon>
        <a-avatar
          :size="20"
          v-bind="avatarUrl ? { src: avatarUrl } : {}"
        >
          <template v-if="!avatarUrl">
            <RiUserLine class="takt-remix-icon" />
          </template>
        </a-avatar>
      </template>
    </a-button>
    <template #overlay>
      <a-menu>
        <a-menu-item @click="handleProfile">
          <UserOutlined />
          {{ $t('common.page.button.profile') }}
        </a-menu-item>
        <a-menu-item @click="handleSettings">
          <SettingOutlined />
          {{ $t('common.page.button.personalsettings') }}
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item @click="handleLogout">
          <LogoutOutlined />
          {{ $t('common.page.button.logout') }}
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { Modal } from 'ant-design-vue'
import {
  UserOutlined,
  SettingOutlined,
  LogoutOutlined
} from '@ant-design/icons-vue'
import { RiUserLine } from '@remixicon/vue'
import { storeToRefs } from 'pinia'
import { useUserStore } from '@/stores/identity/user'
import { EventBus } from '@/utils/event-bus'

const emit = defineEmits<{
  'profile': []
  'settings': []
  'logout': []
}>()

const router = useRouter()
const { userInfo } = storeToRefs(useUserStore())

/** 头像 URL：有用户头像则展示，否则显示默认图标 */
const avatarUrl = computed(() => {
  const avatar = userInfo.value?.avatar
  if (avatar && avatar.trim()) {
    return avatar
  }
  return undefined
})

const handleProfile = () => {
  emit('profile')
  router.push('/profile')
}

const handleSettings = () => {
  emit('settings')
  router.push('/settings')
}

const { t } = useI18n()
const handleLogout = () => {
  const logoutAction = t('common.page.button.logout')
  Modal.confirm({
    title: t('common.tip.confirm.title', { action: logoutAction }),
    content: t('common.tip.confirm.question', { action: logoutAction }),
    centered: true,
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      emit('logout')
      EventBus.emit('user:logout', undefined)
    }
  })
}
</script>
<style scoped>

</style>

