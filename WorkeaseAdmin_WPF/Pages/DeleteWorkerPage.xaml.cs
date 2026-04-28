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
    /// Interaction logic for DeleteWorkerPage.xaml
    /// </summary>
    public partial class DeleteWorkerPage : Page
    {
        public DeleteWorkerPage()
        {
            InitializeComponent();
        }
        // Sa loob ng bawat class (Add, Edit, Delete Worker Pages)
        private void ManageWorkers_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new WorkersPage());
        }
        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AddWorkerPage());
        }
        private void EditWorker_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new EditWorkerPage());
        }
        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteWorkerPage());
        }
    }
}
