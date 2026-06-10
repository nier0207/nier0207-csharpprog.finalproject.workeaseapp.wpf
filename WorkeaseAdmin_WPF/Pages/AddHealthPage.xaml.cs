using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization; 
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddHealthPage : Page
    {
        private readonly HealthService _healthService;
        private readonly ChildService _childService;
        private readonly SessionManager _sessionManager;
        private int? _searchedChildId = null;

        public AddHealthPage()
        {
            InitializeComponent();

            // Dependency Injection
            _healthService = App.Services.GetRequiredService<HealthService>();
            _childService = App.Services.GetRequiredService<ChildService>();
            _sessionManager = App.Services.GetRequiredService<SessionManager>();
        }

        private async void SearchChild_Click(object sender, RoutedEventArgs e)
        {
            string inputId = SearchChildID.Text.Trim();

            if (string.IsNullOrEmpty(inputId) || !int.TryParse(inputId, out int parsedId))
            {
                MessageBox.Show("Please enter a valid numeric Child ID.");
                return;
            }

            try
            {
                var child = await _childService.GetChildByIdAsync(parsedId);

                if (child != null)
                {
                    // Update UI with the child's full name
                    txtChildName.Text = child.ChildFullName;
                    _searchedChildId = child.ChildId;
                }
                else
                {
                    MessageBox.Show("Child record not found.");
                    ResetSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}");
                ResetSelection();
            }
        }

        private async void btnCreateRecord_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation: Selected Child
            if (_searchedChildId == null)
            {
                MessageBox.Show("Please search and select a child first.");
                return;
            }

            // 2. Validation: Session/Worker ID
            var profile = _sessionManager.GetProfile();
            if (profile == null || profile.UserId == 0)
            {
                MessageBox.Show("Session expired or Worker ID invalid. Please log in again.");
                return;
            }

            // 3. Validation: Numeric Inputs (Culture Invariant)
            if (!decimal.TryParse(txtWeight.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight) ||
                !decimal.TryParse(txtHeight.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal height))
            {
                MessageBox.Show("Please enter valid numbers (e.g., 12.5) for weight and height.");
                return;
            }

            // 4. Construct DTO
            var newHealthRecord = new CreateHealthDto
            {
                ChildId = _searchedChildId.Value,
                HealthRecordDate = DateTime.Now,
                HealthRecordWeigtKg = weight,
                HealthRecordHeightCm = height,
                HealthRecordNotes = txtNotes.Text ?? string.Empty
            };

            try
            {
                // 5. Call Service
                var result = await _healthService.CreateHealthRecordAsync(newHealthRecord, profile.UserId);

                if (result != null)
                {
                    MessageBox.Show("Health Record successfully registered!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Navigate back to the summary page
                    this.NavigationService?.Navigate(new HealthPage());
                }
                else
                {
                    MessageBox.Show("Failed to save. The server rejected the data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving record: {ex.Message}");
            }
        }

        private void ResetSelection()
        {
            txtChildName.Text = "No child selected...";
            _searchedChildId = null;
        }

        // --- Navigations ---
        private void ManageHealth_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new HealthPage());
        private void EditHealth_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new EditHealthPage());
        private void DeleteHealth_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new DeleteHealthPage());
    }
}