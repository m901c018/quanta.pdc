using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using cns.Data;
using cns.Models;
using cns.Services;
using cns.Services.Helper;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace cns.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IHostingEnvironment _hostingEnvironment;

        public FormController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult FormApply()
        {
            m_FormPartial model = new m_FormPartial();
            model.m_PDC_Form.IsMB = true;
            model.GUID = Guid.NewGuid().ToString("N");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormApply(m_FormPartial model)
        {
            FormService formService = new FormService(_hostingEnvironment, _context);
            FileService fileService = new FileService(_hostingEnvironment, _context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;
            long FormID = 0;
            model.m_PDC_Form.AppliedFormNo = "CN";
            model.m_PDC_Form.ApplierID = "TestUser";
            model.m_PDC_Form.FormStatus = model.IsSendApply ? "未派單" : "未送件";
            model.m_PDC_Form.ApplyDate = model.IsSendApply ? DateTime.Now : default(DateTime); ;
            model.m_PDC_Form.Creator = userId;
            model.m_PDC_Form.CreatorName = userName;
            model.m_PDC_Form.CreatorDate = DateTime.Now;

            List<PDC_File> ALLFileList = new List<PDC_File>();
            ALLFileList.Add(model.m_BRDFile);
            ALLFileList.Add(model.m_ExcelFile);
            ALLFileList.Add(model.m_pstchipFile);
            ALLFileList.Add(model.m_pstxnetFile);
            ALLFileList.Add(model.m_pstxprtFile);
            ALLFileList.AddRange(model.m_OtherFileList);
            

            if (formService.AddForm(model.m_PDC_Form, ALLFileList, ref ErrorMsg,out FormID))
            {
                long SourceID = FormID;
                string folder = string.Empty;
                //是否直接送出申請
                if (model.IsSendApply)
                {
                    formService.AddForm_StageLog(FormID, Services.Enum.FormEnum.Form_Stage.Apply, model.Result ?? "New", ALLFileList, out SourceID, ref ErrorMsg);
                }

                return RedirectToAction("FormApplyEdit", new { FormID = FormID });
                //if (formService.UpdateFormFile(SourceID, FileBRD, FileExcel, Filepstchip, Filepstxnet, Filepstxprt, FileOther, ref ErrorMsg))
                //{
                //    return RedirectToAction("FormApplyEdit", new { FormID = FormID });
                //}
                //else
                //{
                //    TempData["TempMsg"] = ErrorMsg;
                //    return View(model);
                //}

              
            }

            TempData["TempMsg"] = "申請單新增失敗";
            return View(model);
        }


        [HttpGet]
        public IActionResult FormApplyEdit(long FormID)
        {
            FormService formService = new FormService(_context);
            FileService fileService = new FileService(_hostingEnvironment, _context);
            ExcelHepler excelHepler = new ExcelHepler(_hostingEnvironment);
            m_FormPartial model = new m_FormPartial();

            model.m_PDC_Form = formService.GetFormOne(FormID);
            model.PDC_Form_StageLogList = formService.GetForm_StageLogList(FormID);
            model.PDC_Form_StageLogFileList = fileService.GetForm_StageFileList(model.PDC_Form_StageLogList);
            model.m_PDC_Form_StageLog = model.PDC_Form_StageLogList.OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            long sourceID = FormID;
            if (model.m_PDC_Form.FormStatus != "未送件")
            {
                model.IsSendApply = true;
            }
            else
            {
                model.IsSendApply = false;
            }

            model.m_BRDFile = fileService.GetFileOne(FormID, "FormApplyBRD");
            model.m_ExcelFile = fileService.GetFileOne(FormID, "FormApplyExcel");
            model.m_OtherFileList = fileService.GetFileList("FormApplyOther", FormID);
            model.m_pstchipFile = fileService.GetFileOne(FormID, "FormApplypstchip");
            model.m_pstxnetFile = fileService.GetFileOne(FormID, "FormApplypstxnet");
            model.m_pstxprtFile = fileService.GetFileOne(FormID, "FormApplypstxprt");

            //取得範例
            Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + model.m_ExcelFile.FileFullName, FileMode.Open, FileAccess.Read);
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
            //資料轉為Datatable
            model.ExcelDt = excelHepler.GetDataTableFromExcel(xSSFSheet, true);
            //驗證資料
            model.ErrorMsg = excelHepler.ExcelStackupCheck(model.ExcelDt);

            return View(model);
        }

        [HttpPost]
        public IActionResult FormApplyEdit(m_FormPartial model)
        {
            FormService formService = new FormService(_hostingEnvironment, _context);
            FileService fileService = new FileService(_hostingEnvironment, _context);
            ExcelHepler excelHepler = new ExcelHepler(_hostingEnvironment);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name
            string ErrorMsg = string.Empty;
            model.m_PDC_Form.ApplyDate = model.IsSendApply ? DateTime.Now : default(DateTime);
            model.m_PDC_Form.FormStatus = model.IsSendApply ? "未派單" : "未送件";
            model.m_PDC_Form.ApplyDate = DateTime.Now;
            model.m_PDC_Form.Modifyer = userId;
            model.m_PDC_Form.ModifyerName = userName;
            model.m_PDC_Form.ModifyerDate = DateTime.Now;

            long SourceID = model.m_PDC_Form.FormID;

            List<PDC_File> ALLFileList = new List<PDC_File>();
            ALLFileList.Add(model.m_BRDFile);
            ALLFileList.Add(model.m_ExcelFile);
            ALLFileList.Add(model.m_pstchipFile);
            ALLFileList.Add(model.m_pstxnetFile);
            ALLFileList.Add(model.m_pstxprtFile);
            ALLFileList.AddRange(model.m_OtherFileList);

            if (formService.UpdateForm(model.m_PDC_Form, ALLFileList, ref ErrorMsg))
            {
                //是否直接送出申請
                if (model.IsSendApply)
                {
                    formService.AddForm_StageLog(model.m_PDC_Form.FormID, Services.Enum.FormEnum.Form_Stage.Apply, model.m_Result ?? "New", ALLFileList, out SourceID, ref ErrorMsg);
                }

                return RedirectToAction("FormApplyEdit", new { FormID = model.m_PDC_Form.FormID });
            }
            else
            {
                TempData["TempMsg"] = "申請單編輯失敗";
                return View(model);
            }

        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file,string Extension,string GUID,string type,long OldFileID = 0)
        {
            m_ExcelPartial model = new m_ExcelPartial();
            FileService fileService = new FileService(_hostingEnvironment, _context);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name
            Dictionary<string, string> functionName = new Dictionary<string, string>();
            functionName.Add("UplpadBRDFile", "FormApplyBRD");
            functionName.Add("UplpadExcelFile", "FormApplyExcel");
            functionName.Add("UplpadpstchipFile", "FormApplypstchip");
            functionName.Add("UplpadpstxnetFile", "FormApplypstxnet");
            functionName.Add("UplpadpstxprtFile", "FormApplypstxprt");
            functionName.Add("UplpadOtherFile", "FormApplyOther");

            //驗證是否上傳正確檔案
            if (!string.IsNullOrWhiteSpace(Extension) && !Path.GetExtension(file.FileName).Contains(Extension))
            {
                return Json(new { status = 400, ErrorMessage = "請上傳"+ Extension + "檔案" });
            }
            string FilePath = string.Empty;
            string ErrorMsg = string.Empty;
            PDC_File File = new PDC_File();

            if (!fileService.FileAdd(file, functionName[type], userId, userName, out File, "Temp", 0, GUID)) 
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
            else
            {
                //判斷是否有暫存檔案，有則刪除
                if(OldFileID != 0)
                {
                    PDC_File OldFile = fileService.GetFileOne(OldFileID);
                    if(OldFile.SourceID == 0)
                    {
                        fileService.FileRemove(OldFileID);
                    }
                }

                if(type == "UplpadExcelFile")
                {
                    Stream stream = file.OpenReadStream();
                    //轉NPOI類型
                    XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

                    ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
                    //資料轉為Datatable
                    DataTable ExcelDt = Helper.GetDataTableFromExcel(xSSFSheet, true);

                    //驗證資料
                    model.Errmsg = Helper.ExcelStackupCheck(ExcelDt);

                    return Json(new { Excel = ExcelDt, File = File });
                }
                return Json(File);
            }
        }

        [HttpPost]
        public IActionResult FileDelete(Int64 FileID)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);

            //取得檔案
            if(!FileService.FileRemove(FileID))
            {
                return Json(new { status = 400, ErrorMessage = "刪除失敗" });
            }
            else
            {
                return Json(new { status = 0, ErrorMessage = "" });
            }
        }

        [HttpPost]
        public IActionResult CloseApply(Int64 FormID)
        {
            FormService formService = new FormService(_hostingEnvironment, _context);
            string ErrorMsg = "";
            //取得檔案
            if (!formService.CloseFormApply(FormID,ref ErrorMsg))
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
            else
            {
                return Json(new { status = 0, ErrorMessage = "" });
            }
        }

        [HttpGet]
        public IActionResult Download(Int64 FileID)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);

            PDC_File PDC_File = FileService.GetFileOne(FileID);
            bool IsTemp = false;
            IsTemp = PDC_File.SourceID != 0 ? false : true;
            //取得檔案
            MemoryStream stream = FileService.DownloadFile(PDC_File.FileFullName, IsTemp);


            string sFileName = HttpUtility.UrlEncode(PDC_File.FileName);


            return File(stream.ToArray(), FileHelper.GetContentType(Path.GetExtension(PDC_File.FileFullName)), sFileName);
        }

        [HttpGet]
        public IActionResult FormQuery()
        {
            m_FormPartial model = new m_FormPartial();
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            //暫時寫死，後續帶對方資料
            model.QueryParam.BUCode = "BU_Test";
            model.QueryParam.CompCode = "QCI";
            model.QueryParam.CreatorName = userName;

            return View(model);
        }

        [HttpPost]
        public IActionResult FormQuery([FromForm]m_FormPartial m_FormPartial)
        {
            FormService formService = new FormService(_context);
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)

            m_FormPartial.QueryParam.BUCode = "BU_Test";
            m_FormPartial.QueryParam.CompCode = "QCI";
            m_FormPartial.QueryParam.CreatorName = userName;
            m_FormPartial.QueryParam.ApplierID = "TestUser";

          

            m_FormPartial.vw_FormQueryList = formService.GetFilterFormList(m_FormPartial.QueryParam);

            return View(m_FormPartial);
        }
    }
}