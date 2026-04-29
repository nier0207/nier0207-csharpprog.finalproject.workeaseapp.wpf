using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    /// <summary>
    /// Interaction logic for HealthPage.xaml
    /// </summary>
    public partial class HealthPage : Page
    {
        public HealthPage()
        {
            InitializeComponent();
        }

        // Navigation to Edit Health Page
        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            // Navigation logic here
            NavigationService.Navigate(new EditHealthPage());
        }

        // Navigation to Add Health Page
        private void AddHealth_Click(object sender, RoutedEventArgs e)
        {
            // Navigation logic here
            NavigationService.Navigate(new AddHealthPage());
        }

        // Action for Delete Health Record
        private void DeleteHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DeleteHealthPage());
        }
    }
}