using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cns.Services
{
    public class FileService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        public static string rootPath;

        public FileService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            rootPath = _hostingEnvironment.WebRootPath;
        }

        /// <summary> 儲存檔案並返回檔案路徑
        /// 
        /// </summary>
        /// <param name="file">NetCore檔案類別</param>
        /// <returns></returns>
        public string SaveAndGetExcelPath(IFormFile file)
        {
            //隨機產生檔案名
            var FilePath = rootPath + "\\FileUpload\\" + Guid.NewGuid().ToString("N") + ".xlsx";

            try
            {
                using (Stream fileStream = new FileStream(FilePath, FileMode.CreateNew))
                {
                    file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {

                return "Error";
            }

            return FilePath;
        }

        /// <summary> 下載檔案
        /// 
        /// </summary>
        /// <param name="FileName">檔案名</param>
        /// <returns></returns>
        public MemoryStream DownloadFile(string FileName)
        {
            //檔案名
            var FilePath = rootPath + "\\FileUpload\\" + FileName;
            MemoryStream fileStream = new MemoryStream();

            using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(fileStream);

            return fileStream;
        }

        /// <summary> 下載範例檔案
        /// 
        /// </summary>
        /// <returns></returns>
        public MemoryStream DownloadSampleFile()
        {
            //檔案名
            var FilePath = rootPath + "\\File\\CNS_Sample.xlsx";
            MemoryStream fileStream = new MemoryStream();

            using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(fileStream);

            return fileStream;
        }

        /// <summary> 儲存檔案並返回檔案路徑
        /// 
        /// </summary>
        /// <param name="file">NetCore檔案類別</param>
        /// <returns></returns>
        public string SaveAndGetExcelPath(MemoryStream file,bool IsCsv = false)
        {
            string FilePath = string.Empty;
            //隨機產生檔案名
            if (IsCsv)
                FilePath = rootPath + "\\FileUpload\\" + Guid.NewGuid().ToString("N") + ".csv";
            else
                FilePath = rootPath + "\\FileUpload\\" + Guid.NewGuid().ToString("N") + ".xlsx";

            try
            {
                using (Stream fileStream = new FileStream(FilePath, FileMode.CreateNew))
                {
                    //file.CopyToAsync(fileStream);
                    fileStream.Write(file.ToArray(), 0, file.ToArray().Length);
                }
            }
            catch (Exception ex)
            {

                return "Error";
            }

            return FilePath;
        }

        public PDC_File GetFileOne(Int64 FileID)
        {
            PDC_File PDC_File = new PDC_File();

            PDC_File = _context.PDC_File.Where(x => x.FileID == FileID).SingleOrDefault();

            return PDC_File;
        }

        public List<PDC_File> GetFileList(string FunctionName,Int64 SourceID)
        {
            List<PDC_File> FileList = new List<PDC_File>();

            FileList = _context.PDC_File.Where(x => x.FunctionName == FunctionName && x.SourceID == SourceID).ToList();

            return FileList;
        }

        public List<PDC_File> GetFileList(string FunctionName)
        {
            List<PDC_File> FileList = new List<PDC_File>();

            FileList = _context.PDC_File.Where(x => x.FunctionName == FunctionName).ToList();

            return FileList;
        }

        public Boolean FileAdd(IFormFile file, string FunctionName, string userId, string userName, out PDC_File item, Int64 SourceID = 0, string FileDescription = "")
        {
            item = new PDC_File();
            try
            {
                string FilePath = SaveAndGetExcelPath(file);
                if(FilePath != "Error")
                {
                    item.FileFullName = Path.GetFileName(FilePath);
                    item.FileName = file.FileName;
                    item.FileExtension = Path.GetExtension(FilePath);
                    item.FileType = 2;
                    item.FileCategory = 1;
                    item.FileSize = file.Length;
                    item.FunctionName = FunctionName;
                    item.FileDescription = FileDescription;
                    item.SourceID = SourceID;
                    item.Creator = userId;
                    item.CreatorDate = DateTime.Now;
                    item.CreatorName = userName;
                    _context.PDC_File.Add(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean FileAdd(ref PDC_File item)
        {
            try
            {
                _context.PDC_File.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean FileRemove(Int64 FileID)
        {
            try
            {
                if (_context.PDC_File.Where(x => x.FileID == FileID).Any())
                {
                    PDC_File item = _context.PDC_File.Where(x => x.FileID == FileID).SingleOrDefault();
                    _context.PDC_File.Remove(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
