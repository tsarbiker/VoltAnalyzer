using LanguageManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ViewModel.BusinessViewModels.Home
{
    public class CarInfoVM : ViewModelBase
    {
        #region localized text

        protected override void ManageLanguage_OnLanguageChanged(object sender, LanguageChangedEventArgs changeArgs)
        {
            base.ManageLanguage_OnLanguageChanged(sender, changeArgs);
            RaisePropertyChanged("TotalT");
            RaisePropertyChanged("CostEstimateT");
        }

        public String TotalT
        {
            get { return ManageLanguage.GetLanguageString("CarInfoVM$TotalT", ManageLanguage.LanguageSelected); }
        }

        public String CostEstimateT
        {
            get { return ManageLanguage.GetLanguageString("CarInfoVM$CostEstimateT", ManageLanguage.LanguageSelected); }
        }

        #endregion

        #region property

        #region "Average KM"
        private decimal _averageEVKMDisplay;
        public decimal AverageEVKMDisplay
        {
            get
            {
                return _averageEVKMDisplay;
            }

            set
            {
                _averageEVKMDisplay = value;
                RaisePropertyChanged("AverageEVKMDisplay");
            }
        }

        private decimal _averageEVKM;
        public decimal AverageEVKM
        {
            get
            {
                return _averageEVKM;
            }

            set
            {
                _averageEVKM = value;
                AverageEVKMDisplay = ValueToCircularProgress(_averageEVKM, 100);
                AverageEVKMT = value.ToString();
            }
        }

        private string _averageEVKMT;
        public string AverageEVKMT
        {
            get
            {
                return _averageEVKMT;
            }

            set
            {
                _averageEVKMT = value + " km";
                RaisePropertyChanged("AverageEVKMT");
            }
        }

        public decimal HighEVKMDisplay
        {
            get
            {
                return ValueToCircularProgress(100, 100);
            }
        }

        public decimal MiddleEVKMDisplay
        {
            get
            {
                return ValueToCircularProgress(61, 100); //61km
            }

        }

        public string OfficialEVKM
        {
            get
            {
                return "   Range: 61km  \n          38mi";
            }

        }
        #endregion

        #region "EV Consumption"
        private decimal _averageEVConsumptionDisplay;
        public decimal AverageEVConsumptionDisplay
        {
            get
            {
                return _averageEVConsumptionDisplay;
            }

            set
            {
                _averageEVConsumptionDisplay = value;
                RaisePropertyChanged("AverageEVConsumptionDisplay");
            }
        }

        private decimal _averageEVConsumption;
        public decimal AverageEVConsumption
        {
            get
            {
                return _averageEVConsumption;
            }

            set
            {
                _averageEVConsumption = value;
                AverageEVConsumptionDisplay = ValueToCircularProgress(_averageEVConsumption, 30);
                AverageEVConsumptionT = value.ToString();
            }
        }

        private string _averageEVConsumptionT;
        public string AverageEVConsumptionT
        {
            get
            {
                return _averageEVConsumptionT;
            }

            set
            {
                _averageEVConsumptionT = value + " kwh/100km";
                RaisePropertyChanged("AverageEVConsumptionT");
            }
        }

        private decimal _bestEVConsumption;
        public decimal BestEVConsumption
        {
            get
            {
                return _bestEVConsumption;
            }

            set
            {
                _bestEVConsumption = value;
                BestEVConsumptionT = value.ToString();
            }
        }

        private string _bestEVConsumptionT;
        public string BestEVConsumptionT
        {
            get
            {
                return _bestEVConsumptionT;
            }

            set
            {
                _bestEVConsumptionT = ManageLanguage.GetLanguageString("CarInfoVM$Best", ManageLanguage.LanguageSelected) +  value + " kwh/100km";
                RaisePropertyChanged("BestEVConsumptionT");
            }
        }

        private decimal _worstEVConsumption;
        public decimal WorstEVConsumption
        {
            get
            {
                return _worstEVConsumption;
            }

            set
            {
                _worstEVConsumption = value;
                WorstEVConsumptionT = value.ToString();
            }
        }

        private string _worstEVConsumptionT;
        public string WorstEVConsumptionT
        {
            get
            {
                return _worstEVConsumptionT;
            }

            set
            {
                _worstEVConsumptionT = ManageLanguage.GetLanguageString("CarInfoVM$Worst", ManageLanguage.LanguageSelected) + value + " kwh/100km";
                RaisePropertyChanged("WorstEVConsumptionT");
            }
        }

        public decimal LowEVConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(14, 30);
            }
        }

        public decimal HighEVConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(30, 30);
            }
        }

        public decimal MiddleEVConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(16.9M, 30);
            }

        }

        public string OfficialEVConsumption
        {
            get
            {
                return "    Online: 14kwh\n   Official: 16.9kwh";
            }

        }
        #endregion

        #region "fuel consumption"
        private decimal _averageFuelConsumption;
        public decimal AverageFuelConsumption
        {
            get
            {
                return _averageFuelConsumption;
            }

            set
            {
                _averageFuelConsumption =value;
                AverageFuelConsumptionDisplay = ValueToCircularProgress(_averageFuelConsumption, 10);
                AverageFuelConsumptionT = value.ToString();
            }
        }

        private string _averageFuelConsumptionT;
        public string AverageFuelConsumptionT
        {
            get
            {
                return _averageFuelConsumptionT;
            }

            set
            {
                _averageFuelConsumptionT = value + " l/100km";
                RaisePropertyChanged("AverageFuelConsumptionT");
            }
        }

        private decimal _averageFuelConsumptionDisplay;
        public decimal AverageFuelConsumptionDisplay
        {
            get
            {
                return _averageFuelConsumptionDisplay;
            }

            set
            {
                _averageFuelConsumptionDisplay = value;
                RaisePropertyChanged("AverageFuelConsumptionDisplay");
            }
        }

        private decimal _bestFuelConsumption;
        public decimal BestFuelConsumption
        {
            get
            {
                return _bestFuelConsumption;
            }

            set
            {
                _bestFuelConsumption = value;
                BestFuelConsumptionT = value.ToString() ;
            }
        }

        private string _bestFuelConsumptionT;
        public string BestFuelConsumptionT
        {
            get
            {
                return _bestFuelConsumptionT;
            }

            set
            {
                _bestFuelConsumptionT = ManageLanguage.GetLanguageString("CarInfoVM$Best", ManageLanguage.LanguageSelected) +  value + " l/100km";
                RaisePropertyChanged("BestFuelConsumptionT");
            }
        }

        private decimal _worstFuelConsumption;
        public decimal WorstFuelConsumption
        {
            get
            {
                return _worstFuelConsumption;
            }

            set
            {
                _worstFuelConsumption = value;
                WorstFuelConsumptionT = value.ToString();
            }
        }

        private string _worstFuelConsumptionT;
        public string WorstFuelConsumptionT
        {
            get
            {
                return _worstFuelConsumptionT;
            }

            set
            {
                _worstFuelConsumptionT = ManageLanguage.GetLanguageString("CarInfoVM$Worst", ManageLanguage.LanguageSelected) +  value + " l/100km";
                RaisePropertyChanged("WorstFuelConsumptionT");
            }
        }

        public decimal LowFuelConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(5.9M, 10);
            }
        }

        public decimal HighFuelConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(10, 10);
            }
        }

        public decimal MiddleFuelConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(6.7M, 10);
            }

        }

        public string OfficialFuelConsumption
        {
            get
            {
                return "   City: 5.9l \nHighway: 6.7l";
            }

        }
        #endregion

        #region "Combined Consumption"
        private decimal _averageCombinedConsumptionDisplay;
        public decimal AverageCombinedConsumptionDisplay
        {
            get
            {
                return _averageCombinedConsumptionDisplay;
            }

            set
            {
                _averageCombinedConsumptionDisplay = value;
                RaisePropertyChanged("AverageCombinedConsumptionDisplay");
            }
        }

        private decimal _averageCombinedConsumption;
        public decimal AverageCombinedConsumption
        {
            get
            {
                return _averageCombinedConsumption;
            }

            set
            {
                _averageCombinedConsumption = value;
                AverageCombinedConsumptionDisplay = ValueToCircularProgress(_averageCombinedConsumption, 5);
                AverageCombinedConsumptionT = value.ToString();
            }
        }

        private string _averageCombinedConsumptionT;
        public string AverageCombinedConsumptionT
        {
            get
            {
                return _averageCombinedConsumptionT;
            }

            set
            {
                _averageCombinedConsumptionT = value + " l/100km";
                RaisePropertyChanged("AverageCombinedConsumptionT");
            }
        }

        public decimal LowCombinedConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(1.2M, 5);
            }
        }

        public decimal HighCombinedConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(30, 5);
            }
        }

        public decimal MiddleCombinedConsumptionDisplay
        {
            get
            {
                return ValueToCircularProgress(2.4M, 5);
            }

        }

        public string OfficialCombinedConsumption
        {
            get
            {
                return "   US: 2.4l\nOfficial: 1.2l";
            }

        }
        #endregion

        #region "Total fuel/kw/km"

        private decimal _totalFuelUsed;
        public decimal TotalFuelUsed
        {
            get
            {
                return _totalFuelUsed;
            }

            set
            {
                _totalFuelUsed = value;
                TotalFuelUsedDisplay = value.ToString();
            }
        }

        private decimal _totalCharging;
        public decimal TotalCharging
        {
            get
            {
                return _totalCharging;
            }

            set
            {
                _totalCharging = value;
                TotalChargingDisplay = value.ToString();
            }
        }

        private decimal _totaKM;
        public decimal TotalKM
        {
            get
            {
                return _totaKM;
            }

            set
            {
                _totaKM = value;
                TotalKMDisplay = value.ToString();
            }
        }

        private string _totalFuelUsedDisplay;
        public string TotalFuelUsedDisplay
        {
            get
            {
                return _totalFuelUsedDisplay;
            }

            set
            {
                _totalFuelUsedDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$Liters", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("TotalFuelUsedDisplay");
            }
        }

        private string _totalChargingDisplay;
        public string TotalChargingDisplay
        {
            get
            {
                return _totalChargingDisplay;
            }

            set
            {
                _totalChargingDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$KW", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("TotalChargingDisplay");
            }
        }

        private string _totaKMDisplay;
        public string TotalKMDisplay
        {
            get
            {
                return _totaKMDisplay;
            }

            set
            {
                _totaKMDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$KM", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("TotalKMDisplay");
            }
        }

        #endregion

        #region "Total fuel/kw/km"

        private decimal _costFuel;
        public decimal CostFuel
        {
            get
            {
                return _costFuel;
            }

            set
            {
                _costFuel = value;
                CostFuelDisplay = value.ToString();
            }
        }

        private decimal _costCharging;
        public decimal CostCharging
        {
            get
            {
                return _costCharging;
            }

            set
            {
                _costCharging = value;
                CostChargingDisplay = value.ToString();
            }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get
            {
                return _totalCost;
            }

            set
            {
                _totalCost = value;
                TotalCostDisplay = value.ToString();
            }
        }

        private string _totalCostDisplay;
        public string TotalCostDisplay
        {
            get
            {
                return _totalCostDisplay;
            }

            set
            {
                _totalCostDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$Cost", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("TotalCostDisplay");
            }
        }

        private string _costChargingDisplay;
        public string CostChargingDisplay
        {
            get
            {
                return _costChargingDisplay;
            }

            set
            {
                _costChargingDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$Cost", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("CostChargingDisplay");
            }
        }

        private string _costFuelDisplay;
        public string CostFuelDisplay
        {
            get
            {
                return _costFuelDisplay;
            }

            set
            {
                _costFuelDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$Cost", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("CostFuelDisplay");
            }
        }

        private decimal _costPer100KM;
        public decimal CostPer100KM
        {
            get
            {
                return _costPer100KM;
            }

            set
            {
                _costPer100KM = value;
                CostPer100KMDisplay = value.ToString();
            }
        }

        private string _costPer100KMDisplay;
        public string CostPer100KMDisplay
        {
            get
            {
                return _costPer100KMDisplay;
            }

            set
            {
                _costPer100KMDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$Cost100KM", ManageLanguage.LanguageSelected);
                RaisePropertyChanged("CostPer100KMDisplay");
            }
        }

        #endregion

        #region "Average Speed"
        private string _averageSpeedDisplay;
        public string AverageSpeedDisplay
        {
            get
            {
                return _averageSpeedDisplay;
            }

            set
            {
                _averageSpeedDisplay = value + " Km/h";
                RaisePropertyChanged("AverageSpeedDisplay");
            }
        }

        private decimal _averageSpeed;
        public decimal AverageSpeed
        {
            get
            {
                return _averageSpeed;
            }

            set
            {
                _averageSpeed = value;
                AverageSpeedDisplay = _averageSpeed.ToString();
            }
        }
        #endregion

        #region "Fuel Consumption Graph"
        private List<string> _dateFuelConsumption = new List<string>();
        public List<string> DateFuelConsumption
        {
            get
            {
                return _dateFuelConsumption;
            }

            set
            {
                _dateFuelConsumption = value;
                RaisePropertyChanged("DateFuelConsumption");
            }
        }

        private List<string> _dateEVConsumption = new List<string>();
        public List<string> DateEVConsumption
        {
            get
            {
                return _dateEVConsumption;
            }

            set
            {
                _dateEVConsumption = value;
                RaisePropertyChanged("DateEVConsumption");
            }
        }

        private List<decimal> _consumptionList = new List<decimal>();
        public List<decimal> FuelConsumptionList
        {
            get
            {
                return _consumptionList;
            }

            set
            {
                _consumptionList = value;
                RaisePropertyChanged("ConsumptionList");
            }
        }

        private List<decimal> _evConsumptionList = new List<decimal>();
        public List<decimal> EVConsumptionList
        {
            get
            {
                return _evConsumptionList;
            }

            set
            {
                _evConsumptionList = value;
                RaisePropertyChanged("EvConsumptionList");
            }
        }
        #endregion

        #region "Regen"
        private string _totalRegenDisplay;
        public string TotalRegenDisplay
        {
            get
            {
                return _totalRegenDisplay;
            }

            set
            {
                _totalRegenDisplay = value + " " + ManageLanguage.GetLanguageString("CarInfoVM$KW", ManageLanguage.LanguageSelected); ;
                RaisePropertyChanged("TotalRegenDisplay");
            }
        }

        private decimal _totalRegen;
        public decimal TotalRegen
        {
            get
            {
                return _totalRegen;
            }

            set
            {
                _totalRegen = value;
                TotalRegenDisplay = TotalRegen.ToString();
            }
        }
        #endregion

        #endregion

        #region "Functions"

        private decimal ValueToCircularProgress(decimal a_input, decimal a_maxvalue)
        {
            return (100/a_maxvalue)*a_input;
        }

#endregion
    }
}
