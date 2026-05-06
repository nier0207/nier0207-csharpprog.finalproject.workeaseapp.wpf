using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteCenterPage : Page
    {
        private readonly CenterService _centerService;
        private int _currentCenterId;

        public DeleteCenterPage(CenterService centerService)
        {
            InitializeComponent();
            _centerService = centerService;
        }

        private async void SearchCenter_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(SearchCenterID.Text, out int id))
            {
                MessageBox.Show("Please enter a valid numeric Center ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var center = await _centerService.GetCenterByIdAsync(id);
                if (center != null)
                {
                    _currentCenterId = id;
                    txtCenterName.Text = center.CenterName;
                    txtLocation.Text = center.CenterLocation;
                }
                else
                {
                    MessageBox.Show("Center details not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching center: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCenterId == 0)
            {
                MessageBox.Show("Please search for a center first before attempting to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {txtCenterName.Text}? This cannot be undone.",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool isDeleted = await _centerService.DeleteCenter(_currentCenterId);

                    if (isDeleted)
                    {
                        MessageBox.Show("Center has been successfully deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the center. It might have existing records (like children or workers) linked to it.", "Deletion Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Updated to remove all strings from UI properties
        private void ClearFields()
        {
            _currentCenterId = 0;
            SearchCenterID.Clear(); // Clears the search input
            txtCenterName.Text = string.Empty;
            txtLocation.Text = string.Empty;
        }

        // ─── Navigation ──────────────────────────────────────────────────

        private void ManageCenters_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<CentersPage>();
            this.NavigationService?.Navigate(page);
        }

        private void EditCenter_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditCenterPage>();
            this.NavigationService?.Navigate(page);
        }

        private void AddCenter_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddCenterPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}