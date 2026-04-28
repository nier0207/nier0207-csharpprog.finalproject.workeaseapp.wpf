using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class EditWorkerPage : Page
    {
        public EditWorkerPage()
        {
            InitializeComponent();
        }

        // Method para sa Search Button (IMPORTANTE para mawala ang error)
        private void SearchWorker_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchWorkerID.Text))
            {
                MessageBox.Show("Please enter a Worker ID first.");
                return;
            }
            // Logic para sa pag-search sa database soon...
            MessageBox.Show("Searching for Worker ID: " + SearchWorkerID.Text);
        }

        // Navigation Methods para sa Sidebar
        private void ManageWorkers_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new WorkersPage());
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new AddWorkerPage());
        }

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new DeleteWorkerPage());
        }
    }
}