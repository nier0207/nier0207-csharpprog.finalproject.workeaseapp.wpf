using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class CreateHealthDto
    {
        public int ChildId { get; set; }
        public DateTime HealthRecordDate { get; set; }
        public decimal HealthRecordWeigtKg { get; set; }
        public decimal HealthRecordHeightCm { get; set; }
        public string? HealthRecordNotes { get; set; }
    }
}
