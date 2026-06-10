using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddAttendancePage : Page
    {
        private readonly AttendanceService _attendanceService;
        private readonly ChildService _childService;
        private int? _foundChildId;

        public AddAttendancePage()
        {
            InitializeComponent();
            _attendanceService = App.Services.GetRequiredService<AttendanceService>();
            _childService = App.Services.GetRequiredService<ChildService>();

            // Set default date to today
            dpDate.SelectedDate = DateTime.Today;
        }

        private async void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            string input = SearchChildID.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a Child ID.");
                return;
            }

            try
            {
                if (int.TryParse(input, out int childId))
                {
                    // Fetch child details to confirm name
                    var child = await _childService.GetChildByIdAsync(childId);
                    if (child != null)
                    {
                        txtChildName.Text = $"{child.ChildFullName}";
                        _foundChildId = child.ChildId;
                    }
                    else
                    {
                        MessageBox.Show("Child ID not found.");
                        ClearChildInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching child: {ex.Message}");
                ClearChildInfo();
            }
        }

        private async void CreateAttendance_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (_foundChildId == null)
            {
                MessageBox.Show("Please search and select a valid child first.");
                return;
            }

            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.");
                return;
            }

            try
            {
                // Get status from Tag (True/False)
                var selectedStatus = (cmbStatus.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                bool isPresent = bool.Parse(selectedStatus ?? "True");

                var newAttendance = new CreateAttendanceDto
                {
                    ChildId = _foundChildId.Value,
                    AttendanceDate = dpDate.SelectedDate.Value,
                    IsPresent = isPresent
                };

                // Use the service provided
                var result = await _attendanceService.CreateAttendanceAsync(newAttendance);

                if (result != null)
                {
                    MessageBox.Show("Attendance recorded successfully!");
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearChildInfo()
        {
            txtChildName.Text = string.Empty;
            _foundChildId = null;
        }

        private void ClearForm()
        {
            ClearChildInfo();
            SearchChildID.Text = string.Empty;
            dpDate.SelectedDate = DateTime.Today;
            cmbStatus.SelectedIndex = 0;
        }

        // Navigation
        private void BackToAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new AttendancePage());

        private void EditAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new EditAttendancePage());

        private void DeleteAttendance_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new DeleteAttendancePage());
    }
}