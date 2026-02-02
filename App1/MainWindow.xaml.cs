using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinRT.Interop;
using App1.ViewModels;
using App1.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// Main application window for VoiceScribe
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetWindowSize(600, 700);

            // Initialize services and ViewModel
            var audioCaptureService = new AudioCaptureService();
            var speechRecognitionService = new SpeechRecognitionService();
            var textInputService = new TextInputService();
            var settingsService = new SettingsService();
            var hotKeyService = new HotKeyService();
            
            hotKeyService.SetWindow(this);

            var viewModel = new MainViewModel(
                audioCaptureService,
                speechRecognitionService,
                textInputService,
                settingsService,
                hotKeyService);

            // Since WinUI 3 Window doesn't have DataContext, we'll use Tag as workaround
            // or we can set it directly on child elements
            ((Grid)this.Content).DataContext = viewModel;

            // Set up hotkey handler
            this.Closed += (s, e) => hotKeyService.Unregister();
        }

        private void SetWindowSize(int width, int height)
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(width, height));
        }

        private void OnClearClicked(object sender, RoutedEventArgs e)
        {
            if (((Grid)this.Content).DataContext is MainViewModel viewModel)
            {
                viewModel.RecognizedText = "";
            }
        }
    }
}
