using app.Commands;
using app.Database;
using app.Views.Pages;
using app.Views.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace app.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Db = new UnitOfWork();
            ChangeTheme(CurrentUser.Theme);
            ChangeLanguage(CurrentUser.Language);
            ConfiguringApplicationForUserType();
            ShowPage(new MenuView());
        }

        private Visibility visibilityAdminButton = Visibility.Collapsed;
        private bool isThemeFirstImage = true;
        private bool isLangFirstImage = true;
        private UnitOfWork Db;
        private MainView? view;

        private ImageSource currentLogoThemeImage = new BitmapImage(new Uri("\\Static\\Img\\LightLogo.png", UriKind.Relative));
        private ImageSource currentThemeImage = new BitmapImage(new Uri("\\Static\\Img\\ThemeLight.png", UriKind.Relative));
        private ImageSource currentLangImage = new BitmapImage(new Uri("\\Static\\Img\\rus.png", UriKind.Relative));
         
        private DelegateCommand? showMenuCommand;
        private DelegateCommand<object> exitAccountCommand;
        private DelegateCommand? exitCommand;
        private DelegateCommand? showAdminCommand;
        private DelegateCommand? showProfileCommand;
        private DelegateCommand? showMapCommand;
        private DelegateCommand? showStatisticsPage;
        private DelegateCommand? showBasketCommand;
        private DelegateCommand? showAboutCommand;
        private DelegateCommand toggleImageLangCommand;
        private DelegateCommand? showHomeCommand;
        private DelegateCommand toggleImageThemeCommand;
        private Brush colorHeaderButton;

        #region Property

        public Visibility VisibilityAdminButton
        {
            get => visibilityAdminButton;
            set
            {
                visibilityAdminButton = value;
                OnPropertyChanged(nameof(VisibilityAdminButton));
            }
        }

        public Brush ColorHeaderButton
        {
            get => colorHeaderButton;
            set
            {
                if (colorHeaderButton != value)
                {
                    colorHeaderButton = value;
                    OnPropertyChanged(nameof(ColorHeaderButton));
                }
            }
        }

        public ImageSource CurrentThemeImage
        {
            get => currentThemeImage;
            set
            {
                currentThemeImage = value;
                OnPropertyChanged(nameof(CurrentThemeImage));
            }
        }

        public ImageSource CurrentLogoThemeImage
        {
            get => currentLogoThemeImage;
            set
            {
                currentLogoThemeImage = value;
                OnPropertyChanged(nameof(CurrentLogoThemeImage));
            }
        }

        public ImageSource CurrentLangImage
        {
            get => currentLangImage;
            set
            {
                currentLangImage = value;
                OnPropertyChanged(nameof(CurrentLangImage));
            }
        }

        #endregion

        #region Methods

        private void ConfiguringApplicationForUserType()
        {
            if (CurrentUser.IsAdmin)
            {
                SettingUpForAdmin();
            }
        }

        private void SettingUpForAdmin()
        {
            VisibilityAdminButton = Visibility.Visible;
        }

        private void ChangeLanguage(string Language)
        {
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri(
                        String.Format($"Static/Resources/Lang{Language}.xaml"),
                        UriKind.Relative
                    )
                }
            );
            CurrentUser.Language = Language;
            Db.Save();
        }


        private void ChangeTheme(string Theme)
        {
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri(
                        String.Format($"Static/Themes/{Theme}.xaml"),
                        UriKind.Relative
                    )
                }
            );
            CurrentUser.Theme = Theme;
            ColorHeaderButton = (Theme == "Dark") ? Brushes.White : Brushes.Black;
            Db.Save();
        }

        #endregion

        #region Commands

        public ICommand ExitAccountCommand
        {
            get
            {
                if (exitAccountCommand == null)
                {
                    exitAccountCommand = new DelegateCommand<object>((object obj) =>
                    {
                        new LoginView().Show();
                        (obj as MainView)?.Close();
                    });
                }
                return exitAccountCommand;
            }
        }

        public ICommand ShowAdminCommand
        {
            get
            {
                if (showAdminCommand == null)
                {
                    showAdminCommand = new DelegateCommand(() =>
                    {
                        ShowPage(new AdminView());
                    });
                }
                return showAdminCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new DelegateCommand(
                        System.Windows.Application.Current.Shutdown
                        );
                }
                return exitCommand;
            }
        }

        public ICommand ShowHomeCommand
        {
            get
            {
                if (showHomeCommand == null)
                {
                    showHomeCommand = new DelegateCommand(() =>
                    {
                        ShowPage(new MenuView());
                    });
                }
                return showHomeCommand;
            }
        }

        public ICommand ShowStatisticsPage
        {
            get
            {
                if (showStatisticsPage == null)
                {
                    showStatisticsPage = new DelegateCommand(() =>
                    {
                        ShowPage(new StatisticsView());
                    });
                }
                return showStatisticsPage;
            }
        }
        
        public ICommand ShowMenuCommand
        {
            get
            {
                if (showMenuCommand == null)
                {
                    showMenuCommand = new DelegateCommand(() =>
                    {
                        ShowPage(new MenuView());
                    });
                }
                return showMenuCommand;
            }
        }

        public ICommand ShowBasketCommand
        {
            get
            {
                if (showBasketCommand == null)
                {
                    showBasketCommand = new DelegateCommand(() =>
                    {
                        ShowPage(new BasketView());
                    });
                }
                return showBasketCommand;
            }
        }

        public ICommand ShowProfileCommand
        {
            get
            {
                if (showProfileCommand == null)
                {
                    showProfileCommand = new DelegateCommand(() =>
                    {
                        ShowPage(new ProfileView());
                    });
                }
                return showProfileCommand;
            }
        }
        private bool isDarkTheme = false; // По умолчанию стартовая тема - светлая

        public ICommand ToggleImageThemeCommand
        {
            get
            {
                if (toggleImageThemeCommand == null)
                {
                    toggleImageThemeCommand = new DelegateCommand(() =>
                    {
                        if (isThemeFirstImage)
                        {
                            CurrentLogoThemeImage = new BitmapImage(new Uri("\\Static\\Img\\DarkLogo.png", UriKind.Relative));
                            CurrentThemeImage = new BitmapImage(new Uri("\\Static\\Img\\ThemeDark.png", UriKind.Relative));
                            new Task(() => { ChangeTheme("Dark"); }).Start();
                            isThemeFirstImage = false;
                        }
                        else
                        {
                            CurrentLogoThemeImage = new BitmapImage(new Uri("\\Static\\Img\\LightLogo.png", UriKind.Relative));
                            CurrentThemeImage = new BitmapImage(new Uri("\\Static\\Img\\ThemeLight.png", UriKind.Relative));
                            new Task(() => { ChangeTheme("Light"); }).Start();
                            isThemeFirstImage = true;
                        }
                    });
                }
                return toggleImageThemeCommand;
            }
        }

        public ICommand ToggleImageLangCommand
        {
            get
            {
                if (toggleImageLangCommand == null)
                {
                    toggleImageLangCommand = new DelegateCommand(() =>
                    {
                        if (isLangFirstImage)
                        {
                            CurrentLangImage = new BitmapImage(new Uri("\\Static\\Img\\en.png", UriKind.Relative));
                            ChangeLanguage(".en-US");
                            isLangFirstImage = false;

                        }
                        else
                        {
                            CurrentLangImage = new BitmapImage(new Uri("\\Static\\Img\\rus.png", UriKind.Relative));
                            ChangeLanguage(".ru-RU");
                            isLangFirstImage = true;
                        }
                    });
                }
                return toggleImageLangCommand;
            }
        } 
        #endregion
    }
}
