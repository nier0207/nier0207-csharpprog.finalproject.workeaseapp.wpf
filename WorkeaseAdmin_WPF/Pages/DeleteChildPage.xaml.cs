using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class DeleteChildPage : Page
    {
        public DeleteChildPage()
        {
            InitializeComponent();
        }

        // Logic para mahanap yung data ng bata
        private void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchChildID.Text))
            {
                MessageBox.Show("Please enter a Child ID.");
                return;
            }

            // Dito mo lalagyan ng database connection soon.
            // For now, sample text lang muna para makita mo yung design.
            txtFirstName.Text = "Sample First Name";
            txtLastName.Text = "Sample Last Name";
            txtGuardian.Text = "Sample Guardian";
        }

        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record permanentally?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Record deleted successfully!");
                this.NavigationService.Navigate(new ChildrenPage());
            }
        }

        // Sidebar Links
        private void ManageChildren_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new ChildrenPage());
        private void AddChild_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new AddChildrenPage());
        private void EditChild_Click(object sender, RoutedEventArgs e) => this.NavigationService.Navigate(new EditChildPage());
    }
}