using app.Commands;
using app.Database;
using app.Models;
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
                Product = product,
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

        #region Add to basket 

        public ICommand AddToBasketCommand
        {
            get
            {
                if (addToBasketCommand == null)
                {
                    addToBasketCommand = new DelegateCommand(() =>
                    {
                        if (CurrentUser.ProductsFromBasket.Any(x => x.Product == Product?.Product))
                        {
                            if (CurrentUser.ProductsFromBasket.First(x => x.Product == product?.Product).Quantity + 1 < 10)
                            {
                                SendToModalWindow("The product has been successfully added to the cart");
                                CurrentUser.ProductsFromBasket.First(x => x.Product == product?.Product).Quantity += 1;
                                Db.Save();
                            }
                            else
                            {
                                SendToModalWindow("Exceeded the number of units of the product");
                            }
                        }
                        else
                        {
                            if (CurrentUser.ProductsFromBasket.Count() < 5)
                            {
                                SendToModalWindow("The product has been successfully added to the cart");
                                CurrentUser.ProductsFromBasket.Add(Product);
                            }
                        }
                        Db.Save();
                    });
                }
                return addToBasketCommand;
            }
        }

        #endregion
        #endregion
    }
}
