<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/navigation/takt-side-menu
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:侧边菜单组件,支持折叠和主题切换

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
    :inline-indent="menuInlineIndent"
    :class="['takt-side-menu', `menu-style-${setting.menuStyle}`]"
    @click="handleMenuClick"
    @openChange="handleOpenChange"
  />
</template>

<script setup lang="ts">
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import { useThemeStore } from '@/stores/common/theme'
import { useMenuStore } from '@/stores/identity/menu'
import { useSettingStore } from '@/stores/common/setting'
import { TAKT_MENU_INLINE_INDENT } from '@/utils/common'
import {
  buildMenuParentKeyMap,
  normalizeMenuOpenKeys,
  resolveMenuOpenKeysForPath,
} from '@/utils/takt-menu-open-keys'

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
const { setting } = storeToRefs(useSettingStore())

const selectedKeys = ref<string[]>([])
const openKeys = ref<string[]>([])

const menuItems = computed(() => menuStore.menuItems ?? [])

/** 侧边菜单每层级缩进（px），与 Ant Design inlineIndent 一致 */
const menuInlineIndent = TAKT_MENU_INLINE_INDENT

/** 菜单 key → 父 key（手风琴收拢同级分支） */
const menuParentKeyMap = computed(() => buildMenuParentKeyMap(menuItems.value))

const theme = computed(() => {
  if (props.theme === 'dark') return 'dark'
  return themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'
})

const handleMenuClick = (info: MenuInfo) => {
  const key = String(info.key)
  router.push(key)
  emit('click', key)
}

/**
 * SubMenu 展开/收拢：收起父级时级联移除子孙 openKeys（避免须逐级点击收缩）
 * @param keys 变更后的 openKeys
 */
const handleOpenChange = (keys: string[]) => {
  openKeys.value = normalizeMenuOpenKeys(keys, menuParentKeyMap.value, setting.value.menuAccordion)
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
.takt-side-menu {
  border-right: 0;

  /* 图标与文本间距见 menu-base.css；层级缩进由 :inline-indent 控制 */

  &.menu-style-rounded {
    :deep(.ant-menu-item) {
      border-radius: 4px;
      margin: 4px 4px;
    }
    :deep(.ant-menu-submenu-title) {
      border-radius: 4px;
      margin: 4px 4px;
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
