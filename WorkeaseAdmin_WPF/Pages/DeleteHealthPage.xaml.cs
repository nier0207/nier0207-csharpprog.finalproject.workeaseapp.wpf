using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteHealthPage : Page
    {
        private readonly HealthService _healthService;
        private readonly CenterService _centerService;
        private List<HealthSummaryDto> _allLoadedRecords = new List<HealthSummaryDto>();

        // Track selection
        private int _selectedHealthRecordId = 0;
        private string _selectedChildName = "";

        public DeleteHealthPage()
        {
            InitializeComponent();

            // FIXED DI: Pulling service entities cleanly from Project Service Container framework context
            _healthService = App.Services.GetRequiredService<HealthService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();

            // ✅ AUTOrefresh view load strategy: Triggers an initial search load seamlessly when landing on workspace area
            Search_Click(this, new RoutedEventArgs());
        }

        private async Task LoadCenters()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                if (centers != null) cmbCenterSearch.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dropdown choices context array: {ex.Message}");
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchHealthBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;

            if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                centerIdParam = selectedCenter.CenterId;

            bool isNumeric = int.TryParse(searchText, out int parsedId);
            if (isNumeric)
                childIdParam = parsedId;

            try
            {
                var records = await _healthService.GetFilteredHealthRecordAsync(childIdParam, centerIdParam);
                _allLoadedRecords = records ?? new List<HealthSummaryDto>();

                // Local text segment evaluation filter fallback if text input isn't a numeric data key parameter
                if (!string.IsNullOrWhiteSpace(searchText) && !isNumeric)
                {
                    HealthListView.ItemsSource = _allLoadedRecords
                        .Where(x => x.ChildName != null &&
                                    x.ChildName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }
                else
                {
                    HealthListView.ItemsSource = _allLoadedRecords;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed inside deletion controller component framework: {ex.Message}");
            }
        }

        private void HealthListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HealthListView.SelectedItem is HealthSummaryDto selected)
            {
                _selectedHealthRecordId = selected.HealthRecordId;
                _selectedChildName = selected.ChildName ?? string.Empty;
            }
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHealthRecordId == 0)
            {
                MessageBox.Show("Please click a record in the list first.", "Selection Missing");
                return;
            }

            var dialogMessage = $"Are you sure you want to permanently delete the health metrics record for {_selectedChildName}?";
            if (MessageBox.Show(dialogMessage, "Confirm Permanent Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var success = await _healthService.DeleteHealthRecordAsync(_selectedHealthRecordId);
                if (success)
                {
                    MessageBox.Show("Health record successfully removed.", "Deletion Successful");
                    _selectedHealthRecordId = 0;
                    _selectedChildName = string.Empty;

                    // Re-trigger global automatic search query loop refresh
                    Search_Click(this, new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Delete operation failed. Please check backend network records.", "API Error Error");
                }
            }
        }

        // --- Navigation Methods ---
        private void BackToHealth_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<HealthPage>();
            this.NavigationService?.Navigate(page);
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
    }
}