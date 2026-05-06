using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddCenterPage : Page
    {
        private readonly CenterService _centerService;

        public AddCenterPage(CenterService centerService)
        {
            InitializeComponent();
            _centerService = centerService;

            LoadBarangays();
        }

        private void LoadBarangays()
        {
            var barangays = new List<string>
            {
                "- Select Barangay -",
                "Anapao", "Bangat", "Caisian", "Concordia", "Ilio-ilio",
                "Papallasen", "Poblacion", "Pogoruac", "Don Benito",
                "Sapa Grande", "Sapa Pequeña", "Tambogan", "San Jose", "San Vicente"
            };

            cmbBarangay.ItemsSource = barangays;
            cmbBarangay.SelectedIndex = 0;
        }

        private async void btnCreateCenter_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation
            if (cmbBarangay.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a Barangay.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCenterName.Text))
            {
                MessageBox.Show("Please enter a Center Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Prepare Object
            var newCenter = new Center
            {
                CenterName = txtCenterName.Text.Trim(),
                CenterLocation = cmbBarangay.SelectedItem.ToString() + ", Burgos, Pangasinan"
            };

            // 3. Service Call
            try
            {
                var result = await _centerService.CreateCenterAsync(newCenter);

                if (result != null)
                {
                    MessageBox.Show("Center added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to save the center to the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtCenterName.Clear();
            cmbBarangay.SelectedIndex = 0; // Resets to "- Select Barangay -"
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

        private void DeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteCenterPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}