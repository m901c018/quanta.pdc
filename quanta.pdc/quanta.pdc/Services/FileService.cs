﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.Services.Helper;
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

        private readonly PDC_Member _Member;

        public FileService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, PDC_Member Member)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            rootPath = _hostingEnvironment.WebRootPath;
            _Member = Member;
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

        /// <summary> 儲存檔案並返回檔案路徑
        /// 
        /// </summary>
        /// <param name="file">NetCore檔案類別</param>
        /// <returns></returns>
        public string SaveAndGetExcelPath(MemoryStream file, bool IsCsv = false)
        {
            string FilePath = string.Empty;
            //隨機產生檔案名
            if (IsCsv)
                FilePath = rootPath + "\\FileUpload\\" + Guid.NewGuid().ToString("N") + ".txt";
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

        /// <summary> 下載檔案
        /// 
        /// </summary>
        /// <param name="FileName">檔案名</param>
        /// <returns></returns>
        public MemoryStream DownloadFile(string FileName,bool IsTemp = false)
        {
            string FilePath = string.Empty;
            //檔案名
            if(IsTemp)
                FilePath = rootPath + "\\Temp\\" + FileName;
            else
                FilePath = rootPath + "\\FileUpload\\" + FileName;

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

        
        /// <summary> 取得檔案資料
        /// 
        /// </summary>
        /// <param name="FileID">檔案ID</param>
        /// <returns></returns>
        public PDC_File GetFileOne(Int64 FileID)
        {
            PDC_File PDC_File = new PDC_File();

            PDC_File = _context.PDC_File.Where(x => x.FileID == FileID).SingleOrDefault();

            return PDC_File;
        }

        /// <summary> 取得檔案資料
        /// 
        /// </summary>
        /// <param name="SourceID">關聯ID</param>
        /// <param name="FunctionName">檔案FunctionName</param>
        /// <returns></returns>
        public PDC_File GetFileOne(Int64 SourceID,string FunctionName)
        {
            PDC_File PDC_File = new PDC_File();

            PDC_File = _context.PDC_File.Where(x => x.SourceID == SourceID && x.FunctionName == FunctionName).SingleOrDefault();

            return PDC_File;
        }

        /// <summary> 取得檔案集合
        /// 
        /// </summary>
        /// <param name="FunctionName">檔案FunctionName</param>
        /// <param name="SourceID">關聯ID</param>
        /// <returns></returns>
        public List<PDC_File> GetFileList(string FunctionName,Int64 SourceID)
        {
            List<PDC_File> FileList = new List<PDC_File>();

            FileList = _context.PDC_File.Where(x => x.FunctionName == FunctionName && x.SourceID == SourceID).ToList();

            return FileList;
        }

        /// <summary> 取得所有關卡清單關聯檔案
        /// 
        /// </summary>
        /// <param name="Form_StageLogs">關卡資料</param>
        /// <returns></returns>
        public List<PDC_File> GetForm_StageFileList(List<PDC_Form_StageLog> Form_StageLogs)
        {
            List<PDC_File> result = new List<PDC_File>();

            foreach(PDC_Form_StageLog item in Form_StageLogs)
            {
                result.AddRange(_context.PDC_File.Where(x => x.FunctionName == "FormStage" && x.SourceID == item.StageLogID).ToList());
            }

            return result;
        }

        /// <summary> 取得參數表對應檔案
        /// 
        /// </summary>
        /// <param name="ParameterList">參數表資料</param>
        /// <param name="FunctionName">檔案FunctionName</param>
        /// <returns></returns>
        public List<PDC_File> GetParameterFileList(List<PDC_Parameter> ParameterList,string FunctionName)
        {
            List<PDC_File> result = new List<PDC_File>();

            foreach (PDC_Parameter item in ParameterList)
            {
                result.AddRange(_context.PDC_File.Where(x => x.FunctionName == FunctionName && x.SourceID == item.ParameterID).ToList());
            }

            return result;
        }

        /// <summary> 取得檔案集合
        /// 
        /// </summary>
        /// <param name="FunctionName">檔案FunctionName</param>
        /// <returns></returns>
        public List<PDC_File> GetFileList(string FunctionName)
        {
            List<PDC_File> FileList = new List<PDC_File>();

            FileList = _context.PDC_File.Where(x => x.FunctionName == FunctionName).ToList();

            return FileList;
        }

        /// <summary> 檔案新增
        /// 
        /// </summary>
        /// <param name="file">上傳檔案</param>
        /// <param name="FunctionName">檔案FunctionName</param>
        /// <param name="item">返回檔案資料</param>
        /// <param name="Folder">存取檔案資料夾</param>
        /// <param name="SourceID">關聯ID</param>
        /// <param name="FileDescription">檔案描述</param>
        /// <returns></returns>
        public Boolean FileAdd(IFormFile file, string FunctionName, out PDC_File item, string Folder = "FileUpload", Int64 SourceID = 0, string FileDescription = "")
        {
            item = new PDC_File();
            string FilePath = string.Empty;
            string ErrorMsg = string.Empty;
            FileHelper fileHelper = new FileHelper(_hostingEnvironment);
            try
            {
                if(fileHelper.SaveFile(file, Folder, Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName), out FilePath, out ErrorMsg))
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
                    item.Creator = _Member.MemberID.ToString();
                    item.CreatorDate = DateTime.Now;
                    item.CreatorName = _Member.UserEngName;
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

        /// <summary> 新增檔案資料
        /// 
        /// </summary>
        /// <param name="item">檔案資料</param>
        /// <returns></returns>
        public Boolean FileAdd(ref PDC_File item)
        {
            try
            {
                item.Creator = _Member.MemberID.ToString();
                item.CreatorDate = DateTime.Now;
                item.CreatorName = _Member.UserEngName;
                _context.PDC_File.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary> 更新檔案資料
        /// 
        /// </summary>
        /// <param name="NewFile">檔案資料</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns></returns>
        public bool UpdateFile(PDC_File NewFile, ref string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            try
            {
                PDC_File OldForm = GetFileOne(NewFile.FileID);

                OldForm.SourceID = NewFile.SourceID;
                OldForm.FileRemark = NewFile.FileRemark;
                OldForm.FileNote = NewFile.FileNote;
                OldForm.FileDescription = NewFile.FileDescription;
                _context.SaveChanges();

                ErrorMsg = "儲存成功";
            }
            catch (Exception ex)
            {
                ErrorMsg = "儲存失敗";
                return false;
            }
            return true;
        }

        /// <summary> 更新檔案關聯ID
        /// 
        /// </summary>
        /// <param name="SourceID">關聯ID</param>
        /// <param name="FileIDList">要更新檔案集合</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns></returns>
        public bool UpdateFileSource(Int64 SourceID,List<Int64> FileIDList,ref string ErrorMsg)
        {
            FileHelper fileHelper = new FileHelper(_hostingEnvironment);
            try
            {
                List<PDC_File> NewFileList = new List<PDC_File>();

                foreach (Int64 item in FileIDList)
                {
                    PDC_File File = _context.PDC_File.Where(x => x.FileID == item).SingleOrDefault();
                    File.SourceID = SourceID;
                    NewFileList.Add(File);
                }

                _context.SaveChanges();

                //把檔案從Temp移到FileUpload
                foreach (PDC_File item in NewFileList)
                {
                    fileHelper.RemoveFile(item.FileFullName);
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "更新檔案來源失敗";
                return false;
            }
            return true;
        }

        /// <summary> 移除檔案
        /// 
        /// </summary>
        /// <param name="FileID">檔案ID</param>
        /// <returns></returns>
        public Boolean FileRemove(Int64 FileID)
        {
            FileHelper fileHelper = new FileHelper(_hostingEnvironment);
            try
            {
                if (_context.PDC_File.Where(x => x.FileID == FileID).Any())
                {
                    PDC_File item = _context.PDC_File.Where(x => x.FileID == FileID).SingleOrDefault();
                    //檔案名(只有暫存區檔案需要刪除)
                    var FilePath = rootPath + "\\Temp\\" + item.FileFullName;
                    //如果為檔案則刪除檔案
                    if (item.FileCategory == 1 && item.SourceID == 0 && !FileHelper.DeleteFile(FilePath))
                    {
                        return false;
                    }

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
