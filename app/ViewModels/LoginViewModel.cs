using app.Commands;
using app.Database;
using app.Models;
using DataEncryption;
using DataValidation;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static DataValidation.Validator;

namespace app.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            this.Db = new UnitOfWork();
            this.validator = new Validator(this);
            this.users = Db.Users.GetIEnumerable().ToList();
        }

        private UnitOfWork Db;
        private Validator validator;
        private List<User> users { get; set; }

        private string username = string.Empty;
        private string password = string.Empty;

        private string errorPasswordMessage = string.Empty;
        private string errorUserNameMessage = string.Empty;
        private string errorEmailMessage = string.Empty;

        private DelegateCommand? exitCommand;
        private DelegateCommand<Window>? logInCommand;

        #region Property

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string UserName
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        #endregion

        #region Errors

        public string ErrorEmailMessage
        {
            get => errorEmailMessage;
            set
            {
                errorEmailMessage = value;
                OnPropertyChanged(nameof(ErrorEmailMessage));
            }
        }

        public string ErrorPasswordMessage
        {
            get => errorPasswordMessage;
            set
            {
                errorPasswordMessage = value;
                OnPropertyChanged(nameof(ErrorPasswordMessage));
            }
        }

        public string ErrorUserNameMessage
        {
            get => errorUserNameMessage;
            set
            {
                errorUserNameMessage = value;
                OnPropertyChanged(nameof(ErrorUserNameMessage));
            }
        }

        #endregion

        #region Methods

        private void GoToTheMainPage(Window view)
        {
            CurrentUser = Db.Users.GetIEnumerable().FirstOrDefault(x => x.Username == UserName);
            ShowMainWindow();
            view?.Close();
        }

        private bool IsTheUserDataCorrect()
        {
            return validator.Verify(ValidationBased.Password, Password, nameof(ErrorPasswordMessage)) &
                validator.Verify(ValidationBased.TextTo, UserName, nameof(ErrorUserNameMessage));
        }

        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                if (logInCommand == null)
                {
                    logInCommand = new DelegateCommand<Window>((Window obj) =>
                    {
                        if (IsTheUserDataCorrect())
                        {
                            if (users.Any(x => x.Username == UserName && CryptographerBuilder.Decrypt(x.Password) == Password))
                            {
                                GoToTheMainPage(obj);
                            }
                            else
                            {
                                SendToModalWindow("Пользователь не найден");
                            }
                        }
                    });
                }
                return logInCommand;
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

        #endregion
    }
}