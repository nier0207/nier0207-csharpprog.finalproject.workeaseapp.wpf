using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddCenterPage : Page
    {
        public AddCenterPage()
        {
            InitializeComponent();
        }

        // Navigation papunta sa Main Table
        private void ManageCenters_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new CentersPage());
        }

        // Navigation papunta sa Edit
        private void EditCenter_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditCenterPage());
        }

        // Navigation papunta sa Delete
        private void DeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteCenterPage());
        }
    }
}