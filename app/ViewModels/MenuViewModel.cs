using app.Commands;
using app.Database;
using app.Models;
using app.Views.Windows;
using DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            selectedCategories = new List<string>();
            Products = Db.Products.GetIEnumerable().ToList();
            VisibilityEditProductButton = CurrentUser.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private List<string> selectedCategories;
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

        private Visibility visibilityEditProductButton = Visibility.Collapsed;

        private DelegateCommand? findButtonCommand;
        private DelegateCommand? resetButtonCommand;
        private DelegateCommand<Product>? addToBasketCommand;
        private DelegateCommand<object>? checkBoxCommand;
        private DelegateCommand<Product>? editProductCommand;

        #region Property

        public List<Product>? Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        #region ToggleButtons

        public bool isCheckedToggleButton1;
        public bool IsCheckedToggleButton1
        {
            get => isCheckedToggleButton1;
            set
            {
                isCheckedToggleButton1 = value;
                OnPropertyChanged(nameof(IsCheckedToggleButton1));
            }
        }
        public bool isCheckedToggleButton2;
        public bool IsCheckedToggleButton2
        {
            get => isCheckedToggleButton2;
            set
            {
                isCheckedToggleButton2 = value;
                OnPropertyChanged(nameof(IsCheckedToggleButton2));
            }
        }
        public bool isCheckedToggleButton3;
        public bool IsCheckedToggleButton3
        {
            get => isCheckedToggleButton3;
            set
            {
                isCheckedToggleButton3 = value;
                OnPropertyChanged(nameof(IsCheckedToggleButton3));
            }
        }
        public bool isCheckedToggleButton4;
        public bool IsCheckedToggleButton4
        {
            get => isCheckedToggleButton4;
            set
            {
                isCheckedToggleButton4 = value;
                OnPropertyChanged(nameof(IsCheckedToggleButton4));
            }
        }

        #endregion

        public Visibility VisibilityEditProductButton
        {
            get => visibilityEditProductButton;
            set
            {
                visibilityEditProductButton = value;
                OnPropertyChanged(nameof(VisibilityEditProductButton));
            }
        }

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


        public int SliderValue
        {
            get => sliderValue;
            set
            {
                sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
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

        #endregion

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

        private void FilterProducts()
        {
            // Фильтрация списка продуктов в соответствии с выбранными категориями
            if (selectedCategories.Any())
            {
                Products = new List<Product>(
                    Db.Products.GetIEnumerable()
                    .Where(p => selectedCategories.Contains(p.Category.Name))
                    .ToList());
            }
            else
            {
                Products = new List<Product>(Db.Products.GetIEnumerable().ToList());
            }
        }

        #endregion

        #region Commands

        public ICommand AddToBasketCommand
        {
            get
            {
                if (addToBasketCommand == null)
                {
                    addToBasketCommand = new DelegateCommand<Product>((Product product) =>
                    {
                        using var db = new ApplicationContext();

                        if (db.ProductsFromBasket.Any(x => x.UserId == CurrentUser.Id))
                        {
                            if (db.ProductsFromBasket.Where(x => x.UserId == CurrentUser.Id).First().Quantity + 1 < 10)
                            {
                                var prod = db.ProductsFromBasket.Where(x => x.UserId == CurrentUser.Id).First();
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
                                Product = db.Products.First(x => x.Id == product.Id),
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

        public ICommand ResetButtonCommand
        {
            get
            {
                if (resetButtonCommand == null)
                {
                    resetButtonCommand = new DelegateCommand(() =>
                    {
                        selectedCategories.Clear();
                        FilterProducts();
                        RangeFrom = string.Empty;
                        RangeTo = string.Empty;
                        ErrorRangeFrom = string.Empty;
                        ErrorRangeTo = string.Empty;
                        SliderValue = -1;
                        SearchString = string.Empty;
                        IsCheckedToggleButton1 = false;
                        IsCheckedToggleButton2 = false;
                        IsCheckedToggleButton3 = false;
                        IsCheckedToggleButton4 = false;
                        Products = Db.Products.GetIEnumerable().ToList();
                    });
                }
                return resetButtonCommand;
            }
        }

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

        public ICommand CheckBoxCommand
        {
            get
            {
                if (checkBoxCommand == null)
                {
                    checkBoxCommand = new DelegateCommand<object>((object obj) =>
                    {
                        if (obj is string category)
                        {
                            if (selectedCategories.Contains(category))
                            {
                                selectedCategories.Remove(category);
                            }
                            else
                            {
                                selectedCategories.Add(category);
                            }
                            FilterProducts();
                        }
                    });
                }
                return checkBoxCommand;
            }
        }

        public ICommand EditProductCommand
        {
            get
            {
                if (editProductCommand == null)
                {
                    editProductCommand = new DelegateCommand<Product>((Product product) =>
                    {
                        var view = new ChangeProductView();
                        var viewModel = new ChangeProductViewModel(product);
                        view.DataContext = viewModel;
                        view.ShowDialog();
                        Products = Db.Products.GetIEnumerable().ToList();
                    });
                }
                return editProductCommand;
            }
        }

        #endregion
    }
}
