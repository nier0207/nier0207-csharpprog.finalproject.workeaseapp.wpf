using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddChildrenPage : Page
    {
        private readonly ChildService _childService = new ChildService();
        private readonly UserService _userService = new UserService();
        private readonly CenterService _centerService = new CenterService();

        public AddChildrenPage()
        {
            InitializeComponent();
            LoadFormData();
        }

        private async void LoadFormData()
        {
            try
            {
                // 1. Load Centers
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;

                // 2. Load Guardians (Filtered to Parents only)
                var allUsers = await _userService.GetAllUsersAsync();
                var parentsOnly = allUsers
                    .Where(u => u.UserType != null && u.UserType.Equals("Parent", StringComparison.OrdinalIgnoreCase))
                    .Select(u => new
                    {
                        u.UserId,
                        FullName = u.UserName // Assuming UserName contains the full name string
                    })
                    .ToList();

                cmbGuardian.ItemsSource = parentsOnly;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form data: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void btnCreateChild_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                cmbGuardian.SelectedValue == null ||
                dpBirthDate.SelectedDate == null ||
                cmbGender.SelectedItem == null ||
                cmbCenter.SelectedValue == null)
            {
                MessageBox.Show("Please fill out all required fields, including Guardian and Center.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Prepare the DTO
            var newChildDto = new CreateChildDto // Ensure this model exists in your Models folder
            {
                ChildFirstName = txtFirstName.Text.Trim(),
                ChildLastName = txtLastName.Text.Trim(),
                ChildBirthDate = dpBirthDate.SelectedDate.Value,
                ChildGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                UserId = (int)cmbGuardian.SelectedValue,
                CenterId = (int)cmbCenter.SelectedValue
            };

            // 3. Call Service
            try
            {
                var result = await _childService.CreateChildWithParentAsync(newChildDto);

                if (result != null)
                {
                    MessageBox.Show($"Child record for '{result.ChildFirstName}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to save the record. Please check the server connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            cmbGuardian.SelectedIndex = -1;
            cmbCenter.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            dpBirthDate.SelectedDate = null;
        }

        // --- Sidebar Navigation ---

        private void ManageChildren_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<ChildrenPage>();
            this.NavigationService?.Navigate(page);
        }

        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditChildPage>();
            this.NavigationService?.Navigate(page);
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteChildPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}