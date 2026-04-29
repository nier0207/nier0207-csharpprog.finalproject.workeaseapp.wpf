using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteCenterPage : Page
    {
        public DeleteCenterPage()
        {
            InitializeComponent();
        }

        // Search logic
        private void SearchCenter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchCenterID.Text))
            {
                MessageBox.Show("Pre, paki-input muna yung Center ID.");
                return;
            }

            // Sample data para makita mo yung effect
            txtCenterName.Text = "Sample Enrollment Center";
            txtLocation.Text = "Angeles City, Pampanga";
        }

        // Delete confirmation logic
        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this center? This cannot be undone.",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Center has been deleted.");
                this.NavigationService?.Navigate(new CentersPage());// Balik sa main list
            }
        }

        // Sidebar Navigation
        private void ManageCenters_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new CentersPage());
        private void EditCenter_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new EditCenterPage());
        private void AddCenter_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new AddCenterPage());
    }
}