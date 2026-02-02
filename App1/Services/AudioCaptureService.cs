using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAudioCaptureService
    {
        event EventHandler<byte[]>? AudioDataAvailable;
        event EventHandler<string>? ErrorOccurred;
        
        Task StartRecordingAsync(int deviceId = -1);
        Task StopRecordingAsync();
        bool IsRecording { get; }
        int[] GetAvailableDevices();
        string GetDeviceName(int deviceId);
    }

    public class AudioCaptureService : IAudioCaptureService
    {
        private WaveInEvent? _waveIn;
        private WaveFileWriter? _waveFileWriter;
        private MemoryStream? _memoryStream;
        private bool _isRecording;

        public event EventHandler<byte[]>? AudioDataAvailable;
        public event EventHandler<string>? ErrorOccurred;

        public bool IsRecording => _isRecording;

        public int[] GetAvailableDevices()
        {
            var devices = new int[WaveIn.DeviceCount];
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                devices[i] = i;
            }
            return devices;
        }

        public string GetDeviceName(int deviceId)
        {
            if (deviceId >= 0 && deviceId < WaveIn.DeviceCount)
            {
                return WaveIn.GetCapabilities(deviceId).ProductName;
            }
            return "Default Device";
        }

        public async Task StartRecordingAsync(int deviceId = -1)
        {
            try
            {
                _waveIn = new WaveInEvent
                {
                    DeviceNumber = deviceId >= 0 ? deviceId : 0,
                    WaveFormat = new WaveFormat(16000, 1)
                };

                _memoryStream = new MemoryStream();
                _waveFileWriter = new WaveFileWriter(_memoryStream, _waveIn.WaveFormat);

                _waveIn.DataAvailable += OnWaveInDataAvailable;
                _waveIn.RecordingStopped += OnWaveInRecordingStopped;

                _waveIn.StartRecording();
                _isRecording = true;
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Failed to start recording: {ex.Message}");
                Cleanup();
            }
        }

        public async Task StopRecordingAsync()
        {
            try
            {
                _waveIn?.StopRecording();
                _isRecording = false;
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Failed to stop recording: {ex.Message}");
            }
        }

        private void OnWaveInDataAvailable(object? sender, WaveInEventArgs e)
        {
            _waveFileWriter?.Write(e.Buffer, 0, e.BytesRecorded);
            AudioDataAvailable?.Invoke(this, e.Buffer[..e.BytesRecorded]);
        }

        private void OnWaveInRecordingStopped(object? sender, StoppedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            _waveFileWriter?.Dispose();
            _waveIn?.Dispose();
            _memoryStream?.Dispose();
            _waveFileWriter = null;
            _waveIn = null;
            _memoryStream = null;
            _isRecording = false;
        }
    }
}
