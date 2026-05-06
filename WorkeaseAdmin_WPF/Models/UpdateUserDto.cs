using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPasswordHashed { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public int? CenterId { get; set; } = null;
        public bool UserIsActive { get; set; } = true;
    }
}
