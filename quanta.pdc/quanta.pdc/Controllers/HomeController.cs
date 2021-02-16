using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cns.Data;
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
            string WindowsName = WindowsIdentity.GetCurrent().Name;
            string testName = Environment.UserName;
            string testName1 = Environment.UserDomainName;

            if (!string.IsNullOrWhiteSpace(WindowsName))
            {

            }

            return View();
        }

      
    }
}