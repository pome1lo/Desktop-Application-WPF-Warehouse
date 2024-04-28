using app.Commands;
using app.Database;
using app.Models;
using app.Views.Pages;
using app.Views.Windows;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        private DelegateCommand? addNewProduct;

        #region Property

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

        #endregion

        #region Commands

        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                {
                    deleteUserCommand = new DelegateCommand<User>((User user) =>
                    {
                        if (user.Id == CurrentUser.Id)
                        {
                            SendToModalWindow("Вы не можете удалить сами себя");
                        }
                        else
                        {
                            using (var context = new ApplicationContext())
                            {
                                var ordersToDelete = context.Orders
                                    .Include(x => x.User)
                                    .Include(x => x.Products)
                                    .Where(o => o.User.Id == user.Id).ToList();

                                context.Orders.RemoveRange(ordersToDelete);

                                var userToDelete = context.Users.FirstOrDefault(u => u.Id == user.Id);

                                if (userToDelete != null)
                                {
                                    context.Users.Remove(userToDelete);
                                }
                                context.SaveChanges();
                                Users = context.Users.ToList();
                                SendToModalWindow("Пользователь успешно удален");
                            }


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
        public ICommand AddNewProduct
        {
            get
            {
                if (addNewProduct == null)
                {
                    addNewProduct = new DelegateCommand(() =>
                    {
                        ShowPage(new NewProduct());
                    });
                }
                return addNewProduct;
            }
        }
        #endregion
    }
}
