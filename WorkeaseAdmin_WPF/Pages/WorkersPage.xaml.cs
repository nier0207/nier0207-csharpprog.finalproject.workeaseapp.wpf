using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class WorkersPage : Page
    {
        private readonly UserService _userService;

        public WorkersPage(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        public async Task RefreshData()
        {
            try
            {
                var workers = await _userService.GetAllUsersAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkersListView.ItemsSource = workers;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
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

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteWorkerPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}