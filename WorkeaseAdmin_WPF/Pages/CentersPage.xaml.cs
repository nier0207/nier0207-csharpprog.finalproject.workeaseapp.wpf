using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class CentersPage : Page
    {
        private readonly CenterService _centerService;

        public CentersPage(CenterService centerService)
        {
            InitializeComponent();
            _centerService = centerService;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCentersAsync();
        }

        private async Task LoadCentersAsync()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                CentersListView.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading centers: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCenter_Click(object sender, RoutedEventArgs e)
        {
            var addPage = App.Services.GetRequiredService<AddCenterPage>();
            this.NavigationService?.Navigate(addPage);
        }

        private void EditCenter_Click(object sender, RoutedEventArgs e)
        {
            var editPage = App.Services.GetRequiredService<EditCenterPage>();
            this.NavigationService?.Navigate(editPage);
        }

        private void DeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            var deletePage = App.Services.GetRequiredService<DeleteCenterPage>();
            this.NavigationService?.Navigate(deletePage);
        }
    }
}