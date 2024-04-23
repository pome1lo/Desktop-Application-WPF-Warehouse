using app.Commands;
using app.Database;
using app.Models;
using DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static DataValidation.Validator;

namespace app.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        public MenuViewModel()
        {
            this.Db = new UnitOfWork();
            validator = new Validator(this);
            Products = Db.Products.GetIEnumerable().ToList();
        }

        private string categoryItem = string.Empty;

        private UnitOfWork Db;
        private List<Product>? products;
        private Product? selectedItemForListProducts;
        private Validator validator;

        private string prodNameFroItemCard;
        private string errorRangeFrom = string.Empty;
        private string errorRangeTo = string.Empty;
        private string searchString = string.Empty;
        private string rangeFrom = string.Empty;
        private string rangeTo = string.Empty;
        private int sliderValue = -1;

        private DelegateCommand? findButtonCommand;
        private DelegateCommand? resetButtonCommand;
        private DelegateCommand<object>? addToBasketCommand;

        #region Main List Product

        public List<Product>? Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        #endregion

        #region Search

        public string SearchString
        {
            get => searchString;
            set
            {
                searchString = value;
                Sort();
                OnPropertyChanged(nameof(SearchString));
            }
        }

        #endregion

        #region From/To

        public string RangeFrom
        {
            get => rangeFrom;
            set
            {
                rangeFrom = value;
                validator.Verify(ValidationBased.OrdinaryDigits, RangeFrom, nameof(ErrorRangeFrom));
                OnPropertyChanged(nameof(RangeFrom));
            }
        }

        public string RangeTo
        {
            get => rangeTo;
            set
            {
                rangeTo = value;
                validator.Verify(ValidationBased.OrdinaryDigits, RangeTo, nameof(ErrorRangeTo));
                OnPropertyChanged(nameof(RangeTo));
            }
        }

        #region Errors


        public string ErrorRangeFrom
        {
            get => errorRangeFrom;
            set
            {
                errorRangeFrom = value;
                OnPropertyChanged(nameof(ErrorRangeFrom));
            }
        }

        public string ErrorRangeTo
        {
            get => errorRangeTo;
            set
            {
                errorRangeTo = value;
                OnPropertyChanged(nameof(ErrorRangeTo));
            }
        }


        #endregion

        #region SliderValue

        public int SliderValue
        {
            get => sliderValue;
            set
            {
                sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
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
        #region Methods

        private void Sort()
        {
            var currentProducts = Db.Products.GetIEnumerable().ToList();
            Products = currentProducts.Where(x => x.ProductName.ToLower().Contains(searchString.ToLower())).ToList();
        }

        private void RangeFilter()
        {
            if (!string.IsNullOrEmpty(RangeTo) && !string.IsNullOrEmpty(RangeFrom))
            {
                if (Decimal.TryParse(RangeTo, out var defTo) && Decimal.TryParse(RangeFrom, out var defFrom))
                {
                    if (Decimal.Parse(RangeTo) > Decimal.Parse(RangeFrom))
                    {
                        Products = Db.Products.GetIEnumerable().Where(x => x.Price < Decimal.Parse(RangeTo) && x.Price > Decimal.Parse(RangeFrom)).ToList();
                    }
                    else
                    {
                        ErrorRangeFrom = "Incorrect";
                        ErrorRangeTo = "Incorrect";
                    }
                }
                else
                {
                    ErrorRangeFrom = "Incorrect";
                    ErrorRangeTo = "Incorrect";
                }
            }
            if (!string.IsNullOrEmpty(RangeTo) && Decimal.TryParse(RangeTo, out var To))
            {
                Products = Db.Products.GetIEnumerable().Where(x => x.Price < Decimal.Parse(RangeTo)).ToList();
            }
            if (!string.IsNullOrEmpty(RangeFrom) && Decimal.TryParse(RangeFrom, out var From))
            {
                Products = Db.Products.GetIEnumerable().Where(x => x.Price > Decimal.Parse(RangeFrom)).ToList();
            }

        }

        private void SliderFilter()
        {
            if (SliderValue != -1)
            {
                Products = Db.Products.GetIEnumerable().Where(x => x.Price < SliderValue).ToList();
            }
        }

        #endregion
        #region Add product from bascket

        public ICommand AddToBasketCommand
        {
            get
            {
                if (addToBasketCommand == null)
                {
                    addToBasketCommand = new DelegateCommand<object>((object obj) =>
                    {
                        Product? product = obj as Product;
                        if (CurrentUser.ProductsFromBasket.Any(x => x.Product == product))
                        {
                            if (CurrentUser.ProductsFromBasket.First(x => x.Product == product).Quantity + 1 < 10)
                            {
                                CurrentUser.ProductsFromBasket.First(x => x.Product == product).Quantity += 1;
                            }
                        }
                        else
                        {
                            if (CurrentUser.ProductsFromBasket.Count() < 5)
                            {
                                CurrentUser.ProductsFromBasket.Add(new ProductFromBasket() { Product = product });
                            }
                        }
                        Db.Save();
                        //ProductsFromBasket = new(CurrentUser.ProductsFromBasket);
                    });
                }
                return addToBasketCommand;
            }
        }

        #endregion

        #region Reset

        public ICommand ResetButtonCommand
        {
            get
            {
                if (resetButtonCommand == null)
                {
                    resetButtonCommand = new DelegateCommand(() =>
                    {
                        RangeFrom = string.Empty;
                        RangeTo = string.Empty;
                        ErrorRangeFrom = string.Empty;
                        ErrorRangeTo = string.Empty;
                        SliderValue = -1;
                        SearchString = string.Empty;
                        Products = Db.Products.GetIEnumerable().ToList();
                    });
                }
                return resetButtonCommand;
            }
        }

        #endregion

        #region Search product

        public ICommand FindButtonCommand
        {
            get
            {
                if (findButtonCommand == null)
                {
                    findButtonCommand = new DelegateCommand(() =>
                    {
                        SliderFilter();
                        RangeFilter();
                    });
                }
                return findButtonCommand;
            }
        }

        #endregion
        #endregion
    }
}
