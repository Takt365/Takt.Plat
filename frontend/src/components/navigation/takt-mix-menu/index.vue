<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/navigation/takt-mix-menu
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:混合菜单组件,支持内联和折叠模式

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-menu
    v-model:selected-keys="selectedKeys"
    v-model:open-keys="openKeys"
    mode="inline"
    :theme="theme"
    :items="menuItems"
    :inline-collapsed="collapsed"
    :inline-indent="TAKT_MENU_INLINE_INDENT"
    :accordion="setting.menuAccordion"
    :class="['takt-mix-menu', `menu-style-${setting.menuStyle}`]"
    @click="handleMenuClick"
  />
</template>

<script setup lang="ts">
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import { useThemeStore } from '@/stores/common/theme'
import { useMenuStore } from '@/stores/identity/menu'
import { useSettingStore } from '@/stores/common/setting'
import { TAKT_MENU_INLINE_INDENT } from '@/constants/takt-navigation-menu'
import { resolveMenuOpenKeysForPath } from '@/utils/takt-menu-open-keys'

interface Props {
  collapsed?: boolean
  theme?: 'light' | 'dark'
}

const props = withDefaults(defineProps<Props>(), {
  collapsed: false,
  theme: 'light'
})

const emit = defineEmits<{
  'click': [key: string]
}>()

const route = useRoute()
const router = useRouter()
const themeStore = useThemeStore()
const menuStore = useMenuStore()
const setting = useSettingStore().setting

const selectedKeys = ref<string[]>([])
const openKeys = ref<string[]>([])

const menuItems = computed(() => menuStore.menuItems ?? [])

const theme = computed(() => {
  if (props.theme === 'dark') return 'dark'
  return themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'
})

const handleMenuClick = (info: MenuInfo) => {
  const key = String(info.key)
  router.push(key)
  emit('click', key)
}

watch(
  () => [route.path, menuItems.value] as const,
  ([path]) => {
    selectedKeys.value = [path]
    openKeys.value = resolveMenuOpenKeysForPath(menuItems.value, path)
  },
  { immediate: true }
)
</script>

<style scoped>
.takt-mix-menu {
  border-right: 0;

  &.menu-style-rounded {
    :deep(.ant-menu-item) {
      border-radius: 4px;
      margin: 4px 8px;
    }
    :deep(.ant-menu-submenu-title) {
      border-radius: 4px;
      margin: 4px 8px;
    }
  }

  &.menu-style-plain {
    :deep(.ant-menu-item) {
      border-radius: 0;
      margin: 0;
    }
    :deep(.ant-menu-submenu-title) {
      border-radius: 0;
      margin: 0;
    }
  }
}
</style>
