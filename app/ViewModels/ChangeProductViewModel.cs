using app.Commands;
using app.Database;
using app.Models;
using app.Views.Windows;
using DataValidation;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using static DataValidation.Validator;

namespace app.ViewModels
{
    internal class ChangeProductViewModel : ViewModelBase
    {
        public ChangeProductViewModel(Product product)
        {
            this.product = product;
            this.validator = new Validator(this);
            Db = new UnitOfWork();
            Categories = Db.Categories.GetIEnumerable().ToList();
        }

        private Product product;
        private UnitOfWork Db;
        private Validator validator;
        private List<Category> categories;

        private string errorProductNameMessage = string.Empty;
        private string errorProductPriceMessage = string.Empty;
        private string errorProductCategoryMessage = string.Empty;
        private string errorProductDescriptionMessage = string.Empty;
        private string errorProductPictureMessage = string.Empty;

        private DelegateCommand<ChangeProductView>? editProductCommand;
        private DelegateCommand? openFileCommand;

        #region Property

        public string ImagePath
        {
            get => product.Image;
            set
            {
                product.Image = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        public Category SelectedCategory
        {
            get => product.Category;
            set
            {
                product.Category = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        public List<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public string ProductName
        {
            get => product.ProductName;
            set
            {
                product.ProductName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }
        public decimal ProductPrice
        {
            get => product.Price;
            set
            {
                product.Price = value;
                OnPropertyChanged(nameof(ProductPrice));
            }
        }
        public string ProductDesciption
        {
            get => product.Desciption;
            set
            {
                product.Desciption = value;
                OnPropertyChanged(nameof(ProductDesciption));
            }
        }
        public Category ProductCategory
        {
            get => product.Category;
            set
            {
                product.Category = value;
                OnPropertyChanged(nameof(ProductCategory));
            }
        }

        #endregion

        #region Errors

        public string ErrorProductNameMessage
        {
            get => errorProductNameMessage;
            set
            {
                errorProductNameMessage = value;
                OnPropertyChanged(nameof(ErrorProductNameMessage));
            }
        }

        public string ErrorProductPriceMessage
        {
            get => errorProductPriceMessage;
            set
            {
                errorProductPriceMessage = value;
                OnPropertyChanged(nameof(ErrorProductPriceMessage));
            }
        }

        public string ErrorProductCategoryMessage
        {
            get => errorProductCategoryMessage;
            set
            {
                errorProductCategoryMessage = value;
                OnPropertyChanged(nameof(ErrorProductCategoryMessage));
            }
        }

        public string ErrorProductDescriptionMessage
        {
            get => errorProductDescriptionMessage;
            set
            {
                errorProductDescriptionMessage = value;
                OnPropertyChanged(nameof(ErrorProductDescriptionMessage));
            }
        }

        public string ErrorProductPictureMessage
        {
            get => errorProductPictureMessage;
            set
            {
                errorProductPictureMessage = value;
                OnPropertyChanged(nameof(ErrorProductPictureMessage));
            }
        }

        #endregion

        #region Methods
         
        private bool IsNullCategory()
        {
            if (SelectedCategory == null)
            {
                ErrorProductCategoryMessage = "Incorrect category";
                return false;
            }
            else
            {
                ErrorProductCategoryMessage = "";
                return true;
            }
        }

        private bool NewProductValidate()
        {
            return (
                validator.Verify(ValidationBased.TextTo, ProductName, nameof(ErrorProductNameMessage)) &
                validator.Verify(ValidationBased.OrdinaryDigits, ProductPrice.ToString(), nameof(ErrorProductPriceMessage)) &
                IsNullCategory()
            );
        }

        #endregion

        #region Commands

        public ICommand OpenFileCommand
        {
            get
            {
                if (openFileCommand == null)
                {
                    openFileCommand = new DelegateCommand(() =>
                    {
                        var openFileDialog = new OpenFileDialog
                        {
                            Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                            Title = "Выбрать изображение"
                        };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            ImagePath = openFileDialog.FileName;
                        }
                    });
                }
                return openFileCommand;
            }
        }
         
        public ICommand EditProductCommand
        {
            get
            {
                if (editProductCommand == null)
                {
                    editProductCommand = new DelegateCommand<ChangeProductView>((ChangeProductView view) =>
                    {
                        if (NewProductValidate())
                        {
                            var db = new ApplicationContext();
                            db.Products.Update(product);
                            db.SaveChanges();
                            view.Close();
                        }
                    });
                }
                return editProductCommand;
            }
        } 

        #endregion
    }
}
