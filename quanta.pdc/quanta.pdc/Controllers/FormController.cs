using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cns.Data;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

            return View();
        }

        [HttpPost]
        public IActionResult FormApply(m_FormPartial model)
        {
          
            return View();
        }

        public IActionResult FormQuery()
        {
            return View();
        }
    }
}