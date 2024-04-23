using System.Windows;

namespace app.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        public ModalWindow(string content)
        {
            InitializeComponent();
            this.textBlock.Text = content;
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
         
        private void RollUp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
