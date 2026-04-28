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
    /// Interaction logic for EditChildPage.xaml
    /// </summary>
    public partial class EditChildPage : Page
    {
        public EditChildPage() { InitializeComponent(); }
        private void ManageChildren_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new ChildrenPage()); }
        private void AddChildren_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new AddChildrenPage()); }
        private void DeleteChild_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new DeleteChildPage()); }
    }
}
