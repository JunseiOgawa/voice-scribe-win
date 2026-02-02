using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface ISpeechRecognitionService
    {
        event EventHandler<string>? RecognitionResult;
        event EventHandler<string>? ErrorOccurred;
        
        Task InitializeAsync();
        Task RecognizeAudioAsync(byte[] audioData);
        string CurrentModel { get; set; }
    }

    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private const string ModelDirectory = "models";
        private const string FastModelName = "reazonspeech_nemo_v2_fast.onnx";
        private const string AccurateModelName = "reazonspeech_nemo_v2_accurate.onnx";

        public event EventHandler<string>? RecognitionResult;
        public event EventHandler<string>? ErrorOccurred;

        public string CurrentModel { get; set; } = "fast";

        private bool _isInitialized;
        private string _modelPath = "";

        public async Task InitializeAsync()
        {
            try
            {
                // Check if models exist
                _modelPath = Path.Combine(AppContext.BaseDirectory, ModelDirectory);
                if (!Directory.Exists(_modelPath))
                {
                    Directory.CreateDirectory(_modelPath);
                    ErrorOccurred?.Invoke(this, $"モデルディレクトリを作成しました: {_modelPath}\nReazonSpeechモデルをダウンロードしてください。");
                    await Task.CompletedTask;
                    return;
                }

                var modelFiles = Directory.GetFiles(_modelPath, "*.onnx");
                if (modelFiles.Length == 0)
                {
                    ErrorOccurred?.Invoke(this, 
                        $"モデルが見つかりません: {_modelPath}\nReazonSpeech-NeMo v2をダウンロードしてください。");
                    await Task.CompletedTask;
                    return;
                }

                _isInitialized = true;
                System.Diagnostics.Debug.WriteLine($"初期化完了。見つかったモデル: {string.Join(", ", modelFiles.Select(Path.GetFileName))}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"初期化失敗: {ex.Message}");
                await Task.CompletedTask;
            }
        }

        public async Task RecognizeAudioAsync(byte[] audioData)
        {
            try
            {
                if (audioData == null || audioData.Length == 0)
                {
                    ErrorOccurred?.Invoke(this, "オーディオデータが空です");
                    return;
                }

                // TODO: ONNX Runtimeを使用した実際の音声認識実装
                // 以下は基本的な処理フロー:
                // 1. オーディオデータを前処理
                // 2. ONNXモデルをロード
                // 3. 推論を実行
                // 4. 結果を後処理

                var result = await PerformRecognitionAsync(audioData);
                RecognitionResult?.Invoke(this, result);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"認識失敗: {ex.Message}");
            }
        }

        private async Task<string> PerformRecognitionAsync(byte[] audioData)
        {
            // シミュレーション: 音声データの長さに基づいて処理時間を計算
            float durationSeconds = audioData.Length / (16000f * 2); // 16kHz, 16-bit
            int processingTime = Math.Max(500, (int)(durationSeconds * 1000 * 0.3)); // 処理時間をシミュレート

            await Task.Delay(processingTime);

            // 実装予定: 日本語音声認識結果を返す
            // 現在はモデルが未実装のため空文字列を返す
            return "";
        }

        private float[] PreprocessAudio(byte[] audioData, int sampleRate = 16000)
        {
            // バイト配列をフロート配列に変換
            var floatData = new float[audioData.Length / 2];
            for (int i = 0; i < floatData.Length; i++)
            {
                short sample = BitConverter.ToInt16(audioData, i * 2);
                floatData[i] = sample / 32768f; // [-1, 1]に正規化
            }
            return floatData;
        }

        private List<int> PostProcessOutput(float[] modelOutput)
        {
            // モデル出力をトークンIDのリストに変換
            var tokenIds = new List<int>();
            
            // argmax を取得
            float maxValue = modelOutput[0];
            int maxIdx = 0;
            
            for (int i = 1; i < modelOutput.Length; i++)
            {
                if (modelOutput[i] > maxValue)
                {
                    maxValue = modelOutput[i];
                    maxIdx = i;
                }
            }
            
            tokenIds.Add(maxIdx);
            return tokenIds;
        }
    }
}
