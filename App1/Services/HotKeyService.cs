using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using Windows.System;

namespace App1.Services
{
    public interface IHotKeyService
    {
        event EventHandler? HotKeyPressed;
        bool Register(VirtualKey key, VirtualKeyModifiers modifiers);
        bool Unregister();
        VirtualKey CurrentKey { get; }
        VirtualKeyModifiers CurrentModifiers { get; }
    }

    public class HotKeyService : IHotKeyService
    {
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 9000;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_WIN = 0x0008;

        public event EventHandler? HotKeyPressed;

        public VirtualKey CurrentKey { get; private set; } = VirtualKey.V;
        public VirtualKeyModifiers CurrentModifiers { get; private set; } = VirtualKeyModifiers.Control | VirtualKeyModifiers.Shift;

        private Window? _mainWindow;
        private IntPtr _windowHandle;

        public HotKeyService()
        {
        }

        public void SetWindow(Window window)
        {
            _mainWindow = window;
            _windowHandle = GetWindowHandle(window);
        }

        public bool Register(VirtualKey key, VirtualKeyModifiers modifiers)
        {
            try
            {
                if (_windowHandle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine("Window handle not set");
                    return false;
                }

                CurrentKey = key;
                CurrentModifiers = modifiers;

                uint fsModifiers = ConvertModifiers(modifiers);
                uint vk = (uint)key;

                bool result = RegisterHotKey(_windowHandle, HOTKEY_ID, fsModifiers, vk);
                if (!result)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to register hotkey");
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HotKey registration error: {ex.Message}");
                return false;
            }
        }

        public bool Unregister()
        {
            try
            {
                if (_windowHandle == IntPtr.Zero)
                    return false;

                return UnregisterHotKey(_windowHandle, HOTKEY_ID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HotKey unregistration error: {ex.Message}");
                return false;
            }
        }

        public void OnHotKeyPressed()
        {
            HotKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        private uint ConvertModifiers(VirtualKeyModifiers modifiers)
        {
            uint result = 0;
            if (modifiers.HasFlag(VirtualKeyModifiers.Control))
                result |= MOD_CONTROL;
            if (modifiers.HasFlag(VirtualKeyModifiers.Shift))
                result |= MOD_SHIFT;
            if (modifiers.HasFlag(VirtualKeyModifiers.Menu))
                result |= MOD_ALT;
            if (modifiers.HasFlag(VirtualKeyModifiers.Windows))
                result |= MOD_WIN;
            return result;
        }

        private IntPtr GetWindowHandle(Window window)
        {
            return ((Microsoft.UI.Xaml.Window)window).GetWindowHandle();
        }
    }

    // Extension method for getting window handle
    public static class WindowExtensions
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        public static IntPtr GetWindowHandle(this Microsoft.UI.Xaml.Window window)
        {
            return WinRT.Interop.WindowNative.GetWindowHandle(window);
        }
    }
}
