using System;
using System.Configuration;
using System.IO;

namespace MyMvcTest.Helper
{
    public static class LoggerHelper
    {
        public static void WriteLine(string msg)
        {
            WriteLine(msg, true);
        }

        public static void WriteLine(string msg,bool recordIp)
        {
            WriteLine(msg, recordIp,"C");
        }

        public static void WriteLine(string msg,string disk)
        {
            WriteLine(msg, true,disk);
        }

        public static void WriteLine(string msg,bool recordIp,string disk ="C")
        {
            try
            {
                if (ConfigurationManager.AppSettings["WriteLog"]
                    .Equals("False", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }
            catch (Exception e)
            {
                
            }
            var now = DateTime.Now;
            var directory = $"{disk}://log//{now.Year}//{now.Month}//{now.Day}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileCount = Directory.GetFiles(directory).Length;
            var filePath =fileCount==0? $"{directory}//{fileCount}.txt": $"{directory}//{fileCount-1}.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length/1024/1024>5)
                {
                    File.Create(filePath.Replace($"{Path.GetFileName(filePath)}",$"{fileCount}.txt")).Close();
                    WriteLine(msg, recordIp,disk);
                }
            }

            var ip = recordIp ? GetHostAddress() : "";
            using (var fileStream=new FileStream(filePath,FileMode.Append))
            {
                using (var writer=new StreamWriter(fileStream))
                {
                    writer.WriteLine($"{now}--{ip}--{msg}");
                    writer.WriteLine();
                }
            }
        }

        private static string GetHostAddress()
        {
            try
            {
                var userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress))
                {
                    return userHostAddress;
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private static bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}