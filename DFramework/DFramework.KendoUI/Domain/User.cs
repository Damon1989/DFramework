using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DFramework.Infrastructure;

namespace DFramework.KendoUI.Domain
{
    public class User : AggregateRoot
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public CommonStatus Status { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }

        public void Add(string name,
            string departmentId,
            string phone = "",
            string email = "",
            CommonStatus status = CommonStatus.Normal)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = name;
            DepartmentId = departmentId;
            Phone = phone;
            Email = email;
            Status = status;
            IsAdmin = false;
        }

        public void Edit(string name,
            string departmentId,
            string phone,
            string email,
            CommonStatus status = CommonStatus.Normal)
        {
            Name = name;
            DepartmentId = departmentId;
            Phone = phone;
            Email = email;
            Status = status;
            IsAdmin = false;
            ModifiedTime = DateTime.Now;
        }

        public void Delete()
        {
            Status = CommonStatus.Deleted;
            ModifiedTime = DateTime.Now;
        }

        public void SetAdmin()
        {
            IsAdmin = true;
            ModifiedTime = DateTime.Now;
        }
    }
}