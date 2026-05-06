using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class ChildrenPage : Page
    {
        private readonly ChildService _childService;

        public ChildrenPage(ChildService childService)
        {
            InitializeComponent();
            _childService = childService;
        }

        // IMPORTANT: This must exist because of Loaded="Page_Loaded" in XAML
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadChildrenData();
        }

        private async Task LoadChildrenData()
        {
            try
            {
                var children = await _childService.GetAllChildrenAsync();
                if (children != null)
                {
                    ChildrenListView.ItemsSource = children;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading children: {ex.Message}");
            }
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

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteChildPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}