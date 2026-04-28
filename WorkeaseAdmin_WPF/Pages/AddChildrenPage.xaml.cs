using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkeaseAdmin_WPF.Pages
{
    /// <summary>
    /// Interaction logic for AddChildrenPage.xaml
    /// </summary>
    public partial class AddChildrenPage : Page
    {
        public AddChildrenPage()
        {
            InitializeComponent();
        }
        // Navigation for Sidebar Buttons
        private void ManageChildren_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ChildrenPage());
        }

        private void EditChild_Click(object sender, RoutedEventArgs e)
        {
            // Siguraduhin na may EditChildPage.xaml ka na
            this.NavigationService?.Navigate(new EditChildPage());
        }

        private void DeleteChild_Click(object sender, RoutedEventArgs e)
        {
            // Siguraduhin na may DeleteChildPage.xaml ka na
            this.NavigationService?.Navigate(new DeleteChildPage());
        }
    }
}
