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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormApply(m_FormPartial model)
        {
            FormService formService = new FormService(_context);
            FileService fileService = new FileService(_hostingEnvironment, _context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;
            long FormID = 0;
            model.m_PDC_Form.AppliedFormNo = "CN";
            model.m_PDC_Form.ApplierID = "TestUser";
            model.m_PDC_Form.FormStatus = model.IsSendApply ? "未派單" : "未送件";
            model.m_PDC_Form.Creator = userId;
            model.m_PDC_Form.CreatorName = userName;
            model.m_PDC_Form.CreatorDate = DateTime.Now;
            if(formService.AddForm(model.m_PDC_Form, ref ErrorMsg,out FormID))
            {
                long SourceID = FormID;
                //是否直接送出申請
                if (model.IsSendApply)
                {
                    formService.AddForm_StageLog(FormID, FormService.Form_Stage.Apply, "New",out SourceID, ref ErrorMsg);
                }
                
                PDC_File FileBRD = new PDC_File();
                fileService.FileAdd(model.m_UplpadBRDFile, "FormApplyBRD", userId, userName, out FileBRD, SourceID);

                PDC_File FileExcel = new PDC_File();
                fileService.FileAdd(model.m_UplpadExcelFile, "FormApplyExcel", userId, userName, out FileExcel, SourceID);

                PDC_File Filepstchip = new PDC_File();
                fileService.FileAdd(model.m_UplpadpstchipFile, "FormApplypstchip", userId, userName, out Filepstchip, SourceID);

                PDC_File Filepstxnet = new PDC_File();
                fileService.FileAdd(model.m_UplpadpstxnetFile, "FormApplypstxnet", userId, userName, out Filepstxnet, SourceID);

                PDC_File Filepstxprt = new PDC_File();
                fileService.FileAdd(model.m_UplpadpstxprtFile, "FormApplypstxprt", userId, userName, out Filepstxprt, SourceID);

                if(model.m_UplpadOtherFile != null)
                {
                    PDC_File FileOther = new PDC_File();
                    fileService.FileAdd(model.m_UplpadOtherFile, "FormApplyOther", userId, userName, out FileOther, SourceID);
                }
                

                return RedirectToAction("FormApplyEdit", new { FormID = FormID });
            }

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

            long sourceID = FormID;
            if (model.m_PDC_Form.FormStatus != "未送件")
            {
                sourceID = model.PDC_Form_StageLogList.Where(x => x.StageName == "Apply").OrderByDescending(x => x.CreatorDate).FirstOrDefault().StageLogID;
            }

            model.m_BRDFile = fileService.GetFileOne(sourceID, "FormApplyBRD");
            model.m_ExcelFile = fileService.GetFileOne(sourceID, "FormApplyExcel");
            model.m_Other = fileService.GetFileOne(sourceID, "FormApplyOther");
            model.m_pstchipFile = fileService.GetFileOne(sourceID, "FormApplypstchip");
            model.m_pstxnetFile = fileService.GetFileOne(sourceID, "FormApplypstxnet");
            model.m_pstxprtFile = fileService.GetFileOne(sourceID, "FormApplypstxprt");

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

        [HttpGet]
        public IActionResult Download(Int64 FileID)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);

            PDC_File PDC_File = FileService.GetFileOne(FileID);
            //取得檔案
            MemoryStream stream = FileService.DownloadFile(PDC_File.FileFullName);


            string sFileName = HttpUtility.UrlEncode(PDC_File.FileName);


            return File(stream.ToArray(), FileHelper.GetContentType(Path.GetExtension(PDC_File.FileFullName)), sFileName);
        }

        [HttpGet]
        public IActionResult FormQuery()
        {
            m_FormPartial model = new m_FormPartial();
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            //暫時寫死，後續帶對方資料
            model.BUCode = "BU_Test";
            model.CompCode = "QCI";
            model.CreatorName = userName;

            return View(model);
        }

        [HttpPost]
        public IActionResult FormQuery([FromForm]m_FormPartial m_FormPartial)
        {
            FormService formService = new FormService(_context);
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)

            m_FormPartial.BUCode = "BU_Test";
            m_FormPartial.CompCode = "QCI";
            m_FormPartial.CreatorName = userName;

            m_FormPartial.m_PDC_Form.ApplierID = "TestUser";
            m_FormPartial.m_PDC_Form.AppliedFormNo = m_FormPartial.AppliedFormNo;
            m_FormPartial.m_PDC_Form.BUCode = m_FormPartial.BUCode;
            m_FormPartial.m_PDC_Form.CompCode = m_FormPartial.CompCode;
            m_FormPartial.m_PDC_Form.BoardTypeName = m_FormPartial.BoardTypeName;
            m_FormPartial.m_PDC_Form.FormStatus = m_FormPartial.FormStatus;
            m_FormPartial.m_PDC_Form.PCBLayoutStatus = m_FormPartial.PCBLayoutStatus;
            m_FormPartial.m_PDC_Form.PCBType = m_FormPartial.PCBType;
            m_FormPartial.m_PDC_Form.ProjectName = m_FormPartial.ProjectName;
            m_FormPartial.m_PDC_Form.Revision = m_FormPartial.Revision;
            m_FormPartial.m_PDC_Form.BUCode = m_FormPartial.BUCode;
            m_FormPartial.m_PDC_Form.CompCode = m_FormPartial.CompCode;
            m_FormPartial.m_PDC_Form.CreatorName = userName;

            if (m_FormPartial.SearchDate != null)
                m_FormPartial.m_PDC_Form.CreatorDate = m_FormPartial.SearchDate.Value;

            m_FormPartial.PDC_FormList = formService.GetFilterFormList(m_FormPartial.m_PDC_Form);

            return View(m_FormPartial);
        }
    }
}