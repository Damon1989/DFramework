using DFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MyMvcTest.Helper
{
    public class SendMailHelper
    {
        public static bool SendMail(Mail mail)
        {
            var returnValue = false;
            try
            {
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(mail.SmtpConfig.FromMail,mail.SmtpConfig.FromName),
                    Subject = mail.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    Priority = MailPriority.High,
                    Body = mail.Body,
                    IsBodyHtml = true,
                };
                if (!string.IsNullOrEmpty(mail.Tomail))
                {
                    var list=mail.Tomail.SplitStringByFenHao<string>();
                    list.ForEach(to =>
                    {
                        if (!to.IsNullOrEmpty())
                        {
                            mailMessage.To.Add(to);
                        }
                    });
                }

                if (!string.IsNullOrEmpty(mail.CCmail))
                {
                    var list = mail.CCmail.SplitStringByFenHao<string>();
                    list.ForEach(cc =>
                    {
                        if (!cc.IsNullOrEmpty())
                        {
                            mailMessage.CC.Add(cc);
                        }
                    });
                }

                if (!mail.FileList.IsNullOrCountZero())
                {
                    mail.FileList.ForEach(item => { mailMessage.Attachments.Add(new Attachment(item)); });
                }

                //qq
                //var smtp = new SmtpClient
                //{
                //    EnableSsl = mail.SmtpConfig.EnableSsl,
                //    UseDefaultCredentials = false,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = mail.SmtpConfig.Server,
                //    Credentials = new NetworkCredential(mail.SmtpConfig.UserName, mail.SmtpConfig.Password),
                //    Timeout = 60000
                //};

                //163
                //var smtp = new SmtpClient
                //{
                //    EnableSsl = mail.SmtpConfig.EnableSsl,
                //    UseDefaultCredentials = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = mail.SmtpConfig.Server,
                //    Credentials = new NetworkCredential(mail.SmtpConfig.UserName, mail.SmtpConfig.Password),
                //    Timeout = 60000
                //};

                var smtp = new SmtpClient
                {
                    EnableSsl = mail.SmtpConfig.EnableSsl,
                    UseDefaultCredentials = mail.SmtpConfig.UseDefaultCredentials,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = mail.SmtpConfig.Server,
                    Credentials = new NetworkCredential(mail.SmtpConfig.UserName, mail.SmtpConfig.Password),
                    Timeout = 60000
                };


                smtp.Send(mailMessage);

                returnValue = true;
            }
            catch (Exception e)
            {
                returnValue = false;
                throw;
            }

            return returnValue;
        }

        public static bool SendMail(List<Mail> mailList)
        {
            try
            {
                foreach (var item in mailList)
                {
                    SendMail(item);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
            
        }
    }

    public class Mail
    {
        public string Tomail { get; set; }
        public string CCmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> FileList { get; set; }
        public SmtpConfig SmtpConfig { get; set; }

        public Mail(string subject, string body, string tomail)
            :this(subject, body, tomail,"")
        {

        }

        public Mail(string subject, string body, string tomail, SmtpConfig smtpConfig)
            : this(subject, body, tomail, "", smtpConfig)
        {

        }

        public Mail(string subject, string body, string tomail,IEnumerable<string> fileList)
            : this(subject, body, tomail, "",fileList)
        {

        }

        public Mail(string subject, string body, string tomail, IEnumerable<string> fileList, SmtpConfig smtpConfig)
            : this(subject, body, tomail, "", fileList,smtpConfig)
        {

        }

        public Mail(string subject, string body, string tomail, string ccmail)
           :this(subject,body,tomail,ccmail,null,null)
        {

        }

        public Mail(string subject, string body, string tomail, string ccmail, SmtpConfig smtpConfig)
            : this(subject, body, tomail, ccmail, null, smtpConfig)
        {

        }

        public Mail(string subject, string body, string tomail, string ccmail, IEnumerable<string> fileList)
            : this(subject, body, tomail, ccmail, fileList, null)
        {

        }

        public Mail(string subject, string body, string tomail, string ccmail,IEnumerable<string>fileList, SmtpConfig smtpConfig)
        {
            Subject = subject;
            Body = body;
            Tomail = tomail;
            CCmail = ccmail;
            FileList = fileList;
            if (smtpConfig!=null)
            {
                SmtpConfig = smtpConfig;
            }
            else
            {
                var config = new SmtpConfigHelper();
                SmtpConfig = new SmtpConfig(config.FromMail, config.FromName, config.Server, config.UserName,
                    config.Password, config.EnableSsl,config.UseDefaultCredentials);
            }
        }
    }

    public class SmtpConfig
    {
        public string FromMail { get; set; }
        public string FromName { get; set; }

        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }

        public SmtpConfig(string fromMail
                                        , string fromName
                                        ,string server
                                        ,string userName
                                        ,string password
                                        ,bool enableSsl
                                        ,bool useDefaultCredentials)
        {
            FromMail = fromMail;
            FromName = fromName;
            Server = server;
            UserName = userName;
            Password = password;
            EnableSsl = enableSsl;
            UseDefaultCredentials = useDefaultCredentials;
        }

        
    }

    public class SmtpConfigHelper
    {
        private NameValueCollection smtpConfigSettings = null;

        public SmtpConfigHelper()
        {
            smtpConfigSettings=ConfigurationManager.GetSection("smtpconfiggroup/smtpconfig") as NameValueCollection;
        }

        private bool ContainSection()
        {
            return smtpConfigSettings != null;
        }

        public bool ContainKey(string key)
        {
            if (ContainSection())
            {
                return smtpConfigSettings[key] != null;
            }

            return false;
        }



        //<smtpconfiggroup>
        //<smtpconfig>
        //<!--<add key = "username" value="1015080921@qq.com" />
        //<add key = "password" value="ckwzioxxrpezbbfg" />
        //<add key = "frommail" value="1015080921@qq.com" />
        //<add key = "fromname" value="damon" />
        //<add key = "server" value="smtp.exmail.qq.com" />
        //<add key = "enablessl" value="true"/>-->
        //<!--<add key = "username" value="amerservice3@yodoo.net.cn" />
        //<add key = "password" value="Youdu2019" />
        //<add key = "frommail" value="amerservice3@yodoo.net.cn" />
        //<add key = "fromname" value="亚玛芬费控" />
        //<add key = "server" value="smtp.exmail.qq.com" />
        //<add key = "enablessl" value="true"/>
        //<add key = "usedefaultcredentials" value="false"/>-->
        //<add key = "username" value="15201864775@163.com" />
        //<add key = "password" value="P@ssw0rd112389" />
        //<add key = "frommail" value="15201864775@163.com" />
        //<add key = "fromname" value="damon" />
        //<add key = "server" value="smtp.163.com" />
        //<add key = "enablessl" value="true" />
        //<add key = "usedefaultcredentials" value="true" />
        //</smtpconfig>
        //</smtpconfiggroup>

        public string GetValue(string key)
        {
            return ContainKey(key)
                ? smtpConfigSettings[key]
                : string.Empty;
        }

        public string UserName => GetValue("username");
        public string Password => GetValue("password");
        public string FromMail => GetValue("frommail");
        public string FromName => GetValue("fromname");
        public string Server => GetValue("server");
        public bool EnableSsl => GetValue("enablessl")=="true";
        public bool UseDefaultCredentials => GetValue("usedefaultcredentials") == "true";
    }

}