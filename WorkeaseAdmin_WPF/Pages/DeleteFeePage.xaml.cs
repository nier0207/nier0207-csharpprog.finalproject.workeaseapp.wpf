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
    public partial class DeleteFeePage : Page
    {
        private readonly FeeService _feeService;
        private readonly CenterService _centerService;
        private List<FeeSummaryDto> _allLoadedRecords = new List<FeeSummaryDto>();

        private int _selectedFeeRecordId = 0;
        private string _selectedChildName = "";
        private string _selectedReceiptNo = "";

        public DeleteFeePage()
        {
            InitializeComponent();

            _feeService = App.Services.GetRequiredService<FeeService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();

            // ✅ AUTO-REFRESH LOAD: Automatically executes initial selection pool populate on load
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
                System.Diagnostics.Debug.WriteLine($"Error loading dropdown entries context array: {ex.Message}");
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchFeeBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;
            string? receiptNoParam = null;

            // 1. Handle Center Filter
            if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                centerIdParam = selectedCenter.CenterId;

            // 2. Handle Search Text (Numeric = Child ID, Alpha-Numeric = Receipt #)
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                if (int.TryParse(searchText, out int parsedId))
                {
                    childIdParam = parsedId;
                }
                else
                {
                    receiptNoParam = searchText;
                }
            }

            try
            {
                // 3. Call API with current filter variables securely
                var records = await _feeService.GetFilteredFeeRecordsAsync(childIdParam, centerIdParam, receiptNoParam);
                _allLoadedRecords = records ?? new List<FeeSummaryDto>();

                if (!string.IsNullOrWhiteSpace(searchText) && !int.TryParse(searchText, out _))
                {
                    FeeListView.ItemsSource = _allLoadedRecords
                        .Where(x =>
                            (x.ChildName != null && x.ChildName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (x.FeeRecordReceiptNo != null && x.FeeRecordReceiptNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        )
                        .ToList();
                }
                else
                {
                    FeeListView.ItemsSource = _allLoadedRecords;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}");
            }
        }

        private void FeeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FeeListView.SelectedItem is FeeSummaryDto selected)
            {
                _selectedFeeRecordId = selected.FeeId;
                _selectedChildName = selected.ChildName ?? string.Empty;
                _selectedReceiptNo = selected.FeeRecordReceiptNo ?? string.Empty;
            }
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFeeRecordId == 0)
            {
                MessageBox.Show("Please select a record from the list first.", "No Selection");
                return;
            }

            var result = MessageBox.Show(
                $"Confirm permanent deletion of record:\n\n" +
                $"Receipt: {_selectedReceiptNo}\n" +
                $"Child: {_selectedChildName}",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var success = await _feeService.DeleteFeeRecordAsync(_selectedFeeRecordId);
                if (success)
                {
                    MessageBox.Show("Record deleted successfully.", "Success");
                    _selectedFeeRecordId = 0;
                    _selectedChildName = string.Empty;
                    _selectedReceiptNo = string.Empty;

                    // Re-trigger visual grid row index layout state refresh
                    Search_Click(this, new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Error: Could not delete record from server.", "API Error");
                }
            }
        }

        // --- Navigation ---
        private void BackToFees_Click(object sender, RoutedEventArgs e) =>
            this.NavigationService?.Navigate(App.Services.GetRequiredService<FeesPage>());

        private void EditFee_Click(object sender, RoutedEventArgs e) =>
            this.NavigationService?.Navigate(App.Services.GetRequiredService<EditFeePage>());

        private void AddFee_Click(object sender, RoutedEventArgs e) =>
            this.NavigationService?.Navigate(App.Services.GetRequiredService<AddFeePage>());
    }
}