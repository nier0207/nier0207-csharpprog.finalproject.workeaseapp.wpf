using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class CenterDetailsDto
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; } = string.Empty;
        public string CenterLocation { get; set; } = string.Empty;
        public List<string> CdwWorkers { get; set; } = new();
        public List<string> Children { get; set; } = new();
    }
}
