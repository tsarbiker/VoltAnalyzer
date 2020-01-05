using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageManager
{
    public class ViewModelBase: GalaSoft.MvvmLight.ViewModelBase
    {
        public ViewModelBase()
        {
            ManageLanguage.OnLanguageChanged += new LanguageChangedEventHandler(this.ManageLanguage_OnLanguageChanged);
        }

        protected virtual void ManageLanguage_OnLanguageChanged(object sender, LanguageChangedEventArgs changeArgs)
        {
        }

        ~ViewModelBase()
        {
            ManageLanguage.OnLanguageChanged -= new LanguageChangedEventHandler(this.ManageLanguage_OnLanguageChanged);
        }
    }
}
