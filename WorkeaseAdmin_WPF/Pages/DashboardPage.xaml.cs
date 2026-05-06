using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DashboardPage : Page
    {
        private readonly DashboardService _dashboardService;

        public DashboardPage(DashboardService dashboardService)
        {
            InitializeComponent();
            _dashboardService = dashboardService;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDashboardSummaryAsync();
        }

        private async Task LoadDashboardSummaryAsync()
        {
            try
            {
                var summary = await _dashboardService.GetDashboardSummaryAsync();

                txtCenterTotalNo.Text = summary.TotalCenters.ToString();
                txtUserTotalNo.Text = summary.TotalUsers.ToString();
                txtChildrenTotalNo.Text = summary.TotalChildren.ToString();
                txtAbnormalHealthTotalNo.Text = summary.TotalAbnormalChildren.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard summary: {ex.Message}",
                                "Dashboard Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }
    }
}