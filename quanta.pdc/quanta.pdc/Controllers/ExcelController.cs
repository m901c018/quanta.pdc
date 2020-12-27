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
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment); 
            //讀取檔案轉NPOI
            XSSFWorkbook templateWorkbook = Helper.LoadExcel(file);

            var sheet = templateWorkbook.GetSheet("Stackup");

            var style = sheet.GetRow(4).GetCell(1).CellStyle;

            //把資料轉為DataTable
            List<DataTable> AllExcel = Helper.ReadExcelAsTableNPOI(templateWorkbook);
            //設定標題、欄位style
            ICellStyle headerStyle = Helper.CreateCellStyle("標楷體", 12, HorizontalAlignment.Center, NPOI.HSSF.Util.HSSFColor.Blue.Index);
            ICellStyle DataStyle = Helper.CreateCellStyle("Arial Unicode MS", 12, HorizontalAlignment.Center);
            //把Datatable轉為Excel
            MemoryStream stream = Helper.ExportExcelStream(AllExcel, headerStyle, DataStyle);

            string sFileName = HttpUtility.UrlEncode("CustomerExport.xlsx");

            #region //將NPOI的Excel轉為pdf
            //sFileName = HttpUtility.UrlEncode("QuotationExport.pdf");
            //workbook.Write(ms);
            //ms.Position = 0;

            //Workbook workbook1 = new Workbook(ms);
            //MemoryStream _WeeklyReportPDF = new MemoryStream();
            //workbook1.Save(_WeeklyReportPDF, SaveFormat.Pdf);
            //_WeeklyReportPDF.Position = 0;
            //ms.Close();

            //return File(_WeeklyReportPDF.ToArray(), "application/vnd.ms-pdf", sFileName); 
            #endregion

            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
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