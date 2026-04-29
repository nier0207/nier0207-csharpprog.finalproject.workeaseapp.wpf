using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    /// <summary>
    /// Interaction logic for DeleteHealthPage.xaml
    /// </summary>
    public partial class DeleteHealthPage : Page
    {
        public DeleteHealthPage()
        {
            InitializeComponent();
        }

        // --- NAVIGATION EVENTS ---

        private void BackToHealth_Click(object sender, RoutedEventArgs e)
        {
            // Babalik sa main health monitoring list
            NavigationService.Navigate(new HealthPage());
        }

        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditHealthPage());
        }

        private void AddHealth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddHealthPage());
        }

        // --- FUNCTIONAL EVENTS ---

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchDeleteBox.Text.Trim();

            if (string.IsNullOrEmpty(searchQuery))
            {
                MessageBox.Show("Please enter a Health ID or Name to search.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Dito mo ilalagay ang Database Search Logic
            // Halimbawa: var record = database.HealthRecords.Find(searchQuery);

            // Simulation ng pag-load ng data pagkatapos i-search:
            // txtChildName.Text = record.FullName;
            // WeightBox.Text = record.Weight;
            // HeightBox.Text = record.Height;
            // StatusBox.Text = record.Status;
            // RemarksBox.Text = record.Remarks;
        }

        private void ConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            // Check muna kung may laman ang record (ibig sabihin may na-search)
            if (string.IsNullOrEmpty(txtChildName.Text))
            {
                MessageBox.Show("No record selected. Please search for a record first.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Confirmation message bago burahin nang tuluyan
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to permanently delete the health record of {txtChildName.Text}?\n\nThis action cannot be undone.",
                "Confirm Permanent Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                // TODO: Dito mo ilalagay ang Database Delete Logic
                // SQL: DELETE FROM HealthRecords WHERE ChildName = txtChildName.Text;

                MessageBox.Show("Record has been successfully removed from the system.", "Delete Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh o bumalik sa main list
                NavigationService.Navigate(new HealthPage());
            }
        }
    }
}