using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace MyMvcTest.Helper
{
    public static class ZipHelper
    {
        /// <summary>
        /// 递归压缩文件夹的内部方法
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipStream"></param>
        /// <param name="parentFolderName">相比一级目录的目录节点树</param>
        /// <param name="folderToZipDirectory"></param>
        /// <returns></returns>
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName, string folderToZipDirectory)
        {
            var result = true;
            ZipEntry zipEntry = null;
            FileStream fileStream = null;
            var crc = new Crc32();
            try
            {
                //var filePath = Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/");
                //zipEntry = new ZipEntry(filePath);
                //zipStream.PutNextEntry(zipEntry);
                //zipStream.Flush();

                //var files = Directory.GetFiles(folderToZip);
                //foreach (var file in files)
                //{
                //    fileStream = File.OpenRead(file);
                //    var buffer = new byte[fileStream.Length];
                //    fileStream.Read(buffer, 0, buffer.Length);
                //    filePath = Path.Combine(parentFolderName,
                //        Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file));
                //    zipEntry = new ZipEntry(filePath)
                //    {
                //        DateTime = DateTime.Now,
                //        Size = fileStream.Length
                //    };
                //    fileStream.Close();
                //    crc.Reset();
                //    crc.Update(buffer);
                //    zipEntry.Crc = crc.Value;
                //    zipStream.PutNextEntry(zipEntry);
                //    zipStream.Write(buffer, 0, buffer.Length);
                //}
                var files = Directory.GetFiles(folderToZip);
                foreach (var file in files)
                {
                    using (FileStream fs = File.OpenRead(file))
                    {
                        byte[] buffer = new byte[4 * 1024];
                        var filePath = Path.Combine(parentFolderName,
                                    Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file));
                        ZipEntry entry = new ZipEntry(filePath);     //此处去掉盘符，如D:\123\1.txt 去掉D:
                        entry.DateTime = DateTime.Now;
                        zipStream.PutNextEntry(entry);

                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }

                }
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }

                if (zipEntry != null)
                {
                }

                GC.Collect();
                GC.Collect(1);
            }

            var folders = Directory.GetDirectories(folderToZip);
            foreach (var folder in folders)
            {
                if (!ZipDirectory(folder, zipStream,
                    Path.GetDirectoryName(folder)?.Replace(Path.GetDirectoryName(folderToZipDirectory) ?? "", ""), folderToZipDirectory))
                {
                    return false;
                }
            }

            return result;
        }

        # region 压缩文件夹

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="folderToZip">要压缩的文件夹路径</param>
        /// <param name="zipedFile">压缩文件完整路径</param>
        /// <param name="password">密码</param>
        /// <param name="level">压缩级别</param>
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password = "", int level = 6)
        {
            var result = false;
            if (!Directory.Exists(folderToZip))
            {
                return false;
            }

            var zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(level);
            if (!string.IsNullOrWhiteSpace(password))
            {
                zipStream.Password = password;
            }

            result = ZipDirectory(folderToZip, zipStream, "", folderToZip);
            zipStream.Finish();
            zipStream.Close();

            return result;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string folderToZip, string zipedFile, int level)
        {
            return ZipDirectory(folderToZip, zipedFile, "", level);
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password)
        {
            return ZipDirectory(folderToZip, zipedFile, password, 6);
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <returns></returns>
        public static bool ZipDirectory(string folderToZip, string zipedFile)
        {
            return ZipDirectory(folderToZip, zipedFile, "", 6);
        }

        #endregion

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件全名</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="password">密码</param>
        /// <param name="level">压缩级别</param>
        /// <returns></returns>
        public static bool ZipFile(string fileToZip, string zipedFile, string password = "", int level = 6)
        {
            var result = true;
            ZipOutputStream zipStream = null;
            FileStream fileStream = null;
            ZipEntry zipEntry = null;

            var sss = File.Exists(fileToZip);

            if (!File.Exists(fileToZip))
            {
                return false;
            }

            try
            {
                fileStream = File.OpenRead(fileToZip);
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();

                fileStream = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fileStream);
                if (!string.IsNullOrWhiteSpace(password))
                {
                    zipStream.Password = password;
                }

                zipEntry = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(level);

                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipEntry != null)
                {
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            GC.Collect();
            GC.Collect(1);

            return result;
        }

        public static bool ZipFile(string fileToZip, string zipedFile, string password = "")
        {
            return ZipFile(fileToZip, zipedFile, password, 6);
        }

        public static bool ZipFile(string fileToZip, string zipedFile, int level = 6)
        {
            return ZipFile(fileToZip, zipedFile, "", level);
        }

        public static bool ZipFile(string fileToZip, string zipedFile)
        {
            return ZipFile(fileToZip, zipedFile, "", 6);
        }

        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="fileToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="password"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool Zip(string fileToZip, string zipedFile, string password = "", int level = 6)
        {
            if (Directory.Exists(fileToZip))
            {
                return ZipDirectory(fileToZip, zipedFile, password, level);
            }

            if (File.Exists(fileToZip))
            {
                return ZipFile(fileToZip, zipedFile, password, level);
            }

            return false;
        }

        public static bool Zip(string fileToZip, string zipedFile, string password = "")
        {
            return Zip(fileToZip, zipedFile, password, 6);
        }

        public static bool Zip(string fileToZip, string zipedFile, int level = 6)
        {
            return Zip(fileToZip, zipedFile, "", level);
        }

        public static bool Zip(string fileToZip, string zipedFile)
        {
            return Zip(fileToZip, zipedFile, "", 6);
        }

        /// <summary>
        /// 解压功能（解压压缩文件到指定目录）
        /// </summary>
        /// <param name="fileToUnZip">待解压的文件</param>
        /// <param name="targetFolder">指定解压目标目录</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool UnZip(string fileToUnZip, string targetFolder, string password)
        {
            var result = true;
            FileStream fileStream = null;
            ZipInputStream zipStream = null;
            ZipEntry zipEntry = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
            {
                return false;
            }

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            try
            {
                zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

                while ((zipEntry = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(zipEntry.Name))
                    {
                        fileName = Path.Combine(targetFolder, zipEntry.Name);
                        fileName = fileName.Replace('/', '\\');

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        fileStream = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                //fileStream.Write(data, 0, data.Length);
                            {
                                fileStream.Write(data, 0, size < data.Length ? size : data.Length);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (zipEntry != null)
                {
                }
                GC.Collect();
                GC.Collect(1);
            }

            return result;
        }

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="fileToUnZip">待解压的文件</param>
        /// <param name="targetFolder">指定解压目标目录</param>
        /// <returns>解压结果</returns>
        public static bool UnZip(string fileToUnZip, string targetFolder)
        {
            return UnZip(fileToUnZip, targetFolder, "");
        }
    }
}