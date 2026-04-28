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
    /// Interaction logic for DeleteChildPage.xaml
    /// </summary>
    public partial class DeleteChildPage : Page
    {
        public DeleteChildPage() { InitializeComponent(); }
        private void ManageChildren_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new ChildrenPage()); }
        private void AddChildren_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new AddChildrenPage()); }
        private void EditChild_Click(object sender, RoutedEventArgs e) { NavigationService.Navigate(new EditChildPage()); }
    }
}
