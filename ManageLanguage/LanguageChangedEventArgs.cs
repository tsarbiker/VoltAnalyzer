using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageManager
{
    public class LanguageChangedEventArgs
    {
        public string CurrentLanguage { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> AllTexts;
    }
}
