using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class FeeCalculatedDto
    {
        public decimal FeeTotalAmountPaid { get; set; }
        public decimal FeeTotalAmountOverdue { get; set; }
        public decimal FeeTotalRemainingAmount { get; set; }
    }
}
