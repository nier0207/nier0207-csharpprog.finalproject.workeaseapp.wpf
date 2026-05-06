using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class LoginRequest
    {
        public string LoginEmail { get; set; } = string.Empty;
        public string LoginPassword { get; set; } = string.Empty;
    }
}
