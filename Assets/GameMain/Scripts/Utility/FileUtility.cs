//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//
// Revisor: Man Guan
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using IUV.SDN;
using UnityEngine;

namespace IUV
{
    /// <summary>
    /// 路径相关的实用函数。
    /// </summary>
    public static class FileUtility
    {
        public static string StreamingAssetsPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// 获取规范的路径。
        /// </summary>
        /// <param name="path">要规范的路径。</param>
        /// <returns>规范的路径。</returns>
        public static string GetRegularPath(string path)
        {
            if (path == null)
            {
                return null;
            }

            return path.Replace('\\', '/');
        }

        /// <summary>
        /// 获取连接后的路径。
        /// </summary>
        /// <param name="path">路径片段。</param>
        /// <returns>连接后的路径。</returns>
        public static string GetCombinePath(params string[] path)
        {
            if (path == null || path.Length < 1)
            {
                return null;
            }

            string combinePath = path[0];
            for (int i = 1; i < path.Length; i++)
            {
                combinePath = System.IO.Path.Combine(combinePath, path[i]);
            }

            return GetRegularPath(combinePath);
        }

        /// <summary>
        /// 获取远程格式的路径（带有file:// 或 http:// 前缀）。
        /// </summary>
        /// <param name="path">原始路径。</param>
        /// <returns>远程格式路径。</returns>
        public static string GetRemotePath(params string[] path)
        {
            string combinePath = GetCombinePath(path);
            if (combinePath == null)
            {
                return null;
            }

            return combinePath.Contains("://") ? combinePath : ("file:///" + combinePath).Replace("file:////", "file:///");
        }

        /// <summary>
        /// 获取带有后缀的资源名。
        /// </summary>
        /// <param name="resourceName">原始资源名。</param>
        /// <returns>带有后缀的资源名。</returns>
        public static string GetResourceNameWithSuffix(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception("Resource name is invalid.");
            }

            return string.Format("{0}.dat", resourceName);
        }

        /// <summary>
        /// 获取带有 CRC32 和后缀的资源名。
        /// </summary>
        /// <param name="resourceName">原始资源名。</param>
        /// <param name="hashCode">CRC32 哈希值。</param>
        /// <returns>带有 CRC32 和后缀的资源名。</returns>
        public static string GetResourceNameWithCrc32AndSuffix(string resourceName, int hashCode)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new Exception("Resource name is invalid.");
            }

            return string.Format("{0}.{1:x8}.dat", resourceName, hashCode);
        }

        /// <summary>
        /// 移除空文件夹。
        /// </summary>
        /// <param name="directoryName">要处理的文件夹名称。</param>
        /// <returns>是否移除空文件夹成功。</returns>
        public static bool RemoveEmptyDirectory(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
            {
                throw new Exception("Directory name is invalid.");
            }

            try
            {
                if (!Directory.Exists(directoryName))
                {
                    return false;
                }

                // 不使用 SearchOption.AllDirectories，以便于在可能产生异常的环境下删除尽可能多的目录
                string[] subDirectoryNames = Directory.GetDirectories(directoryName, "*");
                int subDirectoryCount = subDirectoryNames.Length;
                foreach (string subDirectoryName in subDirectoryNames)
                {
                    if (RemoveEmptyDirectory(subDirectoryName))
                    {
                        subDirectoryCount--;
                    }
                }

                if (subDirectoryCount > 0)
                {
                    return false;
                }

                if (Directory.GetFiles(directoryName, "*").Length > 0)
                {
                    return false;
                }

                Directory.Delete(directoryName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool RemoveFile(string file)
        {
            if (IsFileExist(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsDirectoryExist(string dir)
        {
            return Directory.Exists(dir);
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool RemoveDirectory(string dir)
        {
            if (IsDirectoryExist(dir))
            {
                Directory.Delete(dir, true);
                return true;
            }
            return false;
        }

        public static bool CreateDirectory(string dir)
        {
            if (!IsDirectoryExist(dir))
            {
                Directory.CreateDirectory(dir);
                return true;
            }
            return false;
        }

        public static string GetString(string fileName)
        {
            if (!IsFileExist(fileName))
            {
                return null;
            }

            return File.ReadAllText(fileName);
        }

        /// <summary>
        /// 可写目录
        /// </summary>
        /// <returns></returns>
        public static string GetWritablePath()
        {
            return Application.persistentDataPath + "/root/";
        }

        public static bool WriteFileWithCode(string filePath, string data, Encoding code)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(filePath);
                CreateDirectory(path);

                if (code != null)
                {
                    File.WriteAllText(filePath, data, code);
                }
                else
                {
                    File.WriteAllText(filePath, data);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("writeFIle fail. " + filePath);
                throw e;
            }
        }

        public static bool WriteString(string filePath, string data)
        {
            return WriteFileWithCode(filePath, data, Encoding.UTF8);
        }

        public static bool WriteBytes(string filePath, byte[] bytes)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(filePath);
                CreateDirectory(path);

                File.WriteAllBytes(filePath, bytes);
                return true;
            }
            catch (IOException e)
            {
                Debug.LogError("writeFIle fail. " + filePath);
                throw e;
            }
        }

        private static void Write(FileStream fs, byte[] data)
        {
            fs.Write(data, 0, data.Length);
        }

        public static bool WriteFileStream(string filePath, List<byte[]> dataes)
        {
            string path = System.IO.Path.GetDirectoryName(filePath);
            CreateDirectory(path);

            using(FileStream fs = new FileStream(path, System.IO.FileMode.Append))
            {
                for (int i = 0; i < dataes.Count; ++i)
                    Write(fs, dataes[i]);
            }
            return true;
        }

        public static bool WriteFileStream(string dir, string filename, List<byte[]> dataes)
        {
            return WriteFileStream(System.IO.Path.Combine(dir, filename), dataes);
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="oldFile">旧文件名</param>
        /// <param name="newFile">新文件名</param>
        /// <returns></returns>
        public static bool RenameFile(string path, string oldFile, string newFile)
        {
            string _old = path + oldFile;
            string _new = path + newFile;
            try
            {
                if (IsFileExist(_old))
                {
                    RemoveFile(_new);
                }
                else
                    return false;
                File.Move(_old, _new);
                return true;
            }
            catch (IOException e)
            {
                Debug.LogError(e.ToString());
            }
            Debug.LogError("can't found " + _old);
            return false;
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static bool UnZip(string zip)
        {
            ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
            string rootPath = System.IO.Path.GetDirectoryName(zip);
            if (!IsFileExist(zip)) return false;
            // 开始解压
            //FastZipEvents events = new FastZipEvents();
            //events.Progress = onProgress;
            FastZip fast = new FastZip();
            fast.ExtractZip(zip, rootPath, "");
            return true;
        }

        public static void CopyFile(string srcPath, string tarPath)
        {
            if (!File.Exists(srcPath))
            {
                return;
            }
            if (File.Exists(tarPath))
            {
                File.Delete(tarPath);
            }

            File.Copy(srcPath, tarPath);
        }

        public static void CopyDirectory(string sourcePath, string destinationPath, bool filter = true)
        {
            sourcePath = sourcePath.Replace("\r", "").Replace("\n", "");
            destinationPath = destinationPath.Replace("\r", "").Replace("\n", "");
            if (!Directory.Exists(sourcePath))
                return;
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                string destName = System.IO.Path.Combine(destinationPath, fsi.Name);
                if (fsi is System.IO.FileInfo)
                {
                    if (filter)
                    {
                        if (fsi.FullName.IndexOf(".manifest") > 0) continue;
                    }
                    FileInfo dinfo = new FileInfo(destName);
                    FileInfo sinfo = (FileInfo) fsi;
                    if (!dinfo.Exists || sinfo.Length != dinfo.Length || sinfo.LastWriteTime != dinfo.LastWriteTime)
                    {
                        File.Copy(fsi.FullName, destName, true);
                    }
                }
                else
                {
                    RemoveDirectory(destName);
                    CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        public static void SaveJsonToFile<T>(T data, string path)
        {
            string jsonStr = Utility.ToJson(data);

            WriteFileWithCode(path, jsonStr, null);
        }

        public static T LoadFromJsonFile<T>(string path) where T : new()
        {
            string str = GetString(path);
            T data = Utility.ToObject<T>(str);
            return data;
        }
    }
}