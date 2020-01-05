using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LanguageManager
{
    public class ManageLanguage
    {
        public static event LanguageChangedEventHandler OnLanguageChanged;
        private static volatile Dictionary<string, bool> Loaded = new Dictionary<string, bool>();
        private static volatile Dictionary<string, Dictionary<string, Dictionary<string, string>>> AllTexts;
        private static volatile string _languageSelected = "";
        private static volatile List<string> listlanguage = new List<string>();

        public static string LanguageSelected
        {
            get
            {
                return ManageLanguage._languageSelected;
            }
        }

        public static List<string> GetAllLanguages()
        {
            return listlanguage;
        }

        private static void registerLabel(string language, string Name, string ID, string label)
        {
            if (ManageLanguage.AllTexts == null)
                ManageLanguage.AllTexts = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            Dictionary<string, Dictionary<string, string>> dictionary1 = (Dictionary<string, Dictionary<string, string>>)null;
            if (ManageLanguage.AllTexts.ContainsKey(language))
                dictionary1 = ManageLanguage.AllTexts[language];
            if (dictionary1 == null)
            {
                dictionary1 = new Dictionary<string, Dictionary<string, string>>();
                ManageLanguage.AllTexts.Add(language, dictionary1);
            }
            Dictionary<string, string> dictionary2;
            if (dictionary1.ContainsKey(Name))
            {
                dictionary2 = dictionary1[Name];
            }
            else
            {
                dictionary2 = new Dictionary<string, string>();
                dictionary1.Add(Name, dictionary2);
            }
            if (dictionary2.ContainsKey(ID))
                dictionary2[ID] = "double: " + label;
            else
                dictionary2.Add(ID, label);
        }

        public static void ChangeLanguage(string language)
        {
            ManageLanguage._languageSelected = language;

            if (ManageLanguage.AllTexts == null || ManageLanguage.OnLanguageChanged == null)
                return;
            ManageLanguage.OnLanguageChanged(ManageLanguage.OnLanguageChanged.Target, new LanguageChangedEventArgs()
            {
                CurrentLanguage = ManageLanguage.LanguageSelected,
                AllTexts = ManageLanguage.AllTexts
            });
        }

        public static void loadXml(string xmlFile)
        {
            if (ManageLanguage.Loaded.ContainsKey(xmlFile) && (!ManageLanguage.Loaded.ContainsKey(xmlFile) || ManageLanguage.Loaded[xmlFile]))
            {
                return;
            }
            else
            {
                foreach (XElement sub1 in XDocument.Parse(xmlFile).Descendants((XName)"Group"))
                {
                    string Name = sub1.Attribute((XName)"ID").Value;
                    foreach (XElement sub2 in sub1.Descendants((XName)"Element"))
                    {
                        string ID = sub2.Attribute((XName)"ID").Value;
                        foreach (XElement sub3 in sub2.Descendants((XName)"Text"))
                        {
                            string language = sub3.Attribute((XName)"ID").Value;

                            if(!listlanguage.Contains(language))
                            {
                                listlanguage.Add(language);
                            }

                            string Label = sub3.Value;
                            ManageLanguage.registerLabel(language, Name, ID, Label);
                        }
                    }
                }

                if (ManageLanguage.Loaded.ContainsKey(xmlFile))
                {
                    ManageLanguage.Loaded[xmlFile] = true;
                }
                else
                {
                    ManageLanguage.Loaded.Add(xmlFile, true);
                }
            }
        }

        public static string GetLanguageString(string code, string language)
        {
            string res = string.Empty;
            if (ManageLanguage.Loaded.Count > 0 && ManageLanguage.Loaded.ContainsValue(true))
            {
                if (ManageLanguage.AllTexts != null)
                {
                    if (ManageLanguage.AllTexts.ContainsKey(language.ToUpper()) && code != string.Empty)
                    {
                        string[] strArray1 = code.Split('$');
                        if (strArray1.Length > 2)
                        {
                            string[] strArray2 = new string[strArray1.Length - 2];
                            for (int index = 0; index < strArray1.Length - 2; ++index)
                                strArray2[index] = strArray1[index + 2];
                            Dictionary<string, Dictionary<string, string>> allLabel = ManageLanguage.AllTexts[language.ToUpper()];
                            res = !allLabel.ContainsKey(strArray1[0]) ? "Item not in list" : (!allLabel[strArray1[0]].ContainsKey(strArray1[1]) ? "Item not in list" : string.Format(allLabel[strArray1[0]][strArray1[1]], (object[])strArray2));
                        }
                        else
                        {
                            Dictionary<string, Dictionary<string, string>> allLabel = ManageLanguage.AllTexts[language.ToUpper()];
                            if (allLabel.ContainsKey(strArray1[0]))
                                res = !allLabel[strArray1[0]].ContainsKey(strArray1[1]) ? "Item not in list" : allLabel[strArray1[0]][strArray1[1]];
                        }
                    }
                }
                else
                {
                    res = "Language file not loaded";
                }
            }
            else
            {
                res = "Language file not loaded";
            }

            return res;
        }
    }
}
