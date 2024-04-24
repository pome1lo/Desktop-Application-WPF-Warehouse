using app.Commands;
using app.Database;
using app.Models;
using app.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace app.ViewModels
{
    internal class AdminViewModel : ViewModelBase
    {
        public AdminViewModel()
        {
            this.Db = new UnitOfWork();
            Users = Db.Users.GetIEnumerable().ToList();
        }


        private UnitOfWork Db;
        private List<User> users { get; set; }


        private User selectedItemForUsersDB = new();

        private DelegateCommand<User>? deleteUserCommand;
        private DelegateCommand<User>? editUserCommand;

        public List<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public User SelectedItemForUsersDB
        {
            get => selectedItemForUsersDB;
            set
            {
                selectedItemForUsersDB = value;
                OnPropertyChanged(nameof(SelectedItemForUsersDB));
            }
        }


        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                {
                    deleteUserCommand = new DelegateCommand<User>((User user) =>
                    { 
                        if(user.Id == CurrentUser.Id)
                        {
                            SendToModalWindow("Вы не можете удалить сами себя");
                        }
                        else
                        {
                            Db.Users.Delete(user.Id);
                            Db?.Save();
                            Users = Db.Users.GetIEnumerable().ToList();
                            SendToModalWindow("Пользователь успешно удален");
                        }
                    });
                }
                return deleteUserCommand;
            }
        }

        public ICommand EditUserCommand
        {
            get
            {
                if (editUserCommand == null)
                {
                    editUserCommand = new DelegateCommand<User>((User user) =>
                    {
                        var view = new ChangeUserView();
                        var viewModel = new ChangeUserViewModel(user);
                        view.DataContext = viewModel;
                        view.ShowDialog();
                        Users = Db.Users.GetIEnumerable().ToList();
                    });
                }
                return editUserCommand;
            }
        }
    }
}
