using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class DashboardSummary
    {
        public int TotalUsers { get; set; }
        public int TotalCenters { get; set; }
        public int TotalChildren { get; set; }
        public int TotalAbnormalChildren { get; set; }
        public decimal TotalAccumulatedFees { get; set; } // ✅ added
    }
}
