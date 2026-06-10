using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditChildPage : Page
    {
        private readonly ChildService _childService = new ChildService();
        private readonly CenterService _centerService = new CenterService();
        private readonly UserService _userService = new UserService();
        private ChildSummaryDto _currentChild;

        public EditChildPage()
        {
            InitializeComponent();
            LoadFormData();
        }

        private void ClearFields()  
        {
            _currentChild = null;
            SearchChildID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            dpBirthDate.SelectedDate = null;
            cmbCenter.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            cmbGuardian.SelectedIndex = -1;
        }

        private async void LoadFormData()
        {
            try
            {
                // 1. Load Centers Dropdown
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;

                // 2. Load Guardians (Filtered to Parents only to match dropdown requirement)
                var allUsers = await _userService.GetAllUsersAsync();
                var parentsOnly = allUsers
                    .Where(u => u.UserType != null && u.UserType.Equals("Parent", StringComparison.OrdinalIgnoreCase))
                    .Select(u => new
                    {
                        u.UserId,
                        FullName = u.UserName // Fits DisplayMemberPath="FullName"
                    })
                    .ToList();

                cmbGuardian.ItemsSource = parentsOnly;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form dependencies: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchChildID.Text))
            {
                MessageBox.Show("Please enter a valid numeric Child ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int searchChildId = int.TryParse(SearchChildID.Text.Trim(), out int id) ? id : -1;
                var child = await _childService.GetChildByIdAsync(searchChildId);

                if (child != null)
                {
                    _currentChild = child;

                    // Split Full Name mapping text fields
                    string[] nameParts = child.ChildFullName.Split(' ');
                    txtFirstName.Text = nameParts.Length > 0 ? nameParts[0] : "";
                    txtLastName.Text = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                    // Address assignment
                    txtAddress.Text = child.ChildAddress ?? string.Empty;

                    dpBirthDate.SelectedDate = child.ChildBirthDate;
                    cmbCenter.SelectedValue = child.CenterId;

                    // Match and pre-select the Linked Guardian from the database property values
                    cmbGuardian.SelectedValue = child.UserId;

                    foreach (ComboBoxItem item in cmbGender.Items)
                    {
                        if (item.Content.ToString() == child.ChildGender)
                        {
                            cmbGender.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Child not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for child record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_currentChild == null)
            {
                MessageBox.Show("Please search for a child first before saving changes.", "No Record Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("First Name, Last Name, and Address fields are required profiles parameters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var updatedChildDto = new UpdateChildDto
                {
                    ChildFirstName = txtFirstName.Text.Trim(),
                    ChildLastName = txtLastName.Text.Trim(),
                    ChildAddress = txtAddress.Text.Trim(),
                    ChildBirthDate = dpBirthDate.SelectedDate ?? _currentChild.ChildBirthDate,
                    ChildGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    CenterId = (int)(cmbCenter.SelectedValue ?? 0),
                    UserId = cmbGuardian.SelectedValue != null ? (int)cmbGuardian.SelectedValue : _currentChild.UserId,
                };

                bool success = await _childService.UpdateChildAsync(_currentChild.ChildId, updatedChildDto);

                if (success)
                {
                    MessageBox.Show("Child details profiles updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to save child configuration changes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sidebar Navigation links
        private void ManageChildren_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<ChildrenPage>());
        private void AddChild_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<AddChildrenPage>());
        private void DeleteChild_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<DeleteChildPage>());
    }
}