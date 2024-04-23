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
    class NewProductViewModel : ViewModelBase
    {
        public NewProductViewModel()
        {
            this.validator = new Validator(this);
            Db = new UnitOfWork();
        }
        
        private Product product = new();
        private UnitOfWork Db;
        private Validator validator;


        private string errorProductNameMessage = string.Empty;
        private string errorProductPriceMessage = string.Empty;
        private string errorProductCategoryMessage = string.Empty;
        private string errorProductDescriptionMessage = string.Empty;

        private DelegateCommand? addNewProductCommand;

        #region Property

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
        public string ProductImg
        {
            get => product.Image;
            set
            {
                product.Image = @"\Static\Img\" + value;
                OnPropertyChanged(nameof(ProductImg));
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


        #endregion

        private bool NewProductValidate()
        {
            return (
                validator.Verify(ValidationBased.TextTo, ProductName, nameof(ErrorProductNameMessage)) &
                validator.Verify(ValidationBased.OrdinaryDigits, ProductPrice.ToString(), nameof(ErrorProductPriceMessage)) 
            );
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
                            var db = new ApplicationContext();
                            db.Products.Add(product);
                            //Db.Products.Create(product);
                            //Db.Save();
                            db.SaveChanges();
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
