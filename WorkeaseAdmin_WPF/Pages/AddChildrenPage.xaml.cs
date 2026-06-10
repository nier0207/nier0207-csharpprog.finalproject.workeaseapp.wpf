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
                // 1. Fetch available Daycare Centers
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;

                // 2. Fetch Users filtered specifically to Parent entities
                var allUsers = await _userService.GetAllUsersAsync();
                var parentsOnly = allUsers
                    .Where(u => u.UserType != null && u.UserType.Equals("Parent", StringComparison.OrdinalIgnoreCase))
                    .Select(u => new
                    {
                        u.UserId,
                        FullName = u.UserName
                    })
                    .ToList();

                cmbGuardian.ItemsSource = parentsOnly;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error synchronizing dropdown fields: {ex.Message}", "Form Load Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void btnCreateChild_Click(object sender, RoutedEventArgs e)
        {
            // 1. Complete Form Validation Check
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                cmbGuardian.SelectedValue == null ||
                dpBirthDate.SelectedDate == null ||
                cmbGender.SelectedItem == null ||
                cmbCenter.SelectedValue == null)
            {
                MessageBox.Show("Please fill out all required fields, including Address, Guardian, and Center.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Map values into the Data Transfer Object
            var newChildDto = new CreateChildDto
            {
                ChildFirstName = txtFirstName.Text.Trim(),
                ChildLastName = txtLastName.Text.Trim(),
                ChildAddress = txtAddress.Text.Trim(),
                ChildBirthDate = dpBirthDate.SelectedDate.Value,
                ChildGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty,
                UserId = (int)cmbGuardian.SelectedValue,
                CenterId = (int)cmbCenter.SelectedValue
            };

            // 3. Dispatch data down to service layer
            try
            {
                var result = await _childService.CreateChildWithParentAsync(newChildDto);

                if (result != null)
                {
                    // Safe execution fallback so missing properties on the returned dynamic type won't trigger compilation errors
                    string targetName = $"{newChildDto.ChildFirstName} {newChildDto.ChildLastName}";

                    MessageBox.Show($"Child record for '{targetName}' created successfully!", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("The backend was reached but rejected the record modification.", "Storage Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An exception was unhandled by the service connector: {ex.Message}", "Connection Failures", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            cmbGuardian.SelectedIndex = -1;
            cmbCenter.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            dpBirthDate.SelectedDate = null;
        }

        // --- View Routing Controls ---

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