using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Services.ProcessesInfo
{
    interface IProcessesInfoService
    {
        bool IsProcessActive(ProcessEnum processEnum);
        bool DeactivateProcess(ProcessEnum processEnum);
        bool ActivateProcess(ProcessEnum processEnum, string processStringValue);
        string GetStringValueOfProcess(ProcessEnum processEnum);
        List<string> GetAllStringValues();
    }
}
