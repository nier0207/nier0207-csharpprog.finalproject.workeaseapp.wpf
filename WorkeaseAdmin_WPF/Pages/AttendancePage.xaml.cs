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
    public partial class AttendancePage : Page
    {
        private readonly AttendanceService _attendanceService;
        private readonly CenterService _centerService;
        private List<AttendanceSummaryDto> _currentRecords = new List<AttendanceSummaryDto>();

        public AttendancePage()
        {
            InitializeComponent();
            _attendanceService = App.Services.GetRequiredService<AttendanceService>();
            _centerService = App.Services.GetRequiredService<CenterService>();

            dpAttendanceDate.SelectedDate = DateTime.Today;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AttendanceListView.SelectedItem = null;

            await LoadCenters();
            await RefreshList();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            await RefreshList();
        }

        private async Task RefreshList()
        {
            try
            {
                if (dpAttendanceDate.SelectedDate == null) return;

                DateTime date = dpAttendanceDate.SelectedDate.Value;

                // Ensure we handle the center selection safely
                int? centerIdParam = null;
                if (cmbCenterSearch.SelectedItem is Center selectedCenter)
                {
                    centerIdParam = selectedCenter.CenterId;
                }

                // Fetch records
                var records = await _attendanceService.GetFilteredAttendanceAsync(
                    date.Day, date.Month, date.Year, null, centerIdParam);

                _currentRecords = records ?? new List<AttendanceSummaryDto>();

                // Apply filtering logic
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

        private async Task LoadCenters()
        {
            try { cmbCenterSearch.ItemsSource = await _centerService.GetAllCentersAsync(); }
            catch { }
        }

        private void EditAttendance_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditAttendancePage());
        }

        private void AddAttendance_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AddAttendancePage());
        }

        private void DeleteAttendance_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteAttendancePage());
        }
    }
}