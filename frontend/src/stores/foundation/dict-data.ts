// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/stores/foundation
// 文件名称：dict-data.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据 Pinia 缓存（按 dictTypeCode 分组，供 takt-dict-tag 等使用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { getDictDataAll } from '@/api/foundation/dict-data';
import type { TaktDictSelectFieldNames, TaktDictSelectOption, TaktSelectOption } from '@/types/common';
import { createLogger } from '@/utils/logger';

const dictLogger = createLogger('dict-data');

/**
 * 将字典 DTO 转为下拉选项
 * @param dto 字典数据 DTO
 * @returns 下拉选项
 */
function mapDictDataToOption(dto: TaktSelectOption): TaktSelectOption {
  return {
    dictLabel: dto.dictLabel,
    dictValue: dto.dictValue,
    i18nKey: dto.i18nKey,
    dictTypeCode: dto.dictTypeCode,
    extLabel: dto.extLabel,
    extValue: dto.extValue,
    cssClass: dto.cssClass,
    listClass: dto.listClass,
    sortOrder: dto.sortOrder,
  };
}

/**
 * 字典数据状态管理
 */
export const useDictDataStore = defineStore('dict-data', () => {
  const dictMap = ref<Record<string, TaktSelectOption[]>>({});
  const loading = ref(false);
  const loaded = ref(false);

  let loadingPromise: Promise<void> | null = null;

  /**
   * 是否已加载完成
   */
  const isLoaded = computed(() => loaded.value);

  /**
   * 从缓存获取字典项
   * @param value 字典值或扩展标签
   * @param dictTypeCode 字典类型编码
   * @param useExtLabel 是否按 extLabel 匹配
   * @returns 匹配项或 undefined
   */
  function getDictOption(
    value: string | number,
    dictTypeCode: string,
    useExtLabel = false
  ): TaktSelectOption | undefined {
    const options = dictMap.value[dictTypeCode] ?? [];
    const valueText = String(value);

    if (useExtLabel) {
      return options.find((item) => String(item.extLabel) === valueText);
    }

    return options.find((item) => String(item.dictValue) === valueText);
  }

  /**
   * 按字典类型获取下拉选项（供 takt-select 等组件使用）
   * @param dictTypeCode 字典类型编码
   * @param fieldNames label / value 字段映射
   * @returns 含 label、value 的选项列表
   */
  function getDictOptionsForSelect(
    dictTypeCode: string,
    fieldNames: TaktDictSelectFieldNames
  ): TaktDictSelectOption[] {
    const options = dictMap.value[dictTypeCode] ?? [];
    const { labelField, valueField } = fieldNames;

    return options.map((item) => {
      const label = String(item[labelField] ?? item.dictLabel ?? '');
      const rawValue = item[valueField] ?? item.dictValue ?? '';
      const value =
        typeof rawValue === 'string' || typeof rawValue === 'number' ? rawValue : String(rawValue);

      return {
        ...item,
        label,
        value,
      };
    });
  }

  /**
   * 加载全部字典数据（GET TaktDictDatas/all，按 dictTypeCode 分组）
   */
  async function loadAllDictDataAsync(): Promise<void> {
    if (loaded.value) {
      return;
    }

    if (loadingPromise) {
      return loadingPromise;
    }

    loading.value = true;

    loadingPromise = (async () => {
      try {
        const items = await getDictDataAll();

        const grouped: Record<string, TaktSelectOption[]> = {};

        items
          .map(mapDictDataToOption)
          .sort((a, b) => a.sortOrder - b.sortOrder)
          .forEach((option) => {
            const typeCode = option.dictTypeCode;

            if (!typeCode) {
              return;
            }

            if (!grouped[typeCode]) {
              grouped[typeCode] = [];
            }

            grouped[typeCode].push(option);
          });

        dictMap.value = grouped;
        loaded.value = true;
      } catch (error) {
        dictLogger.warn('加载字典数据失败', { action: 'loadAllDictData' }, error);
      } finally {
        loading.value = false;
        loadingPromise = null;
      }
    })();

    return loadingPromise;
  }

  /**
   * 重置缓存（登出等场景）
   */
  function resetDictData(): void {
    dictMap.value = {};
    loaded.value = false;
    loading.value = false;
    loadingPromise = null;
  }

  return {
    dictMap,
    loading,
    loaded,
    isLoaded,
    getDictOption,
    getDictOptionsForSelect,
    loadAllDictDataAsync,
    resetDictData,
  };
});
