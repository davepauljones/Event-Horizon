using System;
using System.Data;

namespace Event_Horizon
{
    public class EventHorizonRamsProfile : DataTable
    {
        public EventHorizonRamsProfile()
        {
            this.Columns.Add("ID", typeof(Int32));
            this.Columns["ID"].DefaultValue = 0;
            this.Columns.Add("ProfileName", typeof(string));
            this.Columns["ProfileName"].DefaultValue = string.Empty;
            this.Columns.Add("CreatedByUserID", typeof(Int32));
            this.Columns["CreatedByUserID"].DefaultValue = 0;
            this.Columns.Add("CreatedDateTime", typeof(DateTime));
            this.Columns["CreatedDateTime"].DefaultValue = DateTime.MinValue;
            
            this.Columns.Add("SiteAndEnvironment", typeof(Int32));
            this.Columns["SiteAndEnvironment"].DefaultValue = 0;
            this.Columns.Add("SE_SlipsTripsAndFallsSameLevel", typeof(Int32));
            this.Columns["SE_SlipsTripsAndFallsSameLevel"].DefaultValue = 0;
            this.Columns.Add("SE_FallsFromHeight", typeof(Int32));
            this.Columns["SE_FallsFromHeight"].DefaultValue = 0;
            this.Columns.Add("SE_UnevenGround", typeof(Int32));
            this.Columns["SE_UnevenGround"].DefaultValue = 0;
            this.Columns.Add("SE_PoorLighting", typeof(Int32));
            this.Columns["SE_PoorLighting"].DefaultValue = 0;
            this.Columns.Add("SE_AdverseWeather", typeof(Int32));
            this.Columns["SE_AdverseWeather"].DefaultValue = 0;
            this.Columns.Add("SE_ConfinedSpaces", typeof(Int32));
            this.Columns["SE_ConfinedSpaces"].DefaultValue = 0;
            this.Columns.Add("SE_WorkingNearWater", typeof(Int32));
            this.Columns["SE_WorkingNearWater"].DefaultValue = 0;
            this.Columns.Add("SE_ExcavationsOpenVoids", typeof(Int32));
            this.Columns["SE_ExcavationsOpenVoids"].DefaultValue = 0;

            this.Columns.Add("AccessAndEgress", typeof(Int32));
            this.Columns["AccessAndEgress"].DefaultValue = 0;
            this.Columns.Add("AE_Ladders", typeof(Int32));
            this.Columns["AE_Ladders"].DefaultValue = 0;
            this.Columns.Add("AE_MobileTowers", typeof(Int32));
            this.Columns["AE_MobileTowers"].DefaultValue = 0;
            this.Columns.Add("AE_MEWPs", typeof(Int32));
            this.Columns["AE_MEWPs"].DefaultValue = 0;
            this.Columns.Add("AE_StairsAndWalkways", typeof(Int32));
            this.Columns["AE_StairsAndWalkways"].DefaultValue = 0;
            this.Columns.Add("AE_RestrictedAccessAreas", typeof(Int32));
            this.Columns["AE_RestrictedAccessAreas"].DefaultValue = 0;
            
            this.Columns.Add("PlantToolsAndEquipmnent", typeof(Int32));
            this.Columns["PlantToolsAndEquipmnent"].DefaultValue = 0;
            this.Columns.Add("PTE_HandTools", typeof(Int32));
            this.Columns["PTE_HandTools"].DefaultValue = 0;
            this.Columns.Add("PTE_PowerTools", typeof(Int32));
            this.Columns["PTE_PowerTools"].DefaultValue = 0;
            this.Columns.Add("PTE_AbrasiveWheels", typeof(Int32));
            this.Columns["PTE_AbrasiveWheels"].DefaultValue = 0;
            this.Columns.Add("PTE_CuttingEquipment", typeof(Int32));
            this.Columns["PTE_CuttingEquipment"].DefaultValue = 0;
            this.Columns.Add("PTE_LiftingEquipment", typeof(Int32));
            this.Columns["PTE_LiftingEquipment"].DefaultValue = 0;
            this.Columns.Add("PTE_PlantMovement", typeof(Int32));
            this.Columns["PTE_PlantMovement"].DefaultValue = 0;
            this.Columns.Add("PTE_CompressedAir", typeof(Int32));
            this.Columns["PTE_CompressedAir"].DefaultValue = 0;
            this.Columns.Add("PTE_GeneratorsAndCompressors", typeof(Int32));
            this.Columns["PTE_GeneratorsAndCompressors"].DefaultValue = 0;

            this.Columns.Add("ManualHandling", typeof(Int32));
            this.Columns["ManualHandling"].DefaultValue = 0;
            this.Columns.Add("MH_Lifting", typeof(Int32));
            this.Columns["MH_Lifting"].DefaultValue = 0;
            this.Columns.Add("MH_Carrying", typeof(Int32));
            this.Columns["MH_Carrying"].DefaultValue = 0;
            this.Columns.Add("MH_PushingPulling", typeof(Int32));
            this.Columns["MH_PushingPulling"].DefaultValue = 0;
            this.Columns.Add("MH_AwkwardLoads", typeof(Int32));
            this.Columns["MH_AwkwardLoads"].DefaultValue = 0;
            this.Columns.Add("MH_RepetitiveHandling", typeof(Int32));
            this.Columns["MH_RepetitiveHandling"].DefaultValue = 0;

            this.Columns.Add("Electrical", typeof(Int32));
            this.Columns["Electrical"].DefaultValue = 0;
            this.Columns.Add("E_LiveElectricalWork", typeof(Int32));
            this.Columns["E_LiveElectricalWork"].DefaultValue = 0;
            this.Columns.Add("E_TemporaryPowerSupplies", typeof(Int32));
            this.Columns["E_TemporaryPowerSupplies"].DefaultValue = 0;
            this.Columns.Add("E_PortableElectricalEquipment", typeof(Int32));
            this.Columns["E_PortableElectricalEquipment"].DefaultValue = 0;
            this.Columns.Add("E_OverheadServices", typeof(Int32));
            this.Columns["E_OverheadServices"].DefaultValue = 0;
            this.Columns.Add("E_UndergroundServices", typeof(Int32));
            this.Columns["E_UndergroundServices"].DefaultValue = 0;
            this.Columns.Add("E_IsolationAndLockoff", typeof(Int32));
            this.Columns["E_IsolationAndLockoff"].DefaultValue = 0;
            
            this.Columns.Add("SubstancesAndCOSHH", typeof(Int32));
            this.Columns["SubstancesAndCOSHH"].DefaultValue = 0;
            this.Columns.Add("SC_FuelsDieselPetrol", typeof(Int32));
            this.Columns["SC_FuelsDieselPetrol"].DefaultValue = 0;
            this.Columns.Add("SC_OilsAndLubricants", typeof(Int32));
            this.Columns["SC_OilsAndLubricants"].DefaultValue = 0;
            this.Columns.Add("SC_SolventsCleaners", typeof(Int32));
            this.Columns["SC_SolventsCleaners"].DefaultValue = 0;
            this.Columns.Add("SC_DustWoodConcreteSilica", typeof(Int32));
            this.Columns["SC_DustWoodConcreteSilica"].DefaultValue = 0;
            this.Columns.Add("SC_Fumes", typeof(Int32));
            this.Columns["SC_Fumes"].DefaultValue = 0;
            this.Columns.Add("SC_BatteryChemicals", typeof(Int32));
            this.Columns["SC_BatteryChemicals"].DefaultValue = 0;

            this.Columns.Add("FireAndExposion", typeof(Int32));
            this.Columns["FireAndExposion"].DefaultValue = 0;
            this.Columns.Add("FE_HotWorks", typeof(Int32));
            this.Columns["FE_HotWorks"].DefaultValue = 0;
            this.Columns.Add("FE_FlammableLiquids", typeof(Int32));
            this.Columns["FE_FlammableLiquids"].DefaultValue = 0;
            this.Columns.Add("FE_GasCylinders", typeof(Int32));
            this.Columns["FE_GasCylinders"].DefaultValue = 0;
            this.Columns.Add("FE_StoredEnergy", typeof(Int32));
            this.Columns["FE_StoredEnergy"].DefaultValue = 0;
            this.Columns.Add("FE_BatteryCharging", typeof(Int32));
            this.Columns["FE_BatteryCharging"].DefaultValue = 0;
            
            this.Columns.Add("NoiseVibrationAndHealth", typeof(Int32));
            this.Columns["NoiseVibrationAndHealth"].DefaultValue = 0;
            this.Columns.Add("NVH_NoiseExposure", typeof(Int32));
            this.Columns["NVH_NoiseExposure"].DefaultValue = 0;
            this.Columns.Add("NVH_HandArmVibration", typeof(Int32));
            this.Columns["NVH_HandArmVibration"].DefaultValue = 0;
            this.Columns.Add("NVH_WholeBodyVibration", typeof(Int32));
            this.Columns["NVH_WholeBodyVibration"].DefaultValue = 0;
            this.Columns.Add("NVH_DustInhalation", typeof(Int32));
            this.Columns["NVH_DustInhalation"].DefaultValue = 0;
            this.Columns.Add("NVH_SkinContactDermatitis", typeof(Int32));
            this.Columns["NVH_SkinContactDermatitis"].DefaultValue = 0;
            this.Columns.Add("NVH_Fatigue", typeof(Int32));
            this.Columns["NVH_Fatigue"].DefaultValue = 0;
            
            this.Columns.Add("PeopleAndBehavior", typeof(Int32));
            this.Columns["PeopleAndBehavior"].DefaultValue = 0;
            this.Columns.Add("PB_InexperiencedWorkers", typeof(Int32));
            this.Columns["PB_InexperiencedWorkers"].DefaultValue = 0;
            this.Columns.Add("PB_LoneWorking", typeof(Int32));
            this.Columns["PB_LoneWorking"].DefaultValue = 0;
            this.Columns.Add("PB_PublicInterface", typeof(Int32));
            this.Columns["PB_PublicInterface"].DefaultValue = 0;
            this.Columns.Add("PB_InterfaceWithOtherContractors", typeof(Int32));
            this.Columns["PB_InterfaceWithOtherContractors"].DefaultValue = 0;
            this.Columns.Add("PB_ViolenceAggression", typeof(Int32));
            this.Columns["PB_ViolenceAggression"].DefaultValue = 0;

            this.Columns.Add("VehiclesAndTraffic", typeof(Int32));
            this.Columns["VehiclesAndTraffic"].DefaultValue = 0;
            this.Columns.Add("VT_SiteTraffic", typeof(Int32));
            this.Columns["VT_SiteTraffic"].DefaultValue = 0;
            this.Columns.Add("VT_ReversingVehicles", typeof(Int32));
            this.Columns["VT_ReversingVehicles"].DefaultValue = 0;
            this.Columns.Add("VT_LoadingUnloading", typeof(Int32));
            this.Columns["VT_LoadingUnloading"].DefaultValue = 0;
            this.Columns.Add("VT_PublicHighways", typeof(Int32));
            this.Columns["VT_PublicHighways"].DefaultValue = 0;
            this.Columns.Add("VT_BanksmanOperations", typeof(Int32));
            this.Columns["VT_BanksmanOperations"].DefaultValue = 0;

            this.Columns.Add("Environmental", typeof(Int32));
            this.Columns["Environmental"].DefaultValue = 0;
            this.Columns.Add("EN_WasteGeneration", typeof(Int32));
            this.Columns["EN_WasteGeneration"].DefaultValue = 0;
            this.Columns.Add("EN_FuelSpills", typeof(Int32));
            this.Columns["EN_FuelSpills"].DefaultValue = 0;
            this.Columns.Add("EN_NoiseNuisance", typeof(Int32));
            this.Columns["EN_NoiseNuisance"].DefaultValue = 0;
            this.Columns.Add("EN_DustEmissions", typeof(Int32));
            this.Columns["EN_DustEmissions"].DefaultValue = 0;
            this.Columns.Add("EN_PollutionOfDrainsWatercourses", typeof(Int32));
            this.Columns["EN_PollutionOfDrainsWatercourses"].DefaultValue = 0;

            this.Columns.Add("EmergencyAndWelfare", typeof(Int32));
            this.Columns["EmergencyAndWelfare"].DefaultValue = 0;
            this.Columns.Add("EW_Fire", typeof(Int32));
            this.Columns["EW_Fire"].DefaultValue = 0;
            this.Columns.Add("EW_FirstAidProvision", typeof(Int32));
            this.Columns["EW_FirstAidProvision"].DefaultValue = 0;
            this.Columns.Add("EW_EmergencyEvacuation", typeof(Int32));
            this.Columns["EW_EmergencyEvacuation"].DefaultValue = 0;
            this.Columns.Add("EW_WelfareFacilities", typeof(Int32));
            this.Columns["EW_WelfareFacilities"].DefaultValue = 0;
            this.Columns.Add("EW_ExtremeTemperatures", typeof(Int32));
            this.Columns["EW_ExtremeTemperatures"].DefaultValue = 0;
        }
    }
}