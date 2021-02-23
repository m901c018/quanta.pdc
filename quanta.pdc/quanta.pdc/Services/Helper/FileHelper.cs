using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace cns.Services.Helper
{
    public class FileHelper
    {
        #region === Declaration ===

        /// <summary>
        /// 判斷檔案上傳位置，若為 True 則為 DFS 空間，若為 False 則為本機空間
        /// </summary>
        public static bool IsReleased = true;


        //public static string DFS_LOCAL_PATH { get { return ConfigurationManager.AppSettings["DFS_LOCAL_PATH"]; } }

        //public static string FILESIZE { get { return Configuration.GetConnectionString("DFS_FILESIZE"); } }

        public static string RootPath { get; set; }

        public FileHelper(IHostingEnvironment _hostingEnvironment)
        {
            RootPath = _hostingEnvironment.WebRootPath;
        }

        /// <summary>
        /// 限制檔案上傳容量
        /// </summary>
        private static int iSizeLimit = 20 * 1024 * 1024; //int.Parse(FILESIZE)

        /// <summary>
        /// 存放匯出 Zip 檔狀態的根目錄
        /// </summary>
        private static string mExportStatusRootPath
        {
            get
            {
                return Path.Combine(RootPath, "App_Data");
            }
        }

        #endregion

        #region === General Functions ===

        public static bool CreateNewFolder(string newPath)
        {
            bool bResult = false;
            if (!string.IsNullOrEmpty(newPath))
            {
                try
                {
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                        bResult = true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return bResult;
        }

        public static bool SaveFile(IFormFile file, string folder, string newFileName,
            out string filePath, out string message)
        {
            bool bResult = false;
            string sMessage = "";
            string sDetinationPath = "";
            if (file != null)
            {
                if (file.Length <= iSizeLimit)
                {
                    string sNewFileName = (!string.IsNullOrEmpty(newFileName) ? newFileName : file.FileName);
                    string sFolderPath = Path.Combine(RootPath, folder);

                    // 路徑不存在則建立新目錄
                    CreateNewFolder(sFolderPath);
                    sDetinationPath = Path.Combine(sFolderPath, sNewFileName);
                    try
                    {
                        using (Stream fileStream = new FileStream(sDetinationPath, FileMode.CreateNew))
                        {
                            file.CopyToAsync(fileStream);
                        }
                        bResult = true;
                    }
                    catch (Exception ex)
                    {
                        sMessage = "儲存時, 發生異常!";
                    }
                }
                else
                {
                    sMessage = "超出系統預設限制大小!";
                }
            }
            else
            {
                sMessage = "檔案不可為 null";
            }
            filePath = sDetinationPath;
            message = sMessage;
            return bResult;
        }

        public static bool RemoveFile(string filePath)
        {
            bool bResult = false;
            string sArchivePath = "";
            string sNewFilePath = "";
            if (File.Exists(filePath))
            {
                try
                {
                    sArchivePath = Path.Combine(RootPath, "Archive");
                    sNewFilePath = Path.Combine(sArchivePath, Path.GetFileName(filePath));
                    CreateNewFolder(sArchivePath);
                    File.Move(filePath, sNewFilePath);
                    //File.Delete(filePath);
                    bResult = true;
                }
                catch (Exception ex)
                {
                }
            }
            return bResult;
        }

        public static bool DeleteFile(string filePath)
        {
            bool bResult = false;
            if (!string.IsNullOrEmpty(filePath))
            {
                FileAttributes fileAttr = File.GetAttributes(filePath);
                if (fileAttr.HasFlag(FileAttributes.Directory))
                {
                    if (Directory.Exists(filePath))
                    {
                        try
                        {
                            Directory.Delete(filePath, true);
                            bResult = true;
                        }
                        catch (Exception ex)
                        {
                            Exception eLog = new Exception("Error: " + ex.ToString());
                            //ExceptionHandler.Handle(eLog);
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                            bResult = true;
                        }
                        catch (Exception ex)
                        {
                            Exception eLog = new Exception("Error: " + ex.ToString());
                            //ExceptionHandler.Handle(eLog);
                        }
                    }
                    else
                    {
                    }
                }
            }
            else
            {
            }
            return bResult;
        }

        public static bool FileLogging(string path, string result)
        {
            string sLogPath = path;
            try
            {
                FileHelper.CreateNewFolder(sLogPath);
                StreamWriter swLog = new StreamWriter(sLogPath + @"\Log.txt", true, Encoding.UTF8);
                swLog.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " - Result:");
                swLog.WriteLine(result);
                swLog.WriteLine("---------------");
                swLog.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static string ReadTextFile(Stream stream)
        {
            string sResult = "";
            if (stream != null)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        if (sr != null)
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return sResult;
        }


        public static string ReadTextFile(string path)
        {
            string sResult = "";
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        StreamReader sr = new StreamReader(stream);
                        if (sr != null)
                        {
                            sResult = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return sResult;
        }

        public static FileInfo[] GetPhiscalFiles()
        {
            string sMessage = "";
            if (Directory.Exists(RootPath))
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(RootPath);
                    if (dirInfo != null)
                    {
                        var files = dirInfo.GetFiles();
                        if (files != null)
                        {
                            return files;
                        }
                    }
                    else
                    {
                        sMessage += "Directory is null!";
                    }
                }
                catch (Exception ex)
                {
                    Exception eLog = new Exception("Error: " + ex.ToString());
                    //ExceptionHandler.Handle(eLog);
                }
            }
            else
            {
                sMessage += "Directory is not existed!";
            }
            Exception eLog2 = new Exception(sMessage);
            //ExceptionHandler.Handle(eLog2);
            return null;
        }

        public static FileInfo[] GetPhiscalFiles(string subPath)
        {
            string sPath = "";
            string sMessage = "";
            if (string.IsNullOrEmpty(subPath))
            {
                sPath = RootPath;
            }
            else
            {
                sPath = subPath;
            }
            if (Directory.Exists(sPath))
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(sPath);
                    if (dirInfo != null)
                    {
                        var files = dirInfo.GetFiles();
                        if (files != null)
                        {
                            return files;
                        }
                        else
                        {
                            sMessage += "Directory is null!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception eLog = new Exception("Error: " + ex.ToString());
                    //ExceptionHandler.Handle(eLog);
                }
            }
            else
            {
                sMessage += "Directory is not existed!";
            }
            Exception eLog2 = new Exception(sMessage);
            //ExceptionHandler.Handle(eLog2);
            return null;
        }

        public static DirectoryInfo[] GetPhiscalDirectories()
        {
            string sMessage = "";
            if (Directory.Exists(RootPath))
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(RootPath);
                    if (dirInfo != null)
                    {
                        var directories = dirInfo.GetDirectories();
                        if (directories != null)
                        {
                            return directories;
                        }
                        else
                        {
                            sMessage += "Directories are null!";
                        }
                    }
                    else
                    {
                        sMessage += "Directory is null!";
                    }
                }
                catch (Exception ex)
                {
                    Exception eLog = new Exception("Error: " + ex.ToString());
                    //ExceptionHandler.Handle(eLog);
                }
            }
            else
            {
                sMessage += "Directory is Not Existed!";
            }
            Exception eLog2 = new Exception(sMessage);
            //ExceptionHandler.Handle(eLog2);
            return null;
        }

        public static DirectoryInfo[] GetPhiscalDirectories(string subPath)
        {
            string sPath = "";
            string sMessage = "";
            if (string.IsNullOrEmpty(subPath))
            {
                sPath = RootPath;
            }
            else
            {
                sPath = subPath;
            }
            if (Directory.Exists(sPath))
            {
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(sPath);
                    if (dirInfo != null)
                    {
                        var directories = dirInfo.GetDirectories();
                        if (directories != null)
                        {
                            return directories;
                        }
                        else
                        {
                            sMessage += "Directories are null!";
                        }
                    }
                    else
                    {
                        sMessage += "Directory is null!";
                    }
                }
                catch (Exception ex)
                {
                    Exception eLog = new Exception("Error: " + ex.ToString());
                    //ExceptionHandler.Handle(eLog);
                }
            }
            else
            {
                sMessage += "Directory is Not Existed!";
            }
            Exception eLog2 = new Exception(sMessage);
            //ExceptionHandler.Handle(eLog2);
            return null;
        }

        public static long GetDirectorySize(string path)
        {
            long lResult = 0;

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                string[] arrFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (string name in arrFiles)
                {
                    FileInfo info = new FileInfo(name);
                    lResult += info.Length;
                }
            }
            return lResult;
        }

        /// <summary>
        /// 取代直接使用 Response.End(), 以更安全的方式結束回應請求
        /// </summary>
        //public static void SafeResponseEnd()
        //{
        //    HttpContext.Current.Response.Flush();
        //    // 阻斷繼續送 HTTP 內容給 Client
        //    HttpContext.Current.Response.SuppressContent = true;
        //    // 跳過所有 HTTP pipeline 中的事件與過濾器，直接調用 EndRequest 事件
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        #endregion

        #region === Zip Handler ===

        public static void ZipFolder(string folderPath, ZipOutputStream zipStream)
        {
            string path = !folderPath.EndsWith("\\") ? string.Concat(folderPath, "\\") : folderPath;
            ZipFolder(path, path, zipStream);
        }

        private static void ZipFolder(string RootFolder, string CurrentFolder, ZipOutputStream zipStream)
        {
            string[] SubFolders = Directory.GetDirectories(CurrentFolder);
            foreach (string Folder in SubFolders)
            {
                ZipFolder(RootFolder, Folder, zipStream);
            }
            try
            {
                string relativePath = string.Concat(CurrentFolder.Substring(RootFolder.Length), "/");
                if (relativePath.Length > 1)
                {
                    ZipEntry dirEntry;
                    dirEntry = new ZipEntry(relativePath);
                    dirEntry.DateTime = DateTime.Now;

                }
                foreach (string file in Directory.GetFiles(CurrentFolder))
                {
                    AddFileToZip(zipStream, relativePath, file);
                }
            }
            catch (Exception ex)
            {
                Exception exception = new Exception("FileHelper.ZipFolder: " + ex.ToString());
                //ExceptionHandler.Handle(exception);
            }
        }

        private static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[10 * 1024 * 1024];
            string fileRelativePath = string.Concat((relativePath.Length > 1 ? relativePath : string.Empty), Path.GetFileName(file));

            try
            {
                ZipEntry entry = new ZipEntry(fileRelativePath);
                entry.DateTime = DateTime.Now;
                zStream.PutNextEntry(entry);

                using (FileStream fs = System.IO.File.OpenRead(file))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zStream.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }
            }
            catch (Exception ex)
            {
                Exception exception = new Exception("FileHelper.AddFileToZip: " + ex.ToString());
                //ExceptionHandler.Handle(exception);
            }
        }

        public static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;		// AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;			// Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];		// 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        #endregion

        #region === Issue File Handler ===

        //public static void CopyFileToTempFolder(List<DocVIssueMaster> issueList, List<DocIssueFile> fileList, string zipFolderPath, string tempFilesFolderPath)
        //{
        //    if (issueList.Any() && fileList.Any())
        //    {
        //        try
        //        {
        //            if (FileHelper.CreateNewFolder(tempFilesFolderPath))
        //            {
        //                foreach (var item in fileList)
        //                {
        //                    var qryIssue = issueList.FirstOrDefault(n => n.IssueID == item.IssueID);
        //                    if (!string.IsNullOrEmpty(item.FilePath) && qryIssue != null)
        //                    {
        //                        string destPath = Path.Combine(tempFilesFolderPath, qryIssue.IssueNo + "_" + item.IssueFileID.ToString() + "_" + item.FileName);
        //                        System.IO.File.Copy(item.FilePath, destPath);
        //                        FileLogging(zipFolderPath, "Destination: " + destPath);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Exception exception = new Exception("FileHelper.CopyFileToTempFolder: " + ex.ToString());
        //            ExceptionHandler.Handle(exception);
        //        }
        //    }
        //}

        //public static void CopyFileToTempFolder_Backup(List<DocVIssueMaster> fileList, string zipFolderPath, string tempFilesFolderPath)
        //{
        //    try
        //    {
        //        if (FileHelper.CreateNewFolder(tempFilesFolderPath))
        //        {
        //            foreach (var item in fileList)
        //            {
        //                if (!string.IsNullOrEmpty(item.FilePath))
        //                {
        //                    string destPath = Path.Combine(tempFilesFolderPath, Path.GetFileName(@item.FilePath));
        //                    System.IO.File.Copy(item.FilePath, destPath);
        //                    FileLogging(zipFolderPath, "Destination: " + destPath);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLogging(zipFolderPath, "CopyFileToTempFolder: " + ex.ToString());
        //    }
        //}

        public static bool CheckIsDownloadingZipForSearch(string projectId)
        {
            bool bResult = false;
            // 若 Zip 正在下載, 會產生一份檔名為「專案代碼」的 txt 檔, 只要這份檔案存在, 則提示使用者稍後下載 
            string sPath = Path.Combine(mExportStatusRootPath, "FileStatus", "Project_" + projectId + ".txt");
            bResult = System.IO.File.Exists(sPath);
            return bResult;
        }

        public static bool CheckIsDownloadingZipForReport(string projectId)
        {
            bool bResult = false;
            // 若 Zip 正在下載, 會產生一份檔名為「報表代碼」的 txt 檔, 只要這份檔案存在, 則提示使用者稍後下載 
            string sPath = Path.Combine(mExportStatusRootPath, "FileStatus", "Report_" + projectId + ".txt");
            bResult = System.IO.File.Exists(sPath);
            return bResult;
        }

        public static bool CreateTempFileForSearch(string projectId)
        {
            bool bResult = false;
            string sFilePath = Path.Combine(mExportStatusRootPath, "FileStatus", "Project_" + projectId + ".txt");
            string sRootPath = Path.Combine(mExportStatusRootPath, "FileStatus");
            if (!System.IO.File.Exists(sFilePath))
            {
                try
                {
                    FileHelper.CreateNewFolder(sRootPath);
                    var file = System.IO.File.Create(sFilePath);
                    if (file != null)
                        file.Close();
                }
                catch (Exception ex)
                {

                }
            }
            return bResult;
        }

        public static bool CreateTempFileForReport(string reportId)
        {
            bool bResult = false;
            string sFilePath = Path.Combine(mExportStatusRootPath, "FileStatus", "Report_" + reportId + ".txt");
            string sRootPath = Path.Combine(mExportStatusRootPath, "FileStatus");
            if (!System.IO.File.Exists(sFilePath))
            {
                try
                {
                    FileHelper.CreateNewFolder(sRootPath);
                    var file = System.IO.File.Create(sFilePath);
                    if (file != null)
                        file.Close();
                }
                catch (Exception ex)
                {

                }
            }
            return bResult;
        }

        public static bool DeleteTempFileForSearch(string projectId)
        {
            bool bResult = false;
            string sPath = Path.Combine(mExportStatusRootPath, "FileStatus", "Project_" + projectId + ".txt");
            if (System.IO.File.Exists(sPath))
            {
                try
                {
                    System.IO.File.Delete(sPath);
                    bResult = true;
                }
                catch (Exception ex)
                {
                    Exception exception = new Exception("FileHelper.DeleteTempFileForSearch: " + ex.ToString());
                    //ExceptionHandler.Handle(exception);
                }
            }
            return bResult;
        }

        public static bool DeleteTempFileForReport(string reportId)
        {
            bool bResult = false;
            string sPath = Path.Combine(mExportStatusRootPath, "FileStatus", "Report_" + reportId + ".txt");
            if (System.IO.File.Exists(sPath))
            {
                try
                {
                    System.IO.File.Delete(sPath);
                    bResult = true;
                }
                catch (Exception ex)
                {
                }
            }
            return bResult;
        }

        #endregion

        public static string GetContentType(string Extension)
        {
            string ContentType = string.Empty;
            switch (Extension)
            {
                case ".asf":
                    ContentType = "video/x-ms-asf";
                    break;
                case ".avi":
                    ContentType = "video/avi";
                    break;
                case ".doc":
                    ContentType = "application/msword";
                    break;
                case ".zip":
                    ContentType = "application/zip";
                    break;
                case ".xls":
                case ".xlsx":
                    ContentType = "application/vnd.ms-excel";
                    break;
                case ".gif":
                    ContentType = "image/gif";
                    break;
                case ".jpg":
                case "jpeg":
                    ContentType = "image/jpeg";
                    break;
                case ".wav":
                    ContentType = "audio/wav";
                    break;
                case ".mp3":
                    ContentType = "audio/mpeg3";
                    break;
                case ".mpg":
                case ".mpeg":
                    ContentType = "video/mpeg";
                    break;
                case ".rtf":
                    ContentType = "application/rtf";
                    break;
                default:
                    ContentType = "application/octet-stream";
                    break;
            }
            return ContentType;
        }
    }
}
