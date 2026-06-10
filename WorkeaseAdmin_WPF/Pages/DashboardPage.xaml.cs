// Pages/DashboardPage.xaml.cs
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
        private readonly FeeService _feeService;

        public DashboardPage(DashboardService dashboardService, FeeService feeService)
        {
            InitializeComponent();
            _dashboardService = dashboardService;
            _feeService = feeService;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDashboardSummaryAsync();
        }

        private async Task LoadDashboardSummaryAsync()
        {
            try
            {
                // 1. Kick off both API tasks concurrently to prevent blocking the UI thread sequentially
                var summaryTask = _dashboardService.GetDashboardSummaryAsync();
                var feesTask = _feeService.GetOverallFeesSummaryAsync(null, null, null);

                await Task.WhenAll(summaryTask, feesTask);

                var summary = await summaryTask;
                var feesSummary = await feesTask;

                // 2. Assign standard baseline statistics counters
                txtCenterTotalNo.Text = summary.TotalCenters.ToString();
                txtUserTotalNo.Text = summary.TotalUsers.ToString();
                txtChildrenTotalNo.Text = summary.TotalChildren.ToString();
                txtAbnormalHealthTotalNo.Text = summary.TotalAbnormalChildren.ToString();

                // 3. Map the correct property from FeesSummaryDto to display total collected fees
                if (feesSummary != null)
                {
                    txtFeeTotalAccumulated.Text = $"₱{feesSummary.TotalCollected:N2}";
                }
                else
                {
                    // Fallback to legacy dashboard counter if the fees service returns null
                    txtFeeTotalAccumulated.Text = $"₱{summary.TotalAccumulatedFees:N2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading dashboard metrics: {ex.Message}",
                    "Dashboard Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                // Safe defensive UI fallback states
                txtFeeTotalAccumulated.Text = "₱0.00";
            }
        }
    }
}