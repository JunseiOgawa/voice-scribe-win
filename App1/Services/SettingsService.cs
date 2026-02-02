using System;
using Windows.Storage;
using Windows.System;

namespace App1.Services
{
    public interface ISettingsService
    {
        int SelectedMicrophoneId { get; set; }
        VirtualKey HotKeyKey { get; set; }
        VirtualKeyModifiers HotKeyModifiers { get; set; }
        string TranscriptionModel { get; set; }
        bool FocusFollowsActiveWindow { get; set; }
        
        void LoadSettings();
        void SaveSettings();
    }

    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDataContainer _localSettings;

        public int SelectedMicrophoneId
        {
            get => _localSettings.Values.ContainsKey("SelectedMicrophoneId") ? (int)_localSettings.Values["SelectedMicrophoneId"] : 0;
            set => _localSettings.Values["SelectedMicrophoneId"] = value;
        }

        public VirtualKey HotKeyKey
        {
            get => _localSettings.Values.ContainsKey("HotKeyKey") ? (VirtualKey)(int)_localSettings.Values["HotKeyKey"] : VirtualKey.V;
            set => _localSettings.Values["HotKeyKey"] = (int)value;
        }

        public VirtualKeyModifiers HotKeyModifiers
        {
            get => _localSettings.Values.ContainsKey("HotKeyModifiers") ? (VirtualKeyModifiers)(int)_localSettings.Values["HotKeyModifiers"] : 
                   (VirtualKeyModifiers.Control | VirtualKeyModifiers.Shift);
            set => _localSettings.Values["HotKeyModifiers"] = (int)value;
        }

        public string TranscriptionModel
        {
            get => _localSettings.Values.ContainsKey("TranscriptionModel") ? (string)_localSettings.Values["TranscriptionModel"] : "fast";
            set => _localSettings.Values["TranscriptionModel"] = value;
        }

        public bool FocusFollowsActiveWindow
        {
            get => _localSettings.Values.ContainsKey("FocusFollowsActiveWindow") ? (bool)_localSettings.Values["FocusFollowsActiveWindow"] : false;
            set => _localSettings.Values["FocusFollowsActiveWindow"] = value;
        }

        public SettingsService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public void LoadSettings()
        {
            // Settings are loaded on-demand from the container
        }

        public void SaveSettings()
        {
            // Settings are saved immediately to the container
        }
    }
}
