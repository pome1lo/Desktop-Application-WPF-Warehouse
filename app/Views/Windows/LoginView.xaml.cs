using System.Windows;

namespace app.Views.Windows
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void NavigateRegisterView(object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
            this.Close();
        }

        private void RollUp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
