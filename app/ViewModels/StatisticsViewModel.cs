using app.Database;
using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.ViewModels
{
    internal class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel()
        {
            this.Db = new UnitOfWork();
            StatisticalData = Db.StatisticalData.GetIEnumerable().ToList();
        }


        private List<Statistics>? statisticalData;
        private UnitOfWork Db;

        #region Property

        public List<Statistics> StatisticalData
        {
            get => statisticalData;
            set
            {
                statisticalData = value;
                OnPropertyChanged(nameof(StatisticalData));
            }
        }

        #endregion
    }
}
