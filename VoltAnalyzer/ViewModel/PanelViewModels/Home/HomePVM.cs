using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using LanguageManager;
using ViewModels.PanelViewModels.Common;
using VoltAnalyzer.Utils.Controls;
using VoltAnalyzer.ValueObjects.Types.Common;
using VoltAnalyzer.ViewModel.BusinessViewModels.Graph;
using VoltAnalyzer.ViewModel.BusinessViewModels.Home;
using VoltAnalyzer.ViewModel.PanelViewModels.Common;

namespace VoltAnalyzer.ViewModel.PanelViewModels.Home
{
    public class HomePVM : ViewModelBase
    {
        #region constructor
        public HomePVM()
        {
            LoadTorqueDataCommand = new RelayCommand(LoadTorqueData);
            DefaultDirectoryCommand = new RelayCommand(DefaultDirectory);

            CloseCommand = new RelayCommand<IWindow>(Close);
            ExpandCommand = new RelayCommand<IWindow>(Expand);
            MinimizeCommand = new RelayCommand<IWindow>(Minimize);
            MoveCommand = new RelayCommand<IWindow>(Move);

            foreach (string item in LanguageManager.ManageLanguage.GetAllLanguages())
            {
                LanguageList.Add(new LanguageSelectorVM(item));
            }

            SelectedLanguage = LanguageList[1];

            RaisePropertyChanged("LanguageList");

            if (ConfigurationManager.AppSettings.AllKeys.Contains("SelectedFolder"))
            {
                SelectedFolder = ConfigurationManager.AppSettings.GetValues("SelectedFolder")[0].ToString();
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("FuelPrice"))
            {
                FuelPrice = decimal.Parse(ConfigurationManager.AppSettings.GetValues("FuelPrice")[0].ToString(), CultureInfo.InvariantCulture);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ElectricityPrice"))
            {
                ElectricityPrice = decimal.Parse(ConfigurationManager.AppSettings.GetValues("ElectricityPrice")[0].ToString(), CultureInfo.InvariantCulture);
            }

            VoltAnalyzerMessage.Subscribe<string>(this, MessageConstants.SetBusyMessage, (string Message) =>
            {
                WaitMessage = Message;
            });

            VoltAnalyzerMessage.Subscribe<string>(this, MessageConstants.ChangeGraph, (string a_type) =>
            {
                switch (a_type)
                {
                    case "FuelC":
                        BasicLinesGVM.GraphConsumption(0, "Home$SerieFuelC", CarInfo.DateFuelConsumption, CarInfo.FuelConsumptionList);
                        break;
                    case "EVC":
                        BasicLinesGVM.GraphConsumption(8,"Home$SerieEVC", CarInfo.DateEVConsumption, CarInfo.EVConsumptionList);
                        break;
                    default:
                        break;
                }
            });

            GraphListGVM.Add(new GraphListVM() { ID = "FuelC", Name = "Fuel consumption" });
            GraphListGVM.Add(new GraphListVM() { ID = "EVC", Name = "EV consumption" });
        }
        #endregion

        #region property

        #region "wait overlay"

        private Boolean _isBusy = false;
        public Boolean IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        private string _waitMessage = "";
        public string WaitMessage
        {
            get
            {
                return _waitMessage;
            }
            set
            {
                _waitMessage = value;
                RaisePropertyChanged("WaitMessage");
            }
        }

        #endregion

        #region "Display message"

        private MessageDisplayVM _messageDisplayVM = new MessageDisplayVM();
        public MessageDisplayVM MessageDisplay
        {
            get
            {
                return _messageDisplayVM;
            }
            set
            {
                _messageDisplayVM = value;
                RaisePropertyChanged("MessageDisplay");
            }
        }

        #endregion

        #region "Basic lines graph"

        private BasicLinesVM _basicLinesGVM = new BasicLinesVM();
        public BasicLinesVM BasicLinesGVM
        {
            get
            {
                return _basicLinesGVM;
            }
            set
            {
                _basicLinesGVM = value;
                RaisePropertyChanged("BasicLinesGVM");
            }
        }

        private List<GraphListVM> _graphListGVM = new List<GraphListVM>();
        public List<GraphListVM> GraphListGVM
        {
            get
            {
                return _graphListGVM;
            }
            set
            {
                _graphListGVM = value;
                RaisePropertyChanged("GraphListGVM");
            }
        }

        #endregion

        private CarInfoVM _carInfoVM = new CarInfoVM();
        public CarInfoVM CarInfo
        {
            get
            {
                return _carInfoVM;
            }
            set
            {
                _carInfoVM = value;
                RaisePropertyChanged("CarInfo");
            }
        }

        #region "Language"
        private LanguageSelectorVM _selectedLanguage;
        public LanguageSelectorVM SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                LanguageManager.ManageLanguage.ChangeLanguage(_selectedLanguage.LanguageName);
            }
        }

        private List<LanguageSelectorVM> _languageList = new List<LanguageSelectorVM>();
        public List<LanguageSelectorVM> LanguageList
        {
            get
            {
                return _languageList;
            }
            set
            {
                _languageList = value;
                RaisePropertyChanged("LanguageList");
            }
        }
        #endregion

        private string _selectedFolder = "";
        public string SelectedFolder
        {
            get
            {
                return _selectedFolder;
            }
            set
            {
                _selectedFolder = value;
                RaisePropertyChanged("SelectedFolder");
            }
        }

        private decimal _fuelPrice = 1.659m;
        public decimal FuelPrice
        {
            get
            {
                return _fuelPrice;
            }
            set
            {
                _fuelPrice = value;
                RaisePropertyChanged("FuelPrice");
            }
        }

        private decimal _electricityPrice = 11.36m;
        public decimal ElectricityPrice
        {
            get
            {
                return _electricityPrice;
            }
            set
            {
                _electricityPrice = value;
                RaisePropertyChanged("ElectricityPrice");
            }
        }

        #endregion

        #region localized text
        protected override void ManageLanguage_OnLanguageChanged(object sender, LanguageChangedEventArgs changeArgs)
        {
            base.ManageLanguage_OnLanguageChanged(sender, changeArgs);
            RaisePropertyChanged("TorqueDataT");
            RaisePropertyChanged("DirectoryT");
        }

        public String TorqueDataT
        {
            get { return ManageLanguage.GetLanguageString("Home$TorqueDataT", ManageLanguage.LanguageSelected); }
        }

        public String DirectoryT
        {
            get { return ManageLanguage.GetLanguageString("Home$DirectoryT", ManageLanguage.LanguageSelected); }
        }
        #endregion

        #region function

        private void LoadData()
        {
            IsBusy = true;

            ViewModelLocator.CurrentInstance.AbstractTorqueData.LoadHomeData((success, eventMessages, result) =>
            {
                if (success)
                {
                    CarInfo = result;
                    CarInfo.CostCharging = Decimal.Round((ElectricityPrice * CarInfo.TotalCharging) / 100, 2);
                    CarInfo.CostFuel = Decimal.Round(FuelPrice * CarInfo.TotalFuelUsed, 2);
                    CarInfo.TotalCost = Decimal.Round(CarInfo.CostFuel + CarInfo.CostCharging, 2);

                    if (CarInfo.TotalKM != 0)
                    {
                        CarInfo.CostPer100KM = Decimal.Round((CarInfo.TotalCost / CarInfo.TotalKM) * 100, 2);
                    }

                    BasicLinesGVM.GraphConsumption(0, "Home$SerieFuelC", CarInfo.DateFuelConsumption, CarInfo.FuelConsumptionList);
                }
                else
                {
                    VoltAnalyzerMessage.Send<string>(MessageConstants.ShowMessageView, eventMessages);
                }

                IsBusy = false;
            }, SelectedFolder);
        }

        #endregion

        #region command execution

        private void DefaultDirectory()
        {
            try
            {
                ViewModelLocator.CurrentInstance.FileDialogVM.BrowseFolderCommand.Execute(null);
                if (!string.IsNullOrEmpty(ViewModelLocator.CurrentInstance.FileDialogVM.SelectedFolder))
                {
                    SelectedFolder = ViewModelLocator.CurrentInstance.FileDialogVM.SelectedFolder;
                }
            }
            catch (Exception ex)
            {
                VoltAnalyzerMessage.Send<string>(MessageConstants.ShowMessageView, "Error during the opening of the selected file: " + ex.Message + " " + ex.InnerException);
            }
        }

        private void Close(IWindow a_window)
        {
            if (a_window != null)
            {
                a_window.Close();
            }
        }

        private void Expand(IWindow a_window)
        {
            if (a_window != null)
            {
                a_window.Expand();
            }
        }

        private void Minimize(IWindow a_window)
        {
            if (a_window != null)
            {
                a_window.Minimize();
            }
        }

        private void Move(IWindow a_window)
        {
            if (a_window != null)
            {
                a_window.Move();
            }
        }

        private void LoadTorqueData()
        {
            if (string.IsNullOrEmpty(SelectedFolder))
            {
                VoltAnalyzerMessage.Send<string>(MessageConstants.ShowMessageView, ManageLanguage.GetLanguageString("Home$FodlerEmpty", ManageLanguage.LanguageSelected));
            }
            else
            {
                var task = new Task(LoadData);
                task.Start();
            }
        }

        #endregion

        #region Command

        private RelayCommand _loadTorqueDataCommand;
        public RelayCommand LoadTorqueDataCommand
        {
            get
            {
                return _loadTorqueDataCommand;
            }
            set
            {
                _loadTorqueDataCommand = value;
                RaisePropertyChanged("LoadTorqueDataCommand");
            }
        }

        private RelayCommand<IWindow> _closeCommand;
        public RelayCommand<IWindow> CloseCommand
        {
            get
            {
                return _closeCommand;
            }
            set
            {
                _closeCommand = value;
                RaisePropertyChanged("CloseCommand");
            }
        }

        private RelayCommand<IWindow> _minimizeCommand;
        public RelayCommand<IWindow> MinimizeCommand
        {
            get
            {
                return _minimizeCommand;
            }
            set
            {
                _minimizeCommand = value;
                RaisePropertyChanged("MinimizeCommand");
            }
        }

        private RelayCommand<IWindow> _expandCommand;
        public RelayCommand<IWindow> ExpandCommand
        {
            get
            {
                return _expandCommand;
            }
            set
            {
                _expandCommand = value;
                RaisePropertyChanged("ExpandCommand");
            }
        }

        private RelayCommand<IWindow> _moveCommand;
        public RelayCommand<IWindow> MoveCommand
        {
            get
            {
                return _moveCommand;
            }
            set
            {
                _moveCommand = value;
                RaisePropertyChanged("MoveCommand");
            }
        }

        private RelayCommand _defaultDirectoryCommand;
        public RelayCommand DefaultDirectoryCommand
        {
            get
            {
                return _defaultDirectoryCommand;
            }
            set
            {
                _defaultDirectoryCommand = value;
                RaisePropertyChanged("DefaultDirectoryCommand");
            }
        }

        #endregion
    }
}
