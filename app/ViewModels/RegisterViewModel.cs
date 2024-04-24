using app.Commands;
using app.Database;
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
    internal class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel()
        {
            this.Db = new UnitOfWork();
            this.users = Db.Users.GetIEnumerable().ToList();
            this.validator = new Validator(this);
        }

        private UnitOfWork Db;
        private List<User> users { get; set; } = new(); 
        private User RegisteredUser= new(); 

        private Validator validator;

        private DelegateCommand? exitCommand;
        private DelegateCommand<RegisterView>? registerCommand;


        public string Email
        {
            get => RegisteredUser.Email;
            set
            {
                RegisteredUser.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => RegisteredUser.Password;
            set
            {
                RegisteredUser.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Login
        {
            get => RegisteredUser.Username;
            set
            {
                RegisteredUser.Username = value;
                OnPropertyChanged(nameof(Login));
            }
        } 

        public string FIO
        {
            get => RegisteredUser.FIO;
            set
            {
                RegisteredUser.FIO = value;
                OnPropertyChanged(nameof(FIO));
            }
        }


        #region Errors

        private string errorPasswordMessage = string.Empty;
        private string errorLoginMessage = string.Empty;
        private string errorEmailMessage = string.Empty; 
        private string errorFIOMessage = string.Empty;

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

        public string ErrorLoginMessage
        {
            get => errorLoginMessage;
            set
            {
                errorLoginMessage = value;
                OnPropertyChanged(nameof(ErrorLoginMessage));
            }
        }
         

        public string ErrorFIOMessage
        {
            get => errorFIOMessage;
            set
            {
                errorFIOMessage = value;
                OnPropertyChanged(nameof(ErrorFIOMessage));
            }
        }
        #endregion

        #region Methods

        private void GoToTheMainPage(Window view)
        {
            //var db = new ApplicationContext();
            ViewModelBase.CurrentUser = Db.Users.GetIEnumerable().FirstOrDefault(x => x.Username == RegisteredUser.Username);
            ShowMainWindow();
            view?.Close();
        }
         
        private bool IsTheUserDataCorrect()
        {
            return (
                validator.Verify(ValidationBased.Password, Password, nameof(ErrorPasswordMessage)) &
                validator.Verify(ValidationBased.TextTo, Login, nameof(ErrorLoginMessage)) &
                validator.Verify(ValidationBased.Email, Email, nameof(ErrorEmailMessage)) &
                validator.Verify(ValidationBased.TextTo, FIO, nameof(ErrorFIOMessage))
            );
        }

        #endregion

        #region Commands

        public ICommand RegisterCommand
        {
            get
            {
                if (registerCommand == null)
                {
                    registerCommand = new DelegateCommand<RegisterView>((RegisterView obj) =>
                    {
                        if (IsTheUserDataCorrect())
                        {
                            if (!users.Any(x => x.Username == RegisteredUser.Username))
                            {
                                RegisteredUser.Password = CryptographerBuilder.Encrypt(RegisteredUser.Password);
                                ///user.Notifications = new() { GetDefaultNotification() };

                                ///new Task(SendMailMessage).Start();

                                //Db.Users.Create(new User()
                                //{
                                //    Username = RegisteredUser.Username,
                                //    FIO = RegisteredUser.FIO,
                                //    Theme = "Light",
                                //    Language = ".ru-RU",
                                //    Email = RegisteredUser.Email,
                                //    IsAdmin = false,
                                //    Password = RegisteredUser.Password,
                                //    ProductsFromBasket = RegisteredUser.ProductsFromBasket,
                                //});

                                var db = new ApplicationContext();
                                db.Users.Add(RegisteredUser);
                                db.SaveChanges();

                                //Db.Users.Create(RegisteredUser);
                                //Db.Save();
                                GoToTheMainPage(obj);
                            }
                            else
                            {
                                SendToModalWindow("A user with this name already exists.");
                            }
                        }
                    });
                }
                return registerCommand;
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
