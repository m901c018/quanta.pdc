using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        /// <param name="excel">NetCore檔案類別</param>
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

        public Boolean FileAdd(IFormFile file, string FunctionName,out PDC_File item,Int64 SourceID = 0, string FileDescription = "")
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
                    item.Creator = "c5805dbf-dac5-41e6-bb72-5eb0b449134d";
                    item.CreatorDate = DateTime.Now;
                    item.CreatorName = "super@admin.com";
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
    }
}
