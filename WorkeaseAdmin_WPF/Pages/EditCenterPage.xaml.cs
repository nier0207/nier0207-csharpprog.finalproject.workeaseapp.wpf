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

        private void ManageCenters_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new CentersPage());
        }

        private void AddCenter_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AddCenterPage());
        }

        private void EditCenter_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditCenterPage());
        }

        private void DeleteCenter_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteCenterPage());
        }
    }
}