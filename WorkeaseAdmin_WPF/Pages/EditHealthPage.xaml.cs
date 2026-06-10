using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditHealthPage : Page
    {
        private readonly HealthService _healthService;
        private readonly CenterService _centerService;

        private List<HealthSummaryDto> _allLoadedRecords = new List<HealthSummaryDto>();
        private int _selectedHealthRecordId = 0;

        public EditHealthPage()
        {
            InitializeComponent();

            _healthService = App.Services.GetRequiredService<HealthService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Load dropdown options
            await LoadCenters();

            // ✅ AUTOrefresh view load strategy: Triggers an initial search instantly when opening page workspace layout frame
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
                Console.WriteLine($"Error loading centers inside page state scope initialization: {ex.Message}");
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchHealthBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;

            if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                centerIdParam = selectedCenter.CenterId;

            if (int.TryParse(searchText, out int parsedId))
                childIdParam = parsedId;

            try
            {
                // Fetch dynamic elements out of web API service connection pool
                var records = await _healthService.GetFilteredHealthRecordAsync(childIdParam, centerIdParam);
                _allLoadedRecords = records ?? new List<HealthSummaryDto>();

                // Local filtering for child name segments if search string isn't numeric structural parameter type
                if (!string.IsNullOrWhiteSpace(searchText) && !childIdParam.HasValue)
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
                MessageBox.Show($"Search failed: {ex.Message}");
            }
        }

        private void HealthListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HealthListView.SelectedItem is HealthSummaryDto selected)
            {
                _selectedHealthRecordId = selected.HealthRecordId;

                txtChildName.Text = selected.ChildName;
                dtHealthDate.SelectedDate = selected.HealthRecordCreatedAt;

                // Using InvariantCulture to keep numeric decimals formatting smooth and robust
                txtWeight.Text = selected.HealthWeightKg.ToString("F2", CultureInfo.InvariantCulture);
                txtHeight.Text = selected.HealthHeightCm.ToString("F1", CultureInfo.InvariantCulture);
                txtNotes.Text = selected.HealthNotes;
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHealthRecordId == 0)
            {
                MessageBox.Show("Please select a record from the list first.");
                return;
            }

            if (!decimal.TryParse(txtWeight.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight) ||
                !decimal.TryParse(txtHeight.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal height))
            {
                MessageBox.Show("Please enter valid decimal numbers for weight and height.");
                return;
            }

            var updateData = new UpdateHealthDto
            {
                HealthRecordDate = dtHealthDate.SelectedDate ?? DateTime.Now,
                HealthRecordWeigtKg = weight,
                HealthRecordHeightCm = height,
                HealthRecordNotes = txtNotes.Text
            };

            var isSuccess = await _healthService.UpdateHealthRecordAsync(updateData, _selectedHealthRecordId);

            if (isSuccess)
            {
                MessageBox.Show("Health Record successfully updated!");

                // Clear selection index layout nodes safely
                _selectedHealthRecordId = 0;
                txtChildName.Text = string.Empty;
                txtWeight.Text = string.Empty;
                txtHeight.Text = string.Empty;
                txtNotes.Text = string.Empty;
                dtHealthDate.SelectedDate = null;

                // Re-execute standard list refresh
                Search_Click(this, new RoutedEventArgs());
            }
            else
            {
                MessageBox.Show("Failed to update record. Please check the API logs.");
            }
        }

        private void ManageHealt_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(App.Services.GetRequiredService<HealthPage>());
        private void AddHealth_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(App.Services.GetRequiredService<AddHealthPage>());
        private void DeleteHealth_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(App.Services.GetRequiredService<DeleteHealthPage>());
    }
}