using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class ChildrenPage : Page
    {
        public ChildrenPage()
        {
            InitializeComponent();
        }

        // ITO ANG HINAHANAP NA DEFINITION:
        private void AddChild_Click(object sender, RoutedEventArgs e)
        {
            // Ang logic para lumipat sa AddChildrenPage
            this.NavigationService?.Navigate(new AddChildrenPage());
        }

        // Para sa ibang buttons sa sidebar kung meron man:
        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditChildPage());
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteChildPage());
        }

        private void ManageChildren_Click(object sender, RoutedEventArgs e)
        {
            // Stay lang sa page na ito
        }
    }
}