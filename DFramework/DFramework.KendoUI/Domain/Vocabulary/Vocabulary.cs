using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFramework.Infrastructure;

namespace DFramework.KendoUI.Domain
{
    public class Vocabulary : AggregateRoot
    {
        public string Id { get; set; }
        public VocabularyType Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public string ParentId { get; set; }

        public Vocabulary(VocabularyType type, string code, string name)
        {
            Type = type;
            Code = code;
            Name = name;
        }

        public void Add(string value, string parentId = "")
        {
            Id = ObjectId.GenerateNewId().ToString();
            Value = value;
            ParentId = parentId;
        }
    }

    public enum VocabularyType
    {
        Region = 0,
        Asset = 1
    }
}