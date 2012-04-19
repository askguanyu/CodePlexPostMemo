//-----------------------------------------------------------------------
// <copyright file="IsolatedStorageUtility.cs" company="GY Corporation">
//     Copyright (c) GY Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GY.WP.ToolKit.Utility
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Runtime.Serialization;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml;
    using GY.WP.ToolKit.ExtensionMethods;
    using Microsoft.Xna.Framework.Media;

    /// <summary>
    ///
    /// </summary>
    public static class IsolatedStorageUtility
    {
        /// <summary>
        ///Gets or sets
        /// </summary>
        public static IsolatedStorageFile IsolatedFile
        {
            get
            {
                return IsolatedStorageFile.GetUserStoreForApplication();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool SaveFile(string fileFullPath, byte[] bytes)
        {
            if ((bytes == null) || string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(Path.GetDirectoryName(fileFullPath)))
            {
                IsolatedFile.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.CreateFile(fileFullPath);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="bytes"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static bool SaveAudioFile(string fileFullPath, byte[] bytes, int rate)
        {
            if ((bytes == null) || string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(Path.GetDirectoryName(fileFullPath)))
            {
                IsolatedFile.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            }

            IsolatedStorageFileStream stream = null;
            try
            {
                stream = IsolatedFile.CreateFile(fileFullPath);
                WaveHeaderWriter.WriteHeader(stream, bytes.Length, rate);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SaveXmlFile(string fileFullPath, object obj)
        {
            if ((obj == null) || string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(Path.GetDirectoryName(fileFullPath)))
            {
                IsolatedFile.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.CreateFile(fileFullPath);
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (XmlWriter xmlWriter = XmlWriter.Create(stream))
                {
                    serializer.WriteObject(xmlWriter, obj);
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="obj"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool SaveXmlFile(string fileFullPath, object obj, byte[] password = null)
        {
            if ((obj == null) || string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(Path.GetDirectoryName(fileFullPath)))
            {
                IsolatedFile.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            }

            IsolatedStorageFileStream stream = null;
            byte[] data;

            try
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (MemoryStream memStream = new MemoryStream())
                {
                    serializer.WriteObject(memStream, obj);
                    data = memStream.ToArray().EncryptData(password);
                }

                stream = IsolatedFile.CreateFile(fileFullPath);
                stream.Write(data, 0, data.Length);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool SaveFile(string fileFullPath, Stream source)
        {
            if ((source == null) || string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(Path.GetDirectoryName(fileFullPath)))
            {
                IsolatedFile.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            }

            IsolatedStorageFileStream stream = null;
            byte[] result = null;

            try
            {
                stream = new IsolatedStorageFileStream(fileFullPath, FileMode.Create, FileAccess.Write, IsolatedFile);
                result = new byte[source.Length];
                source.Read(result, 0, result.Length);
                stream.Write(result, 0, result.Length);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileFolder"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool SaveFile(string fileName, string fileFolder, byte[] bytes)
        {
            if ((bytes == null) || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(fileFolder))
            {
                IsolatedFile.CreateDirectory(fileFolder);
            }

            string filePath = System.IO.Path.Combine(fileFolder, fileName);

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.CreateFile(filePath);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileFolder"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool SaveFile(string fileName, string fileFolder, string text)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(fileFolder))
            {
                IsolatedFile.CreateDirectory(fileFolder);
            }

            string filePath = System.IO.Path.Combine(fileFolder, fileName);

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.CreateFile(filePath);
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return null;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return null;
            }

            IsolatedStorageFileStream stream = null;
            byte[] result = null;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                return result;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static T ReadXmlFile<T>(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return default(T);
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return default(T);
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
            catch (Exception)
            {
                return default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static T ReadXmlFile<T>(string fileFullPath, byte[] password = null)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return default(T);
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return default(T);
            }

            IsolatedStorageFileStream stream = null;
            byte[] data;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);

                using (MemoryStream memStream = new MemoryStream())
                {
                    stream.CopyTo(memStream);
                    data = memStream.ToArray().DecryptData(password);
                }

                using (MemoryStream dataStream = new MemoryStream(data))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(dataStream);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static Stream ReadFileStream(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return null;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return null;
            }

            IsolatedStorageFileStream stream = null;
            byte[] result = null;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
                MemoryStream memStream = new MemoryStream(result);
                return memStream;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static string ReadTextFile(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return string.Empty;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return string.Empty;
            }

            IsolatedStorageFileStream stream = null;
            string result;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                    return result ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static string ReadTextFileFromLine(string fileFullPath, int lineNumber)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return string.Empty;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return string.Empty;
            }

            IsolatedStorageFileStream stream = null;
            string result;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < lineNumber - 1; i++)
                    {
                        reader.ReadLine();
                    }

                    result = reader.ReadToEnd();
                    return result ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static string ReadLineTextFile(string fileFullPath, int lineNumber)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return string.Empty;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return string.Empty;
            }

            IsolatedStorageFileStream stream = null;
            string result = string.Empty;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < lineNumber - 1; i++)
                    {
                        reader.ReadLine();
                    }

                    result = reader.ReadLine();
                    return result ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static ImageSource LoadImage(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return null;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return null;
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.OpenFile(fileFullPath, FileMode.Open, FileAccess.Read);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                return image;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileFolder"></param>
        /// <returns></returns>
        public static ImageSource LoadImage(string fileName, string fileFolder)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string filePath = System.IO.Path.Combine(fileFolder, fileName);

            if (!IsolatedFile.FileExists(filePath))
            {
                return null;
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.OpenFile(filePath, FileMode.Open, FileAccess.Read);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                return image;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string searchPattern)
        {
            try
            {
                return IsolatedFile.GetFileNames(searchPattern);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string[] GetDirectoryNames(string searchPattern)
        {
            try
            {
                return IsolatedFile.GetDirectoryNames(searchPattern);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetSubDirectoryNames(string path)
        {
            try
            {
                return IsolatedFile.GetDirectoryNames(Path.Combine(path, "*"));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string fileFullPath)
        {
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return false;
            }

            if (!IsolatedFile.FileExists(fileFullPath))
            {
                return false;
            }

            try
            {
                IsolatedFile.DeleteFile(fileFullPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return false;
            }

            if (!IsolatedFile.DirectoryExists(folder))
            {
                try
                {
                    IsolatedFile.CreateDirectory(folder);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="folder"></param>
        public static void DeleteDirectory(string folder)
        {
            string searchPattern = folder + "\\*";

            string[] dirs = IsolatedFile.GetDirectoryNames(searchPattern);
            for (int i = 0; i < dirs.Length; i++)
            {
                string dirPath = folder + "\\" + dirs[i];
                DeleteDirectory(dirPath);
            }

            string[] files = IsolatedFile.GetFileNames(searchPattern);
            for (int j = 0; j < files.Length; j++)
            {
                string filePath = folder + "\\" + files[j];
                IsolatedFile.DeleteFile(filePath);
            }

            IsolatedFile.DeleteDirectory(folder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destnFile"></param>
        /// <returns></returns>
        public static bool CopyPictureToMediaLibrary(string sourceFile, string destnFile = null)
        {
            if (string.IsNullOrEmpty(sourceFile))
            {
                return false;
            }

            if (!IsolatedFile.FileExists(sourceFile))
            {
                return false;
            }

            if (string.IsNullOrEmpty(destnFile))
            {
                destnFile = Path.GetFileName(sourceFile);
            }

            IsolatedStorageFileStream stream = null;

            try
            {
                stream = IsolatedFile.OpenFile(sourceFile, FileMode.Open, FileAccess.Read);
                MediaLibrary mediaLibrary = new MediaLibrary();
                mediaLibrary.SavePicture(destnFile, stream);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
