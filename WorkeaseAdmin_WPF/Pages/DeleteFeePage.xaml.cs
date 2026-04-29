using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteFeePage : Page
    {
        public DeleteFeePage()
        {
            InitializeComponent();
        }

        private void BackToFees_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeesPage());
        }

        private void EditFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditFeePage());
        }

        private void AddFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddFeePage());
        }

        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtChildName.Text))
            {
                MessageBox.Show("Please search and select a record first.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete this payment record for {txtChildName.Text}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Stop);

            if (result == MessageBoxResult.Yes)
            {
                // Dito ang logic para sa database delete
                MessageBox.Show("Payment record deleted successfully.", "Success");
                NavigationService.Navigate(new FeesPage());
            }
        }
    }
}