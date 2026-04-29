using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditCenterPage : Page
    {
        public EditCenterPage()
        {
            InitializeComponent();
        }

        private void SearchCenter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchCenterID.Text))
            {
                MessageBox.Show("Please enter a Center ID.");
                return;
            }
            // Logic para i-load ang data
            txtCenterName.Text = "Sample Center";
            txtContactPerson.Text = "Juan Dela Cruz";
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Changes saved successfully!");
        }

        private void ManageCenters_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new CentersPage());
        private void AddCenter_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new AddCenterPage());
        private void DeleteCenter_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new DeleteCenterPage());
    }
}