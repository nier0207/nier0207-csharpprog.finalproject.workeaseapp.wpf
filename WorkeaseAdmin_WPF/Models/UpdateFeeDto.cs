using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class UpdateFeeDto
    {
        public int FeeRecordMonth { get; set; }
        public int FeeRecordYear { get; set; }
        public decimal FeeRecordAmount { get; set; }
        public bool FeeRecordIsPaid { get; set; }
    }
}
