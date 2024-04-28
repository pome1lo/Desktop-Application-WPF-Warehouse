using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlLibrary.Components
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

        public bool IsDarkTheme
        {
            get { return (bool)GetValue(IsDarkThemeProperty); }
            set { SetValue(IsDarkThemeProperty, value); }
        }
        #region Dependecy Property

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsDarkThemeProperty =
          DependencyProperty.Register("IsDarkTheme", typeof(bool), typeof(BindablePasswordBox),
              new PropertyMetadata(false, OnIsDarkThemePropertyChanged));

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

        private static void OnIsDarkThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox control && control.passwordBox != null)
            {
                var isDarkTheme = (bool)e.NewValue;
                var newColor = isDarkTheme ? "#15193b" : "#FFB793";
                control.passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColor));
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                passwordBox = this.Template.FindName("passwordBox", this) as PasswordBox;
                var newColor = IsDarkTheme ? "#15193b" : "#FFB793";
                passwordBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(newColor));
                passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
            }
        }

        #endregion
    }
}
