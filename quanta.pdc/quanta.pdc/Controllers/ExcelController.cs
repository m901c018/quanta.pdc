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
            PDC_File sample = FileService.GetFileList("ConfigurationSample").FirstOrDefault();

            //加入Excel範本
            if (sample != null)
                model.FastLinkFileList.Add(sample);

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
            //存檔並返回檔案路徑
            //string FilePath = Helper.SaveAndGetExcelPath(file);
            Stream stream = file.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
            //資料轉為Datatable
            DataTable ExcelDt = Helper.GetDataTableFromExcel(xSSFSheet, true);
            //驗證資料
            Helper.ExcelStackupCheck(ExcelDt, model);
          
            return PartialView("m_ExcelPartial", model);
        }

        [HttpPost]
        public IActionResult SaveCheckFile([FromBody]object model)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);
            //存檔並返回檔案路徑
            DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(ViewModel.StackupDetalList);
            ////驗證資料
            Helper.ExcelStackupCheck(StackupDetalDt, ViewModel);

            return Json(ViewModel.Errmsg);
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

            //取得範例
            MemoryStream stream = Helper.ExportExcelSample();

            string sFileName = HttpUtility.UrlEncode("Sample.xlsx");


            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

        [HttpPost]
        public IActionResult DownloadCheckFile([FromBody]object model)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            var jsonString = JsonConvert.SerializeObject(model);
            m_ExcelPartial ViewModel = JsonConvert.DeserializeObject<m_ExcelPartial>(jsonString);
            //取得範例
            MemoryStream stream = Helper.ExportExcelSample(ViewModel.StackupDetalList);

            string FilePath = FileService.SaveAndGetExcelPath(stream);

            string FileName = Path.GetFileName(FilePath);

            return Json(FileName);
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            //取得範例
            MemoryStream stream = FileService.DownloadFile(fileName);


            string sFileName = HttpUtility.UrlEncode("CNS" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");


            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
    }
}