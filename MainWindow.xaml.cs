using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace OFGB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        public MainWindow()
        {
            InitializeComponent();

            IntPtr handle = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
            if (DwmSetWindowAttribute(handle, 19, [1], 4) != 0)
            {
                DwmSetWindowAttribute(handle, 20, [1], 4);
            }

            InitializeKeys();
        }

        private void InitializeKeys()
        {
            // Sync provider notifications in File Explorer
            bool key1 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowSyncProviderNotifications");
            cb1.IsChecked = !key1;

            // Get fun facts, tips, tricks, and more on your lock screen
            bool key2 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "RotatingLockScreenOverlayEnabled");
            bool key3 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338387Enabled");
            cb2.IsChecked = !key2 && !key3;

            // Show suggested content in Settings app
            bool key4 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338393Enabled");
            bool key5 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353694Enabled");
            bool key6 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353696Enabled");
            cb3.IsChecked = !key4 && !key5 && !key6;

            // Get tips and suggestions when using Windows
            bool key7 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338389Enabled");
            cb4.IsChecked = !key7;

            // Suggest ways to get the most out of Windows and finish setting up this device
            bool key8 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\UserProfileEngagement", "ScoobeSystemSettingEnabled");
            cb5.IsChecked = !key8;

            // Show me the Windows welcome experience after updates and occasionally when I sign in to highlight what's new and suggested
            bool key9 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-310093Enabled");
            cb6.IsChecked = !key9;

            // Let apps show me personalized ads by using my advertising ID
            bool key10 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo", "Enabled");
            cb7.IsChecked = !key10;

            // Tailored experiences
            bool key11 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled");
            cb8.IsChecked = !key11;

            // "Show recommendations for tips, shortcuts, new apps, and more" on Start
            bool key12 = CreateKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "Start_IrisRecommendations");
            cb9.IsChecked = !key12;
        }

        private bool CreateKey(string loc, string key)
        {
            if (Registry.CurrentUser.OpenSubKey(loc, true) is not null)
            {
                RegistryKey? keyRef = Registry.CurrentUser.OpenSubKey(loc, true);

                if (keyRef is not null && keyRef.GetValue(key) is null)
                {
                    keyRef.SetValue(key, 0);
                    return false;
                }
                else if (keyRef is not null)
                {
                    return (Convert.ToInt32(keyRef.GetValue(key)) != 0);
                }
                else
                {
                    throw new InvalidOperationException("Null KeyRef");
                }
            }
            else
            {
                throw new InvalidOperationException("Error Initializing While Creating Key");
            }
        }

        private bool toggleOptions(string name, bool enable)
        {
            int value = !enable ? unchecked((int)0x00000001u) : unchecked((int)0x00000000u);

            switch (name)
            {
                case "cb1":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "ShowSyncProviderNotifications", value);
                    break;
                case "cb2":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "RotatingLockScreenOverlayEnabled", value);
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338387Enabled", value);
                    break;
                case "cb3":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338393Enabled", value);
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353694Enabled", value);
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-353696Enabled", value);
                    break;
                case "cb4":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-338389Enabled", value);
                    break;
                case "cb5":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\UserProfileEngagement", "ScoobeSystemSettingEnabled", value);
                    break;
                case "cb6":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager", "SubscribedContent-310093Enabled", value);
                    break;
                case "cb7":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo", "Enabled", value);
                    break;
                case "cb8":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Privacy", "TailoredExperiencesWithDiagnosticDataEnabled", value);
                    break;
                case "cb9":
                    Registry.SetValue("HKEY_CURRENT_USER\\" + "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", "Start_IrisRecommendations", value);
                    break;
            }
            return true;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            toggleOptions(((CheckBox)sender).Name, true);
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            toggleOptions(((CheckBox)sender).Name, false);
        }
    }
}
