<!-- ======================================== -->
<!-- 项目名称：节拍工厂·Takt Plat -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：showcase-form.vue -->
<!-- 功能描述：登录页展示区 SVG + common.page.app.slogan / tagline -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex min-h-screen w-full flex-col text-center text-white">
    <!-- 插画区：视口高度 2/3 -->
    <div class="flex h-[66.67vh] w-full shrink-0 items-center justify-center overflow-hidden px-4">
      <object
        type="image/svg+xml"
        :aria-label="ariaLabel"
        :data="objectData"
        class="block h-full max-h-full w-full max-w-full object-contain"
      >
        <img
          :alt="ariaLabel"
          :src="objectData"
          class="h-full max-h-full w-full max-w-full object-contain"
        >
      </object>
    </div>

    <!-- 文案区：视口高度 1/3 -->
    <div class="flex h-[33.33vh] w-full shrink-0 flex-col items-center justify-center gap-2 px-6">
      <p class="text-[48px] font-semibold leading-tight">
        {{ t('common.page.app.slogan') }}
      </p>
      <p class="text-[32px] leading-snug text-white/75">
        {{ t('common.page.app.tagline') }}
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 登录页左侧展示区：SVG 插画 + 应用 slogan / tagline
 */
import { useI18n } from 'vue-i18n';
import taktSmartSvg from '@/assets/images/takt-smart.svg';

/** 组件入参 */
interface Props {
  /** SVG 地址；未传时使用内置 takt-smart.svg */
  src?: string;
  /** 无障碍文案 */
  ariaLabel?: string;
}

const props = withDefaults(defineProps<Props>(), {
  src: taktSmartSvg,
  ariaLabel: 'Onboarding app explainer animation',
});

/** i18n 翻译函数 */
const { t } = useI18n();

/** 绑定到 object :data 与降级 img :src */
const objectData = ref(props.src);

/** 本地 SVG blob URL，切换源或卸载时需释放 */
let blobUrlToRevoke: string | null = null;

/**
 * 拉取 SVG 并将已知浅色背景替换为透明后生成 blob URL
 * @param url 可 fetch 的 SVG 地址
 * @returns {Promise<string>} blob object URL
 */
async function loadSvgWithTransparentBackgroundAsync(url: string): Promise<string> {
  const res = await fetch(url);
  const text = await res.text();
  const transparent = text.replace(
    /style="background-color:#edf2ff"/i,
    'style="background-color:transparent"',
  );
  const blob = new Blob([transparent], { type: 'image/svg+xml' });
  const blobUrl = URL.createObjectURL(blob);
  blobUrlToRevoke = blobUrl;
  return blobUrl;
}

/**
 * 按 props.src 决定透明化 blob 或直连 URL，并清理旧 blob
 */
function ensureObjectData(): void {
  const url = props.src ?? taktSmartSvg;
  if (url === taktSmartSvg) {
    void loadSvgWithTransparentBackgroundAsync(url).then((blobUrl) => {
      objectData.value = blobUrl;
    });
    return;
  }
  if (blobUrlToRevoke) {
    URL.revokeObjectURL(blobUrlToRevoke);
    blobUrlToRevoke = null;
  }
  objectData.value = url;
}

watch(() => props.src, ensureObjectData);

onMounted(ensureObjectData);

onBeforeUnmount(() => {
  if (blobUrlToRevoke) {
    URL.revokeObjectURL(blobUrlToRevoke);
    blobUrlToRevoke = null;
  }
});
</script>
