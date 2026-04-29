using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class FeesPage : Page
    {
        public FeesPage()
        {
            InitializeComponent();
        }

        private void EditFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditFeePage());
        }

        private void AddFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFeePage());
        }

        private void DeleteFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DeleteFeePage());
        }
    }
}