using app.Commands;
using app.Database;
using app.Models;
using app.Views.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace app.ViewModels
{
    internal class BasketViewModel : ViewModelBase
    {
        public BasketViewModel()
        {
            this.Db = new UnitOfWork();
            ProductsFromBasket = Db.ProductsFromBascket.GetIEnumerable().Where(x => x.UserId == CurrentUser.Id).ToList();
        }

        private UnitOfWork Db;
        private List<ProductFromBasket> productsFromBasket;

        private string prodNameFroItemCard;
        private DelegateCommand<ProductFromBasket>? plusItemCardCommand;
        private DelegateCommand<ProductFromBasket>? minusItemCardCommand;
        private DelegateCommand? placeAnOrderCommand;

        #region Property

        public List<ProductFromBasket> ProductsFromBasket
        {
            get => productsFromBasket;
            set
            {
                productsFromBasket = value;
                OnPropertyChanged(nameof(ProductsFromBasket));
            }
        }

        public Product? SelectedItemForListProducts
        {
            get => null;
            set
            {
                ShowProductInfoPage(value);
                OnPropertyChanged(nameof(SelectedItemForListProducts));
            }
        }

        public string ProdNameFroItemCard
        {
            get => prodNameFroItemCard;
            set
            {
                prodNameFroItemCard = value;
                OnPropertyChanged(nameof(ProdNameFroItemCard));
            }
        }

        public decimal Total
        {
            get => ProductsFromBasket.Sum(x => x.Product.Price * x.Quantity);
            set
            {
                OnPropertyChanged(nameof(Total));
            }
        }

        #endregion

        #region Command

        public ICommand PlusItemCardCommand
        {
            get
            {
                if (plusItemCardCommand == null)
                {
                    plusItemCardCommand = new DelegateCommand<ProductFromBasket>((ProductFromBasket prod) =>
                    {
                        if (prod.Quantity + 1 < 10)
                        {
                            prod.Quantity++;

                            var db = new ApplicationContext();
                            db.ProductsFromBasket.Update(prod);
                            db.SaveChanges();
                            ProductsFromBasket = new(db.ProductsFromBasket.Where(x => x.Id == CurrentUser.Id));
                            ShowPage(new BasketView());
                        }
                    });
                }
                return plusItemCardCommand;
            }
        }

        public ICommand MinusItemCardCommand
        {
            get
            {
                if (minusItemCardCommand == null)
                {
                    minusItemCardCommand = new DelegateCommand<ProductFromBasket>((ProductFromBasket prod) =>
                    {
                        if (prod != null)
                        {
                            var db = new ApplicationContext();
                            if (prod.Quantity == 1)
                            {
                                db.ProductsFromBasket.Remove(prod);
                                db.SaveChanges();
                                ProductsFromBasket = new(db.ProductsFromBasket.Where(x => x.Id == CurrentUser.Id));
                            }
                            else
                            {
                                db.ProductsFromBasket.Update(prod);
                                prod.Quantity--;
                                db.SaveChanges();
                            }
                            db.SaveChanges();
                            ProductsFromBasket = new(db.ProductsFromBasket.Where(x => x.Id == CurrentUser.Id));
                            ShowPage(new BasketView());
                        }
                    });
                }
                return minusItemCardCommand; ;
            }
        }

        public ICommand PlaceAnOrderCommand
        {
            get
            {
                if (placeAnOrderCommand == null)
                {
                    placeAnOrderCommand = new DelegateCommand(() =>
                    {
                        using var db = new ApplicationContext();
                        var listProducts = db.ProductsFromBasket.Include(x => x.Product).Where(x => x.UserId == CurrentUser.Id);
                        if (listProducts.Count() > 0)
                        {
                            var listOrederProducts = new List<ProductFromOrder>();
                            foreach (var product in listProducts)
                            {
                                listOrederProducts.Add(new ProductFromOrder()
                                {
                                    Price = product.Product.Price,
                                    Image = product.Product.Image,
                                    ProductName = product.Product.ProductName
                                });
                            }
                            var order = new Order()
                            {
                                Total = Total,
                                User = db.Users.FirstOrDefault(x => x.Id == CurrentUser.Id),
                                Date = DateTime.Now,
                                Products = listOrederProducts
                            };



                            var existingStatistics = db.StatisticalData.FirstOrDefault(s => s.Email == order.User.Email);

                            if (existingStatistics != null)
                            {
                                existingStatistics.NumberOfItemsSold += order.Products.Count();
                            }
                            else
                            {
                                var statistics = new Statistics()
                                {
                                    Total = order.Total,
                                    Username = order.User.Username,
                                    Email = order.User.Email,
                                    NumberOfItemsSold = order.Products.Count()
                                };

                                db.StatisticalData.Add(statistics);
                            } 
                            db.Orders.Add(order);
                            db.ProductsFromBasket.RemoveRange(listProducts);
                            db.SaveChanges();
                            ProductsFromBasket = db.ProductsFromBasket.Where(x => x.UserId == CurrentUser.Id).ToList();
                            SendToModalWindow("Заказ успешно оформлен");
                            ShowPage(new MenuView());
                        }
                        else
                        {
                            SendToModalWindow("Сначала добавьте товары в корзину");
                        }
                    });
                }
                return placeAnOrderCommand;
            }
        }

        #endregion
    }
}
