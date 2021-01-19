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

        private readonly Services.Security.ICommon _security;

        //consume custom security service to get all roles
        public IActionResult Index()
        {
            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);
            if (_context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").Any())
            {
                model.CNS_Sample = _context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            }
            model.HomeLinkList = parameterService.GetParameterList("Configuration_HomeLink");
            model.HomeLinkFileList = _context.PDC_File.Where(x => x.FunctionName == "Configuration_HomeLink").ToList();

            model.PCBTypeList = parameterService.GetSelectList("PCBLayoutConstraint");
            if (model.PCBTypeList.Any())
                model.PCBTypeItemList = parameterService.GetSelectList(Convert.ToInt64(model.PCBTypeList.First().Value));

            model.PCBParameterList = parameterService.GetSelectList("PCBTypeList");

            return View(model);
        }

        public ConfigurationController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager, Services.Security.ICommon security)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;
            _security = security;
        }

        [HttpPost]
        public IActionResult HomeLinkAdd(IFormFile file, string ParameterText, int IsSync, string Link, int HomeLinkOrderNo)
        {
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            m_ConfigurationPartial ViewModel = new m_ConfigurationPartial();
            if (!string.IsNullOrEmpty(Link))
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
                ViewModel.m_HomeLinkFile.FileType = 5;
                ViewModel.m_HomeLinkFile.FileName = Link;
                ViewModel.m_HomeLinkFile.FileCategory = 2;
                ViewModel.m_HomeLinkFile.Creator = "m901c018";
                ViewModel.m_HomeLinkFile.CreatorDate = DateTime.Now;
                ViewModel.m_HomeLinkFile.CreatorName = "Mike";
                _context.PDC_File.Add(ViewModel.m_HomeLinkFile);
                _context.SaveChanges();
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
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);

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
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);

            List<SelectListItem> PCBItemList = parameterService.GetSelectList(ParameterID);

            return Json(PCBItemList);
        }

        [HttpPost]
        public IActionResult SearchPCB(Int64 ParameterID)
        {
            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);

            model.m_PCBParameter = parameterService.GetParameterOne(ParameterID);
            model.PCBFileList = _context.PDC_File.Where(x => x.SourceID == ParameterID && x.FunctionName == "Configuration_PCBFile").ToList();

            return Json(model);
        }

        [HttpPost]
        public IActionResult SavePCB(Int64 ParameterID, string ParameterDesc)
        {
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);

            PDC_Parameter m_PCBParameter = parameterService.GetParameterOne(ParameterID);

            string ErrorMsg = string.Empty;
            m_PCBParameter.ParameterDesc = ParameterDesc;
            parameterService.UpdateParameter(m_PCBParameter, ref ErrorMsg);

            return Json(ErrorMsg);
        }

        [HttpPost]
        public IActionResult PCBFileAdd(IFormFile file, string FileDescription, Int64 ParameterID)
        {
            FileService fileService = new FileService(_hostingEnvironment, _context);

            PDC_File m_PCBFile = new PDC_File();
            string ErrorMsg = string.Empty;

            if (fileService.FileAdd(file, "Configuration_PCBFile", out m_PCBFile, ParameterID, FileDescription))
            {
                return Json(m_PCBFile);
            }
            else
                return Json(new PDC_File());

        }

        [HttpPost]
        public IActionResult SearchPCBParameter(Int64 ParameterID)
        {
            ParameterService parameterService = new ParameterService(_hostingEnvironment, _context);
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
        public IActionResult PCBParameterAdd(string ParameterText,Int64 ParameterID,int OrderNo)
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
        public IActionResult UploadFile(IFormFile file)
        {
            m_ConfigurationPartial model = new m_ConfigurationPartial();
            ExcelHepler Helper = new ExcelHepler(_hostingEnvironment);
            FileService fileService = new FileService(_hostingEnvironment, _context);

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
                if (!fileService.FileAdd(file, "ConfigurationSample", out item))
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