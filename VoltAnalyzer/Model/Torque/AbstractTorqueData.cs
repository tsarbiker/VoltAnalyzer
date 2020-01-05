using VoltAnalyzer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoltAnalyzer.ViewModel.BusinessViewModels.Home;

namespace VoltAnalyzer.Model.DataServices.Torque
{
    public abstract class AbstractTorqueData
    {
        public delegate void LoadHomeDataCallBack(bool a_success, string a_eventMessages, CarInfoVM a_myCarInfo);
        public abstract void LoadHomeData(LoadHomeDataCallBack a_processResult, string a_parameter);
    }
}
