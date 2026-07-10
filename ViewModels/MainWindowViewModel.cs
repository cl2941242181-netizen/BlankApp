using BlankApp.Services.TranslateServices;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace BlankApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            TranslateCommand = new DelegateCommand(ExecuteTranslate, CanExecuteTranslate)
                .ObservesProperty(() => SourceText)
                .ObservesProperty(() => IsTranslating);
            SwapLanguagesCommand = new DelegateCommand(ExecuteSwapLanguages);
            CopyResultCommand = new DelegateCommand(ExecuteCopyResult, CanExecuteCopyResult)
                .ObservesProperty(() => TranslatedText);

            InitializeLanguages();

            SelectedSourceLanguage = Languages.First(l => l.Code == "auto");
            SelectedTargetLanguage = Languages.First(l => l.Code == "en");
        }

        #region 属性

        private string _title = "文本翻译工具";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _sourceText = string.Empty;
        public string SourceText
        {
            get => _sourceText;
            set
            {
                if (SetProperty(ref _sourceText, value))
                    RaisePropertyChanged(nameof(SourceTextLength));
            }
        }

        private string _translatedText = string.Empty;
        public string TranslatedText
        {
            get => _translatedText;
            set => SetProperty(ref _translatedText, value);
        }

        public int SourceTextLength => SourceText?.Length ?? 0;

        private ObservableCollection<LanguageInfo> _languages;
        public ObservableCollection<LanguageInfo> Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        private LanguageInfo _selectedSourceLanguage;
        public LanguageInfo SelectedSourceLanguage
        {
            get => _selectedSourceLanguage;
            set => SetProperty(ref _selectedSourceLanguage, value);
        }

        private LanguageInfo _selectedTargetLanguage;
        public LanguageInfo SelectedTargetLanguage
        {
            get => _selectedTargetLanguage;
            set => SetProperty(ref _selectedTargetLanguage, value);
        }

        private bool _isTranslating;
        public bool IsTranslating
        {
            get => _isTranslating;
            set => SetProperty(ref _isTranslating, value);
        }

        #endregion

        #region 命令

        public DelegateCommand TranslateCommand { get; }
        public DelegateCommand SwapLanguagesCommand { get; }
        public DelegateCommand CopyResultCommand { get; }

        #endregion

        #region 初始化

        private void InitializeLanguages()
        {
            Languages = new ObservableCollection<LanguageInfo>
            {
                new LanguageInfo { Name = "🌐 自动检测", Code = "auto" },
                new LanguageInfo { Name = "🇨🇳 中文", Code = "zh" },
                new LanguageInfo { Name = "🇬🇧 英语", Code = "en" },
                new LanguageInfo { Name = "🇯🇵 日语", Code = "jp" },
                new LanguageInfo { Name = "🇰🇷 韩语", Code = "kor" },
                new LanguageInfo { Name = "🇫🇷 法语", Code = "fra" },
                new LanguageInfo { Name = "🇩🇪 德语", Code = "de" },
                new LanguageInfo { Name = "🇪🇸 西班牙语", Code = "spa" },
                new LanguageInfo { Name = "🇷🇺 俄语", Code = "ru" },
                new LanguageInfo { Name = "🇵🇹 葡萄牙语", Code = "pt" },
                new LanguageInfo { Name = "🇮🇹 意大利语", Code = "it" },
                new LanguageInfo { Name = "🇻🇳 越南语", Code = "vie" },
                new LanguageInfo { Name = "🇹🇭 泰语", Code = "th" },
                new LanguageInfo { Name = "🇸🇦 阿拉伯语", Code = "ara" },
            };
        }

        #endregion

        #region 命令执行

        private bool CanExecuteTranslate()
        {
            return !string.IsNullOrWhiteSpace(SourceText) && !IsTranslating;
        }

        private async void ExecuteTranslate()
        {
            if (string.IsNullOrWhiteSpace(SourceText)) return;

            IsTranslating = true;
            TranslatedText = "翻译中…";

            try
            {
                string fromLang = SelectedSourceLanguage?.Code ?? "auto";
                string toLang = SelectedTargetLanguage?.Code ?? "en";

                // 直接调用百度翻译服务的静态方法
                string jsonResult = await BaiduTranslateService.TranslateAsync(
                    SourceText, fromLang, toLang);

                // 使用 JObject 解析返回的 JSON
                TranslatedText = ParseTranslationResult(jsonResult);
            }
            catch (Exception ex)
            {
                TranslatedText = $"翻译出错：{ex.Message}";
                MessageBox.Show($"翻译请求失败：{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsTranslating = false;
            }
        }

        private void ExecuteSwapLanguages()
        {
            if (SelectedSourceLanguage?.Code == "auto") return;

            var temp = SelectedSourceLanguage;
            SelectedSourceLanguage = SelectedTargetLanguage;
            SelectedTargetLanguage = temp;

            if (!string.IsNullOrWhiteSpace(TranslatedText))
            {
                SourceText = TranslatedText;
                TranslatedText = string.Empty;
            }
        }

        private bool CanExecuteCopyResult()
        {
            return !string.IsNullOrWhiteSpace(TranslatedText);
        }

        private void ExecuteCopyResult()
        {
            if (!string.IsNullOrWhiteSpace(TranslatedText))
            {
                Clipboard.SetText(TranslatedText);
                MessageBox.Show("翻译结果已复制到剪贴板！", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region JSON 解析

        /// <summary>
        /// 解析百度翻译 API 返回的 JSON 字符串，提取翻译文本
        /// </summary>
        private static string ParseTranslationResult(string json)
        {
            try
            {
                JObject root = JObject.Parse(json);

                // 检查是否有错误
                string errorCode = root["error_code"]?.ToString();
                if (!string.IsNullOrEmpty(errorCode))
                {
                    string errorMsg = root["error_msg"]?.ToString() ?? "未知错误";
                    return $"翻译失败（{errorCode}）：{errorMsg}";
                }

                // 提取 trans_result 数组中的 dst 字段
                JArray transResult = root["trans_result"] as JArray;
                if (transResult == null || transResult.Count == 0)
                    return "（未获取到翻译结果）";

                StringBuilder sb = new StringBuilder();
                foreach (JToken item in transResult)
                {
                    string dst = item["dst"]?.ToString();
                    if (!string.IsNullOrEmpty(dst))
                        sb.AppendLine(dst);
                }

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return $"解析结果出错：{ex.Message}";
            }
        }

        #endregion
    }
}
