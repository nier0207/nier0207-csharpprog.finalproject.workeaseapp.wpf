using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditChildPage : Page
    {
        public EditChildPage()
        {
            InitializeComponent();
        }

        // 1. Fix para sa SearchChild_Click error
        private void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Searching for child record...");
        }

        // 2. Fix para sa AddChild_Click error sa sidebar
        private void AddChild_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddChildrenPage());
        }

        // 3. Navigation methods para sa iba pang sidebar buttons
        private void ManageChildren_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new ChildrenPage());
        private void DeleteChild_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new DeleteChildPage());
    }
}