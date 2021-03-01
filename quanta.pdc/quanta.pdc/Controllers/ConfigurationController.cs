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

namespace cns.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        

        public IActionResult Permission()
        {
            return View();
        }

        public IActionResult DownloadCNS()
        {
            return View();
        }
        //consume custom security service to get all roles
        public IActionResult Index()
        {
            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            var user = _userManager.GetUserId(HttpContext.User);

            //組態設定-CNS範本
            if (_context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").Any())
            {
                model.CNS_Sample = _context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            }
            //組態設定-首頁連結
            model.HomeLinkList = parameterService.GetParameterList("Configuration_HomeLink");
            model.HomeLinkFileList = _context.PDC_File.Where(x => x.FunctionName == "Configuration_HomeLink").ToList();

            //組態設定-圖文說明
            model.PCBTypeList = parameterService.GetSelectList("PCBLayoutConstraint");
            if (model.PCBTypeList.Any())
                model.PCBTypeItemList = parameterService.GetSelectList(Convert.ToInt64(model.PCBTypeList.First().Value));

            //組態設定-清單與罐頭
            model.PCBParameterList = parameterService.GetSelectList("PCBTypeList");
            //組態設定-其他
            model.Announcement = parameterService.GetParameterOne("ConfigurationAnnouncement");
            model.Description = parameterService.GetParameterOne("ConfigurationDescription");
            model.ApplyDraw = parameterService.GetParameterOne("ConfigurationApplyDraw");
            model.SendReturn = parameterService.GetParameterOne("ConfigurationSendReturn");
            model.Release = parameterService.GetParameterOne("ConfigurationRelease");
            model.Reject = parameterService.GetParameterOne("ConfigurationReject");
            model.FormApply = parameterService.GetParameterOne("ConfigurationFormApply");
            model.WorkDetail = parameterService.GetParameterOne("ConfigurationWorkDetail");
            model.WorkDetail_File = FileService.GetFileList("ConfigurationWorkDetail", model.WorkDetail.ParameterID).FirstOrDefault();

            model.SystemCategoryList = parameterService.GetSelectList("SystemCategory", false);

            return View(model);
        }

        public ConfigurationController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;

        }

        [HttpPost]
        public IActionResult HomeLinkAdd(IFormFile file, string ParameterText, int IsSync, string Link, int HomeLinkOrderNo)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            ParameterService parameterService = new ParameterService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            m_ConfigurationPartial ViewModel = new m_ConfigurationPartial();
            if (!string.IsNullOrEmpty(Link))
            {
                string ErrorMsg = string.Empty;

                PDC_Parameter NewParameter = new PDC_Parameter();
                NewParameter.IsSync = IsSync == 1 ? true : false;
                NewParameter.OrderNo = HomeLinkOrderNo;
                NewParameter.ParameterText = ParameterText;
                NewParameter.ParameterGroup = "Configuration_HomeLink";
                NewParameter.Creator = userId;
                NewParameter.CreatorName = userName;
                NewParameter.CreatorDate = DateTime.Now;


                if (parameterService.AddParameter(ref NewParameter, ref ErrorMsg))
                {
                    PDC_File NewFile = new PDC_File();
                    NewFile.SourceID = NewParameter.ParameterID;
                    NewFile.FunctionName = "Configuration_HomeLink";
                    NewFile.FileType = 5;
                    NewFile.FileName = Link;
                    NewFile.FileCategory = 2;
                    NewFile.Creator = userId;
                    NewFile.CreatorName = userName;
                    NewFile.CreatorDate = DateTime.Now;
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
                    ViewModel.m_HomeLink.IsSync = IsSync == 1 ? true : false;
                    ViewModel.m_HomeLink.OrderNo = HomeLinkOrderNo;
                    ViewModel.m_HomeLink.ParameterText = ParameterText;
                    ViewModel.m_HomeLink.ParameterGroup = "Configuration_HomeLink";
                    ViewModel.m_HomeLink.Creator = "m901c018";
                    ViewModel.m_HomeLink.CreatorDate = DateTime.Now;
                    ViewModel.m_HomeLink.CreatorName = "Mike";
                    _context.PDC_Parameter.Add(ViewModel.m_HomeLink);
                    _context.SaveChanges();

                    ViewModel.m_HomeLinkFile.SourceID = ViewModel.m_HomeLink.ParameterID;
                    ViewModel.m_HomeLinkFile.FunctionName = "Configuration_HomeLink";
                    ViewModel.m_HomeLinkFile.FileFullName = Path.GetFileName(FilePath);
                    ViewModel.m_HomeLinkFile.FileName = file.FileName;
                    ViewModel.m_HomeLinkFile.FileExtension = Path.GetExtension(FilePath);
                    ViewModel.m_HomeLinkFile.FileType = 2;
                    ViewModel.m_HomeLinkFile.FileCategory = 1;
                    ViewModel.m_HomeLinkFile.FileSize = file.Length;
                    ViewModel.m_HomeLinkFile.Creator = "m901c018";
                    ViewModel.m_HomeLinkFile.CreatorDate = DateTime.Now;
                    ViewModel.m_HomeLinkFile.CreatorName = "Mike";

                    _context.PDC_File.Add(ViewModel.m_HomeLinkFile);
                    _context.SaveChanges();
                }



            }
            //存檔並返回檔案路徑UserManager<IdentityUser>
            //string FilePath = Helper.SaveAndGetExcelPath(file);



            return Json(ViewModel);
        }

        [HttpPost]
        public IActionResult ParameterChangeOrderNo(Int64 ParameterID1, Int64 ParameterID2, int OrderNo1, int OrderNo2)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            ParameterService parameterService = new ParameterService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context);

            PDC_Parameter item1 = parameterService.GetParameterOne(ParameterID1);
            PDC_Parameter item2 = parameterService.GetParameterOne(ParameterID2);

            string ErrorMsg = string.Empty;

            item1.OrderNo = OrderNo1;
            item1.Modifyer = userId;
            item1.ModifyerName = userName;
            item1.ModifyerDate = DateTime.Now;
            parameterService.UpdateParameter(item1, ref ErrorMsg);


            item2.OrderNo = OrderNo2;
            item2.Modifyer = userId;
            item2.ModifyerName = userName;
            item2.ModifyerDate = DateTime.Now;
            parameterService.UpdateParameter(item2, ref ErrorMsg);



            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult HomeLinkDelete(Int64 ParameterID)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            if (_context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).Any())
            {
                PDC_Parameter item = _context.PDC_Parameter.Where(x => x.ParameterID == ParameterID).SingleOrDefault();
                _context.PDC_Parameter.Remove(item);

                if (_context.PDC_File.Where(x => x.SourceID == ParameterID).Any())
                {
                    PDC_File File = _context.PDC_File.Where(x => x.SourceID == ParameterID && x.FunctionName == "Configuration_HomeLink").SingleOrDefault();
                    _context.PDC_File.Remove(File);
                }
                _context.SaveChanges();
            }

            return Json("刪除成功");
        }
        [HttpPost]
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
        public IActionResult PCBChangeItem(Int64 ParameterID)
        {
            ParameterService parameterService = new ParameterService(_context);


            List<SelectListItem> PCBItemList = parameterService.GetSelectList(ParameterID);

            return Json(PCBItemList);
        }

        [HttpPost]
        public IActionResult SearchPCB(Int64 ParameterID)
        {
            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_context);


            model.m_PCBParameter = parameterService.GetParameterOne(ParameterID);
            model.PCBFileList = _context.PDC_File.Where(x => x.SourceID == ParameterID && x.FunctionName == "Configuration_PCBFile").ToList();

            return Json(model);
        }

        [HttpPost]
        public IActionResult SavePCB(Int64 ParameterID, string ParameterDesc)
        {
            ParameterService parameterService = new ParameterService(_context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            PDC_Parameter m_PCBParameter = parameterService.GetParameterOne(ParameterID);

            string ErrorMsg = string.Empty;
            m_PCBParameter.ParameterDesc = ParameterDesc;
            m_PCBParameter.Modifyer = userId;
            m_PCBParameter.ModifyerName = userName;
            m_PCBParameter.ModifyerDate = DateTime.Now;
            parameterService.UpdateParameter(m_PCBParameter, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult PCBFileAdd(IFormFile file, string FileDescription, Int64 ParameterID)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);
            string FileExtension = Path.GetExtension(file.FileName).ToUpper().Replace(".", "");
            List<string> CheckExtension = new List<string>();
            CheckExtension.Add("PNG");
            CheckExtension.Add("JPG");
            CheckExtension.Add("BMP");
            if(!CheckExtension.Where(x => x.Contains(FileExtension)).Any())
            {
                return Json(new { ErrorMsg= "檔案限定PNG/JPG/BMP" });
            }


            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            PDC_File m_PCBFile = new PDC_File();
            string ErrorMsg = string.Empty;

            if (FileService.FileAdd(file, "Configuration_PCBFile", userId, userName, out m_PCBFile,"FileUpload", ParameterID, FileDescription))
            {
                return Json(m_PCBFile);
            }
            else
                return Json(new PDC_File());

        }

        [HttpPost]
        public IActionResult SearchPCBParameter(Int64 ParameterID)
        {
            ParameterService parameterService = new ParameterService(_context);

            List<PDC_Parameter> model = new List<PDC_Parameter>();
            model = parameterService.GetParameterList(ParameterID);

            return Json(model);
        }

        [HttpPost]
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
        public IActionResult PCBParameterAdd(string ParameterText, Int64 ParameterID, int OrderNo)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            PDC_Parameter item = new PDC_Parameter();

            item.IsSync = false;
            item.OrderNo = OrderNo;
            item.ParameterText = ParameterText;
            item.ParameterGroup = "PCBTypeList_Item";
            item.ParameterParentID = ParameterID;
            item.Creator = "c5805dbf-dac5-41e6-bb72-5eb0b449134d";
            item.CreatorDate = DateTime.Now;
            item.CreatorName = "super@admin.com";
            _context.PDC_Parameter.Add(item);
            _context.SaveChanges();

            return Json(item);
        }

        [HttpPost]
        public IActionResult UploadAnnouncement(string ParameterText)
        {
            ParameterService parameterService = new ParameterService(_context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;


            PDC_Parameter item = parameterService.GetParameterOne("ConfigurationAnnouncement");
            item.ParameterText = ParameterText;
            item.Modifyer = userId;
            item.ModifyerName = userName;
            item.ModifyerDate = DateTime.Now;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult UploadDescription(string ParameterText)
        {
            ParameterService parameterService = new ParameterService(_context);

            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;

            PDC_Parameter item = parameterService.GetParameterOne("ConfigurationDescription");
            item.ParameterText = ParameterText;
            item.Modifyer = userId;
            item.ModifyerName = userName;
            item.ModifyerDate = DateTime.Now;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult UploadProjectTime(string ApplyDrawText, string SendReturnText, string ReleaseText, string RejectText)
        {
            ParameterService parameterService = new ParameterService(_context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;

            PDC_Parameter ApplyDrawitem = parameterService.GetParameterOne("ConfigurationApplyDraw");
            ApplyDrawitem.ParameterText = ApplyDrawText;
            ApplyDrawitem.Modifyer = userId;
            ApplyDrawitem.ModifyerName = userName;
            ApplyDrawitem.ModifyerDate = DateTime.Now;

            PDC_Parameter SendReturnitem = parameterService.GetParameterOne("ConfigurationSendReturn");
            SendReturnitem.ParameterText = SendReturnText;
            SendReturnitem.Modifyer = userId;
            SendReturnitem.ModifyerName = userName;
            SendReturnitem.ModifyerDate = DateTime.Now;

            PDC_Parameter Releaseitem = parameterService.GetParameterOne("ConfigurationRelease");
            Releaseitem.ParameterText = ReleaseText;
            Releaseitem.Modifyer = userId;
            Releaseitem.ModifyerName = userName;
            Releaseitem.ModifyerDate = DateTime.Now;

            PDC_Parameter Rejectitem = parameterService.GetParameterOne("ConfigurationReject");
            Rejectitem.ParameterText = RejectText;
            Rejectitem.Modifyer = userId;
            Rejectitem.ModifyerName = userName;
            Rejectitem.ModifyerDate = DateTime.Now;

            parameterService.UpdateParameter(ApplyDrawitem, ref ErrorMsg);
            parameterService.UpdateParameter(SendReturnitem, ref ErrorMsg);
            parameterService.UpdateParameter(Releaseitem, ref ErrorMsg);
            parameterService.UpdateParameter(Rejectitem, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult UploadFormApply(string ParameterText, string ParameterValue, string ParameterDesc)
        {
            ParameterService parameterService = new ParameterService(_context);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            string ErrorMsg = string.Empty;

            PDC_Parameter item = parameterService.GetParameterOne("ConfigurationFormApply");
            item.ParameterText = ParameterText;
            item.ParameterValue = ParameterValue;
            item.ParameterDesc = ParameterDesc;
            item.Modifyer = userId;
            item.ModifyerName = userName;
            item.ModifyerDate = DateTime.Now;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult UploadWorkDetail(string ParameterText, string ParameterValue, IFormFile file)
        {
            ParameterService parameterService = new ParameterService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context);
            string ErrorMsg = string.Empty;
            string userId = _userManager.GetUserId(HttpContext.User);
            string userName = HttpContext.User.Identity.Name;

            PDC_File newFile = new PDC_File();
            PDC_Parameter item = parameterService.GetParameterOne("ConfigurationWorkDetail");
            if (file.Length > 0)
            {
                PDC_File WorkDetail_File = FileService.GetFileList("ConfigurationWorkDetail", item.ParameterID).FirstOrDefault();

                if (WorkDetail_File != null)
                {
                    FileService.FileRemove(WorkDetail_File.FileID);
                }

                FileService.FileAdd(file, "ConfigurationWorkDetail", userId, userName, out newFile, "FileUpload", item.ParameterID);
            }

            item.ParameterText = ParameterText;
            item.ParameterValue = ParameterValue;
            item.Modifyer = userId;
            item.ModifyerName = userName;
            item.ModifyerDate = DateTime.Now;

            parameterService.UpdateParameter(item, ref ErrorMsg);

            return Json(new { ErrorMsg = ErrorMsg, File = newFile });
        }

        [HttpPost]
        public IActionResult DeleteWorkDetailFile(Int64 FileID)
        {

            FileService FileService = new FileService(_hostingEnvironment, _context);
            string ErrorMsg = string.Empty;

            PDC_File WorkDetail_File = FileService.GetFileOne(FileID);

            if (WorkDetail_File.FileID > 0)
            {
                FileService.FileRemove(WorkDetail_File.FileID);
            }



            return Json("刪除成功");
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            FileService FileService = new FileService(_hostingEnvironment, _context);

            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            string userId = "super@admin.com"; //_userManager.GetUserId(HttpContext.User)
            string userName = "Roger Chao (趙偉智)"; //HttpContext.User.Identity.Name

            Stream stream = file.OpenReadStream();
            //轉NPOI類型
            XSSFWorkbook ExcelFile = new XSSFWorkbook(stream);

            if (Helper.ExcelSampleCheck(ExcelFile))
            {
                ////存檔並返回檔案路徑
                //string FilePath = Helper.SaveAndGetExcelPath(file);

                //if (FilePath != "Error")
                //{
                //    model.CNS_Sample.FileFullName = Path.GetFileName(FilePath);
                //    model.CNS_Sample.FileName = file.FileName;
                //    model.CNS_Sample.FileExtension = Path.GetExtension(FilePath);
                //    model.CNS_Sample.FileType = 2;
                //    model.CNS_Sample.FileCategory = 1;
                //    model.CNS_Sample.FileSize = file.Length;
                //    model.CNS_Sample.FunctionName = "ConfigurationSample";
                //    model.CNS_Sample.Creator = "c5805dbf-dac5-41e6-bb72-5eb0b449134d";
                //    model.CNS_Sample.CreatorDate = DateTime.Now;
                //    model.CNS_Sample.CreatorName = "super@admin.com";
                //    _context.PDC_File.Add(model.CNS_Sample);
                //    _context.SaveChanges();
                //}
                PDC_File item = new PDC_File();
                if (!FileService.FileAdd(file, "ConfigurationSample", userId, userName, out item))
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
    }
}