using app.Commands;
using app.Models;
using app.Views.Windows;
using DataEncryption;
using DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static DataValidation.Validator;

namespace app.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            this.validator = new Validator(this);
        }

        private Validator validator;
        private List<User> users { get; set; } = new();
        private User user = new();

        private string errorPasswordMessage = string.Empty;
        private string errorUserNameMessage = string.Empty;
        private string errorEmailMessage = string.Empty;

        private DelegateCommand? exitCommand; 
        private DelegateCommand<Window>? logInCommand;


        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string UserName
        {
            get => user.Username;
            set
            {
                user.Username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

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
                        //GoToTheMainPage(obj);
                        if (IsTheUserDataCorrect())
                        {
                            if (users.Any(x => x.Username == user.Username && CryptographerBuilder.Decrypt(x.Password) == user.Password))
                            {
                                GoToTheMainPage(obj);
                            }
                            else
                            {
                                SendToModalWindow("There is no such user.");
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