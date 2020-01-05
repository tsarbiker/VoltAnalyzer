using GalaSoft.MvvmLight.Command;
using LanguageManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoltAnalyzer.ValueObjects.Types.Common;

namespace VoltAnalyzer.ViewModel.BusinessViewModels.Graph
{
    public class GraphListVM : ViewModelBase
    {
        #region constructor

        public GraphListVM()
        {
            ChangeGraphCommand = new RelayCommand<string>(ChangeGraph);
        }

        #endregion

            #region property

            #region "List name"

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }

        #endregion

        #endregion

        #region command execution

        private void ChangeGraph(string a_type)
        {
            VoltAnalyzerMessage.Send<string>(MessageConstants.ChangeGraph, a_type);
        }

        #endregion

        #region Command

        private RelayCommand<string> _changeGraphCommand;
        public RelayCommand<string> ChangeGraphCommand
        {
            get
            {
                return _changeGraphCommand;
            }
            set
            {
                _changeGraphCommand = value;
                RaisePropertyChanged("ChangeGraphCommand");
            }
        }

        #endregion
    }
}
