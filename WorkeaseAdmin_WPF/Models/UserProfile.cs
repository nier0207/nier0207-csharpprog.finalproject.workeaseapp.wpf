using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkeaseAdmin_WPF.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserHashPassword { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string? CenterName { get; set; }
        public int? CenterId { get; set; }
        public bool UserIsActive { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public DateTime UserUpdatedAt { get; set; }
    }
}
