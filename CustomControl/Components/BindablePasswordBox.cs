using System.Windows;
using System.Windows.Controls;

namespace CustomControl.Components
{
    [TemplatePart(Name = "passwordBox", Type = typeof(PasswordBox))]
    public partial class BindablePasswordBox : UserControl
    {
        #region Property

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        #region Dependecy Property

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new PropertyMetadata(string.Empty));


        #endregion

        #endregion

        #region Constructor

        static BindablePasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(typeof(BindablePasswordBox)));
        }

        #endregion

        #region Fields

        private PasswordBox passwordBox;

        #endregion

        #region Methods

        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                passwordBox = this.Template.FindName("passwordBox", this) as PasswordBox;
                passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
            }
        }

        #endregion
    }
}
