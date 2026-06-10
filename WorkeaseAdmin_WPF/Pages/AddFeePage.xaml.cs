using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddFeePage : Page
    {
        private readonly FeeService _feeService;
        private readonly ChildService _childService;
        private readonly SessionManager _sessionManager;
        private int? _searchedChildId = null;

        public AddFeePage()
        {
            InitializeComponent();

            // Dependency Injection
            _feeService = App.Services.GetRequiredService<FeeService>();
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

        private async void CreatePayment_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation: Selected Child
            if (_searchedChildId == null)
            {
                MessageBox.Show("Please search and select a child first.");
                return;
            }

            // 2. Validation: Session/User ID
            var profile = _sessionManager.GetProfile();
            if (profile == null || profile.UserId == 0)
            {
                MessageBox.Show("Session expired. Please log in again.");
                return;
            }

            // 3. Validation: Amount (Numeric)
            if (!decimal.TryParse(txtAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Please enter a valid numeric amount.");
                return;
            }

            // 4. Validation: Date Selection (Month and Year)
            if (cmbMonth.SelectedItem == null || cmbYear.SelectedItem == null)
            {
                MessageBox.Show("Please select both a month and a year for the fee record.");
                return;
            }

            // Extract Month (from Tag) and Year (from Content)
            int month = int.Parse(((ComboBoxItem)cmbMonth.SelectedItem).Tag.ToString());
            int year = int.Parse(((ComboBoxItem)cmbYear.SelectedItem).Content.ToString());

            // 5. Construct DTO
            var newFeeDto = new CreateFeeDto
            {
                ChildId = _searchedChildId.Value,
                FeeRecordAmount = amount,
                FeeRecordMonth = month,
                FeeRecordYear = year
            };

            try
            {
                // 6. Call Service
                var result = await _feeService.CreateFeeRecordAsync(newFeeDto, profile.UserId);

                if (result != null)
                {
                    MessageBox.Show("Payment record successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.NavigationService?.Navigate(new FeesPage());
                }
                else
                {
                    MessageBox.Show("Failed to save the payment record. Please check your connection.");
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
        private void BackToFees_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new FeesPage());
        private void EditFee_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new EditFeePage());
        private void DeleteFee_Click(object sender, RoutedEventArgs e) => this.NavigationService?.Navigate(new DeleteFeePage());
    }
}