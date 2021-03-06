﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.Services;
using cns.Services.App;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace cns.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private IConfiguration _config;

        public HomeController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public IActionResult Login(string EmpNumber = "")
        {
            AuthenticationService authenticationService = new AuthenticationService(_context);
            PrivilegeService privilegeService = new PrivilegeService(_context);

            bool IsDemo = _config.GetValue<bool>("IdentityDefaultOptions:IsDemo");
            //測試用
            if(IsDemo)
            {
                EmpNumber = string.IsNullOrWhiteSpace(EmpNumber) ? "100041F9" : EmpNumber;
                CurrentUser User = new CurrentUser();

                if (authenticationService.GetAccountDemo(EmpNumber, ref User))
                {

                    User.PrivilegeList = privilegeService.GetPrivilegeList(User.User.MemberID);

                    User.MenuList = authenticationService.GetMenuList(User.PrivilegeList);
                    //Session紀錄
                    HttpContext.Session.SetObjectAsJson(SessionKey.usrInfo, User);
                }
                
            }
            else
            {
                string DomainwithNumber = Environment.UserDomainName + "\\\\" + Environment.UserName;
                Api_QueryUserInfo apiModel = new Api_QueryUserInfo();
                string ErrorMsg = string.Empty;

                if (ApiService.GetApiUserInfo(DomainwithNumber, ref apiModel, ref ErrorMsg)) 
                {
                    CurrentUser User = new CurrentUser();
                    if (authenticationService.GetApiAccount(apiModel, ref User))
                    {
                        User.PrivilegeList = privilegeService.GetPrivilegeList(User.User.MemberID);

                        User.MenuList = authenticationService.GetMenuList(User.PrivilegeList);
                        //Session紀錄
                        HttpContext.Session.SetObjectAsJson(SessionKey.usrInfo, User);
                    }
                }
                else
                {
                    TempData["TempMsg"] = "工號：" + DomainwithNumber + "Api查無資料";
                }
            }
           

            return RedirectToAction("Index");
        }

        public IActionResult Loginoff()
        {
            //TempData["TempMsg"] = "時間已逾時，返回首頁！";

            return RedirectToAction("Login");
        }

        [ActionCheck]
        public IActionResult Index()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            ParameterService parameterService = new ParameterService(_context, UserInfo.User);
            FileService FileService = new FileService(_hostingEnvironment, _context, UserInfo.User);
            m_HomePartial ViewModel = new m_HomePartial();
            //CNS範本
            if (_context.PDC_File.Where(x => x.FunctionName == FileKey.ConfigurationSample).Any())
            {
                ViewModel.m_CNSSampleFile = _context.PDC_File.Where(x => x.FunctionName == FileKey.ConfigurationSample).OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            }
            //首頁連結
            ViewModel.HomeLinkList = parameterService.GetParameterList(ParameterKey.Configuration_HomeLink);
            ViewModel.HomeLinkFileList = _context.PDC_File.Where(x => x.FunctionName == FileKey.Configuration_HomeLink).ToList();
            //首頁公告
            ViewModel.Announcement = parameterService.GetParameterOne(ParameterKey.ConfigurationAnnouncement);


            return View(ViewModel);
        }

        public IActionResult Menu()
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            AuthenticationService authenticationService = new AuthenticationService(_context);
            PrivilegeService privilegeService = new PrivilegeService(_context, UserInfo.User);

            m_HomePartial model = new m_HomePartial();
            List<PDC_Privilege> pDC_Privileges = privilegeService.GetPrivilegeList(UserInfo.User.MemberID);
            model.MenuList = authenticationService.GetMenuList(pDC_Privileges);

            return PartialView(model);

        }

        public IActionResult HeaderUser()
        {
            AuthenticationService authenticationService = new AuthenticationService(_context);
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);
            m_HomePartial model = new m_HomePartial();
            model.User = UserInfo.User;
            model.MemberList = authenticationService.GetAccountList();

            return PartialView(model);

        }
    }
}