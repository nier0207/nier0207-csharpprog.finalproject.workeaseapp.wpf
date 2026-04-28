using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddChildrenPage : Page
    {
        public AddChildrenPage()
        {
            InitializeComponent();
        }

        // Siguraduhin na ang mga pangalan dito ay saktong-sakto sa "Click=" sa XML mo
        private void ManageChildren_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ChildrenPage());
        }

        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditChildPage());
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteChildPage());
        }

        // Dagdag ito kung may "AddChild_Click" ka rin sa loob ng page na ito
        private void AddChild_Click(object sender, RoutedEventArgs e)
        {
            // Do nothing or Refresh page since andito ka na
        }
    }
}