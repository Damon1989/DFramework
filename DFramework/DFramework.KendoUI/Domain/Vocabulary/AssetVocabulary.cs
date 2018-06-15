using DFramework.JsonNet;

namespace DFramework.KendoUI.Domain
{
    public class AssetVocabulary : Vocabulary
    {
        public int Month { get; set; }

        public AssetVocabulary(string code, string name, int month)
            : base(VocabularyType.Asset, code, name)
        {
            Month = month;
        }

        public string GetValue()
        {
            return new AssetVocabularyValue(Code, Name, Month).ToJson();
        }
    }

    public class AssetVocabularyValue
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Month { get; set; }

        public AssetVocabularyValue(string code, string name, int month)
        {
            Code = code;
            Name = name;
            Month = month;
        }
    }
}