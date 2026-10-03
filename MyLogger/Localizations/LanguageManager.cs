using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShareLibrary.Localizations
{
    public class LanguageManager
    {
        public static string CurLanguage { get; set; } = "en";
        private static readonly List<ResourceDictionary> _languageDict = new List<ResourceDictionary>();
        public static void LoadLanguage(string languagecode)
        {
            try
            {
                ClearLanguageResources();
                var resourceUri = new Uri(
                    $"pack://application:,,,/ShareLibrary;component/Localizations/Resources_{languagecode}.xaml",
                    UriKind.Absolute);
                var langDict = new ResourceDictionary { Source = resourceUri };
                _languageDict.Add(langDict);
                CurLanguage = languagecode;
                Application.Current.Resources.MergedDictionaries.Add(langDict);
            }
            catch (Exception ex)
            {
                //Serilog.Log.ForContext("Channel", "LanguageManager").Error(ex, "Failed to load language resources for {LanguageCode}", languagecode);
            }
        }

        private static void ClearLanguageResources()
        {
            foreach (var dict in _languageDict)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dict);
            }
            _languageDict.Clear();
        }
    }
}
