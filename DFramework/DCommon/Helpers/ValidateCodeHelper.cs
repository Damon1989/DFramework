﻿namespace DCommon
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    ///     /  var code = ValidateCodeHelper.GenerateValidateCode(5);
    ///     Session["ValidateCode"] = code;
    ///     var bytes = ValidateCodeHelper.GenerateValidateGraphic(code);
    ///     return File(bytes, @"image/jpeg");
    /// </summary>
    public static class ValidateCodeHelper
    {
        public static string GenerateValidateCode(int length = 4)
        {
            return new ValidateCode().CreateValidateCode(length);
        }

        public static byte[] GenerateValidateGraphic(string validateCode)
        {
            return new ValidateCode().CreateValidateGraphic(validateCode);
        }
    }

    public class ValidateCode
    {
        public int MaxLength => 10;

        public int MinLength => 1;

        public static double GetImageHeight()
        {
            return 22.5;
        }

        // 得到验证码图片的长度
        public static int GetImageWidth(int validateNumLength)
        {
            return (int)(validateNumLength * 12.0);
        }

        public string CreateValidateCode(int length)
        {
            var randMembers = new int[length];
            var validateNums = new int[length];
            var validateNumberStr = string.Empty;

            // 生成起始序列值
            var seekSeek = unchecked((int)DateTime.Now.Ticks);
            var seekRand = new Random(seekSeek);
            var beginSeek = seekRand.Next(0, int.MaxValue - length * 1000);
            var seeks = new int[length];
            for (var i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }

            // 生成随机数
            for (var i = 0; i < length; i++)
            {
                var rand = new Random(seeks[i]);
                var pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, int.MaxValue);
            }

            // 抽取随机数字
            for (var i = 0; i < length; i++)
            {
                var numStr = randMembers[i].ToString();
                var numLength = numStr.Length;
                var rand = new Random();
                var numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = int.Parse(numStr.Substring(numPosition, 1));
            }

            // 生成验证码
            for (var i = 0; i < length; i++) validateNumberStr += validateNums[i].ToString();

            return validateNumberStr;
        }

        public byte[] CreateValidateGraphic(string validateCode)
        {
            var image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            var g = Graphics.FromImage(image);
            try
            {
                // 生成随机生成器
                var random = new Random();

                // 清空图片背景色
                g.Clear(Color.White);

                // 画图片的干扰线
                for (var i = 0; i < 25; i++)
                {
                    var x1 = random.Next(image.Width);
                    var x2 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
                var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue,
                    Color.DarkRed,
                    1.2f,
                    true);
                g.DrawString(validateCode, font, brush, 3, 2);

                // 画图片的前景干扰点
                for (var i = 0; i < 100; i++)
                {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                // 画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                // 保存图片数据
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                // 输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}