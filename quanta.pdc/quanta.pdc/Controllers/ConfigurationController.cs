using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cns.Services.Helper;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.XSSF.UserModel;
using System.Runtime;
using Microsoft.AspNetCore.Identity;
using cns.Data;
using cns.Models;
using cns.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using cns.Services.App;
using static cns.Services.Enum.MemberEnum;

namespace cns.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        

        [ActionCheck]
        public IActionResult Permission()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);
            m_PrivilegePartial model = new m_PrivilegePartial();

            model.vw_ProcessQueryList = privilegeService.GetFilterPrivilegeList(null, Services.Enum.MemberEnum.Role.PDC_Processor);
            model.vw_AssignQueryList = privilegeService.GetFilterPrivilegeList(null, Services.Enum.MemberEnum.Role.PDC_Designator);
            model.vw_AdminQueryList = privilegeService.GetFilterPrivilegeList(null, Services.Enum.MemberEnum.Role.PDC_Administrator);

            return View(model);
        }

        public IActionResult DeletePrivilege(Guid PrivilegeID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);

            if(privilegeService.DeletePrivilege(PrivilegeID))
            {
                return Json(new { status = 0 });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = "刪除失敗" });
            }
        }

        public IActionResult AddPrivilege(Guid MemberID, string type, bool? IsMB, bool? IsMail)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);

            PDC_Member pDC_Member = authenticationService.GetMember(MemberID);

            Role role;
            switch (type)
            {
                case "Admin":
                    role = Role.PDC_Administrator;
                    break;
                case "Assign":
                    role = Role.PDC_Designator;
                    break;
                case "Process":
                    role = Role.PDC_Processor;
                    break;
                default:
                    role = Role.PDC_Processor;
                    break;
            }
            string ErrorMsg = string.Empty;

            if (privilegeService.AddPrivilege(MemberID, role, ref ErrorMsg, IsMB, IsMail)) 
            {
                vw_PrivilegeQuery vw_PrivilegeQuery = privilegeService.GetFilterPrivilegeList(null, role).Where(x => x.MemberID == MemberID).FirstOrDefault();
                return Json(new { status = 0, Privilege = vw_PrivilegeQuery });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
        }
        

        public IActionResult GetMember(string EmpNumber)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            AuthenticationService authenticationService = new AuthenticationService(_context);
            //string DomainwithNumber = Environment.UserDomainName +"\\\\" + Environment.UserName;

            Api_QueryUserInfo apiModel = new Api_QueryUserInfo();
            string ErrorMsg = string.Empty;
            if (ApiService.GetApiUserInfo(EmpNumber,ref apiModel, ref ErrorMsg))
            {
                if(string.IsNullOrWhiteSpace(apiModel.EMPLID))
                {
                    return Json(new { status = 400, ErrorMessage = "查無資料" });

                }
                else
                {
                    CurrentUser User = new CurrentUser();
                    if (authenticationService.GetApiAccount(apiModel, ref User))
                    {
                        return Json(new { status = 0, Member = User.User, apiModel = apiModel });
                    }
                    else
                    {
                        return Json(new { status = 400, Member = "取得資料失敗" });
                    }
                }
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
        }

        [ActionCheck]
        public IActionResult DownloadCNS()
        {
            return View();
        }
        //consume custom security service to get all roles
        [ActionCheck]
        public IActionResult Index()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            AuthenticationService authenticationService = new AuthenticationService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            var user = _userManager.GetUserId(HttpContext.User);

            //組態設定-CNS範本
            if (_context.PDC_File.Where(x => x.FunctionName == FileKey.ConfigurationSample).Any())
            {
                model.CNS_Sample = _context.PDC_File.Where(x => x.FunctionName == FileKey.ConfigurationSample).OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            }
            //組態設定-首頁連結
            model.HomeLinkList = parameterService.GetParameterList(ParameterKey.Configuration_HomeLink);
            model.HomeLinkFileList = _context.PDC_File.Where(x => x.FunctionName == ParameterKey.Configuration_HomeLink).ToList();

            //組態設定-圖文說明
            model.PCBTypeList = parameterService.GetSelectList(ParameterKey.PCBLayoutConstraint);
            if (model.PCBTypeList.Any())
                model.PCBTypeItemList = parameterService.GetSelectList(Convert.ToInt64(model.PCBTypeList.First().Value));

            //組態設定-清單與罐頭
            model.PCBParameterList = parameterService.GetSelectList(ParameterKey.PCBTypeList);
            //組態設定-其他
            model.Announcement = parameterService.GetParameterOne(ParameterKey.ConfigurationAnnouncement);
            model.Description = parameterService.GetParameterOne(ParameterKey.ConfigurationDescription);
            model.ApplyDraw = parameterService.GetParameterOne(ParameterKey.ConfigurationApplyDraw);
            model.SendReturn = parameterService.GetParameterOne(ParameterKey.ConfigurationSendReturn);
            model.Release = parameterService.GetParameterOne(ParameterKey.ConfigurationRelease);
            model.Reject = parameterService.GetParameterOne(ParameterKey.ConfigurationReject);
            model.FormApply = parameterService.GetParameterOne(ParameterKey.ConfigurationFormApply);
            model.WorkDetail = parameterService.GetParameterOne(ParameterKey.ConfigurationWorkDetail);
            model.WorkDetail_File = FileService.GetFileList(ParameterKey.ConfigurationWorkDetail, model.WorkDetail.ParameterID).FirstOrDefault();
            model.CompList = parameterService.GetParameterList(ParameterKey.ConfigurationCompCode);
            model.DeptList = parameterService.GetParameterList(ParameterKey.ConfigurationDeptCode);
            model.DepartmentList = authenticationService.GetDepartmentList();

            model.SystemCategoryList = parameterService.GetSelectList(ParameterKey.SystemCategory, false);

            return View(model);
        }

        public ConfigurationController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;

        }

        [HttpPost]
        [ActionCheck]
        public IActionResult HomeLinkAdd(IFormFile file, string ParameterText, int IsSync, string Link, int HomeLinkOrderNo)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            m_ConfigurationPartial ViewModel = new m_ConfigurationPartial();
            if (!string.IsNullOrEmpty(Link))
            {
                string ErrorMsg = string.Empty;

                PDC_Parameter NewParameter = new PDC_Parameter();
                NewParameter.IsSync = IsSync == 1 ? true : false;
                NewParameter.OrderNo = HomeLinkOrderNo;
                NewParameter.ParameterText = ParameterText;
                NewParameter.ParameterGroup = ParameterKey.Configuration_HomeLink;


                if (parameterService.AddParameter(ref NewParameter, ref ErrorMsg))
                {
                    PDC_File NewFile = new PDC_File();
                    NewFile.SourceID = NewParameter.ParameterID;
                    NewFile.FunctionName = ParameterKey.Configuration_HomeLink;
                    NewFile.FileType = 5;
                    NewFile.FileName = Link;
                    NewFile.FileCategory = 2;
                    FileService.FileAdd(ref NewFile);

                    ViewModel.m_HomeLink = NewParameter;
                    ViewModel.m_HomeLinkFile = NewFile;
                }

            }
            else
            {
                //存檔並返回檔案路徑
                string FilePath = Helper.SaveAndGetExcelPath(file);
                if (FilePath != "Error")
                {
                    string ErrorMsg = string.Empty;
                    PDC_Parameter NewParameter = new PDC_Parameter();

                    NewParameter.IsSync = IsSync == 1 ? true : false;
                    NewParameter.OrderNo = HomeLinkOrderNo;
                    NewParameter.ParameterText = ParameterText;
                    NewParameter.ParameterGroup = ParameterKey.Configuration_HomeLink;

                    if (parameterService.AddParameter(ref NewParameter, ref ErrorMsg))
                    {
                        PDC_File NewFile = new PDC_File();
                        NewFile.SourceID = NewParameter.ParameterID;
                        NewFile.FunctionName = ParameterKey.Configuration_HomeLink;
                        NewFile.FileFullName = Path.GetFileName(FilePath);
                        NewFile.FileName = file.FileName;
                        NewFile.FileExtension = Path.GetExtension(FilePath);
                        NewFile.FileType = 2;
                        NewFile.FileCategory = 1;
                        NewFile.FileSize = file.Length;
                        FileService.FileAdd(ref NewFile);

                        ViewModel.m_HomeLink = NewParameter;
                        ViewModel.m_HomeLinkFile = NewFile;
                    }
                }
            }
            //存檔並返回檔案路徑UserManager<IdentityUser>
            //string FilePath = Helper.SaveAndGetExcelPath(file);



            return Json(ViewModel);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult ParameterChangeOrderNo(Int64 ParameterID1, Int64 ParameterID2, int OrderNo1, int OrderNo2)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            PDC_Parameter item1 = parameterService.GetParameterOne(ParameterID1);
            PDC_Parameter item2 = parameterService.GetParameterOne(ParameterID2);

            string ErrorMsg = string.Empty;

            item1.OrderNo = OrderNo1;
            parameterService.UpdateParameter(item1, ref ErrorMsg);


            item2.OrderNo = OrderNo2;
            parameterService.UpdateParameter(item2, ref ErrorMsg);



            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult HomeLinkDelete(Int64 ParameterID)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            if (_context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).Any())
            {
                PDC_Parameter item = _context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).SingleOrDefault();
                _context.PDC_Parameter.Remove(item);

                if (_context.PDC_File.Where(x => x.SourceID == ParameterID).Any())
                {
                    PDC_File File = _context.PDC_File.Where(x => x.SourceID == ParameterID && x.FunctionName == ParameterKey.Configuration_HomeLink).SingleOrDefault();
                    _context.PDC_File.Remove(File);
                }
                _context.SaveChanges();
            }

            return Json("刪除成功");
        }
        [HttpPost]
        [ActionCheck]
        public IActionResult PCBFileRemove(Int64 FileID)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            if (_context.PDC_File.Where(x => x.FileID == FileID).Any())
            {
                PDC_File item = _context.PDC_File.Where(x => x.FileID == FileID).SingleOrDefault();
                _context.PDC_File.Remove(item);
                _context.SaveChanges();
            }

            return Json("刪除成功");
        }


        [HttpPost]
        [ActionCheck]
        public IActionResult PCBChangeItem(Int64 ParameterID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);


            List<SelectListItem> PCBItemList = parameterService.GetSelectList(ParameterID);

            return Json(PCBItemList);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult SearchPCB(Int64 ParameterID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_context, UserInfo.User);


            model.m_PCBParameter = parameterService.GetParameterOne(ParameterID);
            model.PCBFileList = _context.PDC_File.Where(x => x.SourceID == ParameterID && x.FunctionName == FileKey.Configuration_PCBFile).ToList();

            return Json(model);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult SavePCB(Int64 ParameterID, string ParameterDesc)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            PDC_Parameter m_PCBParameter = parameterService.GetParameterOne(ParameterID);

            string ErrorMsg = string.Empty;
            m_PCBParameter.ParameterDesc = ParameterDesc;
            parameterService.UpdateParameter(m_PCBParameter, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult PCBFileAdd(IFormFile file, string FileDescription, Int64 ParameterID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            string FileExtension = Path.GetExtension(file.FileName).ToUpper().Replace(".", "");
            List<string> CheckExtension = new List<string>();
            CheckExtension.Add("PNG");
            CheckExtension.Add("JPG");
            CheckExtension.Add("BMP");
            if(!CheckExtension.Where(x => x.Contains(FileExtension)).Any())
            {
                return Json(new { ErrorMsg= "檔案限定PNG/JPG/BMP" });
            }


            PDC_File m_PCBFile = new PDC_File();
            string ErrorMsg = string.Empty;

            if (FileService.FileAdd(file, FileKey.Configuration_PCBFile, out m_PCBFile, FileKey.FileUpload, ParameterID, FileDescription))
            {
                return Json(m_PCBFile);
            }
            else
                return Json(new PDC_File());

        }

        [HttpPost]
        [ActionCheck]
        public IActionResult SearchPCBParameter(Int64 ParameterID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            List<PDC_Parameter> model = new List<PDC_Parameter>();
            model = parameterService.GetParameterList(ParameterID);

            return Json(model);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult PCBParameterDelete(Int64 ParameterID)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            if (_context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).Any())
            {
                PDC_Parameter item = _context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).SingleOrDefault();
                _context.PDC_Parameter.Remove(item);

                _context.SaveChanges();
            }

            return Json("刪除成功");
        }


        [HttpPost]
        [ActionCheck]
        public IActionResult PCBParameterAdd(string ParameterText, Int64 ParameterID, int OrderNo)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            PDC_Parameter item = new PDC_Parameter();

            string ErrorMsg = string.Empty;
            item.IsSync = false;
            item.OrderNo = OrderNo;
            item.ParameterText = ParameterText;
            item.ParameterGroup = ParameterKey.PCBTypeList_Item;
            item.ParameterParentID = ParameterID;

            parameterService.AddParameter(ref item, ref ErrorMsg);

            return Json(item);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadAnnouncement(string ParameterText)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            string ErrorMsg = string.Empty;


            PDC_Parameter item = parameterService.GetParameterOne(ParameterKey.ConfigurationAnnouncement);
            item.ParameterText = ParameterText;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadDescription(string ParameterText)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            string ErrorMsg = string.Empty;

            PDC_Parameter item = parameterService.GetParameterOne(ParameterKey.ConfigurationDescription);
            item.ParameterText = ParameterText;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadProjectTime(string ApplyDrawText, string SendReturnText, string ReleaseText, string RejectText)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            string ErrorMsg = string.Empty;

            PDC_Parameter ApplyDrawitem = parameterService.GetParameterOne(ParameterKey.ConfigurationApplyDraw);
            ApplyDrawitem.ParameterText = ApplyDrawText;

            PDC_Parameter SendReturnitem = parameterService.GetParameterOne(ParameterKey.ConfigurationSendReturn);
            SendReturnitem.ParameterText = SendReturnText;

            PDC_Parameter Releaseitem = parameterService.GetParameterOne(ParameterKey.ConfigurationRelease);
            Releaseitem.ParameterText = ReleaseText;

            PDC_Parameter Rejectitem = parameterService.GetParameterOne(ParameterKey.ConfigurationReject);
            Rejectitem.ParameterText = RejectText;

            parameterService.UpdateParameter(ApplyDrawitem, ref ErrorMsg);
            parameterService.UpdateParameter(SendReturnitem, ref ErrorMsg);
            parameterService.UpdateParameter(Releaseitem, ref ErrorMsg);
            parameterService.UpdateParameter(Rejectitem, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadFormApply(string ParameterText, string ParameterValue, string ParameterDesc)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            string ErrorMsg = string.Empty;

            PDC_Parameter item = parameterService.GetParameterOne(ParameterKey.ConfigurationFormApply);
            item.ParameterText = ParameterText;
            item.ParameterValue = ParameterValue;
            item.ParameterDesc = ParameterDesc;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadWorkDetail(string ParameterText, string ParameterValue, IFormFile file)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            string ErrorMsg = string.Empty;

            PDC_File newFile = new PDC_File();
            PDC_Parameter item = parameterService.GetParameterOne(ParameterKey.ConfigurationWorkDetail);
            if (file != null)
            {
                PDC_File WorkDetail_File = FileService.GetFileList(ParameterKey.ConfigurationWorkDetail, item.ParameterID).FirstOrDefault();

                if (WorkDetail_File != null)
                {
                    FileService.FileRemove(WorkDetail_File.FileID);
                }

                FileService.FileAdd(file, FileKey.ConfigurationWorkDetail, out newFile, FileKey.FileUpload, item.ParameterID);
            }

            item.ParameterText = ParameterText;
            item.ParameterValue = ParameterValue;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(new { ErrorMsg = ErrorMsg, File = newFile });
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult DeleteWorkDetailFile(Int64 FileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            string ErrorMsg = string.Empty;

            PDC_File WorkDetail_File = FileService.GetFileOne(FileID);

            if (WorkDetail_File.FileID > 0)
            {
                FileService.FileRemove(WorkDetail_File.FileID);
            }



            return Json("刪除成功");
        }

        [HttpPost]
        [ActionCheck]
        public IActionResult UploadFile(IFormFile file)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);

            Stream stream = file.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            if (Helper.ExcelSampleCheck(ExcelFile))
            {
                
                PDC_File item = new PDC_File();
                if (!FileService.FileAdd(file, FileKey.ConfigurationSample, out item))
                {
                    model.ErrorMsg = "檔案儲存失敗";
                }
                model.CNS_Sample = item;
            }
            else
            {
                model.ErrorMsg = "Failed, 含多餘工作表";
            }


            return Json(model);
        }


        public IActionResult GetApiDept(string CompCode, string DeptCode)
        {
            Api_QueryDept model = new Api_QueryDept();
            string ErrorMsg = string.Empty;

            if (ApiService.GetDeptName(CompCode,DeptCode,ref model,ref ErrorMsg))
            {
                return Json(new { status = 0, Api_QueryDept = model });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
        }

        public IActionResult AddParamter(string ParamterGroup,string ParamterValue)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            List<PDC_Parameter> ParamterList = parameterService.GetParameterList(ParamterGroup);

            string ParamterType = ParamterGroup.Contains("Comp") ? "公司別" : "事業群";

            if (ParamterList.Where(x => x.ParameterValue == ParamterValue).Any()) 
            {
                return Json(new { status = 400, ErrorMessage = "該"+ ParamterType + "已新增" });
            }
            else
            {
                string ErrorMsg = string.Empty;
                PDC_Parameter item = new PDC_Parameter();
                item.ParameterGroup = ParamterGroup;
                item.ParameterParentID = 0;
                item.ParameterValue = ParamterValue;
                item.ParameterText = ParamterValue;
                item.IsSync = false;
                item.OrderNo = ParamterList.Count + 1;

                if(parameterService.AddParameter(ref item, ref ErrorMsg))
                {
                    return Json(new { status = 0 ,Parameter = item });
                }
                else
                {
                    return Json(new { status = 400, ErrorMessage = ParamterType + "新增失敗" });
                }
            }
        }

        public IActionResult UpdateParamter(Int64 ParamterID, string ParamterNewValue)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            PDC_Parameter Paramter = parameterService.GetParameterOne(ParamterID);
            Paramter.ParameterValue = ParamterNewValue;
            Paramter.ParameterText = ParamterNewValue;
            string ErrorMsg = string.Empty;

            string ParamterType = Paramter.ParameterGroup.Contains("Comp") ? "公司別" : "事業群";

            if (parameterService.UpdateParameter(Paramter, ref ErrorMsg))
            {
                return Json(new { status = 0, Parameter = Paramter });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = ParamterType + "更新失敗" });
            }
        }

        public IActionResult DeleteParamter(Int64 ParamterID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);

            string ErrorMsg = string.Empty;

            if (parameterService.DeleteParameter(ParamterID, ref ErrorMsg))
            {
                return Json(new { status = 0 });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = "刪除失敗" });
            }
        }

        public IActionResult AddDepartment(string CompCode,string DeptCode,string DeptshowName)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            AuthenticationService authenticationService = new AuthenticationService(_context, UserInfo.User);
            
            Api_QueryDept model = new Api_QueryDept();
            string ErrorMsg = string.Empty;

            if (authenticationService.CheckDepartmentAny(CompCode, DeptCode))
            {
                return Json(new { status = 400, ErrorMessage = "該對照群已新增" });
            }

            if (ApiService.GetDeptName(CompCode, DeptCode, ref model, ref ErrorMsg))
            {
                PDC_Department pDC_Department = new PDC_Department();
                pDC_Department.BUCode = DeptCode;
                pDC_Department.BUName = model.DEPNAM;
                pDC_Department.CompCode = CompCode;
                pDC_Department.CompName = CompCode;
                pDC_Department.DepartmentID = Guid.NewGuid();
                pDC_Department.ShowName = DeptshowName;

                if(authenticationService.AddDepartment(ref pDC_Department))
                {
                    return Json(new { status = 0, Department = pDC_Department });
                }
                else
                {
                    return Json(new { status = 400, ErrorMessage = "新增對照群失敗" });
                }
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = ErrorMsg });
            }
        }

        public IActionResult DeleteDepartment(Guid DepartmentID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            AuthenticationService authenticationService = new AuthenticationService(_context, UserInfo.User);

            if(authenticationService.DeleteDepartment(DepartmentID))
            {
                return Json(new { status = 0 });
            }
            else
            {
                return Json(new { status = 400, ErrorMessage = "刪除對照群失敗" });
            }
        }
    }
}