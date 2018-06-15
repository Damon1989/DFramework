using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFramework.JsonNet;

namespace DFramework.KendoUI.Domain
{
    public class RegionVocabulary : Vocabulary
    {
        public CommonStatus Status { get; set; }

        public RegionVocabulary(string code, string name, CommonStatus status)
            : base(VocabularyType.Region, code, name)
        {
            Status = status;
        }

        public string GetValue()
        {
            return new RegionValue(Code, Name, Status).ToJson();
        }
    }

    public class RegionValue
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public CommonStatus Status { get; set; }

        public RegionValue(string code, string name, CommonStatus status)
        {
            Code = code;
            Name = name;
            Status = status;
        }
    }
}