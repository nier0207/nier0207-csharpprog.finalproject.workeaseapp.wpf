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
        private readonly HealthService _healthService = new HealthService();
        private readonly CenterService _centerService = new CenterService();
        private List<HealthSummaryDto> _allLoadedRecords = new List<HealthSummaryDto>();

        // Track selection
        private int _selectedHealthRecordId = 0;
        private string _selectedChildName = "";

        public DeleteHealthPage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();
            HealthListView.ItemsSource = null;
        }

        private async Task LoadCenters()
        {
            var centers = await _centerService.GetAllCentersAsync();
            if (centers != null) cmbCenterSearch.ItemsSource = centers;
        }

        // Logic matched from your Edit Page
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchHealthBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;

            if (string.IsNullOrWhiteSpace(searchText) && cmbCenterSearch.SelectedItem == null)
            {
                MessageBox.Show("Enter ID or select a Center.", "Input Required");
                return;
            }

            if (cmbCenterSearch.SelectedItem is Center selectedCenter) centerIdParam = selectedCenter.CenterId;

            bool isNumeric = int.TryParse(searchText, out int parsedId);
            if (isNumeric) childIdParam = parsedId;

            try
            {
                var records = await _healthService.GetFilteredHealthRecordAsync(childIdParam, centerIdParam);
                _allLoadedRecords = records ?? new List<HealthSummaryDto>();
                HealthListView.ItemsSource = _allLoadedRecords;

                // Local filtering for non-numeric search
                if (!string.IsNullOrWhiteSpace(searchText) && !isNumeric)
                {
                    HealthListView.ItemsSource = _allLoadedRecords
                        .Where(x => x.ChildName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Handle Row Selection
        private void HealthListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HealthListView.SelectedItem is HealthSummaryDto selected)
            {
                _selectedHealthRecordId = selected.HealthRecordId;
                _selectedChildName = selected.ChildName;
            }
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHealthRecordId == 0)
            {
                MessageBox.Show("Please click a record in the list first.", "Selection Missing");
                return;
            }

            if (MessageBox.Show($"Delete record for {_selectedChildName}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var success = await _healthService.DeleteHealthRecordAsync(_selectedHealthRecordId);
                if (success)
                {
                    MessageBox.Show("Deleted.");
                    _selectedHealthRecordId = 0;
                    Search_Click(null, null); // Refresh list
                }
                else MessageBox.Show("Delete failed.");
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