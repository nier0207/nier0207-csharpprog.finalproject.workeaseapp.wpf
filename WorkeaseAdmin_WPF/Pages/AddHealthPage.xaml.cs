using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddHealthPage : Page
    {
        public AddHealthPage()
        {
            InitializeComponent();
        }

        private void BackToHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HealthPage());
        }

        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditHealthPage());
        }

        private void DeleteHealth_Click(object sender, RoutedEventArgs e)
        {
            // Pwedeng i-redirect sa main list para mag-delete
            NavigationService.Navigate(new HealthPage());
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            txtChildName.Clear();
            WeightBox.Clear();
            HeightBox.Clear();
            RemarksBox.Clear();
            StatusCombo.SelectedIndex = 0;
            CheckupDatePicker.SelectedDate = null;
        }

        private void CreateRecord_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChildName.Text))
            {
                MessageBox.Show("Please enter the child's name.", "Required Field", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Dito ang logic para i-save sa database
            MessageBox.Show("New health record added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new HealthPage());
        }
    }
}