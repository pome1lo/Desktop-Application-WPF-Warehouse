using app.Commands;
using app.Database;
using app.Database.Repositories.MSSQL;
using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace app.ViewModels
{
    internal class BasketViewModel : ViewModelBase
    {

        #region Constructor

        public BasketViewModel()
        {
            this.Db = new UnitOfWork();
            ProductsFromBasket = CurrentUser.ProductsFromBasket;
        }

        #endregion
        
        private UnitOfWork Db;

        private string prodNameFroItemCard;
        private DelegateCommand? findButtonCommand;
        private DelegateCommand? resetButtonCommand;
        private DelegateCommand<ProductFromBasket>? plusItemCardCommand;
        private DelegateCommand<ProductFromBasket>? minusItemCardCommand;
        private DelegateCommand<ProductFromBasket>? closeItemCardCommand;
        private DelegateCommand<object>? addToBasketCommand;
        private DelegateCommand? placeAnOrderCommand;


        #region List Product From Basket

        public List<ProductFromBasket> ProductsFromBasket
        {
            get => CurrentUser.ProductsFromBasket;
            set
            {
                CurrentUser.ProductsFromBasket = value;
                OnPropertyChanged(nameof(ProductsFromBasket));
            }
        }

        #endregion

        #region Selected Item For List Products

        public Product? SelectedItemForListProducts
        {
            get => null;
            set
            {
                ShowProductInfoPage(value);
                OnPropertyChanged(nameof(SelectedItemForListProducts));
            }
        }

        #endregion
        #region Selected item name for basket list products

        public string ProdNameFroItemCard
        {
            get => prodNameFroItemCard;
            set
            {
                prodNameFroItemCard = value;
                OnPropertyChanged(nameof(ProdNameFroItemCard));
            }
        }

        #endregion

        #region Command

        #region Buttons for item card 

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
                            Db.Save();
                            ProductsFromBasket = new(CurrentUser.ProductsFromBasket);
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
                            if (prod.Quantity == 1)
                            {
                                Db.ProductsFromBascket.Delete(prod.Id);
                                Db.Save();
                                ProductsFromBasket = new(CurrentUser.ProductsFromBasket);
                            }
                            else
                            {
                                prod.Quantity--;
                            }
                            Db.Save();
                            ProductsFromBasket = new(CurrentUser.ProductsFromBasket);
                        }
                    });
                }
                return minusItemCardCommand; ;
            }
        }


        public ICommand CloseItemCardCommand
        {
            get
            {
                if (closeItemCardCommand == null)
                {
                    closeItemCardCommand = new DelegateCommand<ProductFromBasket>((ProductFromBasket prod) =>
                    {
                        Db.ProductsFromBascket.Delete(prod.Id);
                        Db.Save();
                        ProductsFromBasket = new(CurrentUser.ProductsFromBasket);
                    });
                }
                return closeItemCardCommand;
            }
        }

        #endregion
        #region Place an order 

        public ICommand PlaceAnOrderCommand
        {
            get
            {
                if (placeAnOrderCommand == null)
                {
                    placeAnOrderCommand = new DelegateCommand(() =>
                    {
                        if (CurrentUser.ProductsFromBasket.Count() > 0)
                        {
                            SendToModalWindow("✅✅✅");
                        }
                        else
                        {
                            SendToModalWindow("First fill out the shopping cart");
                        }
                    });
                }
                return placeAnOrderCommand;
            }
        }

        #endregion
        #endregion
    }
}
