using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class GenerateMasterListDto
    {
        public int CenterId { get; set; }
        public string CycleInfo { get; set; } = "12th Cycle Implementation";
        public string SchoolYear { get; set; } = "CY 2025-2026";
        public string PreparedBy { get; set; } = string.Empty;
        public string NotedBy { get; set; } = string.Empty;
    }
}
