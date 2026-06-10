using System.Windows;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Windows
{
    public partial class CenterSelectionWindow : Window
    {
        // Change the type here to 'Center' to match what the Service returns
        public Center SelectedCenter { get; private set; }
        private readonly CenterService _centerService = new CenterService();

        public CenterSelectionWindow()
        {
            InitializeComponent();
            LoadCenters();
        }

        private async void LoadCenters()
        {
            // This returns Task<List<Center>>, so the ComboBox is filled with Center objects
            var centers = await _centerService.GetAllCentersAsync();
            cmbCenters.ItemsSource = centers;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCenters.SelectedItem == null)
            {
                MessageBox.Show("Please select a center first.");
                return;
            }

            // Cast to 'Center' to avoid the InvalidCastException
            SelectedCenter = (Center)cmbCenters.SelectedItem;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}