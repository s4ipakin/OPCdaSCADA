using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_DA_Test
{
    class ListOfItemsOPC
    {
        List<string> itemsList = new List<string>()
        {
            "PLC1:.rDissolAlkaliConcSet",
            "PLC1:.rDissolWaterConcSet",
            "PLC1:.rDissolAccidConcSet",
            "PLC1:.rDissolAccidLevelSet",
            "PLC1:.rDissolAlkaliLevelSet",
            "PLC1:.rPA_WorkTime_Acc",
            "PLC1:.rPA_WorkTime_Alk",
            "PLC1:.rConcCheckTime",
            "PLC1:.wFlawSelWafing",
            "PLC1:.wFlawDissolving",
            "PLC1:.wUsedWaterLevelOK",
            "PLC1:.wUsedWaterLevelNotOK",
            "PLC1:.wAfterRinseConcAlk",
            "PLC1:.wAfterRinseConcAcc",
            "PLC1:.Heater_Kp",
            "PLC1:.Heater_Ki",
            "PLC1:.Heater_Kd",
            "PLC1:.Heater_wMaxPower",
            "PLC1:.Heater_wMinPower",
            "PLC1:.Pump_Kp",
            "PLC1:.Pump_Ki",
            "PLC1:.Pump_Kd",
            "PLC1:.Pump_MaxSpeed",
            "PLC1:.Pump_MinSpeed",
            "PLC1:.RemotePumpOnDelay",
            "PLC1:.RemotePumpOffDelay",
            "PLC1:.RemotePumpOnDelay2",
            "PLC1:.RemotePumpOffDelay2",
            "PLC1:.rConcSolutionStartOperationAlk",
            "PLC1:.rConcSolutionStopoperationAlk",
            "PLC1:.rConcSolutionStartOperationAcc",
            "PLC1:.rConcSolutionStopoperationAcc",
            "PLC1:.SelfWashUsedWaterLevelOK",
            "PLC1:.SelfWashWaterLevelOK",
            "PLC1:.SelfWashUsedWaterLevelDrain",
            "PLC1:.SelfWashWaterLevelDrain",
            "PLC1:.SelfWashAlkaliLevelOK",
            "PLC1:.SelfWashAlkaliLevelDrain",
            "PLC1:.SelfWashAccidLevelOK",
            "PLC1:.SelfWashwAccidLevelDrain",
            "PLC1:.SelfWashwPumpFlawSet",
            "PLC1:.SelfWashByAlkaliTime",
            "PLC1:.SelfWashByAccidTime",
            "PLC1:.SelfWashByWaterTime_",
            "PLC1:.rWaterLevelNotOK",
            "PLC1:.rSetPressure"

        };

        public List<string> GetOPCitems()
        {
            return itemsList;
        }

    }
}
