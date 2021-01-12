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

namespace cns.Controllers
{
    //[Authorize(Roles = Services.App.Pages.Excel.RoleName)]
    public class ExcelController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        //consume custom security service to get all roles
        public IActionResult Index()
        {
            return View();
        }

        public ExcelController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
        public IActionResult SaveCheckFile([FromBody] m_ExcelPartial model)
        {
            //m_ExcelPartial model = new m_ExcelPartial();
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            //存檔並返回檔案路徑
            DataTable StackupDetalDt = Helper.GetDataTableFromStackupDetail(model.StackupDetalList);
            //驗證資料
            Helper.ExcelStackupCheck(StackupDetalDt, model);

            return Json(model.Errmsg);
        }

        [HttpGet]
        public IActionResult DownloadSample()
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            //取得範例
            MemoryStream stream = Helper.ExportExcelSample();

            string sFileName = HttpUtility.UrlEncode("Sample.xlsx");


            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
    }
}