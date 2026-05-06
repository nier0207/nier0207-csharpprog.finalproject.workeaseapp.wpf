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
        private ChildSummaryDto _currentChild;

        public EditChildPage()
        {
            InitializeComponent();
            LoadCenters();
        }

        private void ClearFields()
        {
            _currentChild = null;
            SearchChildID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            dpBirthDate.SelectedDate = null;
            cmbCenter.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
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

                    string[] nameParts = child.ChildFullName.Split(' ');
                    txtFirstName.Text = nameParts.Length > 0 ? nameParts[0] : "";
                    txtLastName.Text = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                    dpBirthDate.SelectedDate = child.ChildBirthDate;
                    cmbCenter.SelectedValue = child.CenterId;

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
                MessageBox.Show($"Error searching for child: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_currentChild == null)
            {
                MessageBox.Show("Please search for a child first before saving changes.", "No Record Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First and Last name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var updatedChildDto = new UpdateChildDto
                {
                    ChildFirstName = txtFirstName.Text.Trim(),
                    ChildLastName = txtLastName.Text.Trim(),
                    ChildBirthDate = dpBirthDate.SelectedDate ?? _currentChild.ChildBirthDate,
                    ChildGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    CenterId = (int)(cmbCenter.SelectedValue ?? 0)
                };

                bool success = await _childService.UpdateChildAsync(_currentChild.ChildId, updatedChildDto);

                if (success)
                {
                    MessageBox.Show("Child details updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Failed to update record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sidebar Navigation
        private void ManageChildren_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<ChildrenPage>());
        private void AddChild_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<AddChildrenPage>());
        private void DeleteChild_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(App.Services.GetRequiredService<DeleteChildPage>());
    }
}