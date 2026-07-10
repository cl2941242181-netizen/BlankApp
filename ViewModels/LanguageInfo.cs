namespace BlankApp.ViewModels
{
    /// <summary>
    /// 语言选项（UI 层使用）
    /// </summary>
    public class LanguageInfo
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (obj is LanguageInfo other)
                return Code == other.Code;
            return false;
        }

        public override int GetHashCode() => Code?.GetHashCode() ?? 0;
    }
}
