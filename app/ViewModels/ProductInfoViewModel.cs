using app.Commands;
using app.Database;
using app.Database.Repositories.MSSQL;
using app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace app.ViewModels
{
    internal class ProductInfoViewModel : ViewModelBase
    {
        public ProductInfoViewModel(Product? product)
        {
            Db = new UnitOfWork();

            this.Product = new ProductFromBasket()
            {
                Product = product
            };
        }

        private UnitOfWork Db;

        private ProductFromBasket? product;
        private DelegateCommand? addToBasketCommand;

        public ProductFromBasket? Product
        {
            get => product;
            set
            {
                product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        #region Command
          
        public ICommand AddToBasketCommand
        {
            get
            {
                if (addToBasketCommand == null)
                {
                    addToBasketCommand = new DelegateCommand(() =>
                    { 
                        using var db = new ApplicationContext();
                        if (db.ProductsFromBasket.Include(x => x.Product).Any(x => x.Product.Id == product.Id))
                        {
                            var prod = db.ProductsFromBasket.FirstOrDefault(x => x.UserId == CurrentUser.Id);
                            if (prod.Quantity + 1 < 10)
                            {
                                prod.Quantity += 1;
                                db.ProductsFromBasket.Update(prod);
                                db.SaveChanges();
                                SendToModalWindow("Товар успешно добавлен в корзину");
                            }
                            else
                            {
                                SendToModalWindow("Превышено количество единиц товара");
                            }
                        }
                        else
                        {
                            var prod = new ProductFromBasket()
                            {
                                Product = db.Products.First(x => x.Id == product.Product.Id),
                                Quantity = 1,
                                UserId = CurrentUser.Id
                            };
                            db.ProductsFromBasket.Add(prod);
                            db.Users.Update(CurrentUser);
                            db.SaveChanges();
                            SendToModalWindow("Товар успешно добавлен в корзину");
                        }
                        db.Users.Update(CurrentUser);
                        db.SaveChanges(); 
                    });
                }
                return addToBasketCommand;
            }
        }
         
        #endregion
    }
}
