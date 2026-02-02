using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace App1
{
    /// <summary>
    /// VoiceScribeアプリケーションクラス
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();

            // システムトレイアイコンを初期化（H.NotifyIcon.WinUI使用）
            InitializeSystemTray();
        }

        private void InitializeSystemTray()
        {
            try
            {
                // TODO: H.NotifyIcon.WinUIを使用したシステムトレイ実装
                // 以下のようなコードで実装予定:
                // var notifyIcon = new NotifyIcon();
                // notifyIcon.Text = "VoiceScribe";
                // notifyIcon.Icon = new Icon("Assets/app-icon.ico");
                // notifyIcon.ContextMenu = CreateContextMenu();
                // notifyIcon.Visible = true;

                System.Diagnostics.Debug.WriteLine("システムトレイの初期化");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"システムトレイ初期化エラー: {ex.Message}");
            }
        }
    }
}
