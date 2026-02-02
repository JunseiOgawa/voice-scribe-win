# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-02-02

### Added
- Initial project structure and setup
- WinUI 3 application foundation
- Comprehensive requirements document
- CI/CD workflow (GitHub Actions)
- Project documentation and README
- MIT License

### Planned for Phase 1 (MVP)
- Audio input service (NAudio integration)
- ReazonSpeech-NeMo v2 speech recognition
- Keyboard input simulation for text output
- System tray interface with hotkey support
- Settings page for microphone and hotkey configuration
- Basic unit tests

### Planned for Phase 2
- Dual-model support (fast and accurate modes)
- Advanced focus control options
- In-app text editor for result review
- Recognition history

### Planned for Phase 3
- Voice Activity Detection (VAD)
- Automatic punctuation insertion
- Noise removal
- Custom dictionary support
- Statistics and logging

---

## Development Notes

### Technology Stack
- **Framework**: WinUI 3 (Windows App SDK 1.5+)
- **Language**: C# 12
- **.NET Runtime**: .NET 8 LTS
- **Architecture**: MVVM with CommunityToolkit.Mvvm
- **Speech Engine**: ReazonSpeech-NeMo v2
- **Key Libraries**: NAudio, H.NotifyIcon.WinUI

### Key Milestones
1. ✅ Project structure and requirements
2. ⏳ Audio input foundation (Step 2)
3. ⏳ Speech recognition integration (Step 3)
4. ⏳ Text output mechanism (Step 4)
5. ⏳ UI implementation (Step 5)
6. ⏳ Integration and testing (Step 6)

### Known Issues / To Investigate
- ReazonSpeech-NeMo v2 .NET integration strategy (ONNX vs Python interop)
- Keyboard simulation compatibility with security software
- Performance optimization for real-time processing
