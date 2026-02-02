using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace App1.Services
{
    public interface ITextInputService
    {
        Task InputTextAsync(string text);
    }

    public class TextInputService : ITextInputService
    {
        public async Task InputTextAsync(string text)
        {
            try
            {
                // クリップボード経由でテキストを入力
                await InputViaClipboardAsync(text);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Text input failed: {ex.Message}");
            }
        }

        private async Task InputViaClipboardAsync(string text)
        {
            try
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(text);
                Clipboard.SetContent(dataPackage);

                // Ctrl+V キーをシミュレート（TODO: WinRT APIで実装）
                // 現在のバージョンではクリップボードへの設定のみを行い、
                // Ctrl+Vは外部で実装するか、UIスレッドで実装する必要があります

                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Clipboard input failed: {ex.Message}");
            }
        }

        private bool ContainsJapanese(string text)
        {
            foreach (var ch in text)
            {
                var code = (int)ch;
                if ((code >= 0x3040 && code <= 0x309F) || // Hiragana
                    (code >= 0x30A0 && code <= 0x30FF) || // Katakana
                    (code >= 0x4E00 && code <= 0x9FFF))   // CJK Unified Ideographs
                {
                    return true;
                }
            }
            return false;
        }
    }
}
