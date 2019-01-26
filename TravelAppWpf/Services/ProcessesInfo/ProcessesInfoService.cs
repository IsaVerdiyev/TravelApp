using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Services.ProcessesInfo
{
    class ProcessesInfoService : IProcessesInfoService
    {

        Dictionary<ProcessEnum, string> keyValuePairs = new Dictionary<ProcessEnum, string>();
      
        public bool ActivateProcess(ProcessEnum processEnumKey, string processStringValue)
        {
            if (keyValuePairs.ContainsKey(processEnumKey))
            {
                return false;
            }
            keyValuePairs[processEnumKey] = processStringValue;
            return true;
        }

        public bool DeactivateProcess(ProcessEnum processEnumKey)
        {
            return keyValuePairs.Remove(processEnumKey);
        }

        public List<string> GetAllStringValues()
        {
            return keyValuePairs.Values.ToList();
        }

        public string GetStringValueOfProcess(ProcessEnum processEnum)
        {
            return keyValuePairs[processEnum];
        }

        public bool IsProcessActive(ProcessEnum processEnum)
        {
            return keyValuePairs.ContainsKey(processEnum);
        }
    }
}
