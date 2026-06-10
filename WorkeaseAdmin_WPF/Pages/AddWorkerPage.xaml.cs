using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Required for Regex parsing
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorkeaseAdmin_WPF.Models;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class AddWorkerPage : Page
    {
        private readonly UserService _userService = new UserService();
        private readonly CenterService _centerService = new CenterService();

        public AddWorkerPage()
        {
            InitializeComponent();
            LoadCenters();
        }

        private async void LoadCenters()
        {
            try
            {
                var centers = await _centerService.GetAllCentersAsync();
                cmbCenter.ItemsSource = centers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading centers: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string GetPasswordText()
        {
            return txtPassword.Visibility == Visibility.Visible ? txtPassword.Password : txtPasswordUnmasked.Text;
        }

        private void btnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPasswordUnmasked.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordUnmasked.Visibility = Visibility.Visible;

                imgEye.Source = new BitmapImage(new Uri("pack://application:,,,/Images/eye_hidden.png"));
                txtPasswordUnmasked.Focus();
            }
            else
            {
                txtPassword.Password = txtPasswordUnmasked.Text;
                txtPasswordUnmasked.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;

                imgEye.Source = new BitmapImage(new Uri("pack://application:,,,/Images/eye_visible.png"));
                txtPassword.Focus();
            }
        }

        private async void btnCreateWorker_Click(object sender, RoutedEventArgs e)
        {
            string emailText = txtEmail.Text.Trim();
            string passwordText = GetPasswordText();

            // 1. Basic Required Fields Validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(emailText) ||
                string.IsNullOrWhiteSpace(passwordText))
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Exact Email Suffix Validation (@workease.burgos.ph)
            // Ensures the string ends directly with the domain format
            if (!emailText.EndsWith("@workease.burgos.ph", StringComparison.OrdinalIgnoreCase) || emailText.Length <= 19)
            {
                MessageBox.Show("Email address must use the official corporate domain pattern:\n'example@workease.burgos.ph'",
                                "Invalid Email Pattern", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Strict Password Complexity Validation
            // ^(?=.*[a-z]) : At least one lowercase letter
            // anisotropy (?=.*[A-Z]) : At least one uppercase letter
            // (?=.*\d) : At least one digit
            // (?=.*[^\da-zA-Z]) : At least one special character
            // .{8,} : Minimum 8 characters in total length
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            if (!Regex.IsMatch(passwordText, passwordPattern))
            {
                MessageBox.Show("Password does not meet the complexity requirements:\n\n" +
                                "• Must be at least 8 characters long\n" +
                                "• Must contain at least 1 uppercase letter\n" +
                                "• Must contain at least 1 lowercase letter\n" +
                                "• Must contain at least 1 numeric digit\n" +
                                "• Must contain at least 1 special character (e.g., !, @, #, $, %)",
                                "Weak Password", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select a Role", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 4. CenterId Logic: Nullable for Parent/Admin, Required for CDW
            int? selectedCenterId = cmbCenter.SelectedValue as int?;

            if (selectedRole == "CDW" && selectedCenterId == null)
            {
                MessageBox.Show("A Center must be assigned for CDW workers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 5. Prepare the DTO
            var newUserDto = new CreateUserDto
            {
                UserName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}",
                UserEmail = emailText,
                UserHashPassword = passwordText,
                UserType = selectedRole,
                CenterId = selectedCenterId
            };

            // 6. Call the Service
            try
            {
                var createdUser = await _userService.CreateUserAsync(newUserDto);

                if (createdUser != null)
                {
                    MessageBox.Show($"Worker '{createdUser.UserName}' was registered successfully with ID: {createdUser.UserId}",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to create worker. The email might already be registered or the server rejected the request.",
                                    "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtPasswordUnmasked.Clear();

            txtPasswordUnmasked.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            imgEye.Source = new BitmapImage(new Uri("pack://application:,,,/Images/eye_visible.png"));

            cmbRole.SelectedIndex = -1;
            cmbCenter.SelectedIndex = -1;
        }

        // Sidebar Navigation
        private void ManageWorkers_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<WorkersPage>();
            this.NavigationService?.Navigate(page);
        }

        private void EditWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<EditWorkerPage>();
            this.NavigationService?.Navigate(page);
        }

        private void DeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            var page = App.Services.GetRequiredService<DeleteWorkerPage>();
            this.NavigationService?.Navigate(page);
        }
    }
}