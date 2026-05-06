using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteWorkerPage : Page
    {
        private readonly UserService _userService = new UserService();
        private int? _currentWorkerId;

        public DeleteWorkerPage()
        {
            InitializeComponent();
        }

        private async void SearchWorker_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchWorkerID.Text, out int workerId))
            {
                try
                {
                    // Fetch user details using your existing GetUserByIdAsync service
                    var worker = await _userService.GetUserByIdAsync(workerId);

                    if (worker != null)
                    {
                        _currentWorkerId = worker.UserId;

                        // Map properties to the Read-Only textboxes
                        // Using UserName property from your User model
                        txtWorkerName.Text = worker.UserName;
                        txtRole.Text = worker.UserType; // Mapping UserType to Role display
                    }
                    else
                    {
                        MessageBox.Show("Worker not found. Please check the ID.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching worker: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Worker ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_currentWorkerId == null)
            {
                MessageBox.Show("Please search for a worker first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Standard confirmation dialog for destructive actions
            var result = MessageBox.Show($"Are you sure you want to delete {txtWorkerName.Text}?\nThis cannot be undone.",
                                        "Confirm Deletion",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Call your DeleteUserAsync service
                    bool isDeleted = await _userService.DeleteUserAsync(_currentWorkerId.Value);

                    if (isDeleted)
                    {
                        MessageBox.Show("Worker deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete worker. The server may have rejected the request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during deletion: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearFields()
        {
            _currentWorkerId = null;
            SearchWorkerID.Clear();
            txtWorkerName.Clear();
            txtRole.Clear();
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

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<AddWorkerPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}