using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class FeesSummaryDto
    {
        public decimal TotalCollected { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal TotalOverdue { get; set; }
        public int TotalPaid { get; set; }
        public int TotalUnpaid { get; set; }
        public int TotalOverdueCount { get; set; }
    }
}
