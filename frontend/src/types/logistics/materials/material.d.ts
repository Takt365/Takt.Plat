// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * Takt全局物料实体（租户内共享；字段对齐 SAP MARA；多语言描述见子表 TaktMaterialDescription / SAP MAKT）
 * 对应前端 TaktMaterialDto
 * 继承 TaktTenantDtoBase
 * 对应前端 Material
 * @description 对应后端 TaktMaterialDto
 */
export interface Material extends TenantDtoBase {
  /**
   * MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialId: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）
   */
  validFromDate?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 知识产权名称（填充字段）
   */
  intellectualPropertyName?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * 特性转换名称（填充字段）
   */
  characteristicConversionName?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 品牌（SAP MARA.BRAND_名称（填充字段）
   */
  brandName?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus: number;

  /**
   * 多语言描述列表（主子表关系；对齐 SAP MAKT） （子表：TaktMaterialDescription）
   */
  descriptions?: MaterialDescription[];

}


/**
 * Material 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialQuery
 * @description 对应后端 TaktMaterialQueryDto
 */
export interface MaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode?: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType?: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector?: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup?: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）（范围查询-开始）
   */
  validFromDateStart?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）（范围查询-结束）
   */
  validFromDateEnd?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）（范围查询-开始）
   */
  crossPlantStatusValidFromStart?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）（范围查询-结束）
   */
  crossPlantStatusValidFromEnd?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）（范围查询-开始）
   */
  crossDistributionStatusValidFromStart?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）（范围查询-结束）
   */
  crossDistributionStatusValidFromEnd?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Material DTO
 * 对应前端 MaterialCreate
 * @description 对应后端 TaktMaterialCreateDto
 */
export interface MaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）
   */
  validFromDate?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus: number;

  /**
   * 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
   */
  descriptions?: MaterialDescriptionCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Material DTO
 * 继承 TaktMaterialCreateDto，添加 MaterialId 字段
 * 对应前端 MaterialUpdate
 * @description 对应后端 TaktMaterialUpdateDto
 */
export interface MaterialUpdate extends MaterialCreate {
  /**
   * MaterialID（标识要更新的实体）
   */
  materialId: string;

  /**
   * 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
   */
  descriptions?: any;

}


/**
 * Material 状态更新 DTO
 * 对应前端 MaterialStatus
 * @description 对应后端 TaktMaterialStatusDto
 */
export interface MaterialStatus {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus: number;

}


/**
 * Material 导入模板行 DTO
 * 对应前端 MaterialTemplate
 * @description 对应后端 TaktMaterialTemplateDto
 */
export interface MaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode?: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType?: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector?: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup?: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）
   */
  validFromDate?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus?: number;

  /**
   * 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
   */
  descriptions?: MaterialDescriptionCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Material 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialImport
 * @description 对应后端 TaktMaterialImportDto
 */
export interface MaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode?: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType?: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector?: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup?: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）
   */
  validFromDate?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus?: number;

  /**
   * 多语言描述列表（主子表关系；对齐 SAP MAKT）（子表，级联保存）
   */
  descriptions?: MaterialDescriptionCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Material 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialExport
 * @description 对应后端 TaktMaterialExportDto
 */
export interface MaterialExport {
  /**
   * MaterialID
   */
  materialId: string;

  /**
   * 物料编码（SAP MARA.MATNR）
   */
  materialCode: string;

  /**
   * 完整维护状态（SAP MARA.VPSTA）
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态（SAP MARA.PSTAT）
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（SAP MARA.LVORM）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（SAP MARA.MTART）
   */
  materialType: string;

  /**
   * 行业领域（SAP MARA.MBRSH）
   */
  industrySector: string;

  /**
   * 物料组（SAP MARA.MATKL）
   */
  materialGroup: string;

  /**
   * 旧物料号（SAP MARA.BISMT）
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（SAP MARA.MEINS）
   */
  baseUnit: string;

  /**
   * 采购订单单位（SAP MARA.BSTME）
   */
  orderUnit?: string;

  /**
   * 单据号（SAP MARA.ZEINR）
   */
  documentNumber?: string;

  /**
   * 单据类型（SAP MARA.ZEIAR）
   */
  documentType?: string;

  /**
   * 单据版本（SAP MARA.ZEIVR）
   */
  documentVersion?: string;

  /**
   * 单据页格式（SAP MARA.ZEIFO）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号（SAP MARA.AESZN）
   */
  documentChangeNumber?: string;

  /**
   * 单据页号（SAP MARA.BLATT）
   */
  documentPageNumber?: string;

  /**
   * 单据页数（SAP MARA.BLANZ）
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘（SAP MARA.FERTH）
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（SAP MARA.FORMT）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格（SAP MARA.GROES）
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）（SAP MARA.WRKST）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述（SAP MARA.NORMT）
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（SAP MARA.LABOR）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（SAP MARA.EKWSL）
   */
  purchasingValueKey?: string;

  /**
   * 毛重（SAP MARA.BRGEW）
   */
  grossWeight?: number;

  /**
   * 净重（SAP MARA.NTGEW）
   */
  netWeight?: number;

  /**
   * 重量单位（SAP MARA.GEWEI）
   */
  weightUnit?: string;

  /**
   * 体积（SAP MARA.VOLUM）
   */
  volume?: number;

  /**
   * 体积单位（SAP MARA.VOLEH）
   */
  volumeUnit?: string;

  /**
   * 容器要求（SAP MARA.BEHVO）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（SAP MARA.RAUBE）
   */
  storageConditions?: string;

  /**
   * 温度条件（SAP MARA.TEMPB）
   */
  temperatureConditions?: string;

  /**
   * 低层码（SAP MARA.DISST）
   */
  lowLevelCode?: string;

  /**
   * 运输组（SAP MARA.TRAGR）
   */
  transportationGroup?: string;

  /**
   * 危险品编码（SAP MARA.STOFF）
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（SAP MARA.SPART）
   */
  division?: string;

  /**
   * 竞争对手（SAP MARA.KUNNR）
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）（SAP MARA.EANNR）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量（SAP MARA.WESCH）
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（SAP MARA.BWVOR）
   */
  procurementRule?: string;

  /**
   * 货源（SAP MARA.BWSCL）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（SAP MARA.SAISO）
   */
  seasonCategory?: string;

  /**
   * 标签类型（SAP MARA.ETIAR）
   */
  labelType?: string;

  /**
   * 标签格式（SAP MARA.ETIFO）
   */
  labelForm?: string;

  /**
   * 已停用字段（SAP MARA.ENTAR）
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC（SAP MARA.EAN11）
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（SAP MARA.NUMTP）
   */
  eanCategory?: string;

  /**
   * 长度（SAP MARA.LAENG）
   */
  length?: number;

  /**
   * 宽度（SAP MARA.BREIT）
   */
  width?: number;

  /**
   * 高度（SAP MARA.HOEHE）
   */
  height?: number;

  /**
   * 长宽高单位（SAP MARA.MEABM）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（SAP MARA.PRDHA）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算（SAP MARA.AEKLK）
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识（SAP MARA.CADKZ）
   */
  cadIndicator?: string;

  /**
   * 采购QM激活（SAP MARA.QMPUR）
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量（SAP MARA.ERGEW）
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（SAP MARA.ERGEI）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积（SAP MARA.ERVOL）
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（SAP MARA.ERVOE）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差（SAP MARA.GEWTO）
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差（SAP MARA.VOLTO）
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位（SAP MARA.VABME）
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别（SAP MARA.KZREV）
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料（SAP MARA.KZKFG）
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（SAP MARA.XCHPF）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（SAP MARA.VHART）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）（SAP MARA.FUELG）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子（SAP MARA.STFAK）
   */
  stackingFactor?: number;

  /**
   * 包装物料组（SAP MARA.MAGRV）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（SAP MARA.BEGRU）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（SAP MARA.DATAB）
   */
  validFromDate?: string;

  /**
   * 季节年份（SAP MARA.SAISJ）
   */
  seasonYear?: string;

  /**
   * 价格带类别（SAP MARA.PLGTP）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM（SAP MARA.MLGUT）
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（SAP MARA.EXTWG）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料（SAP MARA.SATNR）
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（SAP MARA.ATTYP）
   */
  materialCategory?: string;

  /**
   * 联产品标识（SAP MARA.KZKUP）
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识（SAP MARA.KZNFM）
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料（SAP MARA.PMATA）
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（SAP MARA.MSTAE）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（SAP MARA.MSTAV）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（SAP MARA.MSTDE）
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期（SAP MARA.MSTDV）
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（SAP MARA.TAKLV）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（SAP MARA.RBNRM）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命（SAP MARA.MHDRZ）
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命（SAP MARA.MHDHB）
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比（SAP MARA.MHDLP）
   */
  storagePercentage?: number;

  /**
   * 含量单位（SAP MARA.INHME）
   */
  contentUnit?: string;

  /**
   * 净含量（SAP MARA.INHAL）
   */
  netContents?: number;

  /**
   * 比较价格单位（SAP MARA.VPREH）
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（SAP MARA.ETIAG）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量（SAP MARA.INHBR）
   */
  grossContents?: number;

  /**
   * 数量换算方法（SAP MARA.CMETH）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号（SAP MARA.CUOBF）
   */
  internalObjectNumber?: string;

  /**
   * 环境相关（SAP MARA.KZUMW）
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（SAP MARA.KOSCH）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（SAP MARA.SPROF）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格（SAP MARA.NRFHG）
   */
  discountInKind?: string;

  /**
   * 制造商零件号（SAP MARA.MFRPN）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（SAP MARA.MFRNR）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号（SAP MARA.BMATN）
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（SAP MARA.MPROF）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（SAP MARA.KZWSM）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（SAP MARA.SAITY）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（SAP MARA.PROFL）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度（SAP MARA.IHIVI）
   */
  highlyViscous?: string;

  /**
   * 散装/液体（SAP MARA.ILOOS）
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（SAP MARA.SERLV）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装（SAP MARA.KZGVH）
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录（SAP MARA.XGCHP）
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖（SAP MARA.KZEFF）
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（SAP MARA.COMPL）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（SAP MARA.IPRKZ）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（SAP MARA.RDMHD）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分（SAP MARA.PRZUS）
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（SAP MARA.MTPOS_MARA）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料（SAP MARA.BFLME）
   */
  logisticalVariants?: string;

  /**
   * 物料锁定（SAP MARA.MATFI）
   */
  materialLocked?: string;

  /**
   * 配置管理相关（SAP MARA.CMREL）
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型（SAP MARA.BBTYP）
   */
  assortmentListType?: string;

  /**
   * 到期日期类型（SAP MARA.SLED_BBD）
   */
  expirationDateType?: string;

  /**
   * GTIN变式（SAP MARA.GTIN_VARIANT）
   */
  gtinVariant?: string;

  /**
   * 通用物料号（SAP MARA.GENNR）
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料（SAP MARA.RMATP）
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关（SAP MARA.GDS_RELEVANT）
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收（SAP MARA.WEORA）
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（SAP MARA.HUTYP_DFLT）
   */
  standardHuType?: string;

  /**
   * 易被盗（SAP MARA.PILFERABLE）
   */
  pilferable?: string;

  /**
   * 仓储存储条件（SAP MARA.WHSTC）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（SAP MARA.WHMATGR）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（SAP MARA.HNDLCODE）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关（SAP MARA.HAZMAT）
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（SAP MARA.HUTYP）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重（SAP MARA.TARE_VAR）
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量（SAP MARA.MAXC）
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差（SAP MARA.MAXC_TOL）
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度（SAP MARA.MAXL）
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度（SAP MARA.MAXB）
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度（SAP MARA.MAXH）
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（SAP MARA.MAXDIM_UOM）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（SAP MARA.HERKL）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（SAP MARA.MFRGR）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期（SAP MARA.QQTIME）
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（SAP MARA.QQTIMEUOM）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（SAP MARA.QGRP）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（SAP MARA.SERIAL）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（SAP MARA.PS_SMARTFORM）
   */
  formName?: string;

  /**
   * 后勤计量单位（SAP MARA.LOGUNIT）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料（SAP MARA.CWQREL）
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（SAP MARA.CWQPROC）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（SAP MARA.CWQTOLGR）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（SAP MARA.ADPROF）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID（SAP MARA.IPMIPPRODUCT）
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格（SAP MARA.ALLOW_PMAT_IGNO）
   */
  variantPriceAllowed?: string;

  /**
   * 介质（SAP MARA.MEDIUM）
   */
  medium?: string;

  /**
   * 实物商品（SAP MARA.COMMODITY）
   */
  physicalCommodity?: string;

  /**
   * 动物源（SAP MARA.ANIMAL_ORIGIN）
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能（SAP MARA.TEXTILE_COMP_IND）
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（SAP MARA.SGT_CSGR）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（SAP MARA.SGT_COVSA）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（SAP MARA.SGT_STAT）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（SAP MARA.SGT_SCOPE）
   */
  segmentationScope?: string;

  /**
   * 细分相关（SAP MARA.SGT_REL）
   */
  segmentationRelevant?: string;

  /**
   * 时装属性1（SAP MARA.FSH_MG_AT1）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（SAP MARA.FSH_MG_AT2）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（SAP MARA.FSH_MG_AT3）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（SAP MARA.FSH_SEALV）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活（SAP MARA.FSH_SEAIM）
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID（SAP MARA.FSH_SC_MID）
   */
  characteristicConversionId?: string;

  /**
   * ANP代码（SAP MARA.ANP）
   */
  anpCode?: string;

  /**
   * 危险品包装状态（SAP MARA.DG_PACK_STATUS）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理（SAP MARA.MCOND）
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（SAP MARA.RETDELC）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（SAP MARA.LOGLEV_RETO）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号（SAP MARA.NSNID）
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（SAP MARA.IMATN）
   */
  fffClass?: string;

  /**
   * 替代链编码（SAP MARA.PICNUM）
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（SAP MARA.BSTAT）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号（SAP MARA.COLOR_ATINN）
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号（SAP MARA.SIZE1_ATINN）
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号（SAP MARA.SIZE2_ATINN）
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（SAP MARA.COLOR）
   */
  color?: string;

  /**
   * 主尺码（SAP MARA.SIZE1）
   */
  mainSize?: string;

  /**
   * 次尺码（SAP MARA.SIZE2）
   */
  secondSize?: string;

  /**
   * 评估特性值（SAP MARA.FREE_CHAR）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（SAP MARA.CARE_CODE）
   */
  careCode?: string;

  /**
   * 品牌（SAP MARA.BRAND_ID）
   */
  brandId?: string;

  /**
   * 纤维代码1（SAP MARA.FIBER_CODE1）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1（SAP MARA.FIBER_PART1）
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（SAP MARA.FIBER_CODE2）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2（SAP MARA.FIBER_PART2）
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（SAP MARA.FIBER_CODE3）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3（SAP MARA.FIBER_PART3）
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（SAP MARA.FIBER_CODE4）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4（SAP MARA.FIBER_PART4）
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（SAP MARA.FIBER_CODE5）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5（SAP MARA.FIBER_PART5）
   */
  fiberPart5?: string;

  /**
   * 时装等级（SAP MARA.FASHGRD）
   */
  fashionGrade?: string;

  /**
   * 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定；平台启用态，非 SAP MSTAE）
   */
  materialStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

