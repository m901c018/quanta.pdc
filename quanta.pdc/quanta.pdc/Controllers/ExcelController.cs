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

namespace cns.Controllers
{
    //[Authorize(Roles = Services.App.Pages.Excel.RoleName)]
    public class ExcelController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        //consume custom security service to get all roles
        public IActionResult Index()
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);
            ParameterService ParameterService = new ParameterService(_context);
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
            //model.ThicknessTotal = decimal.Round(ThicknessTotal, 2);

            //Session紀錄
            HttpContext.Session.SetObjectAsJson("SessionExcelData", ExcelDt);
            HttpContext.Session.SetString("SessionFileName", Path.GetFileName(file.FileName));
            //TempData["SessionExcelData"] = JsonConvert.SerializeObject(ExcelDt);

            //return PartialView("m_ExcelPartial", model);
            return Json(ExcelDt);
        }

        [HttpPost]
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
        public IActionResult SaveFormExcelFile([FromBody]object model)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);

            long FileID = 0;
            Int64.TryParse(HttpContext.Session.GetString("SessionFileID"), out FileID);

            if (FileID > 0)
            {
                FileService FileService = new FileService(_hostingEnvironment, _context);
                PDC_File File = FileService.GetFileOne(FileID);

                string FilePath = _hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName;
                //取得範例
                Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                stream.Position = 0; // <-- Add this, to make it work


                Helper.SaveExcel(stream, FilePath, ViewModel.StackupDetalList);

                return Json(new { status = 0, ErrorMessage = "" });
            }

            return Json(new { status = 400, ErrorMessage = "存檔失敗" });
        }


        [HttpGet]
        public IActionResult DownloadSample()
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            if(FileService.GetFileList("ConfigurationSample").Any())
            {
                PDC_File Sample = FileService.GetFileList("ConfigurationSample").FirstOrDefault();

                return RedirectToAction("Download", new { fileName = Sample.FileFullName });
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
        public IActionResult DownloadCheckFile([FromBody]object model)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);
            string SampleFilePath = "";
            if (FileService.GetFileList("ConfigurationSample").Any())
                SampleFilePath = FileService.GetFileList("ConfigurationSample").FirstOrDefault().FileFullName;

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment, SampleFilePath);

            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);
            //取得範例
            MemoryStream stream = Helper.ExportExcelSample(ViewModel.StackupDetalList);

            string FilePath = FileService.SaveAndGetExcelPath(stream);

            string FileName = Path.GetFileName(FilePath);

            return Json(FileName);
        }

        [HttpGet]
        public IActionResult ExcelEdit(Boolean IsOnlyOnline = false)
        {
            

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context);
            m_ExcelPartial ViewModel = new m_ExcelPartial();
            ParameterService parameterService = new ParameterService(_context);

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
        public IActionResult Download(string fileName)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            string RealFileName = HttpContext.Session.GetString("SessionFileName");

            if (string.IsNullOrWhiteSpace(RealFileName))
                RealFileName = "CNS";
            //取得範例
            MemoryStream stream = FileService.DownloadFile(fileName);


            string sFileName = HttpUtility.UrlEncode(Path.GetFileNameWithoutExtension(RealFileName) + ".xlsx");


            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

        [HttpPost]
        public IActionResult DownloadError([FromBody]object model)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);
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
        public IActionResult DownloadErrorFile(string FileName)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);
            string OutFileName = string.Empty;

            var RealFileName = HttpContext.Session.GetString("SessionFileName");

            if (string.IsNullOrWhiteSpace(RealFileName))
            {
                OutFileName = "Error" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            }
            else
            {
                OutFileName = Path.GetFileNameWithoutExtension(RealFileName) + "Error" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            }

            //取得範例
            MemoryStream stream = FileService.DownloadFile(FileName);

            return File(stream.ToArray(), "text/csv", OutFileName);
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