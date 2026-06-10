using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Pages;
using WorkeaseAdmin_WPF.Services;

namespace WorkeaseAdmin_WPF
{
    public partial class MainWindow : Window
    {
        private readonly SessionManager _session;

        public MainWindow()
        {
            InitializeComponent();

            _session = App.Services.GetRequiredService<SessionManager>();
            var profile = _session.GetProfile();

            if (profile is null)
            {
                MessageBox.Show("User profile not found. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            // Bind profiles meta elements securely onto the layout containers
            txtUserName.Text = profile.UserName ?? "User";
            txtUserEmail.Text = profile.UserEmail ?? "No Email";
            txtUserType.Text = profile.UserType?.ToUpper() ?? "STAFF";

            // Load initial dashboard default presentation window
            MainFrame.Navigate(App.Services.GetRequiredService<DashboardPage>());
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string tag = btn.Tag.ToString();
                PageTitle.Text = tag;

                switch (tag)
                {
                    case "Dashboard":
                        MainFrame.Navigate(App.Services.GetRequiredService<DashboardPage>());
                        break;
                    case "Centers":
                        MainFrame.Navigate(App.Services.GetRequiredService<CentersPage>());
                        break;
                    case "Workers":
                        MainFrame.Navigate(App.Services.GetRequiredService<WorkersPage>());
                        break;
                    case "Children":
                        MainFrame.Navigate(App.Services.GetRequiredService<ChildrenPage>());
                        break;
                    case "Attendance":
                        MainFrame.Navigate(App.Services.GetRequiredService<AttendancePage>());
                        break;
                    case "Health":
                        MainFrame.Navigate(App.Services.GetRequiredService<HealthPage>());
                        break;
                    case "Fees":
                        MainFrame.Navigate(App.Services.GetRequiredService<FeesPage>());
                        break;
                    case "Reports":
                        MainFrame.Navigate(App.Services.GetRequiredService<ReportsPage>());
                        break;
                }
            }
        }

        /// <summary>
        /// HANDLER EVENT: Clears user session context state and resets presentation layer window back to LoginPage.
        /// </summary>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to log out from Workease Admin?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Clean memory footprint profile references 
                    _session.ClearSession();

                    // Swap out standard layout windows presentation threads safely
                    var loginWindow = App.Services.GetRequiredService<LoginWindow>();
                    loginWindow.Show();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during logout: {ex.Message}", "Logout Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}