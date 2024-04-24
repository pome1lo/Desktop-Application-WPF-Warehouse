using app.Commands;
using app.Database;
using app.Models;
using DataValidation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
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
    class NewProductViewModel : ViewModelBase
    {
        public NewProductViewModel()
        {
            this.validator = new Validator(this);
            Db = new UnitOfWork();
            Categories = Db.Categories.GetIEnumerable().ToList();
        }
        
        private Product product = new();
        private UnitOfWork Db;
        private Validator validator;

        private List<Category> categories; 
         
        private string errorProductNameMessage = string.Empty;
        private string errorProductPriceMessage = string.Empty;
        private string errorProductCategoryMessage = string.Empty;
        private string errorProductDescriptionMessage = string.Empty;
        private string errorProductPictureMessage = string.Empty;

        private DelegateCommand? addNewProductCommand;
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
         
        private bool IsNullCategory()
        {
            if(SelectedCategory == null)
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
        public ICommand AddNewProductCommand
        {
            get
            {
                if (addNewProductCommand == null)
                {
                    addNewProductCommand = new DelegateCommand(() =>
                    {
                        if (NewProductValidate())
                        {
                            //var db = new ApplicationContext();
                            product.Category = Db.Categories.GetIEnumerable().FirstOrDefault(x=> x.Name == SelectedCategory.Name);
                            Db.Products.Create(product);
                            
                            //Db.Products.Create(product);
                            //Db.Save();
                            Db.Save();
                            SendToModalWindow("The product was successfully added to the database.");
                            
                            Db.Save();
                        }
                    });
                }
                return addNewProductCommand;
            }
        }
         
    }
}
