using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cns.Data;
using cns.Services;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace cns.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //string WindowsName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //string WindowsName = WindowsIdentity.GetCurrent().Name;
            string WindowsName = Environment.UserName;
            //string testName1 = Environment.UserDomainName;
            if (!string.IsNullOrWhiteSpace(WindowsName))
            {

            }

            ParameterService parameterService = new ParameterService(_context);
            FileService FileService = new FileService(_hostingEnvironment, _context);
            m_HomePartial ViewModel = new m_HomePartial();
            //CNS範本
            if (_context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").Any())
            {
                ViewModel.m_CNSSampleFile  = _context.PDC_File.Where(x => x.FunctionName == "ConfigurationSample").OrderByDescending(x => x.CreatorDate).FirstOrDefault();

            }
            //首頁連結
            ViewModel.HomeLinkList = parameterService.GetParameterList("Configuration_HomeLink");
            ViewModel.HomeLinkFileList = _context.PDC_File.Where(x => x.FunctionName == "Configuration_HomeLink").ToList();
            //首頁公告
            ViewModel.Announcement = parameterService.GetParameterOne("ConfigurationAnnouncement");


            return View(ViewModel);
        }

      
    }
}