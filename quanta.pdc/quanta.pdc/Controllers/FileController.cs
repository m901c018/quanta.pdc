using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using cns.Data;
using cns.Models;
using cns.Services;
using cns.Services.App;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace cns.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;

        }

        [HttpGet]
        [ActionCheck]
        public IActionResult Download(Int64 fileID)
        {
            //讀取使用者資訊
            CurrentUser UserInfo = HttpContext.Session.GetObjectFromJson<CurrentUser>(SessionKey.usrInfo);

            FileService fileService = new FileService(_hostingEnvironment, _context, UserInfo.User);

            PDC_File pDC_File = fileService.GetFileOne(fileID);
            //取得範例
            MemoryStream stream = fileService.DownloadFile(pDC_File.FileFullName);

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(pDC_File.FileName, out contentType);

            string sFileName = HttpUtility.UrlEncode(pDC_File.FileName);


            return File(stream.ToArray(), contentType, sFileName);
        }
    }
}