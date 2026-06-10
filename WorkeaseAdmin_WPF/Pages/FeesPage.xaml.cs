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
    public partial class FeesPage : Page
    {
        private readonly FeeService _feeService;
        private readonly AutoFeeService _autoFeeService;
        private readonly CenterService _centerService;
        private List<FeeSummaryDto> _currentRecords = new List<FeeSummaryDto>();

        public FeesPage()
        {
            InitializeComponent();
            _feeService = App.Services.GetRequiredService<FeeService>();
            _centerService = App.Services.GetRequiredService<CenterService>();
            _autoFeeService = App.Services.GetRequiredService<AutoFeeService>();
            ResetSummaryBoxes();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();
            await RefreshList();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchFeeBox.Text.Trim();
            var selectedCenter = cmbCenterSearch.SelectedItem as Center;

            // VALIDATION: Error if both are empty
            if (string.IsNullOrWhiteSpace(searchText) && selectedCenter == null)
            {
                MessageBox.Show("Please enter a name/ID or select a center to search.", "Empty Search", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? centerIdParam = selectedCenter?.CenterId;
            int? childIdParam = null;
            string? receiptNoParam = null;

            if (int.TryParse(searchText, out int parsedId)) childIdParam = parsedId;
            else if (!string.IsNullOrWhiteSpace(searchText)) receiptNoParam = searchText;

            await RefreshList(childIdParam, centerIdParam, receiptNoParam);

            // Fallback for ID as Receipt No
            if (childIdParam.HasValue && !_currentRecords.Any())
            {
                await RefreshList(null, centerIdParam, searchText);
            }

            await HandleSummaryUpdate(childIdParam);
        }

        private async void MarkAsPaid_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is FeeSummaryDto clickedRecord)
            {
                var confirm = MessageBox.Show($"Record payment for Receipt {clickedRecord.FeeRecordReceiptNo}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    bool success = await _feeService.MarkFeeAsPaidAsync(clickedRecord.FeeId);

                    if (success)
                    {
                        // Update local data properties
                        clickedRecord.IsPaid = true;
                        clickedRecord.FeePaidDate = DateTime.Now;

                        // UI Refresh ONLY (No restart, no scroll reset)
                        FeesListView.Items.Refresh();

                        // Call the bottom textboxes update
                        await UpdateCalculatedTotals(clickedRecord.ChildId);

                        MessageBox.Show("Payment recorded successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error updating payment on server.");
                    }
                }
            }
        }

        private async Task RefreshList(int? childId = null, int? centerId = null, string? receiptNo = null)
        {
            try
            {
                var records = await _feeService.GetFilteredFeeRecordsAsync(childId, centerId, receiptNo);
                _currentRecords = records ?? new List<FeeSummaryDto>();
                FeesListView.ItemsSource = _currentRecords;
            }
            catch { _currentRecords = new List<FeeSummaryDto>(); FeesListView.ItemsSource = null; }
        }

        private async void FeesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FeesListView.SelectedItem is FeeSummaryDto selected)
                await UpdateCalculatedTotals(selected.ChildId);
        }

        private async Task HandleSummaryUpdate(int? searchedId)
        {
            if (searchedId.HasValue) await UpdateCalculatedTotals(searchedId.Value);
            else if (_currentRecords.Any()) await UpdateCalculatedTotals(_currentRecords.First().ChildId);
            else ResetSummaryBoxes();
        }

        private async Task UpdateCalculatedTotals(int childId)
        {
            try
            {
                var totals = await _feeService.GetFeeCalculationsAsync(childId);
                if (totals != null)
                {
                    txtTotalPaid.Text = $"₱{totals.FeeTotalAmountPaid:N2}";
                    txtTotalOverdue.Text = $"₱{totals.FeeTotalAmountOverdue:N2}";
                    txtTotalRemaining.Text = $"₱{totals.FeeTotalRemainingAmount:N2}";
                }
            }
            catch { ResetSummaryBoxes(); }
        }

        private void ResetSummaryBoxes()
        {
            txtTotalPaid.Text = "₱0.00";
            txtTotalOverdue.Text = "₱0.00";
            txtTotalRemaining.Text = "₱0.00";
        }

        private async Task LoadCenters()
        {
            try { cmbCenterSearch.ItemsSource = await _centerService.GetAllCentersAsync(); }
            catch { }
        }

        private async void GenerateMonthlyFee_Click(object sender, RoutedEventArgs e)
        {
            // ── SAFETY GUARD: Check if previous month has actually completed ──
            if (_currentRecords != null && _currentRecords.Any())
            {
                // Find the newest fee record based on its DueDate / generation properties
                var latestRecord = _currentRecords.OrderByDescending(r => r.FeeDueDate).FirstOrDefault();

                if (latestRecord != null)
                {
                    DateTime now = DateTime.Now;

                    // Option A: Check if the latest record matches our current calendar Month/Year context
                    if (latestRecord.FeeDueDate.Month == now.Month && latestRecord.FeeDueDate.Year == now.Year)
                    {
                        MessageBox.Show("Fees have already been generated for the current active month.",
                                        "Action Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Option B: Ensure the target billing tracking month has finished completely 
                    // before a new cycle sequence generates
                    DateTime nextAllowedGenerationDate = new DateTime(latestRecord.FeeDueDate.Year, latestRecord.FeeDueDate.Month, 1).AddMonths(1);
                    if (now < nextAllowedGenerationDate)
                    {
                        MessageBox.Show($"Cannot generate new records yet. The billing cycle for the last generated records ({latestRecord.FeeDueDate:MMMM yyyy}) hasn't concluded.",
                                        "Billing Cycle Active", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }

            var confirm = MessageBox.Show("Generate monthly fees for all active children?", "System Action", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _autoFeeService.GenerateMonthlyFeesAsync();

                    if (success)
                    {
                        MessageBox.Show("Monthly fees generated successfully!", "Success");
                        await RefreshList(); // Refresh to show new records
                    }
                    else
                    {
                        MessageBox.Show("Failed to generate fees. Check if you have Admin permissions.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        // Sidebar: Process Overdue (Orange Button)
        private async void ProcessOverdue_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show("Scan and update unpaid fees to 'Overdue' status?", "System Action", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _autoFeeService.ProcessOverdueFeesAsync();

                    if (success)
                    {
                        MessageBox.Show("Overdue statuses updated successfully!", "Success");
                        await RefreshList(); // Refresh to see status changes
                    }
                    else
                    {
                        MessageBox.Show("Failed to process overdue fees.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void EditFee_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new EditFeePage());
        private void AddFee_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new AddFeePage());
        private void DeleteFee_Click(object sender, RoutedEventArgs e) => NavigationService?.Navigate(new DeleteFeePage());
    }
}