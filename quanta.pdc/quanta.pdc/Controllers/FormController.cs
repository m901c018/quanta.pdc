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
using cns.Services.App;
using cns.Services.Helper;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
        [ActionCheck]
        public IActionResult FormApply()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            m_FormPartial model = new m_FormPartial();
            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.Member = UserInfo.User;
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);
            model.FormApplyResultList = parameterService.GetParameterList(data.Where(x => x.ParameterValue == "FormApplyResult").FirstOrDefault().ParameterID);

            model.m_PDC_Form.IsMB = true;
            model.m_PDC_Form.ApplierID = UserInfo.User.MemberID.ToString();
            model.m_PDC_Form.BUCode = UserInfo.User.BUName;
            model.m_PDC_Form.CompCode = UserInfo.User.CompCode;

            model.GUID = Guid.NewGuid().ToString("N");
            

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionCheck]
        public IActionResult FormApply(m_FormPartial model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            string ErrorMsg = string.Empty;
            long FormID = 0;
            model.m_PDC_Form.AppliedFormNo = "CN";
            model.m_PDC_Form.ApplierID = UserInfo.User.MemberID.ToString();
            model.m_PDC_Form.BUCode = UserInfo.User.BUCode;
            model.m_PDC_Form.CompCode = UserInfo.User.CompCode;
            model.m_PDC_Form.FormStatusCode = model.IsSendApply ? Services.Enum.FormEnum.Form_Status.Apply : Services.Enum.FormEnum.Form_Status.NoApply;
            model.m_PDC_Form.FormStatus = model.IsSendApply ? "未派單" : "未送件";
            model.m_PDC_Form.ApplyDate = model.IsSendApply ? DateTime.Now : default(DateTime);

            List<PDC_File> ALLFileList = new List<PDC_File>();
            ALLFileList.Add(model.m_BRDFile);
            ALLFileList.Add(model.m_ExcelFile);
            ALLFileList.Add(model.m_pstchipFile);
            ALLFileList.Add(model.m_pstxnetFile);
            ALLFileList.Add(model.m_pstxprtFile);
            if(model.m_OtherFileList != null)
                ALLFileList.AddRange(model.m_OtherFileList);
            

            if (formService.AddForm(model.m_PDC_Form, ALLFileList, ref ErrorMsg,out FormID))
            {
                long SourceID = FormID;
                string folder = string.Empty;
                //是否直接送出申請
                if (model.IsSendApply)
                {
                    formService.AddForm_StageLog(FormID, Services.Enum.FormEnum.Form_Stage.Apply, model.m_PDC_Form.Result ?? "New", ALLFileList, out SourceID, ref ErrorMsg);
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
        [ActionCheck]
        public IActionResult FormApplyEdit(long FormID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler excelHepler = new ExcelHepler(_hostingEnvironment);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            m_FormPartial model = new m_FormPartial();

            model.m_PDC_Form = formService.GetFormOne(FormID);
            model.vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = model.m_PDC_Form.AppliedFormNo }).FirstOrDefault();
            model.PDC_Form_StageLogList = formService.GetForm_StageLogList(FormID);
            model.PDC_Form_StageLogFileList = fileService.GetForm_StageFileList(model.PDC_Form_StageLogList);
            model.m_PDC_Form_StageLog = model.PDC_Form_StageLogList.OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");

            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);
            model.FormApplyResultList = parameterService.GetParameterList(data.Where(x => x.ParameterValue == "FormApplyResult").FirstOrDefault().ParameterID);

            long sourceID = FormID;
            if (model.m_PDC_Form.FormStatusCode == Services.Enum.FormEnum.Form_Status.NoApply || model.m_PDC_Form.FormStatusCode == Services.Enum.FormEnum.Form_Status.Reject)
            {
                model.IsSendApply = false;
            }
            else
            {
                model.IsSendApply = true;
            }

            if (model.m_PDC_Form.Creator == UserInfo.User.MemberID.ToString().ToUpper())
            {
                model.IsReadOnly = false;
            }
            else
            {
                model.IsReadOnly = true;
            }

            model.m_BRDFile = fileService.GetFileOne(FormID, "FormApplyBRD");
            model.m_ExcelFile = fileService.GetFileOne(FormID, "FormApplyExcel");
            model.m_OtherFileList = fileService.GetFileList("FormApplyOther", FormID);
            model.m_pstchipFile = fileService.GetFileOne(FormID, "FormApplypstchip");
            model.m_pstxnetFile = fileService.GetFileOne(FormID, "FormApplypstxnet");
            model.m_pstxprtFile = fileService.GetFileOne(FormID, "FormApplypstxprt");

            //取得範例
            Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + model.m_ExcelFile.FileFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
           
            stream.Position = 0; // <-- Add this, to make it work
            //IFormFile File = new FormFile(stream,)
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
        [ActionCheck]
        public IActionResult FormApplyEdit(m_FormPartial model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler excelHepler = new ExcelHepler(_hostingEnvironment);
            string ErrorMsg = string.Empty;
            model.m_PDC_Form.ApplyDate = model.IsSendApply ? DateTime.Now : default(DateTime);
            model.m_PDC_Form.FormStatusCode = model.IsSendApply ? Services.Enum.FormEnum.Form_Status.Apply : Services.Enum.FormEnum.Form_Status.NoApply;
            model.m_PDC_Form.FormStatus = model.IsSendApply ? "未派單" : "未送件";
            model.m_PDC_Form.ApplyDate = model.IsSendApply ? DateTime.Now : default(DateTime);

            long SourceID = model.m_PDC_Form.FormID;

            List<PDC_File> ALLFileList = new List<PDC_File>();
            ALLFileList.Add(model.m_BRDFile);
            ALLFileList.Add(model.m_ExcelFile);
            ALLFileList.Add(model.m_pstchipFile);
            ALLFileList.Add(model.m_pstxnetFile);
            ALLFileList.Add(model.m_pstxprtFile);
            if(model.m_OtherFileList != null)
                ALLFileList.AddRange(model.m_OtherFileList);

            if (formService.UpdateForm(model.m_PDC_Form, ALLFileList, ref ErrorMsg))
            {
                //是否直接送出申請
                if (model.IsSendApply)
                {
                    formService.AddForm_StageLog(model.m_PDC_Form.FormID, Services.Enum.FormEnum.Form_Stage.Apply, model.m_PDC_Form.Result ?? "New", ALLFileList, out SourceID, ref ErrorMsg);
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
        [ActionCheck]
        public IActionResult UploadFile(IFormFile file,string Extension,string GUID,string type,long OldFileID = 0)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            m_ExcelPartial model = new m_ExcelPartial();
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            Dictionary<string, string> functionName = new Dictionary<string, string>();
            functionName.Add("UplpadBRDFile", "FormApplyBRD");
            functionName.Add("UplpadExcelFile", "FormApplyExcel");
            functionName.Add("UplpadpstchipFile", "FormApplypstchip");
            functionName.Add("UplpadpstxnetFile", "FormApplypstxnet");
            functionName.Add("UplpadpstxprtFile", "FormApplypstxprt");
            functionName.Add("UplpadOtherFile", "FormApplyOther");
            Dictionary<string, string> CheckFileName = new Dictionary<string, string>();
            CheckFileName.Add("UplpadBRDFile", "");
            CheckFileName.Add("UplpadExcelFile", "");
            CheckFileName.Add("UplpadpstchipFile", "pstchip.dat");
            CheckFileName.Add("UplpadpstxnetFile", "pstxnet.dat");
            CheckFileName.Add("UplpadpstxprtFile", "pstxprt.dat");
            CheckFileName.Add("UplpadOtherFile", "");

            string FileCheckError = string.Empty;
            //驗證是否上傳正確檔案
            if (!string.IsNullOrWhiteSpace(Extension) && !Path.GetExtension(file.FileName).Contains(Extension))
            {
                FileCheckError = "請上傳" + Extension + "檔案";
            }
            if (!string.IsNullOrWhiteSpace(CheckFileName[type]) && !Path.GetFileName(file.FileName).Contains(CheckFileName[type]))
            {
                FileCheckError += string.IsNullOrWhiteSpace(FileCheckError) ? "檔名需為 :" + CheckFileName[type] : "，檔名需為 :" + CheckFileName[type];
            }
            if (!string.IsNullOrWhiteSpace(FileCheckError))
            {
                return Json(new { status = 400, ErrorMessage = FileCheckError });
            }

            string FilePath = string.Empty;
            string ErrorMsg = string.Empty;
            PDC_File File = new PDC_File();

            if (!fileService.FileAdd(file, functionName[type], out File, "Temp", 0, GUID)) 
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
            else
            {
                //判斷是否有暫存檔案，有則刪除
                if(OldFileID != 0)
                {
                    PDC_File OldFile = fileService.GetFileOne(OldFileID);
                    fileService.FileRemove(OldFileID);
                }

                if(type == "UplpadExcelFile")
                {
                    Stream stream = file.OpenReadStream();
                    //轉NPOI類型
                    XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

                    ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");
                    stream.Dispose();
                    stream.Close();
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
        [ActionCheck]
        public IActionResult FileDelete(Int64 FileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

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
        [ActionCheck]
        public IActionResult CloseApply(Int64 FormID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
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

        [HttpPost]
        [ActionCheck]
        public IActionResult FormExcelEdit(Int64 FileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            PDC_File File = FileService.GetFileOne(FileID);

            
            //取得範例
            Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName, FileMode.Open, FileAccess.Read);

            stream.Position = 0; // <-- Add this, to make it work
            try
            {
                //IFormFile File = new FormFile(stream,)
                //轉NPOI類型
                XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

                ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");

                //資料轉為Datatable
                DataTable sheetDt = Helper.GetDataTableFromExcel(xSSFSheet, true);

                Helper.ExcelStackupCheck(sheetDt);

                //Session紀錄
                HttpContext.Session.SetObjectAsJson("SessionExcelData", sheetDt);
                HttpContext.Session.SetString("SessionFileID", FileID.ToString());
                stream.Close();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                stream.Close();
                stream.Dispose();
            }
            

            return Json(new { status = 0, ErrorMessage = "" });
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult ReloadExcelFile(Int64 FileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            PDC_File File = FileService.GetFileOne(FileID);


            //取得範例
            Stream stream = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + File.FileFullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            stream.Position = 0; // <-- Add this, to make it work
            try
            {
                //IFormFile File = new FormFile(stream,)
                //轉NPOI類型
                XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

                ISheet xSSFSheet = ExcelFile.GetSheet("Stackup");

                //資料轉為Datatable
                DataTable sheetDt = Helper.GetDataTableFromExcel(xSSFSheet, true);

                stream.Close();
                stream.Dispose();

                return Json(new { status = 0, Excel = sheetDt, File = File });
            }
            catch (Exception ex)
            {
                stream.Close();
                stream.Dispose();
            }


            return Json(new { status = 400, ErrorMessage = "讀取檔案失敗" });
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult Download(Int64 FileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            PDC_File PDC_File = FileService.GetFileOne(FileID);
            bool IsTemp = false;
            IsTemp = PDC_File.SourceID != 0 ? false : true;
            //取得檔案
            MemoryStream stream = FileService.DownloadFile(PDC_File.FileFullName, IsTemp);


            string sFileName = HttpUtility.UrlEncode(PDC_File.FileName);


            return File(stream.ToArray(), FileHelper.GetContentType(Path.GetExtension(PDC_File.FileFullName)), sFileName);
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult FormQuery()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            m_FormQueryPartial model = new m_FormQueryPartial();
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            model.QueryParam.BUCode = UserInfo.User.BUCode;
            model.QueryParam.BUName = UserInfo.User.BUName;
            model.QueryParam.CompCode = UserInfo.User.CompCode;
            model.QueryParam.CreatorName = UserInfo.User.UserEngName;

            model.PicDescriptionList = parameterService.GetParameterList("PCBLayoutConstraint_query_Col");
            model.PicDescriptionFileList = FileService.GetParameterFileList(model.PicDescriptionList, "Configuration_PCBFile");

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");

            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            return View(model);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult FormQuery([FromForm]m_FormQueryPartial m_FormPartial)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_context, UserInfo.User);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            m_FormPartial.QueryParam.BUCode = UserInfo.User.BUCode;
            m_FormPartial.QueryParam.BUName = UserInfo.User.BUName;
            m_FormPartial.QueryParam.CompCode = UserInfo.User.CompCode;
            m_FormPartial.QueryParam.CreatorName = UserInfo.User.UserEngName;
            m_FormPartial.QueryParam.ApplierID = UserInfo.User.MemberID.ToString();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");

            m_FormPartial.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            m_FormPartial.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            m_FormPartial.vw_FormQueryList = formService.GetFilterFormList(m_FormPartial.QueryParam);

            return View(m_FormPartial);
        }
    }
}