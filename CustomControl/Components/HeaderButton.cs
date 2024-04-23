using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControl.Components
{
    [TemplatePart(Name = "img", Type = typeof(Image))]
    public class HeaderButton : UserControl
    {
        #region Property

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public ImageSource ImgSource
        {
            get { return (ImageSource)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        }

        #region Dependency property

        public static readonly DependencyProperty CommandProperty =
          DependencyProperty.Register("Command", typeof(ICommand), typeof(HeaderButton),
              new PropertyMetadata(null));

        public static readonly DependencyProperty ImgSourceProperty =
            DependencyProperty.Register("ImgSource", typeof(ImageSource), typeof(HeaderButton));

        #endregion

        #endregion

        #region Fields

        private Image img;

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Template != null)
            {
                img = this.Template.FindName("img", this) as Image;
                img.Source = this.ImgSource;
            }
        }

        #endregion

        #region Constructor

        static HeaderButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderButton),
              new FrameworkPropertyMetadata(typeof(HeaderButton)));
        }

        #endregion
    }
}
