using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddFeePage : Page
    {
        public AddFeePage()
        {
            InitializeComponent();
            PaymentDatePicker.SelectedDate = DateTime.Today;
        }

        private void BackToFees_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeesPage());
        }

        private void EditFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditFeePage());
        }

        private void DeleteFee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FeesPage());
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            txtChildName.Clear();
            AmountBox.Clear();
            RemarksBox.Clear();
            FeeTypeCombo.SelectedIndex = 0;
            PaymentDatePicker.SelectedDate = DateTime.Today;
        }

        private void CreatePayment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChildName.Text) || string.IsNullOrWhiteSpace(AmountBox.Text))
            {
                MessageBox.Show("Please fill in the required fields (Name and Amount).", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Dito ang logic para sa pag-save sa database
            MessageBox.Show("Payment record created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new FeesPage());
        }
    }
}