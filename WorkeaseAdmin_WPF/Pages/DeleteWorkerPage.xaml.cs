using System.Windows;
using System.Windows.Controls;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteWorkerPage : Page
    {
        public DeleteWorkerPage()
        {
            InitializeComponent();
        }

        // Heto yung hinahanap na method para mawala ang error
        private void SearchWorker_Click(object sender, RoutedEventArgs e)
        {
            // Dito mo ilalagay yung logic para mag-search sa database soon
            MessageBox.Show("Searching for Worker ID...");
        }

        // Navigation Methods (Para gumana ang sidebar)
        private void ManageWorkers_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new WorkersPage());
        private void EditWorker_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new EditWorkerPage());
        private void AddWorker_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AddWorkerPage());
    }
}