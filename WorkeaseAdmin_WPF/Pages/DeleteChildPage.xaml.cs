using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteChildPage : Page
    {
        private readonly ChildService _childService = new ChildService();
        private ChildSummaryDto _currentChild; // Stores the found child record

        public DeleteChildPage()
        {
            InitializeComponent();
        }

        // 1. Search Logic: Find the child and display details in ReadOnly fields
        private async void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchChildID.Text))
            {
                MessageBox.Show("Please enter a valid numeric Child ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int searchId = int.TryParse(SearchChildID.Text.Trim(), out int id) ? id : -1;
                var child = await _childService.GetChildByIdAsync(searchId);

                if (child != null)
                {
                    _currentChild = child;

                    // Display details in the new UI fields
                    txtChildName.Text = child.ChildFullName;
                    txtCenter.Text = child.CenterName; // Ensure your Dto has CenterName or CenterId
                }
                else
                {
                    MessageBox.Show("Child record not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for child: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 2. Delete Logic: Permanent removal
        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_currentChild == null)
            {
                MessageBox.Show("Please search for a child record first.", "No Record Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the record for {_currentChild.ChildFullName}? This action cannot be undone.",
                "Confirm Permanent Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Stop);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool isDeleted = await _childService.DeleteChildAsync(_currentChild.ChildId);

                    if (isDeleted)
                    {
                        MessageBox.Show("Record has been permanently deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the record. It may have already been removed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Helper to reset the UI
        private void ClearFields()
        {
            _currentChild = null;
            SearchChildID.Clear();
            txtChildName.Clear();
            txtCenter.Clear();
        }

        // --- Sidebar Navigation ---
        private void ManageChildren_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<ChildrenPage>();
            this.NavigationService?.Navigate(page);
        }

        private void AddChild_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddChildrenPage>();
            this.NavigationService?.Navigate(page);
        }

        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditChildPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}