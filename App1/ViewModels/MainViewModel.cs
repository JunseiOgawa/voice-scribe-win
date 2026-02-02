using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App1.Services;
using Windows.System;

namespace App1.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IAudioCaptureService _audioCaptureService;
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly ITextInputService _textInputService;
        private readonly ISettingsService _settingsService;
        private readonly IHotKeyService _hotKeyService;

        [ObservableProperty]
        private bool isRecording;

        [ObservableProperty]
        private string recognizedText = "";

        [ObservableProperty]
        private ObservableCollection<string> deviceList;

        [ObservableProperty]
        private int selectedDeviceIndex = 0;

        [ObservableProperty]
        private string statusMessage = "Ready";

        [ObservableProperty]
        private string selectedModel = "fast";

        [ObservableProperty]
        private bool focusFollowsActiveWindow = false;

        public MainViewModel(
            IAudioCaptureService audioCaptureService,
            ISpeechRecognitionService speechRecognitionService,
            ITextInputService textInputService,
            ISettingsService settingsService,
            IHotKeyService hotKeyService)
        {
            _audioCaptureService = audioCaptureService;
            _speechRecognitionService = speechRecognitionService;
            _textInputService = textInputService;
            _settingsService = settingsService;
            _hotKeyService = hotKeyService;

            DeviceList = new ObservableCollection<string>();

            // Hook up event handlers
            _audioCaptureService.ErrorOccurred += OnAudioServiceError;
            _speechRecognitionService.RecognitionResult += OnRecognitionResult;
            _speechRecognitionService.ErrorOccurred += OnRecognitionServiceError;
            _hotKeyService.HotKeyPressed += OnHotKeyPressed;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                await _speechRecognitionService.InitializeAsync();
                LoadSettings();
                LoadAudioDevices();
                RegisterHotKey();
                StatusMessage = "Initialized successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Initialization error: {ex.Message}";
            }
        }

        private void LoadAudioDevices()
        {
            DeviceList.Clear();
            var devices = _audioCaptureService.GetAvailableDevices();
            foreach (var deviceId in devices)
            {
                var deviceName = _audioCaptureService.GetDeviceName(deviceId);
                DeviceList.Add(deviceName);
            }

            if (DeviceList.Count > 0)
            {
                SelectedDeviceIndex = _settingsService.SelectedMicrophoneId;
            }
        }

        private void LoadSettings()
        {
            SelectedModel = _settingsService.TranscriptionModel;
            FocusFollowsActiveWindow = _settingsService.FocusFollowsActiveWindow;
        }

        private void RegisterHotKey()
        {
            var key = _settingsService.HotKeyKey;
            var modifiers = _settingsService.HotKeyModifiers;
            _hotKeyService.Register(key, modifiers);
        }

        [RelayCommand]
        private async Task StartRecording()
        {
            try
            {
                if (!IsRecording)
                {
                    await _audioCaptureService.StartRecordingAsync(SelectedDeviceIndex);
                    IsRecording = true;
                    RecognizedText = "";
                    StatusMessage = "Recording...";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error starting recording: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task StopRecording()
        {
            try
            {
                if (IsRecording)
                {
                    await _audioCaptureService.StopRecordingAsync();
                    IsRecording = false;
                    StatusMessage = "Ready";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error stopping recording: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task SubmitText()
        {
            if (!string.IsNullOrEmpty(RecognizedText))
            {
                await _textInputService.InputTextAsync(RecognizedText);
                RecognizedText = "";
                StatusMessage = "Text submitted";
            }
        }

        [RelayCommand]
        private void ChangeDevice(int deviceId)
        {
            SelectedDeviceIndex = deviceId;
            _settingsService.SelectedMicrophoneId = deviceId;
            _settingsService.SaveSettings();
        }

        [RelayCommand]
        private void ChangeModel(string model)
        {
            SelectedModel = model;
            _speechRecognitionService.CurrentModel = model;
            _settingsService.TranscriptionModel = model;
            _settingsService.SaveSettings();
        }

        [RelayCommand]
        private void ToggleFocusFollow()
        {
            FocusFollowsActiveWindow = !FocusFollowsActiveWindow;
            _settingsService.FocusFollowsActiveWindow = FocusFollowsActiveWindow;
            _settingsService.SaveSettings();
        }

        private void OnHotKeyPressed(object? sender, EventArgs e)
        {
            if (IsRecording)
            {
                _ = StopRecordingCommand.ExecuteAsync(null);
            }
            else
            {
                _ = StartRecordingCommand.ExecuteAsync(null);
            }
        }

        private void OnRecognitionResult(object? sender, string text)
        {
            RecognizedText = text;
            StatusMessage = "Recognition complete";
        }

        private void OnRecognitionServiceError(object? sender, string error)
        {
            StatusMessage = error;
        }

        private void OnAudioServiceError(object? sender, string error)
        {
            StatusMessage = error;
        }
    }
}
