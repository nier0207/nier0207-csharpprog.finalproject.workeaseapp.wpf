using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditHealthPage : Page
    {
        public EditHealthPage()
        {
            InitializeComponent();
        }

        private void BackToHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HealthPage());
        }

        private void AddHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddHealthPage());
        }

        private void DeleteHealth_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete functionality available in main Health list.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateRecord_Click(object sender, RoutedEventArgs e)
        {
            // Dito ilalagay yung database update logic
            MessageBox.Show("Health record updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new HealthPage());
        }
    }
}