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
    public partial class EditAttendancePage : Page
    {
        private readonly AttendanceService _attendanceService;
        private readonly CenterService _centerService;
        private List<AttendanceSummaryDto> _currentRecords = new List<AttendanceSummaryDto>();
        private AttendanceSummaryDto _selectedRecord;

        public EditAttendancePage()
        {
            InitializeComponent();
            _attendanceService = App.Services.GetRequiredService<AttendanceService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            // Set default search date immediately
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
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenterSearch.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load centers: {ex.Message}");
            }
        }

        private async Task RefreshList()
        {
            try
            {
                // Reset selection to prevent UI clashing during update
                AttendanceListView.SelectedItem = null;
                _selectedRecord = null;

                DateTime date = dpAttendanceDate.SelectedDate ?? DateTime.Today;

                int? centerIdParam = null;
                if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                {
                    centerIdParam = selectedCenter.CenterId;
                }

                // Fetch records from service
                var records = await _attendanceService.GetFilteredAttendanceAsync(
                    date.Day, date.Month, date.Year, null, centerIdParam);

                _currentRecords = records ?? new List<AttendanceSummaryDto>();

                string searchText = SearchChildBox.Text?.ToLower().Trim() ?? "";

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    AttendanceListView.ItemsSource = _currentRecords;
                }
                else
                {
                    AttendanceListView.ItemsSource = _currentRecords
                        .Where(a => a.ChildName.ToLower().Contains(searchText) ||
                                    a.ChildId.ToString().Contains(searchText))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance: {ex.Message}");
                _currentRecords = new List<AttendanceSummaryDto>();
                AttendanceListView.ItemsSource = null;
            }
        }

        private void AttendanceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRecord = AttendanceListView.SelectedItem as AttendanceSummaryDto;

            if (_selectedRecord != null)
            {
                // UI interaction for editing
                txtChildName.Text = _selectedRecord.ChildName;
                dpEditDate.SelectedDate = _selectedRecord.AttendanceDate;

                // Update Audit TextBlocks
                txtCreatedAt.Text = _selectedRecord.CreatedAt.ToString("MMMM dd, yyyy - hh:mm tt");
                txtUpdatedAt.Text = _selectedRecord.UpdatedAt.ToString("MMMM dd, yyyy - hh:mm tt");

                foreach (ComboBoxItem item in cmbStatus.Items)
                {
                    if (bool.TryParse(item.Tag?.ToString(), out bool isPresent))
                    {
                        if (isPresent == _selectedRecord.IsPresent)
                        {
                            cmbStatus.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            else
            {
                ClearEditFields();
            }
        }

        private async void UpdateAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecord == null)
            {
                MessageBox.Show("Please select a record from the list first.");
                return;
            }

            try
            {
                var statusItem = cmbStatus.SelectedItem as ComboBoxItem;
                if (statusItem == null) return;

                var dto = new UpdateAttendanceDto
                {
                    AttendanceDate = dpEditDate.SelectedDate ?? _selectedRecord.AttendanceDate,
                    IsPresent = bool.Parse(statusItem.Tag.ToString())
                };

                bool success = await _attendanceService.UpdateAttendanceAsync(_selectedRecord.AttendanceId, dto);
                if (success)
                {
                    MessageBox.Show("Record updated successfully.");
                    await RefreshList();
                    ClearEditFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}");
            }
        }

        private async void ToggleStatus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AttendanceSummaryDto record)
            {
                try
                {
                    var dto = new UpdateAttendanceDto
                    {
                        AttendanceDate = record.AttendanceDate,
                        IsPresent = !record.IsPresent
                    };

                    await _attendanceService.UpdateAttendanceAsync(record.AttendanceId, dto);
                    await RefreshList();
                }
                catch { /* Silent fail for quick toggle */ }
            }
        }

        private void ClearEditFields()
        {
            txtChildName.Text = string.Empty;
            txtCreatedAt.Text = string.Empty;
            txtUpdatedAt.Text = string.Empty;
            dpEditDate.SelectedDate = null;
            cmbStatus.SelectedIndex = -1;
        }

        // NAVIGATION HANDLERS (Direct instantiation prevents DI Circular Loops)
        private void BackToAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new AttendancePage());


        private void AddAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new AddAttendancePage());

        private void DeleteAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new DeleteAttendancePage());
    }
}