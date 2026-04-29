using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WorkeaseAdmin_WPF.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // ITO ANG MAHAHALAGANG CODE PARA MAWALA ANG ERROR:
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Pansamantala, papasukin muna natin kahit anong ilagay
            // Pag-aralan natin ang database connection mamaya
            if (txtUsername.Text == "admin" && txtPassword.Password == "1234")
            {
                // Kung tama, lipat sa Dashboard (o kung anong page ang gusto mo)
                NavigationService.Navigate(new DashboardPage());
            }
            else
            {
                MessageBox.Show("Maling Username o Password, pre!");
            }
        }
    }
}