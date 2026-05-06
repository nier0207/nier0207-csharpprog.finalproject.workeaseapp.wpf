using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditCenterPage : Page
    {
        private readonly CenterService _centerService;
        private int _currentCenterId;

        public EditCenterPage(CenterService centerService)
        {
            InitializeComponent();
            _centerService = centerService;
        }

        public EditCenterPage(CenterService centerService, CenterDetailsDto centerToEdit) : this(centerService)
        {
            PopulateFields(centerToEdit);
        }

        private void PopulateFields(CenterDetailsDto center)
        {
            if (center == null) return;

            _currentCenterId = center.CenterId;
            SearchCenterID.Text = center.CenterId.ToString();
            txtCenterName.Text = center.CenterName;
            txtCenterLocation.Text = center.CenterLocation;

            txtCdwName.Text = center.CdwWorkers.Count > 0 ? center.CdwWorkers[0] : "No CDW Assigned";
            listChildren.ItemsSource = center.Children;
        }

        // Added ClearForm method to remove all info/strings from properties
        private void ClearForm()
        {
            _currentCenterId = 0;
            SearchCenterID.Clear();
            txtCenterName.Clear();
            txtCenterLocation.Clear();
            txtCdwName.Clear();
            listChildren.ItemsSource = null;
        }

        private async void SearchCenter_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchCenterID.Text, out int id))
            {
                try
                {
                    var center = await _centerService.GetCenterByIdAsync(id);

                    if (center != null)
                    {
                        PopulateFields(center);
                    }
                    else
                    {
                        MessageBox.Show("Center details not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearForm(); // Clear if searched ID doesn't exist
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching center: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Center ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCenterId == 0)
            {
                MessageBox.Show("Please search for or select a center first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updatedCenter = new Center
            {
                CenterId = _currentCenterId,
                CenterName = txtCenterName.Text.Trim(),
                CenterLocation = txtCenterLocation.Text.Trim()
            };

            try
            {
                bool isSuccess = await _centerService.UpdateCenterAsync(_currentCenterId, updatedCenter);

                if (isSuccess)
                {
                    MessageBox.Show("Center records updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearForm(); // Removes all info from properties after successful update

                    // Optional: If you want to stay on the page, keep the line above. 
                    // If you want to go back, use: this.NavigationService?.GoBack();
                }
                else
                {
                    MessageBox.Show("Failed to update records. Please check your connection or permissions.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Navigation ──────────────────────────────────────────────────

        private void ManageCenters_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<CentersPage>();
            this.NavigationService?.Navigate(page);
        }

        private void AddCenter_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddCenterPage>();
            this.NavigationService?.Navigate(page);
        }

        private void DeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteCenterPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}