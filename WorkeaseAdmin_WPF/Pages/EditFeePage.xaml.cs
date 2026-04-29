using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditFeePage : Page
    {
        public EditFeePage()
        {
            InitializeComponent();
        }

        private void BackToFees_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeesPage());
        }

        private void AddFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFeePage());
        }

        private void DeleteFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeesPage());
        }

        private void UpdatePayment_Click(object sender, RoutedEventArgs e)
        {
            // Database Update Logic
            MessageBox.Show("Payment record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new FeesPage());
        }
    }
}