using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models; // Ensure this matches your DTO namespace
using WorkeaseAdmin_WPF.Services; // Ensure this matches your Service namespace

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class HealthPage : Page
    {
        private readonly HealthService _healthService = new HealthService();
        private readonly CenterService _centerService = new CenterService();

        // This holds the results from the API to allow secondary local filtering (like Name)
        private List<HealthSummaryDto> _currentRecords = new List<HealthSummaryDto>();

        public HealthPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();
            await RefreshList();
        }

        // 1. Reusable method to fetch data from the API based on filters
        private async Task RefreshList(int? childId = null, int? centerId = null)
        {
            try
            {
                // Calls: /api/Health?childId=...&centerId=...
                var records = await _healthService.GetFilteredHealthRecordAsync(childId, centerId);
                _currentRecords = records ?? new List<HealthSummaryDto>();
                HealthListView.ItemsSource = _currentRecords;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load health data: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 2. Load Centers into the ComboBox for filtering
        private async Task LoadCenters()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenterSearch.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not load centers: {ex.Message}");
            }
        }

        // 3. Main Search Logic
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchHealthBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;

            // Step A: Determine if the search box is a Numeric ID
            if (int.TryParse(searchText, out int parsedId))
            {
                childIdParam = parsedId;
            }

            // Step B: Get the Center ID from ComboBox
            if (cmbCenterSearch.SelectedItem is Center selectedCenter)
            {
                centerIdParam = selectedCenter.CenterId;
            }

            // Step C: Call API with the primary filters (ID and Center)
            await RefreshList(childIdParam, centerIdParam);

            // Step D: Secondary Local Filtering for Name
            // If the user typed a name (not an ID), we filter the list returned by the API
            if (childIdParam == null && !string.IsNullOrWhiteSpace(searchText))
            {
                var filteredByName = _currentRecords
                    .Where(x => x.ChildName != null &&
                                x.ChildName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                HealthListView.ItemsSource = filteredByName;
            }
        }

        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditHealthPage>();
            this.NavigationService?.Navigate(page);
        }

        private void AddHealth_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddHealthPage>();
            this.NavigationService?.Navigate(page);
        }

        private async void DeleteHealth_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteHealthPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}