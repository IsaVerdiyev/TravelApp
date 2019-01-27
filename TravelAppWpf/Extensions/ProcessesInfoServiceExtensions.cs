using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAppWpf.Services.ProcessesInfo;

namespace TravelAppWpf.Extensions
{
    static class ProcessesInfoServiceExtensions
    {
        public static string GetOneInfoStringFromAllProcesses(this IProcessesInfoService processesInfoService)
        {
            return processesInfoService.GetAllStringValues().Aggregate((i, j) => i + ", " + j);
        }
    }
}
