using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyMvcTest.Helper
{
    /// <summary>
    /// 描述信息  行号,列号!占用列数，字段名称，字段显示名称，字典显示名称，字典选项Key!单元格高度，单元格宽度
    /// </summary>
    public class ChildExportModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Description("0,0!6,Title!8,40")]
        public string Title { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Description("1,0!2,Name,姓名!4,40")]
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 姓名拼音
        /// </summary>
        public string Xmpy { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Mz { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        public string Gj { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string Jg { get; set; }

        /// <summary>
        /// 出生地
        /// </summary>
        public string Csd { get; set; }

        /// <summary>
        /// 入园日期
        /// </summary>
        public string Ryrq { get; set; }

        /// <summary>
        /// 曾用名/英文名
        /// </summary>
        public string Cym { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public string Xx { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// 就读方式
        /// </summary>
        public string Jdfs { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        public string Jkzk { get; set; }

        /// <summary>
        /// 港澳台侨
        /// </summary>
        public string Gatq { get; set; }

        /// <summary>
        /// 是否寄宿生
        /// </summary>
        public string Jss { get; set; }

        /// <summary>
        /// 是否优抚子女
        /// </summary>
        public string Yfzn { get; set; }

        /// <summary>
        /// 是否军烈属子女
        /// </summary>
        public string Jlszn { get; set; }
        /// <summary>
        /// 是否部队子女
        /// </summary>
        public string Bdzn { get; set; }

        /// <summary>
        /// 是否孤儿
        /// </summary>
        public string Ge { get; set; }

        /// <summary>
        /// 是否独生子女
        /// </summary>
        public string Dszn { get; set; }

        /// <summary>
        /// 是否低保
        /// </summary>
        public string Db { get; set; }

        /// <summary>
        /// 是否农民工子女
        /// </summary>
        public string Nmgzn { get; set; }

        /// <summary>
        /// 进城务工人员随迁子女
        /// </summary>
        public string Sqzn { get; set; }
        /// <summary>
        /// 是否残疾
        /// </summary>
        public string Cjye { get; set; }
        /// <summary>
        /// 残疾类别
        /// </summary>
        public string Cjlb { get; set; }

        /// <summary>
        /// 留守儿童
        /// </summary>
        public string Lset { get; set; }

        /// <summary>
        /// 幼儿特长
        /// </summary>
        public string Tc { get; set; }

        /// <summary>
        /// 特异体质
        /// </summary>
        public string Tz { get; set; }

        /// <summary>
        /// 户口性质
        /// </summary>
        public string Hkxz { get; set; }

        /// <summary>
        /// 非农业户口类型
        /// </summary>
        public string Fnyhklx { get; set; }
        /// <summary>
        /// 户籍类别
        /// </summary>
        public string Hjlb { get; set; }

        /// <summary>
        /// 户口地址
        /// </summary>
        public string HkAddress { get; set; }
        /// <summary>
        /// 现住址
        /// </summary>
        public string JzAddress { get; set; }
        /// <summary>
        /// 居住地邮编
        /// </summary>
        public string Jzyb { get; set; }

        /// <summary>
        /// 人户一致
        /// </summary>
        public string Rhyz { get; set; }

        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string GuarderName { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string GuarderIdNo { get; set; }

        /// <summary>
        /// 监护人电话
        /// </summary>
        public string GuarderPhone { get; set; }
        /// <summary>
        /// 监护人手机
        /// </summary>
        public string GuarderMobilePhone { get; set; }

        /// <summary>
        /// 监护人证件类型
        /// </summary>
        public string GuarderIdType { get; set; }

        /// <summary>
        /// 监护人关系
        /// </summary>
        public string GuarderRelation { get; set; }

        /// <summary>
        /// 签字
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }


       
    }

    /// <summary>
    /// 描述信息  行号,列号!占用列数，字段名称，字段显示名称，字典显示名称，字典选项Key!单元格高度，单元格宽度
    /// </summary>
    public class TravelApplyPdfModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Description("0,0!6,Title!15,40")]
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Description("1,0!4,SubTitle!4,40")]
        public string SubTitle { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        [Description("1,4!2,Number!4,40")]
        public string Number { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Description("2,0!3,Applicant,申请人!4,40")]
        public string Applicant { get; set; }
        /// <summary>
        /// 申请部门
        /// </summary>
        [Description("2,3!3,ApplyDept,申请部门!4,40")]
        public string  ApplyDept { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        [Description("3,0!3,Receiver,收款人!4,40")]
        public string Receiver { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        [Description("3,3!3,PaymentAccount,收款账号!4,40")]
        public string PaymentAccount { get; set; }
        /// <summary>
        /// 事由
        /// </summary>
        [Description("4,0!6,Remark!4,40")]
        public string Remark { get; set; }
        /// <summary>
        /// 事由内容
        /// </summary>
        [Description("5,0!6,RemarkInfo!4,40")]
        public string RemarkInfo { get; set; }

        /// <summary>
        /// 入账信息
        /// </summary>
        [Description("6,0!6,AccountingInfo!4,40")]
        public string AccountingInfo { get; set; }
        /// <summary>
        /// 入账部门
        /// </summary>
        [Description("7,0!6,AccountingDept,入账部门!4,40")]
        public string AccountingDept { get; set; }
        /// <summary>
        /// 入账项目
        /// </summary>
        [Description("8,0!6,AccountingProject,入账项目!4,40")]
        public string AccountingProject { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [Description("9,0!6,BusinessType,业务类型!4,40")]
        public string BusinessType { get; set; }

        /// <summary>
        /// 出差路线
        /// </summary>
        [Description("10,0!6,TravelRouteList,出差路线,list!4,40")]
        public List<TravelRoute> TravelRouteList { get; set; }

        /// <summary>
        /// 出差申请费用预算
        /// </summary>
        [Description("11,0!6,CostBudgetList,出差申请费用预算,list!4,40")]
        public List<CostBudget> CostBudgetList { get; set; }
        /// <summary>
        /// 审批信息
        /// </summary>
        [Description("12,0!6,ApproveInfoList,审批信息,list!4,40")]
        public List<ApproveInfo> ApproveInfoList { get; set; }
        ///// <summary>
        ///// 签字确认
        ///// </summary>
        //[Description("12,0!6,SignConfirm,!4,40")]
        //public string SignConfirm { get; set; }

        ///// <summary>
        ///// 提交人
        ///// </summary>
        //[Description("13,0!3,Submitter,!4,40")]
        //public string Submitter { get; set; }
        ///// <summary>
        ///// 提交日期
        ///// </summary>
        //[Description("13,3!3,SubmitDate,!4,40")]
        //public string SubmitDate { get; set; }

        ///// <summary>
        ///// 币种
        ///// </summary>
        //[Description("6,0!6,Currency,币种!4,40")]
        //public string Currency { get; set; }
    }

    public class TravelRoute
    {
        [Description("0,1!StartTime,出发时间!4,40")]
        public string StartTime { get; set; }
        [Description("1,1!StartPlace,出发地点!4,40")]
        public string StartPlace { get; set; }
        [Description("2,1!EndTime,结束时间!4,40")]
        public string EndTime { get; set; }
        [Description("3,1!TravelCity,出差城市!4,40")]
        public string TravelCity { get; set; }
        [Description("4,1!TravelWay,出行方式!4,40")]
        public string TravelWay { get; set; }
        [Description("5,1!Accommodation,住宿!4,40")]
        public string Accommodation { get; set; }
    }

    public class CostBudget
    {
        /// <summary>
        /// 币种
        /// </summary>
        [Description("0,2!Currency,币种!4,40")]
        public string Currency { get; set; }
        /// <summary>
        /// 费用项目
        /// </summary>
        [Description("2,2!CostProject,费用项目!4,40")]
        public string CostProject { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        [Description("4,2!ApplyAmount,申请金额!4,40")]
        public string ApplyAmount { get; set; }
    }


    public class ApproveInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        [Description("0,1!Category,类型!6,40")]
        public string Category { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [Description("1,1!Approver,审批人!6,40")]
        public string Approver { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        [Description("2,1!ArriveTime,到达时间!6,40")]
        public string ArriveTime { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        [Description("3,1!ApproveTime,审批时间!6,40")]
        public string ApproveTime { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        [Description("4,2!ApproveOpinion,审批意见!6,40")]
        public string ApproveOpinion { get; set; }
    }
}