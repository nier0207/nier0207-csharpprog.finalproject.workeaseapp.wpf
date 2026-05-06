using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditFeePage : Page
    {
        private readonly FeeService _feeService;
        private readonly CenterService _centerService;
        private List<FeeSummaryDto> _allLoadedRecords = new List<FeeSummaryDto>();
        private int _selectedFeeId = 0;

        public EditFeePage()
        {
            InitializeComponent();
            _feeService = App.Services.GetRequiredService<FeeService>();
            _centerService = App.Services.GetRequiredService<CenterService>();
            this.Loaded += Page_Loaded;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();
            FeesListView.ItemsSource = null;
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
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchFeeBox.Text.Trim();
            int? childIdParam = null;
            int? centerIdParam = null;
            string? receiptNoParam = null;

            if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                centerIdParam = selectedCenter.CenterId;

            if (int.TryParse(searchText, out int parsedId))
                childIdParam = parsedId;
            else if (!string.IsNullOrWhiteSpace(searchText))
                receiptNoParam = searchText;

            try
            {
                var records = await _feeService.GetFilteredFeeRecordsAsync(childIdParam, centerIdParam, receiptNoParam);
                _allLoadedRecords = records ?? new List<FeeSummaryDto>();
                FeesListView.ItemsSource = _allLoadedRecords;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}");
            }
        }

        private void FeesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FeesListView.SelectedItem is FeeSummaryDto selected)
            {
                _selectedFeeId = selected.FeeId;
                txtReceiptNo.Text = selected.FeeRecordReceiptNo;
                txtChildName.Text = selected.ChildName;
                txtFeeAmount.Text = selected.FeeMonthlyAmount.ToString("F2", CultureInfo.InvariantCulture);

                // Match Month Int to Tag
                foreach (ComboBoxItem item in cmbFeeMonth.Items)
                {
                    if (int.TryParse(item.Tag?.ToString(), out int val) && val == selected.FeeMonth)
                    {
                        cmbFeeMonth.SelectedItem = item;
                        break;
                    }
                }

                // Match Year
                foreach (ComboBoxItem item in cmbFeeYear.Items)
                {
                    if (item.Content?.ToString() == selected.FeeYear.ToString())
                    {
                        cmbFeeYear.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private async void UpdatePayment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFeeId == 0)
            {
                MessageBox.Show("Please select a record from the list first.");
                return;
            }

            if (!decimal.TryParse(txtFeeAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Please enter a valid numeric amount.");
                return;
            }

            var selectedMonthItem = cmbFeeMonth.SelectedItem as ComboBoxItem;
            int monthInt = int.Parse(selectedMonthItem?.Tag?.ToString() ?? "1");

            var selectedYearItem = cmbFeeYear.SelectedItem as ComboBoxItem;
            int yearInt = int.Parse(selectedYearItem?.Content?.ToString() ?? DateTime.Now.Year.ToString());

            var updateData = new UpdateFeeDto
            {
                FeeRecordMonth = monthInt,
                FeeRecordYear = yearInt,
                FeeRecordAmount = amount,
                FeeRecordIsPaid = true // Assuming update usually marks as paid
            };

            var result = await _feeService.UpdateFeeRecordAsync(_selectedFeeId, updateData);

            if (result != null)
            {
                MessageBox.Show("Payment Record successfully updated!", "Success");

                ClearFields();

                Search_Click(null, null);
            }
            else
            {
                MessageBox.Show("Failed to update record. Check server connectivity.");
            }
        }

        // Logic to clear all input fields
        private void ClearFields()
        {
            _selectedFeeId = 0;
            txtReceiptNo.Clear();
            txtChildName.Clear();
            txtFeeAmount.Clear();
            cmbFeeMonth.SelectedIndex = -1;
            cmbFeeYear.SelectedIndex = -1;
            FeesListView.SelectedItem = null;
        }

        private void BackToFees_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();
        private void AddFee_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new AddFeePage());
        private void DeleteFee_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new DeleteFeePage());
    }
}