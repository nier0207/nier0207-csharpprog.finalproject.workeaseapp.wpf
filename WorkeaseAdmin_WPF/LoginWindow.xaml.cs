using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF
{
    public partial class LoginWindow : Window
    {
        // Changed from ApiService to AuthService
        private readonly AuthService _authService;
        private readonly SessionManager _session;

        public LoginWindow()
        {
            InitializeComponent();

            // Request the specific AuthService from the provider
            _authService = App.Services.GetRequiredService<AuthService>();
            _session = App.Services.GetRequiredService<SessionManager>();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Please enter your email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                ShowError("Please enter your password.");
                return;
            }

            SetBusy(true);
            HideError();

            try
            {
                // ── Step 1: Login using AuthService ───────────────────
                var response = await _authService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Password);

                if (response is null)
                {
                    ShowError("Invalid email or password.");
                    return;
                }

                if (response.UserType != "Admin")
                {
                    ShowError("Access denied. Admin accounts only.");
                    return;
                }

                // ── Step 2: Save token to session ─────────────────────
                _session.SaveSession(response);

                // ── Step 3: Fetch full profile using AuthService ──────
                var profile = await _authService.GetProfileAsync();

                if (profile is null)
                {
                    ShowError("Failed to load user profile.");
                    return;
                }

                // ── Step 4: Save profile to session ───────────────────
                _session.SaveProfile(profile);

                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (HttpRequestException)
            {
                ShowError("Cannot connect to server.\nMake sure the API is running.");
            }
            catch (Exception ex)
            {
                ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        // ... Keep existing Window_MouseDown, ShowError, HideError, and SetBusy ...
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            lblError.Text = string.Empty;
            lblError.Visibility = Visibility.Collapsed;
        }

        private void SetBusy(bool busy)
        {
            btnLogin.IsEnabled = !busy;
            btnLogin.Content = busy ? "Logging in..." : "LOGIN";
            txtUsername.IsEnabled = !busy;
            txtPassword.IsEnabled = !busy;
            Cursor = busy ? Cursors.Wait : Cursors.Arrow;
        }
    }
}