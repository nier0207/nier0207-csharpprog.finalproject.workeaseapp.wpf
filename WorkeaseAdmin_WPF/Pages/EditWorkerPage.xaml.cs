using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditWorkerPage : Page
    {
        private readonly UserService _userService = new UserService();
        private readonly CenterService _centerService = new CenterService();
        private int _currentUserId;

        public EditWorkerPage()
        {
            InitializeComponent();
            LoadCenters();
        }

        private async void LoadCenters()
        {
            try
            {
                // Assuming CenterService has a GetAllCentersAsync method
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load centers: " + ex.Message);
            }
        }

        private async void SearchWorker_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchWorkerID.Text, out int userId))
            {
                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        _currentUserId = user.UserId;

                        // Map API data to UI fields
                        txtFullName.Text = $"{user.UserName}"; // Or bind separately if your API allows
                        txtEmail.Text = user.UserEmail;
                        txtPassword.Password = ""; // Usually keep password blank for security during edit

                        // Set Role ComboBox
                        foreach (ComboBoxItem item in cmbRole.Items)
                        {
                            if (item.Content.ToString() == user.UserType)
                            {
                                cmbRole.SelectedItem = item;
                                break;
                            }
                        }

                        // Set Center ComboBox
                        cmbCenter.SelectedValue = user.CenterId;
                    }
                    else
                    {
                        MessageBox.Show("User details not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching User: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Worker ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUserId == 0)
            {
                MessageBox.Show("Please search for a user first.");
                return;
            }

            var updateDto = new UpdateUserDto
            {
                UserName = txtFullName.Text,
                UserEmail = txtEmail.Text,
                UserPasswordHashed = txtPassword.Password, 
                UserType = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString(),
                CenterId = (int?)cmbCenter.SelectedValue
            };

            try
            {
                bool success = await _userService.UpdateUserAsync(_currentUserId, updateDto);
                if (success)
                {
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Update failed. Please check the data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ManageWorkers_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<WorkersPage>();
            this.NavigationService?.Navigate(page);
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddWorkerPage>();
            this.NavigationService?.Navigate(page);
        }

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            // You mentioned navigating back to CentersPage for delete context
            var page = App.Services.GetRequiredService<DeleteWorkerPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}