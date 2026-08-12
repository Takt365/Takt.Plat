---
name: 17-field-length
description: >-
  Takt 通用字段长度规范（Domain SugarColumn Length：物料/单据/工厂/工序/机种等）。
  用于新建实体、改 Length、代码生成后审阅、统一编码长度时。
---

# 通用字段长度

完整规范：`.cursor/rules/17-field-length.mdc`

## 总则

- 无专项 → **`Length = 40`**
- ❌ 禁止无说明写 50/100；同义字段全库同长

## 速查（高频）

| 语义 | Length |
|------|--------|
| 租户 / 公司 / 工厂 | 3 / 4 / 4 |
| 币种 `CurrencyCode`（含 from/to/scale/condition 等） | 3 |
| 物料编码 / 短描述 / 规格·型号 / 长描述 | 20 / 40 / 70 / 200 |
| **特例** 制造商/销售商物料编码 `manufacturer_material_code` / `seller_material_code` | **40**（❌ 勿套物料编码 20） |
| 工作中心编码 / 描述 / 工单多值汇总 | 10 / 70 / 500（production_order.work_center） |
| 工序编码 / 名称 | 4 / 70 |
| 工艺路线编码 | 8 |
| 生产工单·APS·计划订单 | 12 |
| MDS·销售预测·MPS·MRP | 20 |
| 生产类别 / 班组 / 工单类别 | 4 / 8 / 4 |
| 机种 | 40 |
| 销售/采购订单·报价·询价·价格 | 20 |
| 客户 / 供应商 | 10 / 10 |
| 设备编码 | 18 |
| 入/出库单编码（`inbound_code` / `outbound_code`） | 10 |
| Quality 域编码 / 描述 | 20 / 70 |

**命名**：字符串业务编码一律 `XxxCode`，❌ 禁止 `XxxNo`（整数班次/工步/版本除外）。

## 自检

```
- [ ] 新字段已对照 17-field-length 专项表
- [ ] 冗余码/来源码与主表同 Length
- [ ] 无无说明的 Length=50/100
```
