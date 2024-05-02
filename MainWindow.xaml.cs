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
        const string cur_ver = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\";

        [DllImport("dwmapi.dll")]
        internal static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

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
            bool key1 = CreateKey(cur_ver + "Explorer\\Advanced", "ShowSyncProviderNotifications");
            cb1.IsChecked = !key1;

            // Get fun facts, tips, tricks, and more on your lock screen
            bool key2 = CreateKey(cur_ver + "ContentDeliveryManager", "RotatingLockScreenOverlayEnabled");
            bool key3 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-338387Enabled");
            cb2.IsChecked = !key2 && !key3;

            // Show suggested content in Settings app
            bool key4 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-338393Enabled");
            bool key5 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-353694Enabled");
            bool key6 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-353696Enabled");
            cb3.IsChecked = !key4 && !key5 && !key6;

            // Get tips and suggestions when using Windows
            bool key7 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-338389Enabled");
            cb4.IsChecked = !key7;

            // Suggest ways to get the most out of Windows and finish setting up this device
            bool key8 = CreateKey(cur_ver + "UserProfileEngagement", "ScoobeSystemSettingEnabled");
            cb5.IsChecked = !key8;

            // Show me the Windows welcome experience after updates and occasionally when I sign in to highlight what's new and suggested
            bool key9 = CreateKey(cur_ver + "ContentDeliveryManager", "SubscribedContent-310093Enabled");
            cb6.IsChecked = !key9;

            // Let apps show me personalized ads by using my advertising ID
            bool key10 = CreateKey(cur_ver + "AdvertisingInfo", "Enabled");
            cb7.IsChecked = !key10;

            // Tailored experiences
            bool key11 = CreateKey(cur_ver + "Privacy", "TailoredExperiencesWithDiagnosticDataEnabled");
            cb8.IsChecked = !key11;

            // "Show recommendations for tips, shortcuts, new apps, and more" on Start
            bool key12 = CreateKey(cur_ver + "Explorer\\Advanced", "Start_IrisRecommendations");
            cb9.IsChecked = !key12;
        }

        private static bool CreateKey(string loc, string key)
        {
            if (Registry.CurrentUser.OpenSubKey(loc, true) is not null)
            {
                RegistryKey? keyRef = Registry.CurrentUser.OpenSubKey(loc, true);

                if (keyRef is not null && keyRef.GetValue(key) is null)
                {
                    keyRef.SetValue(key, 0);
                    keyRef.Close();
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
                RegistryKey? keyRef = Registry.CurrentUser.CreateSubKey(loc);

                if (keyRef is not null)
                {
                    keyRef.SetValue(key, 0);
                    keyRef.Close();
                    return false;
                }
                else
                {
                    throw new InvalidOperationException("Null KeyRef");
                }
            }
        }

        private static bool ToggleOptions(string name, bool enable)
        {
            int value = Convert.ToInt32(!enable);

            switch (name)
            {
                case "cb1":
                    Registry.CurrentUser.SetValue(cur_ver + "Explorer\\Advanced\\" + "ShowSyncProviderNotifications", value);
                    break;
                case "cb2":
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "RotatingLockScreenOverlayEnabled", value);
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-338387Enabled", value);
                    break;
                case "cb3":
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-338393Enabled", value);
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-353694Enabled", value);
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-353696Enabled", value);
                    break;
                case "cb4":
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-338389Enabled", value);
                    break;
                case "cb5":
                    Registry.CurrentUser.SetValue(cur_ver + "UserProfileEngagement" + "ScoobeSystemSettingEnabled", value);
                    break;
                case "cb6":
                    Registry.CurrentUser.SetValue(cur_ver + "ContentDeliveryManager" + "SubscribedContent-310093Enabled", value);
                    break;
                case "cb7":
                    Registry.CurrentUser.SetValue(cur_ver + "AdvertisingInfo" + "Enabled", value);
                    break;
                case "cb8":
                    Registry.CurrentUser.SetValue(cur_ver + "Privacy" + "TailoredExperiencesWithDiagnosticDataEnabled", value);
                    break;
                case "cb9":
                    Registry.CurrentUser.SetValue(cur_ver + "Explorer\\Advanced" + "Start_IrisRecommendations", value);
                    break;
            }
            return true;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            ToggleOptions(((CheckBox)sender).Name, true);
        }

        private void Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleOptions(((CheckBox)sender).Name, false);
        }
    }
}
