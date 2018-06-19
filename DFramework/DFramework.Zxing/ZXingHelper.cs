using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace DFramework.Zxing
{
    public class ZXingHelper
    {
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="format"></param>
        /// <param name="filePath"></param>
        public static void GenerateBarCode(string content,
                                           int height = 130,
                                           int width = 240,
                                           BarcodeFormat format = BarcodeFormat.EAN_13,
                                           string filePath = null)
        {
            //设置条形码规格
            var encodeOption = new EncodingOptions
            {
                Height = height,
                Width = width
            };

            var writer = new BarcodeWriter
            {
                Options = encodeOption,
                Format = format
            };
            var img = writer.Write(content);

            filePath = filePath ?? GetFilePath("BarCode", content, format);

            img.Save(filePath, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin"></param>
        /// <param name="format"></param>
        /// <param name="logoFilePath">logo图片地址</param>
        public static void GenerateQrCode(string content,
                                          int width = 200,
                                          int height = 200,
                                          int margin = 1,
                                          BarcodeFormat format = BarcodeFormat.QR_CODE,
                                          string logoFilePath = null)
        {
            var img = GenerateBitmap(content, width, height, margin, format);

            var filePath = GetFilePath("QRCode", content, format);

            SetLogoImgage(img, logoFilePath);

            img.Save(filePath, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin"></param>
        /// <param name="format"></param>
        /// <param name="logoFilStream"></param>
        public static void GenerateQrCode(string content,
                                          int width = 200,
                                          int height = 200,
                                          int margin = 1,
                                          BarcodeFormat format = BarcodeFormat.QR_CODE,
                                          Stream logoFilStream = null)
        {
            var img = GenerateBitmap(content, width, height, margin, format);

            var filePath = GetFilePath("QRCode", content, format);

            SetLogoImgage(img, logoFilStream);

            img.Save(filePath, ImageFormat.Jpeg);
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private static Bitmap GenerateBitmap(string content, int width, int height, int margin, BarcodeFormat format)
        {
            //设置QR二维码的规格
            var qrCodeEncodingOptions = new QrCodeEncodingOptions
            {
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                Margin = margin
            };

            // 生成条形码图片
            var writer = new BarcodeWriter
            {
                Format = format,
                Options = qrCodeEncodingOptions
            };
            return writer.Write(content);
        }

        /// <summary>
        /// 设置logo
        /// </summary>
        /// <param name="img"></param>
        /// <param name="logoFilePath"></param>
        /// <returns></returns>
        private static Bitmap SetLogoImgage(Bitmap img, string logoFilePath)
        {
            if (logoFilePath == null) return img;

            using (var stream = new FileStream(logoFilePath, FileMode.Open))
            {
                return SetLogoImgage(img, stream);
            }
        }

        /// <summary>
        ///  设置logo
        /// </summary>
        /// <param name="img"></param>
        /// <param name="logoFileStream"></param>
        /// <returns></returns>
        private static Bitmap SetLogoImgage(Bitmap img, Stream logoFileStream)
        {
            if (logoFileStream != null)
            {
                var logoImg = Image.FromStream(logoFileStream);
                var g = Graphics.FromImage(img);
                var logoRec = new Rectangle     //设置logo图片的大小和绘制位置
                {
                    Width = img.Width / 6,
                    Height = img.Width / 6,
                };
                logoRec.X = img.Width / 2 - logoRec.Width / 2;
                logoRec.Y = img.Height / 2 - logoRec.Height / 2;
                g.DrawImage(logoImg, logoRec);
            }

            return img;
        }

        /// <summary>
        /// 读取编码内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static string ReadCode(string filePath,
            params BarcodeFormat[] formats)
        {
            var decodeOptions = new DecodingOptions
            {
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.EAN_13
                }
            };

            formats?.ToList().ForEach(item =>
            {
                decodeOptions.PossibleFormats.Add(item);
            });

            var reader = new BarcodeReader
            {
                Options = decodeOptions
            };

            if (!File.Exists(filePath)) return string.Empty;

            var result = reader.Decode(new Bitmap(filePath));
            return result != null ? result.Text : string.Empty;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="fileCategory"></param>
        /// <param name="content"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string GetFilePath(string fileCategory, string content, BarcodeFormat format)
        {
            var now = DateTime.Now;
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}Zxing\\{fileCategory}\\{now.Year}\\{now.Month}\\{format.ToString()}";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath += $"\\{content}.jpg";
            return filePath;
        }
    }
}