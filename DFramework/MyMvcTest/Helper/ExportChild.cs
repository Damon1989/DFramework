using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyMvcTest.Helper
{
    /// <summary>
    /// 枚举描述信息  行号,列号!占用列数，字段名称，字段显示名称，字典显示名称，字典选项Key!单元格高度，单元格宽度
    /// </summary>
    public enum ExportChild
    {
        [Description("0,0!6,Title!8,40")]
        Title = 0,

        [Description("1,0!2,Name,姓名!3,40")]
        Name = 1,

        [Description("1,2!2,Gender,性别!3,40")]
        Gender = 2,

        [Description("1,4!2,Birthday,出生日期!3,40")]
        Birthday = 3,

        [Description("2,0!2,Xmpy,姓名拼音!3,40")]
        Xmpy = 4,

        [Description("2,2!2,Mz,民族!3,40")]
        Mz = 5,

        [Description("2,4!2,Gj,国籍!3,40")]
        Gj = 6,

        [Description("3,0!2,Jg,籍贯!3,40")]
        Jg = 7,

        [Description("3,2!2,Csd,出生地!3,40")]
        Csd = 8,

        [Description("3,4!2,Ryrq,入园日期!3,40")]
        Ryrq = 9,

        [Description("4,0!2,Cym,曾用名/英文名!3,40")]
        Cym = 10,

        [Description("4,2!4,Xx,血型!3,40")]
        Xx = 11,

        [Description("5,0!6,IdNo,证件号码!3,40")]
        IdNo = 12,

        [Description("6,0!6,IdType,证件类型,可选内容,DicChildIdTypeStr!9,40")]
        IdType = 13,

        [Description("7,0!6,Jdfs,就读方式,可选内容,DicJdfsStr!3,40")]
        Jdfs = 14,

        [Description("8,0!6,Jkzk,健康状况,可选内容,DicJkzkStr!3,40")]
        Jkzk = 15,

        [Description("9,0!6,Gatq,港澳台侨外,可选内容,DicGatqStr!11,40")]
        Gatq = 16,

        [Description("10,0!2,Jss,是否寄宿生!3,40")]
        Jss = 17,

        [Description("10,2!2,Yfzn,是否优抚子女!3,40")]
        Yfzn = 18,

        [Description("10,4!2,Jlszn,是否军烈子女!3,40")]
        Jlszn = 19,

        [Description("11,0!2,Bdzn,是否部队子女!3,40")]
        Bdzn = 20,

        [Description("11,2!2,Ge,是否孤儿!3,40")]
        Ge = 21,

        [Description("11,4!2,Sqzn,进城务工人员随迁子女!3,40")]
        Sqzn = 22,

        [Description("12,0!2,Db,是否低保!3,40")]
        Db = 23,

        [Description("12,2!2,Dszn,是否独生子女!3,40")]
        Dszn = 24,

        [Description("12,4!2,Cjye,是否残疾!3,40")]
        Cjye = 25,

        [Description("13,0!6,Cjlb,残疾幼儿类别,可选内容,DicCjlbStr!10,40")]
        Cjlb = 26,

        [Description("14,0!6,Lset,留守儿童,可选内容,DicLsetStr!3,40")]
        Lset = 27,

        [Description("15,2!6,Tc,幼儿特长!3,40")]
        Tc = 28,

        [Description("16,2!6,Tz,特异体质!3,40")]
        Tz = 29,

        [Description("17,0!6,Hkxz,户口性质,可选内容,DicHkxzStr!6,40")]
        Hkxz = 30,

        [Description("18,0!6,Fnyhklx,非农业户口类型,可选内容,DicFnyhklxStr!3,40")]
        Fnyhklx = 31,

        [Description("19,0!6,Hjlb,户籍类别,可选内容,DicHjlbStr!6,40")]
        Hjlb = 32,

        [Description("20,0!6,HkAddress,户口地址!3,40")]
        HkAddress = 33,

        [Description("21,0!6,JzAddress,现住址!3,40")]
        JzAddress = 34,

        [Description("22,0!2,Jzyb,居住地邮编!3,40")]
        Jzyb = 35,

        [Description("22,2!4,Rhyz,人户一致!3,40")]
        Rhyz = 36,

        [Description("23,0!2,GuarderName,监护人姓名!3,40")]
        GuarderName = 37,

        [Description("23,2!4,GuarderIdNo,证件号码!3,40")]
        GuarderIdNo = 38,

        [Description("24,0!6,GuarderIdType,监护人证件类型,可选内容,DicIdTypeStr!9,40")]
        GuarderIdType = 39,

        [Description("25,0!2,GuarderPhone,监护人电话!3,40")]
        GuarderPhone = 40,

        [Description("25,2!4,GuarderMobilePhone,监护人手机!3,40")]
        GuarderMobilePhone = 41,

        [Description("26,0!6,GuarderRelation,监护人关系,可选内容,DicRelationStr!6,40")]
        GuarderRelation = 42,

        [Description("27,0!6,Sign!9,40")]
        Sign = 43,

        [Description("28,0!6,Explain!4,40")]
        Explain = 44,
    }
}