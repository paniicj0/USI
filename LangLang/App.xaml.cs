using LangLang.CLI;
using LangLang.Controllers;
using LangLang.RepositorySQL;
using LangLang.View;
using System.Windows;
using LangLang.RepositorySQL;
namespace LangLang
{
    public partial class App : Application
    {
        public Login loginCLI = new Login();
        public LogInWindow logInWindow = new LogInWindow();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            logInWindow.Show();
            //CLIStartup();

        }

        // startup for wpf application
        private void WPFStartup()
        {
            LogInWindow logInWindow = new LogInWindow();
           // tutor.LoadFromDatabase();
        }

        // startup for cli application
        private void CLIStartup()
        {
            loginCLI.Startup();
        }
    }
}
