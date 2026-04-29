using System.Windows;
using System.Windows.Controls;
using WorkeaseAdmin_WPF.Pages; // Siguraduhin na may 'Pages' folder ka

namespace WorkeaseAdmin_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load Dashboard by default
            MainFrame.Navigate(new DashboardPage());
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                string tag = btn.Tag.ToString();
                PageTitle.Text = tag;

                switch (tag)
                {
                    case "Dashboard": MainFrame.Navigate(new DashboardPage()); break;
                    case "Centers": MainFrame.Navigate(new CentersPage()); break;
                    case "Workers": MainFrame.Navigate(new WorkersPage()); break;
                    case "Children": MainFrame.Navigate(new ChildrenPage()); break;
                    case "Attendance": MainFrame.Navigate(new AttendancePage()); break; // ADD THIS
                    case "Health": MainFrame.Navigate(new HealthPage()); break;         // ADD THIS
                    case "Fees":MainFrame.Navigate(new Pages.FeesPage());break;
                    case "Reports":MainFrame.Navigate(new Pages.ReportsPage());break;
                }
            }
        }
    }
}