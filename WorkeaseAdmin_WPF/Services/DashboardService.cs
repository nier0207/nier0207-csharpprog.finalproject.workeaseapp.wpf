using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkeaseAdmin_WPF.Models;

namespace WorkeaseAdmin_WPF.Services
{
    public class DashboardService : ApiService
    {
        private readonly UserService _userService = new();
        private readonly CenterService _centerService = new();
        private readonly HealthService _healthService = new();
        private readonly ChildService _childService = new();

        public DashboardService() : base() { }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var usersTask = _userService.GetAllUsersAsync();
            var centersTask = _centerService.GetAllCentersAsync();
            var childrenTask = _childService.GetAllChildrenAsync();
            var abnormalBmiTask = _healthService.GetAbnormalBmiDtoAsync();

            await Task.WhenAll(usersTask, centersTask, childrenTask, abnormalBmiTask);

            return new DashboardSummary
            {
                TotalUsers = usersTask.Result?.Count ?? 0,
                TotalCenters = centersTask.Result?.Count ?? 0,
                TotalChildren = childrenTask.Result?.Count ?? 0,
                TotalAbnormalChildren = abnormalBmiTask.Result?.Count ?? 0
            };
        }
    }
}
