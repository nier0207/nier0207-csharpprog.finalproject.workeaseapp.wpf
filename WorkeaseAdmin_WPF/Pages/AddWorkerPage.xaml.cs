using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddWorkerPage : Page
    {
        private readonly UserService _userService = new UserService();
        private readonly CenterService _centerService = new CenterService();

        public AddWorkerPage()
        {
            InitializeComponent();
            LoadCenters();
        }

        private async void LoadCenters()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading centers: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void btnCreateWorker_Click(object sender, RoutedEventArgs e)
        {
            // 1. Basic Validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select a Role", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. CenterId Logic: Nullable for Parent/Admin, Required for CDW
            int? selectedCenterId = cmbCenter.SelectedValue as int?;

            if (selectedRole == "CDW" && selectedCenterId == null)
            {
                MessageBox.Show("A Center must be assigned for CDW workers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Prepare the DTO
            var newUserDto = new CreateUserDto
            {
                UserName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}",
                UserEmail = txtEmail.Text.Trim(),
                UserHashPassword = txtPassword.Password,
                UserType = selectedRole,
                CenterId = selectedCenterId // This will be null if nothing is selected
            };

            // 4. Call the Service
            try
            {
                var createdUser = await _userService.CreateUserAsync(newUserDto);

                if (createdUser != null)
                {
                    MessageBox.Show($"Worker '{createdUser.UserName}' was registered successfully with ID: {createdUser.UserId}",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to create worker. The email might already be registered or the server rejected the request.",
                                    "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            cmbCenter.SelectedIndex = -1;
        }

        // Sidebar Navigation using Dependency Injection
        private void ManageWorkers_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<WorkersPage>();
            this.NavigationService?.Navigate(page);
        }

        private void EditWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditWorkerPage>();
            this.NavigationService?.Navigate(page);
        }

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteWorkerPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}