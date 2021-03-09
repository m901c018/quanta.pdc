using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.Services;
using cns.Services.App;
using cns.Services.Enum;
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
    public class WorkController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;

        public WorkController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult Assign()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            m_AssignPartial model = new m_AssignPartial();

            model.MemberID = UserInfo.User.MemberID.ToString();
            model.m_FormStage = FormEnum.GetAssign_Form_StageDic();
            model.m_ProcessorList = authenticationService.GetAccountList(MemberEnum.Role.PDC_Processor).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.UserEngName, Value = x.MemberID.ToString() }).ToList();
            model.m_CompCodeList = authenticationService.GetAccountList().GroupBy(x=>x.CompCode).Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key, Value = x.Key }).ToList();
            model.m_DeptCodeList = authenticationService.GetAccountList().GroupBy(x => new { x.BUCode,x.BUName}).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key.BUName, Value = x.Key.BUCode }).ToList();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            return View(model);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult Assign(m_AssignPartial model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            WorkService formService = new WorkService(_context, UserInfo.User);

            model.MemberID = UserInfo.User.MemberID.ToString();
            model.m_FormStage = FormEnum.GetAssign_Form_StageDic();
            model.m_ProcessorList = authenticationService.GetAccountList(MemberEnum.Role.PDC_Processor).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.UserEngName, Value = x.MemberID.ToString() }).ToList();
            model.m_CompCodeList = authenticationService.GetAccountList().GroupBy(x => x.CompCode).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key, Value = x.Key }).ToList();
            model.m_DeptCodeList = authenticationService.GetAccountList().GroupBy(x => new { x.BUCode, x.BUName }).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key.BUName, Value = x.Key.BUCode }).ToList();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            model.vw_FormQueryList = formService.GetFilterFormList(model.QueryParam);

            return View(model);
        }

        [ActionCheck]
        public IActionResult WorkArea()
        {
            return View();
        }

        [ActionCheck]
        public IActionResult WorkDetail(string AppliedFormNo)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler excelHepler = new ExcelHepler(_hostingEnvironment);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            PDC_Form Form = formService.GetFormOne(AppliedFormNo);

            m_FormPartial model = new m_FormPartial();

            model.vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = AppliedFormNo }).FirstOrDefault();
            model.m_PDC_Form = Form;
            model.PDC_Form_StageLogList = formService.GetForm_StageLogList(Form.FormID);
            model.PDC_Form_StageLogFileList = fileService.GetForm_StageFileList(model.PDC_Form_StageLogList);
            model.m_PDC_Form_StageLog = model.PDC_Form_StageLogList.OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");

            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);
            model.FormApplyResultList = parameterService.GetParameterList(data.Where(x => x.ParameterValue == "FormApplyResult").FirstOrDefault().ParameterID);

            if (model.vw_FormQuery.PDC_Member == UserInfo.User.MemberID.ToString() && model.vw_FormQuery.Stage == FormEnum.Form_Stage.Assign)
            {
                model.IsReadOnly = false;
            }
            else
            {
                model.IsReadOnly = true;
            }

            model.m_BRDFile = fileService.GetFileOne(Form.FormID, "FormApplyBRD");
            model.m_ExcelFile = fileService.GetFileOne(Form.FormID, "FormApplyExcel");
            model.m_OtherFileList = fileService.GetFileList("FormApplyOther", Form.FormID);
            model.m_pstchipFile = fileService.GetFileOne(Form.FormID, "FormApplypstchip");
            model.m_pstxnetFile = fileService.GetFileOne(Form.FormID, "FormApplypstxnet");
            model.m_pstxprtFile = fileService.GetFileOne(Form.FormID, "FormApplypstxprt");

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

        [ActionCheck]
        public IActionResult WorkDesign(string FromNoArray, bool IsAssign,string PDC_Member, string Result, IFormFile file)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            List<string> FromNoList = FromNoArray.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            string AllErrorMsg = string.Empty;
            string CreaterID = string.Empty;

            foreach (string item in FromNoList)
            {
                string ErrorMsg = string.Empty;
                PDC_Form Form = formService.GetFormOne(item);
                CreaterID = Form.Creator;
                long StageLogID = 0;
                if (formService.AddForm_StageLog(Form.FormID, IsAssign ? FormEnum.Form_Stage.Assign : FormEnum.Form_Stage.Reject, Result,PDC_Member, out StageLogID, ref ErrorMsg))
                {
                    if(file.Length > 0)
                    {
                        PDC_File PDC_File = new PDC_File();
                        fileService.FileAdd(file, "FormStage", out PDC_File, "FileUpload", StageLogID);
                    }
                }
                else
                {
                    AllErrorMsg += "單號:" + item + "，" + ErrorMsg + "\n";
                }
            }

            if(!string.IsNullOrEmpty(AllErrorMsg))
            {
                return Json(new { status = 400, ErrorMessage = AllErrorMsg });
            }
            else
            {
                return Json(new { status = 0, IsSingle = FromNoList.Count == 1 ? true : false, CreaterID = CreaterID, FormNo = FromNoList[0] });

            }
        }
    }
}