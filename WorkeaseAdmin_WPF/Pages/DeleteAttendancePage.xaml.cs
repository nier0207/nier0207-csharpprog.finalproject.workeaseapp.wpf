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
    public partial class DeleteAttendancePage : Page
    {
        private readonly AttendanceService _attendanceService;
        private readonly CenterService _centerService;
        private List<AttendanceSummaryDto> _currentRecords = new List<AttendanceSummaryDto>();

        public DeleteAttendancePage()
        {
            InitializeComponent();
            _attendanceService = App.Services.GetRequiredService<AttendanceService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            // Set default search date immediately to ensure non-nullable ints for the service call
            dpAttendanceDate.SelectedDate = DateTime.Today;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCenters();
            await RefreshList();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            await RefreshList();
        }

        private async Task LoadCenters()
        {
            try
            {
                // Directly set ItemsSource for the Center filter
                cmbCenterSearch.ItemsSource = await _centerService.GetAllCentersAsync();
            }
            catch { /* Handled silently */ }
        }

        private async Task RefreshList()
        {
            try
            {
                // Use selected date or Today to satisfy non-nullable day, month, year requirements
                DateTime date = dpAttendanceDate.SelectedDate ?? DateTime.Today;

                int? centerIdParam = null;
                if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                {
                    centerIdParam = selectedCenter.CenterId;
                }

                // Fetch filtered records from the API
                var records = await _attendanceService.GetFilteredAttendanceAsync(
                    date.Day, date.Month, date.Year, null, centerIdParam);

                _currentRecords = records ?? new List<AttendanceSummaryDto>();

                // Apply client-side text filtering from SearchChildBox
                string searchText = SearchChildBox.Text?.ToLower().Trim() ?? "";

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    AttendanceListView.ItemsSource = _currentRecords;
                }
                else
                {
                    AttendanceListView.ItemsSource = _currentRecords
                        .Where(a => a.ChildName.ToLower().Contains(searchText) ||
                                    a.ChildId.ToString().Contains(searchText) ||
                                    a.AttendanceId.ToString().Contains(searchText))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading records: {ex.Message}");
                _currentRecords = new List<AttendanceSummaryDto>();
                AttendanceListView.ItemsSource = null;
            }
        }

        private void AttendanceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Logic for when a row is selected (optional)
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            // Verify a record is selected before attempting delete
            if (AttendanceListView.SelectedItem is AttendanceSummaryDto selectedRecord)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to PERMANENTLY delete the attendance record for {selectedRecord.ChildName} on {selectedRecord.AttendanceDate:MM/dd/yyyy}?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Call the DeleteAttendanceAsync service
                        bool success = await _attendanceService.DeleteAttendanceAsync(selectedRecord.AttendanceId);

                        if (success)
                        {
                            MessageBox.Show("Record deleted successfully.");
                            await RefreshList(); // Refresh the list after successful deletion
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a record from the list to delete.");
            }
        }

        // Navigation Methods
        private void BackToAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new AttendancePage());

        private void EditAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(App.Services.GetRequiredService<EditAttendancePage>());

        private void AddAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(App.Services.GetRequiredService<AddAttendancePage>());
    }
}