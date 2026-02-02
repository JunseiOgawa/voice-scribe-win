# VoiceScribe - Windows Voice Transcription Application

高精度・高速なローカル音声文字起こしアプリケーション

## 概要

VoiceScribe は Windows 向けの音声文字起こしアプリケーションです。既存のアプリケーションの問題点を解決します：

- ✅ 日本語文字起こしの高精度化
- ✅ 変換速度の改善（高速モデル対応）
- ✅ 柔軟な操作性（フォーカス制御、キーボード操作対応）
- ✅ 完全ローカル処理（オフライン動作、プライバシー保護）

## 技術スタック

- **UI**: WinUI 3（Windows App SDK 1.5+）
- **言語**: C# 12
- **.NET**: .NET 8 LTS
- **アーキテクチャ**: MVVM（CommunityToolkit.Mvvm）
- **音声入力**: NAudio
- **音声認識**: ReazonSpeech-NeMo v2
- **キーボード入力**: Windows Input Simulator
- **システムトレイ**: H.NotifyIcon.WinUI

## 機能（MVP）

### Phase 1 - コア機能
- リアルタイム音声認識（高速モデル）
- マイク入力のキャプチャ・処理
- 日本語文字起こし（ReazonSpeech-NeMo v2）
- アクティブウィンドウへのテキスト入力
- システムトレイ常駐
- ホットキー操作（デフォルト: Ctrl+Shift+V）
- マイク選択、ホットキーカスタマイズ

### Phase 2 - 拡張機能
- 高速 ⇔ 高精度モデルの切り替え
- フォーカス制御オプション（アクティブウィンドウ追従）
- テキスト編集機能（アプリ内エディタ）
- 認識履歴表示

### Phase 3 - 高度な機能
- 音声区切り検出（VAD）
- 句読点自動挿入
- ノイズ除去
- カスタム辞書機能

## パフォーマンス目標

- 認識レイテンシ: 500ms以内（高速モデル）
- メモリ使用量: 1GB以内
- CPU使用率: アイドル時 5%以内
- 対応環境: Windows 10 21H2 以降

## プロジェクト構造

```
voice-scribe-win/
├── .github/
│   └── workflows/          # CI/CD (GitHub Actions)
├── src/
│   ├── VoiceScribe/        # WinUI 3 アプリケーション
│   │   ├── Views/          # XAML UI
│   │   ├── ViewModels/     # MVVM ViewModel
│   │   ├── Models/         # データモデル
│   │   ├── Services/       # ビジネスロジック
│   │   └── Helpers/        # ユーティリティ
│   ├── VoiceScribe.Core/   # 共通ロジック（テスト可能）
│   └── VoiceScribe.Tests/  # 単体テスト
├── docs/
│   └── requirements.md     # 要件定義書
├── models/                 # 音声認識モデル
├── README.md
├── .gitignore
└── LICENSE
```

## セットアップ手順

### 必要な環境
- Windows 10 21H2 以降
- .NET 8 SDK
- Visual Studio 2022 以降（WinUI 3 サポート）

### インストール
1. リポジトリをクローン
   ```bash
   git clone https://github.com/yourusername/voice-scribe-win.git
   ```

2. 依存関係をインストール
   ```bash
   dotnet restore
   ```

3. プロジェクトをビルド
   ```bash
   dotnet build
   ```

4. アプリケーションを実行
   ```bash
   dotnet run --project src/VoiceScribe
   ```

## 使用方法

1. アプリケーションを起動 → システムトレイに常駐
2. **Ctrl+Shift+V** を押下 → 認識開始（アイコンが変化）
3. マイクに向かって日本語で話す
4. もう一度 **Ctrl+Shift+V** または数秒の無音で認識停止
5. テキストがアクティブなウィンドウに入力される

### 設定
システムトレイアイコンを右クリック → **設定** でカスタマイズ可能：
- マイクデバイス選択
- ホットキー変更
- モデル選択（高速 / 高精度）

## 技術的課題と検証

### 主な課題
1. **ReazonSpeech-NeMo v2 の .NET 統合**
   - ONNX Runtime または Python interop での実現

2. **キーボード入力シミュレーション**
   - セキュリティソフトによるブロック対策
   - 管理者権限の必要性確認

3. **パフォーマンス**
   - ローカル処理の最適化
   - リアルタイム処理の実現

## 開発進捗

- [ ] Step 1: プロジェクト初期化
- [ ] Step 2: 音声入力基盤
- [ ] Step 3: 音声認識統合
- [ ] Step 4: テキスト出力機能
- [ ] Step 5: UI実装
- [ ] Step 6: 統合・テスト

## ライセンス

MIT License - 詳細は [LICENSE](LICENSE) を参照

## 参考資料

- [WinUI 3 Documentation](https://learn.microsoft.com/en-us/windows/apps/winui/)
- [NAudio Documentation](https://github.com/naudio/NAudio)
- [ReazonSpeech GitHub](https://github.com/reazon-research/ReazonSpeech)
- [MVVM Community Toolkit](https://github.com/CommunityToolkit/dotnet)

## 貢献

プルリクエストを歓迎します。大きな変更の場合は、まずイシューを開いて変更内容を議論してください。

## サポート

問題が発生した場合は、[GitHub Issues](https://github.com/yourusername/voice-scribe-win/issues) でお知らせください。
