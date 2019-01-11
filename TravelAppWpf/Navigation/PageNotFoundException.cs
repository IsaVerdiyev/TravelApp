using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.Navigation
{
    class PageNotFoundException : Exception
    {
        public PageNotFoundException(Exception ex) : base("Page not found", ex)
        {

        }

        public PageNotFoundException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
