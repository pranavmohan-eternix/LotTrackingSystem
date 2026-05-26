using System.Windows;
using LTS.Data.Database;
using LTS.UI.Views;

namespace LTS.UI
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // CREATE DATABASE TABLES
            DatabaseInitializer.Initialize();

            // OPEN LOGIN WINDOW
            var login = new LoginSignupWindow();

            login.Show();
        }
    }
}