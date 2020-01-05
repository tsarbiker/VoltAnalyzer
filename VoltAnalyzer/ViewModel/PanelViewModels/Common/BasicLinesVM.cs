using LanguageManager;
using LiveCharts;
using LiveCharts.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace VoltAnalyzer.ViewModel.PanelViewModels.Common
{
    public class BasicLinesVM :ViewModelBase
    {
        #region "Properties"

        private LiveCharts.ChartValues<decimal> _series = new LiveCharts.ChartValues<decimal>();
        public LiveCharts.ChartValues<decimal> Series
        {
            get
            {
                return _series;
            }
            set
            {
                _series = value;
                RaisePropertyChanged("Series");
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private List<string> _labelsX = new List<string>();
        public List<string> LabelsX
        {
            get
            {
                return _labelsX;
            }
            set
            {
                _labelsX = value;
                RaisePropertyChanged("Labels");
            }
        }

        private decimal _maxY = 15;
        public decimal MaxY
        {
            get
            {
                return _maxY;
            }
            set
            {
                _maxY = value;
                RaisePropertyChanged("MaxY");
            }
        }

        private decimal _minY = 0;
        public decimal MinY
        {
            get
            {
                return _minY;
            }
            set
            {
                _minY = value;
                RaisePropertyChanged("MinY");
            }
        }

        private decimal _maxX = 15;
        public decimal MaxX
        {
            get
            {
                return _maxX;
            }
            set
            {
                _maxX = value;
                RaisePropertyChanged("MaxX");
            }
        }

        private decimal _minX = 0;
        public decimal MinX
        {
            get
            {
                return _minX;
            }
            set
            {
                _minX = value;
                RaisePropertyChanged("MinX");
            }
        }

        #endregion

        #region "functions"

        public void GraphConsumption(int a_min, string codeText,  List<string> a_dateConsumption, List<decimal> a_consumptionList)
        {
            if (a_consumptionList.Count() != 0)
            {
                LabelsX = new List<string>(a_dateConsumption.AsEnumerable());
                MaxY = a_consumptionList.Max() + 2;
                MinY = a_min;

                MaxX = a_dateConsumption.Count() - 1;
                MinX = a_dateConsumption.Count() - 16;

                Series = new LiveCharts.ChartValues<decimal>(a_consumptionList.AsEnumerable());
                Title = ManageLanguage.GetLanguageString(codeText, ManageLanguage.LanguageSelected);

                var mapper1 = new CartesianMapper<double>()
              .X((value, index) => index) //use the index as X
              .Y((value, index) => value); //use the value as Y
                LiveCharts.Charting.For<double>(mapper1, SeriesOrientation.Horizontal);
            }
        }

        #endregion
    }
}
