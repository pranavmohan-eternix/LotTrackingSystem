using System.ComponentModel;
using System.Windows;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class LoginSignupWindow : Window
    {
        private bool isLoginVisible = false;
        private bool isSignupVisible = false;

        private LoginSignupViewModel vm;

        public LoginSignupWindow()
        {
            InitializeComponent();

            var repo = new UserRepository();
            var service = new UserService(repo);

            vm = new LoginSignupViewModel(service);

            DataContext = vm;

            // LOGIN PASSWORD BINDING
            LoginPasswordBox.PasswordChanged += (s, e) =>
            {
                vm.LoginPassword = LoginPasswordBox.Password;
            };

            // SIGNUP PASSWORD BINDING
            SignupPasswordBox.PasswordChanged += (s, e) =>
            {
                vm.SignupPassword = SignupPasswordBox.Password;
            };

            // CLEAR PASSWORDS WHEN SWITCHING
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(vm.IsLogin))
            {
                ClearLoginPassword();
                ClearSignupPassword();
            }
        }

        // CLEAR LOGIN PASSWORD
        private void ClearLoginPassword()
        {
            LoginPasswordBox.Clear();
            LoginPasswordText.Clear();
        }

        // CLEAR SIGNUP PASSWORD
        private void ClearSignupPassword()
        {
            SignupPasswordBox.Clear();
            SignupPasswordText.Clear();
        }

        // LOGIN PASSWORD TOGGLE
        private void ToggleLoginPassword(object sender, RoutedEventArgs e)
        {
            if (isLoginVisible)
            {
                LoginPasswordBox.Password = LoginPasswordText.Text;

                LoginPasswordBox.Visibility = Visibility.Visible;
                LoginPasswordText.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginPasswordText.Text = LoginPasswordBox.Password;

                LoginPasswordBox.Visibility = Visibility.Collapsed;
                LoginPasswordText.Visibility = Visibility.Visible;
            }

            isLoginVisible = !isLoginVisible;
        }

        // SIGNUP PASSWORD TOGGLE
        private void ToggleSignupPassword(object sender, RoutedEventArgs e)
        {
            if (isSignupVisible)
            {
                SignupPasswordBox.Password = SignupPasswordText.Text;

                SignupPasswordBox.Visibility = Visibility.Visible;
                SignupPasswordText.Visibility = Visibility.Collapsed;
            }
            else
            {
                SignupPasswordText.Text = SignupPasswordBox.Password;

                SignupPasswordBox.Visibility = Visibility.Collapsed;
                SignupPasswordText.Visibility = Visibility.Visible;
            }

            isSignupVisible = !isSignupVisible;
        }
    }
}