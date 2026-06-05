<!-- ======================================== -->
<!-- 项目名称：Takt.Plat -->
<!-- 命名空间：@/components/business/takt-modal -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-21 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 对话框组件，封装 a-modal，统一设置中文按钮文本 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="internalOpen"
    v-bind="modalProps"
    @cancel="handleCancel"
  >
    <template
      v-if="$slots.default"
      #default
    >
      <slot />
    </template>
    <template
      v-if="$slots.title"
      #title
    >
      <slot name="title" />
    </template>
    <template
      v-if="!hideFooter"
      #footer
    >
      <slot name="footer">
        <div class="flex justify-end gap-2 [&_.ant-btn]:inline-flex [&_.ant-btn]:items-center [&_.ant-btn]:gap-1 [&_.anticon]:!me-0">
          <a-button
            @click="handleCancel"
          >
            <template #icon>
              <RiCloseLine class="takt-remix-icon" />
            </template>
            {{ cancelText ?? t('common.page.button.cancel') }}
          </a-button>
          <a-button
            type="primary"
            @click="handleOk"
          >
            <template #icon>
              <RiCheckLine class="takt-remix-icon" />
            </template>
            {{ okText ?? t('common.page.button.submit') }}
          </a-button>
        </div>
      </slot>
    </template>
    <template
      v-if="$slots.closeIcon"
      #closeIcon
    >
      <slot name="closeIcon" />
    </template>
  </a-modal>
</template>

<script setup lang="ts">
import { RiCloseLine, RiCheckLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

interface Props {
  /** 确定按钮文本,默认为"提交" */
  okText?: string | undefined
  /** 取消按钮文本,默认为"取消" */
  cancelText?: string | undefined
  /** 是否显示对话框 */
  open?: boolean
  /** 是否使用视口默认尺寸（70vw×85vh）；紧凑弹窗（如验证码）设为 false */
  useViewportSize?: boolean
  /** 是否隐藏底栏（仅保留标题栏关闭按钮，如验证码弹窗） */
  hideFooter?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  okText: undefined,
  cancelText: undefined,
  open: false,
  useViewportSize: true,
  hideFooter: false,
})

const emit = defineEmits<{
  'update:open': [open: boolean]
  'ok': [e: MouseEvent]
  'cancel': [e: MouseEvent]
}>()

const attrs = useAttrs()

// 内部 open 状态
const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => {
    emit('update:open', value)
  }
})

// 计算 modal 的所有属性，排除已定义的 props；统一默认弹出大小：视口 70% 宽、85% 高；默认垂直水平居中（与 a-modal 的 centered 一致）
const modalProps = computed(() => {
  const { centered, ...rest } = attrs
  const wrapClassName = [
    rest.wrapClassName,
    props.useViewportSize ? 'takt-modal-viewport-size' : null,
  ]
    .filter(Boolean)
    .join(' ')
  const width = rest.width !== undefined && rest.width !== null ? rest.width : '70vw'
  const normalizedCentered = typeof centered === 'boolean' ? centered : true
  return {
    ...rest,
    width: width as string | number,
    wrapClassName,
    centered: normalizedCentered,
    ...(props.hideFooter ? { footer: null } : {}),
  }
})

// 处理确定按钮点击
const handleOk = (e: MouseEvent) => {
  emit('ok', e)
}

// 处理取消按钮点击
const handleCancel = (e: MouseEvent) => {
  emit('cancel', e)
  emit('update:open', false)
}
</script>

<!-- 不 scoped：弹窗 teleport 到 body；宽度由内层 .ant-modal 的内联 width 决定，content 须 100% 填满，勿再写 70vw，否则会宽于父级导致内容视觉上偏右 -->
<style>
.takt-modal-viewport-size.ant-modal-wrap {
  display: flex;
  align-items: center;
  justify-content: center;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal {
  top: 0;
  margin: 0;
  padding-bottom: 0;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-content {
  width: 100%;
  max-width: 100%;
  box-sizing: border-box;
  height: 85vh;
  min-width: 0;
  min-height: 360px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-body {
  flex: 1;
  min-height: 0;
  overflow: auto;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-footer {
  flex-shrink: 0;
}
</style>
