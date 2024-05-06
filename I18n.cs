using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace OFGB
{
    public class I18n : INotifyPropertyChanged
    {
        private readonly ResourceManager manager;

        private static readonly Lazy<I18n> lazy = new(() => new I18n());
        public static I18n Instance => lazy.Value;
        public event PropertyChangedEventHandler? PropertyChanged;

        public I18n()
        {
            manager = new ResourceManager("OFGB.Resources.i18n", typeof(I18n).Assembly);
        }

        public string this[string name]
        {
            get
            {
                ArgumentNullException.ThrowIfNull(name);
                return manager.GetString(name) ?? string.Empty;
            }
        }

        public void ApplyLanguage(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("item[]"));
        }
    }
}