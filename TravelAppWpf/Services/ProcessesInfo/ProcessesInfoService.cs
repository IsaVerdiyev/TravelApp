using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Services.ProcessesInfo
{
    class ProcessesInfoService : IProcessesInfoService
    {

        int id = 0;

        public Dictionary<ProcessEnum, string> ProcessNames { get; }

        public ProcessesInfoService(Dictionary<ProcessEnum, string> processNames)
        {
            ProcessNames = processNames;
        }

        Dictionary<ProcessEnum, Dictionary<int, string>> activeProcesses = new Dictionary<ProcessEnum, Dictionary<int, string>>();
      
        public bool ActivateProcess(ProcessEnum processEnumKey, string processStringValue, int id)
        {
            if (activeProcesses.ContainsKey(processEnumKey))
            {
                if (activeProcesses[processEnumKey].ContainsKey(id))
                {
                    return false;
                }

                activeProcesses[processEnumKey][id] = processStringValue;
                return true;
            }
            else
            {
                activeProcesses[processEnumKey] = new Dictionary<int, string> { { id, processStringValue} };
                return true;
            }
        }

        public bool DeactivateProcess(ProcessEnum processEnumKey, int id)
        {
            if (activeProcesses.ContainsKey(processEnumKey))
            {
                return activeProcesses[processEnumKey].Remove(id);
            }
            else
            {
                return false;
            }
        }

        public List<string> GetAllStringValues()
        {
            return activeProcesses.Where(p => p.Value.Count() != 0).Select(p => p.Value.First().Value).ToList();
        }

        public string GetStringValueOfProcess(ProcessEnum processEnum)
        {
            return activeProcesses[processEnum].First().Value;
        }

        public bool IsProcessWithSpecialIdActive(ProcessEnum processEnum, int id)
        {
            if (activeProcesses.ContainsKey(processEnum))
            {
                return activeProcesses[processEnum].ContainsKey(id);
            }
            else
            {
                return false;
            }
        }

        public int GenerateUniqueId()
        {
            return ++id;
        }

        public bool IsProcessActive(ProcessEnum processEnum)
        {
            if (activeProcesses.ContainsKey(processEnum))
            {
                return activeProcesses[processEnum].Count > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
