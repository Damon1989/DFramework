using System;
using DFramework.Infrastructure;

namespace DFramework.KendoUI.Domain
{
    public class Department : AggregateRoot
    {
        public string Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public CommonStatus Status { get; private set; }

        public void Add(string code, string name, string parentId)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Code = code;
            Name = name;
            ParentId = parentId;
            Status = CommonStatus.Normal;
        }

        public void Edit(string code, string name)
        {
            Code = code;
            Name = name;
            ModifiedTime = DateTime.Now;
        }

        public void Enabled()
        {
            ModifiedTime = DateTime.Now;
            Status = CommonStatus.Normal;
        }

        public void Disabled()
        {
            ModifiedTime = DateTime.Now;
            Status = CommonStatus.Disabled;
        }

        public void Delete()
        {
            ModifiedTime = DateTime.Now;
            Status = CommonStatus.Deleted;
        }
    }
}