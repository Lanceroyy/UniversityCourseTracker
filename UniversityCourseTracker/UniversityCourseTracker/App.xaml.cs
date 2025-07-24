using C971App.Views;


namespace C971App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            // This code changes the main page upon starting the app.

            /*
            var TermListPage = new TermList();
            var navPage = new NavigationPage(TermListPage);
            MainPage = navPage;
            */

            Application.Current.UserAppTheme = AppTheme.Light;

            var LoginPagePage = new LoginPage();
            var navPage = new NavigationPage(LoginPagePage);
            MainPage = navPage;



            //MainPage = new AppShell(); //Original Code
        }
    }
}
