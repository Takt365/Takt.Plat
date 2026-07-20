// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktProductionEquipmentSeedData.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产设备主数据种子（C100/2300 PCBA/SMT 线 17 台全字段；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 生产设备主数据种子（工厂 C100 / 公司 2300：PCBA/SMT 线 17 台）
/// </summary>
public class TaktProductionEquipmentSeedData : ITaktSeedDataCoordinator
{
    private static readonly HashSet<string> TargetPlantCodes = new(StringComparer.Ordinal) { "C100" };

    /// <summary>
    /// 执行顺序（标准生产稼动率种子之后）
    /// </summary>
    public int Order => 492;

    /// <summary>
    /// 初始化生产设备主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化生产设备主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过生产设备主数据种子");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktProductionEquipment>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过生产设备种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过生产设备种子", tenantCode);
            return (0, 0);
        }
        var templates = GetStandardProductionEquipments();
        var insertCount = 0;
        var updateCount = 0;
        foreach (var company in orderedCompanies)
        {
            string plantCode;
            try
            {
                plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            }
            catch (InvalidOperationException ex)
            {
                TaktLogger.Warning("公司 {CompanyCode} 未映射工厂，跳过生产设备种子: {Message}", company.CompanyCode, ex.Message);
                continue;
            }
            if (!TargetPlantCodes.Contains(plantCode))
            {
                continue;
            }
            foreach (var seed in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateProductionEquipmentAsync(
                    repository, tenantCode, company.CompanyCode, plantCode, seed);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information("生产设备主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// C100（东莞/公司 2300）PCBA/SMT 线 17 台设备（按工艺顺序，全字段）
    /// </summary>
    private static List<TaktProductionEquipment> GetStandardProductionEquipments()
    {
        return
        [
            // 1 Panasonic SP18P-L
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "PRI-SP18PL-01",
                ProductionEquipmentName = "松下 SP18P-L 全自动锡膏印刷机",
                EquipmentCategory = 13,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "SP18P-L",
                ModelNo = "NM-EJP1A",
                SerialNo = "1P8V0336",
                EquipmentSpecification = "基板L50×W50～L510×W460mm；厚0.3～4mm；循环8s+印刷；重复定位±12.5μm；丝网框736×736mm",
                ManufacturingDate = new DateTime(2009, 1, 1),
                CommissioningDate = new DateTime(2009, 6, 15),
                StdCycleTimeSeconds = 27m,
                StdMinutesPerUnit = 0.45m,
                StdMinutesPerCycle = 0.45m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 102m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 25m,
                MoldChangeMinutes = 20m,
                MaterialChangeMinutes = 10m,
                MtbfHours = 4000m,
                MttrHours = 2m,
                RepeatabilityAccuracy = 0.0125m,
                PressureControlAccuracy = 2m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.0125m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 1.4m,
                AirConsumptionLpm = 30m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 500m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT1-印刷",
                SortOrder = 1,
                ProductionEquipmentStatus = 1,
            },
            // 2 Panasonic CM602-L（铭牌：机型名 CM602-L / 型号 NM-EJM8A / 序列号 11FV2851 / 制造 20090717）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "SMT-CM602L-01",
                ProductionEquipmentName = "松下 CM602-L 高速多功能贴片机",
                EquipmentCategory = 11,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "CM602-L",
                ModelNo = "NM-EJM8A",
                SerialNo = "11FV2851",
                EquipmentSpecification = "自动生产设备；电源200/220/380/400/420/480V 3Φ 50/60Hz 4.0kVA；供气≤0.78MPa；工作气压0.54MPa 170L/min；短路2.5kA",
                ManufacturingDate = new DateTime(2009, 7, 17),
                CommissioningDate = new DateTime(2009, 11, 1),
                StdCycleTimeSeconds = 7.2m,
                StdMinutesPerUnit = 0.12m,
                StdMinutesPerCycle = 0.12m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 382.5m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 60m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 25m,
                MtbfHours = 5000m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.04m,
                ProcessCapabilityCpk = 1.0m,
                MaxDimensionalTolerance = 0.04m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 4.0m,
                AirConsumptionLpm = 170m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 78m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT1-贴片",
                SortOrder = 2,
                ProductionEquipmentStatus = 1,
            },
            // 3 Tamura TAP30-407PM
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "REF-TAP30407-01",
                ProductionEquipmentName = "田村 TAP30-407PM 回流焊炉",
                EquipmentCategory = 14,
                Manufacturer = "田村",
                EquipmentBrand = "Tamura",
                MachineType = "TAP30-407PM",
                ModelNo = null,
                SerialNo = "T407PM0812",
                EquipmentSpecification = "7温区1冷却；PCB Min50×50 Max300×330mm；链速0.3～1.7m/min；26kVA；边夹传送",
                ManufacturingDate = new DateTime(2008, 8, 1),
                CommissioningDate = new DateTime(2009, 1, 20),
                StdCycleTimeSeconds = 21m,
                StdMinutesPerUnit = 0.35m,
                StdMinutesPerCycle = 0.35m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 131.14m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 15m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 6000m,
                MttrHours = 4m,
                TemperatureControlAccuracy = 5m,
                ProcessCapabilityCpk = 1.33m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 380m,
                RatedPowerKw = 26m,
                CoolingWaterFlowLpm = 5m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 35m,
                OperatingTempRange = "150～280℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 65m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 2000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT1-回流",
                SortOrder = 3,
                ProductionEquipmentStatus = 1,
            },
            // 4 Marantz U22XHML-650（马兰士电子在线 AOI）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AOI-U22XHML-01",
                ProductionEquipmentName = "马兰士电子 U22XHML-650 在线AOI",
                EquipmentCategory = 2,
                Manufacturer = "马兰士",
                EquipmentBrand = "Marantz",
                MachineType = "U22XHML-650",
                ModelNo = null,
                SerialNo = "U22XH1405",
                EquipmentSpecification = "在线式AOI；U22XH ML 系列；基板范围650mm级；元件/焊点外观检测",
                ManufacturingDate = new DateTime(2014, 5, 1),
                CommissioningDate = new DateTime(2014, 9, 1),
                StdCycleTimeSeconds = 33m,
                StdMinutesPerUnit = 0.55m,
                StdMinutesPerCycle = 0.55m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 83.45m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 20m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 5000m,
                MttrHours = 2m,
                RepeatabilityAccuracy = 0.02m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.02m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 220m,
                RatedPowerKw = 2.5m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 60m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT1-AOI",
                SortOrder = 4,
                ProductionEquipmentStatus = 1,
            },
            // 5 Panasonic SP18P-L
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "PRI-SP18PL-02",
                ProductionEquipmentName = "松下 SP18P-L 全自动锡膏印刷机",
                EquipmentCategory = 13,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "SP18P-L",
                ModelNo = "NM-EJP1A",
                SerialNo = "1P8V0412",
                EquipmentSpecification = "基板L50×W50～L510×W460mm；厚0.3～4mm；循环8s+印刷；重复定位±12.5μm；丝网框736×736mm",
                ManufacturingDate = new DateTime(2009, 3, 1),
                CommissioningDate = new DateTime(2010, 2, 1),
                StdCycleTimeSeconds = 27m,
                StdMinutesPerUnit = 0.45m,
                StdMinutesPerCycle = 0.45m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 102m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 25m,
                MoldChangeMinutes = 20m,
                MaterialChangeMinutes = 10m,
                MtbfHours = 4000m,
                MttrHours = 2m,
                RepeatabilityAccuracy = 0.0125m,
                PressureControlAccuracy = 2m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.0125m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 1.4m,
                AirConsumptionLpm = 30m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 500m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-印刷",
                SortOrder = 5,
                ProductionEquipmentStatus = 1,
            },
            // 6 神州视觉 ALD-ST3-450（在线式 3D SPI；铭牌）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "SPI-ALDST3-01",
                ProductionEquipmentName = "神州视觉 ALD-ST3-450 在线3D SPI",
                EquipmentCategory = 16,
                Manufacturer = "东莞市神州视觉科技有限公司",
                EquipmentBrand = "ALeader",
                MachineType = "全自动锡膏印刷检测设备",
                ModelNo = "ALD-ST3-450",
                SerialNo = "01450180237",
                EquipmentSpecification = "Solder Paste Inspection；AC230V 6.5A 50/60Hz 1.5kVA；气压0.2～0.8MPa；外形L1300×W966×H1597mm；重量850kg",
                ManufacturingDate = new DateTime(2018, 8, 1),
                CommissioningDate = new DateTime(2018, 10, 15),
                StdCycleTimeSeconds = 18m,
                StdMinutesPerUnit = 0.30m,
                StdMinutesPerCycle = 0.30m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 153m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 15m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 5500m,
                MttrHours = 2m,
                RepeatabilityAccuracy = 0.01m,
                ProcessCapabilityCpk = 1.67m,
                MaxDimensionalTolerance = 0.01m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 230m,
                RatedPowerKw = 1.5m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 58m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-SPI",
                SortOrder = 6,
                ProductionEquipmentStatus = 1,
            },
            // 7 Panasonic CM602-L
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "SMT-CM602L-02",
                ProductionEquipmentName = "松下 CM602-L 高速多功能贴片机",
                EquipmentCategory = 11,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "CM602-L",
                ModelNo = "NM-EJM8A",
                SerialNo = "1C6M2241",
                EquipmentSpecification = "12吸嘴高速头100000CPH(0.036s/chip)；贴装精度±40μm/chip；PCB L510×W460mm；气压0.49MPa 170L/min",
                ManufacturingDate = new DateTime(2010, 1, 15),
                CommissioningDate = new DateTime(2010, 5, 1),
                StdCycleTimeSeconds = 7.2m,
                StdMinutesPerUnit = 0.12m,
                StdMinutesPerCycle = 0.12m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 382.5m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 60m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 25m,
                MtbfHours = 5000m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.04m,
                ProcessCapabilityCpk = 1.0m,
                MaxDimensionalTolerance = 0.04m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 4.0m,
                AirConsumptionLpm = 170m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 78m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-贴片",
                SortOrder = 7,
                ProductionEquipmentStatus = 1,
            },
            // 8 Panasonic DT401-F（铭牌：机型名 DT401-F / 型号 KXF-E64C / 序列号 125V3535 / 制造 20090717）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "SMT-DT401F-01",
                ProductionEquipmentName = "松下 DT401-F 多功能贴片机",
                EquipmentCategory = 11,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "DT401-F",
                ModelNo = "KXF-E64C",
                SerialNo = "125V3535",
                EquipmentSpecification = "自动生产设备；电源200/220/380/400/420/480V 3Φ 50/60Hz 1.5kVA；供气≤0.78MPa；工作气压0.54MPa 150L/min；短路2.5kA",
                ManufacturingDate = new DateTime(2009, 7, 17),
                CommissioningDate = new DateTime(2009, 11, 1),
                StdCycleTimeSeconds = 10.8m,
                StdMinutesPerUnit = 0.18m,
                StdMinutesPerCycle = 0.18m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 255m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 45m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 20m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                ProcessCapabilityCpk = 1.0m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 1.5m,
                AirConsumptionLpm = 150m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 75m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-贴片2",
                SortOrder = 8,
                ProductionEquipmentStatus = 1,
            },
            // 9 Tamura TNP50-572EM（铭牌：MODEL TNP50-572EM / NO. 5827 / DATE 2008.7 / POWER AC200V 41kVA / WEIGHT 2000kg）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "REF-TNP50572-01",
                ProductionEquipmentName = "田村 TNP50-572EM 氮气回流焊装置",
                EquipmentCategory = 14,
                Manufacturer = "田村",
                EquipmentBrand = "Tamura",
                MachineType = "TNP50-572EM",
                ModelNo = null,
                SerialNo = "5827",
                EquipmentSpecification = "AC200V 41kVA；整机重量2000kg；日本制造；氮气回流焊",
                ManufacturingDate = new DateTime(2008, 7, 1),
                CommissioningDate = new DateTime(2008, 11, 15),
                StdCycleTimeSeconds = 19.2m,
                StdMinutesPerUnit = 0.32m,
                StdMinutesPerCycle = 0.32m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 143.44m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 20m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 6000m,
                MttrHours = 4m,
                TemperatureControlAccuracy = 5m,
                ProcessCapabilityCpk = 1.33m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 41m,
                CoolingWaterFlowLpm = 8m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 40m,
                OperatingTempRange = "150～300℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 65m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 2000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-回流",
                SortOrder = 9,
                ProductionEquipmentStatus = 1,
            },
            // 10 Aleader ALD8710S
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AOI-ALD8710S-01",
                ProductionEquipmentName = "神州视觉 ALD8710S 在线AOI",
                EquipmentCategory = 2,
                Manufacturer = "神州视觉",
                EquipmentBrand = "ALeader",
                MachineType = "ALD8710S",
                ModelNo = null,
                SerialNo = "ALD87101602",
                EquipmentSpecification = "在线式2D AOI；炉后焊点/元件外观检测；高分辨率CCD",
                ManufacturingDate = new DateTime(2016, 2, 1),
                CommissioningDate = new DateTime(2016, 5, 20),
                StdCycleTimeSeconds = 33m,
                StdMinutesPerUnit = 0.55m,
                StdMinutesPerCycle = 0.55m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 83.45m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 20m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 5000m,
                MttrHours = 2m,
                RepeatabilityAccuracy = 0.02m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.02m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 220m,
                RatedPowerKw = 2m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 0m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 60m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "SMT2-AOI",
                SortOrder = 10,
                ProductionEquipmentStatus = 1,
            },
            // 11 Panasonic AV-B（全自动卧式/轴向插件机，非 AOI）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AI-AVB-01",
                ProductionEquipmentName = "松下 AV-B 全自动卧式插件机",
                EquipmentCategory = 12,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "AV-B",
                ModelNo = null,
                SerialNo = "AVB070601",
                EquipmentSpecification = "全自动卧式插件（AI）；轴向/跳线元件插入；与 AV/AVK 系列同族",
                ManufacturingDate = new DateTime(2007, 6, 1),
                CommissioningDate = new DateTime(2008, 1, 15),
                StdCycleTimeSeconds = 0.15m,
                StdMinutesPerUnit = 0.0025m,
                StdMinutesPerCycle = 0.0025m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 20400m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 45m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 20m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 4m,
                AirConsumptionLpm = 120m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 15m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                StorageLocation = "AI1-插件",
                SortOrder = 11,
                ProductionEquipmentStatus = 1,
            },
            // 12 Panasonic AVK-3（全自动卧式插件机，非 AOI）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AI-AVK3-01",
                ProductionEquipmentName = "松下 AVK-3 全自动卧式插件机",
                EquipmentCategory = 12,
                Manufacturer = "松下生产科技株式会社",
                EquipmentBrand = "Panasonic",
                MachineType = "AVK-3",
                ModelNo = null,
                SerialNo = "AVK3080401",
                EquipmentSpecification = "全自动卧式插件（AI）；AVK 系列高速轴向插件；插入节拍约0.15s/点",
                ManufacturingDate = new DateTime(2008, 4, 1),
                CommissioningDate = new DateTime(2008, 9, 1),
                StdCycleTimeSeconds = 0.15m,
                StdMinutesPerUnit = 0.0025m,
                StdMinutesPerCycle = 0.0025m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 20400m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 45m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 20m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 4m,
                AirConsumptionLpm = 120m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 15m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                StorageLocation = "AI2-插件",
                SortOrder = 12,
                ProductionEquipmentStatus = 1,
            },
            // 13 TDK VC-7A（径向/立式自动插件机，非 SPI）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AI-VC7A-01",
                ProductionEquipmentName = "TDK VC-7A 立式径向自动插件机",
                EquipmentCategory = 12,
                Manufacturer = "东电化",
                EquipmentBrand = "TDK",
                MachineType = "VC-7A",
                ModelNo = null,
                SerialNo = "VC7A060501",
                EquipmentSpecification = "Radial Lead Inserter；约9200pcs/h（约0.4s/个）；跨距2.5/5.0mm（可扩7.5mm）；头转0°/+90°（可选四方向反转）；电阻/电容/二极管/三极管/LED；PCB约400×300mm；料站40/80/120；Sequencer链夹送料；切脚/弯脚（N型）",
                ManufacturingDate = new DateTime(2006, 5, 1),
                CommissioningDate = new DateTime(2006, 11, 1),
                StdCycleTimeSeconds = 0.4m,
                StdMinutesPerUnit = 0.0067m,
                StdMinutesPerCycle = 0.0067m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 7038m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 45m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 20m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 3m,
                AirConsumptionLpm = 120m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 15m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                StorageLocation = "AI1-径向",
                SortOrder = 13,
                ProductionEquipmentStatus = 1,
            },
            // 14 TDK VC-7AT（铭牌：MODEL VC-7AT / TYPE VC-7G80RT / MFG.NO AV-7T 091 / 制造 1997-04）
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "AI-VC7AT-01",
                ProductionEquipmentName = "TDK VC-7AT 立式径向自动插件机",
                EquipmentCategory = 12,
                Manufacturer = "东电化",
                EquipmentBrand = "TDK",
                MachineType = "VC-7AT",
                ModelNo = "VC-7G80RT",
                SerialNo = "AV-7T 091",
                EquipmentSpecification = "avisert；3Φ AC200V 50/60Hz 2.5A；整机重量1200kg；台湾制造",
                ManufacturingDate = new DateTime(1997, 4, 1),
                CommissioningDate = new DateTime(1998, 1, 15),
                StdCycleTimeSeconds = 0.4m,
                StdMinutesPerUnit = 0.0067m,
                StdMinutesPerCycle = 0.0067m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 7038m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 45m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 20m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 200m,
                RatedPowerKw = 0.9m,
                AirConsumptionLpm = 120m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 15m,
                OperatingTempRange = "18～28℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 72m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1000m,
                CumulativeRunHours = 0m,
                StorageLocation = "AI2-径向",
                SortOrder = 14,
                ProductionEquipmentStatus = 1,
            },
            // 15 JYI DIANN JT-550
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "SEL-JT550-01",
                ProductionEquipmentName = "精诚智焊 JT-550 选择性波峰焊机",
                EquipmentCategory = 15,
                Manufacturer = "精诚智焊",
                EquipmentBrand = "JYI DIANN",
                MachineType = "JT-550",
                ModelNo = null,
                SerialNo = "JT5501708",
                EquipmentSpecification = "选择性波峰焊；局部焊接；PCB Max550mm级",
                ManufacturingDate = new DateTime(2017, 8, 1),
                CommissioningDate = new DateTime(2017, 11, 15),
                StdCycleTimeSeconds = 210m,
                StdMinutesPerUnit = 3.5m,
                StdMinutesPerCycle = 3.5m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 13.12m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 35m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 15m,
                MtbfHours = 4500m,
                MttrHours = 3m,
                RepeatabilityAccuracy = 0.05m,
                TemperatureControlAccuracy = 3m,
                ProcessCapabilityCpk = 1.33m,
                MaxDimensionalTolerance = 0.05m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 380m,
                RatedPowerKw = 10m,
                AirConsumptionLpm = 60m,
                OperatorCount = 1,
                IsCriticalResource = 0,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 25m,
                OperatingTempRange = "250～300℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 70m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 1500m,
                CumulativeRunHours = 0m,
                StorageLocation = "MI1-选焊",
                SortOrder = 15,
                ProductionEquipmentStatus = 1,
            },
            // 16 ANDA JN-350BS
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "REF-JN350BS-01",
                ProductionEquipmentName = "安达 JN-350BS 氮气回流焊炉",
                EquipmentCategory = 14,
                Manufacturer = "安达",
                EquipmentBrand = "ANDA",
                MachineType = "JN-350BS",
                ModelNo = null,
                SerialNo = "JN3501503",
                EquipmentSpecification = "氮气回流焊；8～10温区；350mm级带宽；N2气氛",
                ManufacturingDate = new DateTime(2015, 3, 1),
                CommissioningDate = new DateTime(2015, 7, 1),
                StdCycleTimeSeconds = 19.8m,
                StdMinutesPerUnit = 0.33m,
                StdMinutesPerCycle = 0.33m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 139.09m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 20m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 6000m,
                MttrHours = 4m,
                TemperatureControlAccuracy = 5m,
                ProcessCapabilityCpk = 1.33m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 380m,
                RatedPowerKw = 28m,
                CoolingWaterFlowLpm = 6m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 40m,
                OperatingTempRange = "150～300℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 65m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 2000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "MI1-回流",
                SortOrder = 16,
                ProductionEquipmentStatus = 1,
            },
            // 17 Kelongwei FL-ADS300
            new TaktProductionEquipment
            {
                ProductionEquipmentCode = "REF-FLADS300-01",
                ProductionEquipmentName = "科隆威 FL-ADS300 氮气回流焊炉",
                EquipmentCategory = 14,
                Manufacturer = "科隆威",
                EquipmentBrand = "Kelongwei",
                MachineType = "FL-ADS300",
                ModelNo = null,
                SerialNo = "FLADS1606",
                EquipmentSpecification = "氮气回流焊；ADS温控；300mm级带宽；N2气氛",
                ManufacturingDate = new DateTime(2016, 6, 1),
                CommissioningDate = new DateTime(2016, 10, 1),
                StdCycleTimeSeconds = 19.8m,
                StdMinutesPerUnit = 0.33m,
                StdMinutesPerCycle = 0.33m,
                TheoreticalSpm = 0m,
                TheoreticalCycleTimeSeconds = 0m,
                StdEquipmentHourlyCapacity = 139.09m,
                AvailabilityRate = 0.85m,
                PerformanceRate = 0.90m,
                SetupMinutes = 20m,
                MoldChangeMinutes = 0m,
                MaterialChangeMinutes = 0m,
                MtbfHours = 6000m,
                MttrHours = 4m,
                TemperatureControlAccuracy = 5m,
                ProcessCapabilityCpk = 1.33m,
                CavityCount = 1,
                QuickMoldChange = 0,
                RatedVoltage = 380m,
                RatedPowerKw = 24m,
                CoolingWaterFlowLpm = 5m,
                OperatorCount = 1,
                IsCriticalResource = 1,
                ParallelCapacity = 1,
                AllowRushOrder = 1,
                WarmupMinutes = 35m,
                OperatingTempRange = "150～300℃",
                OperatingHumidityRange = "40～70%RH",
                NoiseLevelDb = 65m,
                EquipmentRunStatus = 1,
                MaintenanceIntervalHours = 2000m,
                CumulativeRunHours = 0m,
                InterfaceType = "SMEMA",
                StorageLocation = "MI2-回流",
                SortOrder = 17,
                ProductionEquipmentStatus = 1,
            },
        ];
    }

    /// <summary>
    /// 创建或更新生产设备（按编码幂等，全字段同步）
    /// </summary>
    private static async Task<(TaktProductionEquipment Equipment, int InsertCount, int UpdateCount)> CreateOrUpdateProductionEquipmentAsync(
        ITaktCompanySeedRepository<TaktProductionEquipment> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        TaktProductionEquipment seed)
    {
        var equipment = await repository.FirstAsync(e =>
            e.TenantCode == tenantCode
            && e.CompanyCode == companyCode
            && e.PlantCode == plantCode
            && e.ProductionEquipmentCode == seed.ProductionEquipmentCode);
        if (equipment == null)
        {
            equipment = new TaktProductionEquipment
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
            };
            CopySeedFields(equipment, seed);
            equipment = await repository.CreateAsync(equipment);
            return (equipment, 1, 0);
        }
        CopySeedFields(equipment, seed);
        await repository.UpdateAsync(equipment);
        return (equipment, 0, 1);
    }

    /// <summary>
    /// 将种子模板业务字段写入实体（不含租户/公司/工厂/主键）
    /// </summary>
    private static void CopySeedFields(TaktProductionEquipment target, TaktProductionEquipment seed)
    {
        target.ProductionEquipmentCode = seed.ProductionEquipmentCode;
        target.ProductionEquipmentName = seed.ProductionEquipmentName;
        target.EquipmentCategory = seed.EquipmentCategory;
        target.Manufacturer = seed.Manufacturer;
        target.EquipmentBrand = seed.EquipmentBrand;
        target.MachineType = seed.MachineType;
        target.ModelNo = seed.ModelNo;
        target.SerialNo = seed.SerialNo;
        target.EquipmentSpecification = seed.EquipmentSpecification;
        target.ManufacturingDate = seed.ManufacturingDate;
        target.CommissioningDate = seed.CommissioningDate;
        target.DecommissioningDate = seed.DecommissioningDate;
        target.ScrapDate = seed.ScrapDate;
        target.StdCycleTimeSeconds = seed.StdCycleTimeSeconds;
        target.StdMinutesPerUnit = seed.StdMinutesPerUnit;
        target.StdMinutesPerCycle = seed.StdMinutesPerCycle;
        target.TheoreticalSpm = seed.TheoreticalSpm;
        target.TheoreticalCycleTimeSeconds = seed.TheoreticalCycleTimeSeconds;
        target.StdEquipmentHourlyCapacity = seed.StdEquipmentHourlyCapacity;
        target.AvailabilityRate = seed.AvailabilityRate;
        target.PerformanceRate = seed.PerformanceRate;
        target.SetupMinutes = seed.SetupMinutes;
        target.MoldChangeMinutes = seed.MoldChangeMinutes;
        target.MaterialChangeMinutes = seed.MaterialChangeMinutes;
        target.MtbfHours = seed.MtbfHours;
        target.MttrHours = seed.MttrHours;
        target.RepeatabilityAccuracy = seed.RepeatabilityAccuracy;
        target.ShutHeightAccuracy = seed.ShutHeightAccuracy;
        target.InjectionAccuracy = seed.InjectionAccuracy;
        target.TemperatureControlAccuracy = seed.TemperatureControlAccuracy;
        target.PressureControlAccuracy = seed.PressureControlAccuracy;
        target.ProcessCapabilityCpk = seed.ProcessCapabilityCpk;
        target.MaxDimensionalTolerance = seed.MaxDimensionalTolerance;
        target.MaxMoldDimension = seed.MaxMoldDimension;
        target.MinMoldDimension = seed.MinMoldDimension;
        target.MaxMoldWeightTon = seed.MaxMoldWeightTon;
        target.MoldHeightRange = seed.MoldHeightRange;
        target.EjectionType = seed.EjectionType;
        target.EjectionStrokeMm = seed.EjectionStrokeMm;
        target.CavityCount = seed.CavityCount;
        target.QuickMoldChange = seed.QuickMoldChange;
        target.MoldCode = seed.MoldCode;
        target.RatedTonnage = seed.RatedTonnage;
        target.ClampingForceKn = seed.ClampingForceKn;
        target.MaxStrokeMm = seed.MaxStrokeMm;
        target.OpenStrokeMm = seed.OpenStrokeMm;
        target.PlatenSize = seed.PlatenSize;
        target.RatedVoltage = seed.RatedVoltage;
        target.RatedPowerKw = seed.RatedPowerKw;
        target.AirConsumptionLpm = seed.AirConsumptionLpm;
        target.CoolingWaterFlowLpm = seed.CoolingWaterFlowLpm;
        target.OperatorCount = seed.OperatorCount;
        target.IsCriticalResource = seed.IsCriticalResource;
        target.ParallelCapacity = seed.ParallelCapacity;
        target.AllowRushOrder = seed.AllowRushOrder;
        target.WarmupMinutes = seed.WarmupMinutes;
        target.OperatingTempRange = seed.OperatingTempRange;
        target.OperatingHumidityRange = seed.OperatingHumidityRange;
        target.NoiseLevelDb = seed.NoiseLevelDb;
        target.EquipmentRunStatus = seed.EquipmentRunStatus;
        target.MaintenanceIntervalHours = seed.MaintenanceIntervalHours;
        target.CumulativeRunHours = seed.CumulativeRunHours;
        target.InterfaceType = seed.InterfaceType;
        target.StorageLocation = seed.StorageLocation;
        target.EquipmentAdministrator = seed.EquipmentAdministrator;
        target.SortOrder = seed.SortOrder;
        target.ProductionEquipmentStatus = seed.ProductionEquipmentStatus;
    }
}
