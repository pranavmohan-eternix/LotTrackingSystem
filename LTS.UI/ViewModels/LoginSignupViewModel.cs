using LTS.Common.Interfaces;
using LTS.Common.Models;
using LTS.UI.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LTS.UI.ViewModels
{
    public class LoginSignupViewModel : INotifyPropertyChanged //Core WPF interface
    {
        private readonly IUserService _service;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginSignupViewModel(IUserService service)
        {
            _service = service;

            ShowLoginCommand = new RelayCommand(() =>
            {
                ClearSignupFields();

                IsLogin = true;

                OnPropertyChanged(nameof(CurrentSection));
                OnPropertyChanged(nameof(LoginButtonColor));
                OnPropertyChanged(nameof(SignupButtonColor));
            });

            ShowSignupCommand = new RelayCommand(() =>
            {
                ClearLoginFields();

                IsLogin = false;

                OnPropertyChanged(nameof(CurrentSection));
                OnPropertyChanged(nameof(LoginButtonColor));
                OnPropertyChanged(nameof(SignupButtonColor));
            });

            LoginCommand = new RelayCommand(Login);

            SignupCommand = new RelayCommand(Signup);
        }

        // ---------------- STATE ----------------

        private bool _isLogin = true;

        public bool IsLogin
        {
            get => _isLogin;

            set
            {
                _isLogin = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSignup));
            }
        }

        public bool IsSignup => !IsLogin;

        // ---------------- UI STATE ----------------

        public string CurrentSection
        {
            get
            {
                return IsLogin
                    ? "LOGIN"
                    : "CREATE ACCOUNT";
            }
        }

        public string LoginButtonColor
        {
            get
            {
                return IsLogin
                    ? "#4F46E5"
                    : "#D1D5DB";
            }
        }

        public string SignupButtonColor
        {
            get
            {
                return IsSignup
                    ? "#4F46E5"
                    : "#D1D5DB";
            }
        }

        // ---------------- FIELDS ----------------

        public string LoginUsername { get; set; }

        public string LoginPassword { get; set; }

        public string SignupUsername { get; set; }

        public string SignupPassword { get; set; }

        public string Role { get; set; }

        // ---------------- COMMANDS ----------------

        public ICommand ShowLoginCommand { get; }

        public ICommand ShowSignupCommand { get; }

        public ICommand LoginCommand { get; }

        public ICommand SignupCommand { get; }

        // ---------------- LOGIN ----------------

        private void Login()
        {
            var user = _service.Login(
                LoginUsername?.Trim(),
                LoginPassword?.Trim());

            if (user == null)
            {
                MessageBox.Show(
                    "Invalid username or password");

                return;
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var dashboard =
                    new LTS.UI.Views.DashboardWindow(
                        user.Username);

                dashboard.Show();

                System.Windows.Application.Current.MainWindow.Close();
            });
        }

        // ---------------- SIGNUP ----------------

        private void Signup()
        {
            try
            {
                var user = new User
                {
                    Username = SignupUsername?.Trim(),
                    Password = SignupPassword?.Trim(),
                    Role = Role
                };

                _service.Register(user);

                MessageBox.Show(
                    "Account created successfully");

                ClearSignupFields();

                IsLogin = true;

                OnPropertyChanged(nameof(CurrentSection));
                OnPropertyChanged(nameof(LoginButtonColor));
                OnPropertyChanged(nameof(SignupButtonColor));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ---------------- CLEAR METHODS ----------------

        private void ClearLoginFields()
        {
            LoginUsername = string.Empty;

            LoginPassword = string.Empty;

            OnPropertyChanged(nameof(LoginUsername));
            OnPropertyChanged(nameof(LoginPassword));
        }

        private void ClearSignupFields()
        {
            SignupUsername = string.Empty;

            SignupPassword = string.Empty;

            OnPropertyChanged(nameof(SignupUsername));
            OnPropertyChanged(nameof(SignupPassword));
        }

        // ---------------- NOTIFY ----------------

        private void OnPropertyChanged(
            [CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}