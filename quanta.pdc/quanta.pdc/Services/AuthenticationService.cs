using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using cns.Data;
using cns.Models;
using cns.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using cns.Services.Enum;
using cns.Services.Helper;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;
using static cns.Services.Enum.MemberEnum;

namespace cns.Services
{

    public class AuthenticationService
    {

        private readonly ApplicationDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;

        }

        public AuthenticationService(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public List<Menu> GetMenuList(MemberEnum.Role Role)
        {
            List<Menu> MenuList = new List<Menu>();

            switch(Role)
            {
                case MemberEnum.Role.EE_User:
                    MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                    MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1,Action= "Index", Controller = "Excel" });
                    MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2,Action= "FormQuery", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3,Action= "FormApply", Controller = "Form" });
                    break;
                case MemberEnum.Role.PDC_Processor:
                    MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                    MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1, Action = "Index", Controller = "Excel" });
                    MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2, Action = "FormQuery", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3, Action = "FormApply", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 5, ParentMenuID = 0, MenuName = "PDC Work Area", Order = 2 });
                    MenuList.Add(new Menu { MenuID = 6, ParentMenuID = 5, MenuName = "Work/工作區", Order = 1, Action = "WorkArea", Controller = "Work" });
                    break;
                case MemberEnum.Role.PDC_Designator:
                    MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                    MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1, Action = "Index", Controller = "Excel" });
                    MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2, Action = "FormQuery", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3, Action = "FormApply", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 5, ParentMenuID = 0, MenuName = "PDC Work Area", Order = 2 });
                    MenuList.Add(new Menu { MenuID = 7, ParentMenuID = 5, MenuName = "Assign/派單", Order = 1, Action = "Assign", Controller = "Work" });
                    MenuList.Add(new Menu { MenuID = 6, ParentMenuID = 5, MenuName = "Work/工作區", Order = 2, Action = "WorkArea", Controller = "Work" });
                    break;
                case MemberEnum.Role.PDC_Administrator:
                    MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                    MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1, Action = "Index", Controller = "Excel" });
                    MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2, Action = "FormQuery", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3, Action = "FormApply", Controller = "Form" });
                    MenuList.Add(new Menu { MenuID = 5, ParentMenuID = 0, MenuName = "PDC Work Area", Order = 2 });
                    MenuList.Add(new Menu { MenuID = 7, ParentMenuID = 5, MenuName = "Assign/派單", Order = 1, Action = "Assign", Controller = "Work" });
                    MenuList.Add(new Menu { MenuID = 6, ParentMenuID = 5, MenuName = "Work/工作區", Order = 2, Action = "WorkArea", Controller = "Work" });
                    MenuList.Add(new Menu { MenuID = 8, ParentMenuID = 0, MenuName = "System Administration", Order = 3 });
                    MenuList.Add(new Menu { MenuID = 9, ParentMenuID = 8, MenuName = "Download/資料下載", Order = 1, Action = "DownloadCNS", Controller = "Configuration" });
                    MenuList.Add(new Menu { MenuID = 10, ParentMenuID = 8, MenuName = "Privilege/權限設定", Order = 2, Action = "Permission", Controller = "Configuration" });
                    MenuList.Add(new Menu { MenuID = 11, ParentMenuID = 8, MenuName = "Check Rules/規則設定", Order = 3, Action = "", Controller = "" });
                    MenuList.Add(new Menu { MenuID = 12, ParentMenuID = 8, MenuName = "Configuration/組態設定", Order = 4, Action = "Index", Controller = "Configuration" });
                    break;
            }

            return MenuList;
        }

        public List<Menu> GetMenuList(List<PDC_Privilege> PrivilegeList)
        {
            List<Menu> MenuList = new List<Menu>();

            if (PrivilegeList.Where(x => x.RoleID == Role.PDC_Administrator).Any()) 
            {
                MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1, Action = "Index", Controller = "Excel" });
                MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2, Action = "FormQuery", Controller = "Form" });
                MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3, Action = "FormApply", Controller = "Form" });
                MenuList.Add(new Menu { MenuID = 5, ParentMenuID = 0, MenuName = "PDC Work Area", Order = 2 });
                MenuList.Add(new Menu { MenuID = 7, ParentMenuID = 5, MenuName = "Assign/派單", Order = 1, Action = "Assign", Controller = "Work" });
                MenuList.Add(new Menu { MenuID = 6, ParentMenuID = 5, MenuName = "Work/工作區", Order = 2, Action = "WorkArea", Controller = "Work" });
                MenuList.Add(new Menu { MenuID = 8, ParentMenuID = 0, MenuName = "System Administration", Order = 3 });
                MenuList.Add(new Menu { MenuID = 9, ParentMenuID = 8, MenuName = "Download/資料下載", Order = 1, Action = "DownloadCNS", Controller = "Configuration" });
                MenuList.Add(new Menu { MenuID = 10, ParentMenuID = 8, MenuName = "Privilege/權限設定", Order = 2, Action = "Permission", Controller = "Configuration" });
                MenuList.Add(new Menu { MenuID = 11, ParentMenuID = 8, MenuName = "Check Rules/規則設定", Order = 3, Action = "", Controller = "" });
                MenuList.Add(new Menu { MenuID = 12, ParentMenuID = 8, MenuName = "Configuration/組態設定", Order = 4, Action = "Index", Controller = "Configuration" });
            }
            else
            {
                MenuList.Add(new Menu { MenuID = 1, ParentMenuID = 0, MenuName = "PCB Layout Constraint", Order = 1 });
                MenuList.Add(new Menu { MenuID = 2, ParentMenuID = 1, MenuName = "Check/驗證", Order = 1, Action = "Index", Controller = "Excel" });
                MenuList.Add(new Menu { MenuID = 3, ParentMenuID = 1, MenuName = "Query/查詢", Order = 2, Action = "FormQuery", Controller = "Form" });
                MenuList.Add(new Menu { MenuID = 4, ParentMenuID = 1, MenuName = "Apply/表單申請", Order = 3, Action = "FormApply", Controller = "Form" });
                if (PrivilegeList.Any())
                {
                    MenuList.Add(new Menu { MenuID = 5, ParentMenuID = 0, MenuName = "PDC Work Area", Order = 2 });

                    if (PrivilegeList.Where(x => x.RoleID == Role.PDC_Designator).Any())
                        MenuList.Add(new Menu { MenuID = 7, ParentMenuID = 5, MenuName = "Assign/派單", Order = 1, Action = "Assign", Controller = "Work" });

                    if (PrivilegeList.Where(x => x.RoleID == Role.PDC_Processor).Any())
                        MenuList.Add(new Menu { MenuID = 6, ParentMenuID = 5, MenuName = "Work/工作區", Order = 1, Action = "WorkArea", Controller = "Work" });
                }
            }

            return MenuList;
        }

        /// <summary> 取得測試用帳號
        /// 
        /// </summary>
        /// <param name="EmpNumber">工號</param>
        /// <param name="User">帳號Model</param>
        /// <returns></returns>
        public bool GetAccountDemo(string EmpNumber,ref CurrentUser User)
        {
            User = new CurrentUser();

            try
            {
                if (_context.PDC_Member.Where(x => (x.EmpNumber == EmpNumber.Trim() || x.DomainEmpNumber == EmpNumber.Trim()) && x.IsEnabled == false).Any())
                {
                    User.User = _context.PDC_Member.Where(x => (x.EmpNumber == EmpNumber.Trim() || x.DomainEmpNumber == EmpNumber.Trim()) && x.IsEnabled == false).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        /// <summary> 取得帳號
        /// 
        /// </summary>
        /// <param name="EmpNumber">工號</param>
        /// <param name="User">帳號Model</param>
        /// <returns></returns>
        public bool GetMember(string EmpNumber, ref PDC_Member User)
        {
            User = new PDC_Member();

            try
            {
                if (_context.PDC_Member.Where(x => (x.EmpNumber == EmpNumber.Trim() || x.DomainEmpNumber == EmpNumber.Trim()) && x.IsEnabled == false).Any())
                {
                    User = _context.PDC_Member.Where(x => (x.EmpNumber == EmpNumber.Trim() || x.DomainEmpNumber == EmpNumber.Trim()) && x.IsEnabled == false).SingleOrDefault();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }

        /// <summary> 取得帳號
        /// 
        /// </summary>
        /// <param name="EmpNumber">工號</param>
        /// <param name="User">帳號Model</param>
        /// <returns></returns>
        public PDC_Member GetMember(Guid MemberID)
        {
            PDC_Member User = new PDC_Member();

            try
            {
                if (_context.PDC_Member.Where(x => x.MemberID == MemberID && x.IsEnabled == false).Any())
                {
                    User = _context.PDC_Member.Where(x => x.MemberID == MemberID && x.IsEnabled == false).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {

                return new PDC_Member();
            }

            return User;
        }

        /// <summary> 取得帳號
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PDC_Member> GetAccountList()
        {
            List <PDC_Member> MemberList = new List<PDC_Member>();

            MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false).ToList();

            return MemberList;
        }

        /// <summary> 取得帳號清單
        /// 
        /// </summary>
        /// <param name="role">角色代碼</param>
        /// <returns></returns>
        public List<PDC_Member> GetAccountList(Role role)
        {
            List<PDC_Member> MemberList = new List<PDC_Member>();

            switch(role)
            {
                case Role.EE_User:
                    MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false && x.RoleID == Role.EE_User).ToList();
                    break;
                case Role.PDC_Processor:
                    MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false && x.RoleID != Role.EE_User).ToList();
                    break;
                case Role.PDC_Designator:
                    MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false && x.RoleID != Role.EE_User && x.RoleID != Role.PDC_Processor).ToList();
                    break;
                case Role.PDC_Administrator:
                    MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false && x.RoleID != Role.EE_User && x.RoleID != Role.PDC_Processor && x.RoleID != Role.PDC_Designator).ToList();
                    break;
                case Role.PDC_Programmer:
                    MemberList = _context.PDC_Member.Where(x => x.IsEnabled == false).ToList();
                    break;
            }

            return MemberList;
        }

        /// <summary> 取得帳號
        /// 
        /// </summary>
        /// <param name="EmpNumber">工號</param>
        /// <param name="User">帳號Model</param>
        /// <returns></returns>
        public bool GetAccount(string DomainEmpNumber, ref CurrentUser User)
        {
            User = new CurrentUser();

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://qcicore01/SAM_API/api/Software/QueryUserInfo");
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"EmployeeId\":\""+ DomainEmpNumber + "\","
                                + "\"EMail\":\"\"}";

                    streamWriter.Write(json);
                }

                //API回傳的字串
                string responseStr = "";
                //發出Request
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseStr = streamReader.ReadToEnd();
                }

                Api_QueryUserInfo api_QueryUserInfo = new Api_QueryUserInfo();
                api_QueryUserInfo = JsonConvert.DeserializeObject<Api_QueryUserInfo>(responseStr.Replace("[", "").Replace("]", ""));

                if (_context.PDC_Member.Where(x => x.EmpNumber == api_QueryUserInfo.EMPLID.Trim() && x.BUCode == api_QueryUserInfo.DEPCOD.Trim() && x.CompCode == api_QueryUserInfo.COMCOD && x.IsEnabled == false).Any())
                {
                    User.User = _context.PDC_Member.Where(x => x.EmpNumber == api_QueryUserInfo.EMPLID.Trim() && x.BUCode == api_QueryUserInfo.DEPCOD.Trim() && x.CompCode == api_QueryUserInfo.COMCOD && x.IsEnabled == false).SingleOrDefault();
                }
                else
                {
                    PDC_Member pDC_Member = new PDC_Member();
                    pDC_Member.BUCode = api_QueryUserInfo.DEPCOD;
                    pDC_Member.BUName = api_QueryUserInfo.DEPNAM;
                    pDC_Member.CompCode = api_QueryUserInfo.COMCOD;
                    pDC_Member.CompName = api_QueryUserInfo.COMCOD;
                    pDC_Member.Email = api_QueryUserInfo.EMAILD;
                    pDC_Member.EmpNumber = api_QueryUserInfo.EMPLID;
                    pDC_Member.Extension = api_QueryUserInfo.MVPNUM;
                    pDC_Member.IsEnabled = false;
                    pDC_Member.MemberID = Guid.NewGuid();
                    pDC_Member.RoleID = MemberEnum.Role.EE_User;
                    pDC_Member.UserEngName = api_QueryUserInfo.ENGNAM;
                    pDC_Member.UserName = api_QueryUserInfo.ENGNAM;
                    pDC_Member.Creator = "super@admin.com";
                    pDC_Member.CreatorDate = DateTime.Now;
                    pDC_Member.CreatorName = "super@admin.com";
                    _context.PDC_Member.Add(pDC_Member);

                    //如果同工號但部門或公司代碼不同，把原本資料Enabled
                    List<PDC_Member> pDC_MemberList = _context.PDC_Member.Where(x => x.EmpNumber == api_QueryUserInfo.EMPLID.Trim() && x.IsEnabled == false).ToList();
                    foreach(var item in pDC_MemberList)
                    {
                        item.IsEnabled = true;
                    }

                    _context.SaveChanges();

                    User.User = pDC_Member;
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }
    }
}
