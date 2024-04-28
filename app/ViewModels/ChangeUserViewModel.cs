using app.Commands;
using app.Database;
using app.Models;
using app.Views.Windows;
using DataEncryption;
using DataValidation;
using System.Windows.Input;
using static DataValidation.Validator;

namespace app.ViewModels
{
    internal class ChangeUserViewModel : ViewModelBase
    {
        public ChangeUserViewModel(User user)
        {
            this.user = user;
            this.validator = new Validator(this);
            Db = new UnitOfWork();
            Password = CryptographerBuilder.Decrypt(user.Password);
        }

        private UnitOfWork Db;
        private User user = new();
        private Validator validator;

        private string errorUserName = string.Empty;
        private string errorPassword = string.Empty;
        private string errorEmail = string.Empty;
        private string errorFIO = string.Empty;

        private DelegateCommand<ChangeUserView>? editNewUserCommand;

        #region Property

        public string Username
        {
            get => user.Username;
            set
            {
                user.Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsAdmin
        {
            get => user.IsAdmin;
            set
            {
                user.IsAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public string FIO
        {
            get => user.FIO;
            set
            {
                user.FIO = value;
                OnPropertyChanged(nameof(FIO));
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

        public string ErrorUserName
        {
            get => errorUserName;
            set
            {
                errorUserName = value;
                OnPropertyChanged(nameof(ErrorUserName));
            }
        }

        public string ErrorPassword
        {
            get => errorPassword;
            set
            {
                errorPassword = value;
                OnPropertyChanged(nameof(ErrorPassword));
            }
        }

        public string ErrorEmail
        {
            get => errorEmail;
            set
            {
                errorEmail = value;
                OnPropertyChanged(nameof(ErrorEmail));
            }
        }

        public string ErrorFIO
        {
            get => errorFIO;
            set
            {
                errorFIO = value;
                OnPropertyChanged(nameof(ErrorFIO));
            }
        }

        #endregion

        #endregion
         
        #region Methods

        private bool NewUserValidate()
        {
            return validator.Verify(ValidationBased.TextTo, Username, nameof(ErrorUserName)) &
                validator.Verify(ValidationBased.Password, Password, nameof(ErrorPassword)) &
                validator.Verify(ValidationBased.TextTo, FIO, nameof(ErrorFIO)) &
                validator.Verify(ValidationBased.Email, Email, nameof(ErrorEmail));
        }

        #endregion

        #region Commands

        public ICommand EditUserCommand
        {
            get
            {
                if (editNewUserCommand == null)
                {
                    editNewUserCommand = new DelegateCommand<ChangeUserView>((ChangeUserView view) =>
                    {
                        if (NewUserValidate())
                        {
                            user.Password = CryptographerBuilder.Encrypt(user.Password);
                            var db = new ApplicationContext();
                            db.Users.Update(user);
                            db.SaveChanges();

                            SendToModalWindow("Данные успешно изменены.");
                            view.Close();
                        }
                    });
                }
                return editNewUserCommand;
            }
        }

        #endregion
    }
}
