<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/navigation/takt-breadcrumb
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:面包屑导航组件,显示当前页面路径层级

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-breadcrumb v-if="show">
    <a-breadcrumb-item
      v-for="(item, index) in breadcrumbItems"
      :key="index"
    >
      <router-link
        v-if="item.path && index < breadcrumbItems.length - 1"
        :to="item.path"
        class="breadcrumb-link"
      >
        <component
          :is="item.icon"
          v-if="item.icon"
          :class="TAKT_REMIX_ICON_CLASS"
        />
        <span class="breadcrumb-title">{{ item.title }}</span>
      </router-link>
      <span
        v-else
        class="breadcrumb-plain"
      >
        <component
          :is="item.icon"
          v-if="item.icon"
          :class="TAKT_REMIX_ICON_CLASS"
        />
        <span class="breadcrumb-title">{{ item.title }}</span>
      </span>
    </a-breadcrumb-item>
  </a-breadcrumb>
</template>

<script setup lang="ts">
import type { Component } from 'vue';
import type { RouteMeta } from 'vue-router';
import { useI18n } from 'vue-i18n'
import { defaultSetting, useSettingStore } from '@/stores/common/setting'
import { useMenuStore } from '@/stores/identity/menu'
import type { TaktMenuTreeDto } from '@/types/identity/menu'
import {
  getRemixIconComponent,
  preloadRemixIcons,
} from '@/utils/takt-remix-icon'
import { TAKT_REMIX_ICON_CLASS } from '@/utils/common'
import { normalizeRoutePath } from '@/utils/permission'

interface BreadcrumbItem {
  title: string
  path?: string
  icon?: Component
}

const route = useRoute()
const { setting } = storeToRefs(useSettingStore())
const settingSafe = computed(() => setting.value ?? defaultSetting)
const { t } = useI18n()
const menuStore = useMenuStore()

/** 图标缓存版本（预加载后递增） */
const iconRevision = ref(0)

/**
 * 从菜单树中按 routePath 查找节点
 * @param menus 菜单树
 * @param path 当前路由 path
 */
const findMenuByPath = (menus: TaktMenuTreeDto[], path: string): TaktMenuTreeDto | null => {
  const normalized = normalizeRoutePath(path)

  for (const menu of menus) {
    const menuPath = menu.routePath?.trim()
      ? normalizeRoutePath(menu.routePath)
      : ''

    if (menuPath && menuPath === normalized) {
      return menu
    }

    if (menu.children?.length) {
      const found = findMenuByPath(menu.children, normalized)
      if (found) {
        return found
      }
    }
  }

  return null
}

/**
 * 获取翻译文本
 * @param menu 菜单节点
 * @param routeMeta 路由 meta
 */
const getTranslatedTitle = (menu: TaktMenuTreeDto | null, routeMeta: RouteMeta | undefined): string => {
  if (menu?.i18nKey) {
    const translated = t(menu.i18nKey)
    if (translated && translated !== menu.i18nKey) {
      return translated
    }
  }

  if (menu?.menuName) {
    return menu.menuName
  }

  const titleKey = (routeMeta?.title || routeMeta?.titleKey || '') as string
  return titleKey ? t(titleKey) : ''
}

const show = computed(() => settingSafe.value.showBreadcrumb)

const breadcrumbItems = computed(() => {
  void iconRevision.value
  
  const items: BreadcrumbItem[] = []
  const matched = route.matched.filter(item => item.meta && item.meta.title)
  
  matched.forEach((item, index) => {
    const routePath = item.path
    const menu = routePath ? findMenuByPath(menuStore.menuList, routePath) : null
    
    const menuIcon = menu?.icon?.trim() || (item.meta?.icon as string | undefined)
    
    if (menuIcon) {
      void preloadRemixIcons([menuIcon]).then(() => {
        iconRevision.value += 1
      })
    }
    
    const iconComponent = menuIcon ? getRemixIconComponent(menuIcon) : undefined
    
    // 获取翻译后的标题
    const title = getTranslatedTitle(menu, item.meta)
    
    // 构建面包屑项，避免在可选属性上直接使用 undefined
    const breadcrumbItem: BreadcrumbItem = {
      title: title || item.name as string || ''
    }
    
    if (index < matched.length - 1 && routePath !== undefined) {
      breadcrumbItem.path = routePath
    }
    
    if (setting.value.breadcrumbIcon && iconComponent) {
      breadcrumbItem.icon = iconComponent
    }
    
    items.push(breadcrumbItem)
  })
  
  return items
})
</script>

<style scoped>
/* 图标与文本间隔统一 6px */
:deep(.breadcrumb-link),
:deep(.breadcrumb-plain) {
  .breadcrumb-title {
    margin-left: 6px;
  }
}
</style>
