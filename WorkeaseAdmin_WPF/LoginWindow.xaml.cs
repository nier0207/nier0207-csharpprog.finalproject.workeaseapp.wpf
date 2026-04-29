using System;
using System.Windows;
using System.Windows.Input;

namespace WorkeaseAdmin_WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Professional Validation Logic
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password to proceed.", "Required Fields Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Replace this with your actual database authentication logic
            if (username == "admin" && password == "admin123")
            {
                MainWindow mainDashboard = new MainWindow();
                mainDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("The credentials you entered are incorrect. Please try again.", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        // Enables moving the window since WindowStyle is set to None
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the application?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}