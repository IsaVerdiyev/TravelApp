using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAppWpf.ViewModels
{
    class AddTicketViewModel: ViewModelBase
    {
        #region Fields And Properties

        string ticketName;
        public string TicketName { get => ticketName; set => Set(ref ticketName, value); }

        string pdfPath;
        public string PdfPath { get => pdfPath; set => Set(ref pdfPath, value); }

        #endregion
    }
}
