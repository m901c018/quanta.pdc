using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Services.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using cns.ViewModels;
using static cns.Services.Helper.ExcelHepler;
using cns.Models;
using Newtonsoft.Json;
using cns.Services;
using cns.Data;
using cns.Services.App;

namespace cns.Controllers
{
    //[Authorize(Roles = Services.App.Pages.Excel.RoleName)]
    public class ExcelController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        [ActionCheck]
        public IActionResult Index()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ParameterService ParameterService = new ParameterService(_context, UserInfo.User);
            m_ExcelPartial model = new m_ExcelPartial();
            //取得要同步驗證的資料
            model.FastLinkList = ParameterService.GetParameterList("Configuration_HomeLink").Where(x => x.IsSync == true).OrderBy(x => x.OrderNo).ToList();
            //取得要同步驗證的連結
            model.FastLinkFileList = FileService.GetFileList("Configuration_HomeLink").Where(x => model.FastLinkList.Select(pa => pa.ParameterID).Contains(x.SourceID)).ToList();
            //取得Excel範本
            model.m_CNSSampleFile = FileService.GetFileList("ConfigurationSample").FirstOrDefault();

            return View(model);
        }

        public ExcelController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadFile(IFormFile file)
        {
            m_ExcelPartial model = new m_ExcelPartial();
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            //FormApply用，這邊要清掉
            HttpContext.Session.SetString("SessionFileID", "");

            if (file == null)
            {
                ISheet sheet = Helper.GetSheetSample();
                //資料轉為Datatable
                DataTable sheetDt = Helper.GetDataTableFromExcel(sheet, true);
                //驗證資料
                model.Errmsg = Helper.ExcelStackupCheck(sheetDt);
                model.ExcelSheetDts.Add(sheetDt);
                //Session紀錄
                HttpContext.Session.SetObjectAsJson("SessionExcelData", sheetDt);
                HttpContext.Session.SetString("SessionFileName", "");
                //TempData["SessionExcelData"] = JsonConvert.SerializeObject(sheetDt);

                //return PartialView("m_ExcelPartial", model);
                return Json(sheetDt);
            }
            //驗證是否上傳正確檔案
            if(!Path.GetExtension(file.FileName).Contains("xlsx"))
            {
                return Json(new {status= 400, ErrorMessage = "請上傳xlsx檔案"});
            }

            Stream stream = file.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
            //資料轉為Datatable
            DataTable ExcelDt = Helper.GetDataTableFromExcel(xSSFSheet, true);
            //驗證資料
            model.Errmsg = Helper.ExcelStackupCheck(ExcelDt);

            model.ExcelSheetDts.Add(ExcelDt);
            
            //List<ExcelRow> excelRows = Helper.GetExcelRowFromExcel(xSSFSheet, true);
            //Session紀錄
            HttpContext.Session.SetObjectAsJson("SessionExcelData", ExcelDt);
            HttpContext.Session.SetString("SessionFileName", Path.GetFileName(file.FileName));
            //TempData["SessionExcelData"] = JsonConvert.SerializeObject(ExcelDt);
            //    DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(ViewModel.StackupDetalList);
            //    //
            //    stream = Helper.ExportExcelSample(m_ExcelPartial.SheetData, StackupDetalDt);
            //return PartialView("m_ExcelPartial", model);
            return Json(ExcelDt);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult TestUploadFile(IFormFile file)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            string SampleFilePath = "";
            if (FileService.GetFileList("ConfigurationSample").Any())
                SampleFilePath = FileService.GetFileList("ConfigurationSample").OrderByDescending(x => x.CreatorDate).FirstOrDefault().FileFullName;

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment, SampleFilePath);

            Stream stream = file.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
            //資料轉為Datatable
            DataTable ExcelDt = Helper.GetDataTableFromExcel(xSSFSheet, true);

            List<ExcelRow> excelRows = Helper.GetExcelRowFromExcel(xSSFSheet, true);

            MemoryStream Exportstream = Helper.ExportExcelSample(excelRows, ExcelDt);

            return File(Exportstream.ToArray(), "application/vnd.ms-excel", "text.xlsx");
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult SaveCheckFile([FromBody]object model)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);
            //轉DataTable
            DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(ViewModel.StackupDetalList);
            ////驗證資料
            //Helper.ExcelStackupCheck(StackupDetalDt, ViewModel);
            ViewModel.Errmsg = Helper.ExcelStackupCheck(StackupDetalDt);

            ViewModel.ExcelSheetDts.Add(StackupDetalDt);

            long FileID = 0;
            Int64.TryParse(HttpContext.Session.GetString("SessionFileID"), out FileID);

            //if (string.IsNullOrWhiteSpace(ViewModel.Errmsg) && FileID > 0)
            //{
            //    FileService FileService = new FileService(_hostingEnvironment, _context);
            //    PDC_File File = FileService.GetFileOne(FileID);

            //    string FilePath = _hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName;
            //    //取得範例
            //    Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //    stream.Position = 0; // <-- Add this, to make it work
                

            //    Helper.SaveExcel(stream, FilePath, ViewModel.StackupDetalList);

                
            //}

            return Json(ViewModel.Errmsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult SaveFormExcelFile([FromBody]object model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);

            long FileID = 0;
            Int64.TryParse(HttpContext.Session.GetString("SessionFileID"), out FileID);

            if (FileID > 0)
            {
                FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
                PDC_File File = FileService.GetFileOne(FileID);
                string FilePath = string.Empty;
                if (File.SourceID == 0)
                    FilePath = _hostingEnvironment.WebRootPath + "\\Temp\\" + File.FileFullName;
                else
                    FilePath = _hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName;

                //取得範例
                Stream stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                stream.Position = 0; // <-- Add this, to make it work


                Helper.SaveExcel(stream, FilePath, ViewModel.StackupDetalList);

                return Json(new { status = 0, ErrorMessage = "" });
            }

            return Json(new { status = 400, ErrorMessage = "存檔失敗" });
        }


        [HttpGet]
        [ActionCheck]
        public IActionResult DownloadSample()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            if(FileService.GetFileList("ConfigurationSample").Any())
            {
                PDC_File Sample = FileService.GetFileList("ConfigurationSample").OrderByDescending(x => x.CreatorDate).FirstOrDefault();

                //取得範例
                MemoryStream stream = FileService.DownloadFile(Sample.FileFullName);

                string sFileName = HttpUtility.UrlEncode(Sample.FileName);


                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            else
            {
                //取得範例
                MemoryStream stream = FileService.DownloadSampleFile();


                string sFileName = HttpUtility.UrlEncode("CNS_Sample.xlsx");


                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult DownloadCheckFile([FromBody]object model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context,UserInfo.User);
            string SampleFilePath = "";
            if (FileService.GetFileList("ConfigurationSample").Any())
                SampleFilePath = FileService.GetFileList("ConfigurationSample").OrderByDescending(x=>x.CreatorDate).FirstOrDefault().FileFullName;

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment, SampleFilePath);

            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);

            //
            //m_ExcelPartial m_ExcelPartial = HttpContext.Session.GetObjectFromJson<m_ExcelPartial>("SessionSheetRow");

            MemoryStream stream = Helper.ExportExcelSample(ViewModel.StackupDetalList);
            //if (m_ExcelPartial.SheetData.Count > 0)
            //{
            //    DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(ViewModel.StackupDetalList);
            //    //
            //    stream = Helper.ExportExcelSample(m_ExcelPartial.SheetData, StackupDetalDt);
            //}
            //else
            //{
            //    stream = Helper.ExportExcelSample(ViewModel.StackupDetalList);
            //}

            string FilePath = FileService.SaveAndGetExcelPath(stream);

            string FileName = Path.GetFileName(FilePath);

            return Json(FileName);
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult ExcelEdit(Boolean IsOnlyOnline = false)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context,UserInfo.User);
            m_ExcelPartial ViewModel = new m_ExcelPartial();
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            //組態設定線寬線距規則
            ViewModel.m_ExcelRule = parameterService.GetParameterOne("ConfigurationFormApply");
            //Session紀錄
            DataTable ExcelDt = HttpContext.Session.GetObjectFromJson<DataTable>("SessionExcelData");

            long FileID = 0;
            Int64.TryParse(HttpContext.Session.GetString("SessionFileID"), out FileID);

            ViewModel.IsFormApplyEdit = FileID > 0 ? true : false;
            //移除Session
            //HttpContext.Session.Remove("SessionExcelData");
            if (ExcelDt != null && IsOnlyOnline == false)
            {
                ViewModel.ExcelSheetDts.Add(ExcelDt);
            }
            else
            {
                ISheet sheet = Helper.GetSheetSample();
                //資料轉為Datatable
                DataTable sheetDt = Helper.GetDataTableFromExcel(sheet, true);

                ViewModel.ExcelSheetDts.Add(sheetDt);
            }


            return View(ViewModel);
        }

       
        

        [HttpGet]
        [ActionCheck]
        public IActionResult Download(string fileName)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            string RealFileName = HttpContext.Session.GetString("SessionFileName");

            if (string.IsNullOrWhiteSpace(RealFileName))
                RealFileName = "CNS";
            //取得範例
            MemoryStream stream = FileService.DownloadFile(fileName);


            string sFileName = HttpUtility.UrlEncode(Path.GetFileNameWithoutExtension(RealFileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");


            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult DownloadError([FromBody]object model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);
            //資料轉為DataTable
            DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(ViewModel.StackupDetalList);
            ////驗證資料
            ViewModel.Errmsg = Helper.ExcelStackupCheck(StackupDetalDt);
            ViewModel.ExcelSheetDts.Add(StackupDetalDt);

            var contentBytes = new System.Text.UTF8Encoding().GetBytes(ViewModel.Errmsg);
            var outputBytes = new byte[contentBytes.Length + 3];
            outputBytes[0] = (byte)0xEF;
            outputBytes[1] = (byte)0xBB;
            outputBytes[2] = (byte)0xBF;
            Array.Copy(contentBytes, 0, outputBytes, 3, contentBytes.Length);

            MemoryStream fileStream = new MemoryStream(outputBytes);

            string FilePath = FileService.SaveAndGetExcelPath(fileStream, true);

            string FileName = Path.GetFileName(FilePath);

            return Json(FileName);
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult DownloadErrorFile(string FileName)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            string OutFileName = string.Empty;

            var RealFileName = HttpContext.Session.GetString("SessionFileName");

            if (string.IsNullOrWhiteSpace(RealFileName))
            {
                OutFileName = "Error" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(FileName);
            }
            else
            {
                OutFileName = Path.GetFileNameWithoutExtension(RealFileName) + "Error" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(FileName);
            }

            //取得範例
            MemoryStream stream = FileService.DownloadFile(FileName);

            return File(stream.ToArray(), "text/plain", OutFileName);
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}