using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DFramework.Domain;
using DFramework.Infrastructure;

namespace DFramework.KendoUI.Domain
{
    public class Asset : AggregateRoot
    {
        public string Id { get; set; }

        /// <summary>
        /// 资产条码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资产类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// SN号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Money { get; set; }

        public AssetDepartment UseDepartment { get; set; }

        /// <summary>
        /// 购入时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public string Admin { get; set; }

        public string Region { get; set; }

        public AssetDepartment BelongedToDepartment { get; set; }

        public int Month { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Asset()
        {
            UseDepartment = new AssetDepartment();
            BelongedToDepartment = new AssetDepartment();
        }
    }

    public class AssetDepartment
    {
        public string Id { get; set; }
        public string DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }

        public string SubDeptId { get; set; }

        [ForeignKey("SubDeptId")]
        public Department SubDepartment { get; set; }

        public AssetDepartment()
        {
        }

        public AssetDepartment(string deptId, string subDeptId)
        {
            Id = ObjectId.GenerateNewId().ToString();
            DeptId = deptId;
            SubDeptId = subDeptId;
        }
    }
}