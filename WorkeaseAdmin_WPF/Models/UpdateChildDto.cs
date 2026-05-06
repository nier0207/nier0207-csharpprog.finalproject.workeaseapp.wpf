using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class UpdateChildDto
    {
        public string ChildFirstName { get; set; } = string.Empty;
        public string ChildLastName { get; set; } = string.Empty;
        public DateTime ChildBirthDate { get; set; }
        public string ChildGender { get; set; } = string.Empty;
        public int CenterId { get; set; }
    }
}
