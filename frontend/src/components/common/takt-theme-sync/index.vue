<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/components/common/takt-theme-sync -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：监听 Ant Design Vue token 并同步为全局 CSS 变量 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <span class="hidden" aria-hidden="true" />
</template>

<script setup lang="ts">
/**
 * 将 ConfigProvider 上下文中的 Design Token 同步到 :root CSS 变量
 */
import { theme } from 'ant-design-vue';
import { syncAntDesignCssVariables } from '@/utils/theme';

const { token } = theme.useToken();

watchEffect(() => {
  syncAntDesignCssVariables(token.value);
});
</script>
