using System.Windows;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow(
            string username)
        {
            InitializeComponent();

            DataContext =
                new DashboardViewModel(
                    username);
        }

        // LOGOUT

        private void Logout_Click(
            object sender,
            RoutedEventArgs e)
        {
            var login =
                new LoginSignupWindow();

            login.Show();

            Close();
        }
    }
}