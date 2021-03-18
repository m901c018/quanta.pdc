using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);
            WorkService WorkService = new WorkService(_context, UserInfo.User);
            m_AssignPartial model = new m_AssignPartial();

            model.MemberID = UserInfo.User.MemberID.ToString();
            model.PrivilegeList = UserInfo.PrivilegeList;
            model.m_FormStage = FormEnum.GetAssign_Form_StageDic();
            //model.m_ProcessorList = authenticationService.GetAccountList(MemberEnum.Role.PDC_Processor).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.UserEngName + "(" + x.UserName + ")", Value = x.MemberID.ToString() }).ToList();
            model.m_ProcessorList = privilegeService.GetProcessorList();
            model.m_CompCodeList = authenticationService.GetAccountList().GroupBy(x=>x.CompCode).Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key, Value = x.Key }).ToList();
            model.m_DeptCodeList = authenticationService.GetAccountList().GroupBy(x => new { x.BUCode,x.BUName}).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key.BUName, Value = x.Key.BUCode }).ToList();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            bool? IsMB = null;
            if (model.PrivilegeList.Where(x => x.RoleID == MemberEnum.Role.PDC_Designator).Any())
                IsMB = model.PrivilegeList.Where(x => x.RoleID == MemberEnum.Role.PDC_Designator).FirstOrDefault().IsMB;

            model.vw_FormQueryList = WorkService.GetFilterFormList(new QueryParam() { FormStage = FormEnum.Form_Stage.Apply,IsMB = IsMB });
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
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);
            WorkService WorkService = new WorkService(_context, UserInfo.User);

            model.MemberID = UserInfo.User.MemberID.ToString();
            model.PrivilegeList = UserInfo.PrivilegeList;
            model.m_FormStage = FormEnum.GetAssign_Form_StageDic();
            model.m_ProcessorList = privilegeService.GetProcessorList();
            model.m_CompCodeList = authenticationService.GetAccountList().GroupBy(x => x.CompCode).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key, Value = x.Key }).ToList();
            model.m_DeptCodeList = authenticationService.GetAccountList().GroupBy(x => new { x.BUCode, x.BUName }).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = x.Key.BUName, Value = x.Key.BUCode }).ToList();

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);

            model.vw_FormQueryList = WorkService.GetFilterFormList(model.QueryParam);

            return View(model);
        }

        [HttpGet]
        [ActionCheck]
        public IActionResult WorkArea()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            WorkService WorkService = new WorkService(_context, UserInfo.User);
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);
            m_WorkAreaPartial model = new m_WorkAreaPartial();

            model.m_FormStage = FormEnum.GetWorkArea_Form_StageDic();
            model.QueryParam.PDC_Member = UserInfo.User.MemberID.ToString();
            model.QueryParam.CreatorName = UserInfo.User.UserEngName + "(" + UserInfo.User.UserName + ")";
            model.QueryParam.Form_Status = FormEnum.Form_Status.Work;
            model.m_ProcessorList = privilegeService.GetProcessorList();
            model.IsAdmin = UserInfo.PrivilegeList.Where(x => x.RoleID == MemberEnum.Role.PDC_Administrator).Any();

            model.vw_FormQueryList = WorkService.GetFilterWorkFormList(model.QueryParam);

            return View(model);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult WorkArea(m_WorkAreaPartial model)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);
            WorkService WorkService = new WorkService(_context, UserInfo.User);

            model.m_FormStage = FormEnum.GetWorkArea_Form_StageDic();
            if(string.IsNullOrWhiteSpace(model.QueryParam.PDC_Member))
                model.QueryParam.PDC_Member = UserInfo.User.MemberID.ToString();

            model.QueryParam.CreatorName = UserInfo.User.UserEngName + "(" + UserInfo.User.UserName + ")";
            model.m_ProcessorList = privilegeService.GetProcessorList();
            model.IsAdmin = UserInfo.PrivilegeList.Where(x => x.RoleID == MemberEnum.Role.PDC_Administrator).Any();

            model.vw_FormQueryList = WorkService.GetFilterWorkFormList(model.QueryParam);

            return View(model);
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

            m_WorkDetailPartial model = new m_WorkDetailPartial();

            model.vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = AppliedFormNo }).FirstOrDefault();
            model.m_PDC_Form = Form;

            List<PDC_Parameter> data = parameterService.GetParameterList("PCBTypeList");
            model.PCBTypeList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBType").FirstOrDefault().ParameterID);
            model.PCBLayoutStatusList = parameterService.GetSelectList(data.Where(x => x.ParameterValue == "PCBLayoutStatus").FirstOrDefault().ParameterID);
            model.ReleaseResultList = parameterService.GetParameterList(data.Where(x => x.ParameterValue == "ReleaseResult").FirstOrDefault().ParameterID);
            model.RejectResultList = parameterService.GetParameterList(data.Where(x => x.ParameterValue == "RejectResult").FirstOrDefault().ParameterID);

            model.m_ReleaseWorkHour = parameterService.GetParameterOne("ConfigurationRelease");
            model.m_RejectWorkHour = parameterService.GetParameterOne("ConfigurationReject");
            model.Member = UserInfo.User;

            model.m_ConfigurationWorkDetail = parameterService.GetParameterOne("ConfigurationWorkDetail");
            model.m_ConfigurationWorkDetailFile = fileService.GetFileList("ConfigurationWorkDetail", model.m_ConfigurationWorkDetail.ParameterID).FirstOrDefault();

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

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadFile(IFormFile file, string Extension, string GUID, string type, long OldFileID = 0)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            m_ExcelPartial model = new m_ExcelPartial();
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);


            string FileCheckError = string.Empty;
            //驗證是否上傳正確檔案
            if (!string.IsNullOrWhiteSpace(Extension) && !Path.GetExtension(file.FileName).Contains(Extension))
            {
                if(!Path.GetExtension(file.FileName).Contains(".dcf") && !Path.GetExtension(file.FileName).Contains(".dcfx"))
                FileCheckError = "請上傳.dcf/.dcfx檔案";
            }
            
            if (!string.IsNullOrWhiteSpace(FileCheckError))
            {
                return Json(new { status = 400, ErrorMessage = FileCheckError });
            }

            string FilePath = string.Empty;
            string ErrorMsg = string.Empty;
            PDC_File File = new PDC_File();

            if (!fileService.FileAdd(file, "FormStage", out File, "Temp", 0, GUID))
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
            else
            {
                //判斷是否有暫存檔案，有則刪除
                if (OldFileID != 0)
                {
                    PDC_File OldFile = fileService.GetFileOne(OldFileID);
                    fileService.FileRemove(OldFileID);
                }

                return Json(File);
            }
        }

        [ActionCheck]
        public IActionResult WorkRelease(Int64 DcfFileID, decimal ReleaseHour, string ReleaseOtherFileArray, string Result,Int64 FormID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            List<Int64> AllFileList = string.IsNullOrWhiteSpace(ReleaseOtherFileArray) ? new List<long>() : ReleaseOtherFileArray.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
            AllFileList.Add(DcfFileID);

            string AllErrorMsg = string.Empty;

            string ErrorMsg = string.Empty;
            PDC_Form Form = formService.GetFormOne(FormID);

            vw_FormQuery vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = Form.AppliedFormNo }).FirstOrDefault();

            if(vw_FormQuery.Stage.Value == FormEnum.Form_Stage.Assign)
            {
                if(vw_FormQuery.PDC_Member != UserInfo.User.MemberID.ToString())
                {
                    return Json(new { status = 401, ErrorMessage = "該申請單負責人已更換" });
                }
            }
            else
            {
                return Json(new { status = 401, ErrorMessage = "該申請單已抽單" });
            }

            long StageLogID = 0;
            if (formService.AddForm_StageLog(Form.FormID, FormEnum.Form_Stage.Release, Result, UserInfo.User.MemberID.ToString(), out StageLogID, ref ErrorMsg, ReleaseHour))
            {
                if(!fileService.UpdateFileSource(StageLogID, AllFileList, ref ErrorMsg))
                {
                    AllErrorMsg += "單號:" + Form.AppliedFormNo + "，" + ErrorMsg + "\n";
                    formService.DeleteForm_StageLog(StageLogID);
                }
            }
            else
            {
                AllErrorMsg += "單號:" + Form.AppliedFormNo + "，" + ErrorMsg + "\n";
            }
           

            if (!string.IsNullOrEmpty(AllErrorMsg))
            {
                return Json(new { status = 400, ErrorMessage = AllErrorMsg });
            }
            else
            {
                return Json(new { status = 0 });

            }
        }

        [ActionCheck]
        public IActionResult WorkReject(decimal ReleaseHour, string ReleaseOtherFileArray, string Result, Int64 FormID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            List<Int64> AllFileList = string.IsNullOrWhiteSpace(ReleaseOtherFileArray) ? null : ReleaseOtherFileArray.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();

            string AllErrorMsg = string.Empty;

            string ErrorMsg = string.Empty;
            PDC_Form Form = formService.GetFormOne(FormID);

            vw_FormQuery vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = Form.AppliedFormNo }).FirstOrDefault();

            if (vw_FormQuery.Stage.Value == FormEnum.Form_Stage.Assign)
            {
                if (vw_FormQuery.PDC_Member != UserInfo.User.MemberID.ToString())
                {
                    return Json(new { status = 401, ErrorMessage = "該申請單負責人已更換" });
                }
            }
            else
            {
                return Json(new { status = 401, ErrorMessage = "該申請單已抽單" });
            }

            long StageLogID = 0;
            if (formService.AddForm_StageLog(Form.FormID, FormEnum.Form_Stage.Reject, Result, UserInfo.User.MemberID.ToString(), out StageLogID, ref ErrorMsg, ReleaseHour))
            {
                if (AllFileList != null && !fileService.UpdateFileSource(StageLogID, AllFileList, ref ErrorMsg))
                {
                    AllErrorMsg += "單號:" + Form.AppliedFormNo + "，" + ErrorMsg + "\n";
                    formService.DeleteForm_StageLog(StageLogID);
                }
            }
            else
            {
                AllErrorMsg += "單號:" + Form.AppliedFormNo + "，" + ErrorMsg + "\n";
            }


            if (!string.IsNullOrEmpty(AllErrorMsg))
            {
                return Json(new { status = 400, ErrorMessage = AllErrorMsg });
            }
            else
            {
                return Json(new { status = 0 });

            }
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

                vw_FormQuery vw_FormQuery = formService.GetFilterFormList(new QueryParam() { AppliedFormNo = Form.AppliedFormNo }).FirstOrDefault();

                if (vw_FormQuery.Stage.Value == FormEnum.Form_Stage.Assign)
                {
                    if (vw_FormQuery.PDC_Member != UserInfo.User.MemberID.ToString())
                    {
                        return Json(new { status = 400, ErrorMessage = "該申請單負責人已更換" });
                    }
                }
                
                if(vw_FormQuery.Stage.Value == FormEnum.Form_Stage.End)
                {
                    return Json(new { status = 400, ErrorMessage = "該申請單已抽單" });
                }

                CreaterID = Form.Creator;
                long StageLogID = 0;
                if (formService.AddForm_StageLog(Form.FormID, IsAssign ? FormEnum.Form_Stage.Assign : FormEnum.Form_Stage.Reject, Result,PDC_Member, out StageLogID, ref ErrorMsg))
                {
                    if(file != null)
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

        [ActionCheck]
        public IActionResult DownloadFileZip(Int64 FormID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            FormService formService = new FormService(_hostingEnvironment, _context, UserInfo.User);

            PDC_Form pDC_Form = formService.GetFormOne(FormID);
            List<PDC_File> ALLFileList = new List<PDC_File>();

            ALLFileList.Add(fileService.GetFileOne(FormID, "FormApplyBRD"));
            ALLFileList.Add(fileService.GetFileOne(FormID, "FormApplyExcel"));

            ALLFileList.Add(fileService.GetFileOne(FormID, "FormApplypstchip"));
            ALLFileList.Add(fileService.GetFileOne(FormID, "FormApplypstxnet"));
            ALLFileList.Add(fileService.GetFileOne(FormID, "FormApplypstxprt"));
            List<PDC_File> OtherFileList = fileService.GetFileList("FormApplyOther", FormID);

            if (OtherFileList != null)
                ALLFileList.AddRange(OtherFileList);
            //儲存名稱
            string saveFileName = pDC_Form.AppliedFormNo + ".zip";
            FileStream sFile;
            byte[] toBytes;
            using (var ms = new MemoryStream())
            {
                using (var archive = new System.IO.Compression.ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach(PDC_File item in ALLFileList)
                    {
                        sFile = new FileStream(_hostingEnvironment.WebRootPath + "\\FileUpload\\" + item.FileFullName, FileMode.Open, FileAccess.Read);
                        toBytes = new byte[sFile.Length];
                        sFile.Read(toBytes, 0, toBytes.Length);

                        // 設定當前流的位置為流的開始 
                        sFile.Seek(0, SeekOrigin.Begin);

                        var zipEntry = archive.CreateEntry(Path.GetFileName(item.FileName), CompressionLevel.Fastest);
                        using (var zipStream = zipEntry.Open())
                        {
                            zipStream.Write(toBytes, 0, toBytes.Length);
                        }
                    }
                }
                return File(ms.ToArray(), "application/zip", saveFileName);
            }

            //string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            //string zipFileName = Path.Combine(rootPath, "123.txt");

            //using (var fs = new FileStream(saveFileName, FileMode.OpenOrCreate))
            //{
            //    using (ZipArchive zipArchive = new ZipArchive(fs, ZipArchiveMode.Create))
            //    {
            //        string fileName = Path.GetFileName(zipFileName);

            //        var zipArchiveEntry = zipArchive.CreateEntry(fileName);
            //        using (var zipStream = zipArchiveEntry.Open())
            //        {
            //            byte[] bytes = File.ReadAllBytes(zipFileName);
            //            zipStream.Write(bytes, 0, bytes.Length);
            //        }
            //    }
            //}
        }
    }
}