using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Services.ProcessesInfo
{
    interface IProcessesInfoService
    {
        int GenerateUniqueId();
        Dictionary<ProcessEnum, string> ProcessNames { get; }
        bool IsProcessActive(ProcessEnum processEnum, int id);
        bool DeactivateProcess(ProcessEnum processEnum, int id);
        bool ActivateProcess(ProcessEnum processEnum, string processStringValue, int id);
        string GetStringValueOfProcess(ProcessEnum processEnum);
        List<string> GetAllStringValues();
    }
}
