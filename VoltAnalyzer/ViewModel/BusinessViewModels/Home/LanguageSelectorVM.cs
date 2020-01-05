using LanguageManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ViewModel.BusinessViewModels.Home
{
    public class LanguageSelectorVM: ViewModelBase
    {
        #region constructor
        public LanguageSelectorVM(string a_name)
        {
            _languageName = a_name;
            _languagePictureName = "pack://application:,,,/Resources/"+a_name+ ".png";
        }
        #endregion

        #region property
        private string _languageName = "";
        public string LanguageName
        {
            get
            {
                return _languageName;
            }
            set
            {
                _languageName = value;
                RaisePropertyChanged("LanguageName");
            }
        }

        private string _languagePictureName = "";
        public string LanguagePictureName
        {
            get
            {
                return _languagePictureName;
            }
            set
            {
                _languagePictureName = value;
                RaisePropertyChanged("LanguagePictureName");
            }
        }
        #endregion
    }
}
